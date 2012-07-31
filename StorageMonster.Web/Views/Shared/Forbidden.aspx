<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
<%=ViewResources.SharedResources.ForbiddenTitle %>
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<%=ViewResources.SharedResources.ForbiddenContent %>
<% if (HttpContext.Current.Request.UrlReferrer != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Request.UrlReferrer.AbsoluteUri)) { %>
    <br/>   
    <a href="<%=HttpContext.Current.Request.UrlReferrer.AbsoluteUri %>"><%=ViewResources.SharedResources.BackLinkName %></a>
<% } %>
</asp:Content>
