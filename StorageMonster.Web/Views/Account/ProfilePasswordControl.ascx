<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ProfilePasswordModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<% if (Model != null) { %>
    <div class="well">
        <% long stamp = 0;
           if (ViewData[Constants.StampFormKey] != null)
               stamp = (long)ViewData[Constants.StampFormKey];
        %>
        <% using (Html.BeginForm("ChangePassword", "Account", FormMethod.Post)) {%>            
            <fieldset>
                <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
                    <p>
                    <%= Html.LocalizedLabelFor(model => model.OldPassword)%>
                    <%= Html.PasswordFor(model => model.OldPassword)%>
                    <%= Html.ValidationMessageFor(model => Model.OldPassword)%> 
                </p>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.NewPassword)%>
                    <%= Html.PasswordFor(model => model.NewPassword)%>
                    <%= Html.ValidationMessageFor(model => Model.NewPassword)%> 
                </p>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.ConfirmNewPassword)%>
                    <%= Html.PasswordFor(model => model.ConfirmNewPassword)%>
                    <%= Html.ValidationMessageFor(model => Model.ConfirmNewPassword)%> 
                </p>
                <p>
                    <%=Html.AntiForgeryToken(Constants.Salt_Account_Edit) %>
                    <input type="hidden" name="<%=Constants.StampFormKey %>" value="<%=stamp.ToString(CultureInfo.InvariantCulture) %>" /> 
                    <input type="submit" class="btn" value="<%=ViewResources.AccountResources.ProfileSubmitButtonText %>" />
                </p>
            </fieldset>            
        <% } %>
    </div>
<% } %>