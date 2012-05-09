using System;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;
using System.Web.Routing;

namespace StorageMonster.Web.Services
{
	public static class HtmlExtensions
	{
		public static MvcHtmlString LocalizedLabelFor<TModel, TResult>(this HtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression)
		{
			return LocalizedLabelFor(html, expression, null);
		}

		public static MvcHtmlString LocalizedLabelFor<TModel, TResult>(this HtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression, object htmlAttributes)
		{
			string propName = ExpressionHelper.GetExpressionText(expression);
			string unqualifiedPropName = propName.Split('.').Last(); // if there is a . in the name, take the rightmost part.
			ModelMetadata metadata = html.ViewData.ModelMetadata.Properties.First(p => p.PropertyName == propName);

			string finalLabelText = metadata.DisplayName ?? metadata.PropertyName ?? unqualifiedPropName;
			if (String.IsNullOrEmpty(finalLabelText))
				return MvcHtmlString.Empty;
						
			TagBuilder tag = new TagBuilder("label");
			tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(propName));
			

			StringBuilder htmlBuilder = new StringBuilder();
			var localizedAttr = (LocalizedDisplayNameAttribute)metadata.ContainerType.GetProperty(unqualifiedPropName).GetCustomAttributes(typeof(LocalizedDisplayNameAttribute), true).FirstOrDefault();
			if (localizedAttr != null)			
				htmlBuilder.Append(localizedAttr.DisplayName);			
			else			
				htmlBuilder.Append(finalLabelText);
			

			tag.InnerHtml = htmlBuilder.ToString();

			if (htmlAttributes != null)
				tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
	}
}
