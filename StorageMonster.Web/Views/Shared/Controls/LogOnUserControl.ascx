<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

 <ul class="nav pull-right">
<% if (Request.IsAuthenticated) {%>  
    <li class="navbar-text">   
        <p>    
            <%=ViewResources.SharedResources.LogOnWelcome%> <b><%= Html.Encode(Page.User.Identity.Name) %></b>          
        </p>
    </li>
    <li class="divider-vertical"></li>
    <li>
        <%= Html.ActionLink(ViewResources.SharedResources.LogOffLinkContent, "LogOff", "Account")%>
    </li>
<% } else { %> 
    <li>
    <%= Html.ActionLink(ViewResources.SharedResources.LogOnLinkContent, "LogOn", "Account")%>
    </li>
<% } %>
</ul>            
                        
                