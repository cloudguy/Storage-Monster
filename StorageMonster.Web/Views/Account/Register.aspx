<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewResources.AccountResources.RegisterTitle %>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="LogOnContent" runat="server">   
    <% Html.RenderPartial("~/Views/Account/Controls/RegisterControl.ascx", Model); %>    
</asp:Content>
