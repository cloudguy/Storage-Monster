<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="StorageMonster.Services" %>
<%@ Import Namespace="System.Globalization" %>

<script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.validate-1.9.0.min.js")%>" ></script>
<%
    string culture = StorageMonster.Util.RequestContext.GetValue<LocaleData>(StorageMonster.Util.RequestContext.LocaleKey).ShortName;
    String jsLocalizedMessagesUrl = null;
    if (culture != "en")
    {
        jsLocalizedMessagesUrl =  String.Format(CultureInfo.InvariantCulture, "~/Scripts/jquery.validate.messages_{0}.js", culture);
    }
%>  
<%if (!string.IsNullOrEmpty(jsLocalizedMessagesUrl)) {%>   
    <script type="text/javascript" src="<%= Url.Content(jsLocalizedMessagesUrl)%>" ></script>
<%} %>

<script type="text/javascript" src="<%= Url.Content("~/Scripts/MicrosoftMvcJQueryValidation.js")%>" ></script>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/MonsterValidation.js")%>" ></script>
<% Html.EnableClientValidation(); %>