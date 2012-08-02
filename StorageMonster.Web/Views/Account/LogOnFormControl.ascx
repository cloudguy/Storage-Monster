<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.LogOnModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<div id="LogOnControl" class="center-align">
    <% if (!HttpContext.Current.User.Identity.IsAuthenticated) {%>

        <h2><%=ViewResources.AccountResources.LogOnTitle %></h2>

        <% if (!Request.IsAjaxRequest()) { %>
        <p>
            <%=ViewResources.AccountResources.LogOnText%> <%= Html.ActionLink(ViewResources.AccountResources.RegisterLinkContent, "Register", "Account")%> <%=ViewResources.AccountResources.RegisterText %>
        </p>        
        <% } %>     

        <div id="LogonWait">
        </div>

        
        <%= Html.ValidationSummary(ViewResources.AccountResources.LogOnValidationSummary, new { @class = "alert alert-error" })%>        
        
        <div id="LogonError" class="validation-summary-errors" >
        </div>
        

        <% string formId = "LoginForm"; %>
        <% string ajaxHandler = string.Empty; %>
        <% if (Request.IsAjaxRequest())
           { %>          
        <%      formId = formId+"Ajax"; %>         
        <%      ajaxHandler = string.Format(CultureInfo.InvariantCulture, "return LogOnSubmit($('#{0}'), '{1}', $('#AjaxLogOnContainer'));", formId, Url.Action("LogOn", "Account")); %>
        <% } %>  
        
        <% Html.EnableClientValidation(); %>      


        <% using (Html.BeginForm("LogOn", "Account", FormMethod.Post, new { id= formId, onsubmit=ajaxHandler } )) {%>
            <div class="well">
                <fieldset>                    
                    <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
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
                        <input type="submit" class="btn" value="<%=ViewResources.AccountResources.LogOnButtonText %>" />
                        <%= Html.ActionLink(ViewResources.AccountResources.ResetPasswordLinkContent, "ResetPasswordRequest", "Account")%>
                    </p>
                </fieldset>
            </div>
        <% } %>
    <%} %>
</div>