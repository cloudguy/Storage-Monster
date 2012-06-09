<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.LogOnModel>" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services" %>
<%@ Import Namespace="System.Globalization" %>

<div id="LogOnControl">
    <% if (!HttpContext.Current.User.Identity.IsAuthenticated) {%>

        <h2><%=ViewResources.AccountResources.LogOnTitle %></h2>

        <% if (!MvcApplication.IsAjaxRequest(Request)) { %>
        <p>
            <%=ViewResources.AccountResources.LogOnText%> <%= Html.ActionLink(ViewResources.AccountResources.RegisterLinkContent, "Register", "Account")%> <%=ViewResources.AccountResources.RegisterText %>
        </p>
        <% } %>     

        <div id="LogonWait">
        </div>

        <%= Html.ValidationSummary(ViewResources.AccountResources.LogOnValidationSummary)%>        
        
        <div id="LogonError" class="validation-summary-errors" >
        </div>
        

        <% string formId = "LoginForm"; %>
        <% string ajaxHandler = string.Empty; %>
        <% if (MvcApplication.IsAjaxRequest(Request)) { %>
        <%      formId = formId+"Ajax"; %>         
        <%      ajaxHandler = string.Format(CultureInfo.InvariantCulture, "return LogOnSubmit($('#{0}'), '{1}', $('#AjaxLogOnContainer'));", formId, Url.Action("LogOn", "Account")); %>
        <% } else { %>  
        <%      Html.EnableClientValidation(); %>
        <% } %>     
       

        <% using (Html.BeginForm("LogOn", "Account", FormMethod.Post, new { id= formId, onsubmit=ajaxHandler } )) {%>
            <div>
                <fieldset>
                    <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.Email)%>:                              
                        <%= Html.TextBoxFor(model=>model.Email)%>
                        <%= Html.ValidationMessageFor(model=>model.Email)%> 
                    </p>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.Password)%>:                                
                        <%= Html.PasswordFor(model=>model.Password)%>
                        <%= Html.ValidationMessageFor(model=>Model.Password)%> 
                    </p>
                     <p>
                        <%= Html.CheckBoxFor(model=>model.RememberMe)%>
                        <%= Html.LocalizedLabelFor(model => model.RememberMe)%>                    
                        <%= Html.ValidationMessageFor(model=>Model.RememberMe)%> 
                    </p>
                    <p>
                        <input type="submit" value="<%=ViewResources.AccountResources.LogOnButtonText %>" />
                    </p>
                </fieldset>
            </div>
        <% } %>
    <%} %>
<//div>