using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;
using StorageMonster.Common.DataAnnotations;

namespace StorageMonster.Web.Services.Extensions
{
	public static class HtmlExtensions
	{        
        public static MvcHtmlString RequestSuccessInfo(this HtmlHelper htmlHelper, object htmlAttributes)
        {            
            IEnumerable<string> messages = htmlHelper.ViewData.GetRequestSuccessMessages();
            if (messages == null)
                return null;

            StringBuilder htmlBuilder = new StringBuilder();
            
            foreach (var message in messages)
            {
                if (string.IsNullOrEmpty(message))
                    continue;
                TagBuilder divBuilder = new TagBuilder("div");
                divBuilder.MergeAttributes<string, object>(new RouteValueDictionary(htmlAttributes));
                divBuilder.AddCssClass(Constants.RequestInfoHtmlClass);

                TagBuilder spanBuilder = new TagBuilder("span");                
                spanBuilder.SetInnerText(message);
                divBuilder.InnerHtml = spanBuilder.ToString(TagRenderMode.Normal);
                htmlBuilder.Append(divBuilder.ToString(TagRenderMode.Normal));                
            }
            
            return MvcHtmlString.Create(htmlBuilder.ToString());
        }

        public static MvcHtmlString RequestSuccessInfo(this HtmlHelper htmlHelper)
        {
            return RequestSuccessInfo(htmlHelper, null);
        }

		public static MvcHtmlString LocalizedLabelFor<TModel, TResult>(this HtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression)
		{
			return LocalizedLabelFor(html, expression, null);
		}

		public static MvcHtmlString LocalizedLabelFor<TModel, TResult>(this HtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression, object htmlAttributes)
		{
			string propName = ExpressionHelper.GetExpressionText(expression);
			ModelMetadata metadata = html.ViewData.ModelMetadata.Properties.First(p => p.PropertyName == propName);
		    return LocalizedLabel(html, metadata, propName, htmlAttributes);
		}

        private static MvcHtmlString LocalizedLabel(HtmlHelper html, ModelMetadata metadata, string propName, object htmlAttributes)
        {
            string unqualifiedPropName = propName.Split('.').Last(); // if there is a . in the name, take the rightmost part.
            string finalLabelText = metadata.DisplayName ?? metadata.PropertyName ?? unqualifiedPropName;
            if (String.IsNullOrEmpty(finalLabelText))
                return MvcHtmlString.Empty;

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(propName));


            StringBuilder htmlBuilder = new StringBuilder();
            var localizedAttr = (DisplayAttribute)metadata.ContainerType.GetProperty(unqualifiedPropName).GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
            htmlBuilder.Append(localizedAttr != null ? localizedAttr.GetName() : finalLabelText);


            tag.InnerHtml = htmlBuilder.ToString();

            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString LocalizedLabel(this HtmlHelper html, string modelName, object  htmlAttributes)
        {
            return LocalizedLabel(html, ModelMetadata.FromStringExpression(modelName, html.ViewContext.ViewData), modelName, htmlAttributes);
        }

        public static MvcHtmlString LocalizedLabel(this HtmlHelper html, string modelName)
        {
            return LocalizedLabel(html, ModelMetadata.FromStringExpression(modelName, html.ViewContext.ViewData), modelName, null);
        }

        public static MvcHtmlString RenderProperty(this HtmlHelper html, PropertyInfo propertyInfo, IDictionary<string,object > htmlAttributes)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof (MonsterDisplayAttribute), true);
            if (attributes.Length < 1)
                return MvcHtmlString.Empty;

            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            MonsterInputBoxAttribute inputBoxAttribute = attributes[0] as MonsterInputBoxAttribute;
            if (inputBoxAttribute != null)
            {
                if (inputBoxAttribute.Multiline)
                    htmlAttributes["multiline"] = true;
                return html.TextBox(propertyInfo.Name, null, htmlAttributes);
            }

            MonsterPasswordBoxAttribute passwordBoxAttribute = attributes[0] as MonsterPasswordBoxAttribute;
            if (passwordBoxAttribute != null)
            {
                return html.Password(propertyInfo.Name, null, htmlAttributes);
            }

            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString RenderProperty(this HtmlHelper html, PropertyInfo propertyInfo)
        {
            return RenderProperty(html, propertyInfo, null);
        }

	}
}
