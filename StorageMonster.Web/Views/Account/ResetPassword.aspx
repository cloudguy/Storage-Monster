<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.ResetPasswordModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ViewResources.AccountResources.ResetPasswordTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LogOnContent" runat="server">
    <% Html.EnableClientValidation(); %>    

    <%= Html.ValidationSummary(ViewResources.AccountResources.ResetPasswordValidationSummary, new { @class = "alert alert-error" })%> 
    <%= Html.RequestSuccessInfo(new { @class = "alert alert-success" })%>   

    <% if (Model != null) { %>
        <% using(Html.BeginForm()) { %>
            <div class="well">
                <fieldset>    
                    <legend><%=ViewResources.AccountResources.ResetPasswordTitle %></legend>                
                    <p>
                        <%=String.Format(System.Globalization.CultureInfo.CurrentCulture, ViewResources.AccountResources.PasswordRequirementFormat, Model.MinPasswordLength)%>
                    </p>
                    <p>                        
                        <%= Html.LocalizedLabelFor(model => model.NewPassword)%>                               
                        <%= Html.PasswordFor(model => model.NewPassword, new { @class = "fullwidth" })%>
                        <%= Html.ValidationMessageFor(model => model.NewPassword)%>                   
                    </p> 
                     <p>                        
                        <%= Html.LocalizedLabelFor(model => model.ConfirmNewPassword)%>                               
                        <%= Html.PasswordFor(model => model.ConfirmNewPassword, new { @class = "fullwidth" })%>
                        <%= Html.ValidationMessageFor(model => model.ConfirmNewPassword)%>                   
                    </p> 
                    <p>
                        <input type="hidden" name="Token" value="<%=Html.Encode(Model.Token) %>" />
                        <input type="submit" class="btn" value="<%=ViewResources.AccountResources.ResetPasswordButtonText %>" />
                    </p>
                </fieldset>
            </div>
        <% } %>
    <%} %>
</asp:Content>
