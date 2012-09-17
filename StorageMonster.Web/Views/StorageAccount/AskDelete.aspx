<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.StorageAccount.AskDeleteModel>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ViewResources.StorageAccountResources.AskDeleteTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <% Html.RenderPartial("~/Views/StorageAccount/Controls/AskDeleteControl.ascx", Model); %>
</asp:Content>