var MonsterApp;
(function (MonsterApp) {
    (function (RenderUtils) {
        var HtmlHelper = (function () {
            function HtmlHelper() { }
            HtmlHelper.prototype.textBoxFor = function (model, propName, attributes) {
                attributes || (attributes = {
                });
                attributes['id'] || (attributes['id'] = propName);
                attributes['name'] = propName;
                attributes['value'] = model.get(propName);
                attributes['type'] = 'text';
                MonsterApp.MetadataUtils.MetadataExtractor.fillValidationAttributes(model, propName, attributes);
                var input = $('<input/>', attributes);
                return input.wrap('<div/>').parent().html();
            };
            HtmlHelper.prototype.labelFor = function (model, propName, attributes) {
                attributes || (attributes = {
                });
                attributes['for'] = propName;
                var label = $('<label/>', attributes);
                var labelText = MonsterApp.MetadataUtils.MetadataExtractor.getDisplayName(model, propName);
                label.text(labelText);
                return label.wrap('<div/>').parent().html();
            };
            HtmlHelper.prototype.validationMessageFor = function (model, propName, attributes) {
                attributes || (attributes = {
                });
                attributes['data-valmsg-for'] = propName;
                attributes['data-valmsg-replace'] = 'true';
                var span = $('<span/>', attributes);
                return span.wrap('<div/>').parent().html();
            };
            HtmlHelper.prototype.selectFor = function (model, propName, attributes, values) {
                attributes || (attributes = {
                });
                attributes['id'] || (attributes['id'] = propName);
                attributes['name'] = propName;
                MonsterApp.MetadataUtils.MetadataExtractor.fillValidationAttributes(model, propName, attributes);
                var select = $('<select/>', attributes);
                if($.isArray(values)) {
                    var selectedValue = model.get(propName);
                    $.each(values, function (index, selectOption) {
                        var optionAttrs = {
                            'value': selectOption.Value
                        };
                        if(selectedValue == selectOption.Value || selectOption.Selected == true) {
                            optionAttrs['selected'] = 'selected';
                        }
                        var option = $('<option/>', optionAttrs).text(selectOption.Text);
                        select.append(option);
                    });
                }
                return select.wrap('<div/>').parent().html();
            };
            return HtmlHelper;
        })();        
        RenderUtils.Html = new HtmlHelper();
    })(MonsterApp.RenderUtils || (MonsterApp.RenderUtils = {}));
    var RenderUtils = MonsterApp.RenderUtils;
})(MonsterApp || (MonsterApp = {}));
