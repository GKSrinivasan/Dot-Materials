var uploader;
var isErrorCorrection = false;
var clearData = false;
$(document).ready(function () {
    $("#pdata").attr("disabled", true);
    $("#infoTxt1").show();
    uploader = new plupload.Uploader({
        runtimes: 'html5',
        drop_element: 'drop-target',
        browse_button: 'pickfiles',
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
                    if(uploader.files.length > 0)
                        $("#pdata").removeAttr("disabled");
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
                if (uploader.total.uploaded == 0)
                {
                    $('#errorFiles').children().remove();
                    var loadingPanel = $("#ajaxLoadingPanel");
                    loadingPanel.height($(window).height());
                    loadingPanel.width($(window).width());
                    loadingPanel.css('top', $(window).scrollTop());
                    loadingPanel.css('left', $(window).scrollLeft());
                    loadingPanel.show();
                }
                if ((uploader.total.uploaded + 1) == uploader.files.length) {
                    uploader.settings.multipart_params = { isLastFile: 'yes', __RequestVerificationToken:$('input[name="__RequestVerificationToken"]').val() };
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
                        fileProgress.css("background-color", "red");
                        if (result.Message == "Invalid format or Password protected") {
                            var errorFiles = "<div style='word-wrap: break-word;margin-left:2% !important;margin-right:2% !important;'><div class='row'>" + file.name + "<br/> Invalid format or Password protected</div></div><br/>";
                            $("#errorFiles").append(errorFiles);
                        }
                        else {
                            var errorFiles = "<div style='word-wrap: break-word;margin-left:2% !important;margin-right:2% !important;'><div class='row'>" + file.name + "<br/>" + result.Message + "</div></div><br/>";
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
                callworkForceTileData();
              getEmployeeDataErrorCount();
                $("#grdEmployeeData").data("kendoGrid").dataSource.read();
                $("#ajaxLoadingPanel").hide();
                $("#gridHeaderTxt").show();
                $("#grdEmployeeData").show();
               // var a = result.Message;
               // Successmessage(result.Message);
            },
            Error: function (up, err) {
                var fileProgress = $("#" + err.file.id).find(".progress-bar");
                var filePercent = 100;
                fileProgress.css("width", filePercent + "%");
                fileProgress.text(filePercent + "%");
                fileProgress.attr("aria-valuenow", filePercent);
                //Errormessage("Invalid file");
            }
        }
    });
    uploader.init();
    callworkForceTileData();
    if (clearData) {
        $("#UploadData").trigger('click');
        $("#liErrorMessage").css("display", "none");
    }

    $("#UploadData").click(function () {
        $(window).scrollTop(0);
    });
    $("#ChooseFields").click(function () {
        $(window).scrollTop(0);
    });
    $("#idDataCorrect").click(function () {
        $(window).scrollTop(0);
    });

});


function checkDateFormat(value) {
    var DateFormat = ('MM/dd/yyyy').toLowerCase();
    var isValid;
    switch (DateFormat) {
        case "dd-mmm-yyyy":
            var regex = new RegExp("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            isValid = regex.test(value);
            break;
        case "mm/dd/yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "mmm/dd/yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "dd-mm-yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "mmm dd, yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "dd/mm/yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "dd/mmm/yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "mm-dd-yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "mmm-dd-yyyy":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "yyyy/mm/dd":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "yyyy/mmm/dd":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "yyyy-mm-dd":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        case "yyyy-mmm-dd":
            var regex = new RegExp("^[0-9]{2}/[0-9]{2}/[0-9]{4}$");
            isValid = regex.test(value);
            break;
        default:
            isValid = false;
    }
    return isValid;
}

function downloadEmployeeData() {
    var form = $("<form action='../Workforce/GetEmployeeDataTemplate' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

function getAllEmployeesExport() {
    var value = 'NO';
    var form = $("<form action='../Workforce/GetWorkForceUploadedData' method='post'></form>")
    form.append("<input type='text' name='isMeritEligible' value='" + value + "' />");
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

function getMeritEligibleEmployeesExport() {
    var value = 'YES';
    var form = $("<form action='../Workforce/GetWorkForceUploadedData' method='post'></form>")
    form.append("<input type='text' name='isMeritEligible' value='" + value + "' />");
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

function getSelectedFieldsList() {
    $.ajax({
        url: '../WorkForce/_GetTemplateSelectedFields',
        type: "GET",
        async: false,
        contentType: "application/json",
        processData: false,
        success: function (result) {
            $("#workforce-fields").html(result);
            $('#workforce-fields').modal('show');
        }
    });
}

//function callProcessData() {
//    $.ajax({
//        url: '../WorkForce/_GetErrorList',
//        type: "GET",
//        async: false,
//        contentType: "application/json",
//        processData: false,
//        success: function (result) {
//            $("#workforce-error").html(result);
//            $('#workforce-error').modal('show');
//        }
//    });
//}

function callUploadData(e) {
    $.ajax({
        url: '../WorkForce/_UploadData',
        type: "GET",
        async: false,
        contentType: "application/json",
        processData: false,
        success: function (result) {
            $("#uploadData").html(result);
        }
    });
}


function onclick_employeesearch(e) {
    var selectedEmp = e.item[0].innerText;
    selectedEmpID = selectedEmp.split('-')[0].trim();
    $("#hdnSelectedValue").val(selectedEmpID);
    callEmployeeSearch(selectedEmpID);

}

function callEmployeeSearch(selectedEmpID) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "../Workforce/_EmployeeDetails",
        data: { __RequestVerificationToken: token, employeeID: selectedEmpID },
        async: true,
        success: function (result) {
            $("#divEmployeeDetails").html(result);
        }
    })
}

function AdditionalParam(controlId) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    return {
        __RequestVerificationToken: token,
        ColumnName: controlId.id,
    }
}

function addOrUpdateEmployeeDetails() {
    $("#hdnNewSalary").val(calculateNewSalary());
    $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
    $("#hdnNewCompRatio").val(calculateNewCompRatio());
    currentHrlyExceedValidation();
    NewHrlyRateExceedValidation();
    callValidationRules();
    compRatioExceedValidation();
    employeeIDDuplicateValidation();
    if (isValidEmployeeData()) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var data = getEmployeeData();
        $.ajax({
            url: '../WorkForce/AddorUpdateEmployeeDetails',
            type: "POST",
            data: { __RequestVerificationToken:token, data:data },
            success: function (result) {
                Successmessage("Added Successfully");
                callworkForceTileData();
                $("#hdnSelectedValue").val('');
                $("#idDataCorrect").trigger('click');
            }
        });
    }
}

function addOrUpdateEmployeeErrorDetails() {
    callValidationRules(); 
    if (isValidEmployeeData()) {
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
        var data = getEmployeeData();
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '../WorkForce/AddorUpdateEmployeeDetails',
            type: "POST",
            data: { __RequestVerificationToken: token, data: data },
            success: function (result) {
                Successmessage("Added Successfully");
                callworkForceTileData();
                getEmployeeDataErrorCorrection(dataCorrection.ErrorType);
                getEmployeeDataErrorCount();
            }
        });
    }
}

function getEmployeeData() {
    return Data = {
        EmployeeID: $('#EmployeeID').val(),
        FirstName: $('#FirstName').val(),
        MiddleName: $('#MiddleName').val(),
        LastName: $('#LastName').val(),
        PreferredName: $('#PreferredName').val(),
        Gender: $('#Gender').val(),
        JobCode: $('#JobCode').val(),
        JobTitle: $('#JobTitle').val(),
        CurrentGrade: $('#CurrentGrade').val(),
        BusinessUnit: $('#BusinessUnit').val(),
        Function: $('#Function').val(),
        Department: $('#Department').val(),
        EmployeeClass: $('#EmployeeClass').val(),
        PayrollStatus: $('#PayrollStatus').val(),
        FLSAStatus: $('#FLSAStatus').val(),
        EmployeeStatus: $('#EmployeeStatus').val(),
        FTE: $('#FTE').val(),
        ActualWorkHours: $('#ActualWorkHours').val(),
        WorkHours: $('#TotalWorkHours').val(),
        WorkCountry: $('#WorkCountry').val(),
        WorkLocation: $('#WorkLocation').val(),
        HireDate: $('#HireDate').val(),
        TerminationDate: $('#TerminationDate').val(),
        SupervisorID: $('#SupervisorID').val(),
        EmailAddress: $('#EmailAddress').val(),
        PayCurrency: $('#PayCurrency').val(),
        LastPayChangeDate: $('#LastPayChangeDate').val(),
        LastPayChangeReason: $('#LastPayChangeReason').val(),
        CurrentHourlyRate: $('#CurrentHourlyRate').val(),
        CurrentAnnalizedSalary: $('#CurrentAnnualizedSalary').val(),
        CurrentAnnualSalary: $('#CurrentAnnualSalary').val(),
        PayrollSource: $('#PayrollSource').val(),
        SalaryMin: $('#SalaryMin').val(),
        SalaryMid: $('#SalaryMid').val(),
        SalaryMax: $('#SalaryMax').val(),
        MeritProrationDate: $('#MeritProrationDate').val(),
        MeritProrationFactor: $('#MeritProrationFactor').val(),
        MeritPerformanceRating: $('#MeritPerformanceRating').val(),
        MeritEligible: $('#MeritEligible').val(),
        MeritPct: $('#MeritPct').val(),
        MeritAmount: $('#MeritAmount').val(),
        MeritIncreaseGuideline: $('#MeritIncreaseGuideline').val(),
        MeritEffectiveDate: $('#MeritEffectiveDate').val(),
        LumpSumPct: $('#LumpSumPct').val(),
        LumpSumAmount: $('#LumpSumAmount').val(),
        LumpsumEffectiveDate: $('#LumpsumEffectiveDate').val(),
        PromotionEligible: $('#PromotionEligible').val(),
        PromotionPct: $('#PromotionPct').val(),
        PromotionAmount: $('#PromotionAmount').val(),
        PromoteTo: $('#PromoteTo').val(),
        LastPromotionDate: $('#LastPromotionDate').val(),
        PromotionEffectiveDate: $('#PromotionEffectiveDate').val(),
        AdjustmentEligible: $('#AdjustmentEligible').val(),
        AdjustmenPct: $('#AdjustmentPct').val(),
        AdjustmentAmount: $('#AdjustmentAmount').val(),
        AdjustmentEffectiveDate: $('#AdjustmentEffectiveDate').val(),
        NewSalary: $("#hdnNewSalary").val(),
        NewHourlyRate: $("#hdnNewHrlyRate").val(),
        NewComparatio: $("#hdnNewCompRatio").val(),
        Comparatio: $("#hdnCompRatio").val(),
        MoreInfo1: $('#MoreInfo1').val(),
        MoreInfo2: $('#MoreInfo2').val(),
        MoreInfo3: $('#MoreInfo3').val(),
        MoreInfo4: $('#MoreInfo4').val(),
        MoreInfo5: $('#MoreInfo5').val(),
        MoreInfo6: $('#MoreInfo6').val(),
        MoreInfo7: $('#MoreInfo7').val(),
        MoreInfo8: $('#MoreInfo8').val(),
        MoreInfo9: $('#MoreInfo9').val(),
        MoreInfo10: $('#MoreInfo10').val(),
        MoreInfo11: $('#MoreInfo11').val(),
        MoreInfo12: $('#MoreInfo12').val(),
        MoreInfo13: $('#MoreInfo13').val(),
        MoreInfo14: $('#MoreInfo14').val(),
        MoreInfo15: $('#MoreInfo15').val(),
        MoreInfo16: $('#MoreInfo16').val(),
        MoreInfo17: $('#MoreInfo17').val(),
        MoreInfo18: $('#MoreInfo18').val(),
        MoreInfo19: $('#MoreInfo19').val(),
        MoreInfo20: $('#MoreInfo20').val(),
        MoreInfo21: $('#MoreInfo21').val(),
        MoreInfo22: $('#MoreInfo22').val(),
        MoreInfo23: $('#MoreInfo23').val(),
        StagingNumber: dataCorrectionConstants.StagingNumber,
        BudgetProrationFactor: $("#hdnBudgetProrationFactor").val(),
    }
}

function allowDecimalNumberandPCTasInput(e, decimalSeparator, control)
{
    var arrayKeyCodes = [46, 8, 9, 27, 13,45,53];
    if (decimalSeparator == ".") arrayKeyCodes = [46, 8, 9, 27, 13, 110,45,190,53];
    var value = String.fromCharCode(e.keyCode);
    if (value != "n") {
        if ($.inArray(e.keyCode, arrayKeyCodes) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode > 35 && e.keyCode < 40 && e.keyCode != 36 && e.keyCode != 38 && e.keyCode!=39) ||
            //Allows decimal separator based on culture
            (e.key == decimalSeparator)
            ) {
            //Prevent More than one decimal
            //if (e.keyCode == 190 || e.keyCode == 110 || e.keyCode == 46) {
            //    var patt1 = new RegExp("\\.");
            //    var ch = patt1.exec(control.value);
            //    if (ch == ".") {
            //        e.preventDefault();
            //    }
            //}
            // let it happen, don't do anything
            return;
        }
    }
    // Ensure that it is a number and stop the keypress
    //if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
    //    e.preventDefault();
    var key_codes = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 0, 8];

    if (!($.inArray(e.which, key_codes) >= 0)) {
        e.preventDefault();
    }    
}


function allowDecimalNumberOnlyInput(e, decimalSeparator, control) {
    // Allow: backspace, delete, tab, escape, enter and .
    var arrayKeyCodes = [46, 8, 9, 27, 13];
    if (decimalSeparator == ".") arrayKeyCodes = [46, 8, 9, 27, 13, 110, 190];
    if ($.inArray(e.keyCode, arrayKeyCodes) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40) ||
        //Allows decimal separator based on culture
        (e.key == decimalSeparator)
        ) {
        //Prevent More than one decimal
        if (e.keyCode == 190 || e.keyCode == 110 || e.keyCode == 46) {
            var patt1 = new RegExp("\\.");
            var ch = patt1.exec(control.value);
            if (ch == ".") {
                e.preventDefault();
            }
        }
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }

}

//function chooseFieldsFunGrpNames()
//{
//    var groupNames = $("#grdTemplateColumns .k-grouping-row").find('.k-reset');
//    for(var i=0;i<groupNames.length;i++)
//    {        
//        var actualGroupValue = groupNames[i].innerText;        
//        var suffix = actualGroupValue.substring(19);
//        groupNames[i].innerHTML ='<a tabindex=\"-1\" class=\"k-icon k-i-collapse\" href=\"#\"></a>'+ suffix;
//    }
//}

function selectedFieldsFunGrpNames() {
    var groupNames = $("#grdSelectedTemplateDetails .k-grouping-row").find('.k-reset');
    for (var i = 0; i < groupNames.length; i++) {
        var actualGroupValue = groupNames[i].innerText;
        var suffix = actualGroupValue.substring(19);
        groupNames[i].innerHTML = '<a tabindex=\"-1\" class=\"k-icon k-i-collapse\" href=\"#\"></a>' + suffix;
    }
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

function dataCorrectTabClick() {
    saveFor = "DataCorrection"
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

function additionalParameterInfo()
{
    return {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
    }
}



$(document).on("click", ".file-remove .fa-times", function (e) {
    var fileid = $(this).data("fileid");
    uploader.removeFile(fileid);
    $(this).parents(".file-area").remove();
    if (uploader.files.length == 0)
        $("#pdata").attr("disabled", true);
    else
        $("#pdata").removeAttr("disabled");
});

$(document).on("click", "#pdata", function (e) {
    uploader.start();
    return false;
});


$(document).on("click", "[id$=btnDelete]", function (e) {
    var a = e.keyCode;
    var charCode = e.charCode || e.keyCode || e.which;
    if (charCode == 27) {
        alert("Escape is not allowed!");
        return false;
    }
    var grdData = $("#grdEmployeeData").data("kendoGrid");
    var rowItem = $(this).closest("tr");
    var rowIndex = $(rowItem).index();
    var rowData = grdData._data[rowIndex];
    var xmlProcessNum = rowData.XMLProcessNum;
    if (e.keyCode != 27)
    var status = showConfirm(e,"Do you want to delete?")
    if (status) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: "../Workforce/DeleteEmployeeTemplateData",
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

$(document).on("click", "#idChooseFields", function (e) {
    $.ajax({
        url: '../WorkForce/_ChooseFields',
        data: { isWizard: false },
        type: "GET",
        async: false,
        contentType: "application/json",
        processData: false,
        success: function (result) {
            $("#chooseFields").html(result);
            $("#btnWorkForceSave").show();
            $("#idClearAllData").show();
            $("#btnDataCorrectionEmployee").hide();
            $("#btnClearAllData").hide();
            $('#errorFiles').children().remove();
        }
    });
});




$(document).on("click", "#idUploadData", function (e) {
    $.ajax({
        url: '../WorkForce/_UploadData',
        type: "GET",
        async: false,
        contentType: "application/json",
        processData: false,
        success: function (result) {
            $("#uploadData").html(result);
            $("#idClearAllData").show();
            $("#btnWorkForceSave").show();
            $("#btnDataCorrectionEmployee").hide();
            $("#btnClearAllData").hide();
            $('#errorFiles').children().remove();
        }
    });
});

$(document).on("click", "#ChooseFields", function (e) {
    $("#infoTxt2").hide();
    $("#infoTxt1").show();
    $("#btnWorkForceSave").show();
    $("#btnDataCorrectionEmployee").hide();
    $('#errorFiles').children().remove();
    $("#idClearAllData").hide();
});

$(document).on("click", "#UploadData", function (e) {
    $("#infoTxt1").hide();
    $("#infoTxt2").hide();
    $("#btnWorkForceSave").hide();
    $("#btnDataCorrectionEmployee").hide();
    $('#errorFiles').children().remove();
    $("#idClearAllData").hide();
});

$(document).on("click", "#liErrorMessage", function (e) {
    $("#infoTxt1").hide();
    $("#infoTxt2").hide();
    $("#hdnErrorType").val('');
    $("#btnWorkForceSave").hide();
    $("#btnDataCorrectionEmployee").hide();
    $("#btnErrorDataCorrectionEmployee").hide();
    $("#btnDataCorrectionEmployee").hide();
    $("#idClearAllData").hide();
});

$(document).on("click", "#idDataCorrect", function (e) {
    $("#infoTxt1").hide();
    $("#infoTxt2").show();
    var errorType = $("#hdnErrorType").val();
    callDataCorrectionPage(errorType);
});


function callDataCorrectionPage(errorType) {
    $.ajax({
        url: '../WorkForce/_DataCorrection',
        type: "GET",
        async: false,
        data: { errorType: errorType },
        contentType: "application/json",
        processData: false,
        success: function (result) {
            if (errorType != "")
                $("#idClearAllData").hide();
            else
                $("#idClearAllData").show();
            $("#hdnErrorType").val('');
            $("#dataCorrect").html(result);
            $("#btnWorkForceSave").hide();
            $("#idClearAllData").show();
            $("#btnErrorDataCorrectionEmployee").hide();
            $("#btnDataCorrectionEmployee").show();
            $('#errorFiles').children().remove();
        }
    });
}

$(document).on("click", "#btnWorkForceSave", function (e) {
    var postData = [];
    $("[id$=chkFields]:checked").each(function () {
        postData.push($(this).attr('data-fieldname'));
    });

    if (postData.length > 0) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var jsonString = JSON.stringify(postData);
        var jsonData = JSON.parse(jsonString);
        $.ajax({
            url: '../WorkForce/UpdateChooseFields',
            type: "post",
            data: ({ __RequestVerificationToken: token, selectedFields: jsonData }),
            success: function (message) {
                Successmessage(message);
            }
        });
        postData = [];
    }
    objChangeFlag = false;

});

$(document).on("keyup", "[id$=EmployeeID]", function (e) {
    ((this.value == "" || this.value == undefined) && e.keyCode == 9) ? '' : employeeIDValidation(e);
});
$(document).on("keyup", "[id$=FirstName]", function (e) {
    ((this.value == "" || this.value == undefined) && e.keyCode == 9) ? '' : firstNameValidation(e);
});
$(document).on("keyup", "[id$=MiddleName]", function (e) {
    middleNameValidation(e);
});
$(document).on("keyup", "[id$=LastName]", function (e) {
    ((this.value == "" || this.value == undefined) && e.keyCode == 9) ? '' : lastNameValidation(e);
});
$(document).on("keyup", "[id$=Gender]", function (e) {
    genderValidation(e);
});
$(document).on("keyup", "[id$=JobCode]", function (e) {
    jobCodeValidation(e);
});
$(document).on("keyup", "[id$=JobTitle]", function (e) {
    jobTitleValidation(e);
});
$(document).on("keyup", "[id$=CurrentGrade]", function (e) {
    currentGradeValidation(e);
});
$(document).on("keyup", "[id$=BusinessUnit]", function (e) {
    businessUnitValidation(e);
});
$(document).on("keyup", "[id$=Function]", function (e) {
    empFunctionValidation(e);
});
$(document).on("keyup", "[id$=Department]", function (e) {
    departmentValidation(e);
});
$(document).on("keyup", "[id$=EmployeeClass]", function (e) {
    employeeClassValidation(e);
});
//$(document).on("keyup", $("input[name='PayrollStatus_input']"), function (e) {
//    payrollStatusValidation(e);
//});
//$(document).on("keyup","[id$=PayrollStatus]", function (e) {
//    payrollStatusValidation(e);
//});
$(document).on("keyup", "[id$=FLSAStatus]", function (e) {
    flsaStatusValidation(e);
});
$(document).on("keyup", "[id$=EmployeeStatus]", function (e) {
    employeeStatusValidation(e);
});
$(document).on("keyup", "[id$=FTE]", function (e) {
    fteValidation(e);
});
$(document).on("keyup", "[id$=WorkCountry]", function (e) {
    workCountryValidation(e);
});
$(document).on("keyup", "[id$=WorkLocation]", function (e) {
    worklocationValidation(e);
});

$(document).on("keyup", "[id$=SupervisorID]", function (e) {
    ((this.value == "" || this.value == undefined) && e.keyCode == 9) ? '' : supervisorIDValidation(e);
});
$(document).on("keyup", "[id$=EmailAddress]", function (e) {
    emailAddressValidation(e);
});
$(document).on("keyup", "[id$=PayCurrency]", function (e) {
    ((this.value == "" || this.value == undefined) && e.keyCode == 9) ? '' : payCurrencyValidation(e);
});
$(document).on("keyup", "[id$=LastPayChangeReason]", function (e) {
    lastPayChangeReasonValidation(e);
});
$(document).on("keyup", "[id$=PayrollSource]", function (e) {
    payrollSourceValidation(e);
});
$(document).on("keyup", "[id$=MeritPerformanceRating]", function (e) {
    meritPerformanceRatingValidation(e);
});
$(document).on("keyup", "[id$=MeritEligible]", function (e) {
    meritEligibleValidation(e);
});
$(document).on("keyup", "[id$=MeritIncreaseGuideline]", function (e) {
    meritIncreaseGuidelineValidation();
});
$(document).on("keyup", "[id$=PromotionEligible]", function (e) {
    promotionEligibleValidation(e);
});
$(document).on("keyup", "[id$=PromoteTo]", function (e) {
    promotetoValidation(e);
});
$(document).on("keyup", "[id$=MeritPct]", function (e) {
    meritPctValidation(e);
});
$(document).on("change", "#MeritPerformanceRating", function (e) {
    var value = this.value;
    var RatingRangeText;
    for (var i = 0; i < RatingRange.length; i++) {
        if (RatingRange[i].Text == value) {
            RatingRangeText = RatingRange[i].Value;
        }
    }
    $("#MeritIncreaseGuideline").val(RatingRangeText);
   
});
$(document).on("keydown", "[id$=MeritPct]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=LumpSumPct]", function (e) {
    lumpsumPctValidation(e);
});
$(document).on("keydown", "[id$=LumpSumPct]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=PromotionPct]", function (e) {
    promotionPctValidation(e);
});
$(document).on("keydown", "[id$=PromotionPct]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=AdjustmentPct]", function (e) {
    adjustmenPctValidation(e);
});
$(document).on("keydown", "[id$=AdjustmentPct]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});
// Compa Ratio
$(document).on("keyup", "[id$=CompaRatio]", function (e) {
    compaRatioValidation(e);
});
$(document).on("keydown", "[id$=CompaRatio]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
});
//
$(document).on("keypress", "[id$=MeritIncreaseGuideline]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberandPCTasInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=MeritAmount]", function (e) {
    meritamountValidation(e);
});

$(document).on("keydown", "[id$=MeritAmount]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=LumpSumAmount]", function (e) {
    lumpsumAmountValidation(e);
});

$(document).on("keydown", "[id$=LumpSumAmount]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=PromotionAmount]", function (e) {
    promotionAmountValidation(e);
});

$(document).on("keydown", "[id$=PromotionAmount]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=AdjustmentAmount]", function (e) {
    adjustmentAmountValidation(e);
});

$(document).on("keydown", "[id$=AdjustmentAmount]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=TotalWorkHours]", function (e) {
    workHoursValidation(e);
});

$(document).on("keyup", "[id$=ActualWorkHours]", function (e) {
    actualworkhoursValidation(e);
});

$(document).on("keydown", "[id$=TotalWorkHours]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);

    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});
$(document).on("keydown", "[id$=ActualWorkHours]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);

   // var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    //return false;
    //}
});

$(document).on("keyup", "[id$=CurrentHourlyRate]", function (e) {
    currentHourlyRateValidation(e);
});

$(document).on("keydown", "[id$=CurrentHourlyRate]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});


$(document).on("keyup", "[id$=CurrentAnnualizedSalary]", function (e) {
    ((this.value == "" || this.value == undefined) && e.keyCode == 9) ? '' : currentAnnualizedSalaryValidation(e);
});

$(document).on("keydown", "[id$=CurrentAnnualizedSalary]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=CurrentAnnualSalary]", function (e) {
    currentAnnualsalaryValidation(e);
});

$(document).on("keydown", "[id$=CurrentAnnualSalary]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=SalaryMin]", function (e) {
    salaryMinValidation(e);
});

$(document).on("keydown", "[id$=SalaryMin]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=SalaryMid]", function (e) {
    salaryMidValidation(e);
});

$(document).on("keydown", "[id$=SalaryMid]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=SalaryMax]", function (e) {
    salaryMaxValidation(e);
});

$(document).on("keydown", "[id$=SalaryMax]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

$(document).on("keyup", "[id$=MeritProrationFactor]", function (e) {
    meritProrationFactorValidation(e);
});

$(document).on("keydown", "[id$=MeritProrationFactor]", function (e) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    //var code = e.keycode || e.which
    //if (code == 13 || code == 9) {
    //    $(this).trigger("change");
    //    return false;
    //}
});

//$(document).on("keyup", "#HireDate", function (e) {
//    hiredateValidation(e);
//});

//$(document).on("keydown", "#HireDate", function (e) {
//    var code = e.keycode || e.which
//    if (code == 13 || code == 9) {
//        $(this).trigger("change");
//        return false;
//    }
//});

$(document).on("change", "#HireDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
       
    }
    hiredateValidation(e);
});
$(document).on("keyup", "#HireDate", function (e) {
    hiredateValidation(e);
});
$(document).on("keyup", "#TerminationDate", function (e) {
    terminationDateValidation(e);
});

//$(document).on("keydown", "#TerminationDate", function (e) {
//    var code = e.keycode || e.which
//    if (code == 13 || code == 9) {
//        $(this).trigger("change");
//        return false;
//    }
//});

$(document).on("change", "#TerminationDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
});
$(document).on("change", "#MeritProrationDate", function (e) {
    meritProrationDateValidation(e);
});

$(document).on("keyup", "#LastPayChangeDate", function (e) {
    lastPayChangeDateValidation(e);
});

//$(document).on("keydown", "#LastPayChangeDate", function (e) {
//    var code = e.keycode || e.which
//    if (code == 13 || code == 9) {
//        $(this).trigger("change");
//        return false;
//    }
//});

$(document).on("change", "#LastPayChangeDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
});

$(document).on("keyup", "#MeritProrationDate", function (e) {
    meritProrationDateValidation(e);
});

$(document).on("keydown", "#MeritProrationDate", function (e) {
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        setTimeout(function () {
            $("#MeritProrationFactor").focus();
        }, 0);
    }
});

$(document).on("change", "#MeritProrationDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
});

$(document).on("keydown", "#MeritEffectiveDate", function (e) {
    var code = e.keycode || e.which
if (code == 13 || code == 9) {
    setTimeout(function () {
        $("#LumpSumPct").focus();
    }, 0);
}
});

$(document).on("keyup", "#MeritEffectiveDate", function (e) {
    meritEffectiveDateValidation(e);
});

$(document).on("change", "#MeritEffectiveDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
   
});

$(document).on("keyup", "#LumpsumEffectiveDate", function (e) {
    lumpsumEffectiveDateValidation(e);
});

$(document).on("keydown", "#LumpSumEffectiveDate", function (e) {
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        setTimeout(function () {
            $("#PromotionEligible")[0].focus();
        }, 0);
    }
});

$(document).on("change", "#LumpsumEffectiveDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
});

$(document).on("keyup", "#LastPromotionDate", function (e) {
    lastPromotionDateValidation(e);
});

$(document).on("keydown", "#LastPromotionDate", function (e) {
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        setTimeout(function () {
            $("#PromotionEffectiveDate").focus();
        }, 0);
    }
}); 

$(document).on("change", "#LastPromotionDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
});


$(document).on("keyup", "#PromotionEffectiveDate", function (e) {
    promotionEffectiveDateValidation(e);
});

$(document).on("keyup", "#PreferredName", function (e) {
    preferredNameValidation(e);
});

$(document).on("keydown", "#PromotionEffectiveDate", function (e) {
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        setTimeout(function () {
            $("#AdjustmentEligible").focus();
        }, 0);
    }
});

$(document).on("change", "#Gender", function (e) {
    genderValidation(e);
});

$(document).on("change", "#PromotionEffectiveDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
});

$(document).on("keyup", "#AdjustmentEffectiveDate", function (e) {
    adjustmentEffectiveDateValidation(e);
});

//$(document).on("keydown", "#AdjustmentEffectiveDate", function (e) {
//    var code = e.keycode || e.which
//    if (code == 13 || code == 9) {
//        $(this).trigger("change");
//        return false;
//    }
//});

$(document).on("change", "#PayCurrency", function () {
    if (!ruleConfiguration.IsMultiCurrencyEnable && $('#error_PayCurrency').val() == "")
    {
        var payCurrencyValue = $("#PayCurrency").val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: "../WorkForce/PayCurrencyValidation",
            type: "post",
            data: { __RequestVerificationToken: token, payCurrencyCode: payCurrencyValue },
            success: function (result) {
                $('#error_PayCurrency').html(result);
            }
        });
}
});

$(document).on("change", "#AdjustmentEffectiveDate", function (e) {
    var isValid = checkDateFormat(e.target.value);
    if (!isValid) {
        $("#HireDate").val('');
    }
});

$(document).on("click", "#btnClearAllData", function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../WorkForce/ClearAllData",
        type: "post",
        data: { __RequestVerificationToken: token},
        async: true,
        success: function (result) {
            if (result) {
                $("#divTileData").html('');
                $("#gridHeaderTxt").hide();
                $("#grdEmployeeData").hide();
                $("#idDataCorrect").trigger('click');
                $("#liErrorMessage").hide();
                $("#cleardata").hide();
                $("#btn-Cancel").trigger("click");
                Successmessage("Clear all data successfully ");
            }
        }
    });
});


function callworkForceTileData() {
    $.ajax({
        url: '../WorkForce/_GetTileData',
        type: "GET",
        async: false,
        contentType: "application/json",
        processData: false,
        success: function (result) {
            $("#divTileData").html(result);
        }
    });
}

function grdEmployeeLoadedDataBound() {
    var grdUserDetails = $("#grdEmployeeData").data("kendoGrid");
    if (grdUserDetails._data.length == 0) {
        $("#employeeData").hide();
        $("#gridHeaderTxt").hide();
    }
    else {
        $("#employeeData").show();
        $("#gridHeaderTxt").show();
    }
}

function getOrphanEmployeeInformation() {
   // var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../WorkForce/_GetOrphanEmployeeDetails',
        type: 'get',
       // data: { __RequestVerificationToken: token },
        success: function (result) {
            $("#dc-items").html(result);
            $("#dc-items").modal('show');
        }
    });
}

function additionalParamInfo()
{
    var token = $('input[name="__RequestVerificationToken"]').val();
    return {
        __RequestVerificationToken: token
    }
}

function unAssignedUserList() {
    var employeesName = [];
    var orphanEmployeeList = $("#grdOrphanEmployees").data("kendoGrid");
    var rows = orphanEmployeeList.tbody.children('tr');
    var data = orphanEmployeeList.dataSource.view();
    var dataLength = data.length;
    if (dataLength == 0) {
        $("#btnTopLevelManager").hide();
        $("#orphanEmployeeCheckBoxSelectAll").css("display", "none");
    }
    else {
        $.each(data, function (index, rowData) {
            if (rowData.IsChecked)
                $("#corporateUserList").append("<label>" + rowData.EmployeeName + "</label><br>")
            else
                $("#unAssigneduserList").append("<label>" + rowData.EmployeeName + "</label><br>")
        });
    }
}

$(document).on("change", "[id$=orphanEmployeeCheckBox]", function (e) {
    if (this.checked) {
        var grid = $("#grdOrphanEmployees").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var rowData = grid._data[rowIndex];
        rowData.IsChecked = true;
        $("#corporateUserList").children().remove();
        $("#unAssigneduserList").children().remove();
        grid.refresh();
        var checkboxLength = $("[id$=orphanEmployeeCheckBox]").length;
        var checkboxCheckedLength = $("[id$=orphanEmployeeCheckBox]:checked").length;
        if (checkboxLength == checkboxCheckedLength) {
            $("#orphanEmployeeCheckBoxSelectAll")[0].checked = true;
        }
        else {
            $("#orphanEmployeeCheckBoxSelectAll")[0].checked = false;
        }
    }
    else {
        if (this.checked != undefined) {
            var grid = $("#grdOrphanEmployees").data("kendoGrid");
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.IsChecked = false;
            $("#corporateUserList").children().remove();
            $("#unAssigneduserList").children().remove();
            grid.refresh();
            var checkboxLength = $("[id$=orphanEmployeeCheckBox]").length;
            var checkboxCheckedLength = $("[id$=orphanEmployeeCheckBox]:checked").length;
            if (checkboxLength == checkboxCheckedLength) {
                $("#orphanEmployeeCheckBoxSelectAll")[0].checked = true;
            }
            else {
                $("#orphanEmployeeCheckBoxSelectAll")[0].checked = false;
            }
        }
    }

});

$(document).on("change", "#orphanEmployeeCheckBoxSelectAll", function () {
    var grid = $("#grdOrphanEmployees").data("kendoGrid");
    if (this.checked) {
        $("[id$=orphanEmployeeCheckBox]").each(function () {
            this.checked = true;
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.IsChecked = true;
        });
        $("#corporateUserList").children().remove();
        $("#unAssigneduserList").children().remove();
        grid.refresh();
    }
    else {
        if (this.checked != undefined) {
            $("[id$=orphanEmployeeCheckBox]").each(function () {
                this.checked = false;
                var rowIndex = $(this).closest("tr").index();
                var rowData = grid._data[rowIndex];
                rowData.IsChecked = false;
            });
            $("#corporateUserList").children().remove();
            $("#unAssigneduserList").children().remove();
            grid.refresh();
        }
    }

});

$(document).on("click", "#btnTopLevelManager", function () {
    var grid = $("#grdOrphanEmployees").data("kendoGrid");
    var empJobNums = [];
    var length = grid.dataSource._data.length;
    for (var i = 0; i < length; i++) {
        var rowData = grid.dataSource._data[i];
        if (rowData.IsChecked == true) {
            empJobNums.push(rowData);
        }
    }
    //if (empJobNums.length > 0) {
    updateOrphanEmployeesManager(empJobNums);
    //}
    //else {
    //    if (confirm("If you click ok then all orphan employees assign to unassigned manager")) {
    //        return false;
    //    }
    //    updateOrphanEmployeesManager();
    //}

});


function updateOrphanEmployeesManager(empJobNums) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var jsonString = JSON.stringify(empJobNums);
    var jsonData = JSON.parse(jsonString);
    $.ajax({
        url: '../WorkForce/AssignEmployeeToCorporate',
        type: 'post',
        data: {
            __RequestVerificationToken: token,
            EmployeeJobNum: jsonData
        },
        success: function (result) {
            if (result > 0) {
                Successmessage("Successfully Assigned");
            }
            else {
                Successmessage("Assignment Failed");
            }
            $("#dc-items").modal('hide');
            bindErrorDataList();
        }

    });
}



function getEmployeeErrorListExport() {
    var form = $("<form action='../WorkForce/GetEmployeeDataErrorExport' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

function bindErrorDataList() {
    var grdErrorListGrid = $("#grdEmployeeDataError").data("kendoGrid");
    grdErrorListGrid.dataSource.read();
    $("#btnWorkForceSave").hide();
    $("#idClearAllData").hide();

}

function getEmployeeDataErrorCount() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../WorkForce/GetEmployeeDataErrorCount',
        type: "post",
        data: { __RequestVerificationToken: token },
        success: function (result) {
            if (result > 0) {
                $("#liErrorMessage").show();
                $("#idErrorMessage").trigger('click');
            }
            else {
                $("#liErrorMessage").hide();
                if ($("#errorFiles").children().length > 0) {
                    $("#invalidTemplates").html($("#errorFiles").html());
                    $('#divErrorFiles').modal('show');
                }
            }
        }
    });
}


$(document).on("change", "[id$=MeritEligible]", function (e) {
    meritEligibleValidation(e);
    $("#hdnNewSalary").val(calculateNewSalary());
    $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
    $("#hdnNewCompRatio").val(calculateNewCompRatio());
    $("#MeritAmount").val('');
    $("#MeritPct").val('');
    $("#LumpSumAmount").val('');
    $("#LumpSumPct").val('');

});

$(document).on("change", "[id$=PromotionEligible]", function (e) {
    promotionEligibleValidation(e);
    $("#hdnNewSalary").val(calculateNewSalary());
    $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
    $("#hdnNewCompRatio").val(calculateNewCompRatio());
    $("#PromotionPct").val('');
    $("#PromotionAmount").val('');
});



$(document).on("change", "[id$=AdjustmentEligible]", function (e) {
    adjustmentEligibleValidation(e);
    $("#hdnNewSalary").val(calculateNewSalary());
    $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
    $("#hdnNewCompRatio").val(calculateNewCompRatio());
    $("#AdjustmentPct").val('');
    $("#AdjustmentAmount").val('');
});

$(document).on("change", "[id$=FLSAStatus]", function (e) {
    flsaStatusValidation(e);
});

$(document).on("change", "#MeritPct", function (e) {
    if ($("#error_MeritPct").html() == "") {
        var meritPct = getNumber($("#MeritPct").val());
        $("#MeritAmount").val(calculateMeritAmount());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
            ((ruleConfiguration.LumpSumRuleTurnOff) || (ruleConfiguration.LumpSumRuleLumpSumType == meritPageConstants.LumpSumTypeNoAutoCalc)) ? '' : lumpSumRule();
    }
});

$(document).on("change", "#MeritAmount", function (e) {
    if ($("#error_MeritAmount").html() == "") {
        var meritAmount = getNumber($("#MeritAmount").val());
        $("#MeritPct").val(calculateMeritPct());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
        ((ruleConfiguration.LumpSumRuleTurnOff) || (ruleConfiguration.LumpSumRuleLumpSumType == meritPageConstants.LumpSumTypeNoAutoCalc)) ? '' : lumpSumRule();
        //(ruleConfiguration.LumpSumRuleTurnOff) ? lumpSumRule() : '';
    }
});

$(document).on("change", "#PromotionPct", function (e) {
    if ($("#error_PromotionPct").html() == "") {
        var promotionPct = getNumber($("#PromotionPct").val());
        $("#PromotionAmount").val(calculatePromotionAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
    }
});
$(document).on("change", "#CompaRatio", function (e) {
 
    if ($("#error_CompaRatio").html() == "") {
        $("#hdnCompRatio").val(calculateCompRatio());
        }
});
function compaRatioExceedValidation(value) {
   
}

$(document).on("change", "#PromotionAmount", function (e) {
    if ($("#error_PromotionAmount").html() == "") {
        var promotionAmount = getNumber($("#PromotionAmount").val());
        $("#PromotionPct").val(calculatePromotionPct());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
    }
});

$(document).on("click", "#lnkXmlFileName", function (e) {
    var grdData = $("#grdEmployeeData").data("kendoGrid");
    var rowItem = $(this).closest("tr");
    var rowIndex = $(rowItem).index();
    var rowData = grdData._data[rowIndex];
    var xmlProcessNum = rowData.XMLProcessNum;    
    var form = $("<form action='../Workforce/GetExportXmlFile' method='post'></form>")
    form.append("<input type='text' name='xmlProcessNum' value='" + xmlProcessNum + "' />");
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});

$(document).on("change", "#AdjustmentPct", function (e) {
    if ($("#error_AdjustmentPct").html() == "") {
        $("#AdjustmentAmount").val(calculateAdjustmentAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
    }
});

$(document).on("change", "#AdjustmentAmount", function (e) {
    if ($("#error_AdjustmentAmount").html() == "") {
        $("#AdjustmentPct").val(calculateAdjustmentPct());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
    }
});

$(document).on("change", "#LumpSumPct", function (e) {
    if ($("#error_LumpSumPct").html() == "") {
        $("#LumpSumAmount").val(calculateLumpSumAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
    }
});

$(document).on("change", "#LumpSumAmount", function (e) {
    if ($("#error_LumpSumAmount").html() == "") {
        $("#LumpSumPct").val(calculateLumpSumPct());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
    }
});

$(document).on("change", "#CurrentHourlyRate", function (e) {
    if ($("#error_CurrentHourlyRate").html() == "") {
        $("#CurrentAnnualSalary").val(calculateCurrentAnnualSalary());
        $("#CurrentAnnualizedSalary").val(calculateAnnualizedSalary());
        $("#MeritAmount").val(calculateMeritAmount());
        $("#LumpSumAmount").val(calculateLumpSumAmt());
        $("#AdjustmentAmount").val(calculateAdjustmentAmt());
        $("#PromotionAmount").val(calculatePromotionAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        if (ruleConfiguration.ComparativeRatio == true) {
            $("#hdnNewCompRatio").val(calculateNewCompRatio());
            $("#hdnCompRatio").val(calculateCompRatio());

        }
        else
            showCompaRatioAlert();
        
    }
});

$(document).on("change", "#MeritProrationFactor", function (e) {
    if ($("#error_MeritProrationFactor").html() == "") {
        if (ruleConfiguration.ProrationRuleProrate) {
            $("#MeritAmount").val(calculateMeritAmount());
            $("#hdnNewSalary").val(calculateNewSalary());
            $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
            $("#hdnNewCompRatio").val(calculateNewCompRatio());
        }
    }
});

$(document).on("change", "#MeritProrationDate", function (e) {

    if ($("#error_MeritProrationDate").html() == "") {
        var meritProrationFactor = getNumber(getProrationFactor($("#MeritProrationDate").val()));
        $("#MeritProrationFactor").val(meritProrationFactor);
        var budgetProrationFactor = (getNumber(getBudgetProrationFactor($("#MeritProrationDate").val()))).toFixed(2);
        $("#hdnBudgetProrationFactor").val(budgetProrationFactor);
        if (ruleConfiguration.ProrationRuleProrate) {
            $("#MeritAmount").val(calculateMeritAmount());
            $("#hdnNewSalary").val(calculateNewSalary());
            $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
            $("#hdnNewCompRatio").val(calculateNewCompRatio());
        }
    }
});

$(document).on("change", "#SalaryMid", function (e) {
    compRatioExceedValidation();
  });


$(document).on("change", "#EmployeeStatus", function (e) {
    employeeStatusValidation(e);
    if($("#error_EmployeeStatus").html()=="")
    {
    $("#CurrentHourlyRate").val(calculateCurrentHrlyRate());
    $("#CurrentAnnualizedSalary").val(calculateAnnualizedSalary());
    var meritAmount = calculateMeritAmount();
    $("#MeritAmount").val(meritAmount);
       
    if (meritAmount == 0)
        $("#MeritAmount").text("");
            

    // $("#LumpSumAmount").val(calculateLumpSumAmt());
    var adjustmentAmount = calculateAdjustmentAmt();
    var promotionAmount = calculatePromotionAmt();
   
    $("#AdjustmentAmount").val(adjustmentAmount);
    $("#PromotionAmount").val(promotionAmount);
    $("#hdnNewSalary").val(calculateNewSalary());
    $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
    $("#hdnNewCompRatio").val(calculateNewCompRatio());
    $("#hdnCompRatio").val(calculateCompRatio());
    if(adjustmentAmount == 0)
        $("#AdjustmentAmount").val("");
    if (promotionAmount == 0)
        $("#PromotionAmount").val("");
    }    
});

$(document).on("change", "#FTE", function (e) {
    if ($("#error_FTE").html() == "") {
        $("#CurrentAnnualizedSalary").val(calculateAnnualizedSalary());
        $("#CurrentHourlyRate").val(calculateCurrentHrlyRate());
        $("#MeritAmount").val(calculateMeritAmount());
        $("#LumpSumAmount").val(calculateLumpSumAmt());
        $("#AdjustmentAmount").val(calculateAdjustmentAmt());
        $("#PromotionAmount").val(calculatePromotionAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
        $("#hdnCompRatio").val(calculateCompRatio());
    }
});

$(document).on("change", "#ActualWorkHours", function (e) {
    if ($("#error_ActualWorkHours").html() == "") {
        $("#FTE").val(calculateFTE());
        $("#CurrentAnnualizedSalary").val(calculateAnnualizedSalary());
        $("#CurrentHourlyRate").val(calculateCurrentHrlyRate());
        $("#MeritAmount").val(calculateMeritAmount());
        $("#LumpSumAmount").val(calculateLumpSumAmt());
        $("#AdjustmentAmount").val(calculateAdjustmentAmt());
        $("#PromotionAmount").val(calculatePromotionAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
        $("#hdnCompRatio").val(calculateCompRatio());
    }
});

$(document).on("change", "#TotalWorkHours", function (e) {
    if ($("#error_TotalWorkHours").html() == "") {
        $("#FTE").val(calculateFTE());
        $("#CurrentAnnualizedSalary").val(calculateAnnualizedSalary());
        $("#CurrentHourlyRate").val(calculateCurrentHrlyRate());
        $("#MeritAmount").val(calculateMeritAmount());
        $("#LumpSumAmount").val(calculateLumpSumAmt());
        $("#AdjustmentAmount").val(calculateAdjustmentAmt());
        $("#PromotionAmount").val(calculatePromotionAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
        $("#hdnCompRatio").val(calculateCompRatio());
    }
});


$(document).on("change", "#SupervisorID", function (e) {
    var employeeID = $("#EmployeeID").val();
    var supervisorID = $("#SupervisorID").val();
    var payrollStatus = $("#PayrollStatus").val();
    if (isSupervisorIDValid()) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '../WorkForce/GetCirCularReference',
            type:"post",
            async: false,
            data: ({
                __RequestVerificationToken: token, EmployeeID: employeeID, SupervisorID: supervisorID, PayrollStatus: payrollStatus
            }),
            success: function (result) {
                if (result != null && result != "") {                    
                    $('#error_SupervisorID').html(result);
                    $("#SupervisorID").val('');
                }
            }
        });
    }
});

$(document).on("change", "#PayrollStatus", function (e) {
    getCircularReference();
    payrollStatusValidation(e);
});

$(document).on("change", "#EmployeeID", function (e) {
    getCircularReference();
});

function getCircularReference() {
    var employeeID = $("#EmployeeID").val();
    var supervisorID = $("#SupervisorID").val();
    var payrollStatus = $("#PayrollStatus").val();
    if (isSupervisorIDValid()) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '../WorkForce/GetCirCularReference',
            type:"post",
            async: false,
            data: ({
                __RequestVerificationToken: token,
                EmployeeID: employeeID, SupervisorID: supervisorID, PayrollStatus: payrollStatus
            }),
            success: function (result) {
                if (result != null && result != "") {
                    $('#error_SupervisorID').html(result);
                }
            }
        });
    }

}

$(document).on("change focusout", "#CurrentAnnualSalary", function (e) {
    if ($("#error_CurrentAnnualSalary").html() == "") {
        $("#CurrentAnnualizedSalary").val(calculateAnnualizedSalary());
        $("#CurrentHourlyRate").val(calculateCurrentHrlyRate());
        $("#MeritAmount").val(calculateMeritAmount());
        $("#LumpSumAmount").val(calculateLumpSumAmt());
        $("#AdjustmentAmount").val(calculateAdjustmentAmt());
        $("#PromotionAmount").val(calculatePromotionAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        if (ruleConfiguration.ComparativeRatio == true) {
            $("#hdnNewCompRatio").val(calculateNewCompRatio());
            $("#hdnCompRatio").val(calculateCompRatio());
        }
        else {
            showCompaRatioAlert();
        }
    }
    
});

$(document).on("change", "#SalaryMin", function (e) {
    if ($("#error_SalaryMin").html() == "") {
        $("#MeritAmount").val(calculateMeritAmount());
        $("#LumpSumAmount").val(calculateLumpSumAmt());
        $("#AdjustmentAmount").val(calculateAdjustmentAmt());
        $("#PromotionAmount").val(calculatePromotionAmt());
        $("#hdnNewSalary").val(calculateNewSalary());
        $("#hdnNewHrlyRate").val(calculateNewHrlyRate());
        $("#hdnNewCompRatio").val(calculateNewCompRatio());
    }
});


$(document).on("keyup", "#MoreInfo1", function (e) {
    moreinfo1Validation(e);
});


$(document).on("keyup", "#MoreInfo2", function (e) {
    moreinfo2Validation();
});


$(document).on("keyup", "#MoreInfo3", function (e) {
    moreinfo3Validation();
});


$(document).on("keyup", "#MoreInfo4", function (e) {
    moreinfo4Validation();
});


$(document).on("keyup", "#MoreInfo5", function (e) {
    moreinfo5Validation();
});


$(document).on("keyup", "#MoreInfo6", function (e) {
    moreinfo6Validation();
});


$(document).on("keyup", "#MoreInfo7", function (e) {
    moreinfo7Validation();
});


$(document).on("keyup", "#MoreInfo8", function (e) {
    moreinfo8Validation();
});


$(document).on("keyup", "#MoreInfo9", function (e) {
    moreinfo9Validation();
});


$(document).on("keyup", "#MoreInfo10", function (e) {
    moreinfo10Validation();
});


$(document).on("keyup", "#MoreInfo11", function (e) {
    moreinfo11Validation();
});


$(document).on("keyup", "#MoreInfo12", function (e) {
    moreinfo12Validation();
});


$(document).on("keyup", "#MoreInfo13", function (e) {
    moreinfo13Validation();
});


$(document).on("keyup", "#MoreInfo14", function (e) {
    moreinfo14Validation();
});


$(document).on("keyup", "#MoreInfo15", function (e) {
    moreinfo15Validation();
});


$(document).on("keyup", "#MoreInfo16", function (e) {
    moreinfo16Validation();
});


$(document).on("keyup", "#MoreInfo17", function (e) {
    moreinfo17Validation();
});


$(document).on("keyup", "#MoreInfo18", function (e) {
    moreinfo18Validation();
});


$(document).on("keyup", "#MoreInfo19", function (e) {
    moreinfo19Validation();
});


$(document).on("keyup", "#MoreInfo20", function (e) {
    moreinfo20Validation();
});


$(document).on("keyup", "#MoreInfo21", function (e) {
    moreinfo21Validation();
});


$(document).on("keyup", "#MoreInfo22", function (e) {
    moreinfo22Validation();
});


$(document).on("keyup", "#MoreInfo23", function (e) {
    moreinfo23Validation();
});


function getOrphanManagerInformation() {
    $.ajax({
        url: '../WorkForce/_GetOrphanEmployeeDetails',
        type: 'get',
        //data: { __RequestVerificationToken: $('input["name=__RequestVerificationToken"]').val() },
        success: function (result) {
            $("#dc-items").html(result);
            $("#dc-items").modal('show');
        }
    });
}

function getProrationFactor(prorationDate) {
    var prorationFactor = 1;
    if (ruleConfiguration.ProrationRuleProrate) {
        var startDate = new Date(ruleConfiguration.ProrationRuleProrateIncreaseStartDate);
        var endDate = new Date(ruleConfiguration.ProrationRuleProrateIncreaseEndDate);
        if (prorationDate != null) {
            var employeeHireDate = new Date(prorationDate);
            if (startDate > employeeHireDate) {
                prorationFactor = 1;
            }
            else
             if (endDate < employeeHireDate) {
                prorationFactor = 0;
            }

                //To check hire date falls between the configured start date and end date
                //else if (startDate <= employeeHireDate && endDate >= employeeHireDate) {
            else if (ruleConfiguration.ProrationRuleProrationType == 'Daily') {
                var noOfDays = getDateDifference(employeeHireDate, endDate, "days") + 1;
                prorationFactor = calculateProrationFactor(noOfDays);
            }
            else if (ruleConfiguration.ProrationRuleProrationType == 'Weekly') {
                prorationFactor = calculateProrationFactorWeekly(employeeHireDate);
            }
            else if (ruleConfiguration.ProrationRuleProrationType == 'Monthly') {
                prorationFactor = calculateProrationFactorMonthly(employeeHireDate);
            }
            else {
                prorationFactor = 0;
            }
            //}
            //else {
            //    prorationFactor = 0;
            //}
            //}
        }
    }
    return Number(prorationFactor).toFixed(2);
}

function getBudgetProrationFactor(prorationDate) {
    var prorationFactor = 1;
    if (ruleConfiguration.BudgetProration) {
        var startDate = new Date(ruleConfiguration.BudgetProrateIncreaseStartDate);
        var endDate = new Date(ruleConfiguration.BudgetProrateIncreaseEndDate);
        if (prorationDate != null) {
            var employeeHireDate = new Date(prorationDate);
            //if (ruleConfiguration.ProrationApplyMeritDiscretion) {
            //To check hire date less than configured start date
            //if (startDate >= employeeHireDate) {
            //    prorationFactor = 1;
            //}
            //    //To check hire date greater than configured end date 

            //if (startDate >= employeeHireDate) {
            //    prorationFactor = 1;
            //}
            if (startDate > employeeHireDate) {
                prorationFactor = 1;
            }
            else
            if (endDate < employeeHireDate) {
                prorationFactor = 0;
            }

                //To check hire date falls between the configured start date and end date
                //else if (startDate <= employeeHireDate && endDate >= employeeHireDate) {
            else if (ruleConfiguration.BudgetProrationType == 'Daily') {
                var noOfDays = getDateDifference(employeeHireDate, endDate, "days") + 1;
                prorationFactor = calculateBudgetProrationFactor(noOfDays);
            }
            else if (ruleConfiguration.BudgetProrationType == 'Weekly') {
                prorationFactor = calculateBudgetProrationFactorWeekly(employeeHireDate);
            }
            else if (ruleConfiguration.BudgetProrationType == 'Monthly') {
                prorationFactor = calculateBudgetProrationFactorMonthly(employeeHireDate);
            }
            else {
                prorationFactor = 0;
            }
            //}
            //else {
            //    prorationFactor = 0;
            //}
            //}
        }
    }
    return Number(prorationFactor).toFixed(2);
}


function calculateProrationFactorWeekly(employeeHireDate) {
    var prorationFactor = 1;
    var endDate = new Date(ruleConfiguration.ProrationRuleProrateIncreaseEndDate);
    var configuredDays = ruleConfiguration.ProrationRuleProrationLengthtoInclude;
    var noOfWeeks = getDateDifference(employeeHireDate, endDate, "weeks");
    var noOfDays = getDateDifference(employeeHireDate, endDate, "days");
    var remainingDays = noOfDays - (noOfWeeks * 7);
    if (configuredDays >= remainingDays) {
        noOfWeeks++;
    }
    prorationFactor = calculateProrationFactor(noOfWeeks);
    return prorationFactor;
}

function calculateBudgetProrationFactorWeekly(employeeHireDate) {
    var prorationFactor = 1;
    var endDate = new Date(ruleConfiguration.BudgetProrateIncreaseEndDate);
    var configuredDays = ruleConfiguration.BudgetProrationDatesPerMonth;
    var noOfWeeks = getDateDifference(employeeHireDate, endDate, "weeks");
    var noOfDays = getDateDifference(employeeHireDate, endDate, "days");
    var remainingDays = noOfDays - (noOfWeeks * 7);
    if (configuredDays >= remainingDays) {
        noOfWeeks++;
    }
    prorationFactor = calculateProrationFactor(noOfWeeks);
    return prorationFactor;
}

function calculateProrationFactorMonthly(employeeHireDate) {
    var prorationFactor = 1;
    var endDate = new Date(ruleConfiguration.ProrationRuleProrateIncreaseEndDate);
    var configuredDays = ruleConfiguration.ProrationRuleProrationLengthtoInclude;
    var date = employeeHireDate.getDate();
    var month = employeeHireDate.getMonth() + 1;
    var year = employeeHireDate.getFullYear();
    var daysWorked = (new Date(year, month, 0).getDate()) - date + 1;
    var monthsWorked = getDateDifference(employeeHireDate, endDate, "months");
    if (daysWorked > configuredDays) {
        monthsWorked++;
    }
    prorationFactor = calculateProrationFactor(monthsWorked);
    return prorationFactor;
}

function calculateBudgetProrationFactorMonthly(employeeHireDate) {
    var prorationFactor = 1;
    var endDate = new Date(ruleConfiguration.BudgetProrateIncreaseEndDate);
    var configuredDays = ruleConfiguration.BudgetProrationDatesPerMonth;
    var date = employeeHireDate.getDate();
    var month = employeeHireDate.getMonth() + 1;
    var year = employeeHireDate.getFullYear();
    var daysWorked = (new Date(year, month, 0).getDate()) - date + 1;
    var monthsWorked = getDateDifference(employeeHireDate, endDate, "months");
    if (daysWorked > configuredDays) {
        monthsWorked++;
    }
    prorationFactor = calculateProrationFactor(monthsWorked);
    return prorationFactor;
}

function getDateDifference(date1, date2, interval) {
    var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
    var dateone = new Date(date1).getTime();
    var datetwo = new Date(date2).getTime();
    var timediff = datetwo - dateone;
    secdate = new Date(date2);
    firdate = new Date(date1);
    if (isNaN(timediff)) return NaN;
    switch (interval) {
        case "years":
            return secdate.getFullYear() - firdate.getFullYear();
        case "months":
            return ((secdate.getFullYear() * 12 + secdate.getMonth()) - (firdate.getFullYear() * 12 + firdate.getMonth()));
        case "weeks":
            return Math.floor(timediff / week);
        case "days":
            return Math.floor(timediff / day);
        case "hours":
            return Math.floor(timediff / hour);
        case "minutes":
            return Math.floor(timediff / minute);
        case "seconds":
            return Math.floor(timediff / second);
        default:
            return undefined;
    }
}

function calculateProrationFactor(dateValue) {
    var prorationFactor = 1;
    var configuredDateValue = ruleConfiguration.ProrationRuleProrationLength;
    if (configuredDateValue > 0)
        prorationFactor = dateValue / configuredDateValue;
    return (prorationFactor.toFixed(5));
}

function calculateBudgetProrationFactor(dateValue) {
    var prorationFactor = 1;
    var configuredDateValue = ruleConfiguration.BudgetProrationDuration;
    if (configuredDateValue > 0)
        prorationFactor = dateValue / configuredDateValue;
    return (prorationFactor.toFixed(5));
}


function getNumber(value) {
    value = getNumberValue(value, 'en-US');
    var outputValues = ((isNaN(value)) || (value == null) || (value == undefined)) ? 0 : value;
    return outputValues;
}

function getRoundedValue(value,decimalPlaces)
{
    return value.toFixed(decimalPlaces);
}

function getBasesalary()
{
    var CurrentAnnualSalary = getNumber($("#CurrentAnnualSalary").val());
    var employeeClass = $("#EmployeeStatus").val();
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var basesalary = (employeeClass.toLowerCase() == "hourly" ? currentHourlyRate : CurrentAnnualSalary);
    var salaryMin = getNumber($("#SalaryMin").val());
    if (ruleConfiguration.MeritCalculation == "YES" && basesalary < salaryMin) {
        return salaryMin;
    }
    else {
        return basesalary;
    }
}

function getBasesalaryForLumpsum()
{
    var employeeClass = $("#EmployeeStatus").val();
    var CurrentAnnualSalary = getNumber($("#CurrentAnnualSalary").val());
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var basesalary = (employeeClass.toLowerCase() == "hourly" ? currentHourlyRate : CurrentAnnualSalary);
    var salaryMin = getNumber($("#SalaryMin").val());
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var totalWorkHrs = (totalWorkHours == 0) ? 2080 : totalWorkHours;
    if (ruleConfiguration.MeritCalculation == "YES" && basesalary < salaryMin) {
            return (employeeClass.toLowerCase() == "hourly" ? salaryMin * totalWorkHrs : salaryMin);
        }
        else {
            return (CurrentAnnualSalary);
        }
}


function calculateAnnualizedSalary() {
    var annualSalary = getNumber($("#CurrentAnnualSalary").val());
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var actualWorkHours = getNumber($("#ActualWorkHours").val());
    var totalWorkHrs = (totalWorkHours == 0) ? 2080 : totalWorkHours;
    var FTE = getNumber($("#FTE").val());
    if (FTE == 0 && actualWorkHours != 0) {
        FTE = actualWorkHours / totalWorkHrs;
    }
    else if (actualWorkHours == 0 && FTE == 0) {
        FTE = 1;
    }
    var result = ((annualSalary * FTE).toFixed(2));
    return result;// > 0 ? result : "";
}

function calculateFTE() {
    var FTE;
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var actualWorkHours = getNumber($("#ActualWorkHours").val());
    var totalWorkHrs = (totalWorkHours == 0) ? 2080 : totalWorkHours;
    if (actualWorkHours != 0) {
        FTE = actualWorkHours / totalWorkHrs;
    }
    else if (actualWorkHours == 0) {
        FTE = 1;
    }
    return getNumber((FTE).toFixed(5));
}


function calculateMeritPct() {
    var meritAmt = getNumber($("#MeritAmount").val());
    var CurrentAnnualSalary = getBasesalary();
    var employeeClass = $("#EmployeeStatus").val();
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var meritProrationDate = $("#MeritProrationDate").val();
    var meritProrationFactor = getNumber($("#MeritProrationFactor").val());
    var meritEligibleValue = $("#MeritEligible").val();
    if (meritEligibleValue.toLowerCase() == "yes") {
        if (ruleConfiguration.ProrationRuleProrate) {
            if ((meritProrationFactor == 0) && ((meritProrationDate != "") || (meritProrationDate != undefined))) {
                meritProrationFactor = getProrationFactor(meritProrationDate);
            }
            return getNumber((meritAmt * 100) / (meritProrationFactor * (CurrentAnnualSalary))).toFixed(2);
        }
        else {
            return getNumber((CurrentAnnualSalary != 0) ? ((meritAmt * 100) / (CurrentAnnualSalary)).toFixed(2) : 0);
        }
    }
    else
    {
        $("#MeritAmount").val('');
        return '';
    }
}

function calculateMeritAmount() {
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var meritPct = getNumber($("#MeritPct").val());
    var CurrentAnnualSalary = getBasesalary();
    var employeeClass = $("#EmployeeStatus").val();
    var meritProrationDate = $("#MeritProrationDate").val();
    var meritProrationFactor = getNumber($("#MeritProrationFactor").val());
    var meritEligibleValue = $("#MeritEligible").val();
    if (meritEligibleValue.toLowerCase() == "yes") {
        if (ruleConfiguration.ProrationRuleProrate) {
            if ((meritProrationFactor == 0) && ((meritProrationDate != "") || (meritProrationDate != undefined))) {
                meritProrationFactor = getProrationFactor(meritProrationDate);
            }
            var result = getNumber(((meritPct * (meritProrationFactor * (CurrentAnnualSalary))) / 100).toFixed(2));
            return result;// > 0 ? result : "";
        }
        else {
            var result = getNumber(((meritPct * (CurrentAnnualSalary)) / 100).toFixed(2));
            return result;// > 0 ? result : "";
        }
    }
    else
    {
        $("#MeritPct").val('');
        return '';
    }
}

function calculatePromotionPct() {
    var CurrentAnnualSalary = getBasesalary();
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var promotionAmt = getNumber($("#PromotionAmount").val());
    var promotionEligibleValue = $("#PromotionEligible").val();
    var employeeClass = $("#EmployeeStatus").val();
    if (promotionEligibleValue.toLowerCase() == 'yes') {
        var result = getNumber(((CurrentAnnualSalary != 0) ? ((promotionAmt * 100) / CurrentAnnualSalary) : 0).toFixed(2));
        return result;
    }
    else {
        $("#PromotionAmount").val('');
        return '';
    }
}

function calculatePromotionAmt() {
    var promotionPct = getNumber($("#PromotionPct").val());
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var employeeClass = $("#EmployeeStatus").val();
    var CurrentAnnualSalary = getBasesalary();
    var promotionEligibleValue = $("#PromotionEligible").val();
    if (promotionEligibleValue.toLowerCase() == 'yes') {
        var result = getNumber(((promotionPct * (CurrentAnnualSalary)) / 100).toFixed(2));
        return result;
    }
    else
    {
        $("#PromotionPct").val('');
        return '';
    }
}


function calculateAdjustmentPct() {
    var CurrentAnnualSalary = getBasesalary();
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var adjustmentAmt = getNumber($("#AdjustmentAmount").val());
    var adjustmentEligibleValue = $("#AdjustmentEligible").val();
    if (adjustmentEligibleValue.toLowerCase() == "yes") {
        var result = getNumber(((CurrentAnnualSalary != 0) ? ((adjustmentAmt * 100) / CurrentAnnualSalary) : 0).toFixed(2));
        return result;
    }
    else
    {
        $("#AdjustmentAmount").val('');
        return '';
    }
}

function calculateAdjustmentAmt() {
    var CurrentAnnualSalary = getBasesalary();
    var adjustmenPct = getNumber($("#AdjustmentPct").val());
    var adjustmentEligibleValue = $("#AdjustmentEligible").val();
    if (adjustmentEligibleValue.toLowerCase() == "yes") {
        var result = getNumber(((adjustmenPct * (CurrentAnnualSalary)) / 100).toFixed(2));
        return result;
    }
    else {
        $("#AdjustmentPct").val('');
        return '';
    }
}

function calculateLumpSumAmt() {
    var lumpsumPct = getNumber($("#LumpSumPct").val());
    var CurrentAnnualSalary = getBasesalaryForLumpsum();
    var employeeClass = $("#EmployeeStatus").val();
    var meritEligibleValue = $("#MeritEligible").val();
    if (meritEligibleValue.toLowerCase() == "yes") {
        var result = getNumber(((lumpsumPct * CurrentAnnualSalary) / 100).toFixed(2));
        return result;
    }
    else {
        $("#LumpSumPct").val('');
        return '';
    }
}

function calculateLumpSumPct() {
    var CurrentAnnualSalary = getBasesalaryForLumpsum();
    var lumpSumAmt = getNumber($("#LumpSumAmount").val());
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var employeeClass = $("#EmployeeStatus").val();
    var meritEligibleValue = $("#MeritEligible").val();
    if (meritEligibleValue.toLowerCase() == "yes") {
        var result = getNumber(((CurrentAnnualSalary != 0) ? ((lumpSumAmt * 100) / CurrentAnnualSalary) : 0).toFixed(2));
        return result;
    }
    else {
        $("#LumpSumAmount").val('');
        return '';
    }
}


function calculateCurrentHrlyRate() {
    var CurrentAnnualSalary = getNumber($("#CurrentAnnualSalary").val());
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var result = getNumber(((totalWorkHours == 0) ? (CurrentAnnualSalary / 2080) : (CurrentAnnualSalary / totalWorkHours)).toFixed(5));
    return result;
}

function calculateCurrentHrlyRateForNewsalary() {
    var CurrentAnnualSalary = getNumber($("#CurrentAnnualSalary").val());
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var employeeClass = $("#EmployeeStatus").val();
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var basesalary = (employeeClass.toLowerCase() == "hourly" ? currentHourlyRate : CurrentAnnualSalary);
    var salaryMin = getNumber($("#SalaryMin").val());
    if (ruleConfiguration.MeritCalculation == "YES" && (basesalary < salaryMin)) {
        return (employeeClass.toLowerCase() == "hourly") ? salaryMin : getNumber(((totalWorkHours == 0) ? (salaryMin / 2080) : (salaryMin / totalWorkHours)).toFixed(5));
    }
    return (employeeClass.toLowerCase() == "hourly") ? basesalary : getNumber(((totalWorkHours == 0) ? (basesalary / 2080) : (basesalary / totalWorkHours)).toFixed(5));
}

function calculateCurrentAnnualSalary() {
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var result = getNumber(((currentHourlyRate) * ((totalWorkHours == 0) ? 2080 : totalWorkHours)).toFixed(2));
    return result;// > 0 ? result : "";
}

function calculateCompRatio() {

    var result;
    if (ruleConfiguration.ComparativeRatio == true) {
        var CurrentAnnualSalary = getNumber($("#CurrentAnnualSalary").val());
        var salaryMid = getNumber($("#SalaryMid").val());
        var employeeClass = $("#EmployeeStatus").val();
        var currentHourlyRate = calculateCurrentHrlyRate();
        var value = (((employeeClass.toLowerCase() == "hourly" ? currentHourlyRate : CurrentAnnualSalary)));
         result = getNumber(((salaryMid != 0) ? ((value / salaryMid) * 100) : 0).toFixed(2));
    }
    else {
        result = getNumber($("#CompaRatio").val()).toFixed(2);
        //result = getNumber($("#CompaRatio").val().toFixed(2));
    }
    return result;// > 0 ? result : "";
}

function currentHrlyExceedValidation()
{
    var currentHourlyRate = getNumber($("#CurrentHourlyRate").val());
    (currentHourlyRate > 999.99) ? $("#error_CurrentHourlyRate").html('CurrentHourlyRate exceeds 999') : '';
}

function NewHrlyRateExceedValidation()
{
    var NewHrlyRate = getNumber($("#hdnNewHrlyRate").val());
    (NewHrlyRate > 999.99) ? $("#error_NewHrlyRate").html('NewHourlyRate exceeds 999') : $("#error_NewHrlyRate").html(' ');
}


//function newCompRatioExceedValidation() {
//    var newCompRatio = getNumber($("#hdnNewCompRatio").val()); 
//    (newCompRatio > 999.99) ? $("#error_SalaryMid").html('Compa-Ratio exceeds 999') : '';
//}

function compRatioExceedValidation() {
    if ($("#error_SalaryMid").text() == "" || $("#error_SalaryMid").text() == "Compa-Ratio exceeds 999") {
        $("#hdnCompRatio").val(calculateCompRatio());
        $("#hdnNewCompRatio").val(calculateNewCompRatio())
        var newCompRatio = getNumber($("#hdnNewCompRatio").val());
        var compRatio = getNumber($("#hdnCompRatio").val());
        if (newCompRatio > 999.99 || compRatio > 999.99) {
            $("#error_SalaryMid").text('Compa-Ratio exceeds 999');
        }
        else {
            $("#error_SalaryMid").text('');
        }
    }
    //var compRatio = getNumber($("#hdnCompRatio").val());
    //(compRatio > 999.99) ? $("#error_SalaryMid").html('Compa-Ratio exceeds 999') : '';
}

function calculateNewCompRatio() {
    var newSalary = calculateNewSalary();
    var newHrlyRate = calculateNewHrlyRate();
    var salaryMid = getNumber($("#SalaryMid").val());
    var employeeClass = $("#EmployeeStatus").val();
    var value = (((employeeClass.toLowerCase() == "hourly" ? newHrlyRate : newSalary)));

    var result = getNumber(((salaryMid != 0) ? ((value / salaryMid) * 100) : 0).toFixed(2));
    return result ;//> 0 ? result : "";
}

function calculateNewHrlyRate() {
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var employeeClass = $("#EmployeeStatus").val();
    if (employeeClass.toLowerCase() == "hourly") {
        var currentHrlyRate = calculateCurrentHrlyRateForNewsalary();
        var adjustmentAmt = getNumber(calculateAdjustmentAmt());
        var promotionAmt = getNumber(calculatePromotionAmt());
        var meritAmt = getNumber(calculateMeritAmount());
       return (currentHrlyRate + meritAmt + adjustmentAmt + promotionAmt).toFixed(5);
    }
    var newSalary = calculateNewSalary();
    var result = getNumber(((totalWorkHours == 0) ? (newSalary / 2080) : (newSalary / totalWorkHours)).toFixed(5));
    return result;// > 0 ? result : "";
}



function calculateNewSalary() {
    var baseSalary;
    var employeeClass = $("#EmployeeStatus").val();
    var CurrentAnnualSalary = getNumber($("#CurrentAnnualSalary").val());
    var currentHourlyRate = calculateCurrentHrlyRateForNewsalary();
    var CurrentSalary = (employeeClass.toLowerCase() == "hourly" ? currentHourlyRate : CurrentAnnualSalary);
    var salaryMin = getNumber($("#SalaryMin").val());
    (ruleConfiguration.MeritCalculation == "YES" && CurrentSalary < salaryMin)? baseSalary=salaryMin : baseSalary = CurrentSalary;
    var totalWorkHours = getNumber($("#TotalWorkHours").val());
    var adjustmentAmt = getNumber($("#AdjustmentAmount").val());
    var promotionAmt = getNumber($("#PromotionAmount").val());
    var meritAmt = getNumber($("#MeritAmount").val());
    if (employeeClass.toLowerCase() == "hourly")
    {
        return getNumber(((totalWorkHours == 0) ? ((currentHourlyRate + meritAmt + adjustmentAmt + promotionAmt) * 2080) : ((currentHourlyRate + meritAmt + adjustmentAmt + promotionAmt) * totalWorkHours)).toFixed(2))
    }
    else
    {
         return getNumber((baseSalary + meritAmt + adjustmentAmt + promotionAmt).toFixed(2));
    }
 }

function lumpSumRule() {
    if (!ruleConfiguration.LumpSumRuleTurnOff && ruleConfiguration.LumpSumRuleLumpSumType != meritPageConstants.LumpSumTypeNoAutoCalc) {
        if (ruleConfiguration.LumpSumRuleRangeMaxPct != 0)
            lumpSumTypeAutoCalcOverrideRangeMaxPCT();
        else
            lumpSumTypeAutoCalcOverrideRangeMaxAmt();
    }
}

function lumpSumTypeAutoCalcOverrideRangeMaxPCT() {
    var CurrentAnnualSalary = getBasesalary();
    var meritAmountAndAnnualSalary = (CurrentAnnualSalary + getNumber($("#MeritAmount").val())+ getNumber($("#AdjustmentAmount").val()) + getNumber($("#PromotionAmount").val())).toFixed(2);
    var cuttOffValue = ((CurrentAnnualSalary > ((ruleConfiguration.LumpSumRuleRangeMaxPct * getNumber($("#SalaryMax").val()) / 100)) ? CurrentAnnualSalary : ((ruleConfiguration.LumpSumRuleRangeMaxPct * getNumber($("#SalaryMax").val())) / 100)));
    if (meritAmountAndAnnualSalary > cuttOffValue) {
        calculateMeritAndLumpSumAmount(meritAmountAndAnnualSalary, cuttOffValue);
    }
    else {
        $("#LumpSumPct").val('');
        $("#LumpSumAmount").val('');
    }
}


function getLumpSumRuleRangeMaxAmt()
{
    var employeeClass = $("#EmployeeStatus").val();
    var WorkHours = getNumber($("#TotalWorkHours").val());
    var totalWrkHrs = (WorkHours == 0) ? 2080 : getNumber($("#TotalWorkHours").val());
    return getNumber((employeeClass.toLowerCase() == "hourly") ? ((ruleConfiguration.LumpSumRuleRangeMaxAmt / totalWrkHrs).toFixed(2)) : ruleConfiguration.LumpSumRuleRangeMaxAmt);
}

function lumpSumTypeAutoCalcOverrideRangeMaxAmt() {
    var CurrentAnnualSalary = getBasesalary();
    var meritAmountAndAnnualSalary = ((CurrentAnnualSalary) + (getNumber($("#MeritAmount").val())) + getNumber($("#AdjustmentAmount").val()) + getNumber($("#PromotionAmount").val())).toFixed(2);
    var cutOffValue = (((getLumpSumRuleRangeMaxAmt()) + getNumber($("#SalaryMax").val())) > CurrentAnnualSalary) ? ((getLumpSumRuleRangeMaxAmt()) + getNumber($("#SalaryMax").val())) : CurrentAnnualSalary;
    if (meritAmountAndAnnualSalary > cutOffValue) {
        cutOffValue = (CurrentAnnualSalary > getNumber($("#SalaryMax").val())) ? CurrentAnnualSalary : cutOffValue;
        calculateMeritAndLumpSumAmount(meritAmountAndAnnualSalary, cutOffValue);
    }
    else {
        $("#LumpSumPct").val('');
        $("#LumpSumAmount").val('');
    }
}


function calculateMeritAndLumpSumAmount(meritAmountAndAnnualSalary, cuttOffValue) {
    var WorkHours = getNumber($("#TotalWorkHours").val());
    var totalWrkHrs = (WorkHours == 0) ? 2080 : getNumber($("#TotalWorkHours").val());
    var currentHourlyRate = calculateCurrentHrlyRate();
    var CurrentAnnualSalary = getBasesalary();
    var employeeClass = $("#EmployeeStatus").val();
    var value = CurrentAnnualSalary;
    var lumpSumAmt = getNumber((employeeClass.toLowerCase() == "hourly") ? ((meritAmountAndAnnualSalary - cuttOffValue) * totalWrkHrs) : (meritAmountAndAnnualSalary - cuttOffValue));
    var tempLumpSumAmt = (lumpSumAmt).toFixed(2);
    $("#LumpSumAmount").val(tempLumpSumAmt);
    $("#LumpSumPct").val((calculateLumpSumPct()).toFixed(2));
    if (ruleConfiguration.MeritValuesReCalculate) {
        //var newMeritAmt = (employeeClass.toLowerCase() == "hourly") ? (getNumber($("#MeritAmount").val()) - tempLumpSumAmt) / totalWrkHrs : getNumber($("#MeritAmount").val()) - tempLumpSumAmt;
        var newMeritAmt = (employeeClass.toLowerCase() == "hourly") ? Math.abs(getNumber($("#MeritAmount").val()) - (tempLumpSumAmt / totalWrkHrs)) : Math.abs(getNumber($("#MeritAmount").val()) - tempLumpSumAmt);
        $("#MeritAmount").val(((newMeritAmt).toFixed(2)));
        var meritPct = calculateMeritPct();
        $("#MeritPct").val(meritPct);
    }
    calculateNewSalary();
    calculateNewHrlyRate();
    calculateNewCompRatio();
}


$(document).on("click", "[id$=lnkErrorData]", function () {
    var grdEmployeeDataError = $("#grdEmployeeDataError").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdEmployeeDataError._data[rowIndex];
    if (rowData.Error == 'Supervisor ID missing')
        getOrphanEmployeeInformation();
    else {
        $("#hdnErrorType").val(rowData.Error);
        isErrorCorrection = true;
        getEmployeeDataErrorCorrection(rowData.Error);
    }
});

function getEmployeeDataErrorCorrection(selectedError) {
    $.ajax({
        url: '../WorkForce/_GetErrorEmployeeDataCorrection',
        data: {
            errorType: selectedError
        },
        type: "GET",
        async: false,
        contentType: "application/json",
        success: function (result) {
            if (isErrorCorrection)
                $("#idDataCorrect").trigger('click');
            $("#dataCorrect").html(result);
            $("#btnWorkForceSave").hide();
            $("#btnErrorDataCorrectionEmployee").show();
            $("#btnDataCorrectionEmployee").hide();
            isErrorCorrection = false;
        }
    });
}

function employeeIDDuplicateValidation() {
    var empIDValue = $("#EmployeeID").val();
    var employeeSearchID = $("#hdnSelectedValue").val();
    if (empIDValue.trim() != employeeSearchID.trim() && empIDValue!="")
    {
        $.ajax({
            url: '../WorkForce/GetEmployeeIDIsDuplicate',
            data: {
                __RequestVerificationToken : $('input[name="__RequestVerificationToken"]').val(),
                employeeID: empIDValue
            },
            type: "POST",
            async: false,
            success: function (result) {
                if(result)
                    $("#error_EmployeeID").html('Employee ID is already Exists');
            }
        });
    }
}

function employeeIDValidation(e) {
    var empIDValue = $("#EmployeeID").val();
    if (empIDValue == "")
        $("#error_EmployeeID").html('Employee ID is missing');
    else if (empIDValue.length > 255)
        $("#error_EmployeeID").html('Employee ID has exceeded the defined data length');
    else
        $("#error_EmployeeID").html('');
}

function firstNameValidation(e) {
    var firstName = $("#FirstName").val();
    if (firstName == "")
        $("#error_FirstName").html('First Name is missing');
    else if (firstName.length > 255)
        $("#error_FirstName").html('First Name has exceeded the defined data length');
    else
        $("#error_FirstName").html('');
}

function middleNameValidation(e) {
    var middleName = $("#MiddleName").val();
    if (middleName.length > 255)
        $("#error_MiddleName").html('Middle Name has exceeded the defined data length');
    else
        $("#error_MiddleName").html('');
}

function lastNameValidation(e) {
    var lastName = $("#LastName").val();
    if (lastName == "")
        $("#error_LastName").html('Last Name is missing');
    else if (lastName.length > 255)
        $("#error_LastName").html('Last Name has exceeded the defined data length');
    else
        $("#error_LastName").html('');
}

function preferredNameValidation(e) {
    var preferredName = $("#PreferredName").val();
    if (preferredName.length > 255)
        $("#error_PreferredName").html('Preferred name has exceeded the defined data length');
    else
        $("#error_PreferredName").html('');
}
function genderValidation(e) {
    var gender = $("#Gender").val();
    if (gender.length > 255)
        $("#error_Gender").html('Gender has exceeded the defined data length');
    else
        $("#error_Gender").html('');
}

function jobCodeValidation(e) {
    var jobCode = $("#JobCode").val();
    if (jobCode.length > 255)
        $("#error_JobCode").html('Job Code has exceeded the defined data length');
    else
        $("#error_JobCode").html('');
}

function jobTitleValidation(e) {
    var jobCode = $("#JobTitle").val();
    if (jobCode.length > 255)
        $("#error_JobTitle").html('Job Title has exceeded the defined data length');
    else
        $("#error_JobTitle").html('');
}

function currentGradeValidation(e) {
    var currentGrade = $("#CurrentGrade").val();
    if (currentGrade.length > 255)
        $("#error_CurrentGrade").html('Current Grade has exceeded the defined data length');
    else
        $("#error_CurrentGrade").html('');
}

function businessUnitValidation(e) {
    var businessUnit = $("#BusinessUnit").val();
    if (businessUnit.length > 255)
        $("#error_BusinessUnit").html('Business Unit has exceeded the defined data length');
    else
        $("#error_BusinessUnit").html('');
}




function empFunctionValidation(e) {
    var functionValue = $("#Function").val();
    if (functionValue.length > 255)
        $('#error_Function').html('Function has exceeded the defined data length');
    else
        $('#error_Function').html('');
}



function departmentValidation(e) {
    var departmentValue = $("#Department").val();
    if (departmentValue.length > 255)
        $('#error_Department').html('Department has exceeded the defined data length');
    else
        $('#error_Department').html('');
}



function employeeClassValidation(e) {
    var employeeClassValue = $("#EmployeeClass").val();
    if (employeeClassValue.length > 255)
        $('#error_EmployeeClass').html('EmployeeClass has exceeded the defined data length');
    else
        $('#error_EmployeeClass').html('');
}



function payrollStatusValidation(e) {
    var payrollStatusValue = $("#PayrollStatus").val();
    if (payrollStatusValue.length > 1)
        $('#error_PayrollStatus').html('PayrollStatus has exceeded the defined data length');
    else if (payrollStatusValue == "")
        $('#error_PayrollStatus').html('Payroll Status is missing');
    else if (payrollStatusValue.toLowerCase() != "a" && payrollStatusValue.toLowerCase() != "l" && payrollStatusValue.toLowerCase() != "t" && payrollStatusValue.toLowerCase() != "i" && payrollStatusValue.toLowerCase() != "p" && payrollStatusValue.toLowerCase() != "n")
        $('#error_PayrollStatus').html('Payroll Status has got invalid character');
    else
        $('#error_PayrollStatus').html('');
}

function isPayrollStatusValid() {
    var payrollStatusValue = $("#PayrollStatus").val();
    if ((payrollStatusValue.length > 1) || (payrollStatusValue == ""))
        return false;
    else
        return true;
}

function flsaStatusValidation(e) {
    var flsaStatusValue = $("#FLSAStatus").val();
    if (flsaStatusValue.toLowerCase() != "exempt" && flsaStatusValue.toLowerCase() != "non-exempt" && flsaStatusValue != "")
        $('#error_FLSAStatus').html('FLSA Status has got invalid character');
    else if (flsaStatusValue.length > 255)
        $('#error_FLSAStatus').html('FLSA Status has exceeded the defined data length');
    else
        $('#error_FLSAStatus').html('');
}



function employeeStatusValidation(e) {
    var employeeStatusValue = $("#EmployeeStatus").val();
    if (employeeStatusValue == "")
        $('#error_EmployeeStatus').html('Employee Status is missing');
    else if (employeeStatusValue.toLowerCase() != "hourly" && employeeStatusValue.toLowerCase() != "annual")
        $('#error_EmployeeStatus').html('Employee Status has got invalid character');
    else if (employeeStatusValue.length > 255)
        $('#error_EmployeeStatus').html('EmployeeStatus has exceeded the defined data length');
    else
        $('#error_EmployeeStatus').html('');
}



function fteValidation(e) {
    var fteValue = getNumber($("#FTE").val());
    if (fteValue.length > 255)
        $('#error_FTE').html('FTE has exceeded the defined data length');
    else if (!(/^[-+]?\d*\.?\d*$/.test(fteValue)))
        $('#error_FTE').html('FTE has got invalid character');
    else
        $('#error_FTE').html('');
}



function workHoursValidation(e) {
    var workHoursValue = getNumber($("#TotalWorkHours").val());
    if (workHoursValue.length > 4)
        $('#error_TotalWorkHours').html('Total Work hours has exceeded the defined length');
    else if (!(/^[-+]?\d*\d*$/.test(workHoursValue)))
        $('#error_TotalWorkHours').html('Total Work hours has invalid character');
    else
        $('#error_TotalWorkHours').html('');
}



function workCountryValidation(e) {
    var workCountryValue = $("#WorkCountry").val();
    if (workCountryValue.length > 255)
        $('#error_WorkCountry').html('Work Country has exceeded the defined data length');
    else
        $('#error_WorkCountry').html('');
}



function worklocationValidation(e) {
    var worklocationValue = $("#WorkLocation").val();
    if (worklocationValue.length > 255)
        $('#error_WorkLocation').html('Work Location has exceeded the defined data length');
    else
        $('#error_WorkLocation').html('');
}



function hiredateValidation(e) {
    var hiredateValue = $("#HireDate").val();
    if (hiredateValue == "")
        $('#error_HireDate').html('Hire Date is missing');
    else if (!checkDateFormat(hiredateValue))
        $('#error_HireDate').html('Hire Date has got invalid character');
    else
        $('#error_HireDate').html('');
}



function terminationDateValidation(e) {
    var terminationDateValue = $("#TerminationDate").val();
    if (terminationDateValue != "") {
        if (!checkDateFormat(terminationDateValue))
            $('#error_TerminationDate').html('Termination Date is invalid');
        else
            $('#error_TerminationDate').html('');
    }
    else
        $('#error_TerminationDate').html('');
}



function supervisorIDValidation(e) {
    var supervisorIDValue = $("#SupervisorID").val();
    if (supervisorIDValue.length > 255)
        $('#error_SupervisorID').html('SupervisorID has exceeded the defined data length');
    else if (supervisorIDValue == "")
        $('#error_SupervisorID').html('Supervisor ID missing');
    else
        $('#error_SupervisorID').html('');
}

function isSupervisorIDValid(e) {
    var supervisorIDValue = $("#SupervisorID").val();
    if (supervisorIDValue.length > 255 || supervisorIDValue == "")
        return false;
    else
        return true;
}

function emailAddressValidation(e) {
    var emailAddressValue = $("#EmailAddress").val();
    var emailRegex = new RegExp(/^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$/i);
    var valid = emailRegex.test(emailAddressValue);
    if (emailAddressValue == "")
        $('#error_EmailAddress').html('');
    else if (!valid)
        $('#error_EmailAddress').html("Email Address has invalid character");
    else if (emailAddressValue.length > 255)
        $('#error_EmailAddress').html('EmailAddress has exceeded the defined data length');
    else
        $('#error_EmailAddress').html('');
}



function payCurrencyValidation(e) {
    var payCurrencyValue = $("#PayCurrency").val();
    //if (!ruleConfiguration.IsMultiCurrencyEnable && $('#error_PayCurrency').val() == "") {
    //    var payCurrencyValue = $("#PayCurrency").val();
    //    $.ajax({
    //        url: "../WorkForce/PayCurrencyValidation",
    //        type: "post",
    //        data: { payCurrencyCode: payCurrencyValue },
    //        success: function (result) {
    //            $('#error_PayCurrency').html(result);
    //        }
    //    });
    //}
    //else {

        if (payCurrencyValue == "")
            $('#error_PayCurrency').html('Pay Currency is missing');
        else if (payCurrencyValue.length > 3)
            $('#error_PayCurrency').html('Pay Currency has exceeded the defined data length');
        else
            $('#error_PayCurrency').html('');
   // }
}




function lastPayChangeDateValidation(e) {
    var lastPayChangeDateValue = $("#LastPayChangeDate").val();
    if (lastPayChangeDateValue != "") {
        if (!checkDateFormat(lastPayChangeDateValue))
            $('#error_LastPayChangeDate').html('Last Pay Change Date is invalid');
        else
            $('#error_LastPayChangeDate').html('');
    }
    else
        $('#error_LastPayChangeDate').html('');
}



function lastPayChangeReasonValidation(e) {
    var lastPayChangeReasonValue = $("#LastPayChangeReason").val();
    if (lastPayChangeReasonValue.length > 255)
        $('#error_LastPayChangeReason').html('Last Pay Change Reason has exceeded the defined data length');
    else
        $('#error_LastPayChangeReason').html('');
}



function currentHourlyRateValidation(e) {
    var currentHourlyRateValue = $("#CurrentHourlyRate").val();
    var employeeStatusValue = $("#EmployeeStatus").val();    
    if (employeeStatusValue.toLowerCase() == "hourly" && currentHourlyRateValue=="")
        $('#error_CurrentHourlyRate').html('Current hourly rate is missing for employees who falls under Hourly employee status category');
    else if (!(/^[-+]?\d*\.?\d*$/.test(document.getElementById('CurrentHourlyRate').value)))
        $('#error_CurrentHourlyRate').html('Current Hourly Rate has got invalid character');
    else
        $('#error_CurrentHourlyRate').html('');
}



function currentAnnualizedSalaryValidation(e) {
    var currentAnnualizedSalary = $("#CurrentAnnualizedSalary").val();
    var value = currentAnnualizedSalary.replace(',', "");
    if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_CurrentAnnualizedSalary').html('Current Annualized Salary has got invalid character');
    else {
        $('#error_CurrentAnnualizedSalary').html('');
        $("#CurrentAnnualizedSalary").val(value);
    }

}



function currentAnnualsalaryValidation(e) {
    var currentAnnualsalaryValue = $("#CurrentAnnualSalary").val();
    var value = currentAnnualsalaryValue.replace(',', "");
    if (currentAnnualsalaryValue == "")
        $('#error_CurrentAnnualSalary').html('Current Annual Salary is missing ');
    else if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_CurrentAnnualSalary').html('Current Annual Salary  has got invalid character');
    else
        $('#error_CurrentAnnualSalary').html('');
        $("#CurrentAnnualSalary").val(value);
}



function payrollSourceValidation(e) {
    var payrollSourceValue = $("#PayrollSource").val();
    if (payrollSourceValue.length > 255)
        $('#error_PayrollSource').html('Payroll Source has exceeded the defined data length');
    else
        $('#error_PayrollSource').html('');
}



function salaryMinValidation(e) {
    var salaryMinValue = $("#SalaryMin").val();
    var value = salaryMinValue.replace(',', "");
    if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_SalaryMin').html('Salary Min has got invalid character');
    else {
        $('#error_SalaryMin').html('');
        $("#SalaryMin").val(value);
    }
}



function salaryMidValidation(e) {
    if ($('#error_SalaryMid').text() == "" || $('#error_SalaryMid').text().toLowerCase() == 'salary mid has got invalid character') {
        var newCompRatio = getNumber($("#hdnNewCompRatio").val());
        var compRatio = getNumber($("#hdnCompRatio").val());
        var salaryMidValue = $("#SalaryMid").val();
        var value = salaryMidValue.replace(',', "");
        if (!(/^[-+]?\d*\.?\d*$/.test(value))) {
            $('#error_SalaryMid').text('Salary mid has got invalid character');
        }
        else {
            $('#error_SalaryMid').text('');
            $("#SalaryMid").val(value);
        }
    }
}



function salaryMaxValidation(e) {
    var salaryMaxValue = $("#SalaryMax").val();
    var value = salaryMaxValue.replace(',', "");
    if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_SalaryMax').html('Salary Max has got invalid character');
    else {
        $('#error_SalaryMax').html('');
        $("#SalaryMax").val(value);
    }
}


function meritProrationDateValidation(e) {
    var meritEligibleValue = $("#MeritEligible").val();    
    var meritprorationdateValue = $("#MeritProrationDate").val();
    if (!(ruleConfiguration.ProrationRuleProrate))
        $('#error_MeritProrationDate').html('');
    else if (meritprorationdateValue == "" && meritEligibleValue.toLowerCase() != 'no')
        $('#error_MeritProrationDate').html('Merit Proration Date is missing');
    else if (meritprorationdateValue == "" && meritEligibleValue.toLowerCase() == 'no')
        $('#error_MeritProrationDate').html('');
    else if (!checkDateFormat(meritprorationdateValue))
        $('#error_MeritProrationDate').html('Merit Proration Date has got invalid character');
    else
        $('#error_MeritProrationDate').html('');
}



function meritProrationFactorValidation(e) {
    var meritProrationFactorValue = $("#MeritProrationFactor").val();
    if (!(/^[-+]?\d*\.?\d*$/.test(document.getElementById('MeritProrationFactor').value)))
        $('#error_MeritProrationFactor').html('Merit Proration Factor has got invalid character');
    else
        $('#error_MeritProrationFactor').html('');
}



function meritPerformanceRatingValidation(e) {
    var meritPerformanceRatingValue = $("#MeritPerformanceRating").val();
    if (meritPerformanceRatingValue.length > 255)
        $('#error_MeritPerformanceRating').html('Merit Performance Rating Exceeds the character limit');
    else
        $('#error_MeritPerformanceRating').html('');
}



function meritEligibleValidation(e) {
    var meritEligibleValue = $("#MeritEligible").val();
    if (meritEligibleValue.toLowerCase() != 'no')
        $("#mandate_MeritProrationDate").show();
    else
        $("#mandate_MeritProrationDate").hide();

    if (meritEligibleValue.length > 3)
        $('#error_MeritEligible').html('MeritEligible has exceeded the defined data length');
    else if (meritEligibleValue == "")
        $('#error_MeritEligible').html('Merit Eligible is missing');
    else if (meritEligibleValue.toLowerCase() != 'yes' && meritEligibleValue.toLowerCase() != 'no')
        $('#error_MeritEligible').html('Merit Eligible has got invalid character');
    else
        $('#error_MeritEligible').html('');
}



function meritPctValidation(e) {
    var meritPctValue = $("#MeritPct").val();
    if (!(/^[-+]?\d*\.?\d*$/.test(document.getElementById('MeritPct').value)))
        $('#error_MeritPct').html('MeritPct has got invalid character');
    else
        $('#error_MeritPct').html('');

}



function meritamountValidation(e) {
    var meritamountValue = $("#MeritAmount").val();
    var value = meritamountValue.replace(',', "");
    if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_MeritAmount').html('MeritAmount has got invalid character');
    else {
        $('#error_MeritAmount').html('');
        $("#MeritAmount").val(value);
    }
}



function meritIncreaseGuidelineValidation() {
    var meritIncreaseGuidelineValue = $("#MeritIncreaseGuideline").val();
    if (!(/^[0-9\.\-\%\/]+$/.test(meritIncreaseGuidelineValue)) && (meritIncreaseGuidelineValue!=""))
        $('#error_MeritIncreaseGuideline').html('MeritIncreaseGuideline has got invalid character');
     else if (meritIncreaseGuidelineValue.length > 255)
        $('#error_MeritIncreaseGuideline').val('MeritIncreaseGuideline has exceeded the defined data length');
    else 
        $('#error_MeritIncreaseGuideline').html('');
}



function meritEffectiveDateValidation(e) {
    var meritEffectiveDateValue = $("#MeritEffectiveDate").val();
    if (meritEffectiveDateValue != "") {
        if (!checkDateFormat(meritEffectiveDateValue))
            $('#error_MeritEffectiveDate').html('Merit Effective Date has got invalid character');
        else
            $('#error_MeritEffectiveDate').html('');
    }
    else
        $('#error_MeritEffectiveDate').html('');
}



function lumpsumPctValidation(e) {
    var lumpsumPctValue = $("#LumpSumPct").val();
    if (!(/^[-+]?\d*\.?\d*$/.test(lumpsumPctValue)))
        $('#error_LumpSumPct').html('Lump Sum Pct has got invalid character');
    else
        $('#error_LumpSumPct').html('');
}



function lumpsumAmountValidation(e) {
    var lumpsumAmountValue = $("#LumpSumAmount").val();
    var value = lumpsumAmountValue.replace(',', "");
    if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_LumpSumAmount').html('Lump Sum Amount has got invalid character');
    else {
        $('#error_LumpSumAmount').html('');
        $("#LumpSumAmount").val(value);
    }
}



function lumpsumEffectiveDateValidation(e) {
    var lumpsumEffectiveDateValue = $("#LumpsumEffectiveDate").val();
    if (lumpsumEffectiveDateValue != "") {
        if (!checkDateFormat(lumpsumEffectiveDateValue))
            $('#error_LumpsumEffectiveDate').html('Lump Sum Effective Date has got invalid character');
        else
            $('#error_LumpsumEffectiveDate').html('');
    }
    else
        $('#error_LumpsumEffectiveDate').html('');
}



function promotionEligibleValidation(e) {
    var promotionEligibleValue = $("#PromotionEligible").val();
    if (promotionEligibleValue.length > 3)
        $('#error_PromotionEligible').html('PromotionEligible has exceeded the defined data length');
    else if (promotionEligibleValue.toLowerCase() != 'yes' && promotionEligibleValue.toLowerCase() != 'no' && promotionEligibleValue != "")
        $('#error_PromotionEligible').html('Promotion Eligible has got invalid character');
    else
        $('#error_PromotionEligible').html('');
}



function promotionPctValidation(e) {
    var promotionPctValue = getNumber($("#PromotionPct").val());
    if (!(/^[-+]?\d*\.?\d*$/.test(document.getElementById('LumpSumAmount').value)))
        $('#error_PromotionPct').html('Promotion Pct has got invalid character');
    else
        $('#error_PromotionPct').html('');
}



function promotionAmountValidation(e) {
    var promotionAmountValue = $("#PromotionAmount").val();
    var value = promotionAmountValue.replace(',', "");
    if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_PromotionAmount').html('Promotion Amount has got invalid character');
    else {
        $('#error_PromotionAmount').html('');
        $("#PromotionAmount").val(value);
    }
}



function promotetoValidation(e) {    
    var promotionAmountValue = getNumber($("#PromotionAmount").val());
    var promotionPctValue = getNumber($("#PromotionPct").val());
    var promotetoValue = $("#PromoteTo").val();
    if((promotionAmountValue != 0 || promotionPctValue != 0) && promotetoValue=="")
        $('#error_PromoteTo').html('Promote title is missing'); 
    else if (promotetoValue.length > 255)
        $('#error_PromoteTo').html('Promote To has exceeded the defined data length');
    else
        $('#error_PromoteTo').html('');
}



function lastPromotionDateValidation(e) {
    var lastPromotionDateValue = $("#LastPromotionDate").val();
    if (lastPromotionDateValue != "") {
        if (!checkDateFormat(lastPromotionDateValue))
            $('#error_LastPromotionDate').html('Last Promotion Date has got invalid character');
        else
            $('#error_LastPromotionDate').html('');
    }
    else
        $('#error_LastPromotionDate').html('');
}



function promotionEffectiveDateValidation(e) {
    var promotionEffectiveDateValue = $("#PromotionEffectiveDate").val();
    if (promotionEffectiveDateValue != "") {
        if (!checkDateFormat(promotionEffectiveDateValue))
            $('#error_PromotionEffectiveDate').html('Promotion Effective Date has got invalid character');
        else
            $('#error_PromotionEffectiveDate').html('');
    }
    else
        $('#error_PromotionEffectiveDate').html('');
}



function adjustmentEligibleValidation(e) {
    var adjustmentEligibleValue = $("#AdjustmentEligible").val();
    if (adjustmentEligibleValue.length > 3)
        $('#error_AdjustmentEligible').html('Adjustment Eligible has exceeded the defined data length');
    else if (adjustmentEligibleValue.toLowerCase() != 'yes' && adjustmentEligibleValue.toLowerCase() != 'no' && adjustmentEligibleValue != "")
        $('#error_AdjustmentEligible').html('Adjustment Eligible has got invalid character');
    else
        $('#error_AdjustmentEligible').html('');
}



function adjustmenPctValidation(e) {
    var adjustmenPctValue = $("#AdjustmentPct").val();
    if (!(/^[-+]?\d*\.?\d*$/.test(document.getElementById('AdjustmentPct').value)))
        $('#error_AdjustmentPct').html('Adjustment Pct has got invalid character');
    else
        $('#error_AdjustmentPct').html('');
}

function compaRatioValidation(e) {
    if (ruleConfiguration.ComparativeRatio == false) {
        var compaRatioValue = $("#CompaRatio").val();
        if (!(/^[-+]?\d*\.?\d*$/.test(document.getElementById('CompaRatio').value)) || (compaRatioValue > 999.99))
            $('#error_CompaRatio').html('Compa Ratio has got invalid character');
        else if (compaRatioValue == "")
            $('#error_CompaRatio').html('Compa Ratio is missing');
        else
            $('#error_CompaRatio').html('');
    }
    else $('#error_CompaRatio').html('');
}



function adjustmentAmountValidation(e) {
    var adjustmentAmountValue = $("#AdjustmentAmount").val();
    var value = adjustmentAmountValue.replace(',', "");
    if (!(/^[-+]?\d*\.?\d*$/.test(value)))
        $('#error_AdjustmentAmount').html('Adjustment Amount invalid Format');
    else
        $('#error_AdjustmentAmount').html('');
    $("#AdjustmentAmount").val(value);
}



function adjustmentEffectiveDateValidation(e) {
    var adjustmentEffectiveDateValue = $("#AdjustmentEffectiveDate").val();
    if (adjustmentEffectiveDateValue != "") {
        if (!checkDateFormat(adjustmentEffectiveDateValue))
            $('#error_AdjustmentEffectiveDate').html('Adjustment Effective Date is invalid');
        else
            $('#error_AdjustmentEffectiveDate').html('');
    }
    else
        $('#error_AdjustmentEffectiveDate').html('');
}



function moreinfo1Validation() {
    var moreinfo1Value = $("#MoreInfo1").val();
    if (moreinfo1Value.length > 255)
        $('#error_MoreInfo1').html('MoreInfo1 has exceeded the defined data length');
    else
        $('#error_MoreInfo1').html('');
}



function moreinfo2Validation() {
    var moreinfo2Value = $("#MoreInfo2").val();
    if (moreinfo2Value.length > 255)
        $('#error_MoreInfo2').html('MoreInfo2 has exceeded the defined data length');
    else
        $('#error_MoreInfo2').html('');
}



function moreinfo3Validation() {
    var moreinfo3Value = $("#MoreInfo3").val();
    if (moreinfo3Value.length > 255)
        $('#error_MoreInfo3').html('MoreInfo3 has exceeded the defined data length');
    else
        $('#error_MoreInfo3').html('');
}



function moreinfo4Validation() {
    var moreinfo4Value = $("#MoreInfo4").val();
    if (moreinfo4Value.length > 255)
        $('#error_MoreInfo4').html('MoreInfo4 has exceeded the defined data length');
    else
        $('#error_MoreInfo4').html('');
}



function moreinfo5Validation() {
    var moreinfo5Value = $("#MoreInfo5").val();
    if (moreinfo5Value.length > 255)
        $('#error_MoreInfo5').html('MoreInfo5 has exceeded the defined data length');
    else
        $('#error_MoreInfo5').html('');
}



function moreinfo6Validation() {
    var moreinfo6Value = $("#MoreInfo6").val();
    if (moreinfo6Value.length > 255)
        $('#error_MoreInfo6').html('MoreInfo6 has exceeded the defined data length');
    else
        $('#error_MoreInfo6').html('');
}



function moreinfo7Validation() {
    var moreinfo7Value = $("#MoreInfo7").val();
    if (moreinfo7Value.length > 255)
        $('#error_MoreInfo7').html('MoreInfo7 has exceeded the defined data length');
    else
        $('#error_MoreInfo7').html('');
}



function moreinfo8Validation() {
    var moreinfo8Value = $("#MoreInfo8").val();
    if (moreinfo8Value.length > 255)
        $('#error_MoreInfo8').html('MoreInfo8 has exceeded the defined data length');
    else
        $('#error_MoreInfo8').html('');
}



function moreinfo9Validation() {
    var moreinfo9Value = $("#MoreInfo9").val();
    if (moreinfo9Value.length > 255)
        $('#error_MoreInfo9').html('MoreInfo9 has exceeded the defined data length');
    else
        $('#error_MoreInfo9').html('');
}



function moreinfo10Validation() {
    var moreinfo10Value = $("#MoreInfo10").val();
    if (moreinfo10Value.length > 255)
        $('#error_MoreInfo10').html('MoreInfo10 has exceeded the defined data length');
    else
        $('#error_MoreInfo10').html('');
}



function moreinfo11Validation() {
    var moreinfo11Value = $("#MoreInfo11").val();
    if (moreinfo11Value.length > 255)
        $('#error_MoreInfo11').html('MoreInfo11 has exceeded the defined data length');
    else
        $('#error_MoreInfo11').html('');
}



function moreinfo12Validation() {
    var moreinfo12Value = $("#MoreInfo12").val();
    if (moreinfo12Value.length > 255)
        $('#error_MoreInfo12').html('MoreInfo12 has exceeded the defined data length');
    else
        $('#error_MoreInfo12').html('');
}



function moreinfo13Validation() {
    var moreinfo13Value = $("#MoreInfo13").val();
    if (moreinfo13Value.length > 255)
        $('#error_MoreInfo13').html('MoreInfo13 has exceeded the defined data length');
    else
        $('#error_MoreInfo13').html('');
}



function moreinfo14Validation() {
    var moreinfo14Value = $("#MoreInfo14").val();
    if (moreinfo14Value.length > 255)
        $('#error_MoreInfo14').html('MoreInfo14 has exceeded the defined data length');
    else
        $('#error_MoreInfo14').html('');
}



function moreinfo15Validation() {
    var moreinfo15Value = $("#MoreInfo15").val();
    if (moreinfo15Value.length > 255)
        $('#error_MoreInfo15').html('MoreInfo15 has exceeded the defined data length');
    else
        $('#error_MoreInfo15').html('');
}



function moreinfo16Validation() {
    var moreinfo16Value = $("#MoreInfo16").val();
    if (moreinfo16Value.length > 255)
        $('#error_MoreInfo16').html('MoreInfo16 has exceeded the defined data length');
    else
        $('#error_MoreInfo16').html('');
}



function moreinfo17Validation() {
    var moreinfo17Value = $("#MoreInfo17").val();
    if (moreinfo17Value.length > 255)
        $('#error_MoreInfo17').html('MoreInfo17 has exceeded the defined data length');
    else
        $('#error_MoreInfo17').html('');
}



function moreinfo18Validation() {
    var moreinfo18Value = $("#MoreInfo18").val();
    if (moreinfo18Value.length > 255)
        $('#error_MoreInfo18').html('MoreInfo18 has exceeded the defined data length');
    else
        $('#error_MoreInfo18').html('');
}



function moreinfo19Validation() {
    var moreinfo19Value = $("#MoreInfo19").val();
    if (moreinfo19Value.length > 255)
        $('#error_MoreInfo19').html('MoreInfo19 has exceeded the defined data length');
    else
        $('#error_MoreInfo19').html('');
}



function moreinfo20Validation() {
    var moreinfo20Value = $("#MoreInfo20").val();
    if (moreinfo20Value.length > 255)
        $('#error_MoreInfo20').html('MoreInfo20 has exceeded the defined data length');
    else
        $('#error_MoreInfo20').html('');
}



function moreinfo21Validation() {
    var moreinfo21Value = $("#MoreInfo21").val();
    if (moreinfo21Value.length > 255)
        $('#error_MoreInfo21').html('MoreInfo21 has exceeded the defined data length');
    else
        $('#error_MoreInfo21').html('');
}



function moreinfo22Validation() {
    var moreinfo22Value = $("#MoreInfo22").val();
    if (moreinfo22Value.length > 255)
        $('#error_MoreInfo22').html('MoreInfo22 has exceeded the defined data length');
    else
        $('#error_MoreInfo22').html('');
}



function moreinfo23Validation() {
    var moreinfo23Value = $("#MoreInfo23").val();
    if (moreinfo23Value.length > 255)
        $('#error_MoreInfo23').html('MoreInfo23 has exceeded the defined data length');
    else
        $('#error_MoreInfo23').html('');
}




function actualworkhoursValidation(e) {    
    var actualworkhoursValue = getNumber($("#ActualWorkHours").val());
    if ($("#ActualWorkHours").length > 4)
        $('#error_ActualWorkHours').html('ActualWorkHours has exceeded the defined data length');
    else if (!(/^[-+]?\d*\d*$/.test(document.getElementById('ActualWorkHours').value)))
        $('#error_ActualWorkHours').html('Actual work hours has got invalid character');
    else
        $('#error_ActualWorkHours').html('');
}

function isValidEmployeeData() {
    var errorMessage = "";
    $('[id^="error_"]').each(function () {
        errorMessage = errorMessage + ((this.textContent != "" && this.textContent != undefined && this.textContent != " ") ? (this.textContent).trim() + "<br/>" : "");
    });
    errorMessage = (errorMessage.trim() == "<br/>") ? "" : errorMessage.trim();
    if (errorMessage != "") {
        Warningmessage(errorMessage);
        return false;
    }
    else return true;
}


function callValidationRules() {
    employeeIDValidation();
    firstNameValidation();
    middleNameValidation();
    lastNameValidation();
    preferredNameValidation();
    genderValidation();
    jobCodeValidation();
    jobTitleValidation();
    currentGradeValidation();
    businessUnitValidation();
    empFunctionValidation();
    departmentValidation();
    employeeClassValidation();
    payrollStatusValidation();
    flsaStatusValidation();
    employeeStatusValidation();
    fteValidation();
    workHoursValidation();
    workCountryValidation();
    worklocationValidation();
    hiredateValidation();
    terminationDateValidation();
    supervisorIDValidation();
    emailAddressValidation();
    payCurrencyValidation();
    lastPayChangeDateValidation();
    lastPayChangeReasonValidation();
    currentHourlyRateValidation();
    currentAnnualizedSalaryValidation();
    currentAnnualsalaryValidation();
    payrollSourceValidation();
    salaryMinValidation();
    salaryMidValidation();
    salaryMaxValidation();
    meritProrationDateValidation();
    meritProrationFactorValidation();
    meritPerformanceRatingValidation();
    meritEligibleValidation();
    meritPctValidation();
    meritamountValidation();
    meritEffectiveDateValidation();
    meritIncreaseGuidelineValidation();
    lumpsumPctValidation();
    lumpsumAmountValidation();
    lumpsumEffectiveDateValidation();
    promotionEligibleValidation();
    promotionPctValidation();
    promotionAmountValidation();
    promotetoValidation();
    lastPromotionDateValidation();
    promotionEffectiveDateValidation();
    adjustmentEligibleValidation();
    adjustmenPctValidation();
    adjustmentAmountValidation();
    adjustmentEffectiveDateValidation();
    moreinfo1Validation();
    moreinfo2Validation();
    moreinfo3Validation();
    moreinfo4Validation();
    moreinfo5Validation();
    moreinfo6Validation();
    moreinfo7Validation();
    moreinfo8Validation();
    moreinfo9Validation();
    moreinfo10Validation();
    moreinfo11Validation();
    moreinfo12Validation();
    moreinfo13Validation();
    moreinfo14Validation();
    moreinfo15Validation();
    moreinfo16Validation();
    moreinfo17Validation();
    moreinfo18Validation();
    moreinfo19Validation();
    moreinfo20Validation();
    moreinfo21Validation();
    moreinfo22Validation();
    moreinfo23Validation();
    getCircularReference();
    compaRatioValidation();
}


$(document).on("click", "#btnCancel", function (event) {
    $("#dc-items").modal('hide');
});

function showCompaRatioAlert() {
    showAlert("Hi! It looks like you have updated the current pay. This is a gentle reminder to update the current compa-ratio for this employee.");
}

//// Home/////
function employeeDataErrorDataBound() {

    var grid = $("#grdEmployeeDataError").data("kendoGrid");
    var length = grid.dataSource._data.length;
    if (length < 1) {
        $("#liErrorMessage").css("display", "none");
        $("#idDataCorrect").trigger("click");
    }
}
