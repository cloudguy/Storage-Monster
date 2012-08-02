<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<title>
<%=ViewResources.SharedResources.BadRequestTitle%>
</title>

<%=ViewResources.SharedResources.BadRequestContent%>
<%= Html.RouteLink(ViewResources.SharedResources.HomeLinkContent, new { Controller = "Home", Action = "Index" })%>
<% if (HttpContext.Current.Request.UrlReferrer != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Request.UrlReferrer.AbsoluteUri)) { %>
    <br/>   
    <a href="<%=HttpContext.Current.Request.UrlReferrer.AbsoluteUri %>"><%=ViewResources.SharedResources.BackLinkName %></a>
<% } %>
