<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ProfileBaseModel>" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<% if (Model != null) { %>
   <div class="well">
        <% using (Html.BeginForm("Edit", "Account", FormMethod.Post )) {%>
                <legend><%=ViewResources.AccountResources.ProfileTitle%></legend>
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
                    <%= Html.LocalizedLabelFor(model => model.TimeZone)%>
                    <%= Html.DropDownListFor(model => model.TimeZone, Model.SupportedTimeZones)%>                  
                    <%= Html.ValidationMessageFor(model=>model.TimeZone)%>
                </p>    
                <p>
                    <%= Html.AntiForgeryToken(Constants.Salt_Account_Edit) %>
                    <%= Html.HiddenFor(model=>model.Stamp) %> 
                    <input type="submit" class="btn" value="<%=ViewResources.AccountResources.ProfileSubmitButtonText %>" />
                </p>       
        <% } %>
    </div>
<% } %>