<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
 <%=ViewResources.SharedResources.ForbiddenTitle %>
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<%=ViewResources.SharedResources.ForbiddenContent %>
</asp:Content>
