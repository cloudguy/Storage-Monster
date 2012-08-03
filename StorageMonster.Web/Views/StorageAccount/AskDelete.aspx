<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.StorageAccount.AskDeleteModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AskDelete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    

    <%=Html.ValidationSummary("Error", new { @class = "alert alert-error" })%>

    <% if (Model != null) { %>
    Are you sure? <%=Html.Encode(Model.StorageAccountName) %>

        <% using(Html.BeginForm("Delete", "StorageAccount", FormMethod.Post)) {%>
            <input type="hidden" name="returnUrl" value="<%=Html.Encode(Model.ReturnUrl) %>" />
            <input type="hidden" name="Id" value="<%=Model.StorageAccountId.ToString(CultureInfo.InvariantCulture) %>" />
            <%=Html.AntiForgeryToken(Constants.Salt_StorageAccount_Delete) %>
            <button class="btn btn-danger"><span>Таки delete</span></button>
            <a href="<%=Url.Content(Model.ReturnUrl) %>" class="btn btn-action">No</a>
        <% } %>
    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LogOnContent" runat="server">
</asp:Content>
