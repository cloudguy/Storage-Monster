<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.ResetPasswordRequestModel>" %>

<asp:Content ID="resetPasswordRequestTitle" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ViewResources.AccountResources.ResetPasswordRequestTitle %>
</asp:Content>

<asp:Content ID="resetPasswordRequestContent" ContentPlaceHolderID="LogOnContent" runat="server">
    <% Html.RenderPartial("~/Views/Account/Controls/ResetPasswordRequestControl.ascx", Model); %>
</asp:Content>
