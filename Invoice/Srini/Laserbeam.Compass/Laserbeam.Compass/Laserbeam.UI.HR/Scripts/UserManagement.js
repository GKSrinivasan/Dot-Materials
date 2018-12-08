var totalUsersCount = 0;
var activeUsersCount = 0;
var yetToLoginCount = 0;
var lockedUsersCount = 0;
var isFilter = false;
var isModify = false;
var uploader;
var emailChangeFlag = false;
var isErrorTemplate = false;
$(document).ready(function () {
    $("#userDataProcess").attr("disabled", true);
    $("#btnClearFilter").hide();
    getUserTileData();
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        drop_element: 'drop-area',
        browse_button: 'browseBtn',
        url: "../Upload",
        filters: {
            mime_types: [
              { title: "Image files", extensions: "xlx,xlsx" },
            ],
            max_file_size: "10mb",
            prevent_duplicates: true
        },

        dragdrop: true,

        init: {
            FilesAdded: function (up, files) {
                plupload.each(files, function (file) {
                    var fileItem = $("<div class='file-area' id='" + file.id + "' data-fileid='" + file.rid + "'><div class='file-icon'><span class='fa " + getFileIconUsingMIME(file.type) + "'></span></div><div class='file-content'><div style='font-size:14px;'>" + file.name + "</div><div style='font-size:10px;'>" + getFileSizeUsingBytes(file.size) + "</div><div class='progress'><div class='progress-bar'  id = 'upload' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width:0%;'>0%</div></div></div><div class='file-remove'><span class='fa fa-times' data-fileid='" + file.id + "'></span></div> </div>");
                    $("#divFileArea").append(fileItem);
                    if (uploader.files.length > 0)
                        $("#userDataProcess").removeAttr("disabled");
                });
            },

            UploadProgress: function (up, file) {
                var fileProgress = $("#" + file.id).find(".progress-bar");
                var filePercent = file.percent < 75 ? file.percent : 75;
                fileProgress.css("width", filePercent + "%");
                fileProgress.text(filePercent + "%");
                fileProgress.attr("aria-valuenow", filePercent);
            },
            BeforeUpload: function (up, file) {
                if (uploader.total.uploaded == 0) {
                    $('#errorFiles').children().remove();
                    var loadingPanel = $("#ajaxLoadingPanel");
                    loadingPanel.height($(window).height());
                    loadingPanel.width($(window).width());
                    loadingPanel.css('top', $(window).scrollTop());
                    loadingPanel.css('left', $(window).scrollLeft());
                    loadingPanel.show();
                }
                if ((uploader.total.uploaded + 1) == uploader.files.length) {
                    uploader.settings.multipart_params = { isLastFile: 'yes', __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() };
                }
                else
                    uploader.settings.multipart_params = { isLastFile: '', __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() };
                $("#" + file.id).find(".progress").show();
                $(".file-remove").find("#" + file.id).hide();
            },
            FileUploaded: function (up, file, result) {
                result = JSON.parse(result.response);
                if (result.Message != "") {
                    var fileProgress = $("#" + file.id).find(".progress-bar");
                    var filePercent = 100;
                    if (result.Message != "Uploaded successfully" && result.Message != "Processed successfully") {
                        Errormessage(result.Message);
                        isErrorTemplate = true;
                        fileProgress.css("background-color", "red");
                        if (result.Message == "Invalid format or Password protected") {
                            var errorFiles = "<div style='word-wrap: break-word;margin-left:2% !important;margin-right:2% !important;'><div class='row'>" + file.name + "<br/> Invalid format or Password protected</div></div><br/>";
                            $("#errorFiles").append(errorFiles);
                        }
                        else {
                            var errorFiles = "<div style='word-wrap: break-word;margin-left:2% !important;margin-right:2% !important;'><div class='row'>" + file.name + "<br/>" + result.Message + " Columns mismatch with template</div></div><br/>";
                            $("#errorFiles").append(errorFiles);
                        }
                    }
                    fileProgress.css("width", filePercent + "%");
                    fileProgress.text(filePercent + "%");
                    fileProgress.attr("aria-valuenow", filePercent);
                    Successmessage(result.Message);
                }
            },
            UploadComplete: function (up, file) {
                up.splice();
                $('#divFileArea').children().remove();
                getUserTileData();
                $("#grdUserData").data("kendoGrid").dataSource.read();
                getErrorDataCount();
                $("#ajaxLoadingPanel").hide();
            },
            Error: function (up, err) {
                var fileProgress = $("#" + err.file.id).find(".progress-bar");
                var filePercent = 100;
                fileProgress.css("width", filePercent + "%");
                fileProgress.text(filePercent + "%");
                fileProgress.attr("aria-valuenow", filePercent);
            }
        }
    });
    uploader.init();

    $(document).on("hide.bs.modal", "#userData-Error", function (e) {
        $('#errorFiles').children().remove();
    });

});

$(document).on("click", "#lnkXmlUserDataFileName", function (e) {
    var grdData = $("#grdUserData").data("kendoGrid");
    var rowItem = $(this).closest("tr");
    var rowIndex = $(rowItem).index();
    var rowData = grdData._data[rowIndex];
    var xmlProcessNum = rowData.XMLProcessNum;
    var form = $("<form action='../UserManagement/GetExportXmlFile' method='post'></form>")
    form.append("<input type='text' name='xmlProcessNum' value='" + xmlProcessNum + "' />");
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});

$(document).on("click", "[id$=btnDelete]", function (e) {
    var grdData = $("#grdUserData").data("kendoGrid");
    var rowItem = $(this).closest("tr");
    var rowIndex = $(rowItem).index();
    var rowData = grdData._data[rowIndex];
    var xmlProcessNum = rowData.XMLProcessNum;
    var status = showConfirm(e, "Do you want to delete?");
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (status) {
        $.ajax({
            url: "../UserManagement/DeleteUserTemplateData",
            data: {
               __RequestVerificationToken:token,  xmlProcessNum: xmlProcessNum
            },
            type: "post",
            success: function (result) {
                Successmessage(result);
                grdData.dataSource.read();
            }
        });
    }
    else {
        return false;
    }

});

$(document).on("hide.bs.modal", "#divAddUser", function (e) {
    if (isModify == true)
        $("#grdManageUser").data("kendoGrid").dataSource.read();
    isModify = false;
});

$(document).on("click", "#btnAdduser", function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../UserManagement/_UserAddPopUp",
        type: "get",
        success: function (result) {
            $("#divAddUser").html(result);
            $("#divAddUser").modal('show');
        }
    });

});

$(document).on("click", "#btnSendEmail", function (e) {

    $.ajax({
        url: "../UserManagement/_SendEmail",
        type: "get",
        async: true,
        success: function (result) {
            $("#sendReminderClearFilter").hide();
            $("#divSendEmail").html(result);
            $("#divSendEmail").modal('show');
        }
    });
});

$(document).on("click", ".file-remove .fa-times", function (e) {
    var fileid = $(this).data("fileid");
    uploader.removeFile(fileid);
    $(this).parents(".file-area").remove();
    if (uploader.files.length == 0)
        $("#userDataProcess").attr("disabled", true);
    else
        $("#userDataProcess").removeAttr("disabled");
});

$(document).on("click", "#userDataProcess", function (e) {
    uploader.start();
    return false;
});

$(document).on("change", "#ddlUserRole", function () {    
    var userRole = $("#ddlUserRole").data("kendoDropDownList").dataItem().UserRole;
    if (userRole == "--User Role--"){
        $("#txtUserRoleType").val("");        
    }        
    else
    {
        $("#txtUserRoleType").val(userRole);        
    }


     if (userRole == "Admin")
         $("#accessCheckBox").css("visibility", "visible")
        else
        $("#accessCheckBox").css("visibility", "hidden")
});

$(document).on('click', "#helpIcon", function () {
    $('[data-toggle="popover"]').popover();
});

$(document).on('click', '#checkbox2', function (e) {


    if (this.checked) {
        $("#isEmail").val("true");
    }
    else {
        $("#isEmail").val("false");
}
});

$(document).on("click", "#btnCancel", function () {
    $('#addUser').modal('hide');
});
    //FilterSort Popup
$(document).on('click', '#filter', function () {
    $("body").css("overflow", "hidden");
    $.ajax({
            url: '../UserManagement/_FilterSortPopup',
            type: 'GET',
            cache: false,
            async: true,
            dataType: 'html',
            success: function (result) {
            $("#wndFilterSortPopup").html(result);
            var wndFilterSort = $("#wndFilterSortPopup").data("kendoWindow");
            wndFilterSort.options.gridName = "grdManageUser";
            wndFilterSort.center().open();
            $("#filterseperator").css("display", "inline");
            $("#hide").show();
            $("#clearfilter").css("display", "inline");
            $("#btnClearFilter").show();
    }
    });
});

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

$(document).on("click", "#btnClearFilter", function (event) {
        clearFilterSort(true);
        $("#btnClearFilter").hide();
        $("#filter").show();
});
    //Export
$(document).on("click", "#userExport", function (event) {
        var form = $("<form action='../UserManagement/AppUserExport' method='post'></form>")
        form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
        form.appendTo("body");
        form.submit();
        form.remove();
});

$(document).on("click", "#uploadUserTab", function () {
    $("#btnAdduser").hide();
    $("#dropdownMenu1").hide();
});

$(document).on("click", "#userListTab", function () {
    $("#btnAdduser").show();
    $("#dropdownMenu1").show();
    $("#grdManageUser").data("kendoGrid").dataSource.read();
});

$(document).on("click", "#btnDeleteUser", function (e) {
    var userNum = $("#txtUserNum").val();
    var userId = $("#txtUserId").val();
    if (!showConfirm(e, "Do you want to delete the user?")) return false;
    if (userNum != null) {
        $.ajax({
                url: '../UserManagement/DeleteUser',
                data: {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(), userNum: userNum, userId: userId
        },
                type: "post",
                cache: false,
                success: function (result) {
                $("#grdManageUser").data("kendoGrid").dataSource.read();
                getUserTileData();
                $("#divAddUser").modal('hide');
                Successmessage("Deleted the User Successfully");

        }
    });
}
    return false;
});

$(document).on("click", "#btnCancel", function () {
    $('#divAddUser').modal('hide');
});

$(document).on("focusout", "#editUser", function () {
    $("#editUser").modal('hide');
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
            url: "../UserManagement/SendEmailReminderToUser",
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

$(document).on("click", "[id$=chkBox]", function (e) {

    var bool = ($('input#chkBox:checked').length == $("input#chkBox").length) ? true : false
    $("#chkBoxSelectAll")[0].checked = bool;
    emailChangeFlag = true;
});

$(document).on('click', '#checkbox1', function (e) {
    if (this.checked) {
        $("#addUserAlert").show();
    }
    else {
        $("#addUserAlert").hide();
        $("#addAdminAccess").val(false);
    }
});

$(document).on('click', '#adminAccessNotValid', function (e) {
    $("#checkbox1").prop("checked", false);
    $("#addAdminAccess").val(false);
    $("#addUserAlert").hide();
});

$(document).on('click', '#adminAccessValid', function (e) {
    $("#checkbox1").prop("checked", true);
    $("#addAdminAccess").val(true);
    $("#addUserAlert").hide();
});

$(document).on("keydown", "#txtUserId", function (e) {
    var code = e.keyCode || e.which;
    if (code == '9') {
        e.preventDefault();
        setTimeout(function () {
            $("#txtEmployeeID").focus();
        }, 0);
    }
});
                            
$(document).on("change", "#emailReminderSubject", function (e) {
    emailChangeFlag = true;
});

$(document).on("show.bs.modal", "#divAddUser", function (e) {
    $("#addUserAlert").hide();
    $.validator.unobtrusive.parse(document);
    setValidation();
    var a = $("#userStatus").val();
    if (a == "Active" || a == "")
        $("#btnAddUserStatus").prop("checked", true);
    if (a == "Lock")
        $("#btnAddUserStatus").prop("checked", false);
    var b = $("#addAdminAccess").val();
    if (b == "True")
        $("#checkbox1").prop("checked", true);
    if (b == "False")
        $("#checkbox1").prop("checked", false);

    $('[data-toggle="tooltip"]').tooltip();
    addStatus = $("#btnAddUserStatus")[0].checked;
    if (addStatus == false) {
        $("#checkbox2").attr("disabled", true);
        $("#checkbox2").prop("checked", false);
    }
    else if (addStatus == true) {
        $("#checkbox2").attr("disabled", false);
    }
});

$(document).on("show.bs.modal", "#divSendEmail", function (e) {
    $("#sendReminderClearFilter").hide();
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

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

function gridChange(e) {

    var selectedItem = this.dataItem(this.select());
    $.ajax({
        url: '../UserManagement/_UserModifyPopUp',
        data: {
            userNum: selectedItem.UserNum,
        },
        type: "get",
        cache: false,
        success: function (result) {
            isModify = true;
            $("#divAddUser").html(result);
            $("#divAddUser").modal('show');
        }
    });

}

function editStatusClick(e) {
    editStatus = $("#btnEditUserStatus")[0].checked;
    if (editStatus == false) {
        var userStatus = "Active";
        $("#userStatus").val("Active");
    }
    else if (editStatus == true) {
        var userStatus = "Lock";
        $("#userStatus").val("Lock");
    }

    $("#editUserStatus").val(userStatus);
}

function grdUserDataBound() {
    var grdUserDetails = $("#grdUserData").data("kendoGrid");
    if (grdUserDetails._data.length == 0) {
        $("#userData").hide();
        $("#grdHeaderTxt").hide();
    }
    else {
        $("#userData").show();
        $("#grdHeaderTxt").show();
    }
}

function handleClick(a) {
    addStatus = $("#btnAddUserStatus")[0].checked;
    if (addStatus == false) {
        var userStatus = "Active";
        $("#userStatus").val("Active");
        $("#checkbox2").attr("disabled", false);
    }
    else if (addStatus == true) {
        var userStatus = "Lock";
        $("#userStatus").val("Lock");
        $("#checkbox2").attr("disabled", true);
        $("#checkbox2").prop("checked", false);
    }
    $("#addUserStatus").val(userStatus);
}

function userGridDataBound() {
    var grdUserDetails = $("#grdManageUser").data("kendoGrid");
    if (grdUserDetails.dataSource._data.length == 0) {
        var colCount = $("#grdManageUser").find('th').length;
        $("#grdManageUser").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center;background-color:#6686C4;padding-top:30px"></td></tr>');
        $("#grdManageUser").find('.k-grid-content tbody tr td').css("padding-top", 11).append('<b style="background-color:#6686C4 !important;color:white !important;">No Results Found!</b>');
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

function closeAfterUserAdded(modelValue) {
    $("#divAddUser").modal('hide');
    $("#grdManageUser").data("kendoGrid").dataSource.read();
    if (modelValue == 2)
        Successmessage("Great ! User Added  Successfully");
    else
        Successmessage("User information modified successfully");

}

function UserGridData() {
        var token = $('input[name="__RequestVerificationToken"]').val();
        return { __RequestVerificationToken: token };
    }

function additionalParamInfo()
    {
        return {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        }
    }

function showUserStatusImage(status) {
    var commentImage;
    switch (status) {
        case "Active": commentImage = "<label class='label label-success'>Active</label>";
            break;

        case "Yet To Login": commentImage = "<label class='label label-default'>Yet to Login</label>";
            break;

        case 'Locked': commentImage = "<label class='label label-danger'>Locked</label>";
            break;
        default: commentImage = "<label class='label label-danger'>Locked</label>";
    }
    return commentImage;
}

function showMailStatusImage(status, date) {
    var a = "Last mail sent on :" +date;
    var commentImage = (status == true) ? "<span class='glyphicon glyphicon-envelope tooltiptext' style='color:gray;' title='" + a + "'></span>" : "";
    return commentImage;
}

function userModifyPopUp(userNum) {
    var userNum = userNum;
    $.ajax({
            url: '../UserManagement/_UserModifyPopUp',
            data: {
                    userNum: userNum,
    },
            type: "get",
            cache: false,
            success: function (result) {
            isModify = true;
            $("#divAddUser").html(result);
            $("#divAddUser").modal('show');
    }
    });
}

function change(e) {
    var dataItem = this.dataItem(e.item.index());
    var userRole = dataItem.UserRole;
    if (userRole == "Admin")
        $("#accessCheckBox").css("visibility", "hidden")
    else
        $("#accessCheckBox").css("visibility", "hidden")
}

function DisplayNoResultsFound(grid) {
        // Get the number of Columns in the grid
    var dataSource = grid.data("kendoGrid").dataSource;
    var colCount = grid.find('.k-grid-header colgroup > col').length;

        // If there are no results place an indicator row
    if (dataSource._view.length == 0) {
        grid.find('.k-grid-content tbody')
            .append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center"><b>No Results Found!</b></td></tr>');
    }

        // Get visible row count
    var rowCount = grid.find('.k-grid-content tbody tr').length;

        // If the row count is less that the page size add in the number of missing rows
    if (rowCount < dataSource._take) {
        var addRows = dataSource._take -rowCount;
        for (var i = 0; i < addRows; i++) {
            grid.find('.k-grid-content tbody').append('<tr class="kendo-data-row"><td>&nbsp;</td></tr>');
    }
    }
}

function downloadUserTemplateData() {
    var form = $("<form action='../UserManagement/GetUserDataTemplate' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

function getFileIconUsingMIME(fileType) {
    if (fileType.match("image") == "image")
        return "fa-file-image-o";
    else if (fileType.match("word") == "word")
        return "fa-file-word-o";
    else if (fileType.match("excel") == "excel") {
        return "fa-file-excel-o";
    }
    else if (fileType.match("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
        return "fa-file-excel-o";
    }
    else if (fileType.match("powerpoint") == "powerpoint")
        return "fa-file-powerpoint-o";
    else if (fileType.match("pdf") == "pdf")
        return "fa-file-pdf-o";
    else if (fileType.match("text") == "text")
        return "fa-file-text-o";
    else
        return "fa-file-o";

}

function getFileSizeUsingBytes(fileSize) {
    if (fileSize >= 1048576) {
        var info = Math.round(fileSize / 1048576) + " MB)";
        return info;
    }
    else if (fileSize <= 1048576 && fileSize >= 1024) {
        var info = Math.round(fileSize / 1024) + "KB";
        return info;
    }
    else if (fileSize <= 1024) {
        var info = Math.round(fileSize) + "Bytes";
        return info;
    }
    else {
        var info = Math.round(fileSize / 1073741824) + "GB";
        return info;
    }

}

function getUserDataErrorList() {
    $.ajax({
            url: '../UserManagement/_GetUserDataErrorList',
            type: "GET",
            async: false,
            contentType: "application/json",
            processData: false,
            success: function (result) {
            $("#userData-Error").html(result);
            $('#userData-Error').modal('show');
            $("#userDataerrorFiles").html($("#errorFiles").html());
    }
    });
}

function getErrorListExport() {
    var form = $("<form action='../UserManagement/GetUserDataErrorExport' method='post'></form>");
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

function EditorChanged() {

    emailChangeFlag = true;
}

function cancelSendEmailReminder(e) {
    var sendReminderGrid = $("#grdSendReminderNotification").data("kendoGrid");
    sendReminderGrid.dataSource.filter({});
    sendReminderGrid.dataSource.sort({});
    sendReminderGrid.refresh();
    $("#chkBoxSelectAll").removeAttr('checked');
    emailChangeFlag = false;
    $("#divSendEmail").modal('hide');
}

function getErrorDataCount() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../UserManagement/GetUserDataErrorCount",
        type: "Post",
        data:({ __RequestVerificationToken:token}),
            success: function (result) {
            if (result > 0 || isErrorTemplate) {
                getUserDataErrorList();
                if (result > 0)
                    $("#accessControlErrorLink").show();
                isErrorTemplate = false;
            }
            else {
                $("#accessControlErrorLink").hide();
            }
    }
    });
}

function getUserTileData() {
        var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../UserManagement/GetUserTileData",
        type: "post",
        data: {
            __RequestVerificationToken: token
        },
            success: function (result) {
            $("#totalUsers").text(result[0].TotalCount);
            $("#activeUsers").text(result[0].ActiveCount);
            $("#yetToUsers").text(result[0].YetToStartCount);
            $("#lockedUsers").text(result[0].LockedUsersCount);
    }
    });
}

function grdUserErrorRecordDataBound() {
    var grdUserDataError = $("#grdUserDataError").data("kendoGrid");
    if (grdUserDataError._data.length == 0) {
        $("#divUserDataError").hide();
    }
    else {
        $("#divUserDataError").show();
    }
}

function setValidation() {
    $(".input-validation-error").parent().removeClass('has-success').addClass("has-error");
    $("div.validation-summary-errors").has("li:visible").addClass("alert-block alert-danger");

    $('#form').data('validator').settings.onfocusout = function (element) {
        $(element).valid();
    };
}


