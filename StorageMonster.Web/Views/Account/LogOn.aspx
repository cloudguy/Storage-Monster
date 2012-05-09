<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.LogOnModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewResources.AccountResources.LogOnTitle %>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
   
    <h2><%=ViewResources.AccountResources.LogOnTitle %></h2>
    <p>
        <%=ViewResources.AccountResources.LogOnText%> <%= Html.ActionLink(ViewResources.AccountResources.RegisterLinkContent, "Register")%> <%=ViewResources.AccountResources.RegisterText %>
    </p>
    <%= Html.ValidationSummary(ViewResources.AccountResources.LogOnValidationSummary)%>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.Email)%>:                              
                    <%= Html.TextBoxFor(model=>model.Email)%>
                    <%= Html.ValidationMessageFor(model=>model.Email)%> 
                </p>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.Password)%>:                                
                    <%= Html.PasswordFor(model=>model.Password)%>
                    <%= Html.ValidationMessageFor(model=>Model.Password)%> 
                </p>
                 <p>
                    <%= Html.CheckBoxFor(model=>model.RememberMe)%>
                    <%= Html.LocalizedLabelFor(model => model.RememberMe)%>                    
                    <%= Html.ValidationMessageFor(model=>Model.RememberMe)%> 
                </p>
                <p>
                    <input type="submit" value="<%=ViewResources.AccountResources.LogOnButtonText %>" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>