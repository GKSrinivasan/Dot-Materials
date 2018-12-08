
$('body').on('click', function (e) {
    $('[data-toggle="Notificationpopover1"]').each(function () {
        if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
            $(this).popover('hide');
        }
    });
});

function refreshNotification() {
    $.ajax({
        url: "../Dashboard/_NotificationPanel",
        type: "get",
        success: function (result) {
            $("#notificationPopoverContent").html(result);
            if ($("#hdnApprovalCount")[0].value == 0) {
                if ($("#spnBadge")[0] != undefined) {
                    $("#spnBadge").hide();
                    $("#bell").addClass("disabled");
                    $("#bell").unwrap();
                }
            }
            else if ($("#hdnApprovalCount")[0].value > 0) {
                if ($("#spnBadge")[0] != undefined) {
                    $("#spnBadge")[0].innerHTML = $("#hdnApprovalCount")[0].value;
                    $("#bell").removeClass("disabled");
                }
            }
        }
    });
}

function onSelectNotification(notifiedEmployeeNum, modulekey) {
    $('[data-toggle="Notificationpopover1"]').popover('hide');
    var i = 1;
    var token = $('input[name="__RequestVerificationToken"]').val();
  
    var form = $("<form action='" + '../' + modulekey + '/' + 'Home' + "' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + token + "' />");
    form.append("<input type='text' name='notifiedEmployeeNum' value='" + notifiedEmployeeNum + "' />");
    form.append("<input type='text' name='isMyApproval' value='" + i + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}
$(document).click(function (e) {
    var loginContentArea = $(e.target).parents("#login-content");
    if ((e.target.innerHTML).trim() != "Log Out") {
        if ((e.target.innerHTML).trim() != "Profile")
            if (loginContentArea.length)
                return false;
    }
    var loginSlideToggleArea = $(e.target).parents("#loginSlideToggle");
    if(e.target.id == "loginSlideToggle" && $("#login-content").css("display") != "none")
    {
        $("#login-content").hide();
    }
    else if (e.target.id == "loginSlideToggle" || loginSlideToggleArea.length != 0) {
        $("#login-content").show();
    }    
    else {
        $("#login-content").hide();
    }
});

$(document).ready(function () {
    $.ajaxSetup({ cache: false });
});
