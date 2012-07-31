<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.StorageAccount.StorageAccountModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<%@ Import Namespace="StorageMonster.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ViewResources.StorageAccountResources.AddTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   

    <% Html.EnableClientValidation(); %>
    <%=Html.ValidationSummary() %>
    <% using (Html.BeginForm()) { %>
        <div class="well">
            <fieldset>
                <legend><%=ViewResources.StorageAccountResources.AddTitle%></legend>
                <p>             
                    <%= Html.LocalizedLabelFor(model => model.PluginId)%>
                    <%= Html.DropDownListFor(model => model.PluginId, Model.StoragePlugins, new { @class = "input-large" })%>                  
                    <%= Html.ValidationMessageFor(model => model.PluginId)%>
                </p>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.AccountName)%>
                    <%= Html.TextBoxFor(model => model.AccountName, new { @class = "input-large" })%>                            
                    <%= Html.ValidationMessageFor(model => model.AccountName)%>                   
                </p>
                <p>
                    <%=Html.AntiForgeryToken(Constants.Salt_StorageAccount_Add)%>
                    <input type="submit" class="btn" value="<%=ViewResources.StorageAccountResources.AddButtonText %>" />
                </p>
            </fieldset>
        </div>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
