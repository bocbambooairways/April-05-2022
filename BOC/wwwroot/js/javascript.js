    $("#login").click(function () {
        if ($("#username").val() == "" ||
            $("#password").val() == "")
    {
    $('#myModal').modal('show');
    return false;
    }
    });
document.getElementById("remember").disabled = true;
