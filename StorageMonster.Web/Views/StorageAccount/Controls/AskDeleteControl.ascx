<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.StorageAccount.AskDeleteModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="StorageMonster.Web" %>

<div id="askDeleteControl">
    <% Html.EnableClientValidation(); %>
    <% Html.RenderPartial("~/Views/Shared/Controls/MessagesControl.ascx", ViewResources.StorageAccountResources.AskDeleteValidationSummary); %>
    
    <% if (Model != null) { %>
        <h4><%=string.Format(CultureInfo.InvariantCulture, ViewResources.StorageAccountResources.AskDeleteQuestionFormat, Html.Encode(Model.StorageAccountName))%></h4>
       
        <% using(Html.BeginForm("Delete", "StorageAccount", FormMethod.Post)) {%>
            <%=Html.HiddenFor(model=>model.ReturnUrl) %>
            <%=Html.HiddenFor(model=>model.StorageAccountId) %>
            <%=Html.AntiForgeryToken(Constants.Salt_StorageAccount_Delete) %>
           <button class="btn btn-danger"><span><%=ViewResources.StorageAccountResources.AskDeleteSubmitButtonText %></span></button>            
           <a href="<%=Url.Content(Model.ReturnUrl) %>" class="btn btn-action"><%=ViewResources.StorageAccountResources.AskDeleteCancelButtonText %></a>
        <% } %>
    <% } %>
</div>