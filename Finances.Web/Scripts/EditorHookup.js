/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery-ui.js" />
$(document).ready(function () {
    $('.date').datepicker({
        dateFormat: "dd/mm/yyyy",
        onClose: function (dateText, inst) {
            // Automatically append the current year if none entered.
            if (isNaN(Date.parse(dateText))) {
                var dt = new Date();
                dateText += "/" + dt.getFullYear();
                if (!isNaN(Date.parse(dateText))) {
                    $(this).val(dateText);
                }
            }
        }
    });

    $('.autocomplete').each(function () {
        var source = $(this).data("autocompletesource");
        $(this).autocomplete({
            source: source,
            minLength: 0
        });
    });
});