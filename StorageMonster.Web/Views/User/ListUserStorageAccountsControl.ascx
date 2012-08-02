<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.User.UserAccountsModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Security" %>
<%@ Import Namespace="System.Globalization" %>

<div id="accountsList">
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
                                <% using(Html.BeginForm("Edit", "StorageAccount", FormMethod.Get)) {%>  
                                    <input type="hidden" name="Id" value="<%=account.AccountId.ToString(CultureInfo.InvariantCulture) %>" /> 
                                    <input type="submit" class="btn btn-primary" value="<%=ViewResources.UserResources.EditButtonText %>"/>
                                <% } %>                         
                            </td>
                            <td>
                                 <% using(Html.BeginForm("Delete", "StorageAccount", FormMethod.Post)) {%>                               
                                    <input type="hidden" name="Id" value="<%=account.AccountId.ToString(CultureInfo.InvariantCulture) %>" />                          
                                    <input type="submit" class="btn btn-danger" value="<%=ViewResources.UserResources.DeleteButtonText %>"/>
                                <% } %>  
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