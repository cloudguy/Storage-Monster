<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Plugin.StorageQueryResult>" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	GetFolder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>GetFolder</h2>

    <%=Html.ValidationSummary("Error", new { @class = "alert alert-error" })%>
   
    <% if (Model != null && ViewData.ModelState.IsValid) {%>
        <table class="table table-condensed">       
            <tbody>   
                <% foreach (var item in Model.StorageItems.OrderBy(i=>i.Itemtype)) { %>
                    <tr>
                        <td>
                            <% if (item.Itemtype == StorageMonster.Plugin.StorageItemType.Folder) { %>
                                <% using(Html.BeginForm()) { %>
                                    <%=Html.AntiForgeryToken(Constants.Salt_StorageAccount_GetFolder) %>
                                    <input type="hidden" name="id" value="<%=item.StorageAccountId.ToString(CultureInfo.InvariantCulture) %>" />
                                    <input type="hidden" name="path" value="<%=item.Path %>" />
                                    <button type="submit"><span><%= Html.Encode(item.Name) %></span></button> 
                                <% } %>
                            <% } else {%>
                                <%=Html.Encode(item.Name) %>
                            <% } %>                       
                        </td>
                    </tr>
                <%} %>
            </tbody>   
        </table>
    <% } %>

</asp:Content>

