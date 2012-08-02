<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.ResetPasswordRequestModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ViewResources.AccountResources.ResetPasswordRequestTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LogOnContent" runat="server">

    <% Html.EnableClientValidation(); %>
    <h2><%=ViewResources.AccountResources.ResetPasswordRequestTitle %></h2>

    <%= Html.ValidationSummary(ViewResources.AccountResources.ResetPasswordRequestValidationSummary, new { @class = "alert alert-error" })%> 
    <%= Html.RequestSuccessInfo(new { @class = "alert alert-success" })%>   

    <% if (Model != null) { %>
        <% using(Html.BeginForm()) { %>
            <div class="well">
                <fieldset>
                    <legend><%=ViewResources.AccountResources.ResetPasswordRequestFormInfo%></legend>
                    <p>                        
                        <%= Html.LocalizedLabelFor(model => model.Email)%>                               
                        <%= Html.TextBoxFor(model => model.Email, new { @class = "fullwidth" })%>
                        <%= Html.ValidationMessageFor(model=>model.Email)%>                   
                    </p> 
                    <p>
                        <input type="submit" class="btn" value="<%=ViewResources.AccountResources.ResetPasswordRequestButtonText %>" />
                    </p>
                </fieldset>
            </div>
        <% } %>
    <%} %>

</asp:Content>
