<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ProfileModel>" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Models.Accounts" %>

<div id="ProfileControl">  

    <%= Html.ValidationSummary(ViewResources.AccountResources.ProfileEditValidationSummary, new { @class = "alert alert-error" })%>        
    <%= Html.RequestSuccessInfo(new { @class = "alert alert-success" })%>   
   
    <% Html.EnableClientValidation(); %>  

    <% if (Model != null) { %>
        <% Html.RenderPartial("~/Views/Account/ProfileBaseControl.ascx", Model.BaseModel); %>
        <% Html.RenderPartial("~/Views/Account/ProfilePasswordControl.ascx", Model.PasswordModel); %>
    <%} %>
</div>