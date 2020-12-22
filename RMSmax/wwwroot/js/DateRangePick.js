
function checkCondition() {
    var date1 = document.getElementById("date1");
    var date2 = document.getElementById("date2");
    var submitBtn = document.querySelector("#submitBtn");
    
    var o1 = new Date(date1.value);
    var d1 = o1.getTime();
    var o2 = new Date(date2.value);
    var  d2 = o2.getTime();


    if (d1 <= d2 || !d1 && !(!d2) || !d2 && !(!d1)) {
        submitBtn.disabled = false;
    } else {
        submitBtn.disabled = true;
    }


    
    

}