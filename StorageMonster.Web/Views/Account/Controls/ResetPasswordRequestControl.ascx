<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ResetPasswordRequestModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<div id="resetPasswordRequestControl">
    <% Html.EnableClientValidation(); %>
    <h2><%=ViewResources.AccountResources.ResetPasswordRequestTitle %></h2>
    <% Html.RenderPartial("~/Views/Shared/Controls/MessagesControl.ascx", ViewResources.AccountResources.ResetPasswordRequestValidationSummary); %> 

    <% if (Model != null) { %>
        <div class="well">
            <% using(Html.BeginForm()) { %>
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
            <% } %>
        </div>
    <% } %>
</div>