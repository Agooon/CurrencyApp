﻿
$(function () {

    $("#NameCheck").change(function () {
        if (this.checked) {
            $("#nameH").addClass("displayNone");
            $(".nameR").addClass("displayNone");
        }
        else {
            $("#nameH").removeClass("displayNone");
            $(".nameR").removeClass("displayNone");
        }
        $("#NameCheck").val(this.checked);
    });

    $("#DateCheck").change(function () {
        if (this.checked) {
            $("#dateH").addClass("displayNone");
            $(".dateR").addClass("displayNone");
        }
        else {
            $("#dateH").removeClass("displayNone");
            $(".dateR").removeClass("displayNone");
        }
        $("#DateCheck").val(this.checked);
    });

    $("#PriceCheck").change(function () {
        if (this.checked) {
            $("#priceH").addClass("displayNone");
            $(".priceR").addClass("displayNone");
        }
        else {
            $("#priceH").removeClass("displayNone");
            $(".priceR").removeClass("displayNone");
        }
        $("#PriceCheck").val(this.checked);
    });

    $("#CurrencyFCheck").change(function () {
        if (this.checked) {
            $("#currencyFH").addClass("displayNone");
            $(".currencyFR").addClass("displayNone");
        }
        else {
            $("#currencyFH").removeClass("displayNone");
            $(".currencyFR").removeClass("displayNone");
        }
        $("#CurrencyFCheck").val(this.checked);
    });

    $("#RateCheck").change(function () {
        if (this.checked) {
            $("#rateH").addClass("displayNone");
            $(".rateR").addClass("displayNone");
        }
        else {
            $("#rateH").removeClass("displayNone");
            $(".rateR").removeClass("displayNone");
        }
        $("#RateCheck").val(this.checked);
    });

    $("#PriceConCheck").change(function () {
        if (this.checked) {
            $("#priceCH").addClass("displayNone");
            $(".priceCR").addClass("displayNone");
        }
        else {
            $("#priceCH").removeClass("displayNone");
            $(".priceCR").removeClass("displayNone");
        }
        $("#PriceConCheck").val(this.checked);
    });

    $("#DateTableCheck").change(function () {
        if (this.checked) {
            $("#dateTH").addClass("displayNone");
            $(".dateTR").addClass("displayNone");
        }
        else {
            $("#dateTH").removeClass("displayNone");
            $(".dateTR").removeClass("displayNone");
        }
        $("#DateTableCheck").val(this.checked);
    });

    $("#CurrencyTCheck").change(function () {
        if (this.checked) {
            $("#currencyTH").addClass("displayNone");
            $(".currencyTR").addClass("displayNone");
        }
        else {
            $("#currencyTH").removeClass("displayNone");
            $(".currencyTR").removeClass("displayNone");
        }
        $("#CurrencyTCheck").val(this.checked);
    });


});