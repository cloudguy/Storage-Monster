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
                        $('#LogOnControl').remove();                        
                        InsertAjaxHtmlAndEnableValidation(data.LogOnPage, $('#AjaxLogOnContainer'));
                        $('#AjaxLogOnContainer').dialog(
                                                        {
                                                            modal: true,
                                                            closeOnEscape: false,
                                                            open: function (event, ui) {
                                                                //hide close button.
                                                                $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                                                            }
                                                        });                        
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


//this staff enables validaion for target field
//after ajax request
function InsertAjaxHtmlAndEnableValidation(plainHtml, targetSelector) {
    var dom = $(plainHtml);
    $(dom).filter("script").each(function (i) {
        eval($(this).text());
    });
    targetSelector.html(dom.html());
    chargeValidation();  
}


/*-----------------------------------------------------------*/
// LOG ON SECTION
/*-----------------------------------------------------------*/
function BeginLogin() {
    $('#LogonWait').html('Please wait...');
}
function EndLogin() {
    $('#LogonWait').html('');   
}
function RequestLogOnError(jqXHR, textStatus, errorThrown) {
    $('#LogonError').html(MonsterJSMessages.ServerError);
}

function RequestLogOnSuccess(data, selectorToUpdate, logOnUrl) {
    if (data.Success) {
        selectorToUpdate.remove();
        return;
    }
    if (data.Html) {
        InsertAjaxHtmlAndEnableValidation(data.Html, selectorToUpdate);        
        return;
    }
    if (data.Error) {
        $('#LogonError').html(data.Error);        
        return;
    }
    $('#LogonError').html(MonsterJSMessages.ServerError);
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