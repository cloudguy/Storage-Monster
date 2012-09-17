<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.ProfileModel>" %>

<asp:Content ID="editTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewResources.AccountResources.ProfileTitle %>
</asp:Content>

<asp:Content ID="editContent" ContentPlaceHolderID="MainContent" runat="server">   
    <% Html.RenderPartial("~/Views/Account/Controls/ProfileControl.ascx", Model); %>
</asp:Content>