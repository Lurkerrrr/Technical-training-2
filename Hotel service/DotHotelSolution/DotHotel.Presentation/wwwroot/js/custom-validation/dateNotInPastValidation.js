$.validator.addMethod('datenotinpast', function (value, element) {
    if (!value) {
        return true;
    }

    var parts = value.split('-');
    var inputDate = new Date(parts[0], parts[1] - 1, parts[2]);

    var today = new Date();
    today.setHours(0, 0, 0, 0);

    return inputDate >= today;
});

$.validator.unobtrusive.adapters.add('datenotinpast', [], function (options) {
    options.rules['datenotinpast'] = true;
    options.messages['datenotinpast'] = options.message;
});
