var arr_ids = [];
var uploader;
var isErrorTemplate = false;
var LevelBasedWFData = true;
$(document).ready(function () {
    $("#approvalDataProcess").attr("disabled", true);
    $('[data-toggle="tooltip"]').tooltip();
    onWorkFlowPageLoad();
    $("#btnClearFilter").hide();

    uploader = new plupload.Uploader({
        runtimes: 'html5',
        drop_element: 'drop-zone',
        browse_button: 'browsefiles',
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
                        $("#approvalDataProcess").removeAttr("disabled");
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
                            var errorFiles = "<div style='word-wrap: break-word;margin-left:2% !important;margin-right:2% !important;'><div class='row'>" + file.name + "<br/>" + result.Message + " Columns mismatch with templateColumns mismatch with template</div></div><br/>";
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
                GetWorkFlowErrorCount();
                $("#grdApprovalData").data("kendoGrid").dataSource.read();
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

    $(document).on("hide.bs.modal", "#approvalData-Error", function (e) {
        $('#errorFiles').children().remove();
    });

    $(document).on("click", "#approvalDataProcess", function (e) {
        uploader.start();
        return false;
    });


    $(document).on("click", ".file-remove .fa-times", function (e) {
        var fileid = $(this).data("fileid");
        uploader.removeFile(fileid);
        $(this).parents(".file-area").remove();
        if (uploader.files.length == 0)
            $("#approvalDataProcess").attr("disabled", true);
        else
            $("#approvalDataProcess").removeAttr("disabled");
    });


    $(document).on("click", "#customBtn", function (e) {
        $("#uploadWfDataTab").show();
        $("#btnSave").show();
        $("#employeeID").prop("checked", true);
        $('[id*=deleteBtn]').show();
        $('[id*=AppMgr]').show();
        $("#wrkflwlevel").hide();
        var grid = $("#grdWorkFlow").data("kendoGrid");
        if (grid.dataSource._filter != undefined) {
            clearFilterSort(true)
            $("#btnClearFilter").hide();
            $("#filter").show();
        }
        grid.dataSource.read();
    });
    $(document).on("click", "#hierarchyBtn", function (e) {
        addSelectItem();
        if (!showSaveWarning(e, "objChangeFlag")) {
            e._defaultPrevented = true;
            return false;
        }
        LevelBasedWFData = false;
        var grid = $("#grdWorkFlow").data("kendoGrid");
        if (grid.dataSource._filter != undefined) {
            clearFilterSort(true)
            $("#btnClearFilter").hide();
            $("#filter").show();
        }
        var ProcessNum = $("#ddlProcess").val();
        var CustomizeBtn = $('input:radio[name=CustomizeBtn]:checked').val();
        var wfLevel = 0;
        var token =$('input[name="__RequestVerificationToken"]').val();
        if (ProcessNum) {
            var cnfm = showConfirm(e, "You are about to make changes to Workflow configuration by loading current Org structure. Do you want to continue?  \n Press OK to reload. \n Press cancel to continue without reloading");
            if (cnfm) {
                $.ajax({
                    url: '../WorkFlow/ReloadGrid',
                    type: "post",
                    data: ({ __RequestVerificationToken: token, processNum: ProcessNum, Level: 0, LevelBasedWFData: LevelBasedWFData }),
                    success: function (result) {
                        objChangeFlag = false;
                        $("#uploadWfDataTab").hide();
                        $("#btnSave").hide();
                        $('[id*=deleteBtn]').hide();
                        $('[id*=AppMgr]').hide();
                        $("#employeeID").prop("checked", true);
                        $("#wrkflwlevel").show();
                        grid.dataSource.read();
                        Successmessage(result);
                        LevelBasedWFData = true;
                        $("#ddlWFLevels").data("kendoDropDownList").value(-1);
                    }
                });
            }
            else {
                $("#customBtn").prop("checked", true);
                return false;
            }
        }
    });

    $(document).on("click", "[id$=btnDelete]", function (e) {
        var grdData = $("#grdApprovalData").data("kendoGrid");
        var rowItem = $(this).closest("tr");
        var rowIndex = $(rowItem).index();
        var rowData = grdData._data[rowIndex];
        var xmlProcessNum = rowData.XMLProcessNum;
        var token =$('input[name="__RequestVerificationToken"]').val();
        var status = showConfirm(e, "Do you want to delete?")
        if (status) {
            $.ajax({
                url: "../Workflow/DeleteApprovalTemplateData",
                data: {
                    __RequestVerificationToken: token,
                    xmlProcessNum: xmlProcessNum
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

    $(document).on("click", "#employeeID", function (e) {
        var CustomizeBtn = $('input:radio[name=CustomizeBtn]:checked').val();
        if (CustomizeBtn == "Custom") {
            $('[id*=deleteBtn]').show();
            $('[id*=AppMgr]').show();
        }
        else {
            $('[id*=deleteBtn]').hide();
            $('[id*=AppMgr]').hide();
        }
        var grid = $("#grdWorkFlow").data("kendoGrid");
        grid.dataSource.read();
    });

    $(document).on("click", "#employeeName", function (e) {
        if (!showSaveWarning(e, "objChangeFlag")) {
            e._defaultPrevented = true;
            return false;
        }
        $('[id*=deleteBtn]').hide();
        $('[id*=AppMgr]').hide();
        var grid = $("#grdWorkFlow").data("kendoGrid");
        grid.dataSource.read();
    });

    var checkedType = "";

    $(document).on("click", "#lnkXmlApprovalFileName", function (e) {
        var grdData = $("#grdApprovalData").data("kendoGrid");
        var rowItem = $(this).closest("tr");
        var rowIndex = $(rowItem).index();
        var rowData = grdData._data[rowIndex];
        var xmlProcessNum = rowData.XMLProcessNum;
        var form = $("<form action='../WorkFlow/GetExportXmlFile' method='post'></form>")
        form.append("<input type='text' name='xmlProcessNum' value='" + xmlProcessNum + "' />");
        form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
        form.appendTo("body");
        form.submit();
        form.remove();
    });

    $(document).on('click', '#filter', function () {
        $("body").css("overflow", "hidden");
        if ($('#employeeID').is(':checked')) {
            checkedType = "EmployeeID";
        }
        else {
            checkedType = "EmployeeName";
        }
        $.ajax({
            url: '../Workflow/_FilterSort',
            type: 'GET',
            cache: false,
            data: { type: checkedType },
            async: true,
            dataType: 'html',
            success: function (result) {
                $("#wndFilterSort").html(result);
                var wndFilterSort = $("#wndFilterSort").data("kendoWindow");
                wndFilterSort.options.gridName = "grdWorkFlow";
                wndFilterSort.center().open();
                $("#filterseperator").css("display", "inline");
                isFilter = true;
            }
        });
    });

    $(document).on("click", "#btnClearFilter", function (event) {
        clearFilterSort(true);
        $("#btnClearFilter").hide();
        $("#filter").show();
    });

    $(document).on("click", "[id*=deleteBtn]", function (e) {
        var grid = $("#grdWorkFlow").data("kendoGrid");
        var deleteBtnID = ($(this)[0].id).replace("deleteBtn", "");
        var headerId = "AppMgr" + deleteBtnID + "EmpNum";
        var ProcessNum = $("#ddlProcess").val();
        var headerValue = $(this)[0].value;
        var _params = grid.dataSource._params();
        var gridParams = grid.dataSource.transport.parameterMap(_params);
        var token = $("input[name='__RequestVerificationToken']").val();
        var status = showConfirm(e, "Do you want to delete entire level" + deleteBtnID.toLowerCase() + " " + "mangers?")
        if (status) {
            $.ajax({
                url: '../WorkFlow/CopyInWorkFlowGrid',
                type: "post",
                data: {__RequestVerificationToken: token, filter: gridParams.filter, group: gridParams.group, page: gridParams.page, pageSize: gridParams.pageSize, sort: gridParams.sort, processNum: ProcessNum, headerID: headerId, headerValue: headerValue, actionStatus: "Delete"
                },
                //data: JSON.stringify(prepared),
                success: function (result) {
                    var message = (result) ? "Deleted successfully" : "An error occurred while deleting record";
                    objChangeFlag = false;
                    Successmessage(message);
                    grid.dataSource.read();
                    if (grid.dataSource._filter != undefined) {
                        $("#btnClearFilter").trigger("click");
                    }

                }
            });
        }

    });
    $(document).on("change", "[id *= idtxtbox1]", function (e) {
        if (e.target.value == "") {
            e.preventDefault();
            //Warningmessage('LevelOne ManagerID is Mandatory');
            e.target.value = e.target.defaultValue;
            return false;
        }
    });
    $(document).on("click", "#btnSave", function (e) {
        var grid = $("#grdWorkFlow").data("kendoGrid");
        var gridData = grid.dataSource.data();
        a = e.target.value;
        var ddlProcess = $('#ddlProcess').data("kendoDropDownList");
        var dataItem = ddlProcess.dataItem(ddlProcess.select());
        var ProcessNum = $("#ddlProcess").val();
        var header_Value = "";
        var headerId = "";
        var token = $("input[name='__RequestVerificationToken']").val();
        $("[id*=AppMgr]").each(function () {
            if ($(this)[0].value != "") {
                headerId = headerId + "," + $(this)[0].id;
                header_Value = header_Value + "," + $(this)[0].value;
            }
        })
        var postData = [];
        $(gridData).each(function (index, item) {
            if (item.IsChanged) {
                postData.push(item);
            }
        });
        index = $.inArray(((header_Value).replace(",", "")).trim(), arr_ids);
        if (index == -1 && header_Value != "") {
            Warningmessage('Invalid ManagerID')
            return false;
        }
        if ((objChangeFlag)) {
            var jsonPostData = JSON.stringify(postData);
            var gridUpdateData = JSON.parse(jsonPostData);
            $.ajax({
                url: '../WorkFlow/UpdateGridData',
                type: "post",
                data: { __RequestVerificationToken: token, workFlow: gridUpdateData, processNum: ProcessNum, module: dataItem.Text },
                success: function (message) {
                    if (header_Value != "" && headerId != "") {
                        var _params = grid.dataSource._params();
                        var gridParams = grid.dataSource.transport.parameterMap(_params);
                        $.ajax({
                            url: '../WorkFlow/CopyInWorkFlowGrid',
                            type: "post",
                            data: {__RequestVerificationToken:token, filter: gridParams.filter, group: gridParams.group, page: gridParams.page, pageSize: gridParams.pageSize, sort: gridParams.sort, processNum: ProcessNum, headerID: headerId, headerValue: header_Value, actionStatus: "Update"
                            },
                            success: function (result) {
                                var message = (result) ? "Updated successfully" : "An error occurred while saving record";
                                Successmessage(message);
                            }
                        });
                    }
                    objChangeFlag = false;
                    grid.dataSource.read();
                    Successmessage(message);
                    if (grid.dataSource._filter != undefined) {
                        $("#btnClearFilter").trigger("click");
                    }
                    $('[id*=AppMgr]').val('');
                }
            });
        }

    });

    $(document).on("click", "#employeeListLnk", function (e) {
        $("#workflowSection").show();
        $("#btnSave").show();
        $("#dropdownMenu1").show();
    });

    $(document).on("click", "#uploadWFData", function (e) {
        $("#workflowSection").hide();
        $("#btnSave").hide();
        $("#dropdownMenu1").hide();
    });

    $(document).on("change", "[id*=txt_]", function (e) {
        var grid = $("#grdWorkFlow").data("kendoGrid");
        var row = $(this).closest("tr");
        var rowIndex = $(row).index();
        var rowData = grid._data[rowIndex];
        var rowItem1 = grid.dataItem(row);
        var parentId = e.currentTarget.offsetParent.id;
        objChangeFlag = true;
        switch (parentId) {
            case "idtxtbox1": rowData.EmpID_1 = (e.target.value).trim();
                break;
            case "idtxtbox2": rowData.EmpID_2 = (e.target.value).trim();
                break;
            case "idtxtbox3": rowData.EmpID_3 = (e.target.value).trim();
                break;
            case "idtxtbox4": rowData.EmpID_4 = (e.target.value).trim();
                break;
            case "idtxtbox5": rowData.EmpID_5 = (e.target.value).trim();
                break;
            case "idtxtbox6": rowData.EmpID_6 = (e.target.value).trim();
                break;
            case "idtxtbox7": rowData.EmpID_7 = (e.target.value).trim();
                break;
            case "idtxtbox8": rowData.EmpID_8 = (e.target.value).trim();
                break;
            case "idtxtbox9": rowData.EmpID_9 = (e.target.value).trim();
                break;
            case "idtxtbox10": rowData.EmpID_10 = (e.target.value).trim();
                break;

        }
        var isTrue = checkValid((e.target.value).trim(), rowData.EmployeeID);
        if (isTrue == true) {
            setRowChanged(rowItem1);
        }
    });

    $(document).on("focusout", "#grdWorkFlow .k-header input[type='text']", function (e) {
        objChangeFlag = true;
        var isChanged = false;
        if ((e.target.value).trim() == '' || (e.target.value).trim() == "")
            isChanged = true;
        else {
            index = $.inArray((e.target.value).trim(), arr_ids);
            if (index != -1)
                isChanged = true;
            else {
                Warningmessage('Invalid ManagerID');
                isChanged = false;
            }
        }
    });

    $(document).on("change", "#grdWorkFlow .k-header input[type='text']", function (e) {
        objChangeFlag = true;
        var isChanged = false;
        if ((e.target.value).trim() == '' || (e.target.value).trim() == "")
            isChanged = true;
        else {
            index = $.inArray((e.target.value).trim(), arr_ids);
            if (index != -1)
                isChanged = true;
            else {
                Warningmessage('Invalid ManagerID');
                isChanged = false;
            }
        }
    });
});

function additionalParam()
{
    return {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
    }
}


function grdApprovalDataBound() {
    var grdUserDetails = $("#grdApprovalData").data("kendoGrid");
    if (grdUserDetails._data.length == 0) {
        $("#ApprovalData").hide();
        $("#gridHeaderTxt").hide();
    }
    else {
        $("#ApprovalData").show();
        $("#gridHeaderTxt").show();
    }
}


function setRowChanged(rowItem1) {
    rowItem1.IsChanged = true;
}

function onWorkFlowPageLoad() {
    if (IsWorkFlowDataCustomized == "True") {
        $("#customBtn").prop("checked", true)
        $("#uploadWfDataTab").show();
        $("#btnSave").show();
        $('[id*=deleteBtn]').show();
        $('[id*=AppMgr]').show();
        $("#wrkflwlevel").hide();
    }
    else {
        $("#customBtn").prop("checked", false);
        $("#uploadWfDataTab").hide();
        $("#btnSave").hide();
        $('[id*=deleteBtn]').hide();
        $('[id*=AppMgr]').hide();
        $("#wrkflwlevel").show();
    }
}

function checkValid(value, empID) {
    if (value == '' || value == "") {
        isValid = true;
    }
    else {
        if (value == empID) {

            Warningmessage('Cannot assign Employee');
            isValid = false;

        }
        else {

            index = $.inArray(value, arr_ids);
            if (index != -1) {
                isValid = true;

            }
            else {

                Warningmessage('Invalid ManagerID');
                isValid = false;

            }
        }
    }

    return isValid;
}

$(function () {
    $.getJSON("../WorkFlow/GetEmpId", function (values) {

        var empIds = values;
        array_parsed = JSON.parse(empIds);

        $(array_parsed).each(function () {
            arr_ids.push(this.EmployeeID);
        });

    });
});

function AdditionalParameterInfo() {
    var ddlProcess = $('#ddlProcess').data("kendoDropDownList");
    return {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
        ProcessNum: defaultProcessNum
    }
}



function grdData(rowItem, columnNumber) {
    var empBtn = $('input:radio[name=empShow]:checked').val();
    var CustomizeBtn = $('input:radio[name=CustomizeBtn]:checked').val();

    if (CustomizeBtn == "Custom" && empBtn == "EmployeeID") {
        var columnValue = rowItem["EmpID_" + columnNumber];
        ((columnValue == "FALSE") && (columnNumber != 1)) ? $(".disableBtn" + columnNumber).attr('disabled', true) : ((columnValue != "FALSE") && (columnNumber != 1)) ? $(".disableBtn" + columnNumber).removeAttr('disabled', true) : '';
        if ((columnValue == null) || (columnValue == "FALSE"))
            columnValue = "";
        return '<input id="txt_' + columnNumber + '" type="text" value="' + columnValue + '" onkeypress = "return ValidateAllTextBoxes(event);" />';
    }
    else {
        var columnValue = ((empBtn == "EmployeeName")) ? rowItem["EmpName_" + columnNumber] : rowItem["EmpID_" + columnNumber];
        if ((columnValue == null) || (columnValue == "FALSE"))
            columnValue = "";
        return columnValue;
    }
}

function ddlProcessDataBound(e) {
    $("#ddlProcess").data("kendoDropDownList").select(function (listItem) {
        return listItem.Value == defaultProcessNum;
    })

}

function downloadApprovalData() {
    var form = $("<form action='../Workflow/GetApprovalDataTemplate' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />")
    form.appendTo("body");
    form.submit();
    form.remove();
}


$(document).on("click", "#btnWorkFlowExport", function () {
    var moduleNum = defaultProcessNum;
    var form = $("<form action='../Workflow/GetExportGridData' method='post'></form>")
    form.append("<input type='text' name='moduleNum' value='" + moduleNum + "' />")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />")
    form.appendTo("body");
    form.submit();
    form.remove();
});

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

function getApprovalDataErrorList() {
    $.ajax({
        url: '../Workflow/_GetApprovalDataErrorList',
        type: "GET",
        async: false,
        contentType: "application/json",
        processData: false,
        success: function (result) {
            $("#approvalData-Error").html(result);
            $('#approvalData-Error').modal('show');
            $("#approvalDataerrorFiles").html($("#errorFiles").html());
        }
    });
}

function LevelChanged(e) {
    removeItem();
  
    var grid = $("#grdWorkFlow").data("kendoGrid");
    if (grid.dataSource._filter != undefined) {
        clearFilterSort(true)
        $("#btnClearFilter").hide();
        $("#filter").show();
    }
    var ProcessNum = $("#ddlProcess").val();
    var wfLevel = $("#ddlWFLevels").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (ProcessNum) {
        if (wfLevel != "") {
            $.ajax({
                url: '../WorkFlow/ReloadGrid',
                type: "post",
                data:({ __RequestVerificationToken:token, processNum: ProcessNum, Level: wfLevel, LevelBasedWFData: LevelBasedWFData }),
                success: function (result) {
                    objChangeFlag = false;
                    $("#uploadWfDataTab").hide();
                    $("#btnSave").hide();
                    $('[id*=deleteBtn]').hide();
                    $('[id*=AppMgr]').hide();
                    $("#employeeID").prop("checked", true);
                    $("#wrkflwlevel").show();
                    grid.dataSource.read();
                    Successmessage(result);
                }
            });
        }
    }
    else {
        $("#customBtn").prop("checked", true);
        return false;
    }
}

function getApprovalErrorExport() {
    var form = $("<form action='../Workflow/GetApprovalDataErrorExport' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />")
    form.appendTo("body");
    form.submit();
    form.remove();
}

function GetWorkFlowErrorCount() {
    $.ajax({
        url: '../Workflow/GetWorkFlowDataErrorCount',
        type: "POST",
        data: ({ __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()}),
        success: function (result) {
            if (result > 0 || isErrorTemplate == true) {
                getApprovalDataErrorList();
                if (result > 0)
                    $("#workflowErrorListLink").show();
                isErrorTemplate = false;
            }
            else {
                $("#workflowErrorListLink").hide();
            }
        }
    });
}

function ValidateAllTextBoxes(event) {
    var regex = new RegExp("^[a-zA-Z0-9]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}


function grdApprovalErrorRecordDataBound() {
    var grdApprovalDataError = $("#grdApprovalDataError").data("kendoGrid");
    if (grdApprovalDataError._data.length == 0) {
        $("#divUserDataError").hide();
    }
    else {
        $("#divUserDataError").show();
    }
}

function WorkFlowGridDataBound() {
    var workflowGridDetails = $("#grdWorkFlow").data("kendoGrid");
    if (workflowGridDetails.dataSource._data.length == 0) {
        var colCount = $("#grdWorkFlow").find('th').length;
        $("#grdWorkFlow").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center;background-color:#6686C4;color:white !important;><b style="background-color:#6686C4 !important;">No Results Found!</b></td></tr>');        
    }
}

function removeItem() {
    var ddl = $("#ddlWFLevels").data("kendoDropDownList");
    var oldData = ddl.dataSource.data();
    ddl.setOptions({ optionLabel: false });
    ddl.refresh();
}
function addSelectItem() {
    var ddl = $("#ddlWFLevels").data("kendoDropDownList");
    var oldData = ddl.dataSource.data();
    ddl.setOptions({ optionLabel: "-select-" });
    ddl.refresh();
}