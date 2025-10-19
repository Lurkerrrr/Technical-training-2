$(document).ready(function () {
    $('#Filter_AvailableFrom').on('change', function () {
        $('#Filter_AvailableFrom').valid();
        $('#Filter_AvailableTo').valid();
    });
    $('#Filter_AvailableTo').on('change', function () {
        $('#Filter_AvailableTo').valid();
        $('#Filter_AvailableFrom').valid();
    });
});