<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ProfilePasswordModel>" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<% if (Model != null) { %>
    <div class="well">
        <% using (Html.BeginForm("ChangePassword", "Account", FormMethod.Post)) {%>
            <div>
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
                    <input type="submit" class="btn" value="<%=ViewResources.AccountResources.ProfileSubmitButtonText %>" />
                </p>
            </fieldset>
            </div>
        <% } %>
    </div>
<% } %>