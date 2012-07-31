<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.UserMenuModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Utilities" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Models" %>

<%  MenuActivator.ActivationTypeEnum activationType = MenuActivator.ActivationTypeEnum.None;
    int storageAccountId = -1;    
    MenuActivator activator = ViewData[Constants.MenuActivatorViewDataKey] as MenuActivator;
    if (activator != null)
        activationType = activator.ActivationType;
    if (activationType == MenuActivator.ActivationTypeEnum.StorageAccount)
        storageAccountId = activator.StorageAccountId;
    
%>
<div id="menu_wraper" class="well sidebar-nav">
    <ul class="nav nav-list">
        <li class="nav-header">
            <%=ViewResources.SharedResources.MenuTitleStorageAccounts %>
        </li>
        <%  foreach (var accountItem in Model.StorageAccountsCollection.Accounts) %>
        <%  { %>
                <li <%=activationType == MenuActivator.ActivationTypeEnum.StorageAccount 
                        && accountItem.AccountId == storageAccountId? 
                            "class=\"active buttonn-container\"" 
                            : "class=\"buttonn-container\"" %>>
                    <% using(Html.BeginForm("GetFolder", "StorageAccount", FormMethod.Post)) {%>
                        <input type="hidden" name="id" value="<%=accountItem.AccountId.ToString(CultureInfo.InvariantCulture) %>" />
                        <%=Html.AntiForgeryToken(Constants.Salt_StorageAccount_GetFolder) %>
                        <button type="submit"><span><%= Html.Encode(accountItem.StoragePluginName) %>: <%=Html.Encode(accountItem.AccountName.Shorten(20)) %> </span></button>                    
                    <% } %>
                </li>
        <%  } %>
        <li class="divider"></li>
        <li class="nav-header">
            <%=ViewResources.SharedResources.MenuTitleSettings%>
        </li>        
        <li <%=activationType == MenuActivator.ActivationTypeEnum.EditProfile? "class=\"active\"" : string.Empty %>>
            <%=Html.ActionLink(ViewResources.SharedResources.MenuItemProfileMgr, "Profile", "User", new { Id = Model.StorageAccountsCollection.UserId }, null)%> 
        </li>
        <li <%=activationType == MenuActivator.ActivationTypeEnum.ListStorageAccounts? "class=\"active\"" : string.Empty %>>            
            <%=Html.ActionLink(ViewResources.SharedResources.MenuItemAccountsMgr, "StorageAccounts", "User", new { Id = Model.StorageAccountsCollection.UserId }, null)%>
        </li>
        <% if (Request.IsAuthenticated) {%> 
            <li class="divider"></li>
            <li>
                <%= Html.ActionLink(ViewResources.SharedResources.LogOffLinkContent, "LogOff", "Account")%>
            </li>
        <%} %>
    </ul>
</div>


 
