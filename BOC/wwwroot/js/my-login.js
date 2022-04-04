$(function () {
    if ($("#error").val() !== "Model.ErrorMessage" && $("#error").val() !== "") {
        alertify.alert($("#error").val());

    }
    if ($("#result").val() !== "") {
        $('#password').val("");

    }
});