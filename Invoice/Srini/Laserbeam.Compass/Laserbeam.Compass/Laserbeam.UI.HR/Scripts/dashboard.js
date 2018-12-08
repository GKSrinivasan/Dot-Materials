var showChar = 1000;
var contentCount = 0;
var index = 0;
var totalCount = 0;
var first = 1;
var textContent = "";
var lesstext = "less";
var TaskCount;
var approvalType;
var IsTeam = false;
var selectedCultureCode = "";

$(document).ready(function (e) {
    if ($("#collpase1")[0] != undefined) {
        $("#collpase1").click();
    }
    onSetHeight();

    setTimeout(function () {
        $("#dashBoardBudget").height("");
        $("#dashBoardWorkFlowPanel").height("");
        AnnoucementHeight = $("#dashBoardBudget").height();
        DailyTaskHeight = $("#dashBoardWorkFlowPanel").height();
        if (AnnoucementHeight >= DailyTaskHeight) {
            $("#dashBoardBudget").height(AnnoucementHeight);
            $("#dashBoardWorkFlowPanel").height(AnnoucementHeight);
        }
        else if (AnnoucementHeight <= DailyTaskHeight) {
            $("#dashBoardBudget").height(DailyTaskHeight);
            $("#dashBoardWorkFlowPanel").height(DailyTaskHeight);
        }
    }, 250);
    
});

$(document).on("click", "#teamApprovalTab", function (e) {
    IsTeam = true;
    $("#approvalEmployeedetails").data("kendoAutoComplete").element[0].value = "";
    $("#yet-to tr").css("background-color", "");
});

$(document).on("click", "#managerApprovalTab", function (e) {
    IsTeam = false;
    $("#approvalEmployeedetails").data("kendoAutoComplete").element[0].value = "";
    $("#approved tr").css("background-color", "");
});

$(document).on("click", "#liteamStatus,#liManagerStatus", function (e) {
    setTimeout(function () {
        $("#dashBoardBudget").height("");
        $("#dashBoardWorkFlowPanel").height("");
        AnnoucementHeight = $("#dashBoardBudget").height();
        DailyTaskHeight = $("#dashBoardWorkFlowPanel").height();
        if (AnnoucementHeight >= DailyTaskHeight) {
            $("#dashBoardBudget").height(AnnoucementHeight);
            $("#dashBoardWorkFlowPanel").height(AnnoucementHeight);
        }
        else if (AnnoucementHeight <= DailyTaskHeight) {
            $("#dashBoardBudget").height(DailyTaskHeight);
            $("#dashBoardWorkFlowPanel").height(DailyTaskHeight);
        }
    }, 250);
});

$(document).on("click", "[id$=collpase]", function () {
    onSetHeight();
});

$(document).on("click", "#more", function () {
    $("#lessdiv").show();
    $("#morediv").hide();
});

$(document).on("click", "#less", function () {
    $("#lessdiv").hide();
    $("#morediv").show();
});

$(document).on("click", "#lnkManagerName", function (e) {
    var grdTeamStatus = $("#ManagerBudget").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var rowData = grdTeamStatus._data[rowIndex];
    onSelectNotification(rowData.ManagerNum, 'Compensation');
});

function GetApprovalReporteesParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    return {
        __RequestVerificationToken:token,
        type: approvalType,
    }
}

function onSelect_approvalEmployeeSearch(e) {
    var selectedEmp = e.item[0].innerText;
    var selectedValue = selectedEmp.split('-')[0].trim();
    if (selectedValue != 0 || selectedValue != "") {
        if (IsTeam)
        {
            $("#approved tr").css("background-color", "");
            $("#approved tr[emp-num='" + selectedValue + "']")[0].style.backgroundColor = "#FFEFBC";
            //var container = $("#approved"),
            //scrollTo = $("#teamApproval_" + selectedValue);
            //container.animate({ scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop() });
        }
        else {
            //var WhereToMove = ($("tr[emp-num='" + selectedValue + "']")[0].offsetTop) ;
            //jQuery("#yet-to").animate({ scrollTop: WhereToMove }, 0);
            //$("#yet-to").scrollTop(($("tr[emp-num='" + selectedValue + "']")[0].offsetTop) );
            $("#yet-to tr").css("background-color", "");
            $("#yet-to tr[emp-num='" + selectedValue + "']")[0].style.backgroundColor = "#FFEFBC";
            //var grid = $("#yet-to");
            //var scrollContentOffset = grid.find("tbody").offset().top;
            //var selectContentOffset = grid.find("tr[emp-num='" + selectedValue + "']").offset().top;
            //var distance = selectContentOffset - scrollContentOffset;
            //grid.find("tr[emp-num='" + selectedValue + "']").animate({
            //    scrollTop: distance
            //}, 400);
            //var container = $("#yet-to"),
            //scrollTo = $("#yetTo_" + selectedValue);
            //container.animate({ scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop() });
            //container.scrollTop = scrollTo.offset().top - container.offset().top + container.scrollTop();
            //$("html, body").animate({ scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop() }, "slow");
        }       
    }
}

$(document).on("click", "#approvalClear", function (e) {
    $("#approvalEmployeedetails").data("kendoAutoComplete").element[0].value = "";
    if (IsTeam)
    {
        $("#approved tr").css("background-color", "");
    }
    else {
        $("#yet-to tr").css("background-color", "");
    }
});

function formatValue(value, format, culture) {
    if (value == null) return '';
    if (isNaN(value)) return '';
    var symbol = kendo.cultures[culture];
    symbol = (symbol != undefined) ? symbol.numberFormat.currency.symbol : kendo.cultures['en-US'].numberFormat.currency.symbol;
    var currenyCode = dashBoardConstants.currencyCodeFormat;
    var userCurrencyFormat = dashBoardConstants.currencyCode;
    if ((value.length == 1) || format == undefined)
        return value.toString();
    else if (culture == undefined) {
        var val = kendo.toString(value, format);
        val = val.indexOf('-') > -1 ? '(' + val.replace('-', '') + ')' : val;
        if (userCurrencyFormat == "True")
            val = val.replace(symbol, currenyCode + ' ');
        return val;
    }
    else {
        var kndValue = kendo.toString(value, format, culture);
        kndValue = kndValue.indexOf('-') > -1 ? '(' + kndValue.replace('-', '') + ')' : kndValue;
        if (userCurrencyFormat == "True")
            kndValue = kndValue.replace(symbol, currenyCode + ' ');
        return kndValue;
    }
}

function gridFormat(value) {
    var format = "c0";
    var culture = selectedCultureCode;
    if (value == null) return '';
    if (isNaN(value)) return '';
    var symbol = kendo.cultures[culture];
    symbol = (symbol != undefined) ? symbol.numberFormat.currency.symbol : kendo.cultures['en-US'].numberFormat.currency.symbol;
    if ((value.length == 1) || format == undefined)
        return value.toString();
    else if (culture == undefined) {
        var val = kendo.toString(value, format);
        val = val.indexOf('-') > -1 ? '(' + val.replace('-', '') + ')' : val;       
        return val;
    }
    else {
        var kndValue = kendo.toString(value, format, culture);
        kndValue = kndValue.indexOf('-') > -1 ? '(' + kndValue.replace('-', '') + ')' : kndValue;        
        return kndValue;
    }
}

function messagebody(messageSubject, message) {

    var MessageSubject = document.getElementById("MessageSubject");
    var MessageSubjecttext = messageSubject;
    var MessageSubjecttxt = document.createTextNode(MessageSubjecttext);
    MessageSubject.innerText = MessageSubjecttxt.textContent;
    var Message = document.getElementById("MessageBody");
    var Messagetext = message;
    var Messagetxt = document.createTextNode(Messagetext);
    Message.innerHTML = Messagetxt.textContent;
    if (Message.innerHTML.length > showChar) {
        var c = Message.innerHTML.substr(0, showChar);
        Message.innerHTML = c;
        $("#more").show();
        $("#less").show();
    }
    else {
        $("#more").hide();
        $("#less").hide();
        Message.innerHTML = Messagetxt.textContent;
    }
    var Message = document.getElementById("MessageBody1");
    var Messagetext = message;
    var Messagetxt = document.createTextNode(Messagetext);
    Message.innerHTML = Messagetxt.textContent;
    $("#lessdiv").hide();
    $("#morediv").show();
}



/* 
 * Added the javascript functions for My dashboard
 * 
 */

function onDailyTaskToggle() {
    $("#dailyTaskViewDiv").animate({ scrollTop: 1 }, "slow");
    $("#TaskContent").html("");
    $("#charCount").val("");
    $("#TaskAction").val("Add");
    if (TaskCount == 0)
    { $("#noNoteImg").hide(); }
    
    //$("#idTaskActionBtn").html("Submit");
    onSetHeight();
}
function onCancelTask() {
    $("#TaskContent")[0].value = "";
    $("#idTaskToggle").click();
    if(TaskCount==0)
    { $("#noNoteImg").show(); }
}
function onSuccessAddTask(ResponseText) {
    $("#idDailyTaskDiv").html(ResponseText);
    onSetHeight();
}

function onEditDailyTask(TaskID) {
    if ($("#idTaskToggle").attr("aria-expanded") == false || $("#idTaskToggle").attr("aria-expanded") == "" || $("#idTaskToggle").attr("aria-expanded") == undefined || $("#idTaskToggle").attr("aria-expanded") == "false") {
        $("#idTaskToggle").click();
    }
    var WhereToMove = jQuery("#DailyTaskDiv" + TaskID)[0].scrollHeight + $("#ActionItems").height();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Dashboard/_GetEditDailyTask',
        type: 'POST',
        data: { __RequestVerificationToken:token, TaskID: TaskID },
        success: function (ResponseText) {
            $("#TaskContent").html(ResponseText);
            $("#TaskContent")[0].value = ResponseText;
            $("#TaskAction").val("Edit");
            $("#TaskID").val(TaskID);
            setMaximumLimit();
            onSetHeight();
            jQuery("html,body").animate({ scrollTop: WhereToMove }, 0);
        },
        error: function (e) {
        }
    });
}

function onDeleteCompletedDailyTask(TaskID)
{
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Dashboard/_DeleteDailyTask',
        type: 'POST',
        data: { __RequestVerificationToken:token, TaskID: TaskID },
        success: function (ResponseText) {
            $("#DailyTaskDiv" + TaskID).remove();
            onSetHeight();
            if (ResponseText == "0") {
                $("#showCompletedTaskIcon").hide();
                $("#hideCompletedTaskIcon").hide();
            }
        },
        error: function (e) {
        }
    });
}

function onDeleteDailyTask(TaskID) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Dashboard/_DeleteDailyTask',
        type: 'POST',
        data: { __RequestVerificationToken:token, TaskID: TaskID },
        success: function (ResponseText) {
            $("#DailyTaskDiv" + TaskID).remove();
            onSetHeight();
            onShowLessTask();
            if(ResponseText=="0")
            {
                $("#showCompletedTaskIcon").hide();
                $("#hideCompletedTaskIcon").hide();
            }
        },
        error: function (e) {
        }
    });
}

function onSetHeight() {
    $(window).unbind('scroll');
    setTimeout(function () {
        $("#idAnnouncementPanel").height("");
        $("#idDailyTaskPanel").height("");
        AnnoucementHeight = $("#idAnnouncementPanel").height();
        DailyTaskHeight = $("#idDailyTaskPanel").height();
        if (AnnoucementHeight >= DailyTaskHeight) {
            $("#idAnnouncementPanel").height(AnnoucementHeight);
            $("#idDailyTaskPanel").height(AnnoucementHeight);
        }
        else if (AnnoucementHeight <= DailyTaskHeight) {
            $("#idAnnouncementPanel").height(DailyTaskHeight);
            $("#idDailyTaskPanel").height(DailyTaskHeight);
        }
    }, 300);
}

function onSetCompletedTask(TaskID) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Dashboard/_SetDailyTaskComplete',
        type: 'POST',
        data: { __RequestVerificationToken:token, TaskID: TaskID },
        success: function (ResponseText) {
            $("#DailyTaskDiv" + TaskID).remove();
            if ($("#idCompleteTask")[0].outerText != "")
                refreshCompletedTask();
            else {
                $("#showCompletedTaskIcon").show();
                onSetHeight();
            }
            onShowLessTask();
        },
        error: function (e) {
        }
    });
}

function refreshCompletedTask() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Dashboard/_CompletedDailyTask',
        type: 'POST',
        data:{__RequestVerificationToken:token},
        success: function (ResponseText) {
            if ($.trim(ResponseText) != "") {
                $("#idCompleteTask").html(ResponseText);
                $("#hideCompletedTaskIcon").show();
                $("#showCompletedTaskIcon").hide();
                onSetHeight();
            }
        },
        error: function (e) {
        }
    });
}

function onShowCompletedTask() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Dashboard/_CompletedDailyTask',
        type: 'POST',
        data: {__RequestVerificationToken:token},
        success: function (ResponseText) {
            if ($.trim(ResponseText) != "") {
                $("#idCompleteTask").html(ResponseText);
                $("#hideCompletedTaskIcon").show();
                $("#showCompletedTaskIcon").hide();
                onSetHeight();
            }
        },
        error: function (e) {
        }
    });
}

function onHideCompletedTask() {
    $("#hideCompletedTaskIcon").hide();
    $("#showCompletedTaskIcon").show();
    $("#idCompleteTask").html("");
    onSetHeight();

}

function onShowAllTask() {
    $.ajax({
        url: '../Dashboard/_ShowAllTask',
        type: 'GET',
        cache: false,
        success: function (ResponseText) {
            $("#idDailyTaskDiv").html(ResponseText);
            $("#idTAskShowLess").show();
            $("#idTAskShowMore").hide();
            onSetHeight();
        },
        error: function (e) {
        }
    });
}

function onShowAllAnnouncement() {
    $.ajax({
        url: '../Dashboard/_ShowAllAnnouncements',
        type: 'get',
        success: function (ResponseText) {
            $("#idAnnouncementDiv").html(ResponseText);
            $("#idAnnouncementShowLess").show();
            $("#idAnnouncementShowMore").hide();
            onSetHeight();
        },
        error: function (e) {
        }
    });
}

function onShowLessTask() {
    $.ajax({
        url: '../Dashboard/_DailyTask',
        type: 'GET',
        cache: false,
        success: function (ResponseText) {
            $("#idDailyTaskDiv").html(ResponseText);
            onSetHeight();
        },
        error: function (e) {
        }
    });
}

function onShowLessAnnouncement() {
    $.ajax({
        url: '../Dashboard/_Announcements',
        type: 'GET',
        cache: false,
        success: function (ResponseText) {
            $("#idAnnouncementDiv").html(ResponseText);
            onSetHeight();
        },
        error: function (e) {
        }
    });
}

function setMaximumLimit() {
    var textLength = $("#TaskContent").val();
    $("#charCount").val(textLength.length);
}

$(document).on("click", "#btnSendEmail", function (e) {

    $.ajax({
        url: "../Dashboard/_SendEmail",
        type: "get",
        async: true,

        success: function (result) {
            $("#divSendEmail").html(result);
            $("#divSendEmail").modal('show');
        }
    });
});

$(document).on("click", "#chkBoxSelectAll", function (e) {
    if ($(this).is(":checked") == true) {
        kendoCheckAllStatus = true
        $("input[name='chkValue']").each(function () {
            $(this).prop("checked", true);
        });
    }
    else {
        kendoCheckAllStatus == false;
        $("input [name='chkValue']").each(function () {
            $(this).prop("checked", false);
        });
    }
});

$(document).on("click", "#meritexport", function (e) {
    var form = $("<form action='../Dashboard/MeritExport' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});

$(document).on("click", "#promotionExport", function (e) {
    var form = $("<form action='../Dashboard/PromotionExport' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});

$(document).on("click", "#adjmntExport", function (e) {
    var form = $("<form action='../Dashboard/AdjustmentExport' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});


function cancelSendEmailReminder(e) {
    if (!showSaveWarning(event, "emailChangeFlag")) return false;
    var sendReminderGrid = $("#grdSendReminderNotification").data("kendoGrid");
    sendReminderGrid.dataSource.filter({});
    sendReminderGrid.dataSource.sort({});
    sendReminderGrid.refresh();
    $("#chkBoxSelectAll").removeAttr('checked');
    emailChangeFlag = false;
    $("#divSendEmail").modal('hide');
}



function sendYetSubmitSendEmailReminder(managerNum) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var mailSubj = $("#mailSubj").val();
    var mailContent = $("#mailContent").val();
    if (mailSubj != "" && mailContent != "") {
        $.ajax({
            url: '../DashBoard/SendYetToSubmitRemainder',
            data: { __RequestVerificationToken:token, mailSubject: mailSubj, mailContent: mailContent, managerNum: managerNum },
            type: "POST",
            success: function (result) {
                $("#sendRemainderDiv").modal('hide');
            }
        });
    }
    else {
        showAlert("Please enter subject and content");
    }
}
// Email Part

function additionalParamInfo() {
    return {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
    }
}
function sendReminderBound() {
    var grdUserDetails = $("#grdSendReminderNotification").data("kendoGrid");
    if (grdUserDetails.dataSource._data.length == 0) {
        var colCount = $("#grdSendReminderNotification").find('th').length;
        $("#grdSendReminderNotification").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center;background-color:#6686C4;padding-top:30px"></td></tr>');
        $("#grdSendReminderNotification").find('.k-grid-content tbody tr td').css("padding-top", 11).append('<b style="background-color:#6686C4 !important;color:white !important;">No Results Found!</b>');
    }
}
$(document).on('click', '#sendReminderFilter', function () {
    $("body").css("overflow", "hidden");
    $.ajax({
        url: '../UserManagement/_SendEmail_FilterSortPopup',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        success: function (result) {
            $("#EmailFilterSortPopup").html(result);
            var wndFilterSort = $("#EmailFilterSortPopup").data("kendoWindow");
            wndFilterSort.options.gridName = "grdSendReminderNotification";
            wndFilterSort.center().open();
            $("#filterseperator").css("display", "inline");
            $("#sendReminderClearFilter").show();
        }
    });
});
$(document).on("show.bs.modal", "#divSendEmail", function (e) {
    $("#sendReminderClearFilter").hide();
});
$(document).on("click", "#sendEmailReminder", function (event) {
    var a = $("#emailReminderEditor").val();
    var b = $("#emailReminderSubject").val();
    var C = $("#EmailBody").val();

    var grid = $("#grdSendReminderNotification").data("kendoGrid");
    var values = [];
    var postData = [];
    var rowIndexArray = [];
    $("#chkBox input[type=checkbox]:checked").each(function () {
        var rowItem = $(this).closest("tr");
        var rowIndex = $(rowItem).index();
        var rowData = grid._data[rowIndex];
        row = $(this).closest("tr");
        values.push(rowData);
        rowIndexArray.push(rowIndex);

    });
    var emailBody = $("#emailReminderEditor").val();
    var emailSubject = $("#emailReminderSubject").val();
    if (values.length == 0) {
        alert('Select atleast one user');
    }
    else {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var emailPostData = JSON.stringify(values);
        var emailData = JSON.parse(emailPostData);
        $.ajax({
            url: "../Dashboard/SendEmailReminderToUser",
            type: "post",
            data: { __RequestVerificationToken: token, sendReminderMail: emailData, emailBody: $("#emailReminderEditor").val(), emailSubject: $("#emailReminderSubject").val() },
            dataType: 'html',
            success: function (data) {
                emailChangeFlag = false;
                if (data == 1) {
                    $("#divSendEmail").html(data);
                    $("#divSendEmail").modal('hide');
                    Successmessage("Sent Mail Successfully");
                }
                $("#chkBoxSelectAll").removeAttr('checked');
            }

        });
    }
});
function cancelSendEmailReminder(e) {
    var sendReminderGrid = $("#grdSendReminderNotification").data("kendoGrid");
    sendReminderGrid.dataSource.filter({});
    sendReminderGrid.dataSource.sort({});
    sendReminderGrid.refresh();
    $("#chkBoxSelectAll").removeAttr('checked');
    emailChangeFlag = false;
    $("#divSendEmail").modal('hide');
}
$(document).on("click", "[id$=chkBox]", function (e) {

    var bool = ($('input#chkBox:checked').length == $("input#chkBox").length) ? true : false
    $("#chkBoxSelectAll")[0].checked = bool;
    emailChangeFlag = true;
});
$(document).on("click", "#chkBoxSelectAll", function (e) {
    if (this.checked) {
        $("#chkBox input[type=checkbox]").each(function () {
            this.checked = true;
        });
    }
    else {
        $("#chkBox input[type=checkbox]").each(function () {
            this.checked = false;
        });
    }
});
/////////////


