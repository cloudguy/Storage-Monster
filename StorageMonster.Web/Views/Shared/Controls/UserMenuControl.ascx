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
<div class="well sidebar-nav" data-widget="menu">
    <ul class="nav nav-list">
        <li class="nav-header">
            <%=ViewResources.SharedResources.MenuTitleStorageAccounts %>
        </li>
        <%  foreach (var accountItem in Model.StorageAccountsCollection.Accounts) %>
        <%  { %>
                <% 
                    string liClass;
                    if (activationType == MenuActivator.ActivationTypeEnum.StorageAccount && accountItem.AccountId == storageAccountId)
                        liClass = "active";
                    else
                        liClass = string.Empty;
                    string storageAccountName = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", accountItem.StoragePluginName, accountItem.AccountName.Shorten(20));
                %>
                <li class="<%=liClass %>">
                    <%=Html.ActionLink(storageAccountName, "GetFolder", "StorageAccount", new { id = accountItem.AccountId }, new { @class = "ajax_menu_link" })%> 
                </li>                
        <%  } %>
        <li class="divider"></li>
        <li class="nav-header">
            <%=ViewResources.SharedResources.MenuTitleSettings%>
        </li>        
        <li <%=activationType == MenuActivator.ActivationTypeEnum.EditProfile? "class=\"active\"" : string.Empty %>>
            <%=Html.ActionLink(ViewResources.SharedResources.MenuItemProfileMgr, "Edit", "Account", null, new { @class = "ajax_menu_link" })%> 
        </li>
        <li <%=activationType == MenuActivator.ActivationTypeEnum.StorageAccountsSettings? "class=\"active\"" : string.Empty %>>            
            <%=Html.ActionLink(ViewResources.SharedResources.MenuItemAccountsMgr, "StorageAccounts", "User", new { Id = Model.StorageAccountsCollection.UserId }, new { @class = "ajax_menu_link" })%>
        </li>
        <% if (Request.IsAuthenticated) {%> 
            <li class="divider"></li>
            <li>
                <%= Html.ActionLink(ViewResources.SharedResources.LogOffLinkContent, "LogOff", "Account")%>
            </li>
        <%} %>
    </ul>
</div>


 
