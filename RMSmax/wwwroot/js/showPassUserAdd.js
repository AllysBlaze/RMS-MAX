$(function () {

    $("#show1").mousedown(function () {
        $("#new").attr("type", "text");
    });
    $("#show1").mouseup(function () {
        $("#new").attr("type", "password");
    });
    $("#show1").mouseleave(function () {
        $("#new").attr("type", "password");
    });

    $("#show2").mousedown(function () {
        $("#new2").attr("type", "text");
    });
    $("#show2").mouseup(function () {
        $("#new2").attr("type", "password");
    });
    $("#show2").mouseleave(function () {
        $("#new2").attr("type", "password");
    });
});