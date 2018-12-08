var eitherLumporMerit = false;
var recalculateMerit = false;
var autoCalculate = false;
var meritOverRide = false;
var turnOff = false;
var TCC = false;
var EnableSSO = false;
var ratingDropdown = false;
var meritCalculation = false;
var allowSave = true;

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

$(document).ready(function () {
    $("#ProrationRuleEndDatepicker").bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $("#ProrationRuleEndDatepicker").keydown(function (event) { event.preventDefault(); });
    $("#ProrationRuleStartDatepicker").bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $("#ProrationRuleStartDatepicker").keydown(function (event) { event.preventDefault(); });
    //
    $("#mainemptydiv").hide();
    $("div.bhoechie-tab-menu>div.list-group>a").click(function (e) {
        e.preventDefault();
        $(this).siblings('a.active').removeClass("active");
        $(this).addClass("active");
        var index = $(this).index();
        $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
        $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");
    });
    getBusSetting();

    $('#chkSSONo').click(function () {
            $("#IPE").attr("readonly", true);
    });

    $('#chkSSOYes').click(function () {
        $("#IPE").removeAttr('readonly');
    })
});

$(document).on("change", "#ProrationDoNotProrate", function (e) {
    setPageChanged(true);
    $("#prorated1").hide();
    $("#prorated2").hide();
    $("#prorated3").hide();
    $("#prorated4").hide();
    $("#prorated5").hide();
});

$(document).on("change", "#ProrationProrate", function (e) {
    setPageChanged(true);
    $("#prorated1").show();
    $("#prorated2").show();
    $("#prorated3").show();
    $("#prorated4").show();
    $("#prorated5").show();

    $('#ProrationRuleStartDatepicker').data('kendoDatePicker').enable(true);
    $('#ProrationRuleEndDatepicker').data('kendoDatePicker').enable(true);
    $('#fldProrationRule').find('input').attr('disabled', false);
    if ($(this)[0].checked) {
        $('#ProrationDaily').attr('checked', true);
        $('#ProrateLength').val('365');
        $('#ProrateLengthtoInclude').val('');
    }
});


$(document).on("change", "#BaseCurrency", function (e) {
    setPageChanged(true);
});


$(document).on("change", "#OtherCurrency", function (e) {
    setPageChanged(true);
});


$(document).on("change", "#MeritOverrideHardStop", function (e) {
    setPageChanged(true);
    $('#meritsoftstop').find('input').attr('disabled', true);
    $('#merithardstop').find('input').attr('disabled', false);
    $('#meritsoftstop').css('color', '#A0A0A0');
    $('#merithardstop').css('color', 'black');
    $(':input', '#meritsoftstop').val('').removeAttr('checked').removeAttr('selected');
    $(':checkbox, :radio', '#meritsoftstop').prop('checked', false);
});

$(document).on("change", "#MeritOverrideSoftStop", function (e) {
    setPageChanged(true);
    $('#meritsoftstop').find('input').attr('disabled', false);
    $('#merithardstop').find('input').attr('disabled', true);
    $('#merithardstop').css('color', '#A0A0A0');
    $('#meritsoftstop').css('color', 'black');
    $(':input', '#merithardstop').val('').removeAttr('checked').removeAttr('selected');
    $(':checkbox, :radio', '#merithardstop').prop('checked', false);
});

$(document).on("change", "#HardNoExceedGuideline,#HardExceedGuideline,#SoftStopMandatory,#SoftStopNoMandatory", function (e) {
    setPageChanged(true);
});

$(document).on("change", "#MeritOverrideNoJustifiction", function (e) {
    setPageChanged(true);
    $(':input', '#meritsoftstop').val('').removeAttr('checked').removeAttr('selected');
    $(':checkbox, :radio', '#meritsoftstop').prop('checked', false);
    $(':input', '#merithardstop').val('').removeAttr('checked').removeAttr('selected');
    $(':checkbox, :radio', '#merithardstop').prop('checked', false);
});

$(document).on("click", "#ProrationDaily", function (e) {
    setPageChanged(true);
    $('#ProrateLength').val('365');
    $('#ProrateLengthtoInclude').val('');
});
$(document).on("click", "#ProrationWeekly", function (e) {
    setPageChanged(true);
    $('#ProrateLength').val('52');
    $('#ProrateLengthtoInclude').val('');
});
$(document).on("click", "#ProrationMonthly", function (e) {
    setPageChanged(true);
    $('#ProrateLength').val('12');
    $('#ProrateLengthtoInclude').val('16');
});



$(document).on("change", "#ProrateLength", function (e) {
    setPageChanged(true);
});
$(document).on("click", "#chkMeritFeature", function (e) {
    setPageChanged(true);
    enableMerit($(this)[0].checked);
    if ($(this)[0].checked) {
        $('#HardNoExceedGuideline').attr('checked', true);
        $('#ProrationDoNotProrate').attr('checked', true);
    }
    $('#ProrationDoNotProrate').trigger('change');
    enableDisableEitherMeritOrLumpsum();
});

$(document).on("click", "#chkAdjustmentFeature", function (e) {
    setPageChanged(true);
    enableAdjustment($(this)[0].checked);
});

$(document).on("click", "#chkPromotion", function (e) {
    setPageChanged(true);
    enablePromotion($(this)[0].checked);
});

$(document).on("click", "#chkMultiCurrency", function (e) {
    setPageChanged(true);
    enableMulticurrency($(this)[0].checked);
});

$(document).on("click", "#chkBonus", function (e) {
    setPageChanged(true);
    enableBonus($(this)[0].checked);
});

$(document).on("click", "#chkRatingFeature", function (e) {
    setPageChanged(true);
    enableRating($(this)[0].checked);
});

$(document).on("click", "#chkWorkFlowFeature", function (e) {
    setPageChanged(true);
    enableWorkFlow($(this)[0].checked);
});


$(document).on("click", "#btnSaveRule", function (e) {
    if (allowSave) {
        saveRules();
    }
});

$(document).on("click", "#btnExecute", function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../ManageRules/clearApprovalDetails',
        type: 'post',
        data: ({ __RequestVerificationToken: token }),
        success: function (result) {
            (result = 1) ? Successmessage("Workflow information cleared successfully") : '';
        }

    });
});

function enableWorkFlow(status) {
    if (status) {
        $("#divWorkFlowFeature").removeClass('tilesec tile bg-white');
        $("#divWorkFlowFeature").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconWorkFlowFeature").removeClass('iconnotselected');
        $("#lblWorkFlowFeature").removeClass('namewhite');
    }
    else {
        $("#divWorkFlowFeature").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divWorkFlowFeature").addClass('tilesec tile bg-white');
        $("#iconWorkFlowFeature").addClass('iconnotselected');
        $("#lblWorkFlowFeature").addClass('namewhite');
    }
}

function enableRating(status) {
    if (status) {
        $('#chkRatingDropdown').prop('disabled', false);
        $("#divRatingFeature").removeClass('tilesec tile bg-white');
        $("#divRatingFeature").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconRatingFeature").removeClass('iconnotselected');
        $("#lblRatingFeature").removeClass('namewhite');
    }
    else {
        $('#chkRatingDropdown').prop('disabled', true);
        $('#chkRatingDropdown').prop('checked', false);
        $("#divRatingFeature").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divRatingFeature").addClass('tilesec tile bg-white');
        $("#iconRatingFeature").addClass('iconnotselected');
        $("#lblRatingFeature").addClass('namewhite');
    }
}

function enableMulticurrency(status) {
    if (status) {
        $("#divMultiCurrency").removeClass('tilesec tile bg-white');
        $("#divMultiCurrency").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconMultiCurrency").removeClass('iconnotselected');
        $("#lblMultiCurrency").removeClass('namewhite');
    }
    else {
        $("#divMultiCurrency").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divMultiCurrency").addClass('tilesec tile bg-white');
        $("#iconMultiCurrency").addClass('iconnotselected');
        $("#lblMultiCurrency").addClass('namewhite');
    }
}

function enableBonus(status) {
    if (status) {
        $("#divBonus").removeClass('tilesec tile bg-white');
        $("#divBonus").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconBonus").removeClass('iconnotselected');
        $("#lblBonus").removeClass('namewhite');
    }
    else {
        $("#divBonus").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divBonus").addClass('tilesec tile bg-white');
        $("#iconBonus").addClass('iconnotselected');
        $("#lblBonus").addClass('namewhite');
    }
}

function enablePromotion(status) {
    if (status) {
        $("#divPromotion").removeClass('tilesec tile bg-white');
        $("#divPromotion").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconPromotion").removeClass('iconnotselected');
        $("#lblPromotion").removeClass('namewhite');
    }
    else {
        $("#divPromotion").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divPromotion").addClass('tilesec tile bg-white');
        $("#iconPromotion").addClass('iconnotselected');
        $("#lblPromotion").addClass('namewhite');
    }
    $("#ddlRoundpromotionPerc").data("kendoDropDownList").enable(status);
    $("#ddlRoundPromotionHour").data("kendoDropDownList").enable(status);
    $("#ddlRoundPromotionAnnual").data("kendoDropDownList").enable(status);
    $("#ddlDecimalPromotionPerc").data("kendoDropDownList").enable(status);
    $("#ddlDecimalPromotionHour").data("kendoDropDownList").enable(status);
    $("#ddlDecimalPromotionAnnual").data("kendoDropDownList").enable(status);

}

function enableDisableEitherMeritOrLumpsum() {
    if ($('#chkMeritFeature')[0].checked && $('#chkLumpsumFeature')[0].checked) {
        $('#divEitherMeritOrLumpsumYes').find('input').attr('disabled', false);
    }
    else {
        $('#eitherMeritOrLumpsumYes')[0].checked = false;
        $('#eitherMeritOrLumpsumNo')[0].checked = true;
        $('#divEitherMeritOrLumpsumYes').find('input').attr('disabled', true);
    }
}

function enableAdjustment(status) {
    if (status) {
        $("#divAdjustmentFeature").removeClass('tilesec tile bg-white');
        $("#divAdjustmentFeature").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconAdjustmentFeature").removeClass('iconnotselected');
        $("#lblAdjustmentFeature").removeClass('namewhite');
    }
    else {
        $("#divAdjustmentFeature").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divAdjustmentFeature").addClass('tilesec tile bg-white');
        $("#iconAdjustmentFeature").addClass('iconnotselected');
        $("#lblAdjustmentFeature").addClass('namewhite');
    }
    $("#ddlRoundAdjustmentPerc").data("kendoDropDownList").enable(status);
    $("#ddlRoundAdjustmentHour").data("kendoDropDownList").enable(status);
    $("#ddlRoundAdjustmentAnnual").data("kendoDropDownList").enable(status);
    $("#ddlDecimalAdjustmentPerc").data("kendoDropDownList").enable(status);
    $("#ddlDecimalAdjustmentHour").data("kendoDropDownList").enable(status);
    $("#ddlDecimalAdjustmentAnnual").data("kendoDropDownList").enable(status);
}

function enableMerit(status) {
    if (status) {
        $('#fldMeritOverideRule').find('input').attr('disabled', false);
        $('#merithardstop').css('color', 'black');
        $('#meritsoftstop').css('color', 'black');
        $('#meritJustifyonSubmit').css('color', 'black');
        $('#ProrationDoNotProrate').attr('disabled', false);
        $('#ProrationProrate').attr('disabled', false);
        $("#divMeritFeature").removeClass('tilesec tile bg-white');
        $("#divMeritFeature").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconMeritFeature").removeClass('iconnotselected');
        $("#lblMeritFeature").removeClass('namewhite');
        $('#fldProrationRule').find('input').attr('disabled', false);
        $('#fldProrationRule').css('color', '#555');
        (TCC == false) ? $("#chkTCCNo")[0].checked = true : $("#chkTCCYes")[0].checked = true;
        (EnableSSO == false) ? $("#chkSSONo")[0].checked = true : $("#chkSSOYes")[0].checked = true;
        (ratingDropdown == false) ? $("#RatingDropdownNo")[0].checked = true : $("#RatingDropdownYes")[0].checked = true;
        (meritCalculation == false) ? $("#meritCalcNo")[0].checked = true : $("#meritCalcYes")[0].checked = true;
    }
    else {
        $('#fldMeritOverideRule').find('input').attr('checked', false);
        $('#fldMeritOverideRule').find('input').attr('disabled', true);
        $('#fldProrationRule').find('input').attr('checked', false);
        $('#fldProrationRule').find('input').attr('disabled', true);
        $('#merithardstop').css('color', '#A0A0A0');
        $('#meritsoftstop').css('color', '#A0A0A0');
        $('#meritJustifyonSubmit').css('color', '#A0A0A0');
        $('#fldProrationRule').find('input').attr('disabled', true);
        $('#ProrationRuleStartDatepicker').data('kendoDatePicker').enable(false);
        $('#ProrationRuleEndDatepicker').data('kendoDatePicker').enable(false);
        $('#fldProrationRule').css('color', '#A0A0A0');
        $(':input', '#fldProrationRule').val('').removeAttr('checked').removeAttr('selected');
        $(':checkbox, :radio', '#fldProrationRule').prop('checked', false);
        $("#divMeritFeature").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divMeritFeature").addClass('tilesec tile bg-white');
        $("#iconMeritFeature").addClass('iconnotselected');
        $("#lblMeritFeature").addClass('namewhite');
    }
    $("#ddlRoundMeritPerc").data("kendoDropDownList").enable(status);
    $("#ddlRoundMeritHour").data("kendoDropDownList").enable(status);
    $("#ddlRoundMeritAnnual").data("kendoDropDownList").enable(status);
    $("#ddlDecimalMeritPerc").data("kendoDropDownList").enable(status);
    $("#ddlDecimalMeritHour").data("kendoDropDownList").enable(status);
    $("#ddlDecimalMeritAnnual").data("kendoDropDownList").enable(status);
    var statusAdjustment = $("#chkAdjustmentFeature")[0].checked;
    $("#ddlRoundCompoRatioPerc").data("kendoDropDownList").enable(status || statusAdjustment);
    $("#ddlRoundCompoRatioHour").data("kendoDropDownList").enable(status || statusAdjustment);
    $("#ddlRoundCompoRatioAnnual").data("kendoDropDownList").enable(status || statusAdjustment);
    $("#ddlDecimalCompoRatioPerc").data("kendoDropDownList").enable(status || statusAdjustment);
    $("#ddlDecimalCompoRatioHour").data("kendoDropDownList").enable(status || statusAdjustment);
    $("#ddlDecimalCompoRatioAnnual").data("kendoDropDownList").enable(status || statusAdjustment);
}


function enableLumpSum(status) {
    if (status) {
        $('#fldLumpSumRule').find('input').attr('disabled', false);
        $('#fldLumpSumRule').css('color', '#555');
        $("#divLumpsumFeature").removeClass('tilesec tile bg-white');
        $("#divLumpsumFeature").addClass('tilesec tile bg-yellow-lemon selected');
        $("#iconLumpsumFeature").removeClass('iconnotselected');
        $("#lblLumpsumFeature").removeClass('namewhite');
        (eitherLumporMerit == false) ? $("#eitherMeritOrLumpsumNo")[0].checked = true : $("#eitherMeritOrLumpsumYes")[0].checked = true;
        (recalculateMerit == false) ? $("#meritReCalcNo")[0].checked = true : $("#meritReCalcYes")[0].checked = true;
        (autoCalculate == false) ? $("#LumpsumNoAutoCalc")[0].checked = true : $("#LumpsumAutoCalc")[0].checked = true;
        if (autoCalculate == false)
        {
            $("#LumpsumAutoCalcPct").attr("disabled", true);
            $("#LumpsumAutoCalcAmt").attr("disabled", true);
        }
        else {
            $("#LumpsumAutoCalcPct").removeAttr("disabled", true);
            $("#LumpsumAutoCalcAmt").removeAttr("disabled", true);
        }
        (meritOverRide == false) ? $("#LumpsumAutoCalcWithoutOverride")[0].checked = true : $("#LumpsumAutoCalcOverride")[0].checked = true;
        (turnOff == false) ? $("#Lumpsumoff")[0].checked = true : $("#Lumpsumon")[0].checked = true;
    }
    else {
        $('#fldLumpSumRule').find('input').attr('checked', false);
        $('#fldLumpSumRule').find('input').attr('disabled', true);
        $('#fldLumpSumRule').css('color', '#A0A0A0');
        $("#divLumpsumFeature").removeClass('tilesec tile bg-yellow-lemon selected');
        $("#divLumpsumFeature").addClass('tilesec tile bg-white');
        $("#iconLumpsumFeature").addClass('iconnotselected');
        $("#lblLumpsumFeature").addClass('namewhite');        
    }
    $("#ddlRoundLumpSumPerc").data("kendoDropDownList").enable(status);
    $("#ddlRoundLumpSumHour").data("kendoDropDownList").enable(status);
    $("#ddlRoundLumpSumAnnual").data("kendoDropDownList").enable(status);
    $("#ddlDecimalLumpSumPerc").data("kendoDropDownList").enable(status);
    $("#ddlDecimalLumpSumHour").data("kendoDropDownList").enable(status);
    $("#ddlDecimalLumpSumAnnual").data("kendoDropDownList").enable(status);
}

function enableEitherLumpOrMerit(status) {
    if (status) {
        $('#fldLumpSumRule1').find('input').attr('checked', false);
        $('#fldLumpSumRule1').find('input').attr('disabled', true);
        $('#fldLumpSumRule1').css('color', '#A0A0A0');
        $('#fldLumpSumRule2').find('input').attr('checked', false);
        $('#fldLumpSumRule2').find('input').attr('disabled', true);
        $('#fldLumpSumRule2').css('color', '#A0A0A0');
        $('#fldLumpSumRule3').find('input').attr('checked', false);
        $('#fldLumpSumRule3').find('input').attr('disabled', true);
        $('#fldLumpSumRule3').css('color', '#A0A0A0');
        $('#fldLumpSumRule4').find('input').attr('checked', false);
        $('#fldLumpSumRule4').find('input').attr('disabled', true);
        $('#fldLumpSumRule4').css('color', '#A0A0A0');
        $('#fldLumpSumRule5').find('input').attr('checked', false);
        $('#fldLumpSumRule5').find('input').attr('disabled', true);
        $('#fldLumpSumRule5').css('color', '#A0A0A0');
        $(':input', '#LumpsumAutoCalcRangeText').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcOverrideRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
    }
}

function selectMeritTile() {
    $("#chkMeritFeature").trigger('click');
}

function selectPerfRatingTile() {
    $("#chkRatingFeature").trigger('click');
}

function selectLumpSumTile() {
    $("#chkLumpsumFeature").trigger('click');
}

function selectPromotionTile() {
    $("#chkPromotion").trigger('click');
}

function selectAdjustmentTile() {
    $("#chkAdjustmentFeature").trigger('click');
}

function selectWorkFlowTile() {
    $("#chkWorkFlowFeature").trigger('click');
}

function selectMultiCurrencyTile() {
    $("#chkMultiCurrency").trigger('click');
}
function selectBonusTile() {
    $("#chkBonus").trigger('click');
}
function onChange(x) {
    setPageChanged(true);
}
function allowDecimalNumberOnlyInput(e, control) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        return;
    }

    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
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

function allownumbers(e, t) {
    var cultureCode = "en-US";
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, t);
}

function onlyNos(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
    catch (err) {
        alert(err.Description);
    }
}
function getNumberValue(value, culture) {
    var result;
    if (arguments.length == 1 || (arguments.length > 1 && arguments[1] == undefined))
        result = parseFloat(value);
    else result = kendo.parseFloat(value, culture);
    return isNumber(result) ? result : null;
}
function isNumber(value) {
    return (typeof value == "number" && !isNaN(value));
}


function setPageChanged(isPageChanged) {
    objChangeFlag = isPageChanged;
}

function getBusSetting() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../ManageRules/GetBusSetting',
        type: "post",
        data: ({__RequestVerificationToken:token}),
        success: function (busSettingModel) {
            busSettingModel.Merit == "YES" ? $('#chkMeritFeature')[0].checked = true : $('#chkMeritFeature')[0].checked = false;
            busSettingModel.WorkFlow == "YES" ? $('#chkWorkFlowFeature')[0].checked = true : $('#chkWorkFlowFeature')[0].checked = false,
            busSettingModel.Adjustment == "YES" ? $('#chkAdjustmentFeature')[0].checked = true : $('#chkAdjustmentFeature')[0].checked = false;
            busSettingModel.Lumpsum == "YES" ? $('#chkLumpsumFeature')[0].checked = true : $('#chkLumpsumFeature')[0].checked = false;
            busSettingModel.RatingDisplay == "YES" ? $('#chkRatingFeature')[0].checked = true : $('#chkRatingFeature')[0].checked = false;
            busSettingModel.MultiCurrency == "YES" ? $('#chkMultiCurrency')[0].checked = true : $('#chkMultiCurrency')[0].checked = false;
            busSettingModel.Promotion == "YES" ? $('#chkPromotion')[0].checked = true : $('#chkPromotion')[0].checked = false;
            busSettingModel.CurrencyCode == "YES" ? $('#radioCurrencyCode')[0].checked = true : $('#radioCurrencySymbol')[0].checked = true;
            busSettingModel.MeritCalculation == "YES" ? $('#meritCalcYes')[0].checked = true : $('#meritCalcNo')[0].checked = true;
            busSettingModel.MeritReCalculation == "YES" ? $('#meritReCalcYes')[0].checked = true : $('#meritReCalcNo')[0].checked = true;
            busSettingModel.RatingDropdown == "YES" ? $('#RatingDropdownYes')[0].checked = true : $('#RatingDropdownNo')[0].checked = true;
            busSettingModel.ComparativeRatio == "YES" ? $('#CompaRatioDropdownYes')[0].checked = true : $('#CompaRatioDropdownNo')[0].checked = true;
            busSettingModel.EitherMeritOrLumpSum == "YES" ? $('#eitherMeritOrLumpsumYes')[0].checked = true : $('#eitherMeritOrLumpsumNo')[0].checked = true;
            busSettingModel.TCC == "YES" ? $('#chkTCCYes')[0].checked = true : $('#chkTCCNo')[0].checked = true;
            busSettingModel.EnableSSO == "YES" ? $('#chkSSOYes')[0].checked = true : $('#chkSSONo')[0].checked = true;
            busSettingModel.AutoCalculateLumpSum == "YES" ? $('#LumpsumAutoCalcOverride')[0].checked = true : $('#LumpsumAutoCalcWithoutOverride')[0].checked = true;
            busSettingModel.LumpSumRuleTurnOff == "YES" ? $('#Lumpsumon')[0].checked = true : $('#Lumpsumoff')[0].checked = true;
            busSettingModel.LumpsumType == "AutoCalc" ? $('#LumpsumAutoCalc')[0].checked = true : $('#LumpsumNoAutoCalc')[0].checked = true;
            (busSettingModel.MandatoryJustification == "YES" && busSettingModel.MeritOverrideSoftStop == "YES") ? $('#SoftStopMandatory')[0].checked = true : $('#SoftStopMandatory')[0].checked = false;
            (busSettingModel.MandatoryJustification == "NO" && busSettingModel.MeritOverrideSoftStop == "YES") ? $('#SoftStopNoMandatory')[0].checked = true : $('#SoftStopNoMandatory')[0].checked = false;

            eitherLumporMerit = (busSettingModel.EitherMeritOrLumpSum == "YES");
            recalculateMerit = (busSettingModel.MeritReCalculation == "YES");
            autoCalculate = (busSettingModel.LumpsumType == "AutoCalc");
            meritOverRide = (busSettingModel.AutoCalculateLumpSum == "YES");
            turnOff = (busSettingModel.LumpSumRuleTurnOff == "YES");
            TCC = (busSettingModel.TCC == "YES");
            EnableSSO = (busSettingModel.EnableSSO == "YES");
            ratingDropdown = (busSettingModel.RatingDropdown == "YES");
            meritCalculation = (busSettingModel.MeritCalculation == "YES");

            $('#LumpsumAutoCalcPct')[0].value = busSettingModel.RangeMaxPct;
            $('#LumpsumAutoCalcAmt')[0].value = busSettingModel.RangeMaxAmt;
            $("#ddlRoundMeritPerc").data("kendoDropDownList").value(busSettingModel.RoundingMeritPct);
            $("#ddlRoundMeritHour").data("kendoDropDownList").value(busSettingModel.RoundingMeritHourly);
            $("#ddlRoundMeritAnnual").data("kendoDropDownList").value(busSettingModel.RoundingMeritAnnual);
            $("#ddlDecimalMeritPerc").data("kendoDropDownList").value(busSettingModel.DecimalMeritPct);
            $("#ddlDecimalMeritHour").data("kendoDropDownList").value(busSettingModel.DecimalMeritHourly);
            $("#ddlDecimalMeritAnnual").data("kendoDropDownList").value(busSettingModel.DecimalMeritAnnual);
            $("#ddlRoundLumpSumPerc").data("kendoDropDownList").value(busSettingModel.RoundingLumpSumPct);
            $("#ddlRoundLumpSumHour").data("kendoDropDownList").value(busSettingModel.RoundingLumpSumHourly);
            $("#ddlRoundLumpSumAnnual").data("kendoDropDownList").value(busSettingModel.RoundingLumpSumAnnual);
            $("#ddlDecimalLumpSumPerc").data("kendoDropDownList").value(busSettingModel.DecimalLumpSumPct);
            $("#ddlDecimalLumpSumHour").data("kendoDropDownList").value(busSettingModel.DecimalLumpSumHourly);
            $("#ddlDecimalLumpSumAnnual").data("kendoDropDownList").value(busSettingModel.DecimalLumpSumAnnual);
            $("#ddlRoundAdjustmentPerc").data("kendoDropDownList").value(busSettingModel.RoundingAdjustmentPct);
            $("#ddlRoundAdjustmentHour").data("kendoDropDownList").value(busSettingModel.RoundingAdjustmentHourly);
            $("#ddlRoundAdjustmentAnnual").data("kendoDropDownList").value(busSettingModel.RoundingAdjustmentAnnual);
            $("#ddlDecimalAdjustmentPerc").data("kendoDropDownList").value(busSettingModel.DecimalAdjustmentPct);
            $("#ddlDecimalAdjustmentHour").data("kendoDropDownList").value(busSettingModel.DecimalAdjustmentHourly);
            $("#ddlDecimalAdjustmentAnnual").data("kendoDropDownList").value(busSettingModel.DecimalAdjustmentAnnual);
            $("#ddlRoundCompoRatioPerc").data("kendoDropDownList").value(busSettingModel.RoundingCompaRatioPct);
            $("#ddlRoundCompoRatioHour").data("kendoDropDownList").value(busSettingModel.RoundingCompaRatioHourly);
            $("#ddlRoundCompoRatioAnnual").data("kendoDropDownList").value(busSettingModel.RoundingCompaRatioAnnual);
            $("#ddlDecimalCompoRatioPerc").data("kendoDropDownList").value(busSettingModel.DecimalCompaRatioPct);
            $("#ddlDecimalCompoRatioHour").data("kendoDropDownList").value(busSettingModel.DecimalCompaRatioHourly);
            $("#ddlDecimalCompoRatioAnnual").data("kendoDropDownList").value(busSettingModel.DecimalCompaRatioAnnual);
            $("#ddlRoundNewSalaryPerc").data("kendoDropDownList").value(busSettingModel.RoundNewSalaryPct);
            $("#ddlRoundNewSalaryHour").data("kendoDropDownList").value(busSettingModel.RoundNewSalaryHourly);
            $("#ddlRoundNewSalaryAnnual").data("kendoDropDownList").value(busSettingModel.RoundNewSalaryAnnual);
            $("#ddlDecimalNewSalaryPerc").data("kendoDropDownList").value(busSettingModel.DecimalNewSalaryPct);
            $("#ddlDecimalNewSalaryHour").data("kendoDropDownList").value(busSettingModel.DecimalNewSalaryHourly);
            $("#ddlDecimalNewSalaryAnnual").data("kendoDropDownList").value(busSettingModel.DecimalNewSalaryAnnual);
            //Current Salary
            $("#ddlRoundCurrentSalaryPerc").data("kendoDropDownList").value(busSettingModel.RoundCurrentSalaryPct);
            $("#ddlRoundCurrentSalaryHour").data("kendoDropDownList").value(busSettingModel.RoundCurrentSalaryHourly);
            $("#ddlRoundCurrentSalaryAnnual").data("kendoDropDownList").value(busSettingModel.RoundCurrentSalaryAnnual);
            $("#ddlDecimalCurrentSalaryPerc").data("kendoDropDownList").value(busSettingModel.DecimalCurrentSalaryPct);
            $("#ddlDecimalCurrentSalaryHour").data("kendoDropDownList").value(busSettingModel.DecimalCurrentSalaryHourly);
            $("#ddlDecimalCurrentSalaryAnnual").data("kendoDropDownList").value(busSettingModel.DecimalCurrentSalaryAnnual);
            ////////////////
            $("#ddlRoundpromotionPerc").data("kendoDropDownList").value(busSettingModel.RoundingPromotionPct);
            $("#ddlRoundPromotionHour").data("kendoDropDownList").value(busSettingModel.RoundingPromotionHourly);
            $("#ddlRoundPromotionAnnual").data("kendoDropDownList").value(busSettingModel.RoundingPromotionAnnual);
            $("#ddlDecimalPromotionPerc").data("kendoDropDownList").value(busSettingModel.DecimalPromotionPct);
            $("#ddlDecimalPromotionHour").data("kendoDropDownList").value(busSettingModel.DecimalPromotionHourly);
            $("#ddlDecimalPromotionAnnual").data("kendoDropDownList").value(busSettingModel.DecimalPromotionAnnual);
            $("#ddlUserNameFormatConfiguration").data("kendoDropDownList").value(busSettingModel.UserNameFormat);
            $("#ddlEmployeeNameFormatConfiguration").data("kendoDropDownList").value(busSettingModel.EmployeeNameFormat);
            $("#ddlSortOrderFormatConfiguration").data("kendoDropDownList").value(busSettingModel.SortOrderEmployeeNameFormat);
            $("#ddlDateFormatConfiguration").data("kendoDropDownList").value(busSettingModel.DateFormat);


            $("#inpCurrentyear").val(busSettingModel.CurrentYear);
            $("#IPE").val(busSettingModel.IDPEndPoint);            
            $("#inpoldyear").val(busSettingModel.oldYear);
            $("#ddlPasswordLength").data("kendoDropDownList").value(busSettingModel.PasswordLength);
            $("#inpEmailAddress").val(busSettingModel.EmailAddress);
            $("#inpEmailPassword").val(busSettingModel.EmailPassword);
            $("#inpBonusIndividual").val(busSettingModel.BonusIndividualPortion);
            $("#inpBonusGlobal").val(busSettingModel.BonusGlobalPortion);
            $("#IPE").attr("readonly", (busSettingModel.EnableSSO == "NO"));
            

            if (busSettingModel.CurrencyFormat == "UserCurrency") {
                $('#BaseCurrency')[0].checked = true;
                $('#OtherCurrency')[0].checked = false;
            }
            else {
                $('#BaseCurrency')[0].checked = false;
                $('#OtherCurrency')[0].checked = true;
            }

            if (busSettingModel.RatingDisplay == "NO") {
                $('#chkRatingDropdown').prop('checked', false);
                $('#chkRatingDropdown').prop('disabled', true);
            }

            if (busSettingModel.Prorate == "NO") {
                $("#prorated1").hide();
                $("#prorated2").hide();
                $("#prorated3").hide();
                $("#prorated4").hide();
                $("#prorated5").hide();
            }

            enableMerit((busSettingModel.Merit == "YES"));
            enableLumpSum((busSettingModel.Lumpsum == "YES"));
            enableEitherLumpOrMerit((busSettingModel.EitherMeritOrLumpSum == "YES"));
            enableAdjustment((busSettingModel.Adjustment == "YES"));
            enablePromotion((busSettingModel.Promotion == "YES"));
            enableMulticurrency((busSettingModel.MultiCurrency == "YES"));
            enableRating((busSettingModel.RatingDisplay == "YES"));
            enableWorkFlow((busSettingModel.WorkFlow == "YES"));
            enableBonus((busSettingModel.Bonus == "YES"));

            if (busSettingModel.Merit == "YES") {
                (busSettingModel.Prorate == "YES") ? $('#ProrationProrate')[0].checked = true : $('#ProrationProrate')[0].checked = false;
                (busSettingModel.Prorate == "NO") ? $('#ProrationDoNotProrate')[0].checked = true : $('#ProrationDoNotProrate')[0].checked = false;
                busSettingModel.ApplyBudgetCalculations == "YES" ? $('#chkBudgetCalc')[0].checked = true : $('#chkBudgetCalc')[0].checked = false;
                if (busSettingModel.Prorate == "YES") {
                    $("#ProrationRuleStartDatepicker")[0].value = busSettingModel.ProrateIncreaseStartDate;
                    $("#ProrationRuleEndDatepicker")[0].value = busSettingModel.ProrateIncreaseEndDate;
                }

                busSettingModel.ProrationType == "Daily" ? $('#ProrationDaily')[0].checked = true : $('#ProrationDaily')[0].checked = false;
                busSettingModel.ProrationType == "Weekly" ? $('#ProrationWeekly')[0].checked = true : $('#ProrationWeekly')[0].checked = false;
                busSettingModel.ProrationType == "Monthly" ? $('#ProrationMonthly')[0].checked = true : $('#ProrationMonthly')[0].checked = false;
                busSettingModel.ProrationLength != 0 ? $('#ProrateLength').val(busSettingModel.ProrationLength) : $('#ProrateLength').val('');
                busSettingModel.ProrationLengthtoInclude != 0 ? $('#ProrateLengthtoInclude').val(busSettingModel.ProrationLengthtoInclude) : $('#ProrateLengthtoInclude').val('');
                busSettingModel.MeritOverrideNoJustification == "YES" ? $('#MeritOverrideNoJustifiction')[0].checked = true : $('#MeritOverrideNoJustifiction')[0].checked = false;
                (busSettingModel.MeritIncreaseWithinGuideline == "YES" && busSettingModel.MeritOverrideHardStop == "YES") ? $('#HardNoExceedGuideline')[0].checked = true : $('#HardNoExceedGuideline')[0].checked = false;
                (busSettingModel.MeritIncreaseWithinGuideline == "NO" && busSettingModel.MeritOverrideHardStop == "YES") ? $('#HardExceedGuideline')[0].checked = true : $('#HardExceedGuideline')[0].checked = false;
            }


            setRulesPageLoad();
        }

    });
}

function setRulesPageLoad() {
    if ($('#LumpsumAutoCalc')[0].checked) {
        $("#Lumpsumon").removeAttr("disabled", true);
        $("#Lumpsumoff").removeAttr("disabled", true);
        $("#LumpsumAutoCalcOverride").removeAttr("disabled", true);
        $("#LumpsumAutoCalcWithoutOverride").removeAttr("disabled", true);
        if (($("#LumpsumAutoCalcPct").val() != "") && ($("#LumpsumAutoCalcAmt").val() == 0)) {
            $("#LumpsumAutoCalcAmt").attr("disabled", true);
            $("#LumpsumAutoCalcAmt").val("");
            $("#LumpsumAutoCalcPct").removeAttr("disabled", true);
        }
        else {
            $("#LumpsumAutoCalcPct").attr("disabled", true);
            $("#LumpsumAutoCalcPct").val("");
            $("#LumpsumAutoCalcAmt").removeAttr("disabled", true);
        }
    }
    if ($('#LumpsumNoAutoCalc')[0].checked) {
        $("#LumpsumAutoCalcPct").attr("disabled", true);
        $("#LumpsumAutoCalcAmt").attr("disabled", true);
        $("#LumpsumAutoCalcOverride").attr("disabled", true);
        $("#LumpsumAutoCalcWithoutOverride").attr("disabled", true);
        $("#Lumpsumon").attr("disabled", true);
        $("#Lumpsumoff").attr("disabled", true);
    }
}
$(document).on("click", "#nameformat", function (e) {
    $("#mainemptydiv").show();
});

$(document).on("click", ".noemptydiv", function (e) {
    $("#mainemptydiv").hide();
});

$(document).on("change", "#LumpsumAutoCalc", function (e) {
    setPageChanged(true);
    $('#LumpsumAutoCalcRangeText').find('input').attr('disabled', false);
    $('#LumpsumAutoCalcRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcOverrideRangeTextValue').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcRangeText').css('color', 'black');
    $(':input', '#LumpsumAutoCalcRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
    $("#Lumpsumon").removeAttr("disabled", true);
    $("#Lumpsumoff").removeAttr("disabled", true);
    $("#LumpsumAutoCalcOverride").removeAttr("disabled", true);
    $("#LumpsumAutoCalcWithoutOverride").removeAttr("disabled", true);
    
    if (($("#LumpsumAutoCalcPct").val() != "") && ($("#LumpsumAutoCalcAmt").val() == 0)) {
        $("#LumpsumAutoCalcAmt").attr("disabled", true);
        $("#LumpsumAutoCalcAmt").val("");
        $("#LumpsumAutoCalcPct").removeAttr("disabled", true);
    }
    else {
        $("#LumpsumAutoCalcPct").attr("disabled", true);
        $("#LumpsumAutoCalcPct").val("");
        $("#LumpsumAutoCalcAmt").removeAttr("disabled", true);
    }
});

$(document).on("change", "#LumpsumAutoCalcPct", function (e) {
    if ($("#LumpsumAutoCalcPct")[0].value == "") {
        $("#LumpsumAutoCalcAmt").removeAttr("disabled", true);

    }
    else {
        $("#LumpsumAutoCalcAmt")[0].value = "";
        $("#LumpsumAutoCalcAmt").attr("disabled", true);
    }


});

$(document).on("change", "#LumpsumAutoCalcAmt", function (e) {
    if ($("#LumpsumAutoCalcAmt")[0].value == "") {
        $("#LumpsumAutoCalcPct").removeAttr("disabled", true);
    }
    else {
        $("#LumpsumAutoCalcPct")[0].value = "";
        $("#LumpsumAutoCalcPct").attr("disabled", true);
    }
});

$(document).on("change", "#LumpsumAutoCalcOverride", function (e) {
    setPageChanged(true);
    $('#LumpsumAutoCalcRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').find('input').attr('disabled', false);
    $('#LumpsumAutoCalcOverrideRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcRangeText').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcOverrideRangeText').css('color', 'black');
    $(':input', '#LumpsumAutoCalcRangeText').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
});

$(document).on("change", "#LumpsumNoAutoCalc", function (e) {
  
    setPageChanged(true);
    $('#LumpsumAutoCalcRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcRangeText').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcRangeText1').css('color', '#A0A0A0');
    $(':input', '#LumpsumAutoCalcRangeText').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
    $("#Lumpsumon").attr("disabled", true);
    $("#Lumpsumoff").attr("disabled", true);
    $("#LumpsumAutoCalcOverride").attr("disabled", true);
    $("#LumpsumAutoCalcWithoutOverride").attr("disabled", true);
    //
    $('#LumpsumAutoCalcWithoutOverride')[0].checked = true;
    $("#Lumpsumoff")[0].checked = true;
    //
    $("#LumpsumAutoCalcPct").attr("disabled", true);
    $("#LumpsumAutoCalcAmt").attr("disabled", true);
});

$(document).on("change", "#LumpsumAutoCalc1", function (e) {
    setPageChanged(true);
    $('#LumpsumAutoCalcRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcRangeTextValue').find('input').attr('disabled', false);
    $('#LumpsumAutoCalcOverrideRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcOverrideRangeTextValue').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcRangeTextValue').css('color', 'black');
    $(':input', '#LumpsumAutoCalcRangeText').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
});

$(document).on("change", "#LumpsumAutoCalcOverride1", function (e) {
    setPageChanged(true);
    $('#LumpsumAutoCalcRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeTextValue').find('input').attr('disabled', false);
    $('#LumpsumAutoCalcRangeText').css('color', '#A0A0A0');
    $('#LumpsumAutoCalcOverrideRangeTextValue').css('color', 'black');
    $(':input', '#LumpsumAutoCalcRangeText').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
    $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
});


$(document).on("click", "#chkLumpsumFeature", function (e) {
    setPageChanged(true);
    enableLumpSum($(this)[0].checked);
    if ($(this)[0].checked)
        $('#LumpsumNoAutoCalc').attr('checked', true);
    else
        $('#LumpsumAutoCalcRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcRangeTextValue').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeText').find('input').attr('disabled', true);
    $('#LumpsumAutoCalcOverrideRangeTextValue').find('input').attr('disabled', true);

    enableDisableEitherMeritOrLumpsum();
});

$(document).on("change", "#eitherMeritOrLumpsumYes", function (e) {
    setPageChanged(true);
    if ($(this)[0].checked) {
        $('#fldLumpSumRule1').find('input').attr('checked', false);
        $('#fldLumpSumRule1').find('input').attr('disabled', true);
        $('#fldLumpSumRule1').css('color', '#A0A0A0');
        $('#fldLumpSumRule2').find('input').attr('checked', false);
        $('#fldLumpSumRule2').find('input').attr('disabled', true);
        $('#fldLumpSumRule2').css('color', '#A0A0A0');
        $('#fldLumpSumRule3').find('input').attr('checked', false);
        $('#fldLumpSumRule3').find('input').attr('disabled', true);
        $('#fldLumpSumRule3').css('color', '#A0A0A0');
        $('#fldLumpSumRule4').find('input').attr('checked', false);
        $('#fldLumpSumRule4').find('input').attr('disabled', true);
        $('#fldLumpSumRule4').css('color', '#A0A0A0');
        $('#fldLumpSumRule5').find('input').attr('checked', false);
        $('#fldLumpSumRule5').find('input').attr('disabled', true);
        $('#fldLumpSumRule5').css('color', '#A0A0A0');
        $(':input', '#LumpsumAutoCalcRangeText').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcOverrideRangeText').val('').removeAttr('checked').removeAttr('selected');
        $(':input', '#LumpsumAutoCalcOverrideRangeTextValue').val('').removeAttr('checked').removeAttr('selected');
    }
});

$(document).on("change", "#eitherMeritOrLumpsumNo", function (e) {
    setPageChanged(true);
    if ($(this)[0].checked) {
        $('#LumpsumNoAutoCalc').attr('checked', true);
        $('#fldLumpSumRule1').find('input').attr('disabled', false);
        $('#fldLumpSumRule1').css('color', 'black');
        $('#fldLumpSumRule2').find('input').attr('disabled', false);
        $('#fldLumpSumRule2').css('color', 'black');
        $('#fldLumpSumRule3').find('input').attr('disabled', false);
        $('#fldLumpSumRule3').css('color', 'black');
        $('#fldLumpSumRule4').find('input').attr('disabled', false);
        $('#fldLumpSumRule4').css('color', 'black');
        $('#fldLumpSumRule5').find('input').attr('disabled', false);
        $('#fldLumpSumRule5').css('color', 'black');
        $("#meritReCalcNo")[0].checked = true;
        $("#LumpsumAutoCalcWithoutOverride")[0].checked = true;
        $("#Lumpsumoff")[0].checked = true;
        $("#LumpsumAutoCalcPct").attr("disabled", true);
        $("#LumpsumAutoCalcAmt").attr("disabled", true);        
    }
});

 $(document).on("click", "#ProrationProrate", function (event) {
        $("#proratecontent").css("display","inline")
    });
    $(document).on("click", "#ProrationDoNotProrate", function (event) {
        $("#proratecontent").css("display", "none")
    });

    $(document).on("click", "#CompaRatioDropdownYes", function (e) {
        if (!showConfirm(e, "Are you sure you want to make this change? The current compa-ratio you have uploaded will be cleared out and current compa ratio will be recalculated. You cannot retrieve this data.")) return false;
    });

function saveRules() {
    if ($("#ProrationProrate")[0].checked) {
        if ($("#ProrationRuleStartDatepicker").val() == "" || $("#ProrationRuleEndDatepicker").val() == "") {
            alert("Please provide proration date falling between in Proration rule");
            return false;
        }
        if ($("#ProrateLength").val() == "") {
            alert("Please provide Input length of calendar value in Proration rule");
            return false;
        }
        if ($("#ProrationMonthly")[0].checked && $("#ProrateLengthtoInclude").val() == "") {
            alert("Please provide No of days an employee should have worked in a given calendar month to include that month value in Proration rule");
            return false;
        }
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../ManageRules/UpdateBusSetting',
        type: "post",
        data: {
            __RequestVerificationToken:token,
            Merit: $('#chkMeritFeature')[0].checked ? "YES" : "NO",
            Bonus: $('#chkBonus')[0].checked ? "YES" : "NO",
            Adjustment: $('#chkAdjustmentFeature')[0].checked ? "YES" : "NO",
            Lumpsum: $('#chkLumpsumFeature')[0].checked ? "YES" : "NO",
            TCC: $('#chkTCCYes')[0].checked ? "YES" : "NO",
            EnableSSO: $('#chkSSOYes')[0].checked ? "YES" : "NO",
            RatingDisplay: $('#chkRatingFeature')[0].checked ? "YES" : "NO",
            WorkFlow: $('#chkWorkFlowFeature')[0].checked ? "YES" : "NO",
            RatingDropdown: $('#RatingDropdownYes')[0].checked ? "YES" : "NO",
            MultiCurrency: $('#chkMultiCurrency')[0].checked ? "YES" : "NO",
            Promotion: $('#chkPromotion')[0].checked ? "YES" : "NO",
            CurrencyCode: $('#radioCurrencyCode')[0].checked ? "YES" : "NO",
            MeritCalculation: $('#meritCalcYes')[0].checked ? "YES" : "NO",
            MeritReCalculation: $('#meritReCalcYes')[0].checked ? "YES" : "NO",
            EitherMeritOrLumpSum: $('#eitherMeritOrLumpsumYes')[0].checked ? "YES" : "NO",
            Prorate: $('#ProrationProrate')[0].checked ? "YES" : $('#ProrationDoNotProrate')[0].checked ? "NO" : "NO",
            AutoCalculateLumpSum: $('#LumpsumAutoCalcOverride')[0].checked ? "YES" : $('#LumpsumAutoCalcOverride')[0].checked ? "NO" : "NO",
            LumpSumRuleTurnOff: $('#Lumpsumon')[0].checked ? "YES" : $('#Lumpsumoff')[0].checked ? "NO" : "NO",
            LumpsumType: $('#LumpsumAutoCalc')[0].checked ? "AutoCalc" : $('#LumpsumNoAutoCalc')[0].checked ? "NoAutoCalc" : "NoAutoCalc",
            ApplyBudgetCalculations: $('#chkBudgetCalc')[0].checked ? "YES" : "NO",
            ComparativeRatio:$('#CompaRatioDropdownYes')[0].checked ? "YES":$('#CompaRatioDropdownNo')[0].checked ? "NO":"NO",
            ProrateIncreaseStartDate: $('#ProrationRuleStartDatepicker')[0].value != "" ? $('#ProrationRuleStartDatepicker')[0].value : "1/1/2015",
            ProrateIncreaseEndDate: $('#ProrationRuleEndDatepicker')[0].value != "" ? $('#ProrationRuleEndDatepicker')[0].value : "1/1/2015",
            ProrationType: $('#ProrationDaily')[0].checked ? "Daily" : $('#ProrationWeekly')[0].checked ? "Weekly" : $('#ProrationMonthly')[0].checked ? "Monthly" : "NO",
            ProrationLength: $('#ProrateLength')[0].value != "" ? $('#ProrateLength')[0].value : 0,
            ProrationLengthtoInclude: $('#ProrateLengthtoInclude')[0].value != "" ? $('#ProrateLengthtoInclude')[0].value : 0,
            RangeMaxPct: $('#LumpsumAutoCalc')[0].checked ? ($('#LumpsumAutoCalcPct')[0].value != "" ? $('#LumpsumAutoCalcPct')[0].value : 0) : $('#LumpsumAutoCalcPct')[0].value,
            RangeMaxAmt: $('#LumpsumAutoCalc')[0].checked ? ($('#LumpsumAutoCalcAmt')[0].value != "" ? $('#LumpsumAutoCalcAmt')[0].value : 0) : $('#LumpsumAutoCalcAmt')[0].value,
            MeritOverrideHardStop: $('#HardNoExceedGuideline')[0].checked || $("#HardExceedGuideline")[0].checked ? "YES" : "NO",
            MeritOverrideSoftStop: $('#SoftStopNoMandatory')[0].checked || $('#SoftStopMandatory')[0].checked ? "YES" : "NO",
            MeritOverrideNoJustification: $('#MeritOverrideNoJustifiction')[0].checked ? "YES" : "NO",
            MeritIncreaseWithinGuideline: $('#HardNoExceedGuideline')[0].checked ? "YES" : $('#HardExceedGuideline')[0].checked ? "NO" : "Unchecked",
            MandatoryJustification: $('#SoftStopMandatory')[0].checked || $('#SoftStopMandatory')[0].checked ? "YES" : $('#SoftStopNoMandatory')[0].checked ? "NO" : "Unchecked",
            RoundingMeritPct: $('#ddlRoundMeritPerc')[0].value != "" ? $('#ddlRoundMeritPerc')[0].value : 0,
            RoundingMeritHourly: $('#ddlRoundMeritHour')[0].value != "" ? $('#ddlRoundMeritHour')[0].value : 0,
            RoundingMeritAnnual: $('#ddlRoundMeritAnnual')[0].value != "" ? $('#ddlRoundMeritAnnual')[0].value : 0,
            DecimalMeritPct: $('#ddlDecimalMeritPerc')[0].value != "" ? $('#ddlDecimalMeritPerc')[0].value : 0,
            DecimalMeritHourly: $('#ddlDecimalMeritHour')[0].value != "" ? $('#ddlDecimalMeritHour')[0].value : 0,
            DecimalMeritAnnual: $('#ddlDecimalMeritAnnual')[0].value != "" ? $('#ddlDecimalMeritAnnual')[0].value : 0,
            RoundingLumpSumPct: $('#ddlRoundLumpSumPerc')[0].value != "" ? $('#ddlRoundLumpSumPerc')[0].value : 0,
            RoundingLumpSumHourly: $('#ddlRoundLumpSumHour')[0].value != "" ? $('#ddlRoundLumpSumHour')[0].value : 0,
            RoundingLumpSumAnnual: $('#ddlRoundLumpSumAnnual')[0].value != "" ? $('#ddlRoundLumpSumAnnual')[0].value : 0,
            DecimalLumpSumPct: $('#ddlDecimalLumpSumPerc')[0].value != "" ? $('#ddlDecimalLumpSumPerc')[0].value : 0,
            DecimalLumpSumHourly: $('#ddlDecimalLumpSumHour')[0].value != "" ? $('#ddlDecimalLumpSumHour')[0].value : 0,
            DecimalLumpSumAnnual: $('#ddlDecimalLumpSumAnnual')[0].value != "" ? $('#ddlDecimalLumpSumAnnual')[0].value : 0,
            RoundingAdjustmentPct: $('#ddlRoundAdjustmentPerc')[0].value != "" ? $('#ddlRoundAdjustmentPerc')[0].value : 0,
            RoundingAdjustmentHourly: $('#ddlRoundAdjustmentHour')[0].value != "" ? $('#ddlRoundAdjustmentHour')[0].value : 0,
            RoundingAdjustmentAnnual: $('#ddlRoundAdjustmentAnnual')[0].value != "" ? $('#ddlRoundAdjustmentAnnual')[0].value : 0,
            DecimalAdjustmentPct: $('#ddlDecimalAdjustmentPerc')[0].value != "" ? $('#ddlDecimalAdjustmentPerc')[0].value : 0,
            DecimalAdjustmentHourly: $('#ddlDecimalAdjustmentHour')[0].value != "" ? $('#ddlDecimalAdjustmentHour')[0].value : 0,
            DecimalAdjustmentAnnual: $('#ddlDecimalAdjustmentAnnual')[0].value != "" ? $('#ddlDecimalAdjustmentAnnual')[0].value : 0,
            RoundingCompaRatioPct: $('#ddlRoundCompoRatioPerc')[0].value != "" ? $('#ddlRoundCompoRatioPerc')[0].value : 0,
            RoundingCompaRatioHourly: $('#ddlRoundCompoRatioHour')[0].value != "" ? $('#ddlRoundCompoRatioHour')[0].value : 0,
            RoundingCompaRatioAnnual: $('#ddlRoundCompoRatioAnnual')[0].value != "" ? $('#ddlRoundCompoRatioAnnual')[0].value : 0,
            DecimalCompaRatioPct: $('#ddlDecimalCompoRatioPerc')[0].value != "" ? $('#ddlDecimalCompoRatioPerc')[0].value : 0,
            DecimalCompaRatioHourly: $('#ddlDecimalCompoRatioHour')[0].value != "" ? $('#ddlDecimalCompoRatioHour')[0].value : 0,
            DecimalCompaRatioAnnual: $('#ddlDecimalCompoRatioAnnual')[0].value != "" ? $('#ddlDecimalCompoRatioAnnual')[0].value : 0,
            RoundingPromotionPct: $('#ddlRoundpromotionPerc')[0].value != "" ? $('#ddlRoundpromotionPerc')[0].value : 0,
            RoundingPromotionHourly: $('#ddlRoundPromotionHour')[0].value != "" ? $('#ddlRoundPromotionHour')[0].value : 0,
            RoundingPromotionAnnual: $('#ddlRoundPromotionAnnual')[0].value != "" ? $('#ddlRoundPromotionAnnual')[0].value : 0,
            DecimalPromotionPct: $('#ddlDecimalPromotionPerc')[0].value != "" ? $('#ddlDecimalPromotionPerc')[0].value : 0,
            DecimalPromotionHourly: $('#ddlDecimalPromotionHour')[0].value != "" ? $('#ddlDecimalPromotionHour')[0].value : 0,
            DecimalPromotionAnnual: $('#ddlDecimalPromotionAnnual')[0].value != "" ? $('#ddlDecimalPromotionAnnual')[0].value : 0,
            RoundNewSalaryPct: $('#ddlRoundNewSalaryPerc')[0].value != "" ? $('#ddlRoundNewSalaryPerc')[0].value : 0,
            RoundNewSalaryHourly: $('#ddlRoundNewSalaryHour')[0].value != "" ? $('#ddlRoundNewSalaryHour')[0].value : 0,
            RoundNewSalaryAnnual: $('#ddlRoundNewSalaryAnnual')[0].value != "" ? $('#ddlRoundNewSalaryAnnual')[0].value : 0,
            DecimalNewSalaryPct: $('#ddlDecimalNewSalaryPerc')[0].value != "" ? $('#ddlDecimalNewSalaryPerc')[0].value : 0,
            DecimalNewSalaryHourly: $('#ddlDecimalNewSalaryHour')[0].value != "" ? $('#ddlDecimalNewSalaryHour')[0].value : 0,
            DecimalNewSalaryAnnual: $('#ddlDecimalNewSalaryAnnual')[0].value != "" ? $('#ddlDecimalNewSalaryAnnual')[0].value : 0,
            // Current Salary
            RoundCurrentSalaryPct: $('#ddlRoundCurrentSalaryPerc')[0].value != "" ? $('#ddlRoundCurrentSalaryPerc')[0].value : 0,
            RoundCurrentSalaryHourly: $('#ddlRoundCurrentSalaryHour')[0].value != "" ? $('#ddlRoundCurrentSalaryHour')[0].value : 0,
            RoundCurrentSalaryAnnual: $('#ddlRoundCurrentSalaryAnnual')[0].value != "" ? $('#ddlRoundCurrentSalaryAnnual')[0].value : 0,
            DecimalCurrentSalaryPct: $('#ddlDecimalCurrentSalaryPerc')[0].value != "" ? $('#ddlDecimalCurrentSalaryPerc')[0].value : 0,
            DecimalCurrentSalaryHourly: $('#ddlDecimalCurrentSalaryHour')[0].value != "" ? $('#ddlDecimalCurrentSalaryHour')[0].value : 0,
            DecimalCurrentSalaryAnnual: $('#ddlDecimalCurrentSalaryAnnual')[0].value != "" ? $('#ddlDecimalCurrentSalaryAnnual')[0].value : 0,

            /////////////////
            UserNameFormat: $("#ddlUserNameFormatConfiguration").val(),
            EmployeeNameFormat: $("#ddlEmployeeNameFormatConfiguration").val(),
            SortOrderEmployeeNameFormat: $("#ddlSortOrderFormatConfiguration").val(),
            DateFormat: $("#ddlDateFormatConfiguration").val(),
            CurrencyFormat: $('#BaseCurrency')[0].checked ? "UserCurrency" : "BaseCurrency",

            CurrentYear: $('#inpCurrentyear').val(),
            IDPEndPoint: $('#IPE').val(),
            oldYear: $('#inpoldyear').val(),
            PasswordLength:$('#ddlPasswordLength')[0].value != "" ? $('#ddlPasswordLength')[0].value : 0,
            EmailAddress:$("#inpEmailAddress").val(),
            EmailPassword: $("#inpEmailPassword").val(),
            BonusGlobalPortion: $("#inpBonusIndividual").val(),
            BonusIndividualPortion: $("#inpBonusGlobal").val(),

        },
        success: function (message) {
            objChangeFlag = false;
            Successmessage(message);
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: "../ManageRules/PutApplyRules",
                type: "post",
                data:({__RequestVerificationToken:token}),
                success: function (result) {
                    Successmessage((result == "1") ? "Rules applied successfully" : "Rules apply failed");
                    $('#chkBudgetCalc')[0].checked = false;
                    if($("#step3")[0]!=undefined)
                        {
                    $("#step3").trigger("click");}
                }
            });
        }
    });

    return true;
}

$(document).on('change', '#IPE', function (e) {
    var entered_url = $('#IPE').val();
    if (validateURL(entered_url)) {   
        $("#UserStatus").html("");
        allowSave = true;
    } else {
        $('#IPE').focus();
        $("#UserStatus").html("Enter valid URL");
        allowSave = false;
    }
});
$(document).on('change', '#chkSSOYes', function (e) {
    $('#IPE').focus();
    $("#UserStatus").html("Enter the URL to enable SSO login");
    allowSave = false;
});
$(document).on('change', '#chkSSONo', function (e) {
    $('#IPE').val("");
    $("#UserStatus").html("");
    allowSave = true;
});

function validateURL(textval) {
    var urlregex = new RegExp("^(https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$");
    return urlregex.test(textval);
}

function IdentifierTextCopied(element_id) {
    $("#IdentifierURL").focus();
    $("#IdentifierURL").select();
    document.execCommand("copy");
    $("#IPE").focus();
}
function ReplyTextCopied(element_id) {
    $("#ReplyURL").focus();
    $("#ReplyURL").select();
    document.execCommand("copy");
    $("#IPE").focus();
}