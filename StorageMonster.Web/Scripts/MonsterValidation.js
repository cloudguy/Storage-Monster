jQuery.validator.addMethod("minstrlength", function (value, element, params) {
    if (this.optional(element)) {
        return true;
    }
            
    if (value.length >= params.minlength) {                
            return true;                
    }

    return false;
});

jQuery.validator.addMethod("propmatch", function (value, element, params) {
    if (this.optional(element)) {
        return true;
    }
    var valueToCheck = $(element).parents('form:first').find('#' + params.prop_id).val();
    if (value === valueToCheck) {
        return true;
    }

    return false;
});