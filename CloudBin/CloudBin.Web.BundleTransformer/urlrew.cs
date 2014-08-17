using System;
using System.Text;
using BundleTransformer.Core.Helpers;
using BundleTransformer.Core.PostProcessors;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core;
using System.Text.RegularExpressions;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.Resources;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web.Optimization;
using System.Web.Hosting;
using BundleTransformer.Core.Utilities;
using System.Web.Caching;

namespace CloudBin.Web.BundleTransformer
{
	/*
	public sealed class UrlRewritingCssPostProcessorCustom : PostProcessorBase
	{
		/// <summary>
		/// Relative path resolver
		/// </summary>
		private readonly IRelativePathResolver _relativePathResolver;

		/// <summary>
		/// Regular expression for working with CSS <code>@import</code> rules
		/// </summary>
		private static readonly Regex _cssImportRuleRegex =
			new Regex(@"@import\s*(?<quote>'|"")(?<url>[\w \-+.:,;/?&=%~#$@()\[\]{}]+)(\k<quote>)",
			          RegexOptions.IgnoreCase);


		/// <summary>
		/// Constructs instance of URL rewriting CSS-postprocessor
		/// </summary>
		public UrlRewritingCssPostProcessorCustom()
			: this(new CommonRelativePathResolver())
		{ }

		/// <summary>
		/// Constructs instance of URL rewriting CSS-postprocessor
		/// </summary>
		/// <param name="relativePathResolver">Relative path resolver</param>
		public UrlRewritingCssPostProcessorCustom(IRelativePathResolver relativePathResolver)
		{
			_relativePathResolver = relativePathResolver;
		}


		/// <summary>
		/// Transforms relative paths to absolute in CSS-file
		/// </summary>
		/// <param name="asset">CSS-asset</param>
		/// <returns>Processed CSS-asset</returns>
		public override IAsset PostProcess(IAsset asset)
		{
			if (asset == null)
			{
				throw new ArgumentException(Strings.Common_ValueIsEmpty, "asset");
			}

			InnerPostProcess(asset);

			return asset;
		}

		/// <summary>
		/// Transforms relative paths to absolute in CSS-files
		/// </summary>
		/// <param name="assets">Set of CSS-assets</param>
		/// <returns>Set of processed CSS-assets</returns>
		public override IList<IAsset> PostProcess(IList<IAsset> assets)
		{
			if (assets == null)
			{
				throw new ArgumentException(Strings.Common_ValueIsEmpty, "assets");
			}

			if (assets.Count == 0)
			{
				return assets;
			}

			var assetsToProcessing = assets.Where(a => a.IsStylesheet && !a.RelativePathsResolved).ToList();
			if (assetsToProcessing.Count == 0)
			{
				return assets;
			}

			foreach (var asset in assetsToProcessing)
			{
				InnerPostProcess(asset);
			}

			return assets;
		}

		private void InnerPostProcess(IAsset asset)
		{
			string url = asset.Url;
			string content = ResolveAllRelativePaths(asset.Content, url);

			asset.Content = content;
			asset.RelativePathsResolved = true;
		}

		/// <summary>
		/// Transforms all relative paths to absolute in CSS-code
		/// </summary>
		/// <param name="content">Text content of CSS-asset</param>
		/// <param name="path">CSS-file path</param>
		/// <returns>Processed text content of CSS-asset</returns>
		public string ResolveAllRelativePaths(string content, string path)
		{
			int contentLength = content.Length;
			if (contentLength == 0)
			{
				return content;
			}

			MatchCollection urlRuleMatches = CommonRegExps.CssUrlRuleRegex.Matches(content);
			MatchCollection importRuleMatches = _cssImportRuleRegex.Matches(content);

			if (urlRuleMatches.Count == 0 && importRuleMatches.Count == 0)
			{
				return content;
			}

			var nodeMatches = new List<CssNodeMatch>();

			foreach (Match urlRuleMatch in urlRuleMatches)
			{
				var nodeMatch = new CssNodeMatch(urlRuleMatch.Index,
				                                 urlRuleMatch.Length,
				                                 CssNodeType.UrlRule,
				                                 urlRuleMatch);
				nodeMatches.Add(nodeMatch);
			}

			foreach (Match importRuleMatch in importRuleMatches)
			{
				var nodeMatch = new CssNodeMatch(importRuleMatch.Index,
				                                 importRuleMatch.Length,
				                                 CssNodeType.ImportRule,
				                                 importRuleMatch);
				nodeMatches.Add(nodeMatch);
			}

			MatchCollection multilineCommentMatches = CommonRegExps.CssMultilineCommentRegex.Matches(content);

			foreach (Match multilineCommentMatch in multilineCommentMatches)
			{
				var nodeMatch = new CssNodeMatch(multilineCommentMatch.Index,
				                                 multilineCommentMatch.Length,
				                                 CssNodeType.MultilineComment,
				                                 multilineCommentMatch);
				nodeMatches.Add(nodeMatch);
			}

			nodeMatches = nodeMatches
				.OrderBy(n => n.Position)
					.ThenByDescending(n => n.Length)
					.ToList()
					;

			var contentBuilder = new StringBuilder();
			int endPosition = contentLength - 1;
			int currentPosition = 0;

			foreach (CssNodeMatch nodeMatch in nodeMatches)
			{
				CssNodeType nodeType = nodeMatch.NodeType;
				int nodePosition = nodeMatch.Position;
				Match match = nodeMatch.Match;

				if (nodePosition < currentPosition)
				{
					continue;
				}

				if (nodeType == CssNodeType.UrlRule || nodeType == CssNodeType.ImportRule)
				{
					ProcessOtherContent(contentBuilder, content,
					                    ref currentPosition, nodePosition);

					if (nodeType == CssNodeType.UrlRule)
					{
						GroupCollection urlRuleGroups = match.Groups;

						string url = urlRuleGroups["url"].Value.Trim();
						string quote = urlRuleGroups["quote"].Success ?
							urlRuleGroups["quote"].Value : string.Empty;

						string urlRule = match.Value;
						string processedUrlRule = ProcessUrlRule(path, url, quote);

						contentBuilder.Append(processedUrlRule);
						currentPosition += urlRule.Length;
					}
					else if (nodeType == CssNodeType.ImportRule)
					{
						GroupCollection importRuleGroups = match.Groups;

						string url = importRuleGroups["url"].Value.Trim();

						string importRule = match.Value;
						string processedImportRule = ProcessImportRule(path, url);

						contentBuilder.Append(processedImportRule);
						currentPosition += importRule.Length;
					}
				}
				else if (nodeType == CssNodeType.MultilineComment)
				{
					int nextPosition = nodePosition + match.Length;

					ProcessOtherContent(contentBuilder, content,
					                    ref currentPosition, nextPosition);
				}
			}

			if (currentPosition > 0 && currentPosition <= endPosition)
			{
				ProcessOtherContent(contentBuilder, content,
				                    ref currentPosition, endPosition + 1);
			}

			return contentBuilder.ToString();
		}

		/// <summary>
		/// Process a CSS <code>@import</code> rule
		/// </summary>
		/// <param name="parentAssetUrl">URL of parent CSS-asset file</param>
		/// <param name="assetUrl">URL of CSS-asset file</param>
		/// <returns>Processed CSS <code>@import</code> rule</returns>
		private string ProcessImportRule(string parentAssetUrl, string assetUrl)
		{
			string processedAssetUrl = assetUrl;
			if (!UrlHelpers.StartsWithProtocol(assetUrl) && !UrlHelpers.StartsWithDataUriScheme(assetUrl))
			{
				processedAssetUrl = _relativePathResolver.ResolveRelativePath(parentAssetUrl, assetUrl);
			}

			string result = string.Format(@"@import ""{0}""", processedAssetUrl);

			return result;
		}

		/// <summary>
		/// Process a CSS <code>url</code> rule
		/// </summary>
		/// <param name="parentAssetUrl">URL of parent CSS-asset file</param>
		/// <param name="assetUrl">URL of CSS-asset file</param>
		/// <param name="quote">Quote</param>
		/// <returns>Processed CSS <code>url</code> rule</returns>
		private string ProcessUrlRule(string parentAssetUrl, string assetUrl, string quote)
		{
			string processedAssetUrl = assetUrl;
			if (!UrlHelpers.StartsWithProtocol(assetUrl) && !UrlHelpers.StartsWithDataUriScheme(assetUrl))
			{
				processedAssetUrl = _relativePathResolver.ResolveRelativePath(parentAssetUrl, assetUrl);
			}
			if (processedAssetUrl.StartsWith ("~/", StringComparison.OrdinalIgnoreCase))
			{
				processedAssetUrl = processedAssetUrl.TrimStart ('~');
			}

			string result = string.Format("url({0}{1}{0})", quote, processedAssetUrl);

			return result;
		}

		/// <summary>
		/// Process a other stylesheet content
		/// </summary>
		/// <param name="contentBuilder">Content builder</param>
		/// <param name="assetContent">Text content of CSS-asset</param>
		/// <param name="currentPosition">Current position</param>
		/// <param name="nextPosition">Next position</param>
		private static void ProcessOtherContent(StringBuilder contentBuilder, string assetContent,
		                                        ref int currentPosition, int nextPosition)
		{
			if (nextPosition > currentPosition)
			{
				string otherContent = assetContent.Substring(currentPosition,
				                                             nextPosition - currentPosition);

				contentBuilder.Append(otherContent);
				currentPosition = nextPosition;
			}
		}
	}


	public class CommonRelativePathResolver : IRelativePathResolver
	{
		/// <summary>
		/// Virtual file system wrapper
		/// </summary>
		private readonly IVirtualFileSystemWrapper _virtualFileSystemWrapper;


		/// <summary>
		/// Constructs instance of common relative path resolver
		/// </summary>
		public CommonRelativePathResolver()
			: this(new VirtualFileSystemWrapperq())
		{ }

		/// <summary>
		/// Constructs instance of common relative path resolver
		/// </summary>
		/// <param name="virtualFileSystemWrapper">Virtual file system wrapper</param>
		public CommonRelativePathResolver(IVirtualFileSystemWrapper virtualFileSystemWrapper)
		{
			_virtualFileSystemWrapper = virtualFileSystemWrapper;
		}


		/// <summary>
		/// Transforms relative path to absolute
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <param name="relativePath">The relative path</param>
		public string ResolveRelativePath(string basePath, string relativePath)
		{
			if (string.IsNullOrWhiteSpace(basePath))
			{
				throw new ArgumentException(string.Format(Strings.Common_ArgumentIsEmpty, "basePath"), "basePath");
			}

			if (string.IsNullOrWhiteSpace(relativePath))
			{
				throw new ArgumentException(string.Format(Strings.Common_ArgumentIsEmpty, "relativePath"), "relativePath");
			}

			string processedRelativePath = UrlHelpers.ProcessBackSlashes(relativePath);

			if (processedRelativePath.StartsWith("/") || UrlHelpers.StartsWithProtocol(processedRelativePath))
			{
				return processedRelativePath;
			}

			if (processedRelativePath.StartsWith("~/"))
			{
				return _virtualFileSystemWrapper.ToAbsolutePath(processedRelativePath);
			}

			string processedBasePath = UrlHelpers.ProcessBackSlashes(Path.GetDirectoryName(basePath));

			string absolutePath = UrlHelpers.Combine(processedBasePath, processedRelativePath);
			if (absolutePath.IndexOf("./", StringComparison.Ordinal) != -1)
			{
				absolutePath = UrlHelpers.Normalize(absolutePath);
			}
			absolutePath = _virtualFileSystemWrapper.ToAbsolutePath(absolutePath);

			return absolutePath;
		}
	}


*/














	//mono fix for IVirtualFileSystemWrapper.ToAbsolutePath (string virtualPath)
	internal sealed class VirtualFileSystemWrapper : IVirtualFileSystemWrapper
	{
		private readonly IVirtualFileSystemWrapper _innerWrapper;

		internal VirtualFileSystemWrapper(IVirtualFileSystemWrapper innerWrapper)
		{
			_innerWrapper = innerWrapper;
		}

		bool IVirtualFileSystemWrapper.FileExists (string virtualPath)
		{
			return _innerWrapper.FileExists (virtualPath);
		}

		string IVirtualFileSystemWrapper.GetFileTextContent (string virtualPath)
		{
			return _innerWrapper.GetFileTextContent (virtualPath);
		}

		byte[] IVirtualFileSystemWrapper.GetFileBinaryContent (string virtualPath)
		{
			return _innerWrapper.GetFileBinaryContent (virtualPath);
		}

		Stream IVirtualFileSystemWrapper.GetFileStream (string virtualPath)
		{
			return _innerWrapper.GetFileStream (virtualPath);
		}

		string IVirtualFileSystemWrapper.ToAbsolutePath (string virtualPath)
		{
			return System.Web.VirtualPathUtility.ToAbsolute (virtualPath);
		}

		string IVirtualFileSystemWrapper.GetCacheKey (string virtualPath)
		{
			return _innerWrapper.GetCacheKey (virtualPath);
		}

		CacheDependency IVirtualFileSystemWrapper.GetCacheDependency (string virtualPath, string[] virtualPathDependencies, DateTime utcStart)
		{
			return _innerWrapper.GetCacheDependency (virtualPath, virtualPathDependencies, utcStart);
		}

		bool IVirtualFileSystemWrapper.IsTextFile (string virtualPath, int sampleSize, out Encoding encoding)
		{
			return _innerWrapper.IsTextFile (virtualPath, sampleSize, out encoding);
		}

	}


	internal sealed class FileSystemContext : IFileSystemContext
	{
		private readonly IVirtualFileSystemWrapper _virtualFilesSystemWrapper;
		private readonly IRelativePathResolver _relativePathResolver;
		internal FileSystemContext(IVirtualFileSystemWrapper virtualFilesSystemWrapper, IRelativePathResolver relativePathResolver)
		{
			_virtualFilesSystemWrapper = virtualFilesSystemWrapper;
			_relativePathResolver = relativePathResolver;
		}


		IVirtualFileSystemWrapper IFileSystemContext.GetVirtualFileSystemWrapper ()
		{
			return _virtualFilesSystemWrapper;
		}
		IRelativePathResolver IFileSystemContext.GetCommonRelativePathResolver ()
		{
			return _relativePathResolver;
		}

	}

	internal sealed class BundleTransformerContext : IBundleTransformerContext
	{
		private readonly IBundleTransformerContext _innerContext;
		private readonly IFileSystemContext _fileSystemContext;
		internal BundleTransformerContext(IBundleTransformerContext innerContext){
			_innerContext = innerContext;
			_fileSystemContext = new FileSystemContext(new VirtualFileSystemWrapper(_innerContext.FileSystem.GetVirtualFileSystemWrapper()), _innerContext.FileSystem.GetCommonRelativePathResolver());
		}

		#region IBundleTransformerContext implementation
		global::BundleTransformer.Core.Configuration.IConfigurationContext IBundleTransformerContext.Configuration {
			get {
				return _innerContext.Configuration;
			}
		}
		IFileSystemContext IBundleTransformerContext.FileSystem {
			get {
				return _fileSystemContext;
			}
		}
		IAssetContext IBundleTransformerContext.Styles {
			get {
				return _innerContext.Styles;
			}
		}
		IAssetContext IBundleTransformerContext.Scripts {
			get {
				return _innerContext.Scripts;
			}
		}
		bool IBundleTransformerContext.IsDebugMode {
			get {
				return _innerContext.IsDebugMode;
			}
		}
		#endregion
	}

	/*
	public sealed class VirtualFileSystemWrapperq : IVirtualFileSystemWrapper
	{
		/// <summary>
		/// Gets a value that indicates whether a file exists in the virtual file system
		/// </summary>
		/// <param name="virtualPath">The path to the virtual file</param>
		/// <returns>Result of checking (true – exist; false – not exist)</returns>
		public bool FileExists(string virtualPath)
		{
			return BundleTable.VirtualPathProvider.FileExists(virtualPath);
		}

		/// <summary>
		/// Gets a text content of the specified file
		/// </summary>
		/// <param name="virtualPath">The path to the virtual file</param>
		/// <returns>Text content</returns>
		public string GetFileTextContent(string virtualPath)
		{
			string content;

			try
			{
				VirtualFile virtualFile = BundleTable.VirtualPathProvider.GetFile(virtualPath);
				var stringBuilder = new StringBuilder();

				using (var streamReader = new StreamReader(virtualFile.Open()))
				{
					// Fixes a single CR/LF
					while (streamReader.Peek() >= 0)
					{
						stringBuilder.AppendLine(streamReader.ReadLine());
					}
				}

				content = stringBuilder.ToString();
			}
			catch (FileNotFoundException e)
			{
				throw new FileNotFoundException(
					string.Format(Strings.Common_FileNotExist, virtualPath), virtualPath, e);
			}

			return content;
		}

		/// <summary>
		/// Gets a binary content of the specified file
		/// </summary>
		/// <param name="virtualPath">The path to the virtual file</param>
		/// <returns>Binary content</returns>
		public byte[] GetFileBinaryContent(string virtualPath)
		{
			byte[] bytes;

			try
			{
				VirtualFile virtualFile = BundleTable.VirtualPathProvider.GetFile(virtualPath);

				using (var stream = virtualFile.Open())
				{
					bytes = new byte[stream.Length];
					stream.Read(bytes, 0, (int) stream.Length);
				}
			}
			catch (FileNotFoundException e)
			{
				throw new FileNotFoundException(
					string.Format(Strings.Common_FileNotExist, virtualPath), virtualPath, e);	
			}

			return bytes;
		}

		/// <summary>
		/// Gets a file stream
		/// </summary>
		/// <param name="virtualPath">The path to the virtual file</param>
		/// <returns>File stream</returns>
		public Stream GetFileStream(string virtualPath)
		{
			Stream stream;

			try
			{
				VirtualFile virtualFile = BundleTable.VirtualPathProvider.GetFile(virtualPath);
				stream = virtualFile.Open();
			}
			catch (FileNotFoundException e)
			{
				throw new FileNotFoundException(
					string.Format(Strings.Common_FileNotExist, virtualPath), virtualPath, e);	
			}

			return stream;
		}

		/// <summary>
		/// Converts a virtual path to an application absolute path
		/// </summary>
		/// <param name="virtualPath">The virtual path to convert to an application-relative path</param>
		/// <returns>The absolute path representation of the specified virtual path</returns>
		public string ToAbsolutePath(string virtualPath)
		{


		//	return System.Web.VirtualPathUtility.ToAbsolute (virtualPath);

			VirtualFile virtualFile = BundleTable.VirtualPathProvider.GetFile(virtualPath);
			return virtualFile.VirtualPath;
		}

		/// <summary>
		/// Returns a cache key to use for the specified virtual path
		/// </summary>
		/// <param name="virtualPath">The path to the virtual resource</param>
		/// <returns>A cache key for the specified virtual resource</returns>
		public string GetCacheKey(string virtualPath)
		{
			return BundleTable.VirtualPathProvider.GetCacheKey(virtualPath);
		}

		/// <summary>
		/// Creates a cache dependency based on the specified virtual paths
		/// </summary>
		/// <param name="virtualPath">The path to the primary virtual resource</param>
		/// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource</param>
		/// <param name="utcStart">The UTC time at which the virtual resources were read</param>
		/// <returns>A System.Web.Caching.CacheDependency object for the specified virtual resources</returns>
		public CacheDependency GetCacheDependency(string virtualPath, string[] virtualPathDependencies, 
		                                          DateTime utcStart)
		{
			return BundleTable.VirtualPathProvider.GetCacheDependency(virtualPath, virtualPathDependencies, 
			                                                          utcStart);
		}

		/// <summary>
		/// Detect if a file is text and detect the encoding
		/// </summary>
		/// <param name="virtualPath">The path to the virtual file</param>
		/// <param name="sampleSize">Number of characters to use for testing</param>
		/// <param name="encoding">Detected encoding</param>
		/// <returns>Result of check (true - is text; false - is binary)</returns>
		public bool IsTextFile(string virtualPath, int sampleSize, out Encoding encoding)
		{
			bool isTextContent;

			using (Stream fileStream = GetFileStream(virtualPath))
			{
				isTextContent = Utils.IsTextStream(fileStream, sampleSize, out encoding);
			}

			return isTextContent;
		}
	}*/



}

