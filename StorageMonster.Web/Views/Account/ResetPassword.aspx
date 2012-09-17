<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.ResetPasswordModel>" %>

<asp:Content ID="resetPasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ViewResources.AccountResources.ResetPasswordTitle %>
</asp:Content>

<asp:Content ID="resetPasswordContent" ContentPlaceHolderID="LogOnContent" runat="server">
    <% Html.RenderPartial("~/Views/Account/Controls/ResetPasswordControl.ascx", Model); %>
</asp:Content>
