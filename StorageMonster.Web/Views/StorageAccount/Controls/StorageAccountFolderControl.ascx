<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.StorageAccount.FolderModel>" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services" %>
<%@ Import Namespace="StorageMonster.Services" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Linq" %>

<div class="disablable-enabled" data-widget="folderwidget">
    <div class="topglasspanel"></div>
    <% Html.EnableClientValidation(); %>
    <% Html.RenderPartial("~/Views/Shared/Controls/MessagesControl.ascx", string.Empty); %>
   
    <% if (Model != null && Model.Content != null && ViewData.ModelState.IsValid) {%>
        <% var iconProvider = IocContainer.Instance.Resolve<IIconProvider>(); %>
        <% Html.RenderPartial("~/Views/StorageAccount/Controls/StorageNavigationControl.ascx", Model); %>
        <div class="well">
        <% if (Model.Content.StorageItems.FirstOrDefault() != null) { %>
            <table class="table table-condensed">       
                <thead>
                    <tr>
                        <th class="folder_nav_chkbox">
                            <input type="checkbox" class="js_only"/>
                        </th>
                        <th class="folder_nav_name">
                            Name
                        </th>
                        <th class="folder_nav_size">
                            Size
                        </th>
                        <th class="folder_nav_created">
                            Created
                        </th>
                    </tr>            
                </thead>
                <tbody>   
                    <% foreach (var item in Model.Content.StorageItems.OrderByDescending(i=>i.Itemtype)) { %>
                        <tr>                    
                            <td class="folder_nav_chkbox">
                                <input type="checkbox" name="zz" />
                            </td>
                            <td class="folder_nav_name">
                                <% 
                                    string iconPath;
                                    if (item.Itemtype == StorageMonster.Plugin.StorageItemType.Folder)
                                        iconPath = iconProvider.GetIconPath(item.Name, ItemType.Folder); 
                                    else
                                        iconPath = iconProvider.GetIconPath(item.Name, ItemType.File);  
                                %>
                                <% if (item.Itemtype == StorageMonster.Plugin.StorageItemType.Folder) { %> 
                                    <a class="ajax_folder_link" href="<%= Url.Action("GetFolder", "StorageAccount", new { id = item.StorageAccountId, path = item.Path}) %>">
                                        <img src="<%=Url.Content(iconPath) %>" alt="" />
                                        <%=Html.Encode(item.Name) %>
                                    </a>
                                <% } else {%>
                                    <a href="<%= Url.Action("GetFile", "StorageAccount", new { id = item.StorageAccountId, url = item.Path}) %>">
                                        <img src="<%=Url.Content(iconPath) %>" alt="" />
                                        <%=Html.Encode(item.Name) %>
                                    </a>
                                <% } %>                       
                            </td>
                            <td class="folder_nav_size">
                                Дохуя Gb
                            </td>
                            <td class="folder_nav_created">
                              ---
                            </td>
                        </tr>
                    <%} %>
                </tbody>   
            </table> 
        <% } else { %>
            <h3>Nothing here yet</h3>
        <% } %>
        </div>

        http://multipartparser.codeplex.com
        https://bitbucket.org/lorenzopolidori/http-form-parser/src
        <form method="post" enctype="multipart/form-data" action="<%=Url.Content("~/upload.smh") %>">
        <%--<% using(Html.BeginForm("Index", "Home", FormMethod.Post, new {enctype="multipart/form-data"})) { %>--%>
            <input type="hidden" name="fileName" value="zzz" />
            <input type="file" name="file" />
            <input type="submit" name="submit" />
        <%--<% } %>--%>
        </form>
        <div class="btn-toolbar well"> 
            <button class="btn btn-primary">Create folder</button>
            <button class="btn btn-success">Upload</button>  
            <button class="btn btn-danger">Delete</button>               
        </div>
    <% } %>
</div>