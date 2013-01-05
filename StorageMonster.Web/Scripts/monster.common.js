﻿var MonsterApp = {
    Views: {},
    Context: {},
    Models: {}
};

MonsterApp.CssSelectors = {
    ModalLogonId: ' #modalLogon ',
    ModalLogonBodyClass: ' .modal-body ',
    ModalLogonHeaderId: ' #modalLogonHeader ',
    ErrorInfoBlock: ' div[data-errorinfo] ',
    TemplateCacheId: ' #templateCache '
};

MonsterApp.TemplateNames = {
    MasterViewTmpl: "MasterViewTmpl",
    ProfileTmpl: "ProfileTmpl"
};

function lockFormLogon() {
    $('<div>')
        .addClass('loader')
        .appendTo(MonsterApp.CssSelectors.ModalLogonId);
}
function unlockFormLogon() {
    $('div.loader').remove();
    $(MonsterApp.CssSelectors.ModalLogonId).modal('hide');
}

function showLogOnForm(html, title, oldOptions) {
    $(MonsterApp.CssSelectors.ModalLogonId + MonsterApp.CssSelectors.ModalLogonBodyClass).html(html);
    $(MonsterApp.CssSelectors.ModalLogonHeaderId).text(title);
    $.validator.unobtrusive.parse($(MonsterApp.CssSelectors.ModalLogonId));
    $(MonsterApp.CssSelectors.ModalLogonId).modal('show');
    $(MonsterApp.CssSelectors.ModalLogonId + MonsterApp.CssSelectors.ModalLogonBodyClass + 'form').on('submit', function (e) {
        e.preventDefault();
        var form = $(MonsterApp.CssSelectors.ModalLogonId + 'form');
        if (!form.valid())
            return;
        var url = form.attr('action');
        var formData = form.serialize();
        $.ajax({
            url: url,
            data: formData,
            dataType: 'json',
            type: 'post',
            success: function (data) {
                if (typeof data.Authorized != 'undefined'){
                    if (data.Authorized === false) {
                        showLogOnForm(data.LogOnPage, data.LogOnTitle, oldOptions);
                        $('div.loader').remove();
                        return;
                    }
                    if (data.Authorized === true) {
                        unlockFormLogon();
                        MonsterApp.Ajax(oldOptions);
                    }
                }
            },
            error: function (responce) {
                var message = MonsterApp.Messages.ServerError;
                if (typeof responce.Error != 'undefined') 
                    message = responce.Error;
                $(MonsterApp.CssSelectors.ModalLogonId + MonsterApp.CssSelectors.ErrorInfoBlock).text(message).show();
                $('div.loader').remove();
            },
            beforeSend: function() {
                lockFormLogon();
            }
        });
    });
}

MonsterApp.Ajax = function (options) {
    if (!options || typeof options != 'object')
        throw MonsterApp.Messages.AjaxOptionsFail;
    return $.ajax({
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
        success: function (data) {
            if (typeof data.Error != 'undefined') {
                if ($.isFunction(options.error)) 
                    options.error(data);
                return;
            }
            if (typeof data.Authorized != 'undefined' && data.Authorized === false) {
                    showLogOnForm(data.LogOnPage, data.LogOnTitle, options);
                    $('div.loader').remove();
                    return;
            }
            if ($.isFunction(options.success))
                options.success(data);
        },
        complete: function(complete) {
            if ($.isFunction(options.complete))
                options.complete(complete);
        }
    });
};
