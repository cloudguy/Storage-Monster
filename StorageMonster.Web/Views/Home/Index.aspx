<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server" >
<script type="text/javascript" src="Scripts/template.js"></script>
<script type="text/javascript" src="Scripts/jquery.treeview.js"></script>
<link rel="stylesheet" href="Content/jquery.treeview.css" />
</asp:Content>
 
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>
    <script>
        function accounts(addr) {
        $.template('test','<ul>${Name}</ul>');
        this.init = function () {

            $.get(addr, function (prevent) {
                $('#storages').html('');
                var html = ' ';
                html += '<ul id="browser" class="filetree treeview-famfamfam treeview">';
                for (i in prevent.UserStorages) {
                    html += '<li>';
                    html += '<span class="folder">' + prevent.UserStorages[i].Name + '</span>';
                    html += '<ul>';

                    for (j in prevent.UserAccounts) {

                        html += (prevent.UserStorages[i].Id == prevent.UserAccounts[j].StorageId) ? '<li><span class="file">' + prevent.UserAccounts[j].AccountLogin + '</span></li>' : '';
                    }
                    html += '</ul>';
                    html += '</li>';
                }
                html += '</ul>';
                $('#storages').html(html);
                $("#browser").treeview({
                    collapsed: true
                });
            }
                , "json")

        }
        }
        var acc = new accounts("Storage/List");
        acc.init();
       
     $('#refresh').live('click', function () {
         acc.init();
         return false;
     });
    </script>
    <a id="refresh" href="">Refresh</a>
    <div id="storages" ></div>
</asp:Content>

