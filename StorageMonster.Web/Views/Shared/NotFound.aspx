<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master"%>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
<%=ViewResources.SharedResources.NotFoundTitle %>
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<%=ViewResources.SharedResources.NotFoundContent %>
<br/>
<%= Html.RouteLink(ViewResources.SharedResources.HomeLinkContent, new { Controller = "Home", Action = "Index" })%>
<% if (HttpContext.Current.Request.UrlReferrer != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Request.UrlReferrer.AbsoluteUri)) { %>
    <br/>   
    <a href="<%=HttpContext.Current.Request.UrlReferrer.AbsoluteUri %>"><%=ViewResources.SharedResources.BackLinkName %></a>
<% } %>
</asp:Content>