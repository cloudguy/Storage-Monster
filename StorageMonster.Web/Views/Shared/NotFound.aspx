<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master"%>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
 <%=ViewResources.SharedResources.NotFoundTitle %>
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
 <%=ViewResources.SharedResources.NotFoundContent %>
 <br/>
<%= Html.RouteLink(ViewResources.SharedResources.HomeLinkContent, new { Controller = "Home", Action = "Index" })%>
</asp:Content>