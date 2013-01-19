declare var $;
declare var _;
declare var MonsterApp;

interface IHtmlHelper {
    textBoxFor: (model: any, propName: string, attributes: any) => any;
    labelFor: (model: any, propName: string, attributes: any) => any;
    validationMessageFor: (model: any, propName: string, attributes: any) => any;
}

module MonsterApp.RenderUtils {

    class HtmlHelper implements IHtmlHelper {
        textBoxFor(model: any, propName: string, attributes: any) {
            attributes || (attributes = {});
            attributes['id'] || (attributes['id'] = propName);
            attributes['name'] = propName;
            attributes['value'] = model.get(propName);
            attributes['type'] = 'text';
            MonsterApp.MetadataUtils.MetadataExtractor.fillValidationAttributes(model, propName, attributes);
            var input = $('<input/>', attributes);
            return input.wrap('<div/>').parent().html();
        }

        labelFor(model: any, propName: string, attributes: any) {
            attributes || (attributes = {});
            attributes['for'] = propName;
            var label = $('<label/>', attributes);
            var labelText = MonsterApp.MetadataUtils.MetadataExtractor.getDisplayName(model, propName);
            label.text(labelText);
            return label.wrap('<div/>').parent().html();
        }

        validationMessageFor(model: any, propName: string, attributes: any) {
            attributes || (attributes = {});
            attributes['data-valmsg-for'] = propName;
            attributes['data-valmsg-replace'] = 'true';
            var span = $('<span/>', attributes);
            return span.wrap('<div/>').parent().html();
        }

        selectFor(model: any, propName: string, attributes: any, values: Array) {
            attributes || (attributes = {});
            attributes['id'] || (attributes['id'] = propName);
            attributes['name'] = propName;
            MonsterApp.MetadataUtils.MetadataExtractor.fillValidationAttributes(model, propName, attributes);
            var select = $('<select/>', attributes);
            if ($.isArray(values)) {
                var selectedValue = model.get(propName);

                $.each(values, (index, selectOption) => {
                    var optionAttrs = { 'value': selectOption.Value };
                    if (selectedValue == selectOption.Value || selectOption.Selected == true)
                        optionAttrs['selected'] = 'selected';               
                    var option = $('<option/>', optionAttrs).text(selectOption.Text);
                    select.append(option);
                });               
            }
            return select.wrap('<div/>').parent().html();
        }
    }
    export var Html: IHtmlHelper = new HtmlHelper();    
}
