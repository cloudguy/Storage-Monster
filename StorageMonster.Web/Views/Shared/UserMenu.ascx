<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.UserMenuModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Util" %>


Storage accounts
<br/>
<% foreach (var accountItem in Model.Accounts) { %>
    <div id='menu_account_<%=accountItem.AccountId %>'>
        <a href='<%=Url.Action("Content", "Storage", new {accountId = accountItem.AccountId}) %>'>
            <%= Html.Encode(accountItem.StorageName) %> <br/>
            <%=Html.Encode(accountItem.AccountLogin.ShortenString(10)) %> - <%=Html.Encode(accountItem.AccountServer.ShortenString(20)) %>
        </a>         
        <br/>
        ----------------------------------------
    </div>
    
<%} %>

<br/>

Profile
<br/>
<%=Html.ActionLink("Profile Manager", "Profile", "User", new {Id = Model.UserId}, new {@class = "link_class"} ) %>
<br/>
<%=Html.ActionLink("Account Manager", "Accounts", "User", new {Id = Model.UserId}, new {@class = "link_class"}) %>
