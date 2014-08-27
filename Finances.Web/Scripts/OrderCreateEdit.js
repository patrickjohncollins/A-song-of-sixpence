$(document).ready(function () {

    $("#Days").change(daysChange);
    $("#Rate").change(rateChange);
    $("#Travel").change(travelChange);

    function daysChange() {
        reformatAndCalculate("#Days");
    }

    function rateChange() {
        reformatAndCalculate("#Days");
    }

    function travelChange() {
        reformatAndCalculate("#Days");
    }

    function reformatAndCalculate(selector) {
        reformatDecimal(selector);
        calculateFeeAndTotal();
    }

    function reformatDecimal(field) {
        $(field).val(Number.parseLocale($(field).val()).localeFormat());
    }

    function calculateFeeAndTotal() {
        var fee = calculateFee();
        var travel = Number.parseLocale($("#Travel").val());
        var total = fee;
        if (!isNaN(travel)) fee += travel;
        $("#Total").val(total/*.localeFormat()*/);
        return total;
    }

    function calculateFee() {
        var days = Number.parseLocale($("#Days").val());
        var rate = Number.parseLocale($("#Rate").val());
        var fee = 0;
        if (!isNaN(days) && !isNaN(rate)) fee = days * rate;
        $("#Fee").val(fee/*.localeFormat()*/);
        return fee;
    }

});
