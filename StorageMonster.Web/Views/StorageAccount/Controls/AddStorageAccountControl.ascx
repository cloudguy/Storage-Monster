<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StorageMonster.Web.Models.StorageAccount.StorageAccountModel>" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>

<div id="addStorageAccountControl">
    <% Html.EnableClientValidation(); %>
    <% Html.RenderPartial("~/Views/Shared/Controls/MessagesControl.ascx", ViewResources.StorageAccountResources.AddStorageAccountValidationSummary); %>

    <% if (Model != null) { %>
        <div class="well">
            <% using (Html.BeginForm()) { %>    
                <fieldset>
                    <legend><%=ViewResources.StorageAccountResources.AddTitle%></legend>
                    <p>             
                        <%= Html.LocalizedLabelFor(model => model.PluginId)%>
                        <%= Html.DropDownListFor(model => model.PluginId, Model.StoragePlugins)%>                  
                        <%= Html.ValidationMessageFor(model => model.PluginId)%>
                    </p>
                    <p>
                        <%= Html.LocalizedLabelFor(model => model.AccountName)%>
                        <%= Html.TextBoxFor(model => model.AccountName)%>                            
                        <%= Html.ValidationMessageFor(model => model.AccountName)%>                   
                    </p>
                    <p>
                        <%=Html.AntiForgeryToken(Constants.Salt_StorageAccount_Add)%>
                        <input type="submit" class="btn" value="<%=ViewResources.StorageAccountResources.AddButtonText %>" />
                    </p>
                </fieldset>    
            <%} %>
        </div>
    <% } %>
</div>