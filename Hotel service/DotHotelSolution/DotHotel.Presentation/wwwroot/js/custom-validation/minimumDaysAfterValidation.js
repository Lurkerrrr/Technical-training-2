$.validator.addMethod('minimumdaysafter', function (value, element, params) {
    if (!value || !params.other) {
        return true;
    }

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

    var currentDate = new Date(value);
    var otherDate = new Date(otherElement.val());
    var minimumDays = parseInt(params.days) || 1;

    if (isNaN(currentDate.getTime()) || isNaN(otherDate.getTime())) {
        return true;
    }

    var minimumDate = new Date(otherDate);
    minimumDate.setDate(minimumDate.getDate() + minimumDays);

    return currentDate >= minimumDate;
});

$.validator.unobtrusive.adapters.add('minimumdaysafter', ['other', 'days'], function (options) {
    options.rules['minimumdaysafter'] = {
        other: options.params.other,
        days: options.params.days
    };
    options.messages['minimumdaysafter'] = options.message;
});