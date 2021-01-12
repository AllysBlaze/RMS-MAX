$(function () { 
                $("#show1").mousedown(function () {
                $("#old").attr("type", "text");
                });

                $("#show1").mouseup(function () {
                $("#old").attr("type", "password");
                });

                $("#show2").mousedown(function () {
                $("#new").attr("type", "text");
                });
                $("#show2").mouseup(function () {
                $("#new").attr("type", "password");
                });

                $("#show3").mousedown(function () {
                $("#new2").attr("type", "text");
                });
                $("#show3").mouseup(function () {
                $("#new2").attr("type", "password");
                });
});