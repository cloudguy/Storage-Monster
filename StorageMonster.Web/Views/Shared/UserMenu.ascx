<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.UserMenuModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Util" %>
<script>
    $(function () {
        var accordion_head = $('.accordion > li > a'),
				accordion_body = $('.accordion li > .sub-menu');

       

        accordion_head.first().addClass('active').next().slideDown('normal');

        

        accordion_head.on('click', function (event) {

             

            event.preventDefault();

           

            if ($(this).attr('class') != 'active') {
                accordion_body.slideUp('normal');
                $(this).next().stop(true, true).slideToggle('normal');
                accordion_head.removeClass('active');
                $(this).addClass('active');
            }

        });
    
    });
</script>
<div id="wraper">
<ul class="accordion">
<li id="one" class="files">
<a href="#one">Storage<span><%=Model.Accounts.Count().ToString(CultureInfo.CurrentCulture)%></span></a>
<ul class="sub-menu">
<% int counter=1; foreach (var accountItem in Model.Accounts) { %>
     
    <li><a href="<%=Url.Action("Content", "Storage", new {accountId = accountItem.AccountId}) %>"><em>0<%=counter.ToString(CultureInfo.CurrentCulture)%>
    </em><%= Html.Encode(accountItem.StorageName) %>    
    <span><%=Html.Encode(accountItem.AccountLogin.ShortenString(10)) %> - <%=Html.Encode(accountItem.AccountServer.ShortenString(20)) %> </span></a></li>
     
         
             
            
         
    
<%   counter++; } %>
</ul>
</li>

<li id="two" class="mail">
<a href="#two">Profile</a>
 <ul class="sub-menu">

<li><%=Html.ActionLink("Profile Manager", "Profile", "User", new {Id = Model.UserId}, new {@class = "link_class", myattr="attr"} ) %>  </li>

<li><%=Html.ActionLink("Account Manager", "Accounts", "User", new {Id = Model.UserId}, new {@class = "link_class"}) %> </li>
</ul>
</li>
</ul>
</div>


 
