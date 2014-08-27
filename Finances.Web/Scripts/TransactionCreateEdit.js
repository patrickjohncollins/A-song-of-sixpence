$(document).ready(function () {

    $("#DebitAccountID").change(accountChange).keypress(accountChange);
    $("#CreditAccountID").change(accountChange).keypress(accountChange);
    $("#Foreign").change(function () { showHideForeignAmount(false) });
    $("#ForeignTotal").change(foreignTotalChange);
    $("#ExchangeRate").change(exchangeRateChange);
    $("#Total").change(totalChange);
    $("#TotalExTax").change(function () { reformatDecimal("#TotalExTax") });
    $("#TotalTax").change(function () { reformatDecimal("#TotalTax") });
    $("#Repeat").change(function () { showHideRepeat() });

    showHideForeignAmount(true);
    showHideBusinessFields(true);
    showHideRepeat();

    function showHideForeignAmount(startup) {
        if ($("#Foreign").is(":checked")) {
            $("div.ForeignAmount").slideDown();
        } else {
            if (startup) {
                $("div.ForeignAmount").hide();
            } else {
                $("div.ForeignAmount").slideUp();
            }
            // TODO : Clear field values
        }
    }

    function accountChange() {
        showHideBusinessFields(false);
    }

    function showHideBusinessFields(startup) {
        var debitAccount = $("#DebitAccountID option:selected").text();
        var creditAccount = $("#CreditAccountID option:selected").text();
        if (debitAccount.indexOf("Soc Gen") >= 0 || creditAccount.indexOf("Soc Gen") >= 0) { // hard-coding strings is cheating...
            if (startup) {
                $("div.Business").show();
            } else {
                $("div.Business").slideDown();
            }
        } else {
            if (startup) {
                $("div.Business").hide();
            } else {
                $("div.Business").slideUp();
            }
        }
    }

    function showHideRepeat() {
        if ($("#Repeat").is(":checked")) {
            $("div.Repeat").show();
        } else {
            $("div.Repeat").hide();
        }
    }


    function foreignTotalChange() {
        reformatDecimal("#ForeignTotal");
        calculateAmountFromForeign();
    }
    function exchangeRateChange() {
        reformatDecimal("#ExchangeRate");
        calculateAmountFromForeign();
    }
    function totalChange() {
        reformatDecimal("#Total");
    }

    function reformatDecimal(field) {
        $(field).val(Number.parseLocale($(field).val()).localeFormat());
    }

    function calculateAmountFromForeign() {
        if ($("#Foreign").is(":checked")) {
            calculateAmount("#Total", "#ForeignTotal");
        }
        function calculateAmount(localAmount, foreignAmount) {
            foreignAmountVal = Number.parseLocale($(foreignAmount).val());
            exchangeRateVal = Number.parseLocale($("#ExchangeRate").val());
            localAmountVal = foreignAmountVal / exchangeRateVal;
            $(localAmount).val(localAmountVal.localeFormat("N"));
        }
    }
    
});
