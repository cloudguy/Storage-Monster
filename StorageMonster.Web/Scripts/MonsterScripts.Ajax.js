(function ($) {
    $.fn.extend({
        //this staff enables validaion for target field
        //after ajax request
        monsterhtml: function (plainHtml) {
            var dom = $(plainHtml);
            $(dom).filter("script").each(function (i) {
                eval($(this).text());
            });
            this.html(plainHtml);
            chargeValidation();
            return this;
        }
    });
})(jQuery);

function MonsterAjax(options) {
    if (typeof options == 'undefined')
        return;

    $.ajax({
        url: options.url,
        type: options.type,
        data: options.data,
        beforeSend: options.beforeSend,
        error: options.error,
        success: function (data) {
            if (data.Unauthorized) {
                $('#logOnControl').remove();
                $('#ajaxLogOnContainer').monsterhtml(data.LogOnPage);               
                $('#ajaxLogOnContainer').modal(
                    { backdrop: "static", keyboard: false }
                );
                return;
            }
            if (data.Error) {
                if (typeof options.error == 'function')
                    options.error();
                return;
            }

            if (typeof options.success == 'function')
                options.success(data);
        },
        complete: options.complete
    });
}


/*-----------------------------------------------------------*/
// LOG ON SECTION
/*-----------------------------------------------------------*/
function BeginLogin() {  
    $('#logoncontent').removeClass("disablable-enabled");
    $('#logoncontent').addClass("disablable");
    $('#logoncontent').addClass("disablable-disabled");     
    
}
function EndLogin() {
    $('#logoncontent').removeClass("disablable");
    $('#logoncontent').removeClass("disablable-disabled");
    $('#logoncontent').addClass("disablable-enabled");  
}
function RequestLogOnError(jqXHR, textStatus, errorThrown) {
    //warning show div    
    $('#logonError').text(MonsterJSMessages.ServerError);
}

function RequestLogOnSuccess(data, selectorToUpdate, logOnUrl) {
    if (data.Success) {        
        selectorToUpdate.modal('toggle');
        return;
    }
    if (data.Html) {
        InsertAjaxHtmlAndEnableValidation(data.Html, selectorToUpdate);        
        return;
    }
    if (data.Error) {       
//warning show div     
        $('#logonError').text(data.Error);        
        return;
    }
    $('#logonError').text(MonsterJSMessages.ServerError);
}

function LogOnSubmit(formSelector, logOnUrl, selectorToUpdate) {    
    formSelector.validate();
    if (formSelector.valid()) {
        $.ajax({
            url: logOnUrl,
            type: 'POST',
            data: formSelector.serialize(),
            beforeSend: BeginLogin,
            error: RequestLogOnError,
            success: function(data) { RequestLogOnSuccess(data, selectorToUpdate, logOnUrl); },
            complete: EndLogin
        });
    }    
    return false;
}