<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.Accounts.ProfileModel>" %>

<div id="profileControl" data-widget="profilewidget">
    <div class="topglasspanel"></div>
    <div>
    <% Html.EnableClientValidation(); %>
    <% Html.RenderPartial("~/Views/Shared/Controls/MessagesControl.ascx", ViewResources.AccountResources.ProfileEditValidationSummary); %>

    <% if (Model != null) { %>
        <% Html.RenderPartial("~/Views/Account/Controls/ProfileBaseControl.ascx", Model.BaseModel); %>
        <% Html.RenderPartial("~/Views/Account/Controls/ProfilePasswordControl.ascx", Model.PasswordModel); %>
    <% } %>
    </div>
</div>