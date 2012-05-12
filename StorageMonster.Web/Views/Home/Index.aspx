<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.UserMenuModel>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server" >
</asp:Content>
 
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	StorageMonster - Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("UserMenu", Model); %>

</asp:Content>

