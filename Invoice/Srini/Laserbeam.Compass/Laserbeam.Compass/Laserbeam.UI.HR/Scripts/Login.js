$(document).on("click", "#lnkAccountLoginForgotPassword", function (e) {
    var userId = $("#UserID").val();
    $("#hndForgotPasswordUserId").val(userId);
    $("#forgotPassword").submit();
});


// Author         : Karthikeyan Shanmugam
// Modified Date  : 21-Feb-2017
// Reviewed By    : Hariharasubramaniyan
// Reviewed Date  : 21-Feb-2017
// comment        : keyup and popover function for password strength color changes on text and strength progress bar

$(document).on("keyup", "#txtpassword", function (e) {
    $('#txtpassword').popover({
        html: true,
        placement: 'bottom',
        trigger: "manual",
        content: function () {
            return $('#popover_content_wrapper').html();
        }
    });
    // comment :To show and hide the popover text based on the textbox value
    if ($("#txtpassword").val().length > 0) {
        if (!$("#txtpassword").data("bs.popover").tip().hasClass("in"))
            $("#txtpassword").popover('show');
    }
    else {
        $("#txtpassword").popover('hide');
    } 
    var strength = check_password();    
});

$(document).on("keydown", "#txtpassword", function (e) {
    if (e.keyCode == "9") {
        $("#txtpassword").popover('hide');
    }
});
$(document).on("focusout", "#txtpassword", function (e) {
        $("#txtpassword").popover('hide');
});

$(document).on("keydown", "#password1", function (e) {
    if (e.keyCode == 13) {
        $("#btnResetPassword").trigger("click");
    }
});

$(document).on("click", "#btnResetPassword", function (e) {
    var strength = check_password();
    if (strength != 100){        
        $('#txtpassword').popover({
            html: true,
            placement: 'bottom',
            trigger: "manual",
            content: function () {
                return $('#popover_content_wrapper').html();
            }
        });
        // comment :To show and hide the popover text based on the textbox value
        if ($("#txtpassword").val().length > 0) {
            if (!$("#txtpassword").data("bs.popover").tip().hasClass("in"))
                $("#txtpassword").popover('show');
        }
        else {
            $("#txtpassword").popover('hide');
        }
        check_password();
        e.preventDefault();
    }
        
    else
        $("#form1").submit();
});


$(document).on("keyup", "#btnResetPassword", function (e) {    
    var strength = check_password();
    if (strength == 100 && e.keyCode == "13")
        $("#form1").submit();        
    else
    {   
        $('#txtpassword').popover({
            html: true,
            placement: 'bottom',
            trigger: "manual",
            content: function () {
                return $('#popover_content_wrapper').html();
            }
        });
        // comment :To show and hide the popover text based on the textbox value
        if ($("#txtpassword").val().length > 0) {
            if (!$("#txtpassword").data("bs.popover").tip().hasClass("in"))
                $("#txtpassword").popover('show');
        }
        else {
            $("#txtpassword").popover('hide');
        }
        check_password();
        e.preventDefault();
    }
        
});

function isInArray(value, array) {
    return array.indexOf(value) > -1;
}

function check_password() {
    var strength = 0;
    var val = $("#txtpassword").val();
    if (val != "") {
        // comment : If the password length is greater than or equal to 12
        if (val.length >= $("#txtPasswordLength").val()) {
            strength += 25;
            $("#numbers").css('color', '#ec7063');
            $("#textnumbers").css('color', '#ec7063');
        }
        else {
            $("#numbers").css('color', '#555');
            $("#textnumbers").css('color', '#555');
        }
        // comment : If the password match with one uppercase and lowercase character
        if ((val.match(/[a-z]/)) && (val.match(/[A-Z]/))) {
            strength += 25;
            $("#character").css('color', '#ec7063');
            $("#textcharacter").css('color', '#ec7063');
        }
        else {
            $("#character").css('color', '#555');
            $("#textcharacter").css('color', '#555');
        }
        // comment : If the password match with one number
        if (val.match(/[0-9]/)) {
            strength += 25;
            $("#onenumber").css('color', '#ec7063');
            $("#textonenumber").css('color', '#ec7063');
        }
        else {
            $("#onenumber").css('color', '#555');
            $("#textonenumber").css('color', '#555');
        }
        // comment : If the password match with  Special character
        if (val.match(/([!,@,%,&,#,$,^,*,?,_,~])/)) {
            strength += 25;
            $("#specialchar").css('color', '#ec7063');
            $("#textspecialchar").css('color', '#ec7063');
        }
        else {
            $("#specialchar").css('color', '#555');
            $("#textspecialchar").css('color', '#555');
        }

        $("#strengthbar").css('width', strength + "%");
    }
    else {
        // comment : If the password length is not equal or less than to 12
        $("#numbers").css('color', '#555');
        $("#textnumbers").css('color', '#555');
        // comment :If the password not match with one uppercase and lowercase character
        $("#character").css('color', '#555');
        $("#textcharacter").css('color', '#555');
        // comment : If the password not match with one number
        $("#onenumber").css('color', '#555');
        $("#textonenumber").css('color', '#555');
        // comment : If the password not match with  Special character
        $("#specialchar").css('color', '#555');
        $("#textspecialchar").css('color', '#555');

        $("#strengthbar").css('width', strength + "%");
    }
    // To change color of progress bar based on strength width
    if (strength > 0) {
        if (strength == 25)
        { $("#strengthbar").css('background', '#f5382f'); }
        else if (strength == 50)
        { $("#strengthbar").css('background', '#f09025 '); }
        else if (strength == 100)
        { $("#strengthbar").css('background', '#83ce40'); }
    }
    return strength;
}
