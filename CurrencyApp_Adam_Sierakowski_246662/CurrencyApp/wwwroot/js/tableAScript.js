
$(function () {
    $("#mainOnesCK").val(this.checked);
    $("#mainOnesCK").change(function () {
        if (this.checked) {
            $(".rate").not(".mainOne").addClass("displayNone");
        }
        else {
            $(".rate").not(".mainOne").removeClass("displayNone");
        }
        $("#mainOnesCK").val(this.checked);
    });

    $("#btnChangeView").click(function () {
        if ($("#mainOnesCK")[0].checked){
            $("#mainOnesCK").prop("checked", false).change();
        }
        else {
            $("#mainOnesCK").prop('checked', true).change();
        }
    });

});