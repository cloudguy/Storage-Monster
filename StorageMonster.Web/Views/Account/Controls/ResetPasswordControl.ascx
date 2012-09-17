<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ResetPasswordModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>
<%@ Import Namespace="System.Globalization" %>

<div id="resetPasswordControl">
    <% Html.EnableClientValidation(); %>    
    <% Html.RenderPartial("~/Views/Shared/Controls/MessagesControl.ascx", ViewResources.AccountResources.ResetPasswordValidationSummary); %>

    <% if (Model != null) { %>
        <div class="well">
            <% using(Html.BeginForm()) { %>        
                <fieldset>    
                    <legend><%=ViewResources.AccountResources.ResetPasswordTitle %></legend> 
                    <p>
                        <%=String.Format(CultureInfo.CurrentCulture, ViewResources.AccountResources.PasswordRequirementFormat, Model.MinPasswordLength)%>
                    </p>             
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
                        <%= Html.HiddenFor(model=>model.Token) %>
                        <input type="submit" class="btn" value="<%=ViewResources.AccountResources.ResetPasswordButtonText %>" />
                    </p>
                </fieldset>
            <% } %>
        </div>
    <%} %>
</div>