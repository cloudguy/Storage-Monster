<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Object>" %>
<%@ Import Namespace="StorageMonster.Common.DataAnnotations" %>
<%@ Import Namespace="StorageMonster.Web" %>
<%@ Import Namespace="StorageMonster.Web.Services.Extensions" %>
<%@ Import Namespace="StorageMonster.Utilities" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>


<% Html.EnableClientValidation(); %>
<%=Html.ValidationSummary()  %>

<% if (Model != null) { %>
    <% using (Html.BeginForm()) {%>
        <div class="well">
            <% if (ViewData[Constants.StorageAccountTitleViewDataKey] != null) { %>
            <%     string accountName = (string)ViewData[Constants.StorageAccountTitleViewDataKey];%>
                   <h2><%=Html.Encode(accountName)%></h2>
            <% } %>

            <% Type displayAttrType = typeof(MonsterDisplayAttribute);
               var propList =  ReflectionHelper.GetPropertiesWithAttribute(Model, displayAttrType) 
                   .OrderBy(p=>((MonsterDisplayAttribute)p.GetCustomAttributes(displayAttrType, true)[0]).ShowOrder);
               long stamp = 0;
               if (ViewData[Constants.StampFormKey] != null)
                    stamp = (long)ViewData[Constants.StampFormKey];
            %>
            <% foreach (var propertyInfo in propList) { %>
                <p>
                    <%=Html.LocalizedLabel(propertyInfo.Name)%>
                    <%=Html.RenderProperty(propertyInfo, new Dictionary<string, object> { {"class", "input-large"} })%>
                    <%=Html.ValidationMessage(propertyInfo.Name)%>
                </p>
            <%} %>
            <p>
                <%=Html.Hidden(Constants.StorageAccountIdFormKey, ViewData[Constants.StorageAccountIdFormKey])%>       
                <%=Html.Hidden(Constants.StampFormKey, stamp)%> 
                <%=Html.AntiForgeryToken(Constants.Salt_StorageAccount_Edit)%>
                <input type="submit" class="btn" name="submit" />
            </p>            
        </div>
    <%} %>

<% } %>