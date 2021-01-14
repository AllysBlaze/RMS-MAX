$(function () { 

    $("#show").mousedown(function () {
        $("#pass").attr("type", "text");
    });
    $("#show").mouseup(function () {
        $("#pass").attr("type", "password");
    });
    $("#show").mouseleave(function () {
        $("#pass").attr("type", "password");
    });
});