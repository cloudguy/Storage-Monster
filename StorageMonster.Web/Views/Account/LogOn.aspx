<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.LogOnModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewResources.AccountResources.LogOnTitle %>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">   
    <% Html.RenderPartial("~/Views/Account/LogOnFormControl.ascx", Model); %>
</asp:Content>