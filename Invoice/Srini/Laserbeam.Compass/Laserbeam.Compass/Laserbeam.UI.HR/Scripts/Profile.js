var uploader;
var lastEditedCommentNum = 0;
$(document).ready(function () {
    uploader = new plupload.Uploader({
        runtimes: 'flash,html5',
        drop_element: 'dropContent',
        browse_button: 'browseBtn',
        url: "../UpdateUploadedImage",
        chunk_size: '1mb',
       // multipart: true,
        filters: {
            mime_types: [
              { title: "Image files", extensions: "jpg,png" },
            ],
            max_file_size: "1mb",
            prevent_duplicates: true
        },

        views: {
            list: true,
            thumbs: true, // Show thumbs
            active: 'thumbs'
        },

        dragdrop: true,

        init: {

            FilesAdded: function (up, files) {
                $("#dropContent").removeAttr("border", "1px dashed red");
                $("#dropContent").css("border", "1px dashed #00abf0");
                var file_count = up.files.length;
                for (i = 0; i < file_count - 1; i++) {
                    up.removeFile(up.files[i]);
                }
                plupload.each(files, function (file) {
                    $('#divFileArea').children().remove();
                    var img = new o.Image();
                    img.onload = function () {
                        var div = document.createElement('div');
                        div.id = this.uid;
                        document.getElementById('divFileArea').appendChild(div);
                        this.embed(div.id, {
                            width: 250,
                            height: 63,
                        });
                    };
                    img.load(file.getSource());
                });

            },
            BeforeUpload: function (up, file) {
                uploader.settings.multipart_params = {__RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() };
            },
            FileUploaded: function (up, file, result) {
                if (file.length != 0) {
                    Successmessage("Saved Successfully");
                    setTimeout(location.reload.bind(location), 2500);
                }
            },
            UploadComplete: function (up, file) {
            },
            Error: function (up, err) {

            }
        }
    });
    uploader.init();
});

$(document).on("click", "#btnAddTask", function (e) {
    $.ajax({
        url: '../Profile/_AddTask',
        type: "Get",
        async: false,
        contentType: "application/json",
        processData: false,
        success: function (message) {
            $("#divAddtask").html(message);
            $("#divAddtask").modal('show');
        }
    });

});



function getNumber(value) {
    value = getNumberValue(value, 'en-US');
    var outputValues = ((isNaN(value)) || (value == null) || (value == undefined)) ? 0 : value;
    return outputValues;
}
function grdTaskDataBound() {
    var grdMeritGrid = $("#TaskData").data("kendoGrid");
    $("#taskCount").html(grdMeritGrid.dataSource._total)
    if ((grdMeritGrid._data.length) > 0) {
        var a = grdMeritGrid._data[0].TotalSpent
        var time = a.split(':');
        var hours = time[0];
        var minutes = time[1];
        var remainHrs = hoursSpent - hours;
        var remainMins = Math.abs(60 - minutes);
        if (minutes > 0)
            remainHrs = remainHrs - 1;
        if (minutes == 0)
            remainMins = 0;
        var oldSpentPct = ($("#hoursSpentPct")[0].innerHTML).replace('%', "");
        var hrsSpentPct = ((getNumber(hours + "." + minutes) / hoursSpent) * 100).toFixed(0);
        $("#totalHrsSpent").html(hours + ":" + minutes);
        $("#availableHours").html(remainHrs + ":" + remainMins);
        $("#hoursSpentPct").html(hrsSpentPct + "%")
        if (remainHrs <= 0 && remainMins <= 0)
            $("#btnAddTask").attr("disabled", true)
        else
            $("#btnAddTask").attr("disabled", false)
        var SpentPct = (Number(hrsSpentPct) > 100) ? 100 : hrsSpentPct
        $("#chart").removeClass("p" + oldSpentPct);
        $("#chart").addClass("p" + SpentPct);
    }
}

$(document).on("change", "#spentHoursTxt", function (e) {
    var totalSpent = $("#totalHrsSpent")[0].innerHTML;
    var totalSpentHrs = totalSpent.split(':');
    var value = $("#spentHoursTxt").val();
    var string = value;
    strx = string.split(':');
    array = [];
    array = array.concat(strx);
    var Hrs = $("#availableHours")[0].innerHTML;
    var remainHrs = Hrs.split(':')
    if (value == "") {
        $("#error_hrsEg").html("SpentHours is missing");
    }
    else if (!(/^[0-9]{1,2}:[0-9]{1,2}$/.test(value))) {
        $("#error_hrsEg").html('');
        $("#error_hrsEg").html("Invalid time format");
        return false;
    }
    else if(Number(remainHrs[0])==0 && remainHrs[1]==undefined)
    {
        return false;
    }

    else if (((Number(totalSpentHrs[0]) + Number(array[0])) > hoursSpent) || (((Number(totalSpentHrs[0]) + Number(array[0])) == hoursSpent) && ((Number(totalSpentHrs[1]) + Number(array[1])) > 0)))
{
        $("#error_hrsEg").html('');
        $("#error_hrsEg").html("Spent hours is exceed");
        return false;
}
    //else if ((Number(remainHrs[0]) < Number(array[0]))||((Number(remainHrs[0]) <= Number(array[0]))&&(Number(remainHrs[1]) <= Number(array[1])))) {
    //    $("#error_hrsEg").html('');
    //    $("#error_hrsEg").html("Spent hours is exceed");
    //    return false;
    //}
    else {
        $("#error_hrsEg").html("");
    }
});

$(document).on("click", "#btnWorkFlowExport", function () {
    var moduleNum = defaultProcessNum;
    var form = $("<form action='../Workflow/GetExportGridData' method='post'></form>");
    form.append("<input type='text' name='moduleNum' value='" + moduleNum + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});
$(document).on("click", "#taskExportBtn", function (e) {
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();
    var form = $("<form action='../Profile/TaskExport' method='post'></form>");
    form.append("<input type='text' name='fromDate' value='" + fromDate + "' />");
    form.append("<input type='text' name='toDate' value='" + toDate + "' />");
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});

$(document).on("click", "#editTask", function (e) {
    var grdMeritGrid = $("#TaskData").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var rowData = grdMeritGrid._data[rowIndex];
    $.ajax({
        url: '../Profile/_EditTask',
        type: "post",
        data: { __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() ,taskNum: rowData.SupportTaskNum },
        success: function (message) {
            $("#divEdittask").html(message);
            $("#divEdittask").modal('show');
        }
    });

});



$(document).on("click", "[id$=commentShow]", function () {
    var grdMeritGrid = $("#TaskData").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid._data[rowIndex];
    CommentClick(rowData, row);
});

function saveSupportComment(supportTaskCommentsNum) {
    var commentType = $("#commentEditBox_" + supportTaskCommentsNum).attr('data-commentLabel');
    $("#IsEditItem").val(true);
    var ItemCommentValue = $("#commentEditBox_" + supportTaskCommentsNum).val();
    $("#SupportTaskCommentsNum").val(supportTaskCommentsNum);
    $("#taskComment").text(ItemCommentValue);
    $("#btnCmtEdit_" +supportTaskCommentsNum).show();
    $("#frmSupportTaskComment").submit();
}
function closeAfterCommentsSlide() {
    $("#divSupportTaskComments").modal('hide');
}

function teamchanged() {
    var $dropdownElement = $("#ddlTeamTitle");
    $($dropdownElement).getKendoDropDownList().list.find("li.k-item").first().hide(); 
    if ($("#ddlTeamTitle").val() == "") {
        $("#error_ddlTeamTitle").html("Team is missing");
    }
    else {
        $("#error_ddlTeamTitle").html("");
    }
}

function teamchangedInEdit() {
    //var $dropdownElement = $("#ddlEditTeamTitle");
    //$($dropdownElement).getKendoDropDownList().list.find("li.k-item").first().hide();
    if ($("#ddlEditTeamTitle").val() == "") {
        $("#errorEdit_ddlTeamTitleEdit").html("Team is missing");
    }
    else {
        $("#errorEdit_ddlTeamTitleEdit").html("");
    }
}

function startDateChanged() {
    if ($("#addStartDate").val() == "" || $("#addStartDate").val() == undefined) {
        $("#error_startDate").html("StartDate is missing");
    }
    else {
        $("#error_startDate").html("");
    }
}

function startDateEditValidation() {
    if ($("#startDate").val() == "" || $("#startDate").val() == undefined) {
        $("#errorEdit_startDateEdit").html("StartDate is missing");
    }
    else {
        $("#errorEdit_startDateEdit").html("");
    }
}

function endDateChanged() {
    if ($("#addEndDate").val() == "" || $("#addEndDate").val() == undefined) {
        $("#error_endDate").html("endDate is missing");
    }

    else {
        $("#error_endDate").html("");
    }
}


function endDateEditValidation() {
    if ($("#endDate").val() == "" || $("#endDate").val() == undefined) {
        $("#errorEdit_endDateEdit").html("endDate is missing");
    }

    else {
        $("#errorEdit_endDateEdit").html("");
    }
}

function refreshpage()
{
    Successmessage("Saved Successfully");
    setTimeout(location.reload.bind(location), 2500);
    }


function DefineToDateEnableExport() {
    if(($("#fromDate").val()!=="") && ($("#toDate").val()!=""))
    {
        $("#taskExportBtn").attr("disabled", false);
    }
}

    $(document).on("click", "#checkValidation", function (e) {
    teamchanged();
    startDateChanged();
    endDateChanged();
    $("#spentHoursTxt").trigger("change");
    $("#teamTitleTxt").trigger("change");
    $("#teamDescriptionTxt").trigger("change");
    var errorMessage = "";
    $('[id^="error_"]').each(function () {
        errorMessage = errorMessage + ((this.textContent != "" && this.textContent != undefined && this.textContent != " ") ? (this.textContent).trim() + "<br/>" : "");
    });
    errorMessage = (errorMessage.trim() == "<br/>") ? "" : errorMessage.trim();
    if (errorMessage == "") {
        $("#addForm").submit();
    }
    });

$(document).on("keyup", "#teamTitleTxt", function (e) {
    var textLength = $("#teamTitleTxt").val();
    $("#countTxt").val(textLength.length)
});

$(document).on("keyup", "#teamTitleEditTxt", function (e) {
    var textLength = $("#teamTitleEditTxt").val();
    $("#countTxtInEdit").val(textLength.length)
});

$(document).on("click", "#addSupportHours,#minusSupportHours", function (e) {
    var spentHours = $("#supportHrsTxt").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
            url: '../Profile/updateSpentHours',
            type: "Post",
            data: {  __RequestVerificationToken:token,spentHours: spentHours},
            success: function (message) {
            var grdMeritGrid = $("#TaskData").data("kendoGrid");
            if ((grdMeritGrid._data.length) > 0) {
                var a = grdMeritGrid._data[0].TotalSpent
                var time = a.split(':');
                var hours = time[0];
                var minutes = time[1];
                var remainHrs = spentHours -hours;
                var remainMins = Math.abs(60 -minutes);
                if (minutes > 0)
                    remainHrs = remainHrs -1;
                if (minutes == 0)
                    remainMins = 0;
                var oldSpentPct = ($("#hoursSpentPct")[0].innerHTML).replace('%', "");
                var newSpentPct = ((getNumber(hours + "." +minutes) / spentHours) * 100).toFixed(0);
                var SpentPct = (Number(newSpentPct) > 100) ? 100 : newSpentPct
                $("#chart").removeClass("p" +oldSpentPct);
                $("#chart").addClass("p" +SpentPct);
                $("#availableHours").html(remainHrs + ":" +remainMins);
                $("#hoursSpentPct").html(newSpentPct + "%");
                $("#btnAddTask").attr("disabled", false);
            }
        }
    });
});

$(document).on("click", "#updateTask", function (e) {
    teamchangedInEdit();
    startDateEditValidation();
    endDateEditValidation();
    spenthoursvalidation();
    $("#teamTitleEditTxt").trigger("change");
    $("#teamDescriptionEditTxt").trigger("change");
    var errorMessage = "";
    $('[id^="errorEdit_"]').each(function () {
        errorMessage = errorMessage + ((this.textContent != "" && this.textContent != undefined && this.textContent != " ") ? (this.textContent).trim() + "<br/>" : "");
    });
    errorMessage = (errorMessage.trim() == "<br/>") ? "" : errorMessage.trim();
    if (errorMessage == "") {
        $("#editForm").submit();
}
});

    function spenthoursvalidation() {
    var value = $("#editedSpentHours").val();
    if (value == "") {
        $("#error_hrsEgEdit").html("SpentHours is missing");
    }
    else if (!(/^[0-9]{1,2}:[0-9]{1,2}$/.test(value))) {
        $("#errorEdit_hrsEgEdit").html('');
        $("#errorEdit_hrsEgEdit").html("Invalid time format");
        return false;
    }
    else {
        $("#errorEdit_hrsEgEdit").html('');
    }
}

    $(document).on("change", "#editedSpentHours", function (e) {
        var value = $("#editedSpentHours").val();
        var oldValue = e.target.defaultValue;
        var oldHrs = oldValue.split(':');
    var string = value;
    strx = string.split(':');
    array = [];
    array = array.concat(strx);
    var newHrs = (Number(array[0]) - Number(oldHrs[0]));
    var newMins = (Number(array[1]) - Number(oldHrs[1]));
    var Hrs = $("#availableHours")[0].innerHTML;
    var remainHrs = Hrs.split(':')
    if (value == "") {
        $("#errorEdit_hrsEgEdit").html("Spent Hours is missing");
    }
    else if (!(/^[0-9]{1,2}:[0-9]{1,2}$/.test(value))) {
        $("#errorEdit_hrsEgEdit").html('');
        $("#errorEdit_hrsEgEdit").html("Invalid time format");
        return false;
    }
    else if ((Number(remainHrs[0]) < newHrs) || ((Number(remainHrs[0]) == newHrs) && ((Number(remainHrs[1]) < newMins)))) {
        $("#errorEdit_hrsEgEdit").html('');
        $("#errorEdit_hrsEgEdit").html("Spent hours is exceed");
        return false;
    }
    else {
        $("#errorEdit_hrsEgEdit").html("");
}
});

$(document).on("change", "#teamTitleTxt", function (e) {
    if ($("#teamTitleTxt").val() == "") {
        $("#error_teamTitleTxt").html("TeamTitle  is missing");
    }
    else {
        $("#error_teamTitleTxt").html("");
}
});

$(document).on("change", "#teamTitleEditTxt", function (e) {
    if ($("#teamTitleEditTxt").val() == "") {
        $("#errorEdit_teamTitleEditTxt").html("TeamTitle  is missing");
    }
    else {
        $("#errorEdit_teamTitleEditTxt").html("");
}
});

$(document).on("change", "#firstname", function (e) {
    var value = $("#firstname").val();
    if (value == "")
        $('#Accerror_firstname').html('Firstname is missing');
    else
        $('#Accerror_firstname').html('');
});


$(document).on("change", "#useridTxt", function (e) {
    var value = $("#useridTxt").val();
    if (value == "")
        $('#Accerror_useridTxt').html('UserID is missing');
        else
        $('#Accerror_useridTxt').html('');
});
function userIDDuplicateValidation()
{
    var value = $("#useridTxt").val();
    if (value != "") {
        $.ajax({
            url: "../profile/GetUserIsDuplicate",
            data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                userID: value
            },
            type: "POST",
            async:false,
            success: function (result) {
                if(result)
                $('#Accerror_useridTxt').html('UserID is already Exists');
            }
        });
    }
}
$(document).on("click", "#accountUpdateBtn", function (e) {
    userIDDuplicateValidation();
    var errorMessage = "";
    $('[id^="Accerror_"]').each(function () {
        errorMessage = errorMessage + ((this.textContent != "" && this.textContent != undefined && this.textContent != " ") ? (this.textContent).trim() + "<br/>" : "");
    });
    errorMessage = (errorMessage.trim() == "<br/>") ? "" : errorMessage.trim();
    if (errorMessage == "") {
        $("#accountForm").submit();
}
});


$(document).on("change", "#teamDescriptionTxt", function (e) {
    if ($("#teamDescriptionTxt").val() == "") {
        $("#error_teamDescriptionTxt").html("Teamdescription is missing");
    }
    else {
        $("#error_teamDescriptionTxt").html("");
}
});

$(document).on("change", "#teamDescriptionEditTxt", function (e) {
    if ($("#teamDescriptionEditTxt").val() == "") {
        $("#errorEdit_teamDescriptionEditTxt").html("Teamdescription is missing");
    }
    else {
        $("#errorEdit_teamDescriptionEditTxt").html("");
}
});

$(document).on("show.bs.modal", "#divSupportTaskComments", function (e) {
    var date = new Date()
    var offsetms = date.getTimezoneOffset() * 60 * 1000;
    $('.posted-date').each(function () {
        var text = $(this).html();
        var serverDate = new Date(text);
        serverDate = new Date(serverDate.valueOf() - offsetms);
        $(this).html(serverDate.toLocaleTimeString());
    });
    $('.tooltiptext').each(function () {
        var text = $(this).html();
        var serverDate = new Date(text);
        serverDate = new Date(serverDate.valueOf() - offsetms);
        $(this).html(serverDate.toLocaleDateString() + " " + serverDate.toLocaleTimeString());
    });
});

    function cancelSupportComment(supportTaskCommentsNum) {
    $("#cmtText_" +supportTaskCommentsNum).show();
        //$("#meritEditBox" + num + "_" + empCommentNum).css("display", "none");
    $("#cmtSaveBtn_" +supportTaskCommentsNum).hide();
    $("#cmtCloseBtn_" +supportTaskCommentsNum).hide();
    $("#cmtCollapse_" +supportTaskCommentsNum).removeClass("in");
    $("#cmtCollapse_" +supportTaskCommentsNum).addClass("collapse");
    $("#taskComment").show();
    $("#btnCmtMandateSave").show();
    $("#btnCmtEdit_" + supportTaskCommentsNum).show();
}

    function supportCommentDelete(supportTaskCommentsNum) {
    var ItemCommentValue = document.getElementById("comment_" + $("#btnCmtDelete_" +supportTaskCommentsNum).attr('data-mycommentitemid'));
    if (supportTaskCommentsNum != 0 && supportTaskCommentsNum != undefined) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
                url: '../Profile/DeleteComment',
                data: {
                    __RequestVerificationToken: token, SupportTaskCommentsNum: supportTaskCommentsNum
        },
                type: "post",
                async: true,
                cache: false,
                success: function (result) {
                Successmessage("Deleted Successfully");
                ItemCommentValue.style.display = "none";
                closeAfterCommentsSlide();
        }
        });
    }
    return false;
}

    function cmtEditClick(supportTaskCommentsNum) {
    var ItemCommentValue = $("#btnCmtEdit_" +supportTaskCommentsNum).attr('data-myitemid');
    $("#commentEditBox_" +supportTaskCommentsNum).text(ItemCommentValue);
    $("#taskComment").css("display", "none");
    $("#btnCmtMandateSave").css("display", "none");
    $("#cmtSaveBtn_" +supportTaskCommentsNum).show();
    $("#cmtCloseBtn_" +supportTaskCommentsNum).show();
    var isCollapsed = $("#commentEditBox_" +supportTaskCommentsNum).is(":visible");
    if (!isCollapsed) {
        $("#cmtText_" + supportTaskCommentsNum).css("display", "none");
        $("#btnCmtEdit_" + supportTaskCommentsNum).hide();
    }
    else
    {
        $("#btnCmtEdit_" + supportTaskCommentsNum).show();
        $("#cmtText_" +supportTaskCommentsNum).show();
        $("#taskComment").show();
        $("#btnCmtMandateSave").show();
    }

    if (lastEditedCommentNum != supportTaskCommentsNum) {
        if (lastEditedCommentNum != 0) {
            var isCollapsed = $("#commentEditBox_" +lastEditedCommentNum).is(":visible");
            if (isCollapsed) {
                $("#cmtText_" +lastEditedCommentNum).show();
                $("#cmtCollapse_" +lastEditedCommentNum).removeClass("in");
        }
    }
        lastEditedCommentNum = supportTaskCommentsNum;
    }
}

    function refreshGridData() {
    $("#TaskData").data("kendoGrid").dataSource.read();
    $("#divAddtask").modal('hide');
}

    function editedSuccess() {
    $("#TaskData").data("kendoGrid").dataSource.read();
    $("#modelClose").trigger("click");
}

    function CommentClick(rowData, row) {
        var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
            url: '../Profile/_SupportTaskComment',
            type: "post",
            data: {
                __RequestVerificationToken:token,supportTaskNum: rowData.SupportTaskNum
    },
            success: function (result) {
            isPopupEdited = false;
            $("#divSupportTaskComments").html(result);
            $("#divSupportTaskComments").modal('show');
    }
    });
}


    function uploadImage() {
    uploader.start();
}

    function TextCopied(element_id) {
    $("#link").attr("readonly", false);
    var aux = document.createElement("div");
    aux.setAttribute("contentEditable", true);
    aux.innerHTML = document.getElementById(element_id).value;
    aux.setAttribute("onfocus", "document.execCommand('selectAll',false,null)");
    document.body.appendChild(aux);
    aux.focus();
    document.execCommand("copy");
    document.body.removeChild(aux);
    $("#link").attr("readonly", true);
}

    function additionalParamInfo()
    {
        return {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        }
    }

