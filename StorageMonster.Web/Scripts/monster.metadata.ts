declare var $;
declare var _;
declare var MonsterApp;

interface IMetadataHelper {
    getMetadataForProperty: (model: any, propName: string) => any;    
    getDisplayName: (model: any, propName: string) => any;  
    fillValidationAttributes: (model: any, propName: string, attributeObject: any) => any;
}


module MonsterApp.MetadataUtils {    
    class MetadataHelper implements IMetadataHelper {
        getMetadataForProperty(model: any, propName: string) {
            var metaData = model.get('MetaData');
            if (!MonsterApp.Utils.isObjectPresent(metaData))
                return undefined;
            return metaData[propName];
        }

        getDisplayName(model: any, propName: string) {
            var metadata = this.getMetadataForProperty(model, propName);
            if (!MonsterApp.Utils.isObjectPresent(metadata))
                return '';
            var displayName = metadata['DisplayName'];
            if (!MonsterApp.Utils.isObjectPresent(displayName))
                return '';
            return displayName;
        }

        fillValidationAttributes(model: any, propName: string, attributeObject: any) {
            attributeObject || (attributeObject = {});
            var metadata = this.getMetadataForProperty(model, propName);
            if (!MonsterApp.Utils.isObjectPresent(metadata))
                return attributeObject;

            var validators = metadata['Validators'];
            if (!$.isArray(validators))
                return attributeObject;

            var validationAttributes = {
                'data-val': 'true'
            };

            _.reduce(validators, this.fillValidatorAttributes, validationAttributes, this);

            return $.extend(attributeObject, validationAttributes);
        }

        fillValidatorAttributes(validationAttributes: any, validator: any) {
            var rules = validator['ValidationRules'];
            if (!$.isArray(rules))
                return validationAttributes;
            return _.reduce(rules, this.fillValidatorRuleAttributes, validationAttributes, this);
        }

        fillValidatorRuleAttributes(validationAttributes: any, validationRule: any) {
            var validationType = validationRule['ValidationType'];
            if (!MonsterApp.Utils.isObjectPresent(validationType))
                return validationAttributes;
            var errorMessage = validationRule['ErrorMessage'] || '';
            var rulePrefix = 'data-val-' + validationType;
            validationAttributes[rulePrefix] = errorMessage;
            var validationParameters = validationRule['ValidationParameters'];
            if (!MonsterApp.Utils.isObjectPresent(validationParameters))
                return validationAttributes;

            return _.chain(validationParameters)
                    .pairs((p) => p)
                    .reduce((memo, val) => { memo[rulePrefix + '-' + val[0]] = val[1]; return memo; }, validationAttributes)
                    .value();
        }
    }
    
    export var MetadataExtractor: IMetadataHelper = new MetadataHelper();
}
