<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ProfileModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<div id="ProfileControl">  

        <h2><%=ViewResources.AccountResources.ProfileTitle %></h2>       

        <div id="ProfileWait">
        </div>

        <%= Html.ValidationSummary(ViewResources.AccountResources.LogOnValidationSummary)%>        
        
        <div id="ProfileError" class="validation-summary-errors" >
        </div>
        

        <% string formId = "ProfileForm"; %>
        <% string ajaxHandler = string.Empty; %>
        <% if (Request.IsAjaxRequest())
           { %>
        <%      formId = formId+"Ajax"; %>         
        <%      ajaxHandler = string.Format(System.Globalization.CultureInfo.InvariantCulture, "return LogOnSubmit($('#{0}'), '{1}', $('#AjaxLogOnContainer'));", formId, Url.Action("LogOn", "Account")); %>
        <% } %>    
       
        <% Html.EnableClientValidation(); %>  

        <% using (Html.BeginForm("Edit", "Account", FormMethod.Post, new { id= formId, onsubmit=ajaxHandler } )) {%>
            <div>
                <fieldset>
                    <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
                    <p>                  
                        <%= Html.LocalizedLabelFor(model => model.Email)%>:                              
                        <%= Html.Encode(Model.Email)%>                       
                    </p>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.UserName)%>:                                
                        <%= Html.PasswordFor(model => model.UserName)%>
                        <%= Html.ValidationMessageFor(model => Model.UserName)%> 
                    </p>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.Password)%>:                                
                        <%= Html.PasswordFor(model=>model.Password)%>
                        <%= Html.ValidationMessageFor(model=>Model.Password)%> 
                    </p>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.ConfirmPassword)%>:                                
                        <%= Html.PasswordFor(model => model.ConfirmPassword)%>
                        <%= Html.ValidationMessageFor(model => Model.ConfirmPassword)%> 
                    </p>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.Locale)%>:
                        <%= Html.DropDownListFor(model=>model.Locale, Model.SupportedLocales) %>                  
                        <%= Html.ValidationMessageFor(model=>model.Locale)%>
                    </p>
                    <p>
                        <input type="submit" value="<%=ViewResources.AccountResources.ProfileSubmitButtonText %>" />
                    </p>
                </fieldset>
            </div>
        <% } %>    
</div>