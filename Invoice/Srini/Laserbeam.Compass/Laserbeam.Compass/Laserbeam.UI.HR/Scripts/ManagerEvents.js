//----- Variable Declarations-----
var GridDisplay = "Local";
var preferredCulture = 'en-US';
var ddlCurrencyCulture = 'en-US';
var ddlExchangeRate = 1;
var pageChanged = false;
var pageno = 1;
var MeritAmtLocal;
var MeritPCT;
var MeritAmtUSD;
var LumpSumAmtUSD;
var changeType;
var readOnly = false;
var ddlCurrencyCodeNum;
var isSubmitChecked = false;
var isApproveChecked = false;
var inputChanged = false;
var defaultTools = kendo.ui.Editor.defaultTools;
defaultTools["insertLineBreak"].options.shift = false;
delete defaultTools["insertParagraph"].options;
var isMyApproval;
var isApprovalLevel = false;
var isTopLevelManager = false;
var lastEditedCommentNum = 0;
var isRollUpEnabled;
var isDirectEnabled;
var inputChanged;
var totalCurrentSalary;
var totalNewSalary;
var approveVisiblity = false;
var submitVisiblity = true;
var revertRowIndex = null;
var revertMeritPct = null;
var revertMeritlocalAmt = null;
var revertMeritUSDAmt = null;
var revertRowData = null;
var revertRow = null;
var previousUid = null;
var objChangeFlag;
var BudgetPercentage;



//-----End Declarations-------

//----- Document Methods-----
$(document).ready(function () {
    isDirectSelected = true;
    isRollUpEnabled = true;
    //enabledLumpsum = RuleConfiguration.FeatureConfigurationLumpSum;
});
$(document).on("click", "#btnMeritMandateSave", function (e) {
    var readOnly = $("#CommentPopup").data("kendoWindow").options.readOnly;
    if (readOnly)
        return false;
    var index = $("#CommentPopup").closest(".k-window-content").data("kendoWindow").options.Values;
    var commentText = $("#meritcomment").val();
    var comment_MaxLength = 2000;
    if ($("#meritcomment").val().trim() == '')
        return false;
    else if (commentText.length > comment_MaxLength) {
        showAlert("Comment should not exceed the character limit");
        return false;
    }
    index = (index != undefined) ? index : revertRowIndex;
    revertRowData = null;
    revertRow = null;
    revertRowIndex = null;
    revertMeritPct = null;
    revertMeritlocalAmt = null;
    revertMeritUSDAmt = null;
    var rowData = $("#CommentPopup").data("kendoWindow").options.rowData;
    var row = $("#CommentPopup").data("kendoWindow").options.row;
    rowData.TotalCommentsCount = rowData.TotalCommentsCount + 1;
    calculateMeritAmt(rowData, index, row);
    return true;
    var readOnly = $("#CommentPopup").data("kendoWindow").options.readOnly;
    disableControls(readOnly);
});


$(document).on("hide.bs.modal", "#divPromotion", function (e) {

    if (!isPopupEdited) {
        if (!showSaveWarning(e, "inputChanged")) return false;
        ClearCommentChangeFlag();
    }
    isPopupEdited = false;
});

$(document).on("hide.bs.modal", "#divMeritComment", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowData = grdMeritGrid._data[revertRowIndex];
    if (rowData != undefined) {
        rowData.MeritPCT = revertMeritPct;
        rowData.MeritAmtLocal = revertMeritlocalAmt;
        rowData.MeritAmtUSD = revertMeritUSDAmt;
        refreshRow(grdMeritGrid, revertRow);
        revertRowIndex = null;
        revertRow = null;
    }
});

$(document).on("hide.bs.modal", "#divComment", function (e) {
    if (!isPopupEdited) {
        //if (!showSaveWarning(e, "inputChanged")) return false;
        var rowData = $("#CommentPopup").data("kendoWindow").options.rowData;
        var row = $("#CommentPopup").data("kendoWindow").options.row;
        if (CommentPageTypeConstants.PageType == CommentPageTypeConstants.meritMandate) {
            var rowIndex = CommentPageTypeConstants.rowIndex;
            rollbackMeritPct(rowData, rowIndex, row);
        }
        else if (CommentPageTypeConstants.PageType == CommentPageTypeConstants.generalComment) {
            var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
            rowData.TotalUnReadComment = 0;
            refreshRow(grdMeritGrid, row);
        }
        ClearCommentChangeFlag();
    }
    isPopupEdited = false;
});

$(document).on('change', '#selectedRollupChk', function () {
    if ($(this).is(':checked')) {
        $("#hndIsSelectedRollup").val(1);
        BindBudgetData();
        $(this).checked = true;
    }
    else {
        $("#hndIsSelectedRollup").val(0);
        BindBudgetData();
    }
});
$(document).on('click', '#btnDirect', function () {
    $("#btnRollUp").removeClass("bg-yellow-lemon selected");
    $("#btnRollUp").addClass("bg-white");
    $("#btnDirect").removeClass("bg-white");
    $("#btnDirect").addClass("bg-yellow-lemon selected");

    $("#hndIsSelectedRollup").val(0);
    BindBudgetData();

});
//$(document).on('click', '#btnDirectRollUp', function () {
//    var chkDirectRollup = $('#chkDirectRollUp').is(':checked');
//    if (chkDirectRollup == false) {
//        $("#hndIsSelectedRollup").val(1);
//        $("#txtDirectRollup").text("Direct & Indirect Budget");
//        $("#chkDirectRollUp").prop("checked", true);
//    }
//    if (chkDirectRollup == true) {
//        $("#hndIsSelectedRollup").val(0);
//        $("#txtDirectRollup").html("Direct <br/> Budget");
//        $("#chkDirectRollUp").prop("checked", false);
//    }
//    BindBudgetData();

//});

$(document).on('click', '#DirectBudget', function () {
    $("#hndIsSelectedRollup").val(0);
    BindBudgetData();

});
$(document).on('click', '#DirectIndirect', function () {
    $("#hndIsSelectedRollup").val(1);
    BindBudgetData();

});

$(document).on('click', '#btnRollUp', function () {

    $("#btnDirect").removeClass("bg-yellow-lemon selected");
    $("#btnDirect").addClass("bg-white");
    $("#btnRollUp").removeClass("bg-white");
    $("#btnRollUp").addClass("bg-yellow-lemon selected");
    $("#hndIsSelectedRollup").val(1);
    BindBudgetData();

});

$(document).on("click", "[id$=EmployeeName]", function () {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CallEmployeeNameClick(rowData.EmployeeNum, rowData.EmployeeName);
});



$(document).on("change", "[id*=ddlMeritPerfRating]", function (e) {

    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    var selectedData = $("#ddlMeritPerfRating_" + rowData.Empjobnum).data("kendoDropDownList").dataItem($("#ddlMeritPerfRating_" + rowData.Empjobnum).data("kendoDropDownList").select());
    var RatingRangeText;
    for (var i = 0; i < RatingRange.length; i++) {
        if (RatingRange[i].Value == selectedData.Value) {
            RatingRangeText = RatingRange[i].Text;
        }
    }
    ChangeMeritPerfRating(rowData, row, rowIndex, RatingRangeText);
    checkMeritGuideLine(rowData, rowIndex)
    refreshRow(grdMeritGrid, row);

    rollUpDirectDisabled();
});

$(document).on("click", "[id$=lnkNewTitle]", function () {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CallGradeClick(rowData, row, rowIndex);
});

$(document).on("click", "[id$=lnkComment]", function () {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    $("#CommentPopup").closest(".k-window-content").data("kendoWindow").options.Values = rowIndex;
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CallCommentClick(rowData, row);
});


$(document).on("keydown", "#txtSearch", function (e) {
    var selectedData = $("#ddlColumns").data("kendoDropDownList").dataItem($("#ddlColumns").data("kendoDropDownList").select());
    var selectedValue = selectedData.Value;
    if (selectedValue == "BaseSalaryLocal" || selectedValue == "BaseSalaryUSD" || selectedValue == "CurrentAnnualizedSalaryLocal" || selectedValue == "CurrentAnnualizedSalaryUSD" || selectedValue == "HrlySalaryLocal" ||
    selectedValue == "HrlySalaryUSD" || selectedValue == "ProRation" || selectedValue == "ProRatedPCT" || selectedValue == "ProRatedAmtLocal" || selectedValue == "ProRatedAmtUSD" || selectedValue == "HrlyProRatedAmtLocal" ||
    selectedValue == "HrlyProRatedAmtUSD" || selectedValue == "CountryBudgetLocal" || selectedValue == "CountryBudgetUSD" || selectedValue == "BudgetLocal" || selectedValue == "BudgetUSD" || selectedValue == "BudgetPCT" ||
    selectedValue == "MeritPCT" || selectedValue == "MeritAmtLocal" || selectedValue == "MeritAmtUSD" || selectedValue == "HrlyMeritAmtLocal" || selectedValue == "HrlyMeritAmtUSD" ||
            selectedValue == "PromotionPct" || selectedValue == "PromotionAmtLocal" || selectedValue == "PromotionAmtUSD" ||
            selectedValue == "AdjustmentPct" || selectedValue == "AdjustmentAmtLocal" || selectedValue == "AdjustmentAmtUSD" ||
            selectedValue == "NewSalaryLocal" || selectedValue == "NewSalaryUSD" || selectedValue == "HrlyNewSalaryLocal" ||
            selectedValue == "HrlyNewSalaryUSD" || selectedValue == "NewCompaRatio" || selectedValue == "BonusTargetLocal" ||
            selectedValue == "BonusTargetUSD" || selectedValue == "BonusPCT" || selectedValue == "BonusAMTLocal" || selectedValue == "BonusAMTUSD" ||
            selectedValue == "TCCLocal" || selectedValue == "TCCUSD" || selectedValue == "LumpSumPct" ||
            selectedValue == "LumpSumAmtLocal" || selectedValue == "LumpSumAmtUSD" || selectedValue == "HrlyPromotionAmtLocal" ||
            selectedValue == "HrlyPromotionAmtUSD" || selectedValue == "HrlyAdjustmentAmtLocal" ||
            selectedValue == "HrlyAdjustmentAmtUSD" ||
            selectedValue == "HrlyLumpSumAmtLocal" ||
            selectedValue == "HrlyLumpSumAmtUSD" ||
            selectedValue == "MktMinLocal" ||
            selectedValue == "MktMinUSD" ||
            selectedValue == "MktMidLocal" ||
            selectedValue == "MktMidUSD" ||
            selectedValue == "MktMaxLocal" ||
            selectedValue == "MktMaxUSD" ||
            selectedValue == "NewMktMinLocal" ||
            selectedValue == "NewMktMinUSD" ||
            selectedValue == "NewMktMidLocal" ||
            selectedValue == "NewMktMidUSD" ||
            selectedValue == "NewMktMaxLocal" ||
            selectedValue == "NewMktMaxUSD" ||
            selectedValue == "HrlyNewMktMinLocal" ||
            selectedValue == "HrlyNewMktMinUSD" ||
            selectedValue == "HrlyNewMktMidLocal" ||
            selectedValue == "HrlyNewMktMidUSD" ||
            selectedValue == "HrlyNewMktMaxLocal" ||
            selectedValue == "HrlyNewMktMaxUSD" ||
            selectedValue == "HrlyMktMinLocal" ||
            selectedValue == "HrlyMktMinUSD" ||
            selectedValue == "HrlyMktMidLocal" ||
            selectedValue == "HrlyMktMidUSD" ||
            selectedValue == "HrlyMktMaxLocal" ||
            selectedValue == "HrlyMktMaxUSD") {
        var cultureCode = preferredCulture;
        var employeeCulture = kendo.cultures[cultureCode];
        var decimalSeparator = employeeCulture.numberFormat['.'];
        allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    }
});

$(document).on("keydown", "[id$=txtMeritPCT]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var employeeCulture = kendo.cultures[preferredCulture];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtMeritPCT]", function (e) {
    if (triggerEnterEvent == true) {
        var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
        var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
        var rowIndex = $(this).closest("tr").index();
        var row = $(this).closest("tr");
        var rowData = grdMeritGrid.dataItem(row);
        MeritPCT = rowData.MeritPCT;
        MeritAmtLocal = rowData.MeritAmtLocal;
        MeritAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule((MeritAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "annual") : roundingRule((MeritAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "hourly");
        revertRowData = rowData;
        revertRow = row;
        revertRowIndex = rowIndex;
        revertMeritPct = MeritPCT;
        revertMeritlocalAmt = MeritAmtLocal;
        revertMeritUSDAmt = MeritAmtUSD;
        if (!calculateForMeritPct(rowData, this.value, rowIndex, row)) {
            rowData.MeritPCT = MeritPCT;
            rowData.MeritAmtLocal = MeritAmtLocal;
            rowData.MeritAmtUSD = MeritAmtUSD;
            refreshRow(grdMeritGrid, row);
        }
        if (e.isTrigger != undefined) {
            triggerEnterEvent = false;
        }
    }
    rollUpDirectDisabled();
    rebindOfTotalSpentSummary();
});



$(document).on("contextmenu", "[id$=txtMeritPCT],[id$=txtMeritAmtLocal],[id$=txtMeritAmtUSD],[id$=txtPromotionPctLocal],[id$=txtPromotionAmtLocal],[id$=txtPromotionPctUSD],[id$=txtPromotionAmtUSD],[id$=txtAdjustmentPct],[id$=txtAdjustmentAmtLocal],[id$=txtAdjustmentAmtUSD],[id$=txtBonusPCT],[id$=txtLumpSumPct],[id$=txtLumpSumAmtLocal],[id$=txtLumpSumAmtUSD],[id$=txtBonusPCT]", function (e) {
    return false;
});

$(document).on("hover", ".proration", function () {
    {
        var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var row = $(this).closest("tr");
        var rowData = grdMeritGrid.dataItem(row);
        $(this).kendoTooltip({
            autoHide: true,
            content: 'Increase amount is prorated and rounded to the ' + RuleConfiguration.GeneralConfigurationRoundingMeritAnnual + ' ' + RuleConfiguration.GeneralConfigurationDecimalMeritAnnual
        });
    }
});

$(document).on("hover", ".mktMinLocal", function () {
    {
        var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var row = $(this).closest("tr");
        var rowData = grdMeritGrid.dataItem(row);
        $(this).kendoTooltip({
            autoHide: true,
            content: 'Market Min will be used for calculating the Merit Increase'
        });
    }
});

$(document).on("hover", ".mktMinUSD", function () {
    {
        var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var row = $(this).closest("tr");
        var rowData = grdMeritGrid.dataItem(row);
        $(this).kendoTooltip({
            autoHide: true,
            content: 'Market Min will be used for calculating the Merit Increase'
        });
    }
});

$(document).on("hover", "#ProRationId", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    var prorationCheck = rowData.ProRation;
    var Prorationvalue = prorationCheck == null ? "Proration value 0.00000" : "Proration value" + " " + prorationCheck.toFixed(5)
    $(this).kendoTooltip({
        autoHide: true,
        content: Prorationvalue
    });

});

$(document).on("hover", "[id$=txtNewSalaryLocal]", function () {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    if (rowData.NewSalaryLocal != null) {
        $(this).kendoTooltip({
            autoHide: true, content: 'New Salary is rounded to the ' + RuleConfiguration.GeneralConfigurationRoundingNewSalaryAnnual + ' ' + RuleConfiguration.GeneralConfigurationDecimalNewSalaryAnnual
        });
    }
});

$(document).on("hover", "[id$=txtNewSalaryUSD]", function () {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    if (rowData.NewSalaryUSD != null) {
        $(this).kendoTooltip({
            autoHide: true, content: 'New Salary is rounded to the Absolute 1'
        });
    }
});

$(document).on("keydown", "[id$=txtMeritAmtLocal]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtMeritAmtLocal]", function (e) {

    if (triggerEnterEvent == true) {
        var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var row = $(this).closest("tr");
        var rowData = grdMeritGrid.dataItem(row);
        MeritPCT = rowData.MeritPCT;
        MeritAmtLocal = rowData.MeritAmtLocal;
        MeritAmtUSD = rowData.MeritAmtUSD;
        revertRowData = rowData;
        revertRow = row;
        revertRowIndex = rowIndex;
        revertMeritPct = MeritPCT;
        revertMeritlocalAmt = MeritAmtLocal;
        revertMeritUSDAmt = MeritAmtUSD;
        if (!CalculateMeritAmtLocal(rowData, this.value, rowIndex, row)) {
            rowData.MeritPCT = MeritPCT;
            rowData.MeritAmtLocal = MeritAmtLocal;
            rowData.MeritAmtUSD = MeritAmtUSD;
            refreshRow(grdMeritGrid, row);
        }
        if (e.isTrigger != undefined) {
            triggerEnterEvent = false;
        }
    }
    //  rebindOfTotalSpentSummary();
});

$(document).on("keydown", "[id$=txtMeritAmtUSD]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtMeritAmtUSD]", function (e) {
    if (triggerEnterEvent == true) {
        var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var row = $(this).closest("tr");
        var rowData = grdMeritGrid.dataItem(row);
        MeritPCT = rowData.MeritPCT;
        MeritAmtLocal = rowData.MeritAmtLocal;
        MeritAmtUSD = rowData.MeritAmtUSD;
        revertRowData = rowData;
        revertRow = row;
        revertRowIndex = rowIndex;
        revertMeritPct = MeritPCT;
        revertMeritlocalAmt = MeritAmtLocal;
        revertMeritUSDAmt = MeritAmtUSD;
        if (!CalculateMeritAmtUSD(rowData, this.value, rowIndex, row)) {
            rowData.MeritPCT = MeritPCT;
            rowData.MeritAmtLocal = MeritAmtLocal;
            rowData.MeritAmtUSD = MeritAmtUSD;
            refreshRow(grdMeritGrid, row);
        }
        if (e.isTrigger != undefined) {
            triggerEnterEvent = false;
        }
    }
    // rebindOfTotalSpentSummary();
});

$(document).on("keydown", "[id$=txtPromotionPctLocal]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var employeeCulture = kendo.cultures[preferredCulture];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtPromotionPctLocal]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculatePromotionPCT(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});

$(document).on("keydown", "[id$=txtPromotionPctUSD]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var employeeCulture = kendo.cultures[preferredCulture];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtPromotionPctUSD]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculatePromotionPCT(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);

    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});

$(document).on("keydown", "[id$=txtPromotionAmtLocal]", function (e) {

    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtPromotionAmtLocal]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculatePromotionAmtLocal(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});

$(document).on("keydown", "[id$=txtPromotionAmtUSD]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtPromotionAmtUSD]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculatePromotionAmtUSD(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});

$(document).on("keydown", "[id$=txtAdjustmentPct]", function (e) {

    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var employeeCulture = kendo.cultures[preferredCulture];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});
$(document).on("keydown", "[id$=txtBonusPct]", function (e) {

    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var employeeCulture = kendo.cultures[preferredCulture];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtAdjustmentPct]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateAdjustmentPCT(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});

$(document).on("change", "[id$=txtBonusPct]", function (e) {

    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateBonusPCT(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});

$(document).on("keydown", "[id$=txtAdjustmentAmtLocal]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});
$(document).on("keydown", "[id$=txtBonusAmt]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtAdjustmentAmtLocal]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateAdjustmentAmtLocal(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});
$(document).on("change", "[id$=txtBonusAmt]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateBonusAmt(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});
$(document).on("keydown", "[id$=txtAdjustmentAmtUSD]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtAdjustmentAmtUSD]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateAdjustmentAmtUSD(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }

});

$(document).on("keydown", "[id$=txtLumpSumPct]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var employeeCulture = kendo.cultures[preferredCulture];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInput(e, decimalSeparator, e.target);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtLumpSumPct]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateLumpSumPCT(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }
});

$(document).on("keydown", "[id$=txtLumpSumAmtLocal]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var cultureCode = (RuleConfiguration.FeatureConfigurationMultiCurrency) ? rowData.CultureCode : preferredCulture;
    var employeeCulture = kendo.cultures[cultureCode];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtLumpSumAmtLocal]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateLumpSumAmtLocal(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);

    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }


});

$(document).on("keydown", "[id$=txtLumpSumAmtUSD]", function (e) {
    triggerEnterEvent = true;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritReportees.dataItem(row);
    var employeeCulture = kendo.cultures[preferredCulture];
    var decimalSeparator = employeeCulture.numberFormat['.'];
    allowDecimalNumberOnlyInputAmt(e, decimalSeparator);
    var code = e.keycode || e.which
    if (code == 13 || code == 9) {
        $(this).trigger("change");
        return false;
    }
});

$(document).on("change", "[id$=txtLumpSumAmtUSD]", function (e) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var rowIndex = $(this).closest("tr").index();
    var row = $(this).closest("tr");
    var rowData = grdMeritGrid.dataItem(row);
    CalculateLumpSumAmtUSD(rowData, this.value, row);
    refreshRow(grdMeritGrid, row);
    if (e.isTrigger != undefined) {
        triggerEnterEvent = false;
    }


});



$(document).on("click", "#btnCompensationSave", function (event) {
    triggerSaveEvent();
    rollUpDirectEnabled();
    Successmessage("Saved Successfully");
    objChangeFlag = false;
});



//----- End Document Methods-----

//----- Functions-----


function CalculatePromotionPCT(rowData, currentValue, row) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var oldPromotionAmount = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule((rowData.PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "annual") : roundingRule((rowData.PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "hourly");
    var PromotionAmtLocal = rowData.PromotionAmtLocal;
    var value = getNumberValue(currentValue, preferredCulture);
    if (value !== null) {
        value = (currentValue.indexOf('%') > -1) ? value * 100 : value;
        value = roundingRule(value, "promotion", "percentage");
        var oldPromotionpct = rowData.PromotionPct;
        if (isNaN(value) || value > 999.00) {
            showAlert("Given increase exceeds the maximum limit");
            rowData.PromotionPct = oldPromotionpct;
            return false;
        }
        rowData.PromotionPct = value;
        var promotionAmtLocal;
        if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
            if (GridDisplay == "USD") {
                var promotionAmtUSD = ((rowData.CurrentAnnualSalaryUSDForCalc * value) / 100);
                promotionAmtLocal = (promotionAmtUSD / rowData.MeritExchangeRate);
            }
            else {
                promotionAmtLocal = ((rowData.CurrentAnnualSalaryLocalForCalc * value) / 100);
            }
        }
        else {
            if (GridDisplay == "USD") {
                var promotionAmtUSD = ((rowData.CurrentHourlyRateUSDForCalc * value) / 100);
                promotionAmtLocal = (promotionAmtUSD / rowData.MeritExchangeRate);
            }
            else {
                promotionAmtLocal = ((rowData.CurrentHourlyRateLocalForCalc * value) / 100);
            }
        }
        rowData.PromotionAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(promotionAmtLocal, "promotion", "annual") : roundingRule(promotionAmtLocal, "promotion", "hourly");
        rowData.PromotionAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule((rowData.PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "annual") : roundingRule((rowData.PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "hourly");
        var hourlyAmtLocal = calculateHourly(rowData.PromotionAmtLocal);
        var hourlyAmtUSD = calculateHourly(rowData.PromotionAmtUSD);
    }
    else {
        rowData.PromotionPct = null;
        rowData.PromotionAmtLocal = null;
        rowData.PromotionAmtUSD = null;
        rowData.HrlyPromotionAmtLocal = null;
        rowData.HrlyPromotionAmtUSD = null;
    }
    if (rowData.EmployeeStatus.toLowerCase() == "annual")
        fn_BudgetPromotionSpentChanges((PromotionAmtLocal != null) ? (PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0, (rowData.PromotionAmtLocal != null) ? (rowData.PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0);
    else
        fn_BudgetPromotionSpentChanges((PromotionAmtLocal != null) ? (rowData.TotalWorkHrs != null ? ((PromotionAmtLocal * rowData.TotalWorkHrs) / rowData.MeritExchangeRate) * selectedData.ExchangeRate : ((PromotionAmtLocal * 2080) / rowData.MeritExchangeRate) * selectedData.ExchangeRate) : 0, (rowData.PromotionAmtLocal != null) ? (rowData.TotalWorkHrs != null ? ((rowData.PromotionAmtLocal * rowData.TotalWorkHrs) / rowData.MeritExchangeRate) * selectedData.ExchangeRate : ((rowData.PromotionAmtLocal * 2080) / rowData.MeritExchangeRate) * selectedData.ExchangeRate) : 0);


    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    var index = row.index();
    checkPromotionGuideLine(rowData, index);
    rowData.IsPromotionEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;
}

function fn_BudgetPromotionSpentChanges(oldValue, newValue) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var cultureCode = (selectedData != undefined) ? selectedData.CultureCode : preferredCulture;
    var newIncreaseValue = newValue - oldValue;
    var hdnPromotionSpent = document.getElementById("hdnPromotionSpent");
    var hdnMeritSpent = document.getElementById("hdnMeritSpent").value;
    var hdnLumpSumSpent = document.getElementById("hdnLumpSumSpent").value;
    var hdnMeritBudget = document.getElementById("hdnMeritBudget").value;
    var hdnSpentValue = hdnPromotionSpent.value;
    var lblSpent = document.getElementById("promotionSpentAmt");
    var lblTotalSpent = document.getElementById("totalSpentAmt");
    var lblBalance = document.getElementById("meritBalance");
    var newSpentAmt = (eval(hdnSpentValue) + eval(newIncreaseValue));
    hdnPromotionSpent.value = newSpentAmt;
    newSpentAmt1 = (eval(newSpentAmt) != 0) ? (roundBudget(eval(newSpentAmt))) : 0;
    newSpentAmt = (newSpentAmt1 != NaN) ? newSpentAmt : 0;
    hdnMeritSpent1 = (eval(hdnMeritSpent) != 0) ? (roundBudget(eval(hdnMeritSpent))) : 0;
    hdnMeritSpent = (hdnMeritSpent1 != NaN) ? hdnMeritSpent : 0;
    hdnLumpSumSpent1 = (eval(hdnLumpSumSpent) != 0) ? (roundBudget(eval(hdnLumpSumSpent))) : 0;
    hdnLumpSumSpent = (hdnLumpSumSpent1 != NaN) ? hdnLumpSumSpent : 0;
    hdnMeritBudget = (eval(hdnMeritBudget) != 0) ? (roundBudget(eval(hdnMeritBudget))) : 0;
    hdnLumpSumSpent = (RuleConfiguration.MeritValuesReCalculate == false && RuleConfiguration.LumpSumRuleLumpSumType == "AutoCalc") ? 0 : hdnLumpSumSpent;
    var totalSpent = (eval(newSpentAmt) + eval(hdnMeritSpent) + eval(hdnLumpSumSpent));
    var newBalance = (eval(hdnMeritBudget) - eval(totalSpent));
    lblBalance.innerHTML = formatBudgetCurrency(eval(newBalance), 'c0', cultureCode);
    var balancepct = "(";
    newBalance = Math.round(newBalance);
    if (newBalance < 0) {
        $("#meritBalance").removeClass("counter2");
        $("#meritBalance").addClass("counter3");
        $("#balancePct").removeClass("counter2");
        $("#balancePct").addClass("counter3");
    }
    else {
        $("#meritBalance").removeClass("counter3");
        $("#meritBalance").addClass("counter2");
        $("#balancePct").addClass("counter2");
        $("#balancePct").removeClass("counter3");
    }
    lblSpent.innerHTML = formatBudgetCurrency(eval(newSpentAmt), 'c0', cultureCode);
    totalSpent = roundToWholeNumber(eval(newSpentAmt)) + roundToWholeNumber(eval(hdnMeritSpent)) + roundToWholeNumber(eval(hdnLumpSumSpent));
    var oldBalanceClass = document.getElementById("balanceDonughtClass").className;
    var a = (totalCurrentSalary != 0) ? ((eval(Math.abs(newBalance)) / eval(totalCurrentSalary)) * 100).toFixed(0) : 0;
    var balancepct = (totalCurrentSalary != 0) ? ((eval(Math.abs(newBalance)) / eval(totalCurrentSalary)) * 100).toFixed(0) : 0;
    var balanceChartPct = Math.round((BudgetPercentage != 0) ? (balancepct / BudgetPercentage) * 100 : 0);
    (newBalance > 0) ? $("#balancePct").text(a.toString() + "%") : $("#balancePct").text("(" + a + "%)");
    if (BudgetPercentage == balancepct) {
        var newBalanceClass = "c100 small p" + 100;
    }
    else {
        //var newBalanceClass = "c100 small p" + balanceChartPct;
        var PromotionColor = newBalance > 0 ? 1 : newBalance == 0 ? 0 : -1;
        if (PromotionColor == 1) {
            var newBalanceClass = "c100 small p" + balanceChartPct;
        }
        else if (PromotionColor == 0) {
            var newBalanceClass = "c100 small p100";
        }
        else if (PromotionColor == -1) {
            var newBalanceClass = "c100 small x100 p100";
        }
    }
    $("#balanceDonughtClass").removeClass(oldBalanceClass);
    $("#balanceDonughtClass").addClass(newBalanceClass);
    //(newBalance < 0) ? $("#balancePct").text(balancepct.concat(a + "%)")) : $("#balancePct").text(a.toString() + "%");
   
    
}

function CalculatePromotionAmtLocal(rowData, currentValue, row) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var oldPromotionAmtUSD = rowData.PromotionAmtUSD;
    var PromotionAmtLocal = rowData.PromotionAmtLocal;
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        value = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(value, "promotion", "annual") : roundingRule(value, "promotion", "hourly");
        if (!isNaN(value)) {
            if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
                var promotionPct = ((value / rowData.CurrentAnnualSalaryLocalForCalc) * 100 == Infinity) ? 0 : (value / rowData.CurrentAnnualSalaryLocalForCalc) * 100;
            }
            else {
                var promotionPct = ((value / rowData.CurrentHourlyRateLocalForCalc) * 100 == Infinity) ? 0 : (value / rowData.CurrentHourlyRateLocalForCalc) * 100;
            }
            value = (promotionPct == 0 ? 0 : value)
            if (promotionPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                rowData.PromotionAmtLocal = oldPromotionAmtLocal;
                return false;
            }
            rowData.PromotionPct = roundingRule(promotionPct, "promotion", "percentage");
            rowData.PromotionAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(value, "promotion", "annual") : roundingRule(value, "promotion", "hourly");
            rowData.PromotionAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule((rowData.PromotionAmtLocal * rowData.MeritExchangeRate), "general", "annual") : roundingRule((rowData.PromotionAmtLocal * rowData.MeritExchangeRate), "general", "hourly");
            var hourlyAmtLocal = calculateHourly(rowData.PromotionAmtLocal);
            var hourlyAmtUSD = calculateHourly(rowData.PromotionAmtUSD);

        }
        else {
            showAlert("Please enter the data valid format");
            rowData.PromotionAmtLocal = oldPromotionAmtLocal;
            return false;
        }
    }
    else {
        rowData.PromotionPct = null;
        rowData.PromotionAmtLocal = null;
        rowData.PromotionAmtUSD = null;
        rowData.HrlyPromotionAmtLocal = null;
        rowData.HrlyPromotionAmtUSD = null;
    }
    if (rowData.EmployeeStatus.toLowerCase() == "annual")
        fn_BudgetPromotionSpentChanges((PromotionAmtLocal != null) ? (PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0, (rowData.PromotionAmtLocal != null) ? (rowData.PromotionAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0);
    else
        fn_BudgetPromotionSpentChanges((PromotionAmtLocal != null) ? (rowData.TotalWorkHrs != null ? ((PromotionAmtLocal * rowData.TotalWorkHrs) / rowData.MeritExchangeRate) * selectedData.ExchangeRate : ((PromotionAmtLocal * 2080) / rowData.MeritExchangeRate) * selectedData.ExchangeRate) : 0, (rowData.PromotionAmtLocal != null) ? (rowData.TotalWorkHrs != null ? ((rowData.PromotionAmtLocal * rowData.TotalWorkHrs) / rowData.MeritExchangeRate) * selectedData.ExchangeRate : ((rowData.PromotionAmtLocal * 2080) / rowData.MeritExchangeRate) * selectedData.ExchangeRate) : 0);
    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    var index = row.index();
    checkPromotionGuideLine(rowData, index);
    rowData.IsPromotionEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;

}

function CalculatePromotionAmtUSD(rowData, currentValue, row) {
    var oldPromotionAmtUSD = rowData.PromotionAmtUSD;
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        value = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(value, "general", "annual") : roundingRule(value, "general", "hourly");
        if (!isNaN(value)) {
            if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
                var promotionPct = ((value / rowData.CurrentAnnualSalaryLocalForCalc) * 100 == Infinity) ? 0.00 : (value / rowData.CurrentAnnualSalaryLocalForCalc) * 100;
            }
            else {
                var promotionPct = ((value / rowData.CurrentHourlyRateLocalForCalc) * 100 == Infinity) ? 0.00 : (value / rowData.CurrentHourlyRateLocalForCalc) * 100;
            }
            value = (promotionPct == 0 ? 0 : value);
            if (promotionPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                rowData.PromotionAmtUSD = oldPromotionAmtUSD;
                return false;
            }
            var promotionAmtLocal = value / rowData.MeritExchangeRate;
            rowData.PromotionPct = roundingRule(promotionPct, "promotion", "percentage");
            rowData.PromotionAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(promotionAmtLocal, "promotion", "annual") : roundingRule(promotionAmtLocal, "promotion", "hourly");
            rowData.PromotionAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.PromotionAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.PromotionAmtLocal * rowData.MeritExchangeRate, "general", "hourly");
            var hourlyAmtLocal = calculateHourly(rowData.PromotionAmtLocal);
            var hourlyAmtUSD = calculateHourly(rowData.PromotionAmtUSD);

        }
        else {
            showAlert("Please enter the data valid format");
            rowData.PromotionAmtUSD = oldPromotionAmtUSD;
            return false;
        }
    }
    else {
        rowData.PromotionPct = null;
        rowData.PromotionAmtLocal = null;
        rowData.PromotionAmtUSD = null;
        rowData.HrlyPromotionAmtLocal = null;
        rowData.HrlyPromotionAmtUSD = null;
    }
    if (rowData.EmployeeStatus.toLowerCase() == "annual")
        fn_BudgetPromotionSpentChanges((oldPromotionAmtUSD != null) ? oldPromotionAmtUSD : 0, (rowData.PromotionAmtUSD != null) ? rowData.PromotionAmtUSD : 0);
    else
        fn_BudgetPromotionSpentChanges((oldPromotionAmtUSD != null) ? (rowData.TotalWorkHrs != null ? oldPromotionAmtUSD * rowData.TotalWorkHrs : oldPromotionAmtUSD * 2080) : 0, (rowData.PromotionAmtUSD != null) ? (rowData.TotalWorkHrs != null ? rowData.PromotionAmtUSD * rowData.TotalWorkHrs : rowData.PromotionAmtUSD * 2080) : 0);
    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    var index = row.index();
    checkPromotionGuideLine(rowData, index);
    rowData.IsPromotionEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;

}

function CalculateLumpSumPCT(rowData, currentValue, row) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var oldLumpSumAmount = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule((rowData.LumpSumAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "annual") : roundingRule((rowData.LumpSumAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate, "general", "hourly");
    var LumpSumAmtLocal = rowData.LumpSumAmtLocal;
    var value = getNumberValue(currentValue, preferredCulture);
    if (value != null) {
        value = (currentValue.indexOf('%') > -1) ? value * 100 : value;
        value = roundingRule(value, "lumpsum", "percentage");
        var oldLumpSumPct = rowData.LumpSumPct;
        if (isNaN(value) || value > 999.00) {
            showAlert("Given increase exceeds the maximum limit");
            rowData.LumpSumPct = oldLumpSumPct;
            return false;
        }
        rowData.LumpSumPct = value;
        var lumpSumAmountUSD;
        var lumpSumAmountLocal;
        if (GridDisplay == "USD") {
            lumpSumAmountUSD = ((rowData.CurrentAnnualSalaryUSDForCalc * value) / 100);
            lumpSumAmountLocal = (lumpSumAmountUSD / rowData.MeritExchangeRate);
        }
        else {
            lumpSumAmountLocal = ((rowData.CurrentAnnualSalaryLocalForLumpSumCalc * value) / 100);
        }
        rowData.LumpSumAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(lumpSumAmountLocal, "lumpsum", "annual") : roundingRule(lumpSumAmountLocal, "lumpsum", "hourly");
        lumpSumAmountUSD = (rowData.LumpSumAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate;
        rowData.LumpSumAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(lumpSumAmountUSD, "general", "annual") : roundingRule(lumpSumAmountUSD, "general", "hourly");

    }
    else {
        rowData.LumpSumPct = null;
        rowData.LumpSumAmtLocal = null;
        rowData.LumpSumAmtUSD = null;
        rowData.HrlyLumpSumAmtLocal = null;
        rowData.HrlyLumpSumAmtUSD = null;
        HideSubmitApproveCheckBoxes(rowData);
    }

    fn_BudgetLumpSumSpentChanges((LumpSumAmtLocal != null) ? (LumpSumAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0, (rowData.LumpSumAmtLocal != null) ? (rowData.LumpSumAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0);

    CalculateTCC(rowData);
    var index = row.index();
    checkLumpSumGuideLine(rowData, index);
    rowData.IsMeritEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;
}

function fn_BudgetLumpSumSpentChanges(oldValue, newValue) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var cultureCode = (selectedData != undefined) ? selectedData.CultureCode : preferredCulture;
    var newIncreaseValue = newValue - oldValue;
    var hdnPromotionSpent = document.getElementById("hdnPromotionSpent").value;
    var hdnMeritSpent = document.getElementById("hdnMeritSpent").value;
    var hdnLumpSumSpent = document.getElementById("hdnLumpSumSpent")
    var hdnAdjustmentSpent = document.getElementById("hdnAdjustmentSpent")
    var hdnMeritBudget = document.getElementById("hdnMeritBudget").value;
    var hdnSpentValue = hdnLumpSumSpent.value;
    var lblSpent = document.getElementById("lumpsumSpentAmt");
    var lblTotalSpent = document.getElementById("totalSpentAmt");
    var lblBalance = document.getElementById("meritBalance");
    var newSpentAmt = (eval(hdnSpentValue) + eval(newIncreaseValue));
    hdnLumpSumSpent.value = newSpentAmt;
    newSpentAmt1 = (eval(newSpentAmt) != 0) ? (roundBudget(eval(newSpentAmt))) : 0;
    newSpentAmt = (newSpentAmt != NaN) ? newSpentAmt : 0;
    hdnMeritSpent1 = (eval(hdnMeritSpent) != 0) ? (roundBudget(eval(hdnMeritSpent))) : 0;
    hdnMeritSpent = (hdnMeritSpent1 != NaN) ? hdnMeritSpent : 0;
    hdnPromotionSpent1 = (eval(hdnPromotionSpent) != 0) ? (roundBudget(eval(hdnPromotionSpent))) : 0;
    hdnPromotionSpent = (hdnPromotionSpent1 != NaN) ? hdnPromotionSpent : 0;
    hdnMeritBudget = (eval(hdnMeritBudget) != 0) ? (roundBudget(eval(hdnMeritBudget))) : 0;
    var totalSpent = (eval(newSpentAmt) + eval(hdnMeritSpent) + eval(hdnPromotionSpent));
    var newBalance = (eval(hdnMeritBudget) - eval(totalSpent));
    lblBalance.innerHTML = formatBudgetCurrency(eval(newBalance), 'c0', cultureCode);
    var balancepct = "(";
    newBalance = Math.round(newBalance);
    if (newBalance < 0) {
        $("#meritBalance").removeClass("counter2");
        $("#meritBalance").addClass("counter3");
        $("#balancePct").removeClass("counter2");
        $("#balancePct").addClass("counter3");
    }
    else {
        $("#meritBalance").removeClass("counter3");
        $("#meritBalance").addClass("counter2");
        $("#balancePct").addClass("counter2");
        $("#balancePct").removeClass("counter3");
    }
    lblSpent.innerHTML = formatBudgetCurrency(eval(newSpentAmt), 'c0', cultureCode);
    totalSpent = roundToWholeNumber(eval(newSpentAmt)) + roundToWholeNumber(eval(hdnMeritSpent)) + roundToWholeNumber(eval(hdnPromotionSpent));
    var oldBalanceClass = document.getElementById("balanceDonughtClass").className;
    var a = ((eval(Math.abs(newBalance)) / eval(totalCurrentSalary)) * 100).toFixed(0);
    var balancepct = ((eval(Math.abs(newBalance)) / eval(totalCurrentSalary)) * 100).toFixed(0);

    var balanceChartPct = Math.round((balancepct / BudgetPercentage) * 100);
    (newBalance > 0) ? $("#balancePct").text(a.toString() + "%") : $("#balancePct").text("(" + a + "%)");
    if (BudgetPercentage == balancepct) {
        var newBalanceClass = "c100 small p" + 100;
    }
    else {
        // var newBalanceClass = "c100 small p" + balanceChartPct;
        var lumpcolor = newBalance > 0 ? 1 : newBalance == 0 ? 0 : -1;
        if (lumpcolor == 1) {
            var newBalanceClass = "c100 small p" + balanceChartPct;
        }
        else if (lumpcolor == 0) {
            var newBalanceClass = "c100 small p100";
        }
        else if (lumpcolor == -1) {
            var newBalanceClass = "c100 small x100 p100";
        }
    }
    $("#balanceDonughtClass").removeClass(oldBalanceClass);
    $("#balanceDonughtClass").addClass(newBalanceClass);
    //(newBalance < 0) ? $("#balancePct").text(balancepct.concat(a + "%)")) : $("#balancePct").text(a.toString() + "%");
}

function CalculateLumpSumAmtLocal(rowData, currentValue, row) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    var oldLumpSumAmtUSD = rowData.LumpSumAmtUSD;
    var LumpSumAmtLocal = rowData.LumpSumAmtLocal;
    if (value != null) {
        value = roundingRule(value, "lumpsum", "annual");
        if (!isNaN(value)) {
            var lumpSumPct = ((value / rowData.CurrentAnnualSalaryLocalForLumpSumCalc) * 100 == Infinity) ? 0 : (value / rowData.CurrentAnnualSalaryLocalForLumpSumCalc) * 100;
            value = (lumpSumPct == 0 ? 0 : value);
            if (lumpSumPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                rowData.LumpSumAmtLocal = oldLumpSumAmtLocal;
                return false;
            }
            rowData.LumpSumPct = roundingRule(lumpSumPct, "lumpsum", "percentage");
            rowData.LumpSumAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(value, "lumpsum", "annual") : roundingRule(value, "lumpsum", "hourly");
            var hourlyAmtLocal = calculateHourly(rowData.LumpSumAmtLocal);
            var lumpSumAmtUSD = rowData.LumpSumAmtLocal * rowData.MeritExchangeRate;
            rowData.LumpSumAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(lumpSumAmtUSD, "general", "annual") : roundingRule(lumpSumAmtUSD, "general", "hourly");

        }
        else {
            showAlert("Please enter the data valid format");
            rowData.LumpSumAmtLocal = oldLumpSumAmtLocal;
            return false;
        }
    }
    else {
        rowData.LumpSumPct = null;
        rowData.LumpSumAmtLocal = null;
        rowData.LumpSumAmtUSD = null;
        rowData.HrlyLumpSumAmtLocal = null;
        rowData.HrlyLumpSumAmtUSD = null;
        HideSubmitApproveCheckBoxes(rowData);
    }
    fn_BudgetLumpSumSpentChanges((LumpSumAmtLocal != null) ? (LumpSumAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0, (rowData.LumpSumAmtLocal != null) ? (rowData.LumpSumAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0);
    CalculateTCC(rowData);
    var index = row.index();
    checkLumpSumGuideLine(rowData, index);
    rowData.IsMeritEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;

}

function CalculateLumpSumAmtUSD(rowData, currentValue, row) {
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        value = roundingRule(value, "promotion", "annual");
        var oldLumpSumAmtUSD = rowData.LumpSumAmtUSD;
        if (!isNaN(value)) {
            var lumpSumPct = (value / rowData.CurrentAnnualSalaryUSDForCalc) * 100;
            lumpSumPct = roundingRule(lumpSumPct, "promotion", "percentage");
            if (lumpSumPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                rowData.LumpSumAmtUSD = oldLumpSumAmtUSD;
                return false;
            }
            var lumpSumAmtLocal = value / rowData.MeritExchangeRate;
            rowData.LumpSumPct = lumpSumPct;
            rowData.LumpSumAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(lumpSumAmtLocal, "lumpsum", "annual") : roundingRule(lumpSumAmtLocal, "lumpsum", "hourly");
            rowData.LumpSumAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.LumpSumAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.LumpSumAmtLocal * rowData.MeritExchangeRate, "general", "hourly");

        }
        else {
            showAlert("Please enter the data valid format");
            rowData.LumpSumAmtUSD = oldLumpSumAmtUSD;
            return false;
        }
    }
    else {
        rowData.LumpSumPct = null;
        rowData.LumpSumAmtLocal = null;
        rowData.LumpSumAmtUSD = null;
        rowData.HrlyLumpSumAmtLocal = null;
        rowData.HrlyLumpSumAmtUSD = null;
        HideSubmitApproveCheckBoxes(rowData);
    }
    fn_BudgetLumpSumSpentChanges((oldLumpSumAmtUSD != null) ? oldLumpSumAmtUSD : 0, (rowData.LumpSumAmtUSD != null) ? rowData.LumpSumAmtUSD : 0);
    CalculateTCC(rowData);
    var index = row.index();
    checkLumpSumGuideLine(rowData, index);
    rowData.IsMeritEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;
}

function CalculateAdjustmentPCT(rowData, currentValue, row) {
    var oldAdjustmentAmtUSD = rowData.AdjustmentAmtUSD;
    var oldAjustmentpct = rowData.AdjustmentPct;
    var value = getNumberValue(currentValue, preferredCulture);
    if (value != null) {
        value = (currentValue.indexOf('%') > -1) ? value * 100 : value;
        value = roundingRule(value, "adjustment", "percentage");
        if (isNaN(value) || value > 999) {
            showAlert('Adjustment % exceeds the maximum limit');
            rowData.AdjustmentPct = oldAjustmentpct;
            return false;
        }
        rowData.AdjustmentPct = value;
        var adjustmentAmtLocal;
        if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
            if (GridDisplay == "USD") {
                var adjustmentAmtUSD = ((rowData.CurrentAnnualSalaryUSDForCalc * value) / 100);
                adjustmentAmtLocal = (adjustmentAmtUSD / rowData.MeritExchangeRate);
            }
            else {
                adjustmentAmtLocal = ((rowData.CurrentAnnualSalaryLocalForCalc * value) / 100);
            }
        }
        else {
            if (GridDisplay == "USD") {
                var adjustmentAmtUSD = ((rowData.CurrentHourlyRateUSDForCalc * value) / 100);
                adjustmentAmtLocal = (adjustmentAmtUSD / rowData.MeritExchangeRate);
            }
            else {
                adjustmentAmtLocal = ((rowData.CurrentHourlyRateLocalForCalc * value) / 100);
            }
        }
        rowData.AdjustmentAmtLocal = (rowData.EmployeeStatus.toLowerCase()=="annual")? roundingRule(adjustmentAmtLocal, "adjustment", "annual") : roundingRule(adjustmentAmtLocal, "adjustment", "hourly");
        rowData.AdjustmentAmtUSD = (rowData.EmployeeStatus.toLowerCase()== "annual") ? roundingRule(rowData.AdjustmentAmtLocal * rowData.MeritExchangeRate, "general", "annual"): (rowData.AdjustmentAmtLocal * rowData.MeritExchangeRate, "general", "hourly");

    }
    else {
        rowData.AdjustmentPct = null;
        rowData.AdjustmentAmtLocal = null;
        rowData.AdjustmentAmtUSD = null;
        rowData.HrlyAdjustmentAmtLocal = null;
        rowData.HrlyAdjustmentAmtUSD = null;
    }
    // fn_BudgetAdjustmentSpentChanges((oldAdjustmentAmtUSD != null) ? oldAdjustmentAmtUSD : 0, (rowData.AdjustmentAmtUSD != null) ? rowData.AdjustmentAmtUSD : 0);
    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    var index = row.index();
    checkAdjustmentGuideLine(rowData, index);
    rowData.IsAdjustmentEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;
}
function CalculateBonusPCT(rowData, currentValue, row) {

    var oldBonusAmt = rowData.BonusAmt;
    var oldBonusPct = rowData.BonusPct;
    var value = getNumberValue(currentValue, preferredCulture);
    if (value != null) {
        value = (currentValue.indexOf('%') > -1) ? value * 100 : value;
        value = roundingRule(value, "adjustment", "percentage");
        if (isNaN(value) || value > 999) {
            showAlert('Adjustment % exceeds the maximum limit');
            rowData.AdjustmentPct = oldAjustmentpct;
            return false;
        }
        rowData.BonusPct = value;
        var bonusAmt;
        bonusAmt = ((rowData.IndividualPortion * value) / 100);

        rowData.BonusAmt = roundingRule(bonusAmt, "adjustment", "annual");
        rowData.Payout = rowData.BonusAmt + (rowData.GlobalPortion != null ? rowData.GlobalPortion : 0);


    }
    else {
        rowData.BonusAmt = null;
        rowData.BonusPct = null;

    }
    // fn_BudgetAdjustmentSpentChanges((oldAdjustmentAmtUSD != null) ? oldAdjustmentAmtUSD : 0, (rowData.AdjustmentAmtUSD != null) ? rowData.AdjustmentAmtUSD : 0);
    //CalculateNewSalary(rowData);
     CalculateTCC(rowData);
    var index = row.index();
    checkBonusGuideLine(rowData, index);
    rowData.IsBonusEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;
}

function fn_BudgetAdjustmentSpentChanges(oldValue, newValue) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var cultureCode = (selectedData != undefined) ? selectedData.CultureCode : preferredCulture;
    var newIncreaseValue = newValue - oldValue;
    var hdnPromotionSpent = document.getElementById("hdnPromotionSpent").value;
    var hdnMeritSpent = document.getElementById("hdnMeritSpent").value;
    var hdnLumpSumSpent = document.getElementById("hdnLumpSumSpent").value
    var hdnAdjustmentSpent = document.getElementById("hdnAdjustmentSpent");
    var hdnMeritBudget = document.getElementById("hdnMeritBudget").value;
    var hdnSpentValue = hdnMeritSpent.value;
    var lblSpent = document.getElementById("totalAdjustmentSpentAmt");
    var lblTotalSpent = document.getElementById("totalMeritSpentAmt");
    var lblBalance = document.getElementById("meritBalance");
    var newSpentAmt = (eval(hdnSpentValue) + eval(newIncreaseValue));
    hdnAdjustmentSpent.value = newSpentAmt;
    newSpentAmt = (eval(newSpentAmt) != 0) ? (roundBudget(eval(newSpentAmt) / selectedData.ExchangeRate)) : 0;
    hdnMeritSpent = (eval(hdnMeritSpent) != 0) ? (roundBudget(eval(hdnMeritSpent) / selectedData.ExchangeRate)) : 0;
    hdnPromotionSpent = (eval(hdnPromotionSpent) != 0) ? (roundBudget(eval(hdnPromotionSpent) / selectedData.ExchangeRate)) : 0;
    hdnLumpSumSpent = (eval(hdnLumpSumSpent) != 0) ? (roundBudget(eval(hdnLumpSumSpent) / selectedData.ExchangeRate)) : 0;
    hdnMeritBudget = (eval(hdnMeritBudget) != 0) ? (roundBudget(eval(hdnMeritBudget) / selectedData.ExchangeRate)) : 0;
    hdnLumpSumSpent = (RuleConfiguration.MeritValuesReCalculate == false && RuleConfiguration.LumpSumRuleLumpSumType == "AutoCalc") ? 0 : hdnLumpSumSpent;
    var totalSpent = (eval(newSpentAmt) + eval(hdnMeritSpent) + eval(hdnPromotionSpent) + eval(hdnLumpSumSpent));
    var newBalance = (eval(hdnMeritBudget) - eval(totalSpent));
    if (newBalance < 0)
        lblBalance.style.color = "Red";
    else
        lblBalance.style.color = "";

    lblBalance.innerHTML = formatBudgetCurrency(eval(newBalance), 'c0', cultureCode);
    lblSpent.innerHTML = formatBudgetCurrency(eval(newSpentAmt), 'c0', cultureCode);
    totalSpent = roundToWholeNumber(eval(newSpentAmt)) + roundToWholeNumber(eval(hdnMeritSpent)) + roundToWholeNumber(eval(hdnPromotionSpent)) + roundToWholeNumber(eval(hdnLumpSumSpent));
    lblTotalSpent.innerHTML = formatBudgetCurrency(eval(totalSpent), 'c0', cultureCode);

}

function CalculateAdjustmentAmtLocal(rowData, currentValue, row) {
    var oldAdjustmentAmtUSD = rowData.AdjustmentAmtUSD;
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        value = (rowData.EmployeeStatus.toLowerCase() == 'annual') ? roundingRule(value, "adjustment", "annual") : roundingRule(value, "adjustment", "hourly");
        
        var oldAdjustmentAmtLocal = rowData.AdjustmentAmtLocal;
        if (!isNaN(value)) {
            if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
                var adjustmentPct = ((value / rowData.CurrentAnnualSalaryLocalForCalc) * 100 == Infinity) ? 0 : (value / rowData.CurrentAnnualSalaryLocalForCalc) * 100;
            }
            else {
                var adjustmentPct = ((value / rowData.CurrentHourlyRateLocalForCalc) * 100 == Infinity) ? 0 : (value / rowData.CurrentHourlyRateLocalForCalc) * 100;
            }
            value = (adjustmentPct == 0 ? 0 : value);
            if (adjustmentPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                rowData.AdjustmentAmtLocal = oldAdjustmentAmtLocal;
                return false;
            }
            rowData.AdjustmentPct = roundingRule(adjustmentPct, "promotion", "percentage");
            rowData.AdjustmentAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(value, "adjustment", "annual") : roundingRule(value, "adjustment", "hourly");
            rowData.AdjustmentAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.AdjustmentAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.AdjustmentAmtLocal * rowData.MeritExchangeRate, "general", "hourly");


        }
        else {
            showAlert("Please enter the data valid format");
            rowData.AdjustmentAmtLocal = oldAdjustmentAmtLocal;
            return false;
        }
    }
    else {
        rowData.AdjustmentPct = null;
        rowData.AdjustmentAmtLocal = null;
        rowData.AdjustmentAmtUSD = null;
        rowData.HrlyAdjustmentAmtLocal = null;
        rowData.HrlyAdjustmentAmtUSD = null;
    }
    // fn_BudgetAdjustmentSpentChanges((oldAdjustmentAmtUSD != null) ? oldAdjustmentAmtUSD : 0, (rowData.AdjustmentAmtUSD != null) ? rowData.AdjustmentAmtUSD : 0);
    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    var index = row.index();
    checkAdjustmentGuideLine(rowData, index);
    rowData.IsAdjustmentEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;

}
function CalculateBonusAmt(rowData, currentValue, row) {

    var oldBonusAmt = rowData.BonusAmt;
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        value = roundingRule(value, "adjustment", "annual");
        // var oldAdjustmentAmtLocal = rowData.AdjustmentAmtLocal;
        if (!isNaN(value)) {
            var bonusPct = ((value / rowData.IndividualPortion) * 100 == Infinity) ? 0 : (value / rowData.IndividualPortion) * 100;

            value = (bonusPct == 0 ? 0 : value);
            if (bonusPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                rowData.BonusAmt = oldBonusAmt;
                return false;
            }
            rowData.BonusPct = roundingRule(bonusPct, "adjustment", "percentage");
            rowData.BonusAmt = roundingRule(value, "adjustment", "annual")
            rowData.Payout = rowData.BonusAmt + (rowData.GlobalPortion != null ? rowData.GlobalPortion : 0);


        }
        else {
            showAlert("Please enter the data valid format");
            rowData.BonusAmt = oldBonusAmt;
            return false;
        }
    }
    else {
        rowData.BonusAmt = null;
        rowData.BonusPct = null;

    }
    // fn_BudgetAdjustmentSpentChanges((oldAdjustmentAmtUSD != null) ? oldAdjustmentAmtUSD : 0, (rowData.AdjustmentAmtUSD != null) ? rowData.AdjustmentAmtUSD : 0);
    //CalculateNewSalary(rowData);
     CalculateTCC(rowData);
    var index = row.index();
    checkBonusGuideLine(rowData, index);
    rowData.IsBonusEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;

}


function CalculateAdjustmentAmtUSD(rowData, currentValue, row) {
    var oldAdjustmentAmtUSD = rowData.AdjustmentAmtUSD;
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        value = roundingRule(value, "general", "annual");
        if (!isNaN(value)) {
            if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
                var adjustmentPct = ((value / rowData.CurrentAnnualSalaryLocalForCalc) * 100 == Infinity) ? 0 : (value / rowData.CurrentAnnualSalaryLocalForCalc) * 100;
            }
            else {
                var adjustmentPct = ((value / rowData.CurrentHourlyRateLocalForCalc) * 100 == Infinity) ? 0 : (value / rowData.CurrentHourlyRateLocalForCalc) * 100;
            }
            value = (adjustmentPct == 0 ? 0 : value);
            if (adjustmentPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                rowData.AdjustmentAmtUSD = oldAdjustmentAmtUSD;
                return false;
            }
            var adjustmentAmtUSD = value;
            var adjustmentAmtLocal = value / rowData.MeritExchangeRate;
            rowData.AdjustmentPct = roundingRule(adjustmentPct, "promotion", "percentage");
            rowData.AdjustmentAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(adjustmentAmtLocal, "adjustment", "annual") : roundingRule(adjustmentAmtLocal, "adjustment", "hourly");
            rowData.AdjustmentAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.AdjustmentAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.AdjustmentAmtLocal * rowData.MeritExchangeRate, "general", "hourly");


        }
        else {
            showAlert("Please enter the data valid format");
            rowData.AdjustmentAmtUSD = oldAdjustmentAmtUSD;
            return false;
        }
    }
    else {
        rowData.AdjustmentPct = null;
        rowData.AdjustmentAmtLocal = null;
        rowData.AdjustmentAmtUSD = null;
        rowData.HrlyAdjustmentAmtLocal = null;
        rowData.HrlyAdjustmentAmtUSD = null;
    }
    // fn_BudgetAdjustmentSpentChanges((oldAdjustmentAmtUSD != null) ? oldAdjustmentAmtUSD : 0, (rowData.AdjustmentAmtUSD != null) ? rowData.AdjustmentAmtUSD : 0);
    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    var index = row.index();
    checkAdjustmentGuideLine(rowData, index);
    rowData.IsAdjustmentEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;
}

function calculateBudgetIsExceed(rowData) {
    var proRationFactor = RuleConfiguration.ProrationRuleProrate ? rowData.ProRation : 1;
    var oldMeritAmt = MeritAmtUSD != null ? MeritAmtUSD : 0;
    var newMeritAmt = ((rowData.CurrentAnnualSalaryUSDForCalc * rowData.MeritPCT * proRationFactor) / 100);
    var newMeritIncreaseValue = newMeritAmt - oldMeritAmt;
    var hdnPromotionSpent = document.getElementById("hdnPromotionSpent").value;
    var hdnMeritSpentValue = document.getElementById("hdnMeritSpent").value;
    var hdnLumpSumSpent = document.getElementById("hdnLumpSumSpent").value;
    var hdnMeritBudget = document.getElementById("hdnMeritBudget").value;
    var newMeritSpentAmt = (eval(hdnMeritSpentValue) + eval(newMeritIncreaseValue));
    var totalSpent = (eval(newMeritSpentAmt) + eval(hdnPromotionSpent) + eval(hdnLumpSumSpent));
    var newBalance = (eval(hdnMeritBudget) - eval(totalSpent));
    if (newBalance >= 0)
        return true;
    else
        return false;
}

function showMeritAlert(title, message) {
    swal({
        title: title,
        html: message,
        allowOutsideClick: false,
        width: 350,
    });
}

function meritOverrideRule(rowData, rowIndex, row) {

    var isGuideLineExceed = CheckGuideLineExceed(rowData.MeritRange, rowData.MeritPCT);
    if (RuleConfiguration.MeritOverrideHardStop) {
        if (RuleConfiguration.MeritOverrideIncreaseWithinGuideline) {
            if (isGuideLineExceed) {
                //showAlert("Please keep your decision within the recommendation");
                showMeritAlert("Merit Pay outside guideline", "The merit increase should be entered within the specified guideline.");
                return false;
            }
            else {
                calculateMeritAmt(rowData, rowIndex, row);
                return true;
            }
        }
        else {
            if (calculateBudgetIsExceed(rowData) == true) {
                calculateMeritAmt(rowData, rowIndex, row);
                return true;
            }
            else {
                //showAlert("Exceeds the manager budget");
                showMeritAlert("Salary Increase exceeds your budget", "The given increase(s) cannot exceed the allocated budget. Update the increase(s) and save your work.");
                return false;
            }
        }
    }
    else if (RuleConfiguration.MeritOverrideSoftStop) {
        if (RuleConfiguration.MeritOverrideMandatoryJustification && isGuideLineExceed) {
            OpenMeritMandateCommentPopUp(rowData, rowIndex, row);
            return true;
        }
        else {
            calculateMeritAmt(rowData, rowIndex, row);
            return true;
        }
    }
}


function onCollapsableClick(e) {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    if (selectedData != undefined) {
        var hdnPromotionSpent = document.getElementById("hdnPromotionSpent").value;
        var hdnMeritSpent = document.getElementById("hdnMeritSpent").value;
        var hdnLumpSumSpent = document.getElementById("hdnLumpSumSpent").value;
        var hdnMeritBudget = document.getElementById("hdnMeritBudget").value;
        var hdnBonusSpent = document.getElementById("hdnBonusSpent").value;
        var hdnBonusBudget = document.getElementById("hdnBonusBudget").value;
        var meritBudget = (hdnMeritBudget != 0) ? (roundBudget(hdnMeritBudget / selectedData.ExchangeRate)) : 0;
        var bonusBudget = (hdnBonusBudget != 0) ? (roundBudget(hdnBonusBudget / selectedData.ExchangeRate)) : 0;
        var meritSpent = (hdnMeritSpent != 0) ? (roundBudget(hdnMeritSpent / selectedData.ExchangeRate)) : 0;
        var lumpSumSpent = (hdnLumpSumSpent != 0) ? (roundBudget(hdnLumpSumSpent / selectedData.ExchangeRate)) : 0;
        var promotionSpent = (hdnPromotionSpent != 0) ? (roundBudget(hdnPromotionSpent / selectedData.ExchangeRate)) : 0;
        var bonusSpent = (hdnBonusSpent != 0) ? (roundBudget(hdnBonusSpent / selectedData.ExchangeRate)) : 0;
        lumpSumSpent = (RuleConfiguration.MeritValuesReCalculate == false && RuleConfiguration.LumpSumRuleLumpSumType == "AutoCalc") ? 0 : lumpSumSpent;
        var balance = (eval(meritBudget) - (eval(meritSpent) + eval(lumpSumSpent) + eval(promotionSpent)));
        var bonusBalance = (eval(bonusBudget) - eval(bonusSpent));
        createChart(eval(balance), eval(meritSpent), eval(lumpSumSpent), eval(promotionSpent), ddlCurrencyCulture);
    }
}
//

function rebindOfTotalSpentSummary() {
    var hdnPromotionSpent = document.getElementById("hdnPromotionSpent").value;
    var hdnMeritSpent = document.getElementById("hdnMeritSpent").value;
    var hdnLumpSumSpent = document.getElementById("hdnLumpSumSpent").value;
    var hdnAdjustmentSpent = document.getElementById("hdnAdjustmentSpent").value;
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var cultureCode = (selectedData != undefined) ? selectedData.CultureCode : preferredCulture;
    var meritSpent = (hdnMeritSpent != 0) ? (roundBudget(hdnMeritSpent / selectedData.ExchangeRate)) : 0;
    var lumpSumSpent = (hdnLumpSumSpent != 0) ? (roundBudget(hdnLumpSumSpent / selectedData.ExchangeRate)) : 0;
    var promotionSpent = (hdnPromotionSpent != 0) ? (roundBudget(hdnPromotionSpent / selectedData.ExchangeRate)) : 0;
    var bonusSpent = (hdnBonusSpent != 0) ? (roundBudget(hdnBonusSpent / selectedData.ExchangeRate)) : 0;
    var adjustmentSpent = (hdnAdjustmentSpent != 0) ? (roundBudget(hdnAdjustmentSpent / selectedData.ExchangeRate)) : 0;
    var totalSpent = roundToWholeNumber(eval(meritSpent)) + roundToWholeNumber(eval(lumpSumSpent)) + roundToWholeNumber(eval(promotionSpent));
    $("#totalSpentAmt").text(formatBudgetCurrency(eval(totalSpent), 'c0', cultureCode));
    $("#balanceAmt").text($("#meritBalance").text());

}


function onMultiCurrencyDropDownChange() {
    BindBudgetData();
}


function calculateForMeritPct(rowData, currentValue, rowIndex, row) {
    //if (rowData.MeritRange == null || rowData.MeritRange == "&nbsp;" || rowData.MeritRange == " " || rowData.MeritRange == "" || rowData.MeritPerformanceRatingNum == null) {
    //    showAlert("Rating Guideline is not configured ");
    //    return false;
    //}
    var value = getNumberValue(currentValue, preferredCulture);
    if (value != null) {
        value = (currentValue.indexOf('%') > -1) ? value * 100 : value;
        value = roundingRule(value, "merit", "percentage");
        MeritPCT = rowData.MeritPCT;
        if (isNaN(value))
            value = 0;
        if (value > 999) {
            showAlert("Merit % exceeds the maximum limit");
            return false;
        }
        changeType = "pct";
        rowData.MeritPCT = value;
        if (!meritOverrideRule(rowData, rowIndex, row)) {
            return false;
        }
        return true;
    }
    else {
        changeType = "pct";
        rowData.MeritPCT = value;
        if (!meritOverrideRule(rowData, rowIndex, row)) {
            return false;
        }
        return true;
    }
}

function CalculateMeritAmtLocal(rowData, currentValue, rowIndex, row) {
    var proRationFactor = RuleConfiguration.ProrationRuleProrate ? rowData.ProRation : 1;
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        var meritAmtLocal = value;
        if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
            value = roundingRule(meritAmtLocal, "merit", "annual");
        }
        else {
            value = roundingRule(meritAmtLocal, "merit", "hourly");
        }
        var meritAmtUSD = value * rowData.MeritExchangeRate;
        if (!isNaN(value)) {
            if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
                var meritPct = ((value / (rowData.CurrentAnnualSalaryLocalForCalc * proRationFactor)) * 100 == Infinity) ? 0 : (value / (rowData.CurrentAnnualSalaryLocalForCalc * proRationFactor)) * 100;
            }
            else {
                var meritPct = ((value / (rowData.CurrentHourlyRateLocalForCalc * proRationFactor)) * 100 == Infinity) ? 0 : (value / (rowData.CurrentHourlyRateLocalForCalc * proRationFactor)) * 100;
            }
            value = (meritPct == 0 ? 0 : value);
            if (meritPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                return false;
            }
            changeType = "local";
            rowData.MeritPCT = meritPct;
            rowData.MeritAmtLocal = (meritPct != 0) ? value : 0;
            rowData.MeritAmtUSD = (meritPct != 0) ? meritAmtUSD : 0;
            if (!meritOverrideRule(rowData, rowIndex, row)) {
                return false;
            }
            return true;
        }
        else {
            showAlert("Please enter the data valid format");
            return false;
        }
    }
    else {
        changeType = "local";
        rowData.MeritPCT = null;
        rowData.MeritAmtLocal = null;
        rowData.MeritAmtUSD = null;
        if (!meritOverrideRule(rowData, rowIndex, row)) {
            return false;
        }
        return true;
    }
}

function CalculateMeritAmtUSD(rowData, currentValue, rowIndex, row) {
    //if (rowData.MeritRange == null || rowData.MeritRange == "&nbsp;" || rowData.MeritRange == " " || rowData.MeritRange == "" || rowData.MeritPerformanceRatingNum == null) {
    //    showAlert("Rating Guideline is not configured");
    //    return false;
    //}
    var proRationFactor = RuleConfiguration.ProrationRuleProrate ? rowData.ProRation : 1;
    var value = getNumberValue(currentValue, (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay && GridDisplay == "Local") ? rowData.CultureCode : preferredCulture);
    if (value != null) {
        var meritAmtUSD = value;
        value = roundingRule(meritAmtUSD, "general", "annual");
        var meritAmtLocal = (value / rowData.MeritExchangeRate);
        if (!isNaN(value)) {
            var meritPct = ((value / (rowData.CurrentAnnualSalaryUSDForCalc * proRationFactor)) * 100 == Infinity) ? 0 : (value / (rowData.CurrentAnnualSalaryUSDForCalc * proRationFactor)) * 100;
            if (meritPct > 999.00) {
                showAlert("Given increase exceeds the maximum limit");
                return false;
            }
            changeType = "usd";
            rowData.MeritPCT = meritPct;
            rowData.MeritAmtLocal = (meritPct != 0) ? meritAmtLocal : 0;
            rowData.MeritAmtUSD = (meritPct != 0) ? value : 0;
            if (!meritOverrideRule(rowData, rowIndex, row)) {
                return false;
            }
            return true;
        }
        else {
            showAlert("Please enter the data valid format");
            return false;
        }
    }
    else {
        changeType = "usd";
        rowData.MeritPCT = null;
        rowData.MeritAmtLocal = null;
        rowData.MeritAmtUSD = null;
        if (!meritOverrideRule(rowData, rowIndex, row)) {
            return false;
        }
        return true;
    }
}

function calculateMeritAmt(rowData, rowIndex, row) {
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    if (rowData == rowIndex)
        rowData = grdMeritGrid._data[rowIndex];
    LumpSumAmtUSD = rowData.LumpSumAmtUSD;
    if (rowData.MeritPCT != null) {
        var proRationFactor = RuleConfiguration.ProrationRuleProrate ? rowData.ProRation : 1;
        var newMeritAmtUSD;
        var newMeritAmtLocal;
        if (changeType == "pct") {
            if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
                if (GridDisplay == "USD") {
                    newMeritAmtUSD = ((rowData.CurrentAnnualSalaryUSDForCalc * rowData.MeritPCT * proRationFactor) / 100);
                    newMeritAmtLocal = (newMeritAmtUSD / rowData.MeritExchangeRate);
                }
                else {
                    newMeritAmtLocal = ((rowData.CurrentAnnualSalaryLocalForCalc * rowData.MeritPCT * proRationFactor) / 100);
                    newMeritAmtUSD = (newMeritAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate;
                }
            }
            else {
                if (GridDisplay == "USD") {
                    newMeritAmtUSD = ((rowData.CurrentHourlyRateUSDForCalc * rowData.MeritPCT * proRationFactor) / 100);
                    newMeritAmtLocal = (newMeritAmtUSD / rowData.MeritExchangeRate);
                }
                else {
                    newMeritAmtLocal = ((rowData.CurrentHourlyRateLocalForCalc * rowData.MeritPCT * proRationFactor) / 100);
                    newMeritAmtUSD = (newMeritAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate;
                }
            }
        }
        else {
            newMeritAmtUSD = rowData.MeritAmtUSD;
            newMeritAmtLocal = rowData.MeritAmtLocal;
        }
        rowData.MeritPCT = roundingRule(rowData.MeritPCT, "merit", "percentage");
        rowData.MeritAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(newMeritAmtLocal, "merit", "annual") : roundingRule(newMeritAmtLocal, "merit", "hourly");
        newMeritAmtUSD = (rowData.MeritAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate;
        rowData.MeritAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(newMeritAmtUSD, "general", "annual") : roundingRule(newMeritAmtUSD, "general", "hourly");

    }
    else {
        rowData.MeritPCT = null;
        rowData.MeritAmtLocal = null;
        rowData.MeritAmtUSD = null;
        rowData.HrlyMeritAmtLocal = null;
        rowData.HrlyMeritAmtUSD = null;
        HideSubmitApproveCheckBoxes(rowData);
    }
    rowData.IsMeritEdited = true;
    rowData.dirty = true;
    objChangeFlag = true;
    lumpSumRule(rowData, "merit");
    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    if (rowData.EmployeeStatus.toLowerCase() == "annual")
        fn_BudgetMeritSpentChanges((MeritAmtLocal != null) ? (MeritAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0, (rowData.MeritAmtLocal != null) ? (rowData.MeritAmtLocal / rowData.MeritExchangeRate) * selectedData.ExchangeRate : 0, (LumpSumAmtUSD != null) ? LumpSumAmtUSD : 0, (rowData.LumpSumAmtUSD != null) ? rowData.LumpSumAmtUSD : 0);
    else
        fn_BudgetMeritSpentChanges((MeritAmtLocal != null) ? (rowData.TotalWorkHrs != null ? ((MeritAmtLocal * rowData.TotalWorkHrs) / rowData.MeritExchangeRate) * selectedData.ExchangeRate : ((MeritAmtLocal * 2080) / rowData.MeritExchangeRate) * selectedData.ExchangeRate) : 0, (rowData.MeritAmtLocal != null) ? (rowData.TotalWorkHrs != null ? ((rowData.MeritAmtLocal * rowData.TotalWorkHrs) / rowData.MeritExchangeRate) * selectedData.ExchangeRate : ((rowData.MeritAmtLocal * 2080) / rowData.MeritExchangeRate) * selectedData.ExchangeRate) : 0, (LumpSumAmtUSD != null) ? (rowData.TotalWorkHrs != null ? LumpSumAmtUSD : LumpSumAmtUSD) : 0, (rowData.LumpSumAmtUSD != null) ? (rowData.TotalWorkHrs != null ? rowData.LumpSumAmtUSD : rowData.LumpSumAmtUSD) : 0);
    var index = row.index();
    checkMeritGuideLine(rowData, index);
    refreshRow(grdMeritGrid, row);
}


function checkCompGuideLines(rowData, index) {
    checkMeritGuideLine(rowData, index);
    checkAdjustmentGuideLine(rowData, index);
    checkPromotionGuideLine(rowData, index);
    checkLumpSumGuideLine(rowData, index);
}

function checkMeritGuideLine(rowData, index) {
    if (CheckGuideLineExceed(rowData.MeritRange, rowData.MeritPCT))
        setColor(index, GridDisplay == "USD" ? "usd" : "local", rowData.IsMeritEligible ? "Red" : "");
    else
        setColor(index, GridDisplay == "USD" ? "usd" : "local", "");
}

function checkAdjustmentGuideLine(rowData, index) {
    if (CheckGuideLineExceed(rowData.MeritRange, rowData.AdjustmentPct))
        setColor(index, GridDisplay == "USD" ? "AdjustmentUSD" : "AdjustmentLocal", "Red");
    else
        setColor(index, GridDisplay == "USD" ? "AdjustmentUSD" : "AdjustmentLocal", "");
}
function checkBonusGuideLine(rowData, index) {
    if (CheckGuideLineExceed(rowData.MeritRange, rowData.BonusPct))
        setColor(index, "BonusAmt", "Red");
    else
        setColor(index, "BonusAmt", "");
}

function checkPromotionGuideLine(rowData, index) {
    if (CheckGuideLineExceed(rowData.MeritRange, rowData.PromotionPct))
        setColor(index, GridDisplay == "USD" ? "promotionUSD" : "promotionLocal", rowData.IsPromotionEligible ? "Red" : "");
    else
        setColor(index, GridDisplay == "USD" ? "promotionUSD" : "promotionLocal", "");
}

function checkLumpSumGuideLine(rowData, index) {
    if (CheckGuideLineExceed(rowData.MeritRange, rowData.LumpSumPct))
        setColor(index, GridDisplay == "USD" ? "LumpSumUSD" : "LumpSumLocal", rowData.IsMeritEligible ? "Red" : "");
    else
        setColor(index, GridDisplay == "USD" ? "LumpSumUSD" : "LumpSumLocal", "");
}


function rollbackMeritPct(rowData, rowIndex, row) {
    var grdMerit = $("#ReporteeGrid").data("kendoGrid");
    if (rowData == rowIndex)
        rowData = grdMerit._data[rowIndex];
    rowData.MeritPCT = MeritPCT;
    rowData.MeritAmtLocal = MeritAmtLocal;
    rowData.MeritAmtUSD = MeritAmtUSD;
    refreshRow(grdMerit, row);
}


function fn_BudgetMeritSpentChanges(oldMeritValue, newMeritValue, oldLumpsumValue, newLumpsumValue) {
   
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var cultureCode = (selectedData != undefined) ? selectedData.CultureCode : preferredCulture;
    var newMeritIncreaseValue = newMeritValue - oldMeritValue;
    var newLumpsumIncreaseValue = newLumpsumValue - oldLumpsumValue;
    var hdnPromotionSpent = document.getElementById("hdnPromotionSpent").value;
    var hdnAdjustmentSpent = document.getElementById("hdnAdjustmentSpent").value;
    var hdnMeritSpent = document.getElementById("hdnMeritSpent");
    var hdnLumpSumSpent = document.getElementById("hdnLumpSumSpent");
    var hdnMeritBudget = document.getElementById("hdnMeritBudget").value;
    var hdnMeritSpentValue = hdnMeritSpent.value;
    var hdnLumpsumSpentValue = hdnLumpSumSpent.value;
    var lblMeritSpent = document.getElementById("meritSpentAmt");
    var lblLumpsumSpent = document.getElementById("lumpsumSpentAmt");
    var lblTotalMeritSpent = document.getElementById("totalMeritSpentAmt");
    var lblTotalLumpsumSpent = document.getElementById("totalLumpsumSpentAmt");

    var lblBalance = document.getElementById("meritBalance");
    var newMeritSpentAmt = (eval(hdnMeritSpentValue) + eval(newMeritIncreaseValue));
    var newLumpsumSpentAmt = (eval(hdnLumpsumSpentValue) + eval(newLumpsumIncreaseValue));
    hdnMeritSpent.value = newMeritSpentAmt;
    hdnLumpSumSpent.value = newLumpsumSpentAmt;
    newMeritSpentAmt1 = (eval(newMeritSpentAmt) != 0) ? roundBudget(eval(newMeritSpentAmt)) : 0;
    newMeritSpentAmt = (newMeritSpentAmt1 != NaN) ? newMeritSpentAmt : 0;
    hdnMeritSpent1 = (eval(hdnMeritSpent) != 0) ? roundBudget(eval(hdnMeritSpent)) : 0;
    hdnMeritSpent = (hdnMeritSpent1 != NaN) ? hdnMeritSpent : 0;
    hdnPromotionSpent1 = (eval(hdnPromotionSpent) != 0) ? roundBudget(eval(hdnPromotionSpent)) : 0;
    hdnPromotionSpent = (hdnPromotionSpent1 != NaN) ? hdnPromotionSpent : 0;
    newLumpsumSpentAmt1 = (eval(newLumpsumSpentAmt) != 0) ? roundBudget(eval(newLumpsumSpentAmt)) : 0;
    newLumpsumSpentAmt = (newLumpsumSpentAmt1 != NaN) ? newLumpsumSpentAmt : 0;
    hdnMeritBudget = (eval(hdnMeritBudget) != 0) ? roundBudget(eval(hdnMeritBudget)) : 0;
    lblMeritSpent.innerHTML = formatBudgetCurrency(eval(newMeritSpentAmt), 'c0', cultureCode);
    if(lblLumpsumSpent!=null)
    lblLumpsumSpent.innerHTML = formatBudgetCurrency(eval(newLumpsumSpentAmt), 'c0', cultureCode);
    newLumpsumSpentAmt = newLumpsumSpentAmt; //(RuleConfiguration.MeritValuesReCalculate == false && RuleConfiguration.LumpSumRuleLumpSumType == "AutoCalc") ? 0 : newLumpsumSpentAmt;
    var totalSpent = (eval(newMeritSpentAmt) + eval(hdnPromotionSpent) + eval(newLumpsumSpentAmt));
    var newBalance = (eval(hdnMeritBudget) - eval(totalSpent));
    lblBalance.innerHTML = formatBudgetCurrency(eval(newBalance), 'c0', cultureCode);
    var balancepct = "(";
    newBalance = Math.round(newBalance);
    if (newBalance < 0) {
        $("#meritBalance").removeClass("counter2");
        $("#meritBalance").addClass("counter3");
        $("#balancePct").removeClass("counter2");
        $("#balancePct").addClass("counter3");
    }
    else {
        $("#meritBalance").removeClass("counter3");
        $("#meritBalance").addClass("counter2");
        $("#balancePct").addClass("counter2");
        $("#balancePct").removeClass("counter3");
    }

    totalSpent = roundToWholeNumber(eval(newMeritSpentAmt)) + roundToWholeNumber(eval(hdnPromotionSpent)) + roundToWholeNumber(eval(newLumpsumSpentAmt));
    var oldBalanceClass = document.getElementById("balanceDonughtClass").className;
    var a = ((eval(Math.abs(newBalance)) / eval(totalCurrentSalary)) * 100).toFixed(0);
    var balancepct = ((eval(Math.abs(newBalance)) / eval(totalCurrentSalary)) * 100).toFixed(0);
    var balanceChartPct = Math.round((balancepct / BudgetPercentage) * 100);
    //var balanceChartPct = (balancepct / BudgetPercentage) * 100;
    (newBalance > 0) ? $("#balancePct").text(a.toString() + "%") : $("#balancePct").text("(" + a + "%)");
    if (BudgetPercentage == balancepct) {

        var newBalanceClass = "c100 small p" + 100;
    }
    else {
       
        var MeritColor = newBalance > 0 ? 1 : newBalance == 0 ? 0 : -1;

        if (MeritColor == 1) {
           
            var newBalanceClass = "c100 small p" + balanceChartPct;
        }
        else if (MeritColor == 0 ) {
            var newBalanceClass = "c100 small p100" ;
        }
        else if (MeritColor == -1) {
           
            var newBalanceClass = "c100 small x100 p100";           
        }
        
    }
    $("#balanceDonughtClass").removeClass(oldBalanceClass);
    $("#balanceDonughtClass").addClass(newBalanceClass);
    //(newBalance > 0) ? $("#balancePct").text(a.toString() + "%") : $("#balancePct").text("(" + a + "%)");
}

function roundToWholeNumber(value) {
    return Number(Math.round(value));
}

function roundBudget(value) {
    return Number(Math.round(value + 'e2') + 'e-2')
}

function lumpSumRule(rowData, type) {
    //if (((RuleConfiguration.LumpSumRuleLumpSumType == meritPageConstants.LumpSumTypeAutoCalc) && (RuleConfiguration.AutoCalculateLumpSum == "YES")) || ((RuleConfiguration.LumpSumRuleLumpSumType == meritPageConstants.LumpSumTypeAutoCalc) && (RuleConfiguration.AutoCalculateLumpSum =="NO"))) {
    if (RuleConfiguration.LumpSumRuleLumpSumType == meritPageConstants.LumpSumTypeAutoCalc) {
        var proRationFactor = RuleConfiguration.ProrationRuleProrate ? rowData.ProRation : 1;
        //if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
        var meritAmountLocal = (rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0;
        var promotionAmountLocal = (rowData.PromotionAmtLocal != null) ? rowData.PromotionAmtLocal : 0;
        var adjustmentAmountLocal = (rowData.AdjustmentAmtLocal != null) ? rowData.AdjustmentAmtLocal : 0;
        //}
        //else {
        //    var meritAmountLocal = (rowData.MeritAmtLocal != null) ? (rowData.TotalWorkHrs != null ? (rowData.MeritAmtLocal * rowData.TotalWorkHrs) : (rowData.MeritAmtLocal * 2080)) : 0;
        //    var promotionAmountLocal = (rowData.PromotionAmtLocal != null) ? (rowData.TotalWorkHrs != null ? (rowData.PromotionAmtLocal * rowData.TotalWorkHrs) : (rowData.PromotionAmtLocal * 2080)) : 0;
        //    var adjustmentAmountLocal = (rowData.AdjustmentAmtLocal != null) ? (rowData.TotalWorkHrs != null ? (rowData.AdjustmentAmtLocal * rowData.TotalWorkHrs) : (rowData.AdjustmentAmtLocal * 2080)) : 0;
        //}
        var currentSalaryRate = (rowData.EmployeeStatus.toLowerCase() == 'hourly') ? (rowData.CurrentHourlyRateLocalForCalc) : ((rowData.CurrentAnnualSalaryLocalForCalc != null) ? rowData.CurrentAnnualSalaryLocalForCalc : 0);
        var newSalaryCalcRate = currentSalaryRate + meritAmountLocal + promotionAmountLocal + adjustmentAmountLocal;
        var newSalaryLocal = (rowData.EmployeeStatus.toLowerCase() == 'annual') ? newSalaryCalcRate : (newSalaryCalcRate * (rowData.TotalWorkHrs != null ? rowData.TotalWorkHrs : 2080));
        var newHourlyRateLocal = (rowData.EmployeeStatus.toLowerCase() == 'annual') ? (newSalaryCalcRate / (rowData.TotalWorkHrs != null ? rowData.TotalWorkHrs : 2080)) : newSalaryCalcRate;
        rowData.NewSalaryLocal = roundingRule(newSalaryLocal, "newsalary", "annual");
        rowData.NewHourlyRateLocal = roundingRule(newHourlyRateLocal, "newsalary", "hourly");
        var lumpSumRangeMaxPct = RuleConfiguration.LumpSumRuleRangeMaxPct;
        var LumpSumRuleRangeMaxAmt = RuleConfiguration.LumpSumRuleRangeMaxAmt;
        
        var cuttOffValueLocal = (lumpSumRangeMaxPct != 0) ? ((currentSalaryRate > ((lumpSumRangeMaxPct * rowData.MktMaxLocal) / 100)) ? currentSalaryRate : ((lumpSumRangeMaxPct * rowData.MktMaxLocal) / 100)) : (((LumpSumRuleRangeMaxAmt + rowData.MktMaxLocal) > currentSalaryRate) ? (LumpSumRuleRangeMaxAmt + rowData.MktMaxLocal) : currentSalaryRate);
        var newsalaryForCalc = (rowData.EmployeeStatus.toLowerCase() == 'annual') ? newSalaryLocal : newHourlyRateLocal;
        if (newsalaryForCalc > cuttOffValueLocal) {
            if (!RuleConfiguration.MeritValuesReCalculate) {
                cuttOffValueLocal = (lumpSumRangeMaxPct == 0) ? (((LumpSumRuleRangeMaxAmt + rowData.MktMaxLocal) > rowData.CurrentAnnualSalaryLocalForCalc) ? rowData.MktMaxLocal : rowData.CurrentAnnualSalaryLocalForCalc) : cuttOffValueLocal;
                cuttOffValueLocal = (rowData.CurrentAnnualSalaryLocalForCalc > rowData.MktMaxLocal) ? rowData.CurrentAnnualSalaryLocalForCalc : cuttOffValueLocal;
            }
            var tempLumpSumAmtLocal = newsalaryForCalc - cuttOffValueLocal;
            var lumpSumAmtLocal = (rowData.EmployeeStatus.toLowerCase() == 'annual') ? tempLumpSumAmtLocal : (tempLumpSumAmtLocal * (rowData.TotalWorkHrs != null ? rowData.TotalWorkHrs : 2080));
            rowData.LumpSumAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(lumpSumAmtLocal, "lumpsum", "annual") : roundingRule(lumpSumAmtLocal, "lumpsum", "hourly");
            rowData.LumpSumAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.LumpSumAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.LumpSumAmtLocal * rowData.MeritExchangeRate, "general", "hourly");

            var lumpSumPct = (rowData.LumpSumAmtLocal * 100) / rowData.CurrentAnnualSalaryLocalForCalc;
            rowData.LumpSumPct = roundingRule(lumpSumPct, "lumpsum", "percentage");
            if (RuleConfiguration.MeritValuesReCalculate) {
                if (rowData.EmployeeStatus == 'annual') {
                    var newMeritAmtLocal = ((rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0) - tempLumpSumAmtLocal;
                    var meritPct = (newMeritAmtLocal * 100) / (rowData.CurrentAnnualSalaryLocalForCalc * proRationFactor);
                    meritPct = roundingRule(meritPct, "merit", "percentage");
                }
                else {
                    var newMeritAmtLocal = ((rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0) - tempLumpSumAmtLocal;
                    var meritPct = (newMeritAmtLocal * 100) / (rowData.CurrentHourlyRateLocalForCalc * proRationFactor);
                    meritPct = roundingRule(meritPct, "merit", "percentage");
                }
                rowData.MeritPCT = meritPct;
                rowData.MeritAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(newMeritAmtLocal, "merit", "annual") : roundingRule(newMeritAmtLocal, "merit", "hourly");
                rowData.MeritAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.MeritAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.MeritAmtLocal * rowData.MeritExchangeRate, "general", "hourly");

            }
        }
        else {
            rowData.LumpSumAmtUSD = 0;
            rowData.LumpSumAmtLocal = 0;
            rowData.LumpSumPct = 0;
        }
    }
}

function CalculateNewSalary(rowData) {
    var meritAmountLocal = 0;
    var promotionAmountLocal = 0;
    var adjustmentAmountLocal = 0;
    var newSalaryLocal = 0;
    if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
        meritAmountLocal = (rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0;
        promotionAmountLocal = (rowData.PromotionAmtLocal != null) ? rowData.PromotionAmtLocal : 0;
        adjustmentAmountLocal = (rowData.AdjustmentAmtLocal != null) ? rowData.AdjustmentAmtLocal : 0;
    }
    else {
        meritAmountLocal = (rowData.MeritAmtLocal != null) ? (rowData.TotalWorkHrs != null ? (rowData.MeritAmtLocal * rowData.TotalWorkHrs) : (rowData.MeritAmtLocal * 2080)) : 0;
        promotionAmountLocal = (rowData.PromotionAmtLocal != null) ? (rowData.TotalWorkHrs != null ? (rowData.PromotionAmtLocal * rowData.TotalWorkHrs) : (rowData.PromotionAmtLocal * 2080)) : 0;
        adjustmentAmountLocal = (rowData.AdjustmentAmtLocal != null) ? (rowData.TotalWorkHrs != null ? (rowData.AdjustmentAmtLocal * rowData.TotalWorkHrs) : (rowData.AdjustmentAmtLocal * 2080)) : 0;
    }
    //var meritAmountLocal = (rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0;
    //var promotionAmountLocal = (rowData.PromotionAmtLocal != null) ? rowData.PromotionAmtLocal : 0;
    //var adjustmentAmountLocal = (rowData.AdjustmentAmtLocal != null) ? rowData.AdjustmentAmtLocal : 0;
    if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
        newSalaryLocal = ((rowData.CurrentAnnualSalaryLocalForNewSalaryCalc != null) ? rowData.CurrentAnnualSalaryLocalForNewSalaryCalc : 0) + meritAmountLocal + promotionAmountLocal + adjustmentAmountLocal;
    }
    else {
        newSalaryLocal = ((rowData.CurrentAnnualSalaryLocalForNewSalaryCalc != null) ? rowData.CurrentAnnualSalaryLocalForNewSalaryCalc : 0) + meritAmountLocal + promotionAmountLocal + adjustmentAmountLocal;
        // newSalaryLocal = (rowData.TotalWorkHrs != null) ? (newSalaryLocal * rowData.TotalWorkHrs) : (newSalaryLocal*2080);
    }
    var lumpsumAmtLocal = (rowData.LumpSumAmtLocal != null) ? rowData.LumpSumAmtLocal : 0;
    var newSalaryLocalForLumpSum = newSalaryLocal;
    //newSalaryLocal = (RuleConfiguration.MeritValuesReCalculate == false) ? newSalaryLocal - lumpsumAmtLocal : newSalaryLocal;
    if (newSalaryLocal < rowData.CurrentAnnualSalaryLocalForCalc && rowData.CurrentAnnualSalaryLocalForCalc > rowData.MktMaxLocal) {
        newSalaryLocal = rowData.CurrentAnnualSalaryLocalForCalc;
        lumpsumAmtLocal = meritAmountLocal;
    }
    var newSalaryLocalNew = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(newSalaryLocal, "newsalary", "annual") : roundingRule(newSalaryLocal, "newsalary", "hourly");
    var newSalaryUSDNew = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.NewSalaryLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.NewSalaryLocal * rowData.MeritExchangeRate, "general", "hourly");
    if (newSalaryLocalNew <= rowData.CurrentAnnualSalaryLocalForCalc) {
        rowData.NewSalaryLocal = newSalaryLocal;
        rowData.NewHourlyRateLocal = rowData.NewSalaryLocal / (rowData.TotalWorkHrs != null ? rowData.TotalWorkHrs : 2080);
        rowData.NewHourlyRateLocal = roundingRule(rowData.NewHourlyRateLocal, "newsalary", "hourly");
    }
    else {
        rowData.NewSalaryLocal = newSalaryLocalNew;
        rowData.NewHourlyRateLocal = rowData.NewSalaryLocal / (rowData.TotalWorkHrs != null ? rowData.TotalWorkHrs : 2080);
        rowData.NewHourlyRateLocal = roundingRule(rowData.NewHourlyRateLocal, "newsalary", "hourly");
    }

    rowData.NewSalaryUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.NewSalaryLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.NewSalaryLocal * rowData.MeritExchangeRate, "general", "hourly");
    rowData.NewHourlyRateUSD = rowData.NewSalaryUSD / (rowData.TotalWorkHrs != null ? rowData.TotalWorkHrs : 2080);
    var newCompaRatio = 0;
    if (rowData.EmployeeStatus.toLowerCase() == 'annual') {
        newCompaRatio = (rowData.MktMidLocal == null || rowData.MktMidLocal == 0) ? 0 : (rowData.NewSalaryLocal / rowData.MktMidLocal) * 100;
    }
    else {

        newCompaRatio = (rowData.MktMidLocal == null || rowData.MktMidLocal == 0) ? 0 : (rowData.NewHourlyRateLocal / rowData.MktMidLocal) * 100;
    }
    rowData.NewCompaRatio = roundingRule(newCompaRatio, "", "comparatio");
    if (RuleConfiguration.MeritValuesReCalculate == false)
        rowData.LumpSumAmtLocal = lumpsumAmtLocal;
    if (rowData.LumpSumPct == null) {
        rowData.LumpSumAmtLocal = null;
        rowData.LumpSumAmtUSD = null;
    }
}

function CalculateTCC(rowData) {
    var lumpsumAmtLocal = (rowData.LumpSumAmtLocal != null) ? rowData.LumpSumAmtLocal : 0;
    var lumpsumAmtUSD = (rowData.LumpSumAmtUSD != null) ? rowData.LumpSumAmtUSD : 0;
    var bonusAmtLocal = (rowData.AdjustedBonusAMTLocal != null) ? rowData.AdjustedBonusAMTLocal : 0;
    var payoutAmt = (rowData.Payout != null) ? rowData.Payout : 0;
    var bonusAmtUSD = (rowData.AdjustedBonusAmtUSD != null) ? rowData.AdjustedBonusAmtUSD : 0;
    var newSalLocal = (rowData.NewSalaryLocal != null) ? rowData.NewSalaryLocal : 0;
    var lumpsumAmtLocal = (rowData.LumpSumAmtLocal != null) ? rowData.LumpSumAmtLocal : 0;
    //newSalLocal = (RuleConfiguration.MeritValuesReCalculate == false) ? newSalLocal - lumpsumAmtLocal : newSalLocal;
    var newSalUSD = (rowData.NewSalaryUSD != null) ? rowData.NewSalaryUSD : 0;
    var tccLocal = newSalLocal + lumpsumAmtLocal;
    var tccUSD = newSalUSD + lumpsumAmtUSD;
    rowData.TCCLocal = roundingRule(tccLocal, "tccgeneral", "annual");
    rowData.TCCUSD = roundingRule(tccUSD, "tccgeneral", "annual");
}

function BindCompTreeView() {
    var token = $('input[name="__RequestVerificationToken"]').val();
  
    $.ajax({
        url: "../Compensation/GetCompManagerTreeData",
        type: "POST",
        data: { __RequestVerificationToken: token, managerNum: $("#hndSelectedManagerNum").val(), pageType: $("#hndPageType").val() },
        cache: false,
        success: function (dataValue) {
            var datasource = getTreeViewHierarchialDataSource(dataValue, "ManagerNum", "ReportingManagerNum", "IsTreeTop", "RowNumber");
            var treeView = $("#ddlCompManagerTreeView").kendoExtDropDownTreeView({
                treeview: {
                    template: '#=ShowStatusCompManagerTree(item)#',
                    dataTextField: "ManagerName",
                    dataValueField: "ManagerNum",
                    loadOnDemand: false,
                    dataSource: datasource,
                    select: ddlCompManagerTreeOnSelect
                }
            }).data("kendoExtDropDownTreeView");
            var managerNum = $("#hndSelectedManagerNum").val();
            var managerLineage = $("#hndManagerLineage").val();
            var MenuType = $("#hndMenuType").val();
            var rowNumber = $("#hndRowNumber").val();
            if (treeView != undefined) {
                treeView._treeview.expand(".k-item");
                if (rowNumber == undefined || rowNumber == null || rowNumber == "") {
                    if (isMyApproval == "True") {
                        getManagerName();
                    }
                    else {
                        var nodeText = "";
                        var nodeSelect = "";
                        if (treeView._treeview.dataSource._data.length > 0) {
                            if (treeView._treeview.dataSource._data[0].ManagerName == "My Organization") {
                                if (treeView._treeview.dataSource._data.length == 1) {
                                    nodeText = treeView._treeview.dataSource._data[0].ManagerName + " (" + treeView._treeview.dataSource._data[0].CompCompletedCount + "/" + treeView._treeview.dataSource._data[0].ReporteeCount + ")";
                                    nodeSelect = treeView._treeview.findByUid(treeView._treeview.dataSource._data[0].uid);
                                    $("#hndSelectedManagerNum").val(treeView._treeview.dataSource._data[0].ManagerNum)
                                    $("#hndMenuType").val(treeView._treeview.dataSource._data[0].MenuType)
                                }
                                else {
                                    nodeText = treeView._treeview.dataSource._data[1].ManagerName + " (" + treeView._treeview.dataSource._data[1].CompCompletedCount + "/" + treeView._treeview.dataSource._data[1].ReporteeCount + ")";
                                    nodeSelect = treeView._treeview.findByUid(treeView._treeview.dataSource._data[1].uid);
                                    $("#hndSelectedManagerNum").val(treeView._treeview.dataSource._data[1].ManagerNum)
                                    $("#hndMenuType").val(treeView._treeview.dataSource._data[1].MenuType)
                                }
                            }
                            else {
                                nodeText = treeView._treeview.dataSource._data[0].ManagerName + " (" + treeView._treeview.dataSource._data[0].CompCompletedCount + "/" + treeView._treeview.dataSource._data[0].ReporteeCount + ")";
                                nodeSelect = treeView._treeview.findByUid(treeView._treeview.dataSource._data[0].uid);
                                $("#hndSelectedManagerNum").val(treeView._treeview.dataSource._data[0].ManagerNum)
                                $("#hndMenuType").val(treeView._treeview.dataSource._data[0].MenuType)
                                refreshGrid();
                            }
                        }
                        setCompTreeViewDrodDownText(nodeText);
                        treeView._treeview.select(nodeSelect);
                        if (treeView._treeview.dataSource._data.length != 0) {
                            $("#SelectedManagerName").text(treeView._treeview.dataSource._data[1].ManagerName);
                        }
                    }
                }
            }
        }
    });
    if (DefaultCurrencyNum != '0')
        BindBudgetData();
}

function setCompTreeViewDrodDownText(text) {
    $('#ddlCompManagerTreeView').find('.k-input').text(text);
}

function ddlCompManagerTreeOnSelect(e) {
    if (!showSaveWarning(e, "objChangeFlag")) {
        e._defaultPrevented = true;
        return false;
    }
    var node = e.sender.dataItem(e.node);
    $("#ManagerTreesearch").val("");
    CallManagerAction(node);
    if ($("#clearfilter").is(":visible")) {
        $("#clearfilter").trigger("click");
    }
   // $("span.k-in").parent().find('span').removeClass("highlight");
    $('#ddlCompManagerTreeView span.k-in').find('span.highlight').removeClass('highlight')
    
}


$(document).on('click', '#menureanalytic,#home,#menuworkforce,#menuexchangerate,#menumanageuser,#menurules,#menubudget,#menucommunication,#menuworkflow,#menurating,#menucustomizeview,#menurecommendincrease', function (e) {
    if (!showSaveWarning(e, "objChangeFlag")) {
        e._defaultPrevented = true;
        return false;
    }
});
function CallManagerAction(node, employeeJobNum, employeeNum) {
    var managerNum;
    if (node.ManagerName == "My Organization") {
        $("#hndIsRollup").val(1);
        managerNum = node.LoggedInEmployeeNum;
        $("#hndLoggedInEmployeeNum").val(node.LoggedInEmployeeNum);
        myOrgNode = node.ManagerName;
        if (meritPageConstants.Multicurrency == "False")
            $("#inDirectBudgetSection").hide();
        else
            $("#budgetCheckBox").hide();
    }
    else {
        managerNum = node.ManagerNum;
        $("#hndIsRollup").val(0);
        if (meritPageConstants.Multicurrency == "False")
            $("#inDirectBudgetSection").show();
        else
            $("#budgetCheckBox").show();
        //$("#budgetCheckBox").show();
    }

    $("#hndSelectedManagerNum").val(managerNum);
    $("#hndManagerLineage").val(node.ManagerLineage);
    $("#hndMenuType").val(node.MenuType);
    $("#hndRowNumber").val(node.RowNumber);
    getApprovalStatus();
    refreshGridTreeChange();
    $("#SelectedManagerName").text(node.ManagerName);
    $("#sidebar-wrapper").toggleClass("active");
}

function ShowStatusCompManagerTree(item, highLightText) {
    //var finalImg = getMeritTreeViewStatusImage(item);
    var Class = "";
    if (item.OverFlow) {
        Class = " class='OverFlow'";
    }

    var finalTitle = "";
    if (item.IsOverSpent == true)
        finalTitle = "<span style='color:red' id=" + item.ManagerNum + "_" + item.ManagerNum + Class + ">" + managerTreeViewHighLightText(item.ManagerName,highLightText) + " (<span id=" + item.ManagerNum + ">" + item.CompCompletedCount + "</span>/" + item.ReporteeCount + ")" + "</span> ";// + finalImg;
    else
        finalTitle = "<span id=" + item.ManagerNum + "_" + item.ManagerNum + Class + ">" + managerTreeViewHighLightText(item.ManagerName, highLightText) + " (<span id=" + item.ManagerNum + ">" +item.CompCompletedCount + "</span>/" +item.ReporteeCount + ")" + "</span> ";//+ finalImg;
    return finalTitle;
}

function managerTreeViewHighLightText(text, highlightText)
{
    
    if (highlightText == undefined || highlightText == null || highlightText == "") return text;

    var highlightTextLength = highlightText.length;
    
    var p = text.toUpperCase().indexOf(highlightText.toUpperCase());

    if (p < 0) return text;

    var s1 = '', s2 = '';

    var high = '<span class="highlight">' + text.substr(p, highlightTextLength) + '</span>';

    if (p > 0) {
        s1 = text.substr(0, p);
    }

    if (p + highlightTextLength < text.length) {
        s2 = text.substring(p + highlightTextLength)
    }

    var result = s1 + high + s2;
    return result;
}

function meritTreeflag(ManagerNum, ManagerLevelAppStatus) {
    if (ManagerLevelAppStatus == 1) {
        document.getElementById("imgApproval_" + ManagerNum).style.color = "#FE6D00";
    }
    else if (ManagerLevelAppStatus == 2) {
        document.getElementById("imgApproval_" + ManagerNum).style.color = "#29D800";
    }
    else if (ManagerLevelAppStatus == 3) {
        document.getElementById("imgApproval_" + ManagerNum).style.color = "#FF0000";
    }
    else if (ManagerLevelAppStatus == 4) {
        document.getElementById("imgApproval_" + ManagerNum).style.color = "#FBCA00";
    }
    else {
        document.getElementById("imgApproval_" + ManagerNum).style.color = "transparent";
    }

}

function getMeritTreeViewStatusImage(item) {
    var ManagerLevelAppStatus = item.ManagerLevelAppStatus;
    var ManagerIDNum = item.ManagerNum;
    var img = "";
    if (ManagerLevelAppStatus == 1) {
        img = "<span id='imgApproval_" + ManagerIDNum + "' class='glyphicon glyphicon-one-fine-dot' style='color:#FE6D00'/>"; //orange
    }
    else if (ManagerLevelAppStatus == 2) {
        img = "<span id='imgApproval_" + ManagerIDNum + "' class='glyphicon glyphicon-one-fine-dot' style='color:#29D800'/>"; //Green
    }
    else if (ManagerLevelAppStatus == 3) {
        img = "<span id='imgApproval_" + ManagerIDNum + "' class='glyphicon glyphicon-one-fine-dot' style='color:#FF0000'/>"; //red
    }
    else if (ManagerLevelAppStatus == 4) {
        img = "<span id='imgApproval_" + ManagerIDNum + "' class='glyphicon glyphicon-one-fine-dot' style='color:#FBCA00'/>"; //yellow
    }
    else if (ManagerLevelAppStatus == 0) {
        img = "<span id='imgApproval_" + ManagerIDNum + "' class='glyphicon glyphicon-one-fine-dot' style='color:transparent'/>"; //yellow
    }
    if ((img != "") == true)
        finalImg = "" + img + "";
    else
        finalImg = "";

    return finalImg;
}

function getCompCompletedCount() {
    var compMenuType = $("#hndMenuType").val();
    var selectedManagerNum = $("#hndSelectedManagerNum").val();
    if (compMenuType == 1 && ($("#hndIsRollup").val() != 1)) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '../Compensation/GetCompCompletedCount',
            type: "Post",
            data: {
                __RequestVerificationToken:token,
                selectedManagerNum: selectedManagerNum
            },
            success: function (result) {
                $("#" + selectedManagerNum)[0].innerText = result[0];
                var selectedManagerText = $("#" + selectedManagerNum + "_" + selectedManagerNum)[0].innerText;
                if (result[1] == 1)
                    $("#" + selectedManagerNum + '_' + selectedManagerNum)[0].style.color = "red";
                else
                    $("#" + selectedManagerNum + '_' + selectedManagerNum)[0].style.color = "#6e6259 ";
                setCompTreeViewDrodDownText(selectedManagerText);
            }
        });
    }
}







function getManagerName() {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '../Compensation/GetManagerName',
        type: "post",
        cache: false,
        data: {
            __RequestVerificationToken: token,
            selectedManagerNum: $("#hndSelectedManagerNum").val()
        },
        success: function (result) {
            setCompTreeViewDrodDownText(result);
            isMyApproval = "False";
        }
    })
}


function ChangeMeritPerfRating(rowData, row, index, RatingRangeText) {

    var guideline;
    var grdMeritGrid = $("#ReporteeGrid").data("kendoGrid");
    var id = "#ddlMeritPerfRating_" + rowData.Empjobnum;
    var selectedData = $("#ddlMeritPerfRating_" + rowData.Empjobnum).data("kendoDropDownList").dataItem($("#ddlMeritPerfRating_" + rowData.Empjobnum).data("kendoDropDownList").select());
    var hdnPerfMaster = rowData.RatingAndGuideline;
    if (hdnPerfMaster != null) {
        var splitPerfMaster = hdnPerfMaster.split("|");
        for (var i = 0; i < splitPerfMaster.length; i++) {
            var rating = splitPerfMaster[i].split("_");
            if (rating[0] == selectedData.Value) {
                guideline = rating[1];
                break;
            }
        }
        if (guideline == undefined) {
            rowData.MeritRange = null;
            rowData.MeritPerformanceRating = selectedData.Text;
            rowData.MeritPerformanceRatingNum = selectedData.Value;
            return false;
        }
        if (guideline.length > 0) {
            rowData.MeritRange = guideline;
            rowData.MeritPerformanceRating = selectedData.Text;
            rowData.MeritPerformanceRatingNum = selectedData.Value;
            checkCompGuideLines(rowData, index);
        }
    }
    else {

        rowData.MeritPerformanceRating = null;
        rowData.MeritPerformanceRating = selectedData.Text;
        rowData.MeritPerformanceRatingNum = selectedData.Value;
        // if (RatingRangeText != null) {
        rowData.MeritRange = RatingRangeText;
        rowData.MeritIncreaseGuideline = RatingRangeText;
        // }
        rowData.dirty = true;
        rowData.IsMeritEdited = true;
        refreshRow(grdMeritGrid, row);
        objChangeFlag = true;
        return false;
    }
    rowData.dirty = true;
    rowData.IsMeritEdited = true;
    refreshRow(grdMeritGrid, row);
    objChangeFlag = true;
    return true;
}




function CallEmployeeNameClick(employeeNum, employeeName) {
    var employeeNum = employeeNum;
    var employeeName = employeeName;
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '../Compensation/_EmployeeInfo',
        cache: false,
        type: "POST",
        data: {
            __RequestVerificationToken: token,
            employeeNum: employeeNum
        },
        success: function (result) {
            $("#divEmployeeInfo").html(result);
            $("#divEmployeeInfo").modal('show');
            $("#divInfoTitle").text(employeeName);
            $("#divInfoTitle1").text("History of " + employeeName);
        }
    });
}


function CallGradeClick(rowData, row, index) {
    var isPromotionEligible = (rowData.IsPromotionEligible == true && rowData.IsLocked == false && rowData.CurrentAnnualSalaryLocalForCalc > 0) ? false : true;
    var newGradetitle = (rowData.NewTitle != null) ? rowData.NewTitle : "";
    var empJobnum = rowData.Empjobnum;
    $("#CommentPopup").data("kendoWindow").options.readOnly = isPromotionEligible;
    $("#CommentPopup").data("kendoWindow").options.rowData = rowData;
    $("#CommentPopup").data("kendoWindow").options.row = row;
    $("#CommentPopup").data("kendoWindow").options.index = index;


    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '../Compensation/_Promotion',
        cache: false,
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            newGradeTitle: newGradetitle, empJobNum: empJobnum
        },
        success: function (result) {
            $("#divPromotion").html(result);
            $("#divPromotion").modal('show');
            $("#divPromotion #divPromotionTitle").text("Enter promotion comment for " + rowData.EmployeeName);
            $("#promotionComment").attr("placeholder", "Write a review comments for " + rowData.EmployeeName)
        }
    });
}

function CallCommentClick(rowData, row) {
    var compensationType = ReporteesPageConstatns.compensationCommentType;
    $("#CommentPopup").data("kendoWindow").options.rowData = rowData;
    $("#CommentPopup").data("kendoWindow").options.row = row;
    $("#CommentPopup").data("kendoWindow").options.readOnly = (rowData.CurrentAnnualSalaryLocalForCalc > 0) ? false : true;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Comment/_Comment',
        cache: false,
        type: "Post",
        data: {
            __RequestVerificationToken:token, empJobNum: rowData.Empjobnum, commentType: compensationType
        },
        success: function (result) {
            isPopupEdited = false;
            $("#divComment").html(result);
            $("#divComment").modal('show');
            $("#divComment #myCmtModalLabel").text(rowData.EmployeeName);
            $("#comment").attr("placeholder", "Write a review comments for " + rowData.EmployeeName)
            setTimeout(function () {
                updateScrollDown("generalComments");
            }, 300)

        }
    });
}

function updateScrollDown(id) {
    var element = document.getElementById(id);
    element.scrollTop = element.scrollHeight;
}
function refreshGrid() {
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    grdMeritReportees.dataSource.read();
    BindBudgetData();
}

function refreshGridTreeChange() {
    getApprovalStatus();
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    grdMeritReportees.dataSource._data = [];
    var gridPage = grdMeritReportees.dataSource._pageSize;
    grdMeritReportees.dataSource.query({ page: pageno, pageSize: gridPage, filter: [], sort: [] });
    BindBudgetData();
}

function grdMeritReportees_sync() {
    refreshGrid();
    getCompCompletedCount();
}

var OpenCommentPopUp = (function (rowData, row, compensationType, empjobnum, change) {
  
    if (compensationType == "Merit")
        compensationType = ReporteesPageConstatns.meritmandateType;
    if (compensationType == "Adjustment")
        compensationType = ReporteesPageConstatns.adjustmentType;
    var comment = $("#CommentPopup").data("kendoWindow");
    var token = $('input[name="__RequestVerificationToken"]').val();
    comment.options.rowData = rowData;
    comment.options.row = row;
    $.ajax({
        url: '../Comment/_Comment',
        type: "Post",
        cache: false,
        data: { __RequestVerificationToken:token, empJobNum: rowData.Empjobnum, commentType: compensationType },
        success: function (result) {
            $("#divComment").html(result);
            $("#divComment").modal('show');
            $("#divComment #divCommentTitle").text("Enter comment for " + rowData.EmployeeName);
        }
    });
});

function CheckGuideLineExceed(newGuideline, incrPct) {
    incrPct = parseFloat((incrPct == null) ? 0 : incrPct);
    var lowerBound, upperBound;

    if (incrPct != NaN && incrPct != "" && newGuideline != null) {
        var valueSplit = newGuideline.split("-");
        if (valueSplit.length == 2) {
            lowerBound = newGuideline.split('-')[0].replace('%', '');
            upperBound = newGuideline.split('-')[1].replace('%', '');
        }
        else if (valueSplit.length == 1) {
            lowerBound = 0;
            upperBound = newGuideline.replace('%', '');
        }
        else {
            lowerBound = newGuideline.replace('%', '');
            upperBound = newGuideline.replace('%', '');
        }
        //lowerBound = eval(lowerBound) + eval(ReporteesPageConstatns.RangeExceedPCT);
        upperBound = eval(upperBound);// + eval(meritPageConstants.RangeExceedPCT);
        if ((eval(incrPct) < eval(lowerBound)) || (eval(incrPct) > eval(upperBound))) {
            return true;
        }
    }
    return false;

}



function setColor(rowIndex, controlName, color) {
    var tr = $("#ReporteeGrid .k-grid-content").find('tr');
    var control = $(tr[rowIndex]).find("[id$=" + controlName + "]")[0];
    //var control = $("[id$=" + controlName + "]")[rowIndex];
    if (control != null && control != undefined) {
        control.style.color = color;
    }
}

function setDisable(rowIndex, controlName, isReadOnly) {
    var tr = $("#ReporteeGrid .k-grid-content").find('tr');
    var control = $(tr[rowIndex]).find("[id$=" + controlName + "]")[0];
    //var control = $("[id$=" + controlName + "]")[rowIndex];
    if (control != null && control != undefined) {
        control.disabled = isReadOnly;
    }
}

function calculateHourly(value) {
    if (RuleConfiguration.HourlyRate == 0 || isNaN(eval(value)) || (eval(value) == undefined) || (eval(value) == 0))
        return 0;
    return (eval(value) / eval(RuleConfiguration.HourlyRate));
}

function applyKendoControlStyles() {
    $(".gridControl").each(function () {
        eval($(this).children("script").last().html());
    });
}

function ShowOutlierComments(rowData, row, compensationType, change) {
    if (onCommentPopupClose == 0) {
        OpenCommentPopUp(rowData, row, compensationType, rowData.Empjobnum, change);
    }
}

function enableDisableControls(readOnly) {
    $("#ReporteeGrid input").attr('disabled', readOnly);
}

function refreshRow(grid, row) {

    var dataItem = grid.dataItem(row);

    var rowChildren = $(row).children('td[role="gridcell"]');
    var locked = 0;
    var lockedColumns = [];
    for (var i = 0; i < grid.columns.length; i++) {
        if (grid.columns.length > i + locked) {
            var columnAll = grid.columns[i];
            if (columnAll.locked == true) {
                locked = locked + 1;
                lockedColumns.push(columnAll);
            }
            var column = grid.columns[i + locked];
            var template = column.template;
            var cell = rowChildren.eq(i);

            if (template !== undefined) {
                var kendoTemplate = kendo.template(template);

                // Render using template
                cell.html(kendoTemplate(dataItem));
            } else {
                var fieldValue = dataItem[column.field];

                var format = column.format;
                var values = column.values;

                if (values !== undefined && values != null) {
                    // use the text value mappings (for enums)
                    for (var j = 0; j < values.length; j++) {
                        var value = values[j];
                        if (value.value == fieldValue) {
                            cell.html(value.text);
                            break;
                        }
                    }
                } else if (format !== undefined) {
                    // use the format
                    cell.html(kendo.format(format, fieldValue));
                } else {
                    // Just dump the plain old value
                    cell.html(fieldValue);
                }
            }
        }
    }
    var row = row[0];
    if (row != undefined) {
        var index = row.rowIndex;
        if (index != undefined && index != null) {
            var gridId = (grid._cellId).replace('_active_cell', '');
            var frozenRowChildren = $($("#" + gridId + " .k-grid-content-locked").find('tr')[index]).children('td[role="gridcell"]');
            for (var i = 0; i < lockedColumns.length; i++) {
                var column = lockedColumns[i];
                var template = column.template;
                var cell = frozenRowChildren.eq(i);

                if (template !== undefined) {
                    var kendoTemplate = kendo.template(template);

                    // Render using template
                    cell.html(kendoTemplate(dataItem));
                } else {
                    var fieldValue = dataItem[column.field];

                    var format = column.format;
                    var values = column.values;

                    if (values !== undefined && values != null) {
                        // use the text value mappings (for enums)
                        for (var j = 0; j < values.length; j++) {
                            var value = values[j];
                            if (value.value == fieldValue) {
                                cell.html(value.text);
                                break;
                            }
                        }
                    } else if (format !== undefined) {
                        // use the format
                        cell.html(kendo.format(format, fieldValue));
                    } else {
                        // Just dump the plain old value
                        cell.html(fieldValue);
                    }
                }
            }
            enableDisableInputControls(dataItem, index)
        }

    }

    $("#progress_" + dataItem.MyHRID).tooltip('show');
    $("#progressUSD_" + dataItem.MyHRID).tooltip('show');
}

function enableDiasableMeritTextBox(index, status) {
    var tr = $("#ReporteeGrid .k-grid-content").find('tr');
    $(tr[index]).find('[id*=txtMeritPCT]').attr('disabled', status);
    setDisable(index, GridDisplay == "USD" ? "txtMeritAmtUSD" : "txtMeritAmtLocal", status);
}

function enableDiasableLumpSumTextBox(index, status) {
    var tr = $("#ReporteeGrid .k-grid-content").find('tr');
    $(tr[index]).find('[id*=txtLumpSumPct]').attr('disabled', status);
    setDisable(index, GridDisplay == "USD" ? "txtLumpSumAmtUSD" : "txtLumpSumAmtLocal", status);
}

function meritVisibility(dataItem, index) {
    var tr = $("#ReporteeGrid .k-grid-content").find('tr');
    //var kndRatingDrop = $("#ddlMeritPerfRating_" + dataItem.Empjobnum).data("kendoDropDownList");
    //if (kndRatingDrop != undefined)
    //    kndRatingDrop.enable(false);
    //else {
    //    var id = "#ddlMeritPerfRating_" + dataItem.Empjobnum;
    //    $(tr[index]).find(id).attr('disabled', true);
    //}
    $(tr[index]).find('[id*=txtMeritPCT]').attr('disabled', true);
    $(tr[index]).find('[id*=txtLumpSumPct]').attr('disabled', true);
    setDisable(index, GridDisplay == "USD" ? "txtMeritAmtUSD" : "txtMeritAmtLocal", true);
    setDisable(index, GridDisplay == "USD" ? "txtLumpSumAmtUSD" : "txtLumpSumAmtLocal", true);
}

function adjustmentVisibility(dataItem, index) {
    var tr = $("#ReporteeGrid .k-grid-content").find('tr');
    $(tr[index]).find('[id*=txtAdjustmentPct]').attr('disabled', true);
    setDisable(index, GridDisplay == "USD" ? "txtAdjustmentAmtUSD" : "txtAdjustmentAmtLocal", true);
}

function promotionVisibility(dataItem, index) {
    setDisable(index, GridDisplay == "USD" ? "txtPromotionAmtUSD" : "txtPromotionAmtLocal", true);
    setDisable(index, GridDisplay == "USD" ? "txtPromotionPctUSD" : "txtPromotionPctLocal", true);
}

function promotionControlsBeforeNewTitle(dataItem, index) {
    if (dataItem.NewTitle == undefined || dataItem.NewTitle == null || dataItem.NewTitle == undefined || dataItem.NewTitle == null) {
        setDisable(index, GridDisplay == "USD" ? "txtPromotionAmtUSD" : "txtPromotionAmtLocal", true);
        setDisable(index, GridDisplay == "USD" ? "txtPromotionPctUSD" : "txtPromotionPctLocal", true);
    }
}

function HideSubmitApproveCheckBoxes(rowData) {
    //if (rowData.MeritPCT == null && rowData.LumpSumPct == null) {
    //    rowData.IsSubmit = false;
    //    rowData.IsApprove = false;
    //    rowData.SubmitIsChecked = false;
    //    rowData.ApprovalIsChecked = false;
    //}
}
function showApproveReopenButtonVisiblity(rowData) {

    if ((((rowData.WorkFlowStatus == "ActionRequired" || rowData.WorkFlowStatus == "Reopen") && approveVisiblity == false && rowData.IsFirstLevelManager == 0) || (rowData.WorkFlowStatus != "InProgress" && rowData.IsSuperAdmin == true && approveVisiblity == false))) {
        approveVisiblity = true;
    }
    if ((rowData.WorkFlowStatus == "InProgress" || rowData.WorkFlowStatus == "Reopen") && submitVisiblity == false) {
        submitVisiblity = true;
    }

    approveReopenButtonVisiblity(approveVisiblity, rowData.approvalCount, rowData.reopenCount);

    submitButtonVisiblity(submitVisiblity, rowData.submitCount);
}
function approveReopenButtonVisiblity(buttonVisiblity, approveCount, ReopenCount) {
    if (buttonVisiblity == true) {
        if (approveCount > 0)
            $("#btnApprove").show();
        if (ReopenCount > 0)
            $("#Reject").show();
    }
    else {
        $("#btnApprove").hide();
        $("#Reject").hide();
    }
}
function submitButtonVisiblity(buttonVisiblity, submitCount) {
    if (buttonVisiblity == true && submitCount > 0) {
        $("#btnSubmit").show();

    }
    else {
        $("#btnSubmit").hide();

    }
}

function onMeritReporteesBound(e) {
    approveVisiblity = false;
    submitVisiblity = false;
    if ($("#ReporteeGrid").data("kendoGrid").dataSource._data.length > 0) {
        var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
        var rows = grdMeritReportees.tbody.children('tr');
        var data = grdMeritReportees.dataSource.view();
        $.each(data, function (index, rowData) {
            enableDisableInputControls(rowData, index);
            checkMeritGuideLine(rowData, index);
            checkAdjustmentGuideLine(rowData, index);
            checkPromotionGuideLine(rowData, index);
            checkLumpSumGuideLine(rowData, index);
            showApproveReopenButtonVisiblity(rowData);
        });
        $.each(grdMeritReportees.columns, function () {
            var index = grdMeritReportees.columns.indexOf(this);

            if ((this.field && this.field.match("USD") == "USD") || (this.template && this.template.match("USD") == "USD")) {

                if (GridDisplay == "USD") {
                    $("#ReporteeGrid table th:nth-child(" + index + ")").show();
                    $("#ReporteeGrid table td:nth-child(" + index + ")").show();
                }
                else {
                    $("#ReporteeGrid table th:nth-child(" + index + ")").hide();
                    $("#ReporteeGrid table td:nth-child(" + index + ")").hide();
                }

            } else if ((this.field && this.field.match("Local") == "Local") || (this.template && this.template.match("Local") == "Local")) {

                if (GridDisplay == "Local") {
                    $("#ReporteeGrid table th:nth-child(" + index + ")").show();
                    $("#ReporteeGrid table td:nth-child(" + index + ")").show();
                }
                else {
                    $("#ReporteeGrid table th:nth-child(" + index + ")").hide();
                    $("#ReporteeGrid table td:nth-child(" + index + ")").hide();
                }
            }
        });
        applyKendoControlStyles();
        
        $('[id*=progress_]').tooltip({ trigger: 'manual' }).tooltip('show');
        $('.grid-popover').tooltip({ trigger: 'manual' }).tooltip('show');
    }
    else if ($("#ReporteeGrid").data("kendoGrid").dataSource._data.length == 0) {
        var CompensationGrid = $("#ReporteeGrid").data("kendoGrid");
        if (CompensationGrid.dataSource._view.length == 0) {
            var colCount = $("#ReporteeGrid").find('th').length;
            $("#ReporteeGrid .k-grid-content").find('table').css('width', '');
            $("#ReporteeGrid .k-grid-content").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center;background-color:#6686C4;color:white !important;">No Results Found!</td></tr>');
        }
    }

}

function enableDisableInputControls(rowData, index) {
    var tr = $("#ReporteeGrid .k-grid-content").find('tr');
    $(tr[index]).find('input').attr('disabled', (rowData.IsLocked || (rowData.CurrentAnnualSalaryLocalForCalc <= 0)));
    if (!rowData.IsLocked && (rowData.CurrentAnnualSalaryLocalForCalc != 0)) {
        if (RuleConfiguration.EitherMeritOrlumpsum) {
            enableDiasableMeritTextBox(index, (rowData.LumpSumPct != null));
            enableDiasableLumpSumTextBox(index, (rowData.MeritPCT != null));
        }
        if (!rowData.IsMeritEligible)
            meritVisibility(rowData, index);
        if (!rowData.IsAdjustmentEligible)
            adjustmentVisibility(rowData, index);
        if (!rowData.IsPromotionEligible)
            promotionVisibility(rowData, index);
        else
            promotionControlsBeforeNewTitle(rowData, index);
    }
}


function requestPage(event) {
    if (!showSaveWarning(event, "objChangeFlag")) {
        event.preventDefault();
        return false;
    }
}

function formatBudgetCurrency(value, format, culture) {
    if (value == null) return '';
    if (isNaN(value)) return '';
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var symbol = kendo.cultures[culture];
    symbol = (symbol != undefined) ? symbol.numberFormat.currency.symbol : kendo.cultures['en-US'].numberFormat.currency.symbol;
    var currenyCode = (selectedData != undefined) ? selectedData.CurrencyCode : "USD";
    if ((value.length == 1) || format == undefined)
        return value.toString();
    else if (culture == undefined) {
        var val = kendo.toString(value, format);
        val = val.indexOf('-') > -1 ? '(' + val.replace('-', '') + ')' : val;
        if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay)
            val = val.replace(symbol, currenyCode + ' ');
        return val;
    }
    else {
        var kndValue = kendo.toString(value, format, culture);
        kndValue = kndValue.indexOf('-') > -1 ? '(' + kndValue.replace('-', '') + ')' : kndValue;
        //if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay)
        kndValue = kndValue.replace(symbol, currenyCode + ' ');
        return kndValue;
    }
}

function formatGridCurrency(value, keyValue, subcategory, culture) {
    if (value == null || isNaN(value)) return '';
    value = eval(value);
    var symbol = kendo.cultures[culture];
    symbol = (symbol != undefined) ? symbol.numberFormat.currency.symbol : kendo.cultures['en-US'].numberFormat.currency.symbol;

    if (subcategory == 'percentage') {
        var cultureFormat = getCultureFormat(keyValue, subcategory);
        var result = formatValue(value / 100, cultureFormat, culture);
        result = result.replace(' ', '');
        result = result.indexOf('-') > -1 ? '(' + result.replace('-', '') + ')' : result;
        if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay && GridDisplay == "Local")
            result = result.replace(symbol, '');
        return result;
    }
    else {

        var cultureFormat = getCultureFormat(keyValue, subcategory);
        var oldlength = parseInt(cultureFormat[1]);
        var stringValue = value.toString();
        if (numberOfDecimalPlaces(value) != 0) {
            var length = stringValue.split('.')[1].length;
            if (length < oldlength)
                cultureFormat = "c" + length;
        }
        var result = formatValue(value, cultureFormat, culture);
        result = result.replace(' ', '');
        result = result.indexOf('-') > -1 ? '(' + result.replace('-', '') + ')' : result;
        if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay && GridDisplay == "Local")
            result = result.replace(symbol, '');
        return result;
    }
}

function numberOfDecimalPlaces(number) {
    let match = (number + "").match(/(?:\.(\d+))?$/);
    if (!match || !match[1]) {
        return 0;
    }

    return match[1].length;
}


function check_float(e, control) {
    var charCode = "";
    if (e.which == undefined)
        charCode = e.keyCode;
    else
        charCode = e.which;
    if (control.readOnly == true) return;

    else if (!((
        (charCode <= 57)) || charCode == 0 || (charCode == 13) || (charCode == 8)
    || (charCode == 37) || (charCode == 16))) {
        e.preventDefault();
        return false;
    }
    return true;
}

function checkPercentage(value) {
    if (!value.match(/^([0-9]{0,1})*\.?[0-9]+%?$/) && value.length > 0) {
        showAlert("Only numbers are allowed!");
        return false;
    }
    return true;
}

function roundingRule(dataValue, keyValue, subcategory) {
    if (subcategory == "percentage") {
        if (keyValue == "merit") {
            var type = RuleConfiguration.GeneralConfigurationRoundingMeritPct.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalMeritPct;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "adjustment") {
            var type = RuleConfiguration.GeneralConfigurationRoundingAdjustmentPct.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalAdjustmentPct;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "lumpsum") {
            var type = RuleConfiguration.GeneralConfigurationRoundingLumpSumPct.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalLumpSumPct;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }

        else if (keyValue == "promotion") {
            var type = RuleConfiguration.GeneralConfigurationRoundingPromotionPct.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalPromotionPct;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "bonus") {
            var type = RuleConfiguration.GeneralConfigurationRoundingBonusPct.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalBonusPct;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "general") {
            var type = "absolute"
            var configValue = "0.01"
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
    }
    else if (subcategory == "annual") {
        if (keyValue == "merit") {
            var type = RuleConfiguration.GeneralConfigurationRoundingMeritAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalMeritAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "adjustment") {
            var type = RuleConfiguration.GeneralConfigurationRoundingAdjustmentAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalAdjustmentAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }

        else if (keyValue == "promotion") {
            var type = RuleConfiguration.GeneralConfigurationRoundingPromotionAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalPromotionAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "lumpsum") {
            var type = RuleConfiguration.GeneralConfigurationRoundingLumpSumAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalLumpSumAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "bonus") {
            var type = RuleConfiguration.GeneralConfigurationRoundingBonusAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalBonusAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "newsalary") {
            var type = RuleConfiguration.GeneralConfigurationRoundingNewSalaryAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalNewSalaryAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "currentsalary") {
            var type = RuleConfiguration.GeneralConfigurationRoundingCurrentSalaryAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalCurrentSalaryAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "finalbonus") {
            var type = RuleConfiguration.GeneralConfigurationRoundingFinalBonusAnnual.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalFinalBonusAnnual;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "general") {
            var type = "absolute"
            var configValue = "0.001"
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "tccgeneral") {
            var type = "absolute"
            var configValue = "1"
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
    }
    else if (subcategory == "hourly") {
        if (keyValue == "merit") {
            var type = RuleConfiguration.GeneralConfigurationRoundingMeritHourly.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalMeritHourly;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "adjustment") {
            var type = RuleConfiguration.GeneralConfigurationRoundingAdjustmentHourly.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalAdjustmentHourly;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "promotion") {
            var type = RuleConfiguration.GeneralConfigurationRoundingPromotionHourly.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalPromotionHourly;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }

        else if (keyValue == "lumpsum") {
            var type = RuleConfiguration.GeneralConfigurationRoundingLumpSumHourly.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalLumpSumHourly;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "bonus") {
            var type = RuleConfiguration.GeneralConfigurationRoundingBonusHourly.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalBonusHourly;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "newsalary") {
            var type = RuleConfiguration.GeneralConfigurationRoundingNewSalaryHourly.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalNewSalaryHourly;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "currentsalary") {
            var type = RuleConfiguration.GeneralConfigurationRoundingCurrentSalaryHourly.toLowerCase();
            var configValue = RuleConfiguration.GeneralConfigurationDecimalCurrentSalaryHourly;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
        else if (keyValue == "general") {
            var type = "absolute"
            var configValue = "0.01"
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
        }
    }
    else if (subcategory == "comparatio") {
        var type = RuleConfiguration.GeneralConfigurationRoundingCompaRatioPct.toLowerCase();
        var configValue = RuleConfiguration.GeneralConfigurationDecimalCompaRatioPct;
        var roundedValue = roundingNumbers(type, dataValue, configValue);
        return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN) ? 0 : roundedValue;
    }
}

function roundingNumbers(type, dataValue, configValue) {
  
    var decimalPlaces = getdecimalPlaces(configValue);
    if (configValue == 50) {
        if (type == "absolute") {
            var s = 50 * Math.round(dataValue / 50);
            return s;
        }
        else if (type == "roundup") {
            var s = 50 * Math.ceil(dataValue / 50);
            return s;
        }
        else if (type == "rounddown") {
            var s = 50 * Math.floor(dataValue / 50);
            return s;
        }
        else if (type == "nearest") {
            var s = 50 * Math.round(dataValue / 50);
            return s;
        }
    }
    if (decimalPlaces > 0) {
        if (type == "absolute") {
            return Number(Math.round(dataValue + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
        }
        else if (type == "roundup") {
          
            return Number(Math.ceil(dataValue + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
        }
        else if (type == "rounddown") {
            return Number(Math.floor(dataValue + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
        }
        else if (type == "nearest") {
            return Number(dataValue.toFixed(decimalPlaces));
        }
    }
    else {
        decimalPlaces = Math.abs(decimalPlaces);
        if (type == "absolute") {
            return Number(Math.round(dataValue + 'e-' + decimalPlaces) + 'e' + decimalPlaces);
        }
        else if (type == "roundup") {
            return Number(Math.ceil(dataValue + 'e-' + decimalPlaces) + 'e' + decimalPlaces);
        }
        else if (type == "rounddown") {
            return Number(Math.floor(dataValue + 'e-' + decimalPlaces) + 'e' + decimalPlaces);
        }
        else if (type == "nearest") {
            return Number(Math.round(dataValue / configValue) * configValue);
        }
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

function allowDecimalNumberOnlyInputAmt(e, decimalSeparator) {
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
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}

function decimalPlaces(configValue) {
    var regexp = ('' + configValue).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
    if (!regexp) { return 0; }

    return Math.max(0,
        // Number of digits right of decimal point.
       (regexp[1] ? regexp[1].length : 0)
       // Adjust for scientific notation.
       - (regexp[2] ? +regexp[2] : 0));
}

function getdecimalPlaces(configValue) {
    var noOfDecimalPlaces = decimalPlaces(configValue);
    if (noOfDecimalPlaces <= 0) {
        var configValueLength = configValue.toString().replace(/[^0]/g, "").length;
        noOfDecimalPlaces = -Math.abs(configValueLength);
    }
    return noOfDecimalPlaces;
}

function getnoOfDecimalPlaces(configValue) {
    var noOfDecimalPlaces = decimalPlaces(configValue);
    return noOfDecimalPlaces;
}

function getCultureFormat(keyValue, subcategory) {
    if (subcategory == "percentage") {
        if (keyValue == "merit") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalMeritPct;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('p' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "adjustment") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalAdjustmentPct;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('p' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "promotion") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalPromotionPct;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('p' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "lumpsum") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalLumpSumPct;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('p' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "bonus") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalBonusPct;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('p' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "general") {
            return ('p2').toString();
        }
    }
    else if (subcategory == "annual") {
        if (keyValue == "merit") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalMeritAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "adjustment") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalAdjustmentAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }

        else if (keyValue == "promotion") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalPromotionAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "lumpsum") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalLumpSumAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "bonus") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalBonusAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "newsalary") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalNewSalaryAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "currentsalary") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalCurrentSalaryAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "finalbonus") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalFinalBonusAnnual;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "general") {
            return ('c0').toString();
        }
        else if (keyValue == "tccgeneral") {
            return ('c0').toString();
        }
    }
    else if (subcategory == "hourlyrate") {
        if (keyValue == "merit") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalMeritHourly;
            var noOfDecimalPlaces = 5;// getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "adjustment") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalAdjustmentHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }

        else if (keyValue == "promotion") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalPromotionHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "lumpsum") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalLumpSumHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "bonus") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalBonusHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "newsalary") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalNewSalaryHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "currentsalary") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalCurrentSalaryHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "general") {
            return ('c0').toString();
        }
    }
    else if (subcategory == "hourly") {
        if (keyValue == "merit") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalMeritHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "adjustment") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalAdjustmentHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }

        else if (keyValue == "promotion") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalPromotionHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "lumpsum") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalLumpSumHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "bonus") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalBonusHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "newsalary") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalNewSalaryHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "currentsalary") {
            var configValue = RuleConfiguration.GeneralConfigurationDecimalCurrentSalaryHourly;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
        else if (keyValue == "general") {
            return ('c0').toString();
        }
    }
    else if (subcategory == "comparatio") {
        var configValue = RuleConfiguration.GeneralConfigurationDecimalCompaRatioPct;
        var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
        return ('n' + noOfDecimalPlaces).toString();
    }
}

function CharacterCount(id) {

    var html = id.value();
    var text = $("<div>").html(html).text();
    return text.length;
    $("#message").html("");
}

function KeyDown(e) {

    var keycode = e.charCode || e.keyCode;
    var KeyCodearray = [8, 9, 35, 36, 37, 38, 39, 40, 46];
    if (KeyCodearray.indexOf(keycode) == -1) {
        var textvaluelength = CharacterCount(this);
        var maxlength = this.textarea[0].maxLength;
        if (textvaluelength + 1 > maxlength) {

            e.preventDefault();

        }
    }

}

function KeyUp(e) {

    var textvaluelength = CharacterCount(this);
    var countID = "#" + this.textarea[0].getAttribute('countid');
    $(countID)[0].value = textvaluelength;
}

function Paste(e) {
    var maxlength = this.textarea[0].maxLength;
    var id = this
    var editor = $(e.sender.element).data("kendoEditor");
    var countID = "#" + this.textarea[0].getAttribute('countid');
    e.html = $("<div></div>").html(e.html).text();
    var editorTextLength = $("<div></div>").html(editor.value()).text().length;
    var pasteTextLength = e.html.length;
    if ((editorTextLength + pasteTextLength) > maxlength) {
        e.html = "";
        alert("Exceeds characters Limit");
        $(countID)[0].value = CharacterCount(id);
    }
}

function Select(e) {
    this.focus();
}

function Change(e) {
    inputChanged = true;
}


function GetReporteesCompParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
   
    return {
        __RequestVerificationToken: token,
        selectedManagerNum: $("#hndSelectedManagerNum").val(),
        compMenuType: $("#hndMenuType").val(),
        isRollup: $("#hndIsRollup").val() == 1
    }
}

function PutReporteesCompParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();

    return {
        __RequestVerificationToken: token,
        
    }
}

function enableDisableCheckBox(id, value) {
    var checkboxLength = $("[id$=" + id + "]").length;
    for (var i = 0; i < checkboxLength; i++) {
        $("[id$=" + id + "]")[i].disabled = value;
    }
}

function newCompProgressBar(newcomp, newSal, minVal, maxVal, NewHourlyRateLocal, EmployeeStatus) {
    if (EmployeeStatus == 'annual') {
        var rng = (maxVal - minVal) / 100;
        var wid = (newSal - minVal) / rng;
        if (wid < 0 || newcomp <= 0)
            return 'none';
        else if (wid <= 100)
            return '#1abb9c';
        else
            return '#c7342a';
    }
    else {
        var rng = (maxVal - minVal) / 100;
        var wid = (NewHourlyRateLocal - minVal) / rng;
        if (wid < 0 || newcomp <= 0)
            return 'none';
        else if (wid <= 100)
            return '#1abb9c';
        else
            return '#c7342a';
    }
}

function newCompPrgWidth(newcomp, newSal, minVal, maxVal, NewHourlyRateLocal, EmployeeStatus) {
    if (EmployeeStatus == 'annual') {
        var rng = (maxVal - minVal) / 100;
        var wid = (newSal - minVal) / rng;
        if (newcomp <= 0)
            return 0;
        return wid;
    }
    else {
        var rng = (maxVal - minVal) / 100;
        var wid = (NewHourlyRateLocal - minVal) / rng;
        if (newcomp <= 0)
            return 0;
        return wid;
    }
}

var saveWarning = (function () {
    if (objChangeFlag == true && confirm(meritPageConstants.warningMessage)) {
        return false;
    }
    ClearChangeFlag();
    return true;
});
var BindBudgetData = (function () { 
    var a = ddlCurrencyCulture;
    var b = ddlCurrencyCodeNum;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Compensation/GetBudgetSpentData',
        type: "post",
        cache: false,
        data: {
            __RequestVerificationToken: token,
            employeeNum: $("#hndSelectedManagerNum").val(), compMenuType: $("#hndMenuType").val(), currencyCulture: ddlCurrencyCulture, currencyCodeNum: ddlCurrencyCodeNum, isRollup: ($("#hndIsRollup").val() == 1), isSelectedRollup: ($("#hndIsSelectedRollup").val() == 1)
        },
        success: function (result) {
            if (!result[0].isInDirects) {
                if (meritPageConstants.Multicurrency == "False")
                    $("#inDirectBudgetSection").hide();
                else
                    $("#budgetCheckBox").hide();
                //$("#budgetCheckBox").hide();
            }
            if (result[0] != null) {
                var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
                var cultureCode = (selectedData != undefined) ? selectedData.CultureCode : preferredCulture;
                $("#meritBudget").text(formatBudgetCurrency(result[0].MeritBudget, 'c0', cultureCode));
                var a = Math.round(result[0].BudgetPct.toFixed(0));
                BudgetPercentage = a;
                if (isNaN(a))
                    $("#budgetPct").text("0%");
                else
                    $("#budgetPct").text(a + "%");
                document.getElementById("budgetDonughtClass");
                var oldBudgetClass = document.getElementById("budgetDonughtClass").className;
                var newBudgetClass = "c100 small p100";
                $("#budgetDonughtClass").removeClass(oldBudgetClass);
                $("#budgetDonughtClass").addClass(newBudgetClass);
                var oldBalanceClass = document.getElementById("balanceDonughtClass").className;
                document.getElementById("hdnPromotionSpent").value = result[0].PromotionSpent;
                document.getElementById("hdnMeritSpent").value = result[0].MeritSpent;
                document.getElementById("hdnLumpSumSpent").value = result[0].LumpSumSpent;
                document.getElementById("hdnAdjustmentSpent").value = result[0].AdjustmentSpent;
                document.getElementById("hdnMeritBudget").value = result[0].MeritBudget;
                document.getElementById("hdnBaseSalary").value = result[0].BaseSalary;
                $("#meritSpentAmt").text(formatBudgetCurrency(result[0].MeritSpent, 'c0', cultureCode));
                $("#promotionSpentAmt").text(formatBudgetCurrency(result[0].PromotionSpent, 'c0', cultureCode));
                $("#lumpsumSpentAmt").text(formatBudgetCurrency(result[0].LumpSumSpent, 'c0', cultureCode));
                $("#totalMeritSpentAmt").text(formatBudgetCurrency(result[0].MeritSpent, 'c0', cultureCode));
                $("#totalPromotionSpentAmt").text(formatBudgetCurrency(result[0].PromotionSpent, 'c0', cultureCode));
                $("#totalLumpsumSpentAmt").text(formatBudgetCurrency(result[0].LumpSumSpent, 'c0', cultureCode));
                $("#totalAdjustmentSpentAmt").text(formatBudgetCurrency(result[0].AdjustmentSpent, 'c0', cultureCode));
                $("#beforeIncreaseMin").text(result[0].BeforeIncreaseMin);
                $("#beforeIncreaseMid").text(result[0].BeforeIncreaseMid);
                $("#beforeIncreaseMax").text(result[0].BeforeIncreaseMax);
                $("#afterIncreaseMin").text(result[0].AfterIncreaseMin);
                $("#afterIncreaseMid").text(result[0].AfterIncreaseMid);
                $("#afterIncreaseMax").text(result[0].AfterIncreaseMax);
                totalCurrentSalary = result[0].TotalCurrentSalary;
                totalNewSalary = result[0].TotalNewSalary;
                $("#currentSalary").text(formatBudgetCurrency(result[0].TotalCurrentSalary, 'c0', cultureCode));
                $("#newSalary").text(formatBudgetCurrency(result[0].TotalNewSalary, 'c0', cultureCode));
                var a = (((result[0].TotalNewSalary - result[0].TotalCurrentSalary) / (result[0].TotalCurrentSalary)) * 100);
                if (isNaN(a)) {
                    $("#increasePct").text((0).toFixed(2) + "%");
                    $("#increasePct").css("color", "#646d78");
                }
                else {
                    if (a < 0) {
                        $("#increasePct").text("(" + (((result[0].TotalNewSalary - result[0].TotalCurrentSalary) / (result[0].TotalCurrentSalary)) * 100).toFixed(2) + "%)");
                        $("#increasePct").css("color", "red");
                    }
                    else {
                        $("#increasePct").text((((result[0].TotalNewSalary - result[0].TotalCurrentSalary) / (result[0].TotalCurrentSalary)) * 100).toFixed(2) + "%");

                        $("#increasePct").css("color", "black");
                    }
                }

                $("#meritBalance").text(formatBudgetCurrency(result[0].Balance, 'c0', cultureCode));
                $("#totalSpentAmt").text(formatBudgetCurrency(result[0].MeritBudget - result[0].Balance, 'c0', cultureCode));
                var balance = Math.round(result[0].Balance);
                if (balance < 0) {
                    $("#meritBalance").removeClass("counter2");
                    $("#meritBalance").addClass("counter3");
                    $("#balancePct").removeClass("counter2");
                    $("#balancePct").addClass("counter3");
                    var a = Math.abs((((result[0].Balance) / result[0].BaseSalary) * 100).toFixed(0));
                    var balancepct = "(";
                    if (isNaN(a) || a.toString() == "Infinity")
                        $("#balancePct").text("0%");
                    else
                        $("#balancePct").text(balancepct.concat(Math.abs((((result[0].Balance) / result[0].BaseSalary) * 100).toFixed(0)) + "%)"));
                    var budgetPct = Math.round(result[0].BudgetPct.toFixed(0));
                    var balancepct = Math.round((((result[0].Balance) / result[0].BaseSalary) * 100).toFixed(0));
                    if (budgetPct == balancepct) {
                        var newBalanceClass = "c100 small p" + 100;
                    } else if (balancepct < 0)
                    {
                        var newBalanceClass = "c100 small x100 p100";
                    }
                    else
                        var newBalanceClass = "c100 small p" + a;
                    $("#balanceDonughtClass").removeClass(oldBalanceClass);
                    $("#balanceDonughtClass").addClass(newBalanceClass);
                    $("#balanceAmt").text(formatBudgetCurrency(result[0].Balance, 'c0', cultureCode));
                    if (result[0].Balance < 0) {
                        $("#balanceAmt").css("background", "red");
                        $("#balanceAmt").css("color", "white");
                        $("#balanceDonughtClass").css("border", "#1abc9c !important");
                    }
                    else {
                        $("#balanceAmt").css("background", "white");
                        $("#balanceAmt").css("color", "#6E6259");
                    }
                }
                else {
                    $("#meritBalance").addClass("counter2");
                    $("#meritBalance").removeClass("counter3");
                    $("#balancePct").addClass("counter2");
                    $("#balancePct").removeClass("counter3");
                    var a = Math.round((((result[0].Balance) / result[0].BaseSalary) * 100).toFixed(0));
                    if (isNaN(a) || a.toString() == "Infinity")
                        $("#balancePct").text("0%");
                    else
                        $("#balancePct").text(a + "%");
                    $("#balancePct").css("color", "");
                    var budgetPct = Math.round(result[0].BudgetPct.toFixed(0));
                    var balancepct = Math.round((((result[0].Balance) / result[0].BaseSalary) * 100).toFixed(0));
                    var balanceChartPct = Math.round((balancepct / budgetPct) * 100);
                    if (budgetPct == balancepct) {
                        var newBalanceClass = "c100 small p" + 100;
                    } else if (balancepct < 0) {
                        var newBalanceClass = "c100 small x100 p100";
                    }
                    else
                        var newBalanceClass = "c100 small p" + balanceChartPct;                    
                        // var newBalanceClass = "c100 small p" + a;
                    $("#balanceDonughtClass").removeClass(oldBalanceClass);
                    $("#balanceDonughtClass").addClass(newBalanceClass);
                    $("#balanceAmt").text(formatBudgetCurrency(result[0].Balance, 'c0', cultureCode));
                    if (result[0].Balance < 0) {
                        $("#balanceAmt").css("background", "red");
                        $("#balanceAmt").css("color", "white");
                    }
                    else {
                        $("#balanceAmt").css("background", "white");
                        $("#balanceAmt").css("color", "#6E6259");
                    }
                }
                createChart(result[0].Balance, result[0].MeritSpent, result[0].LumpSumSpent, result[0].AdjustmentSpent, result[0].PromotionSpent, cultureCode);

            }

        }
    });

    function formatBudgetCurrency(value, format, culture) {
        if (value == null) return '';
        if (isNaN(value)) return '';
        var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
        var symbol = kendo.cultures[culture];
        symbol = (symbol != undefined) ? symbol.numberFormat.currency.symbol : kendo.cultures['en-US'].numberFormat.currency.symbol;
        var currenyCode = (selectedData != undefined) ? selectedData.CurrencyCode : "USD";
        if ((value.length == 1) || format == undefined)
            return value.toString();
        else if (culture == undefined) {
            var val = kendo.toString(value, format);
            val = val.indexOf('-') > -1 ? '(' + val.replace('-', '') + ')' : val;
            if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay)
                val = val.replace(symbol, currenyCode + ' ');
            return val;
        }
        else {
            var kndValue = kendo.toString(value, format, culture);
            kndValue = kndValue.indexOf('-') > -1 ? '(' + kndValue.replace('-', '') + ')' : kndValue;
            if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay)
                kndValue = kndValue.replace(symbol, currenyCode + ' ');
            return kndValue;
        }
    }
});


//#region MyRegion
function createChart(balance, meritSpent, lumpSumSpent, adjustemntValue, promotionSpent, ddlCurrencyCulture) {
    lumpSumSpent = lumpSumSpent;//(RuleConfiguration.MeritValuesReCalculate == false && RuleConfiguration.LumpSumRuleLumpSumType == "AutoCalc") ? 0:lumpSumSpent;
    var budgetSpent = (RuleConfiguration.FeatureConfigurationMerit ? parseFloat(meritSpent) : 0) + (RuleConfiguration.FeatureConfigurationLumpSum ? parseFloat(lumpSumSpent) : 0) + (RuleConfiguration.FeatureConfigurationPromotion ? parseFloat(promotionSpent) : 0);
    var totalSpent = (RuleConfiguration.FeatureConfigurationMerit ? parseFloat(meritSpent) : 0) + (RuleConfiguration.FeatureConfigurationLumpSum ? parseFloat(lumpSumSpent) : 0) + (RuleConfiguration.FeatureConfigurationPromotion ? parseFloat(promotionSpent) : 0);
    totalSpent = totalSpent > 0 ? totalSpent : 200;
    var budget = parseFloat(balance) + parseFloat(totalSpent);
    budget = budget == 0 ? 1 : budget;
    var budgetSpentPct = (parseFloat(budgetSpent) / parseFloat(budget)) * 100;
    budgetSpentPct = (budgetSpentPct != Infinity) ? budgetSpentPct : 0;
    var totalSpentPct = (parseFloat(totalSpent) / parseFloat(budget)) * 100;
    totalSpentPct = (totalSpentPct != Infinity) ? totalSpentPct : 0;
    totalSpentPct = (parseFloat(totalSpent) > parseFloat(budget)) ? totalSpentPct : 100;
    var a = ((meritSpent / budget) * 100).toFixed(2);
    var b = ((promotionSpent / budget) * 100).toFixed(2);
    var c = ((lumpSumSpent / budget) * 100).toFixed(2);
    var d = parseFloat(a) + parseFloat(b);
    var e = parseFloat(a) + parseFloat(b) + parseFloat(c);

    var first = (a / e) * 100;
    var second = (d / e) * 100;    
    $("#gauge").kendoRadialGauge({
        pointer: {
            value: 65,
            color: "transparent"
        },
        gaugearea: {
            margin: 20,
        },
        scale: {
            minorUnit: 5,
            startAngle: -30,
            endAngle: 210,
            max: 100,
            labels: {
                position: "inside",
                visible: false
            },
            ranges: [
                {
                    from: 0,
                    to: first,
                    color: "#F79646"
                }, {
                    from: first,
                    to: second,
                    color: "#4BACC6"
                }, {
                    from: second,
                    to: 100,
                    color: "#b65708"
                }
            ]
        }
    });

}


function onChange() {
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    if (ddlcurrencyDropOldvalue != selectedData.CurrencyNum) {

        ddlcurrencyDropOldvalue = selectedData.CurrencyNum;
        ddlCurrencyCulture = selectedData.CultureCode;
        ddlExchangeRate = selectedData.ExchangeRate;
        ddlCurrencyCodeNum = selectedData.CurrencyNum;
    }
    BindBudgetData();
}


function roundBudget(value) {
    return Number(Math.round(value + 'e2') + 'e-2')
}

function roundToWholeNumber(value) {
    return Number(Math.round(value));
}
function formatBudgetCurrency(value, format, culture) {
    if (value == null) return '';
    if (isNaN(value)) return '';
    var selectedData = $("#ddlLocalCurrencies").data("kendoDropDownList").dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());
    var symbol = kendo.cultures[culture];
    symbol = (symbol != undefined) ? symbol.numberFormat.currency.symbol : kendo.cultures['en-US'].numberFormat.currency.symbol;
    var currenyCode = (selectedData != undefined) ? selectedData.CurrencyCode : "USD";
    if ((value.length == 1) || format == undefined)
        return value.toString();
    else if (culture == undefined) {
        var val = kendo.toString(value, format);
        val = val.indexOf('-') > -1 ? '(' + val.replace('-', '') + ')' : val;
        if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay)
            val = val.replace(symbol, currenyCode + ' ');
        return val;
    }
    else {
        var kndValue = kendo.toString(value, format, culture);
        kndValue = kndValue.indexOf('-') > -1 ? '(' + kndValue.replace('-', '') + ')' : kndValue;
        if (RuleConfiguration.FeatureConfigurationCurrencyCodeDisplay)
            kndValue = kndValue.replace(symbol, currenyCode + ' ');
        return kndValue;
    }
}


function refreshBudgetGrid() {
    var grdBudget = $("#grdDirectBudget").data("kendoGrid");
    grdBudget.dataSource.read();
}
function dataBound() {
    var currencyDrop = $("#ddlLocalCurrencies").data("kendoDropDownList");
    if (currencyDrop.dataSource._data.length > 0) {
        if (DefaultCurrencyNum != '0')
            currencyDrop.value(DefaultCurrencyNum);
        else
            currencyDrop.value(0);

        ddlcurrencyDropOldvalue = currencyDrop.value();
        var selectedData = currencyDrop.dataItem($("#ddlLocalCurrencies").data("kendoDropDownList").select());

        if (selectedData != undefined) {
            ddlCurrencyCodeNum = selectedData.CurrencyNum;
            ddlCurrencyCulture = (selectedData.CultureCode != undefined) ? selectedData.CultureCode : 'en-US';
            ddlExchangeRate = (selectedData.ExchangeRate != undefined) ? selectedData.ExchangeRate : 1;
        }

        BindBudgetData();
    }
    return false;
}


$('#budgetCollapse label[class="switch"]').click(function (e) {
    if (objChangeFlag != true) {
        e.stopPropagation();
    }
});


$(function () {
    $("#budgetCollapse").accordion({
        collapsible: true,
        heightStyle: "content",
        active: false,
        beforeActivate: function (event, ui) {
            var fromIcon = $(event.originalEvent.target).is('.ui-accordion-header > .ui-icon');
            return fromIcon;
        },
        activate: function (event, ui) {
            if (ui.oldPanel.length == 1)
                $("#chartTourId").hide();
            else
                $("#chartTourId").show();
            onCollapsableClick();
            $("#budgetCollapse").removeClass('ui-corner-all');
        }

    });
});

$(function () {
    var icons = {
        header: "iconClosed",
        activeHeader: "iconOpen"
    };
    $("#budgetCollapse").accordion({
        icons: icons
    });
});

$("#divBudgetData [data-toggle=popover]").popover({
    html: true, container: 'body',
    content: function (e) {
        if (!objChangeFlag)
            $("#rollupDirectMsg").text("Click to view selected manager Direct or Rollup budget");
        else
            $("#rollupDirectMsg").text("Oops! you have not saved your inputs. Save your work to proceed further.");
        return $('#merit-rollupDirectchkBox').html();
    }
});

var OpenMeritMandateCommentPopUp = (function (rowData, rowIndex, row) {
    var compensationType = ReporteesPageConstatns.meritmandateType;
    var comment = $("#CommentPopup").data("kendoWindow");
    comment.options.rowData = rowData;
    comment.options.row = row;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Compensation/_MeritExceedComment',
        type: "Post",
        data: {
            __RequestVerificationToken: token,
            empJobNum: rowData.Empjobnum, commentType: compensationType, rowIndex: rowIndex
        },
        success: function (result) {
            $("#divMeritComment").html(result);
            $("#divMeritComment").modal('show');
            $("#divMeritComment #myModalLabel").text("Notes about " + rowData.EmployeeName);
            $("#meritcomment").attr("placeholder", "Write a review comments for " + rowData.EmployeeName)
        }
    });
});
//#endregion 


function closeAfterMeritSlide() {
    ClearCommentChangeFlag();
    isPopupEdited = true;
    $("#divMeritComment").modal('hide');
}

function meritCommentDelete(employeeCommentNum, num) {
    var readOnly = $("#CommentPopup").data("kendoWindow").options.readOnly;
    //var rowData = $("#CommentPopup").data("kendoWindow").options.rowData;
    var index = $("#CommentPopup").data("kendoWindow").options.Values;
    var ItemCommentValue = document.getElementById("comment_" + employeeCommentNum);
    $("comment_" + employeeCommentNum).hide();
    if (readOnly)
        return false;
    if (meritmandCommentsCount <= 1) {
        showAlert("You cannot delete the comment as they are tied to Merit/Promotion increase");
        return false;
    }
    if (employeeCommentNum != 0 && employeeCommentNum != undefined) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '../Compensation/DeleteComment',
            data: { __RequestVerificationToken: token, commentKey: employeeCommentNum },
            type: "post",
            success: function (result) {
                Successmessage("Deleted Successfully");
                inputChanged = false;
                //var rowData = $("#CommentPopup").closest(".k-window-content").data("kendoWindow").options.rowData;
                //var row = $("#CommentPopup").data("kendoWindow").options.row;
                var grdMerit = $("#ReporteeGrid").data("kendoGrid");
                var row = $(grdMerit.tbody).find("tr").eq(index);
                var rowData = grdMerit.dataItem(row);
                rowData.TotalCommentsCount = (rowData.TotalCommentsCount - 1);
                //var rowData = grdMerit._data[index];
                meritmandCommentsCount = meritmandCommentsCount - 1;
                refreshRow(grdMerit, row);
                ItemCommentValue.style.display = "none";
                $("#divMeritComment").modal('hide');
            }
        });
    }
    return false;
}



function meritEditClick(empCommentNum, num) {
    var ItemCommentValue = $("#btnMeritEdit_" + empCommentNum).attr('data-myitemid');
    $("#meritEditBox" + num + "_" + empCommentNum).text(ItemCommentValue);
    $("#meritcomment").css("display", "none");
    $("#btnMeritMandateSave").css("display", "none");
    $("#meritSaveBtn" + num + "_" + empCommentNum).show();
    $("#meritCloseBtn" + num + "_" + empCommentNum).show();
    var isCollapsed = $("#meritEditBox" + num + "_" + empCommentNum).is(":visible");
    if (!isCollapsed) {
        $("#meritText" + num + "_" + empCommentNum).css("display", "none");
    }
    else {
        $("#meritText" + num + "_" + empCommentNum).show();
        $("#meritcomment").show();
        $("#btnMeritMandateSave").show();
    }

    if (lastEditedCommentNum != empCommentNum) {
        if (lastEditedCommentNum != 0) {
            var isCollapsed = $("#meritEditBox" + num + "_" + lastEditedCommentNum).is(":visible");
            if (isCollapsed) {
                $("#meritText" + num + "_" + lastEditedCommentNum).show();
                $("#meritCollapse_" + lastEditedCommentNum).removeClass("in");
            }
            //else
            //$("#generalText" + num + "_" + lastEditedCommentNum).css("display", "none");
        }
        lastEditedCommentNum = empCommentNum;
    }
}



function saveMeritComment(empCommentNum, CompensationTypeNum, num) {
    var commentType = $("#meritEditBox" + num + "_" + empCommentNum).attr('data-commentLabel');
    $("#IsMeritEditItem").val(true);
    var ItemCommentValue = $("#meritEditBox" + num + "_" + empCommentNum).val();
    $("#MeritEmpCommentNum").val(empCommentNum);
    $("#meritcomment").text(ItemCommentValue)
    $("#CompensationTypeNum").val(CompensationTypeNum);
    $("#frmMeritComment").submit();
    if (revertRowIndex != null) {
        revertRowData = null;
        revertRowIndex = null;
        revertMeritPct = null;
        revertMeritlocalAmt = null;
        revertMeritUSDAmt = null;
        var index = $("#CommentPopup").closest(".k-window-content").data("kendoWindow").options.Values;
        var rowData = $("#CommentPopup").data("kendoWindow").options.rowData;
        var row = $("#CommentPopup").data("kendoWindow").options.row;
        calculateMeritAmt(rowData, index, row);
    }
}


function cancelMeritComment(empCommentNum, CompensationTypeNum, num) {
    //var  existingComment = $('p').text();
    //$("#comment").text(existingComment)
    $("#meritText" + num + "_" + empCommentNum).show();
    //$("#meritEditBox" + num + "_" + empCommentNum).css("display", "none");
    $("#meritSaveBtn" + num + "_" + empCommentNum).hide();
    $("#meritCloseBtn" + num + "_" + empCommentNum).hide();
    $("#meritCollapse_" + empCommentNum).removeClass("in");
    $("#meritCollapse_" + empCommentNum).addClass("collapse");
    $("#meritcomment").show();
    $("#btnMeritMandateSave").show();
    //$("#closeDiv").hide();
}
function rollUpDirectDisabled() {
    isRollUpEnabled = false;
    isDirectEnabled = false;
    rollUpDirectVisibility();
}
function rollUpDirectEnabled() {
    isRollUpEnabled = true;
    isDirectEnabled = true;
    rollUpDirectVisibility();
}
function rollUpDirectVisibility() {
    if (isDirectEnabled === true) {
        $("#btnDirect").prop("disabled", false);
    }
    if (isDirectEnabled === false) {
        $("#btnDirect").prop("disabled", true);
    }
    if (isRollUpEnabled === true) {
        $("#btnRollUp").prop("disabled", false);
    }
    if (isRollUpEnabled === false) {
        $("#btnRollUp").prop("disabled", true);
    }
}

function select(e) {
    if (e.item[0].innerText != "Show in USD" && e.item[0].innerText != "Show in Local")
        e._defaultPrevented = !showSaveWarning(e);
}

function OnselectTypeChange(e) {
    var selectedData = $("#ddlActions").data("kendoDropDownList").dataItem($("#ddlActions").data("kendoDropDownList").select());
    var data = this.dataItem();
    var SelectedValue = data.Value;
    var ddlActions = $("#ddlActions").data("kendoDropDownList");
    if (SelectedValue == "Filter") {
        $("body").css("overflow", "hidden");
        $.ajax({
            url: '../Compensation/_FilterSort',
            type: 'POST',
            cache: false,
            async: true,
            dataType: 'html',
            success: function (result) {
                $("#wndFilterSort").html(result);
                var wndFilterSort = $("#wndFilterSort").data("kendoWindow");
                wndFilterSort.options.gridName = "ReporteeGrid";
                wndFilterSort.center().open();
            }
        });
    }
    else if (SelectedValue == "ClearFilter") {
        clearFilterSort();
        $("#" + "ddlActions" + "_listbox .k-item")[2].style.display = "None";
        refreshGrid();
    }
    else if (SelectedValue == "USD") {

        var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
        GridDisplay = SelectedValue;
        grdMeritReportees.refresh();
        displayLocal();
    }
    else if (SelectedValue == "Local") {
        GridDisplay = SelectedValue;
        var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
        grdMeritReportees.refresh();
        displayUSD();
    }

    ddlActions.select(0);
}

function displayUSD() {
    setDisplayForddlActions('USD', "Block");
    setDisplayForddlActions('Local', "None");
}

function displayLocal() {
    setDisplayForddlActions('USD', "None");
    setDisplayForddlActions('Local', "Block");
}

function setDisplayForddlActions(text, display1) {
    $("#" + "ddlActions" + "_listbox .k-item").filter(function () { return $.text([this]).indexOf(text) > -1; }).each(function () { this.style.display = display1; });
}

function ddlActionsDataBound() {
    $("#" + "ddlActions" + "_listbox .k-item")[0].style.display = "None";
    $("#" + "ddlActions" + "_listbox .k-item")[2].style.display = "None";
    $("#" + "ddlActions" + "_listbox .k-item")[3].style.display = "None";
    if (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay)
        displayUSD();
    else {
        setDisplayForddlActions('USD', "None");
        setDisplayForddlActions('Local', "None");
    }

}

var ClearChangeFlag = (function () {
    objChangeFlag = false;
});

function lumpSumRule1(rowData) {
    if (RuleConfiguration.LumpSumRuleLumpSumType == meritPageConstants.LumpSumTypeAutoCalcWithOutOverride || RuleConfiguration.LumpSumRuleLumpSumType == meritPageConstants.LumpSumTypeAutoCalcWithOverride) {
        if (RuleConfiguration.LumpSumRuleRangeMaxPct != 0)
            lumpSumTypeAutoCalcOverrideRangeMaxPCT(rowData);
        else
            lumpSumTypeAutoCalcOverrideRangeMaxAmt(rowData);
    }
}

function lumpSumTypeAutoCalcOverrideRangeMaxPCT(rowData) {
    var meritAmountAndAnnulaisedSalaryLocal = ((rowData.CurrentAnnualSalaryLocalForCalc != null) ? rowData.CurrentAnnualSalaryLocalForCalc : 0) + ((rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0);
    var cuttOffValueLocal = ((rowData.CurrentAnnualSalaryLocalForCalc > ((RuleConfiguration.LumpSumRuleRangeMaxPct * rowData.MktMaxLocal) / 100)) ? rowData.CurrentAnnualSalaryLocalForCalc : ((RuleConfiguration.LumpSumRuleRangeMaxPct * rowData.MktMaxLocal) / 100));
    if (meritAmountAndAnnulaisedSalaryLocal > cuttOffValueLocal) {
        calculateMeritAndLumpSumAmount(rowData, meritAmountAndAnnulaisedSalaryLocal, cuttOffValueLocal);
    }
    else {
        rowData.LumpSumAmtUSD = 0;
        rowData.LumpSumAmtLocal = 0;
        rowData.LumpSumPct = 0;
    }
}


function lumpSumTypeAutoCalcOverrideRangeMaxAmt(rowData) {
    var meritAmountAndAnnulaisedSalaryLocal = ((rowData.CurrentAnnualSalaryLocalForCalc != null) ? rowData.CurrentAnnualSalaryLocalForCalc : 0) + ((rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0);
    var cutOffValueLocal = ((RuleConfiguration.LumpSumRuleRangeMaxAmt + rowData.MktMaxLocal) > rowData.CurrentAnnualSalaryLocalForCalc) ? (RuleConfiguration.LumpSumRuleRangeMaxAmt + rowData.MktMaxLocal) : rowData.CurrentAnnualSalaryLocalForCalc;
    if (meritAmountAndAnnulaisedSalaryLocal > cutOffValueLocal) {
        cutOffValueLocal = (rowData.CurrentAnnualSalaryLocalForCalc > rowData.MktMaxLocal) ? rowData.CurrentAnnualSalaryLocalForCalc : cutOffValueLocal;
        calculateMeritAndLumpSumAmount(rowData, meritAmountAndAnnulaisedSalaryLocal, cutOffValueLocal);
    }
    else {
        rowData.LumpSumAmtUSD = 0;
        rowData.LumpSumAmtLocal = 0;
        rowData.LumpSumPct = 0;
    }
}
function Change(e) {
    inputChanged = true;
}
function calculateMeritAndLumpSumAmount(rowData, meritAmountAndAnnulaisedSalaryLocal, cuttOffValueLocal) {
    var lumpSumAmtLocal = meritAmountAndAnnulaisedSalaryLocal - cuttOffValueLocal;
    var tempLumpSumAmtLocal = lumpSumAmtLocal;
    rowData.LumpSumAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(lumpSumAmtLocal, "lumpsum", "annual") : roundingRule(lumpSumAmtLocal, "lumpsum", "hourly");
    rowData.LumpSumAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.LumpSumAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.LumpSumAmtLocal * rowData.MeritExchangeRate, "general", "hourly");
    var hourlylumpSumAmtUSD = calculateHourly(rowData.LumpSumAmtUSD);
    var hourlylumpSumAmtLocal = calculateHourly(rowData.LumpSumAmtLocal);
    var lumpSumPct = (rowData.LumpSumAmtLocal * 100) / rowData.CurrentAnnualSalaryLocalForCalc;
    rowData.LumpSumPct = roundingRule(lumpSumPct, "lumpsum", "percentage");
    if (RuleConfiguration.MeritValuesReCalculate) {
        var newMeritAmtLocal = ((rowData.MeritAmtLocal != null) ? rowData.MeritAmtLocal : 0) - tempLumpSumAmtLocal;
        var meritPct = (newMeritAmtLocal * 100) / rowData.CurrentAnnualSalaryLocalForCalc;
        meritPct = roundingRule(meritPct, "merit", "percentage");
        rowData.MeritPCT = meritPct;
        rowData.MeritAmtLocal = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(newMeritAmtLocal, "merit", "annual") : roundingRule(newMeritAmtLocal, "merit", "hourly");
        rowData.MeritAmtUSD = rowData.EmployeeStatus.toLowerCase() == 'annual' ? roundingRule(rowData.MeritAmtLocal * rowData.MeritExchangeRate, "general", "annual") : roundingRule(rowData.MeritAmtLocal * rowData.MeritExchangeRate, "general", "hourly");
        var hourlyAmtLocal = calculateHourly(rowData.MeritAmtLocal);
        var hourlyAmtUSD = calculateHourly(rowData.MeritAmtUSD);
    }
}

function checKIsNaN(value) {
    return isNaN(value) ? 0 : value;
}

//----- End Functions-----
var approvalSelectedEmpID = null;
var approvalPageNo = 1;
function GetApprovalReporteesParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
   
    return {
        __RequestVerificationToken: token,
        selectedManagerNum: $("#hndSelectedManagerNum").val(),
        compMenuType: $("#hndMenuType").val(),
        isRollup: $("#hndIsRollup").val() == 1
    }
}


function getApprovalStatus() {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '../Compensation/GetApproveReopenStatus',
        type: "Post",
        cache: false,
        data: {
            __RequestVerificationToken: token,
            selectedManagerNum: $("#hndSelectedManagerNum").val(),
            compMenuType: $("#hndMenuType").val(),
            isRollup: $("#hndIsRollup").val() == 1
        },
        success: function (result) {
            if (result) {
                $("#btnApprove").show();
                $("#Reject").show();
            }
            else {
                $("#btnApprove").hide();
                $("#Reject").hide();
            }
        }
    });
}
function onSelect_approvalEmployeeSearch(e) {
    var selectedEmp = e.item[0].innerText;
    approvalSelectedEmpID = selectedEmp.split('-')[0].trim();
    approvalPageNo = 1;
    approveGridDataBound();
}

function approveGridDataBound() {
    var grid = $("#grdApproveReportees").data("kendoGrid");
    var length = grid.dataSource._data.length;
    var search = (approvalSelectedEmpID == null) ? 0 : 1;
    $("#approvalCount").text(length);
    var rowData = (grid.dataSource.view().length > 0) ? grid.dataSource.view()[0] : null;
    var data = grid.dataSource.view();
    grid.element.find(".k-grid-content").animate({
        scrollTop: 0
    }, 400);
    if (grid.dataSource._data.length == 0) {
        approvalSelectedEmpID = null;
        $("#approveChkAll").hide();
        var colCount = $("#grdApproveReportees").find('th').length;
        $("#grdApproveReportees").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:left;background-color:#6686C4;padding-top:30px"></td></tr>');
        $("#grdApproveReportees").find('.k-grid-content tbody tr td').css("padding-top", 11).append('<b style="padding-left:100px;background-color:#6686C4 !important;color:white !important;">No Results Found!</b>');
    }

    $.each(data, function (index, rowData) {
        if (approvalSelectedEmpID != null && rowData.EmployeeID == approvalSelectedEmpID) {
            approvalSelectedEmpID = null;
            if (previousUid != null) {
                if (grid.tbody.find("tr[data-uid=" + previousUid + "]") != undefined)
                    grid.tbody.find("tr[data-uid=" + previousUid + "]").removeClass("selected");
            }
            previousUid = rowData.uid;
            // grid.refresh();
            grid.tbody.find("tr[data-uid=" + rowData.uid + "]").addClass("selected");
            $("#grdApproveReportees .k-grid-content-locked").find("tr").eq(index).addClass("selected");
            var scrollContentOffset = grid.element.find("tbody").offset().top;
            var selectContentOffset = grid.tbody.find("tr[data-uid=" + rowData.uid + "]").offset().top;
            var distance = selectContentOffset - scrollContentOffset;
            grid.element.find(".k-grid-content").animate({
                scrollTop: distance
            }, 400);
        }
    });
    if (approvalSelectedEmpID != null) {
        approvalPageNo += 1;
        // refreshApproveGrid();
    }
    if (search == 0 && $("#approveChkAll")[0].checked == false)
        $("#approveChkAll").click();
}


//function refreshApproveGrid() {
//    var grdApproveReportees = $("#grdApproveReportees").data("kendoGrid");
//    grdApproveReportees.refresh();
//    $("#approveChkAll").click();
//    $("#approvalEmployeedetails").data("kendoAutoComplete").element[0].value = "";
//}


$(document).on("click", "#approveClear", function (e) {
    $("#approvalEmployeedetails").data("kendoAutoComplete").element[0].value = "";
    var grid = $("#grdApproveReportees").data("kendoGrid");
    //grid.refresh();
    if (previousUid != null) {
        if (grid.tbody.find("tr[data-uid=" + previousUid + "]") != undefined)
            grid.tbody.find("tr[data-uid=" + previousUid + "]").removeClass("selected");
    }
});
$(document).on("click", "#reopenClear", function (e) {
    $("#reopenEmployeedetails").data("kendoAutoComplete").element[0].value = "";
    var grid = $("#grdReopenReportees").data("kendoGrid");
    //grid.refresh();
    if (previousUid != null) {
        if (grid.tbody.find("tr[data-uid=" + previousUid + "]") != undefined)
            grid.tbody.find("tr[data-uid=" + previousUid + "]").removeClass("selected");
    }
});
$(document).on("click", "#submitClear", function (e) {
    $("#submitEmployeedetails").data("kendoAutoComplete").element[0].value = "";
    var grid = $("#grdSubmitReportees").data("kendoGrid");
    //grid.refresh();
    if (previousUid != null) {
        if (grid.tbody.find("tr[data-uid=" + previousUid + "]") != undefined)
            grid.tbody.find("tr[data-uid=" + previousUid + "]").removeClass("selected");
    }
});

//$(document).on("change", "#budgetCheckBox", function (e) {
//    if (this.checked) {
//        $("#hndIsSelectedRollup").val(1);
//    }
//    else {
//        $("#hndIsSelectedRollup").val(0);
//    }
//    BindBudgetData();
//});

$(document).on("change", "[id$=indApprovalChkbox]", function (e) {
    //objChangeFlag = true;
    if (this.checked) {
        var grid = $("#grdApproveReportees").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var rowData = grid._data[rowIndex];
        rowData.ApprovalIsChecked = true;
        var checkboxLength = $("[id$=indApprovalChkbox]").length;
        var checkboxCheckedLength = $("[id$=indApprovalChkbox]:checked").length;
        if (checkboxLength == checkboxCheckedLength) {
            $("#approveChkAll")[0].checked = true;
        }
        else {
            $("#approveChkAll")[0].checked = false;
        }
    }

    else {
        if (this.checked != undefined) {
            var grid = $("#grdApproveReportees").data("kendoGrid");
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.ApprovalIsChecked = false;
            var checkboxLength = $("[id$=indApprovalChkbox]").length;
            var checkboxCheckedLength = $("[id$=indApprovalChkbox]:checked").length;
            if (checkboxLength == checkboxCheckedLength)
                $("#approveChkAll")[0].checked = true;
            else
                $("#approveChkAll")[0].checked = false;
        }
    }
});


$(document).on("change", "#approveChkAll", function () {
    var id = "indApprovalChkbox";
    // objChangeFlag = true;
    if (this.checked) {
        $("[id$=" + id + "]").each(function () {
            this.checked = true;
            var grid = $("#grdApproveReportees").data("kendoGrid");
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.ApprovalIsChecked = true;
        });

    }
    else {
        if (this.checked != undefined) {
            $("[id$=" + id + "]").each(function () {
                this.checked = false;
                var grid = $("#grdApproveReportees").data("kendoGrid");
                var rowIndex = $(this).closest("tr").index();
                var rowData = grid._data[rowIndex];
                rowData.ApprovalIsChecked = false;
            });
        }
    }
});


$(document).on('click', '#clearfilter', function () {
    clearFilterSort();
    refreshGrid();
    $("#filterseperator").css("display", "none");
    $("#clearfilter").css("display", "none");
    $("#filter").show();
});


$(document).on("click", "[id$=btnApproveSelectedEmployees]", function () {
    var checkboxCheckedLength = $("[id$=indApprovalChkbox]:checked").length;
    if (checkboxCheckedLength > 0) {
        var comment = $("#approvalComment").val();
        approveSelectedEmployees(comment);
    }
    else {
        showAlert("Please select atleast one user");
        return false;
    }
});

//function clearApprovalEmployeeSearch() {
//    approvalPageNo = 1;
//    refreshApproveGrid();
//}

function approveSelectedEmployees(comment) {
    objChangeFlag = false;
    var selectedManagerNum = eval($("#hndSelectedManagerNum").val());
    var compMenuType = $("#hndMenuType").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var grid = $("#grdApproveReportees").data("kendoGrid");
    var postData = [];
    var length = grid.dataSource._data.length;
    for (var i = 0; i < length; i++) {
        var rowData = grid.dataSource._data[i];
        if (rowData.ApprovalIsChecked == true) {
            postData.push(rowData);
        }
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    var jsonString = JSON.stringify(postData)
    var jsonData = JSON.parse(jsonString)
    $.ajax({
        url: '../Compensation/CompApproval',
        type: "post",
        async: true,
        cache: false,
        data: {
            __RequestVerificationToken: token,
            selectedRows: jsonData,
            selectedManagerNum: selectedManagerNum,
            MenuType: compMenuType,
            isRollup: isRollup,
            approvalStatus: 2,
            comment: comment
        },
        success: function (result) {
            refreshGridTreeChange();
            $("#divApprovePopup").modal('hide');
            Successmessage("Approved Successfully");
            var tr = $("#ReporteeGrid .k-grid-content").find('tr');
            if (tr.length > 0 && ($(tr[0]).find('input')[1]) != undefined) {
                window.setTimeout(function () {
                    $(tr[0]).find('input')[1].focus();
                }, 50);
            }
        }
    });
}


var reopenSelectedEmpID = null;
var reopenPageNo = 1;
function GetReOpenReporteesParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    
    return {
        __RequestVerificationToken: token,
        selectedManagerNum: $("#hndSelectedManagerNum").val(),
        compMenuType: $("#hndMenuType").val(),
        isRollup: $("#hndIsRollup").val() == 1
    }
}

function onSelect_reopenEmployeeSearch(e) {
    var selectedEmp = e.item[0].innerText;
    reopenSelectedEmpID = selectedEmp.split('-')[0].trim();
    reopenPageNo = 1;
    reopenGridDataBound();
}

//function clearReopenEmployeeSearch() {
//    reopenPageNo = 1;
//    refreshReopenGrid();
//}

function reopenGridDataBound() {
    var grid = $("#grdReopenReportees").data("kendoGrid");
    var length = grid.dataSource._data.length;
    (length != 0 && length != undefined) ? $("#btnReopenSelectedEmployees").attr("disabled", false) : $("#btnReopenSelectedEmployees").attr("disabled", true);
    // var length = grid.pager.dataSource._pristineTotal;
    $("#reopenCount").text(length);
    var rowData = (grid.dataSource.view().length > 0) ? grid.dataSource.view()[0] : null;
    var data = grid.dataSource.view();
    grid.element.find(".k-grid-content").animate({
        scrollTop: 0
    }, 400);
    if (grid.dataSource._data.length == 0) {
        reopenSelectedEmpID = null;
        $("#reopenChkAll").hide();
        var colCount = $("#grdReopenReportees").find('th').length;
        $("#grdReopenReportees").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:left;background-color:#6686C4;padding-top:30px"></td></tr>');
        $("#grdReopenReportees").find('.k-grid-content tbody tr td').css("padding-top", 11).append('<b style="padding-left:100px;background-color:#6686C4 !important;color:white !important;">No Results Found!</b>');
    }

    $.each(data, function (index, rowData) {
        if (reopenSelectedEmpID != null && rowData.EmployeeID == reopenSelectedEmpID) {
            reopenSelectedEmpID = null;
            if (previousUid != null) {
                if (grid.tbody.find("tr[data-uid=" + previousUid + "]") != undefined)
                    grid.tbody.find("tr[data-uid=" + previousUid + "]").removeClass("selected");
            }
            previousUid = rowData.uid;
            //grid.refresh();
            grid.tbody.find("tr[data-uid=" + rowData.uid + "]").addClass("selected");
            $("#grdReopenReportees .k-grid-content-locked").find("tr").eq(index).addClass("selected");
            var scrollContentOffset = grid.element.find("tbody").offset().top;
            var selectContentOffset = grid.tbody.find("tr[data-uid=" + rowData.uid + "]").offset().top;
            var distance = selectContentOffset - scrollContentOffset;
            grid.element.find(".k-grid-content").animate({
                scrollTop: distance
            }, 400);
        }
    });
    if (reopenSelectedEmpID != null) {
        reopenPageNo += 1;
        //refreshReopenGrid();
    }
}


//function refreshReopenGrid() {
//    var grdReopenReportees = $("#grdReopenReportees").data("kendoGrid");
//    grdReopenReportees.refresh();
//    if ($("#reopenChkAll")[0].checked == true)
//        $("#reopenChkAll").click();
//    $("#reopenEmployeedetails").data("kendoAutoComplete").element[0].value = "";
//}


$(document).on("change", "[id$=indReopenChkbox]", function (e) {
    //objChangeFlag = true;
    if (this.checked) {
        var grid = $("#grdReopenReportees").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var rowData = grid._data[rowIndex];
        rowData.ApprovalIsChecked = true;
        var checkboxLength = $("[id$=indReopenChkbox]").length;
        var checkboxCheckedLength = $("[id$=indReopenChkbox]:checked").length;
        if (checkboxLength == checkboxCheckedLength) {
            $("#reopenChkAll")[0].checked = true;
        }
        else {
            $("#reopenChkAll")[0].checked = false;
        }
    }

    else {
        if (this.checked != undefined) {
            var grid = $("#grdReopenReportees").data("kendoGrid");
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.ApprovalIsChecked = false;
            var checkboxLength = $("[id$=indReopenChkbox]").length;
            var checkboxCheckedLength = $("[id$=indReopenChkbox]:checked").length;
            if (checkboxLength == checkboxCheckedLength)
                $("#reopenChkAll")[0].checked = true;
            else
                $("#reopenChkAll")[0].checked = false;
        }
    }
});


$(document).on("change", "#reopenChkAll", function () {
    var id = "indReopenChkbox";
    // objChangeFlag = true;
    if (this.checked) {
        $("[id$=" + id + "]").each(function () {
            this.checked = true;
            var grid = $("#grdReopenReportees").data("kendoGrid");
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.ApprovalIsChecked = true;
        });

    }
    else {
        if (this.checked != undefined) {
            $("[id$=" + id + "]").each(function () {
                this.checked = false;
                var grid = $("#grdReopenReportees").data("kendoGrid");
                var rowIndex = $(this).closest("tr").index();
                var rowData = grid._data[rowIndex];
                rowData.ApprovalIsChecked = false;
            });
        }
    }
});


function reopenSelectedEmployees(comment) {
    objChangeFlag = false;
    var selectedManagerNum = eval($("#hndSelectedManagerNum").val());
    var compMenuType = $("#hndMenuType").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var grid = $("#grdReopenReportees").data("kendoGrid");
    var postData = [];
    var length = grid.dataSource._data.length;
    for (var i = 0; i < length; i++) {
        var rowData = grid.dataSource._data[i];
        if (rowData.ApprovalIsChecked == true) {
            postData.push(rowData);
        }
    }

    var jsonString = JSON.stringify(postData)
    var jsonData = JSON.parse(jsonString)

    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Compensation/CompApproval',
        type: "post",
        cache: false,
        data: ({
            __RequestVerificationToken:token,
            selectedRows: jsonData,
            selectedManagerNum: selectedManagerNum,
            MenuType: compMenuType,
            isRollup: isRollup,
            approvalStatus: 3,
            comment: comment
        }),
        success: function (result) {
            refreshGridTreeChange();
            $("#divReopenPopup").modal('hide');
            Successmessage("Rejected Successfully");
            var tr = $("#ReporteeGrid .k-grid-content").find('tr');
            if (tr.length > 0 && ($(tr[0]).find('input')[1]) != undefined) {
                window.setTimeout(function () {
                    $(tr[0]).find('input')[1].focus();
                }, 50);
            }
        }
    });
}


$(document).on("click", "[id$=btnReopenSelectedEmployees]", function () {
    var checkboxCheckedLength = $("[id$=indReopenChkbox]:checked").length;
    if (checkboxCheckedLength > 0) {
        var comment = $("#reopenComment").val();
        if (comment != "")
            reopenSelectedEmployees(comment);
        else {
            showAlert("Please provide the comment");
            return false;
        }
    }
    else {
        showAlert("Please select atleast one user");
        return false;
    }
});


var submitSelectedEmpID = null;
var submitPageNo = 1;
function GetSubmitReporteesParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    return {
        __RequestVerificationToken:token,
        selectedManagerNum: $("#hndSelectedManagerNum").val(),
        compMenuType: $("#hndMenuType").val(),
        isRollup: $("#hndIsRollup").val() == 1
    }
}

function onSelect_submitEmployeeSearch(e) {
    var selectedEmp = e.item[0].innerText;
    submitSelectedEmpID = selectedEmp.split('-')[0].trim();
    submitPageNo = 1;
    submitGridDataBound();
}

function submitGridDataBound() {
    var grid = $("#grdSubmitReportees").data("kendoGrid");
    var length = grid.dataSource._data.length;
    // var length = grid.pager.dataSource._pristineTotal;
    $("#recordCount").text(length);
    var submitSearch = (submitSelectedEmpID == null) ? 0 : 1;
    var rowData = (grid.dataSource.view().length > 0) ? grid.dataSource.view()[0] : null;
    var data = grid.dataSource.view();
    grid.element.find(".k-grid-content").animate({
        scrollTop: 0
    }, 400);
    if (grid.dataSource._data.length == 0) {
        submitSelectedEmpID = null;
        $("#submitChkAll").hide();
        var colCount = $("#grdSubmitReportees").find('th').length;
        $("#grdSubmitReportees").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:left;background-color:#6686C4;padding-top:30px"></td></tr>');
        $("#grdSubmitReportees").find('.k-grid-content tbody tr td').css("padding-top", 11).append('<b style="padding-left:100px;background-color:#6686C4 !important;color:white !important;">No Results Found!</b>');
    }

    $.each(data, function (index, rowData) {
        if (submitSelectedEmpID != null && rowData.EmployeeID == submitSelectedEmpID) {
            submitSelectedEmpID = null;
            //grid.refresh();
            if (previousUid != null) {
                if (grid.tbody.find("tr[data-uid=" + previousUid + "]") != undefined)
                    grid.tbody.find("tr[data-uid=" + previousUid + "]").removeClass("selected");
            }
            previousUid = rowData.uid;
            grid.tbody.find("tr[data-uid=" + rowData.uid + "]").addClass("selected");
            $("#grdSubmitReportees .k-grid-content-locked").find("tr").eq(index).addClass("selected");
            var scrollContentOffset = grid.element.find("tbody").offset().top;
            var selectContentOffset = grid.tbody.find("tr[data-uid=" + rowData.uid + "]").offset().top;
            var distance = selectContentOffset - scrollContentOffset;
            grid.element.find(".k-grid-content").animate({
                scrollTop: distance
            }, 400);
        }
    });
    if (submitSelectedEmpID != null) {
        submitPageNo += 1;
        //  refreshsubmitGrid();
    }
    if (submitSearch == 0 && $("#submitChkAll")[0].checked == false)
        $("#submitChkAll").click();
}

//function clearSubmitEmployeeSearch() {
//    submitPageNo = 1;
//    refreshsubmitGrid();
//    $("#submitChkAll").click();
//}

//function refreshsubmitGrid() {
//    var grdSubmitReportees = $("#grdSubmitReportees").data("kendoGrid");
//    grdSubmitReportees.refresh();
//    $("#submitEmployeedetails").data("kendoAutoComplete").element[0].value = "";
//}


$(document).on("change", "[id$=indSubmitChkbox]", function (e) {
    //objChangeFlag = true;
    if (this.checked) {
        var grid = $("#grdSubmitReportees").data("kendoGrid");
        var rowIndex = $(this).closest("tr").index();
        var rowData = grid._data[rowIndex];
        rowData.SubmitIsChecked = true;
        var checkboxLength = $("[id$=indSubmitChkbox]").length;
        var checkboxCheckedLength = $("[id$=indSubmitChkbox]:checked").length;
        if (checkboxLength == checkboxCheckedLength) {
            $("#submitChkAll")[0].checked = true;
        }
        else {
            $("#submitChkAll")[0].checked = false;
        }
    }

    else {
        if (this.checked != undefined) {
            var grid = $("#grdSubmitReportees").data("kendoGrid");
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.SubmitIsChecked = false;
            var checkboxLength = $("[id$=indSubmitChkbox]").length;
            var checkboxCheckedLength = $("[id$=indSubmitChkbox]:checked").length;
            if (checkboxLength == checkboxCheckedLength)
                $("#submitChkAll")[0].checked = true;
            else
                $("#submitChkAll")[0].checked = false;
        }
    }
});


$(document).on("change", "#submitChkAll", function () {
    var id = "indSubmitChkbox";
    //objChangeFlag = true;
    if (this.checked) {
        $("[id$=" + id + "]").each(function () {
            this.checked = true;
            var grid = $("#grdSubmitReportees").data("kendoGrid");
            var rowIndex = $(this).closest("tr").index();
            var rowData = grid._data[rowIndex];
            rowData.SubmitIsChecked = true;
        });

    }
    else {
        if (this.checked != undefined) {
            $("[id$=" + id + "]").each(function () {
                this.checked = false;
                var grid = $("#grdSubmitReportees").data("kendoGrid");
                var rowIndex = $(this).closest("tr").index();
                var rowData = grid._data[rowIndex];
                rowData.SubmitIsChecked = false;
            });
        }
    }
});


function submitSelectedEmployees(comment) {
    objChangeFlag = false;
    var selectedManagerNum = eval($("#hndSelectedManagerNum").val());
    var compMenuType = $("#hndMenuType").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var grid = $("#grdSubmitReportees").data("kendoGrid");
    var postData = [];
    var length = grid.dataSource._data.length;
    for (var i = 0; i < length; i++) {
        var rowData = grid.dataSource._data[i];
        if (rowData.SubmitIsChecked == true) {
            postData.push(rowData);
        }
    }

    var jsonString = JSON.stringify(postData)
    var jsonData = JSON.parse(jsonString)

    var token = $('input[name="__RequestVerificationToken"]').val();
    
    $.ajax({
        url: '../Compensation/CompApproval',
        type: "Post",
        cache: false,
        data: {
            __RequestVerificationToken:token,
            selectedRows: jsonData,
            selectedManagerNum: selectedManagerNum,
            MenuType: compMenuType,
            isRollup: isRollup,
            approvalStatus: 1,
            comment: comment
        },
        success: function (result) {
            refreshGridTreeChange();
            $("#divSubmitPopup").modal('hide');
            Successmessage("Submitted Successfully");
            var tr = $("#ReporteeGrid .k-grid-content").find('tr');
            if (tr.length > 0 && ($(tr[0]).find('input')[1]) != undefined) {
                window.setTimeout(function () {
                    $(tr[0]).find('input')[1].focus();
                }, 50);
            }
        }
    });
}
$(document).on("click", "[id$=btnSubmit]", function () {
    triggerSaveEvent();
    objChangeFlag = false;
});
$(document).on("click", "[id$=Reject]", function () {
    triggerSaveEvent();
    objChangeFlag = false;
});
$(document).on("click", "[id$=btnApprove]", function () {
    triggerSaveEvent();
    objChangeFlag = false;
});
$(document).on("click", "#btnCancelSubmit", function () {
    objChangeFlag = false;
});
$(document).on("click", "#btnCancelApprove", function () {
    objChangeFlag = false;
});
$(document).on("click", "#btnCancelReopen", function () {
    objChangeFlag = false;
});

$(document).on("click", "[id$=btnSubmitSelectedEmployees]", function () {
    var checkboxCheckedLength = $("[id$=indSubmitChkbox]:checked").length;
    if (checkboxCheckedLength > 0) {
        var comment = $("#submissionComment").val();
        submitSelectedEmployees(comment);
    }
    else {
        showAlert("Please select atleast one user");
        return false;
    }
});

$(document).on("click", "#arrowid", function () {
    triggerSaveEvent();
    BindBudgetData();
    objChangeFlag = false;
});

function chartClick() {
    triggerSaveEvent()
    BindBudgetData();
    objChangeFlag = false;
}

function triggerSaveEvent() {
    if (objChangeFlag == true) {
        objChangeFlag = false;
        var grdMerit = $("#ReporteeGrid").data("kendoGrid");
        grdMerit.dataSource.unbind("sync");
        grdMerit.dataSource.bind("sync", grdMeritReportees_sync);
        grdMerit.dataSource.sync();
    }
    objChangeFlag = false;
}
function showCommentImage(empJobNum, totalComments, unReadComments) {
    var commentImage = (totalComments == 0) ?
        "<img src='../../Images/cmt-blank.png'></img>" :
        (unReadComments == 0) ?
        "<img src='../../Images/cmt-read.png'></img>" :
        "<img src='../../Images/cmt-unread.png'></img>";
    return commentImage;
}
function showStatusImage(WorkFlowStatus, FlagTooltip) {
    var statusImage = "";
    switch (WorkFlowStatus) {

        case "Approved": //Approve
            statusImage = "<img src='../../Images/MeritStatus/approved-icon.png' title='" + FlagTooltip + "'></img>";
            break;
        case "Reopen"://Reopen
            statusImage = "<img src='../../Images/MeritStatus/rejected-icon.png' title='" + FlagTooltip + "'></img>";
            break;
        case "ActionRequired"://
            statusImage = "<img src='../../Images/MeritStatus/action-req1.png' title='" + FlagTooltip + "'></img>";
            break;
        case "Submitted"://yellow
            
            statusImage = "<img src='../../Images/MeritStatus/inprogress-icon.png' title='" + FlagTooltip + "'></img>";
            break;
        case "InProgress"://transaparent
            statusImage = "<img src='../../Images/MeritStatus/yet-to-start.png' title='" + FlagTooltip + "'></img>";
            break;

    }
    return statusImage;

}

$("#element").on("click.someNamespace", function () { console.log("anonymous!"); });

$(document).on("click", "#menu-close", function (e) {
    $("#sidebar-wrapper").toggleClass("active");
});
$(document).on("click", "#menu-toggle", function (e) {
    $("#sidebar-wrapper").toggleClass("active");
});

$(document).on("click", "#corporateTeam", function (e) {
    $("#sidebar-wrapper").toggleClass("active");
});
//$(document).on("click", "#SelectedManagerName", function (e) {
//    $("#sidebar-wrapper").toggleClass("active");
//});
$(document).mouseup(function (e) {
    var container = $("#sidebar-wrapper");
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.removeClass('active');
    }
});

// Promotion Popup Related Code


$(document).on("click", "#btnPromotionEdit", function (e) {
    var readOnly = $("#CommentPopup").data("kendoWindow").options.readOnly;
    if (readOnly)
        return false;
    if ($(this).parent().find('input').first().val() != null) {
        $("#EmpPromotionCommentNum").val($(this).parent().find('input')[0].value);
    }
    var ItemCommentValue = $(this).attr('data-myitemid');
    $("#promotionComment").text(ItemCommentValue);
    $("#textboxPromotion").attr("disabled", "disabled");
    $("#btnPromotionRefresh").show();
    return false;

});

$(document).on("click", "#btnPromotionRefresh", function (e) {
    if (!showSaveWarning(e, "inputChanged")) return false;
    $("#EmpPromotionCommentNum").val(0);
    $("#textboxPromotion").attr("disabled", false);
    $("#promotionComment").text('');
    $("#textboxPromotion").text('');
    $("#btnPromotionRefresh").hide();
    return false;
});
$(document).on("click", "#btnPromotionCommDelete", function (e) {
    //var row = $("#CommentPopup").data("kendoWindow").options.row;
    var index = $("#CommentPopup").data("kendoWindow").options.Values;
    var grdMerit = $("#ReporteeGrid").data("kendoGrid");
    var row = $(grdMerit.tbody).find("tr").eq(index);
    var readOnly = $("#CommentPopup").data("kendoWindow").options.readOnly;
    var rowData = grdMerit.dataItem(row);
    //var rowData = $("#CommentPopup").data("kendoWindow").options.rowData;
    if (readOnly)
        return false;
    if (promotionCommentsCount <= 1 && rowData.NewTitle != null) {
        showAlert("You cannot delete the comment as they are tied to Merit/Promotion increase");
        return false;
    }
    if (!showConfirm(e, "Do you want to delete the comment?")) return false;
    var employeeCompCommentNum;
    var ItemCommentValue;
    if ($(this).parent().find('input').first().val() != null) {
        employeeCompCommentNum = $("#EmpPromotionCommentNum").val($(this).parent().find('input')[0].value)[0].value;
        ItemCommentValue = document.getElementById("comment_" + $(this).attr('data-mycommentitemid'));
    }
    if (employeeCompCommentNum != 0 && employeeCompCommentNum != undefined) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '../Comment/DeleteComment',
            data: { __RequestVerificationToken: token, commentKey: employeeCompCommentNum },
            type: "post",
            success: function (result) {
                Successmessage("Deleted Successfully");
                inputChanged = false;
                $("#divPromotion").modal('hide');
                promotionCommentsCount = promotionCommentsCount - 1;
                if (rowData != null || rowData != undefined) {
                    rowData.PromotionCommentsCount = rowData.PromotionCommentsCount - 1;
                    rowData.TotalCommentsCount = rowData.TotalCommentsCount - 1;
                    ItemCommentValue.style.display = "none";
                    if (rowData.PromotionCommentsCount == 0)
                        refreshRow(grdMerit, row);
                }
            }
        });
    }
    else
        return false;
});

$(document).on("click", "#btnPromotionRevert", function (e) {
    var readOnly = $("#CommentPopup").data("kendoWindow").options.readOnly;
    if (readOnly)
        return false;
    //   if (promotionCommentsCount > 0) {
    var rowData = $("#CommentPopup").data("kendoWindow").options.rowData;
    var row = $("#CommentPopup").data("kendoWindow").options.row;
    var rowIndex = $("#CommentPopup").data("kendoWindow").options.index;
    var grdMerit = $("#ReporteeGrid").data("kendoGrid");
    if (!showConfirm(e, "Are you sure to delete the promotion? If you click OK, it will also delete the promotion title,comments and increase.")) return false;
    rowData.PromotionAmtLocal = null;
    rowData.PromotionAmtUSD = null;
    rowData.HrlyPromotionAmtLocal = null;
    rowData.HrlyPromotionAmtUSD = null;
    rowData.PromotionPct = null;
    CalculateNewSalary(rowData);
    CalculateTCC(rowData);
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Compensation/RevertPromotion',
        data: { __RequestVerificationToken: token, empJobNum: rowData.Empjobnum, newSalaryLocal: rowData.NewSalaryLocal, newHrlyRate: rowData.NewHourlyRateLocal, newCompRatio: rowData.NewCompaRatio, TCC: rowData.TCCLocal },
        type: "post",
        success: function (result) {
            Successmessage("Deleted Successfully");
            fn_BudgetPromotionSpentChanges((rowData.PromotionAmtUSD != null) ? rowData.PromotionAmtUSD : 0, 0);
            rowData.PromotionCommentNum = null;
            rowData.TotalCommentsCount = rowData.TotalCommentsCount - rowData.PromotionCommentsCount;
            rowData.PromotionCommentsCount = 0;
            rowData.NewGrade = null;
            rowData.NewTitle = null;
            refreshRow(grdMerit, row);
            $("#divPromotion").modal('hide');
        }
    });
});
function disableControls(readOnly) {
    if (readOnly) {
        $("#btnPromotionPopupOk").attr("disabled", true).addClass("k-state-disabled");
        $("#textboxPromotion").attr("disabled", true).addClass("k-state-disabled");
        $("#promotionComment").attr("disabled", true).addClass("k-state-disabled");
        $("#btnPromotionRefresh").attr("disabled", true).addClass("k-state-disabled");
        $("#btnPromotionRevert").attr("disabled", true).addClass("k-state-disabled");
        $("[id$=btnDelete],[id*=btnDelete],[id$=btnPromotionEdit],[id*=btnPromotionEdit]").attr("disabled", true).addClass("k-state-disabled");
    }
}

$(document).on("click", "#btnPromotionPopupOk", function (e) {
    var readOnly = $("#CommentPopup").data("kendoWindow").options.readOnly;
    // if (readOnly)
    // return false;
    var commentsValue = $("#promotionComment").val();
    var comment_MaxLength = 2000;
    var promotionText = $("#textboxPromotion").val();
    inputChanged = false;
    var rowData = $("#CommentPopup").data("kendoWindow").options.rowData;
    var row = $("#CommentPopup").data("kendoWindow").options.row;
    var rowIndex = $("#CommentPopup").data("kendoWindow").options.index;

    if ($.trim($('#textboxPromotion').val()) == '' || $.trim($('#promotionComment').val()) == '') {
        //showAlert("PLease enter alpha-numeric characters");
    }
    else if (commentsValue == "" || promotionText == "")
        showAlert("You are doing great! Please enter 'Promote to' and comment to complete the promotion process");
    else if (commentsValue.length > comment_MaxLength) {
        showAlert("Comment should not exceed the character limit");
        return false;
    }
    else {
        var promCommentNum = $("#EmpPromotionCommentNum").val();
        var selectedData = $("#textboxPromotion").val();
        if (selectedData != '') {
            var grdMerit = $("#ReporteeGrid").data("kendoGrid");
            var promotionComment = $("#promotionComment").val();
            rowData.PromotionCommentNum = promCommentNum;
            rowData.PromotionComment = promotionComment;
            rowData.NewGrade = selectedData;
            rowData.NewTitle = selectedData;
            rowData.PromotionCommentsCount = rowData.PromotionCommentsCount + 1;
            rowData.TotalCommentsCount = rowData.TotalCommentsCount + 1;
            rowData.dirty = true;
            refreshRow(grdMerit, row);

            var token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                url: '../Compensation/PromotionComment',
                data: { __RequestVerificationToken: token, empJobNum: rowData.Empjobnum, employeeCompCommentNum: rowData.PromotionCommentNum, comments: rowData.PromotionComment, grade: rowData.NewGrade },
                type: 'Post',
                async: true,
                cache: true,
                success: function (result) {
                    $("#divPromotion").modal('hide');
                }
            });
        }
    }
});
//
//Manager Action
$(document).on("click", "#btnSubmit", function (e) {

    $.ajax({
        url: "../Compensation/_SubmitPopUp",
        type: "Get",
        success: function (result) {
            $("#divSubmitPopup").html(result);
            $("#divSubmitPopup").modal('show');
        }
    });

});
$(document).on("click", "#btnApprove", function (e) {

    $.ajax({
        url: "../Compensation/_ApprovePopUp",
        type: "Get",
        success: function (result) {
            $("#divApprovePopup").html(result);
            $("#divApprovePopup").modal('show');
        }
    });

});
$(document).on("click", "#Reject", function (e) {

    $.ajax({
        url: "../Compensation/_ReopenPopUp",
        type: "Get",
        success: function (result) {
            $("#divReopenPopup").html(result);
            $("#divReopenPopup").modal('show');
        }
    });

});
$(document).on('click', '#filter', function () {
    $("body").css("overflow", "hidden");
    $.ajax({
        url: "../Compensation/_FilterSort",
      
        type: 'Get',
        cache: false,
        async: true,
        dataType: 'html',
        success: function (result) {
            $("#wndFilterSort").html(result);
            var wndFilterSort = $("#wndFilterSort").data("kendoWindow");
            wndFilterSort.options.gridName = "ReporteeGrid";
            wndFilterSort.center().open();
            $("#filterseperator").css("display", "inline");
        }
    });
});
function select(e) {
    if (e.item[0].innerText != "Show in USD" && e.item[0].innerText != "Show in Local")
        e._defaultPrevented = !showSaveWarning(e);
}
function displayUSD() {
    setDisplayForddlActions('USD', "Block");
    setDisplayForddlActions('Local', "None");
}
function displayLocal() {
    setDisplayForddlActions('USD', "None");
    setDisplayForddlActions('Local', "Block");
}
function setDisplayForddlActions(text, display1) {
    $("#" + "ddlActions" + "_listbox .k-item").filter(function () { return $.text([this]).indexOf(text) > -1; }).each(function () { this.style.display = display1; });
}

function ddlActionsDataBound() {
    $("#" + "ddlActions" + "_listbox .k-item")[0].style.display = "None";
    $("#" + "ddlActions" + "_listbox .k-item")[2].style.display = "None";
    $("#" + "ddlActions" + "_listbox .k-item")[3].style.display = "None";
    if (RuleConfiguration.FeatureConfigurationMultiCurrencyDisplay)
        displayUSD();
    else {
        setDisplayForddlActions('USD', "None");
        setDisplayForddlActions('Local', "None");
    }

}
var ClearChangeFlag = (function () {
    objChangeFlag = false;
});
$(document).on('keyup', '#ManagerTreesearch', function () {
    $('span.k-in > span.highlight').each(function () {
        $(this).parent().text($(this).parent().text());
    });

    var term = this.value;
    var tlen = term.length;
    if ($.trim($(this).val()) == '') {
        $('#ddlCompManagerTreeView .k-ext-treeview').scrollTop(0);
    }

    $('#ddlCompManagerTreeView span.k-in').each(function () {
        var nodeData = $('#ddlCompManagerTreeView .k-ext-treeview').data('kendoTreeView').dataItem($(this));
        $('#ddlCompManagerTreeView .k-ext-treeview').scrollTop($('#ddlCompManagerTreeView').find('span.k-in:icontains("' + term + '")').offset().top);
        var htmlValue = ShowStatusCompManagerTree(nodeData, term);
        $(this).html(htmlValue);
    });

});

$(document).on('click', '#usdAmt', function () {
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    GridDisplay = managerActionTypeUSD;
    grdMeritReportees.refresh();
    displayLocal();
    $("#usdAmt").css("display", "none");
    $("#localAmt").css("display", "inline");
    $("#clearfilter").css("display", "none");

});
$(document).on('click', '#localAmt', function () {
    GridDisplay = managerActionTypeLocal;
    var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
    grdMeritReportees.refresh();
    displayUSD();
    $("#usdAmt").css("display", "inline");
    $("#localAmt").css("display", "none");
    $("#clearfilter").css("display", "none");
});
var saveWarning = (function () {
    if (objChangeFlag == true && confirm(globalWarningMessage)) {
        return false;
    }
    ClearChangeFlag();
    return true;
});
//function select(e) {
//    if (e.item[0].innerText != "Show in USD" && e.item[0].innerText != "Show in Local")
//        e._defaultPrevented = !showSaveWarning(e);
//}

function OnselectTypeChange(e) {
    var selectedData = $("#ddlActions").data("kendoDropDownList").dataItem($("#ddlActions").data("kendoDropDownList").select());
    var data = this.dataItem();
    var SelectedValue = data.Value;
    var ddlActions = $("#ddlActions").data("kendoDropDownList");
    if (SelectedValue == filterValue) {
        $("body").css("overflow", "hidden");
        $.ajax({
            url: "../Compensation/_FilterSort",
            type: 'Get',
            cache: false,
            async: true,
            dataType: 'html',
            success: function (result) {
                $("#wndFilterSort").html(result);
                var wndFilterSort = $("#wndFilterSort").data("kendoWindow");
                wndFilterSort.options.gridName = "ReporteeGrid";
                wndFilterSort.center().open();
            }
        });
    }
    else if (SelectedValue == clearFilterValue) {
        clearFilterSort();
        $("#" + "ddlActions" + "_listbox .k-item")[2].style.display = "None";
        refreshGrid();
    }
    else if (SelectedValue == exportValue) {
        var selectedManagerNum = $("#hndSelectedManagerNum").val();
        window.location.href = "../Compensation/_ExportData?managerNum=" + selectedManagerNum + "&loggedInEmployeeNum=" + '@Model.CompensationTypeConfiguration.LoggedInEmployeeNum' + "&compMenuType=" + '@Model.CompMenuType';
    }
    else if (SelectedValue == managerActionTypeUSD) {

        var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
        GridDisplay = SelectedValue;
        grdMeritReportees.refresh();
        displayLocal();
    }
    else if (SelectedValue == managerActionTypeLocal) {
        GridDisplay = SelectedValue;
        var grdMeritReportees = $("#ReporteeGrid").data("kendoGrid");
        grdMeritReportees.refresh();
        displayUSD();
    }

    ddlActions.select(0);
}
//
//Filter 
function addFilterSort() {
    var option = document.createElement("option");
    var column = $("#ddlColumns").val();
    var columnText = $("#ddlColumns").data("kendoDropDownList").text();
    var operation = $("#ddlOperations").val();
    var operationText = $("#ddlOperations").data("kendoDropDownList").text();
    var value = (columnValue == "WorkFlow") ? $("#ddlValues").val() : $("#txtSearch").val();
    if (operation == "asc" || operation == "desc") {
        option.text = columnText + " -> " + operation;
        option.value = column + "|" + operation;
        $("#lstSorts").append(option);
    } else {
        option.text = columnText + " -> " + operationText + " -> " + value;
        option.value = column + "|" + operation + "|" + value;
        $("#lstFilters").append(option);
    }
}

function clearFilterSort() {
    var wndFilterSort = $("#divFilterSort").closest(".k-window-content").data("kendoWindow");
    var gridName = wndFilterSort.options.gridName;
    var grid = $("#" + gridName).data("kendoGrid");
    $("#lstSorts").empty();
    $("#lstFilters").empty();
    ($("#ddlColumns").data("kendoDropDownList") != undefined) ? $("#ddlColumns").data("kendoDropDownList").value(null) : "";
    ($("#ddlOperations").data("kendoDropDownList") != undefined) ? $("#ddlOperations").data("kendoDropDownList").value(null) : "";
    $("#txtSearch").val('');
    //$("#btnApply").data("kendoButton").enable(($("#lstFilters option").length > 0 || $("#lstSorts option").length > 0));

    $("#btnAddToList").prop("disabled", !($("#lstFilters option").length > 0 || $("#lstSorts option").length > 0));
    var gridPage = 0;
    var gridgroup = 0;
    if (grid.pager != undefined) {
        gridPage = grid.pager.dataSource._pageSize;
        gridGroup = grid.pager.dataSource._group;
    }
    grid.dataSource.query({ page: 1, pageSize: gridPage, filter: [], sort: [], group: gridgroup });
}

function showOrHideOperations(dataType, value) {
    var items = $("[id='ddlOperations_listbox']").last().find(".k-item");
    if (value == 'WorkFlow') {
        items[1].style.display = "none";
        items[2].style.display = "none";
        items[3].style.display = "none";
        items[4].style.display = "none";
        items[5].style.display = "none";
        items[6].style.display = "block";
        items[7].style.display = "none";
        items[8].style.display = "none";
        items[9].style.display = "none";
        items[10].style.display = "none";
        items[11].style.display = "none";
    }
    else {
        if (dataType == "string") {
            items[1].style.display = "block";
            items[2].style.display = "block";
            items[3].style.display = "block";
            items[4].style.display = "block";
            items[5].style.display = "block";
            items[6].style.display = "none";
            items[7].style.display = "none";
            items[8].style.display = "none";
            items[9].style.display = "none";
            items[10].style.display = "none";
            items[11].style.display = "none";
        }
        else {
            items[1].style.display = "block";
            items[2].style.display = "block";
            items[3].style.display = "none";
            items[4].style.display = "none";
            items[5].style.display = "none";
            items[6].style.display = "block";
            items[7].style.display = "block";
            items[8].style.display = "block";
            items[9].style.display = "block";
            items[10].style.display = "block";
            items[11].style.display = "block";
        }
    }
}
function CopensationClosePopup(e) {
    var $filterListBox = $("#lstFilters").text();
    var $sortListBox = $("#lstSorts").text();
    var ddlSelectedCol = $("#ddlColumns").data("kendoDropDownList").value();
    if (ddlSelectedCol != '' || $filterListBox != '' || $sortListBox != '')
        ddlChanged = true;
    else
        ddlChanged = false;
}
$(document).on('change', '#ddlColumns', function () {
//$("#ddlColumns").change(function () {
    var ddlColumns = $("#ddlColumns").data("kendoDropDownList");
    if (ddlColumns.select() > 0) {
        var wndFilterSort = $(this).closest(".k-window-content").data("kendoWindow");
        var gridName = wndFilterSort.options.gridName;
        var grid = $("#" + gridName).data("kendoGrid");
        var gridColumns = grid.options.dataSource.schema.model.fields;
        var dataType = gridColumns[ddlColumns.dataItem(ddlColumns.select()).Value].type;
        showOrHideOperations(dataType, ddlColumns.dataItem(ddlColumns.select()).Value);
        columnValue = ddlColumns.dataItem(ddlColumns.select()).Value;
    }
    $("#ddlOperations").data("kendoDropDownList").select(0);
    if (ddlColumns.dataItem(ddlColumns.select()).Value == "WorkFlow") {
        $("#txtSearch").hide();
        $("#drpSearchValue").show();
        $("#ddlValues").data("kendoDropDownList").select(0);
        $("#ddlValues").data("kendoDropDownList").enable(false);
    }
    else {
        $("#txtSearch").show();
        $("#drpSearchValue").hide();
        $("#txtSearch").val("");
        $("#txtSearch").prop("disabled", true);
    }
    $("#ddlOperations").data("kendoDropDownList").enable(ddlColumns.select() > 0);
});

//$("#ddlOperations").change(function () {
    $(document).on('change', '#ddlOperations', function () {
    var ddlOperations = $("#ddlOperations").data("kendoDropDownList");
    var index = ddlOperations.select();
    var value = $("#ddlOperations").val();
    if (columnValue == "WorkFlow" && value == "eq") {
        $("#ddlValues").data("kendoDropDownList").enable(true);
    }
    else {
        $("#txtSearch").val("");
        $("#btnAddToList").prop("disabled", !(value == "asc" || value == "desc"));
        $("#txtSearch").prop("disabled", (index > 0 && (value == "asc" || value == "desc")));
    }
});

   // $("#ddlValues").change(function () {
        $(document).on('change', '#ddlValues', function () {
    var ddlValues = $("#ddlValues").data("kendoDropDownList");
    var index = ddlValues.select();
    var value = $("#ddlValues").val();
    if (index > 0)
        $("#btnAddToList").prop("disabled", false);
    else
        $("#btnAddToList").prop("disabled", true);
});

        //$("#txtSearch").keyup(function () {
            $(document).on('keyup', '#txtSearch', function () {
    $("#btnAddToList").prop("disabled", !(this.value.length > 0));
});

          //  $("#btnAddToList").click(function () {
                $(document).on('click', '#btnAddToList', function () {
    addFilterSort();
    var ddlOperations = $("#ddlOperations").data("kendoDropDownList");
    var ddlColumns = $("#ddlColumns").data("kendoDropDownList");
    $("#btnApply").prop("disabled", !($("#lstFilters option").length > 0 || $("#lstSorts option").length > 0));
    //$("#btnApply").data("kendoButton").enable(($("#lstFilters option").length > 0 || $("#lstSorts option").length > 0));
    $("#ddlColumns").data("kendoDropDownList").select(0);
    $("#ddlOperations").data("kendoDropDownList").select(0);
    $("#ddlValues").data("kendoDropDownList").select(0);
    $("#txtSearch").show();
    $("#drpSearchValue").hide();
    $("#txtSearch").val("");
    $("#ddlOperations").data("kendoDropDownList").enable(ddlColumns.select() > 0);
    $("#txtSearch").prop("disabled", !(ddlOperations.select() > 0) || !(ddlColumns.select() > 0));
    $("#btnAddToList").prop("disabled", !((ddlOperations.select() > 0) || (ddlColumns.select() > 0)));
    //$("#btnAddToList").data("kendoButton").enable((ddlOperations.select() > 0) || (ddlColumns.select() > 0));
                });

               // $("#btnApply").click(function () {
                    $(document).on('click', '#btnApply', function () {
                    var wndFilterSort = $(this).closest(".k-window-content").data("kendoWindow");
                    var gridName = wndFilterSort.options.gridName;
                    var grid = $("#" + gridName).data("kendoGrid");
                    var gridFields = grid.options.dataSource.schema.model.fields;
                    var filter = { logic: "and", filters: [] };
                    var sort = [];
                    var filterConditions = [];
                    $("#lstFilters option").each(function () { filterConditions.push(this.value); });
                    filterConditions.sort();
                    var filterIndex = 0;
                    $.each(filterConditions, function (itemIndex) {
                        var filterLogic = { logic: "or", filters: [] };
                        var previsorValue = (itemIndex > 0) ? filterConditions[itemIndex - 1].split("|") : null;
                        var filterValue = this.split("|");
                        var valueObject = (gridFields[filterValue[0]].type == "number") ? Number(filterValue[2]) : filterValue[2];
                        if (previsorValue != null && filterValue[0] == previsorValue[0]) {
                            filter.filters[filterIndex].filters.push({ field: filterValue[0], type: gridFields[filterValue[0]].type, operator: filterValue[1], value: valueObject });
                        }
                        else {
                            filterLogic.filters.push({ field: filterValue[0], type: gridFields[filterValue[0]].type, operator: filterValue[1], value: valueObject });
                            filter.filters.push(filterLogic);
                            if (previsorValue != null && filterValue[0] != previsorValue[0]) {
                                filterIndex++;
                            }
                        }
                    });


                    $.each($("#lstSorts option"), function () {
                        var sortValue = this.value.split("|");
                        sort.push({ field: sortValue[0], type: gridFields[sortValue[0]].type, dir: sortValue[1] })
                    });
                    wndFilterSort.close();
                    var gridPage = 0;
                    var gridGroup = 0;
                    if (grid.pager != undefined) {
                        gridPage = grid.pager.dataSource._pageSize;
                        gridGroup = grid.pager.dataSource._group;
                    }


                    grid.dataSource.query({ page: 1, pageSize: gridPage, filter: filter, sort: sort, group: gridGroup });
                    $("body").css("overflow", "auto");
                    $("#divFilterSort").empty();
                    $("#clearfilter").show();
                    $("#filter").css("display", "none");
                });

                //$("#btnClose").click(function (e) {
                    $(document).on('click', '#btnClose', function (e) {
                    if (!showSaveWarning(e, "ddlChanged")) return false;
                    $(this).closest(".k-window-content").data("kendoWindow").close();
                    $("body").css("overflow", "auto");
                    $("#divFilterSort").empty();
                });
//