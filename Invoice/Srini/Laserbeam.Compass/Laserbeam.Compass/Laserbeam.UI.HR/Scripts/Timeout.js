
var sec = 59;   //60 set the seconds
var min = 29;   //29 set the minutes
function refresh() {
    sec = 59;
    min = 29;
}

function countDown() {
    sec--;
    if (sec == -01) {
        sec = 59;
        if (min>0)
        min = min - 1;
    }
    else {
        min = min;
    }
    if (sec <= 9) { sec = "0" + sec; }
    time = (min <= 9 ? "0" + min : min) + ":" + sec + "";
    if (document.getElementById('theTime')) { document.getElementById('theTime').innerHTML = "Your session will be logged out in " + time; }
    if (document.getElementById('divCountDown')) {
        var divCountDown = document.getElementById('divCountDown');
        divCountDown.innerHTML = time;
        if (min == '09' && sec == '59') {
            OpenModelPopup();
        }
        if (sec <= 10) {
            divCountDown.style.color = '#ff0000';
        }

        SD = window.setTimeout("countDown();", 1000);
        if (min == '00' && sec == '00') {
            window.location = "../Account/SSOLogIn";
        }
    }

}
window.onload = countDown;

function CloseModelPopup() {
    $("#timeoutPopup").modal('hide');
}


function OpenModelPopup() {
    $("#timeoutPopup").modal('show');
    $(".modal-backdrop").addClass("removeZIndex");
}

function refreshPage() {
    var divCountDown = document.getElementById('divCountDown');
    divCountDown.style.color = '#000000';
    CloseModelPopup();
    refresh();
    window.location = window.location;
    return false;
}


