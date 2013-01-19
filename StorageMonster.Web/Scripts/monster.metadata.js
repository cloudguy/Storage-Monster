var MonsterApp;
(function (MonsterApp) {
    (function (MetadataUtils) {
        var MetadataHelper = (function () {
            function MetadataHelper() { }
            MetadataHelper.prototype.getMetadataForProperty = function (model, propName) {
                var metaData = model.get('MetaData');
                if(!MonsterApp.Utils.isObjectPresent(metaData)) {
                    return undefined;
                }
                return metaData[propName];
            };
            MetadataHelper.prototype.getDisplayName = function (model, propName) {
                var metadata = this.getMetadataForProperty(model, propName);
                if(!MonsterApp.Utils.isObjectPresent(metadata)) {
                    return '';
                }
                var displayName = metadata['DisplayName'];
                if(!MonsterApp.Utils.isObjectPresent(displayName)) {
                    return '';
                }
                return displayName;
            };
            MetadataHelper.prototype.fillValidationAttributes = function (model, propName, attributeObject) {
                attributeObject || (attributeObject = {
                });
                var metadata = this.getMetadataForProperty(model, propName);
                if(!MonsterApp.Utils.isObjectPresent(metadata)) {
                    return attributeObject;
                }
                var validators = metadata['Validators'];
                if(!$.isArray(validators)) {
                    return attributeObject;
                }
                var validationAttributes = {
                    'data-val': 'true'
                };
                _.reduce(validators, this.fillValidatorAttributes, validationAttributes, this);
                return $.extend(attributeObject, validationAttributes);
            };
            MetadataHelper.prototype.fillValidatorAttributes = function (validationAttributes, validator) {
                var rules = validator['ValidationRules'];
                if(!$.isArray(rules)) {
                    return validationAttributes;
                }
                return _.reduce(rules, this.fillValidatorRuleAttributes, validationAttributes, this);
            };
            MetadataHelper.prototype.fillValidatorRuleAttributes = function (validationAttributes, validationRule) {
                var validationType = validationRule['ValidationType'];
                if(!MonsterApp.Utils.isObjectPresent(validationType)) {
                    return validationAttributes;
                }
                var errorMessage = validationRule['ErrorMessage'] || '';
                var rulePrefix = 'data-val-' + validationType;
                validationAttributes[rulePrefix] = errorMessage;
                var validationParameters = validationRule['ValidationParameters'];
                if(!MonsterApp.Utils.isObjectPresent(validationParameters)) {
                    return validationAttributes;
                }
                return _.chain(validationParameters).pairs(function (p) {
                    return p;
                }).reduce(function (memo, val) {
                    memo[rulePrefix + '-' + val[0]] = val[1];
                    return memo;
                }, validationAttributes).value();
            };
            return MetadataHelper;
        })();        
        MetadataUtils.MetadataExtractor = new MetadataHelper();
    })(MonsterApp.MetadataUtils || (MonsterApp.MetadataUtils = {}));
    var MetadataUtils = MonsterApp.MetadataUtils;
})(MonsterApp || (MonsterApp = {}));
