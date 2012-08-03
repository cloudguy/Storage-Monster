<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ProfileBaseModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<% if (Model != null) { %>
   <div class="well">
        <h2><%=ViewResources.AccountResources.ProfileTitle %></h2>            
        <% long stamp = 0;
           if (ViewData[Constants.StampFormKey] != null)
               stamp = (long)ViewData[Constants.StampFormKey];
        %>
        <% using (Html.BeginForm("Edit", "Account", FormMethod.Post )) {%>
            <div>
                <fieldset>
                    <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
                    <p>                                                     
                        <%= Html.LocalizedLabelFor(model => model.Email, new { @class = "display_inline" })%>:
                        <b><%= Html.Encode(Model.Email)%></b>
                        <%= Html.HiddenFor(model=>model.Email) %>
                    </p>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.UserName)%>
                        <%= Html.TextBoxFor(model => model.UserName)%>
                        <%= Html.ValidationMessageFor(model => Model.UserName)%> 
                    </p>                      
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.Locale)%>
                        <%= Html.DropDownListFor(model=>model.Locale, Model.SupportedLocales) %>                  
                        <%= Html.ValidationMessageFor(model=>model.Locale)%>
                    </p>
                    <p>
                        <%=Html.AntiForgeryToken(Constants.Salt_Account_Edit) %>
                        <input type="hidden" name="<%=Constants.StampFormKey %>" value="<%=stamp.ToString(CultureInfo.InvariantCulture) %>" />                        
                        <input type="submit" class="btn" value="<%=ViewResources.AccountResources.ProfileSubmitButtonText %>" />
                    </p>
                </fieldset>
            </div>
        <% } %>    
    </div>
<% } %>