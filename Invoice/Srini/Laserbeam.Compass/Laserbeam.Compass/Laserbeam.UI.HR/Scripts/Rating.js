$(document).on("click", "[id$=btnConfigureRatingDelete]", function (e) {
    DeleteRating($(this), e);
});

$(document).on("click", "[id$=configureRatingDescription]", function (e) {
    EditRatingDescription($(this));
});

$(document).on("click", "[id$=configureLowRange]", function (e) {
    EditRatingLowRange($(this));
});

$(document).on("click", "[id$=configureHighRange]", function (e) {
    EditRatingHighRange($(this));
});

$(document).on("click", "[id$=configureSortOrder]", function (e) {
    EditSortOrder($(this));
});
$(document).on("click", "#lnkMeritRating", function (e) {
    $("#divBtnSave").hide();
    $("#divBtnAdd").show();
    var grdConfigureRatingForRange = $("#grdConfigureRating").data("kendoGrid");
    grdConfigureRatingForRange.dataSource.read();
});
$(document).on("click", "#lnkMeritRange", function (e) {
    $("#divBtnSave").show();
    $("#divBtnAdd").hide();
    var grdConfigureRatingForRange = $("#grdConfigureRatingForRange").data("kendoGrid");
    grdConfigureRatingForRange.dataSource.read();
});
$(document).ready(function () {
    $("#divBtnSave").hide();
    $("#divBtnAdd").show();
});

$(document).on("keydown", "[id$=txtMinRange]", function (e) {

    allowDecimalNumberOnlyInput(e);
});

$(document).on("keydown", "[id$=txtMaxRange]", function (e) {

    allowDecimalNumberOnlyInput(e);
});


$(document).on("keydown", "[id$=editScoreRangeLow]", function (e) {

    allowDecimalNumberOnlyInput(e);
});

$(document).on("keydown", "[id$=editScoreRangeHigh]", function (e) {

    allowDecimalNumberOnlyInput(e);
});
$(document).on("keypress", "[id$=txtMinRange]", function (e) {
    var a = this;
    var txt = $("#txtMinRange").val();
    if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
        var substr = txt.split(".")[1].substring(0, 1);
        $("#txtMinRange").val(txt.split(".")[0] + "." + substr);
    }
});
$(document).on("keypress", "#txtMaxRange", function (e) {

    var txt = $("#txtMaxRange").val();
    if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
        var substr = txt.split(".")[1].substring(0, 1);
        $("#txtMaxRange").val(txt.split(".")[0] + "." + substr);
    }
});
$(document).on("keypress", "#editScoreRangeLow", function (e) {

    var txt = $("#editScoreRangeLow").val();
    if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
        var substr = txt.split(".")[1].substring(0, 1);
        $("#editScoreRangeLow").val(txt.split(".")[0] + "." + substr);
    }
});
$(document).on("keypress", "#editScoreRangeHigh", function (e) {

    var txt = $("#editScoreRangeHigh").val();
    if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
        var substr = txt.split(".")[1].substring(0, 1);
        $("#editScoreRangeHigh").val(txt.split(".")[0] + "." + substr);
    }
});

$(document).on("change", "[id$=txtMinRange]", function (e) {

    setPageChanged(true);
    var grdConfigureRatingForRange = $("#grdConfigureRatingForRange").data("kendoGrid");
    var row = $(this).closest("tr");
    var rowData = grdConfigureRatingForRange.dataItem(row);
    var rowIndex = $(row).index();
    rowData.MinRange = getNumberValue(this.value) == null ? 0 : getNumberValue(this.value);
    rowData.dirty = true;
    grdConfigureRatingForRange.refresh();
    objChangeFlag = true;
});
$(document).on("change", "[id$=txtMaxRange]", function (e) {
  
    setPageChanged(true);
    var grdConfigureRatingForRange = $("#grdConfigureRatingForRange").data("kendoGrid");
    var row = $(this).closest("tr");
    var rowData = grdConfigureRatingForRange.dataItem(row);
    var rowIndex = $(row).index();
    rowData.MaxRange = getNumberValue(this.value) == null ? 0 : getNumberValue(this.value);
    rowData.dirty = true;
    grdConfigureRatingForRange.refresh();
    objChangeFlag = true;
});
$(document).on("click", "#btnSaveRatingRange", function (e) {
  
    var gridUser = $("#grdConfigureRatingForRange").data("kendoGrid");
    gridUser.dataSource.sync();
   // objChangeFlag = false;
    //Successmessage("Great ! you have successfully set the Range.");
});
$(document).on("click", "#addConfigureRatingButton", function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../Rating/_EditConfigureRatingPopup",
        type: "get",
        success: function (result) {
            $("#divRating").html(result);
            $("#divRating").modal('show');
        }
    });
});

$(document).on("show.bs.modal", "#divRating", function (e) {
    $.validator.unobtrusive.parse(document);
    setValidation();
});

$(document).on("click", "#CancelAddRatingButton", function () {
    $("#divRating").modal("hide");
    $("#grdConfigureRating").data("kendoGrid").refresh();
});


$.validator.unobtrusive.adapters.add(
'notequalto', ['other'], function (options) {
    options.rules['notEqualTo'] = "[name='" + options.params.other + "']";
    if (options.message) {
        options.messages['notEqualTo'] = options.message;
    }
});

$.validator.addMethod('notEqualTo', function (value, element, param) {
    return this.optional(element) || value != $(param).val();
}, '');

$.validator.setDefaults({
    showErrors: function (errorMap, errorList) {
        $.each(this.successList, function (index, value) {
            $(value).parent().removeClass("has-error");
            return $(value).popover("hide");
        });
        return $.each(errorList, function (index, value) {
            var _popover;
            _popover = $(value.element).popover({
                trigger: "manual",
                placement: "bottom",
                content: value.message,
                template: "<div class=\"popover\"><div class=\"arrow\"></div><div class=\"popover-inner\"><div class=\"popover-content\"><span class=\"glyphicon glyphicon-hand-right\"></span><p></p></div></div></div>"
            });
            $(value.element).parent().addClass("has-error");
            _popover.data("bs.popover").options.content = value.message;
            return $(value.element).popover("show");
        });
    }
});

function setValidation() {
    $(".input-validation-error").parent().removeClass('has-success').addClass("has-error");
    $("div.validation-summary-errors").has("li:visible").addClass("alert-block alert-danger");

    $('form').data('validator').settings.onfocusout = function (element) {
        $(element).valid();
    };
}

function gridRating_sync() {
  
    var gridUser = $("#grdConfigureRatingForRange").data("kendoGrid");
    gridUser.dataSource.read();
}
function onUpdateRequestEnd(e) {
    
    if (e.type == "update") {
        Successmessage(e.response.Message);
        $("#grdConfigureRatingForRange").data("kendoGrid").dataSource.read();
    }
}
function DeleteRating(deleteData, e) {
    var configRatingGrid = $("#grdConfigureRating").data("kendoGrid");
    var row = deleteData.closest("tr");
    var rowIndex = $(row).index();
    var rowData = configRatingGrid._data[rowIndex];
    var newRatingNum = rowData.RatingNum;
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (!showConfirm(e, "Do you want to delete the Rating?")) return false;
    $.ajax({
        url: "../Rating/DeleteRatingData",
        type: "post",
        data: {
            __RequestVerificationToken: token,
            ratingNum: newRatingNum
        },
        success: function (result) {
            Successmessage("Deleted Successfully");
            inputChanged = false;
            $('#grdConfigureRating').data('kendoGrid').refresh();
            $('#grdConfigureRating').data('kendoGrid').dataSource.read();
            $("#divRating").modal('hide');
        }
    });
}

function EditRatingDescription(editRatingDesc) {
    EditRatingDetails(editRatingDesc);
}

function EditRatingLowRange(editLowRange) {
    EditRatingDetails(editLowRange);
}

function EditRatingHighRange(editHighRange) {
    EditRatingDetails(editHighRange);
}

function EditSortOrder(editSortOrder) {
    EditRatingDetails(editSortOrder);
}

function EditRatingDetails(editRatingValues) {
    var grdMeritGrid = $("#grdConfigureRating").data("kendoGrid");
    var row = editRatingValues.closest("tr");
    var rowIndex = $(row).closest("tr").index();
    var rowData = grdMeritGrid._data[rowIndex];
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../Rating/_RatingPopUpModify",
        type: "post",
        data: {
            __RequestVerificationToken: token,
            ratingNum: rowData.RatingNum,
            ratingID: rowData.RatingId,
            ratingDescription: rowData.RatingDescription,
            ratingType: rowData.RatingType,
            lowRange: rowData.LowRange,
            highRange: rowData.HighRange,
            ratingOrder: rowData.RatingOrder,
            minRange: rowData.MinRange,
            maxRange: rowData.MaxRange,
        },
        success: function (result) {
            $("#divRating").html(result);
            $("#divRating").modal('show');
        }
    });
}
function allowDecimalNumberOnlyInput(e, control) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}
function CloseAfterUpdateRating() {
    $("#grdConfigureRating").data("kendoGrid").dataSource.read();
    $("#divRating").modal('hide');
    Successmessage("Saved Successfully");
}


function ValidateInt(event) {
    var regex = new RegExp("[0-9]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}

function additionalParamInfo()
{
    return {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
    }
}
