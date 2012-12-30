var AjaxMessages = {
    AjaxOptionsFail: "options must be object"
};
var CssSelectors = {
    ModalLogon: 'ModalLogon',
    ModalLogonBody: 'modal-body'
};
function lockFormLogon() {
    $('<div>')
        .addClass('loader')
        .appendTo('#' + CssSelectors.ModalLogon);
}
function unlockFormLogon() {
    $('div.loader').remove();
    $('#' + CssSelectors.ModalLogon).modal('hide');
}
function WrapLogOn(html) {
    
    $('#' + CssSelectors.ModalLogon + ' .' + CssSelectors.ModalLogonBody).html(html);

    $.validator.unobtrusive.parse($('#' + CssSelectors.ModalLogon));
    $('#' + CssSelectors.ModalLogon).modal('show');
    $('#' + CssSelectors.ModalLogon+' .'+ CssSelectors.ModalLogonBody+' form').on('submit', function (e) {
        e.preventDefault();
        var form = $('#'+ CssSelectors.ModalLogon+' form');
        if (!form.valid())
            return;
        var url = form.attr('action');
        var data = form.serialize();
        Ajax({
            url     : url,
            data    : data,
            dataType: 'json',
            type: 'post',
            success: function (responce) {
                if (typeof responce.Authorized != 'undefined' && responce.Authorized) {
                    unlockFormLogon();
                }
                
            },
            beforeSend:function() {
                lockFormLogon();
            }
        });
    });
}

var Ajax = function(options) {
    if (!options || typeof options != 'object')
        throw AjaxMessages.AjaxOptionsFail;
    $.ajax({
        url: options.url,
        cache: options.cache,
        data: options.data,
        dataType: options.dataType,
        type: options.type,
        beforeSend: function(params) {
            if ($.isFunction(options.beforeSend))
                options.beforeSend(params);
        },
        error: function(error) {
            if ($.isFunction(options.error))
                options.error(error);
        },
        success: function(success) {
            if ($.isFunction(options.success))
                if (typeof success.Authorized != 'undefined' && success.Authorized === false) {
                    WrapLogOn(success.LogOnPage);
                    $('div.loader').remove();
                    return;
                }
            options.success(success);
        },
        complete: function(complete) {
            if ($.isFunction(options.complete))
                options.complete(complete);
        }
    });
};
