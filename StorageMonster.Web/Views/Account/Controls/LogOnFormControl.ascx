<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.LogOnModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>
<%@ Import Namespace="StorageMonster.Services" %>
<%@ Import Namespace="StorageMonster.Web.Services" %>

<% 
    string generalClass;
    string headerClass;
    string bodyClass; 
    if (Request.IsAjaxRequest())
    {
        generalClass = "modal hide";
        headerClass = "modal-header";
        bodyClass = "modal-body disablable-enabled";
    }
    else
    {
        generalClass = string.Empty;
        headerClass = string.Empty;
        bodyClass = "well"; 
    }
%>


<div id="logOnControl" class="<%=@generalClass %>">

    <% Html.EnableClientValidation(); %>
    <div class="<%=headerClass %>">
        <h2><%=ViewResources.AccountResources.LogOnTitle %></h2>
    </div>
    <div class="<%=bodyClass %>"> 
        <div class="glasspanel"></div>
        <% Html.RenderPartial("~/Views/Shared/Controls/MessagesControl.ascx", ViewResources.AccountResources.LogOnValidationSummary); %>
        <div id="logonError" class="alert alert-error" style="display:none" ></div>

        <% if (!Request.IsAjaxRequest()) { %>
        <p>
            <%=ViewResources.AccountResources.LogOnText%> <%= Html.ActionLink(ViewResources.AccountResources.RegisterLinkContent, "Register", "Account")%> <%=ViewResources.AccountResources.RegisterText %>
        </p>        
        <% } %>    

        <% 
            string formId = "loginForm";
            string ajaxHandler = string.Empty; 
            if (Request.IsAjaxRequest())
            {           
                formId = formId+"Ajax";          
                ajaxHandler = string.Format(CultureInfo.InvariantCulture, "return LogOnSubmit($('#{0}'), '{1}', $('#ajaxLogOnContainer'));", formId, Url.Action("LogOn", "Account"));
            } 
        %>  

        <% using (Html.BeginForm("LogOn", "Account", FormMethod.Post, new { id= formId, onsubmit=ajaxHandler } )) {%>        
            <fieldset>    
                <% if (!Request.IsAjaxRequest()) { %>                
                <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
                <% } %>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.Email)%> 
                    <%= Html.TextBoxFor(model => model.Email, new { @class = "fullwidth" })%>
                    <%= Html.ValidationMessageFor(model=>model.Email)%>
                </p>
                <p>                        
                    <%= Html.LocalizedLabelFor(model => model.Password)%>                             
                    <%= Html.PasswordFor(model => model.Password, new { @class = "fullwidth" })%>
                    <%= Html.ValidationMessageFor(model=>Model.Password)%>
                </p>
                <p>
                    <label class = "checkbox">
                        <%= Html.CheckBoxFor(model => model.RememberMe)%>
                        <%= Html.LocalizedLabelFor(model => model.RememberMe)%>                    
                        <%= Html.ValidationMessageFor(model=>Model.RememberMe)%> 
                    </label>
                </p>
                <p>
                    <%=Html.HiddenFor(model=>model.ReturnUrl) %>
                    <input type="submit" class="btn" value="<%=ViewResources.AccountResources.LogOnButtonText %>" />
                    <%= Html.ActionLink(ViewResources.AccountResources.ResetPasswordLinkContent, "ResetPasswordRequest", "Account", null, new { target = "_blank" })%>
                </p>
            </fieldset>        
        <% } %> 
    </div>
</div>