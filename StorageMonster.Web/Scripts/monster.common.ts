declare var $;
declare var MonsterApp;
module MonsterApp {   

    export var TemplateNames = {
        MasterViewTmpl: "MasterViewTmpl",
        ProfileTmpl: "ProfileTmpl"
    };

    export var CssSelectors = {
        ModalLogonId: ' #modalLogon ',
        ModalLogonBodyClass: ' .modal-body ',
        ModalLogonHeaderId: ' #modalLogonHeader ',
        ErrorInfoBlock: ' div[data-errorinfo] ',
        TemplateCacheId: ' #templateCache '
    };

    function lockFormLogon() {
        $('<div>')
            .addClass('loader')
            .appendTo(CssSelectors.ModalLogonId);
    }
    function unlockFormLogon() {
        $('div.loader').remove();
        $(CssSelectors.ModalLogonId).modal('hide');
    }

    function showLogOnForm(html, title, oldOptions) {
        $(CssSelectors.ModalLogonId + CssSelectors.ModalLogonBodyClass).html(html);
        $(CssSelectors.ModalLogonHeaderId).text(title);
        $.validator.unobtrusive.parse($(CssSelectors.ModalLogonId));
        $(CssSelectors.ModalLogonId).modal('show');
        $(CssSelectors.ModalLogonId + CssSelectors.ModalLogonBodyClass + 'form').on('submit', function (e) {
            e.preventDefault();
            var form = $(CssSelectors.ModalLogonId + 'form');
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
                    if (typeof data.Authorized != 'undefined') {
                        if (data.Authorized === false) {
                            showLogOnForm(data.LogOnPage, data.LogOnTitle, oldOptions);
                            $('div.loader').remove();
                            return;
                        }
                        if (data.Authorized === true) {
                            unlockFormLogon();
                            Ajax(oldOptions);
                        }
                    }
                },
                error: function (responce) {
                    var message = MonsterApp.Messages.ServerError;
                    if (typeof responce.Error != 'undefined')
                        message = responce.Error;
                    $(CssSelectors.ModalLogonId + CssSelectors.ErrorInfoBlock).text(message).show();
                    $('div.loader').remove();
                },
                beforeSend: function () {
                    lockFormLogon();
                }
            });
        });
    }

    export function Ajax(options: any) {
        options || (options = {});

        var successFunction = options.success;
        options.success = (data: any) => {
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
            if ($.isFunction(successFunction))
                successFunction(data);
        };

        return $.ajax(options);
    }
}