<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.StorageAccount.FolderModel>" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Plugin" %>
<%@ Import Namespace="StorageMonster.Services" %>
<%@ Import Namespace="StorageMonster.Web.Services" %>

<% if (Model != null && ViewData.ModelState.IsValid && Model.Content.IsValid) { %>
    <% var iconProvider = IocContainer.Instance.Resolve<IIconProvider>(); %>
    <div id="storageNavigation">      
        <ul class="nav_history_breadcrumb breadcrumb"> 
            <% StoragePathItem item = Model.Content.CurrentPath; %>
            <% int navCounter = 1; %>
            <% while (item != null) { %> 
                <li>                    
                    <% if (navCounter == 1) { %>
                        <a class="nav_history_imagelink ajax_folder_link" href="<%=Url.Action("GetFolder", "StorageAccount", new { id = Model.StorageAccount.Id, path = item.Url }, null) %>">
                            <img src="<%=Url.Content(iconProvider.GetImagePath("network_drive.png")) %>" alt="img" />
                        </a>          
                    <% } else { %>
                        <%=Html.ActionLink(item.Name, "GetFolder", "StorageAccount", new { id = Model.StorageAccount.Id, path = item.Url }, new { @class = "ajax_folder_link" })%>                       
                    <% } %>                              
                    <span class="divider">/</span>
                </li>
                <% item = item.ChildItem; %>
                <% navCounter++; %>
            <% } %>
        </ul>
    </div>
<% } %>