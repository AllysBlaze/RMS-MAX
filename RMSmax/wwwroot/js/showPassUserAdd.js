$(function () { 
                $("#show1").mousedown(function () {
                    $("#new").attr("type", "text");
                });

                $("#show1").mouseup(function () {
                    $("#new").attr("type", "password");
                });

                $("#show2").mousedown(function () {
                $("#new2").attr("type", "text");
                });
                $("#show2").mouseup(function () {
                $("#new2").attr("type", "password");
                });
});