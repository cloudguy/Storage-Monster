using CloudBin.Web.Core.Theming;
using System;
using System.Web.Mvc;

namespace CloudBin.Web.Core.Html
{
    public static class ThemingExtensions
    {
        private static readonly Lazy<IThemeProvider> ThemeProviderLazy = new Lazy<IThemeProvider>(() =>
        {
            return (IThemeProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IThemeProvider));
        });

        public static MvcHtmlString RenderDefaultThemeBundle(this CloudBinHtmlHelper helper)
        {
            return helper.RenderThemeBundle(ThemeProviderLazy.Value.DefaultTheme);
        }
        public static MvcHtmlString RenderThemeBundle(this CloudBinHtmlHelper helper, Theme theme)
        {
            return helper.RenderStyleBundle(theme.BundleName);
        }
    }
}
