<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.User.UserAccountsModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Security" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<div id="accountsList">
    <%= Html.ValidationSummary(string.Empty, new { @class = "alert alert-error" }) %>
    <%= Html.RequestSuccessInfo(new { @class = "alert alert-success" })%>

    <% Identity identity = (Identity)HttpContext.Current.User.Identity; %>
    <% if (Model.AccountsCollection.Accounts.Count() > 0) { %>
        <table class="table table-condensed">
            <thead>
                <tr>
                    <th><%=ViewResources.UserResources.StoragePluginTypeHeader %></th>
                    <th><%=ViewResources.UserResources.StorageAccountNameHeader %></th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>   
                <% foreach (var account in Model.AccountsCollection.Accounts) { %>    
                    <tr>
                        <td><%=Html.Encode(account.StoragePluginName) %> </td>
                        <td><%=Html.Encode(account.AccountName) %></td>
                        <% if (identity.UserId == Model.AccountsCollection.UserId) { %>
                            <td>
                                <%= Html.ActionLink(ViewResources.UserResources.EditButtonText, "Edit", "StorageAccount", 
                                    new { Id = account.AccountId }, 
                                    new { @class = "btn btn-primary" })%>
                            </td>
                            <td>
                                <%= Html.ActionLink(ViewResources.UserResources.DeleteButtonText, "AskDelete", "StorageAccount", 
                                    new { Id = account.AccountId, returnUrl = ViewContext.HttpContext.Request.Url.PathAndQuery }, 
                                    new { @class = "btn btn-warning" })%>
                            </td>
                        <% } else { %>
                            <td></td><td></td>
                        <% } %>
                    </tr>
                <% } %>
            </tbody>
        </table>
    <% } %>
    <% if (identity.UserId == Model.AccountsCollection.UserId) {%>    
        <%=Html.ActionLink(ViewResources.UserResources.AddAccountLinkText, "Add", "StorageAccount")%>                
    <% } %>
        
</div>