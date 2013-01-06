MonsterApp.RenderUtils = (function () {
    var util = {};
    util.metadataKey = "MetaData";
    util.emptyStringStab = function() {
        return '';
    };
    util.undefinedStab = function() {
        return undefined;
    };
    util.getNameProperty = function(propName) {
        return ' name="' + _.escape(propName) + '" ';
    };
    util.getIdProperty = function (propName) {
        return ' id="' + _.escape(propName) + '" ';
    };
    util.getAttributesString = function (attributes) {
        return MonsterApp.Utils.ifPresent(attributes,
            function() {
                return _.chain(attributes)
                    .pairs(function(p) { return p; })
                    .reduce(function(memo, val) { return memo + _.escape(val[0]) + "=" + "'" + _.escape(val[1]) + "' "; }, '')
                    .value();
            },
            util.emptyStringStab);
        
    };
    util.getMetadataForProperty = function(model, propName) {
        return MonsterApp.Utils.ifPresent(model.get(util.metadataKey),
            function (metaData) {
                return MonsterApp.Utils.ifPresent(metaData[propName], function(prop) {
                    return prop;
                }, util.undefinedStab);
            },
            util.undefinedStab
        );
    };
    util.getValidationString = function (model, propName) {
        var validationString = '';
        var metadata = util.getMetadataForProperty(model, propName) || {};
        var validators = metadata['Validators'];
        if (!_.isArray(validators))
            return validationString;
        validationString = _.reduce(validators, util.getValidatorString, ' data-val="true" ');
        return validationString;
    };

    util.getValidatorString = function (validationString, validator) {
        var rules = validator['ValidationRules'];
        if (!_.isArray(rules))
            return validationString;
        return _.reduce(rules, util.getValidatorRuleString, validationString);
    };
    
    util.getValidatorRuleString = function(validationString, validationRule) {
        var validationType = validationRule['ValidationType'];
        if (!MonsterApp.Utils.isPresent(validationType))
            return validationString;
        var errorMessage = validationRule['ErrorMessage'] || '';
        var rulePrefix = ' data-val-' + _.escape(validationType);
        validationString += rulePrefix + '="' + _.escape(errorMessage) + '" ';
        var validationParameters = validationRule['ValidationParameters'];
        if (!_.isObject(validationParameters))
            return validationString;
       
        return validationString + _.chain(validationParameters)
                    .pairs(function (p) { return p; })
                    .reduce(function (memo, val) { return memo + rulePrefix+'-'+_.escape(val[0]) + "=" + "'" + _.escape(val[1]) + "' "; }, '')
                    .value();
    };
    
    util.getDisplayName = function(model, propName) {
        var metadata = util.getMetadataForProperty(model, propName) || {};
        return MonsterApp.Utils.ifPresent(metadata['DisplayName'], function(prop) {
            return prop;
        }, util.emptyStringStab);
    };
    
    util.LabelFor = function (model, propName, attributes) {
        var label = '<label for="' + _.escape(propName) + '" ' + util.getAttributesString(attributes) + '>';
        label+=_.escape(util.getDisplayName(model, propName));
        label += "</label>";
        return label;
    };
    
    util.textBoxFor = function (model, propName, attributes) {
        var input = '<input type="text" '
            + util.getNameProperty(propName)
            + util.getIdProperty(propName)
            + util.getAttributesString(attributes)
            + util.getValidationString(model, propName)
            + ' value="' + _.escape(model.get(propName)) + '"'
            + ' />';
        return input;
    };
    
    util.selectFor = function (model, propName, attributes, values) {
        var select = '<select '
            + util.getAttributesString(attributes)
            + util.getNameProperty(propName)
            + util.getIdProperty(propName)
            + util.getValidationString(model, propName)
            + ' >';
        if (_.isArray(values)) {
            var selectedValue = model.get(propName);
            select += values.reduce(function (memo, option) {
                memo += '<option ';
                if (selectedValue == option.Value || option.Selected == true)
                    memo += 'selected = "selected"';
                memo += ' value="' + _.escape(option.Value) + '">' + _.escape(option.Text) + '</option>';
                return memo;
            }, '');
        }
        select += '</select>';
        return select;
    };
    util.validationMessageFor = function (model, propName, attributes) {
        var span = '<span ' + 'data-valmsg-for="'+_.escape(propName) + '"'
            + util.getAttributesString(attributes)
            + ' data-valmsg-replace="true" >'
            + '</span>';
        return span;
    };
    return util;
})();

