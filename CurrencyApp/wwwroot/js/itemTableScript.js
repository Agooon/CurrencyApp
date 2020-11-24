
$(function () {
    $("#NameCheck").val(this.checked);
    $("#NameCheck").change(function () {
        console.log("zmiana!");
        if (this.checked) {
            $("#nameH").addClass("displayNone");
            $(".nameR").addClass("displayNone");
        }
        else {
            $("#nameH").removeClass("displayNone");
            $(".nameR").removeClass("displayNone");
        }
        $("#NameCheck").val(this.checked);
        //var url = "/Items/SetCheckboxes";
        //$.ajax({
        //    url: url,
        //    type: "POST",
        //    data: { checkBoxes: checkBoxes },
        //    beforeSend: function (xhr) {
        //        xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        //    }
        //}).done(function (result) {

        //});
    })

});