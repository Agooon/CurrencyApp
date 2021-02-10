
$(function () {

    // Setup
    // 1
    if ($("#Checkboxes_NameCheck")[0].checked) {
        $("#nameH").addClass("displayNone");
        $(".nameR").addClass("displayNone");
    }
    // 2
    if ($("#Checkboxes_DateCheck")[0].checked) {
        $("#dateH").addClass("displayNone");
        $(".dateR").addClass("displayNone");
    }
    // 3
    if ($("#Checkboxes_PriceCheck")[0].checked) {
        $("#priceH").addClass("displayNone");
        $(".priceR").addClass("displayNone");
    }
    // 4
    if ($("#Checkboxes_CurrencyFCheck")[0].checked) {
        $("#currencyFH").addClass("displayNone");
        $(".currencyFR").addClass("displayNone");
    }
    // 5
    if ($("#Checkboxes_RateCheck")[0].checked) {
        $("#rateH").addClass("displayNone");
        $(".rateR").addClass("displayNone");
    }
    // 6
    if ($("#Checkboxes_PriceConCheck")[0].checked) {
        $("#priceCH").addClass("displayNone");
        $(".priceCR").addClass("displayNone");
    }
    // 7
    if ($("#Checkboxes_DateTableCheck")[0].checked) {
        $("#dateTH").addClass("displayNone");
        $(".dateTR").addClass("displayNone");
    }
    // 8
    if ($("#Checkboxes_CurrencyTCheck")[0].checked) {
        $("#currencyTH").addClass("displayNone");
        $(".currencyTR").addClass("displayNone");
    }

    // For change event

    $("#Checkboxes_NameCheck").change(function () {
        if (this.checked) {
            $("#nameH").addClass("displayNone");
            $(".nameR").addClass("displayNone");
        }
        else {
            $("#nameH").removeClass("displayNone");
            $(".nameR").removeClass("displayNone");
        }
        $("#Checkboxes_NameCheck").val(this.checked);
    });

    $("#Checkboxes_DateCheck").change(function () {
        if (this.checked) {
            $("#dateH").addClass("displayNone");
            $(".dateR").addClass("displayNone");
        }
        else {
            $("#dateH").removeClass("displayNone");
            $(".dateR").removeClass("displayNone");
        }
        $("#Checkboxes_DateCheck").val(this.checked);
    });

    $("#Checkboxes_PriceCheck").change(function () {
        if (this.checked) {
            $("#priceH").addClass("displayNone");
            $(".priceR").addClass("displayNone");
        }
        else {
            $("#priceH").removeClass("displayNone");
            $(".priceR").removeClass("displayNone");
        }
        $("#Checkboxes_PriceCheck").val(this.checked);
    });

    $("#Checkboxes_CurrencyFCheck").change(function () {
        if (this.checked) {
            $("#currencyFH").addClass("displayNone");
            $(".currencyFR").addClass("displayNone");
        }
        else {
            $("#currencyFH").removeClass("displayNone");
            $(".currencyFR").removeClass("displayNone");
        }
        $("#Checkboxes_CurrencyFCheck").val(this.checked);
    });

    $("#Checkboxes_RateCheck").change(function () {
        if (this.checked) {
            $("#rateH").addClass("displayNone");
            $(".rateR").addClass("displayNone");
        }
        else {
            $("#rateH").removeClass("displayNone");
            $(".rateR").removeClass("displayNone");
        }
        $("#Checkboxes_RateCheck").val(this.checked);
    });

    $("#Checkboxes_PriceConCheck").change(function () {
        if (this.checked) {
            $("#priceCH").addClass("displayNone");
            $(".priceCR").addClass("displayNone");
        }
        else {
            $("#priceCH").removeClass("displayNone");
            $(".priceCR").removeClass("displayNone");
        }
        $("#Checkboxes_PriceConCheck").val(this.checked);
    });

    $("#Checkboxes_DateTableCheck").change(function () {
        if (this.checked) {
            $("#dateTH").addClass("displayNone");
            $(".dateTR").addClass("displayNone");
        }
        else {
            $("#dateTH").removeClass("displayNone");
            $(".dateTR").removeClass("displayNone");
        }
        $("#Checkboxes_DateTableCheck").val(this.checked);
    });

    $("#Checkboxes_CurrencyTCheck").change(function () {
        if (this.checked) {
            $("#currencyTH").addClass("displayNone");
            $(".currencyTR").addClass("displayNone");
        }
        else {
            $("#currencyTH").removeClass("displayNone");
            $(".currencyTR").removeClass("displayNone");
        }
        $("#Checkboxes_CurrencyTCheck").val(this.checked);
    });


});