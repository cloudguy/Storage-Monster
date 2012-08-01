<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StorageMonster.Web.Models.Accounts.RegisterModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>


<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewResources.AccountResources.RegisterTitle %>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="LogOnContent" runat="server">   
<div>
    <% Html.EnableClientValidation(); %>

    <h2><%=ViewResources.AccountResources.CreateNewAccount %></h2>
    
    <p>
        <%=String.Format(CultureInfo.CurrentCulture, ViewResources.AccountResources.PasswordRequirementFormat, Model.MinPasswordLength)%>        
        <br/>
        <%=ViewResources.AccountResources.NameRequirement %>
    </p>
    <%= Html.ValidationSummary(ViewResources.AccountResources.RegisterValidationSummary, new { @class = "alert alert-error" })%>   

    <% using (Html.BeginForm()) { %>
        <div class="well">
            <fieldset>
                <legend><%=ViewResources.AccountResources.AccountInfoTitle %></legend>
                <p>                        
                    <%= Html.LocalizedLabelFor(model => model.UserName)%>                               
                    <%= Html.TextBoxFor(model => model.UserName, new { @class = "fullwidth" })%>
                    <%= Html.ValidationMessageFor(model=>model.UserName)%>                   
                </p>               
                <p>
                    <%= Html.LocalizedLabelFor(model => model.Email)%>                             
                    <%= Html.TextBoxFor(model => model.Email, new { @class = "fullwidth" })%>
                    <%= Html.ValidationMessageFor(model=>model.Email)%> 
                </p>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.Password)%>                           
                    <%= Html.PasswordFor(model => model.Password, new { @class = "fullwidth" })%>
                    <%= Html.ValidationMessageFor(model=>Model.Password)%> 
                </p>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.ConfirmPassword)%>                             
                    <%= Html.PasswordFor(model => model.ConfirmPassword, new { @class = "fullwidth" })%>
                    <%= Html.ValidationMessageFor(model=>model.ConfirmPassword)%> 
                </p>
                <p>
                    <%= Html.LocalizedLabelFor(model => model.Locale)%>
                    <%= Html.DropDownListFor(model => model.Locale, Model.SupportedLocales, new { @class = "fullwidth" })%>                  
                    <%= Html.ValidationMessageFor(model=>model.Locale)%>
                </p>
                <p>
                    <input type="submit" class="btn" value="<%=ViewResources.AccountResources.RegisterButtonText %>" />
                </p>
            </fieldset>
        </div>
    <% } %>
</div>
</asp:Content>
