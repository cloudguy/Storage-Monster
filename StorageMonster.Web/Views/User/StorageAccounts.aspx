<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.User.UserAccountsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ViewResources.UserResources.StorageAccountsTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%  Html.ValidationSummary();                                                                           %>
<%  if (ViewContext.ViewData.ModelState.IsValid)                                                        %>
<%  {                                                                                                   %>
<%      Html.RenderPartial("ListUserStorageAccountsControl", Model);                                           %>
<%  }                                                                                                   %>
</asp:Content>


