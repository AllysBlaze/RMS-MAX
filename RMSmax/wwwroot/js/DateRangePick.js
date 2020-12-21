window.addEventListener('load',
    function () {
        var n = document.querySelector("#btnCheck");
        n.addEventListener('click', checkCondition, true);
    }
);


function checkCondition () {
    var date1 = document.getElementById("date1");
    var date2 = document.getElementById("date2");

    console.log("Działa");
    console.log(date1);
    console.log(date1.nodeValue);
    console.log(date1.value);
    //jak warunek jest zly to zablokowac szukanie ! button- disabled.
}