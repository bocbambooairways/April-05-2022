$(function () {
    let isMobile = window.matchMedia("only screen and (max-width: 760px)").matches;
    if (isMobile) {
        $("#TypeOfDevice").val("Mobile");
    }
    else {
        $("#TypeOfDevice").val("DeskTop");
    }
});