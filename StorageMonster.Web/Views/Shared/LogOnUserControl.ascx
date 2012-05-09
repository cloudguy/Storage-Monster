<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        <%=ViewResources.SharedResources.LogOnWelcome%> <b><%= Html.Encode(Page.User.Identity.Name) %></b>
        [ <%= Html.ActionLink(ViewResources.SharedResources.LogOffLinkContent, "LogOff", "Account")%> ]
<%
    }
    else {
%> 
        [ <%= Html.ActionLink(ViewResources.SharedResources.LogOnLinkContent, "LogOn", "Account")%> ]
<%
    }
%>
