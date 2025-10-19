$.validator.addMethod('requiredwith', function (value, element, params) {
    var currentId = element.id;
    var otherFieldName = params.other;

    var otherElementId;
    if (currentId.includes('_')) {
        var parts = currentId.split('_');
        parts[parts.length - 1] = otherFieldName;
        otherElementId = parts.join('_');
    } else {
        otherElementId = otherFieldName;
    }

    var otherElement = $('#' + otherElementId);
    if (otherElement.length === 0) {
        return true;
    }

    var otherValue = otherElement.val();

    if (!otherValue || !otherValue.trim()) {
        return true;
    }

    if (!value || !value.trim()) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.add('requiredwith', ['other'], function (options) {
    options.rules['requiredwith'] = {
        other: options.params.other
    };
    options.messages['requiredwith'] = options.message;
});