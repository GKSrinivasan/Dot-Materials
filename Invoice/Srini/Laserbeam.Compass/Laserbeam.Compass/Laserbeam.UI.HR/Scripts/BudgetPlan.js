// Variables Declarations
var totalSalary;
var applyBudgetChange = false;
var prorateStartDateValue = '';
var prorateEndDateValue = '';
var startDateValue = '';
var endDateValue = '';
var totalSpentValue = 0;
var balanceValue = 0;
var totalCountValue = 0;
var totalSalaryValue = 0;
var budgetAmtValue = 0;
var budgetPctValue = 0;
var adjustBudgetValue = 0;
var adjustBudgetPct = 0;
var meritSpentValue = 0;
var lumpsumSpentValue = 0;
var promotionSpentValue = 0;
var adjustmentSpentValue = 0;
var proratedBudgetValue = 0;
var proratedBudgetPct = 0;
var budgetPercent = 0;
var baseCurrency;
var isProrationChanged = false;
var isPageChanged = false;
var isFilter = false;


$(document).on("click", "#btnClearFilter", function (event) {

    clearFilterSort(true);
    $("#btnClearFilter").hide();
    $("#filter").show();
    isFilter = false;
});
$(document).ready(function () {
    $("#btnCreateGroup").hide();
    $("#btnClearFilter").hide();
    $(document).on("hide.bs.modal", "#divProration", function (e) {
     
        // if (!showSaveWarning(e, "isProrationChanged")) return false;
        if (isProrationVisible == "True") {
            $("#prorateCheckBox").prop("checked", true);
            $("#idProrateTooltip").css("display", "inline");
        }
        else {
            $("#prorateCheckBox").prop("checked", false);
            $("#idProrateTooltip").css("display", "none");
            $("#proratedPopUp").hide();
        }
    });
    if (isProrationVisible == "True") {
        $("#prorateCheckBox").prop("checked", true);
        $("#idProrateTooltip").css("display", "inline");
    }
    else {
        $("#prorateCheckBox").prop("checked", false);
        $("#idProrateTooltip").css("display","none");
        $("#proratedPopUp").hide();
    }
    
    $(document).on("click", "#proratedPopUp", function (event) {
        $.ajax({
            url: "../BudgetPlan/_Proration",
            type: "Get",
            success: function (result) {
                $("#divProration").html(result);
                $("#divProration").modal('show');
            }
        });
    });
    $(document).on("keydown", "#Budgetpercent", function (e) {
        // setPageChanged(true);
        allowDecimalNumberOnlyInput(e);
    });
    $(document).on("keypress", "#Budgetpercent", function (e) {
        var txt = $("#Budgetpercent").val();
        if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
            var substr = txt.split(".")[1].substring(0, 1);
            $("#Budgetpercent").val(txt.split(".")[0] + "." + substr);
        }
    });
    $(document).on("keydown", "#amt", function (e) {
        // setPageChanged(true);
        allowDecimalNumberOnlyInput(e);
    });

    $(document).on("keydown", "#proratedBudgetpercent", function (e) {
        // setPageChanged(true);
        allowDecimalNumberOnlyInput(e);
    });
    $(document).on("keypress", "#proratedBudgetpercent", function (e) {
        var txt = $("#proratedBudgetpercent").val();
        if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
            var substr = txt.split(".")[1].substring(0, 1);
            $("#proratedBudgetpercent").val(txt.split(".")[0] + "." + substr);
        }
    });
    $(document).on("keypress", "#proratedAmt", function (e) {
        var txt = $("#proratedAmt").val();
        if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
            var substr = txt.split(".")[1].substring(0, 1);
            $("#proratedAmt").val(txt.split(".")[0] + "." + substr);
        }
    });
    $(document).on("keydown", "#proratedAmt", function (e) {
        // setPageChanged(true);
        allowDecimalNumberOnlyInput(e);
    });


    // Created By     :   
    // Created Date   :   
    // Comment        :   Budget pct value update

    $(document).on("click", "#btnApplyBudgetPct", function (e) {
        var filterEmployee="";
        var selectedCurrencyCodeNum;
        var grid = $("#grdBudgetPlan").data("kendoGrid");
        if (grid.dataSource._filter != undefined) {
            if (grid.dataSource._filter.length != 0 || grid.dataSource._sort.length != 0) {
                var data = grid.dataSource._view;
                var length = grid.dataSource._view.length;
                for (var i = 0; i < length; i++) {
                    filterEmployee = filterEmployee + "," + data[i].ManagerNum
                }
            }
        }
        if (isProrationVisible == "False") {
            if (applyBudgetChange == true && $("#Budgetpercent").val() != "") {
                var budgetPct = (getNumberValue($("#Budgetpercent").val(), preferredCulture) == null) ? formatGridValue(getNumberValue($("#Budgetpercent").val(), preferredCulture), "BudgetPct", "Percentage", preferredCulture) : +(getNumberValue($("#Budgetpercent").val(), preferredCulture)).toFixed(2);
                $("#Budgetpercent").text(budgetPct + "%");
                var token = $('input[name="__RequestVerificationToken"]').val();
                $.ajax({
                    url: "../BudgetPlan/PutBudgetPct",
                    type: "post",
                    data: {
                        __RequestVerificationToken: token, BudgetPercent: budgetPct, isProration: false, filterEmployee: filterEmployee,
                    },
                    success: function (result) {
                        $("#grdBudgetPlan").data("kendoGrid").dataSource.read();
                        applyBudgetChange = false;
                        Successmessage("Great ! you have successfully distributed the budget for all managers");

                        objChangeFlag = false;

                    }
                });
            }
        }
        if (isProrationVisible == "True") {
            if (applyBudgetChange == true && $("#proratedBudgetpercent").val() != "") {
                var budgetPct = formatGridValue(roundingRule(getNumberValue($("#proratedBudgetpercent").val(), preferredCulture), "BudgetPct", "Percentage"), "BudgetPct", "Percentage", preferredCulture);
                $("#proratedBudgetpercent").text(budgetPct + "%");
                var token = $('input[name="__RequestVerificationToken"]').val();
                $.ajax({
                    url: "../BudgetPlan/PutBudgetPct",
                    type: "post",
                    data: {
                        __RequestVerificationToken: token, BudgetPercent: budgetPct, isProration: true, filterEmployee: filterEmployee,
                    },
                    success: function (result) {
                        $("#grdBudgetPlan").data("kendoGrid").dataSource.read();
                        applyBudgetChange = false;
                        objChangeFlag = false;
                        Successmessage("Great ! you have successfully distributed the budget for all managers");
                    }
                });
            }
        }
    });
    $(document).on("change", "#Budgetpercent", function (e) {
        applyBudgetChange = true;
        //setPageChanged(true);
        var dataValue = getNumberValue($(e.target).val());
        if (dataValue > 999) {
            showAlert("Value exceeds the maximum limit");
            $(this).val('');
        }
        else {
            calculateBudgetAmt(dataValue);
        }
        var len = $(e.target).val().length;
        var index = $(e.target).val().indexOf('.');
        if (index > 0) {
            var CharAfterdot = (len) - (index + 1);
            if (CharAfterdot > 4) {
                $(this).val(dataValue.toFixed(4));
            }
        }
        $("[id=txtAdjustedBudget]").attr('disabled', 'disabled');
        $("[id=txtAdjustedBudgetPCT]").attr('disabled', 'disabled');

        $("#btnApplyBudgetPct").show();
        $("#btnCreateGroup").hide();
        Warningmessage(" Click on Distribute budget button to release the allocated budget to managers. ");
        objChangeFlag = true;
    });
    $(document).on("change", "#amt", function (e) {
        applyBudgetChange = true;
        var dataValue = getNumberValue($(e.target).val());
        calculateBudgetPct(dataValue);
        $("[id=txtAdjustedBudget]").attr('disabled', 'disabled');
        $("[id=txtAdjustedBudgetPCT]").attr('disabled', 'disabled');

        $("#btnApplyBudgetPct").show();
        $("#btnCreateGroup").hide();
        Warningmessage(" Click on Distribute budget button to release the allocated budget to managers. ");
    });

    $(document).on("change", "#proratedBudgetpercent", function (e) {
        applyBudgetChange = true;
        //setPageChanged(true);
        var dataValue = getNumberValue($(e.target).val());
        if (dataValue > 999) {
            showAlert("Value exceeds the maximum limit");
            $(this).val('');
        }
        else {
            calculateProratedBudgetAmt(dataValue);
        }
        var len = $(e.target).val().length;
        var index = $(e.target).val().indexOf('.');
        if (index > 0) {
            var CharAfterdot = (len) - (index + 1);
            if (CharAfterdot > 4) {
                $(this).val(dataValue.toFixed(4));
            }
        }
        $("[id=txtAdjustedBudget]").attr('disabled', 'disabled');
        $("[id=txtAdjustedBudgetPCT]").attr('disabled', 'disabled');

        $("#btnApplyBudgetPct").show();
        $("#btnCreateGroup").hide();
        Warningmessage(" Click on Distribute budget button to release the allocated budget to managers. ");
        objChangeFlag = true;
    });
    $(document).on("change", "proratedAmt", function (e) {
        applyBudgetChange = true;
        var dataValue = getNumberValue($(e.target).val());
        calculateProratedBudgetPct(dataValue);
        $("[id=txtAdjustedBudget]").attr('disabled', 'disabled');
        $("[id=txtAdjustedBudgetPCT]").attr('disabled', 'disabled');

        $("#btnApplyBudgetPct").show();
        $("#btnCreateGroup").hide();
        Warningmessage(" Click on Distribute budget button to release the allocated budget to managers. ");
        objChangeFlag = true;
    });


    $(document).on("keydown", "[id$=txtAdjustedBudgetPCT]", function (e) {

        allowDecimalNumberOnlyInput(e);
    });
    $(document).on("keypress", "[id$=txtAdjustedBudgetPCT]t", function (e) {
        var txt = $("[id$=txtAdjustedBudgetPCT]").val();
        if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
            var substr = txt.split(".")[1].substring(0, 1);
            $("[id$=txtAdjustedBudgetPCT]").val(txt.split(".")[0] + "." + substr);
        }
    });
    $(document).on("keypress", "[id$=txtAdjustedBudget]", function (e) {
        var txt = $("[id$=txtAdjustedBudget]").val();
        if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
            var substr = txt.split(".")[1].substring(0, 1);
            $("[id$=txtAdjustedBudget]").val(txt.split(".")[0] + "." + substr);
        }
    });
    $(document).on("keydown", "[id$=txtAdjustedBudget]", function (e) {
        // setPageChanged(true);
        allowDecimalNumberOnlyInput(e);
    });
    $(document).on("click", "#btnCreateGroup", function (e) {
        var gridUser = $("#grdBudgetPlan").data("kendoGrid");
        gridUser.dataSource.sync();
        objChangeFlag = false;
        $("[id=Budgetpercent]").attr('disabled', false);
        $("[id=amt]").attr('disabled', false);
        $("[id=proratedBudgetpercent]").attr('disabled', false);
        $("[id=proratedAmt]").attr('disabled', false);
        Successmessage("Great ! you have successfully distributed the budget for all managers");
    });

});
$(document).on('click', '#filter', function () {
    $("body").css("overflow", "hidden");
    var isProratedBudgetEnable = $("th[data-field='ProratedBudget']").css("display") == "none" ? false : true;
    var isBudgetEnable = $("th[data-field='Budget']").css("display") == "none" ? false : true;
    $.ajax({
        url: '../BudgetPlan/_FilterSort',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        data : {IsProratedBudgetEnable : isProratedBudgetEnable, IsBudgetEnable : isBudgetEnable},
        success: function (result) {
            $("#wndFilterSort").html(result);
            var wndFilterSort = $("#wndFilterSort").data("kendoWindow");
            wndFilterSort.options.gridName = "grdBudgetPlan";
            wndFilterSort.center().open();
            $("#filterseperator").css("display", "inline");
            //$("#btnClearFilter").show();
            isFilter = true;
            //$("#clearfilter").css("display", "inline");

        }
    });
});

$(document).on("click", "#prorationDaily", function (event) {
    prorationType = "Daily";
    $("#txtProrationDuration").val(365);
    $("#prorationSample").text("365 days")
    $("#prorationDaily").removeClass("btn-deactive");
    $("#prorationDaily").addClass("btn-active");
    $("#prorationWeekly").addClass("btn-deactive");
    $("#prorationMonthly").addClass("btn-deactive");
    $("#prorationType").val("Daily");
    $("#datePerMonth").html("");
    isProrationChanged = true;

});
$(document).on("click", "#prorationWeekly", function (event) {
    prorationType = "Weekly";
    $("#txtProrationDuration").val(52)
    $("#prorationSample").text("52 weeks");
    $("#prorationWeekly").removeClass("btn-deactive");
    $("#prorationWeekly").addClass("btn-active");
    $("#prorationDaily").addClass("btn-deactive");
    $("#prorationMonthly").addClass("btn-deactive");
    $("#prorationType").val("Weekly");
    $("#datePerMonth").html("");
    isProrationChanged = true;
});
$(document).on("click", "#prorationMonthly", function (event) {
    prorationType = "Monthly";
    $("#txtProrationDuration").val(12);
    $("#prorationSample").text("12 months");
    $("#prorationMonthly").removeClass("btn-deactive");
    $("#prorationMonthly").addClass("btn-active");
    $("#prorationDaily").addClass("btn-deactive");
    $("#prorationWeekly").addClass("btn-deactive");
    $("#prorationType").val("Monthly");
    $("#datePerMonth").html("");
    var currentValue = $("#txtProrationDatesPerMonth").val();
    $("#txtProrationDatesPerMonth").val((currentValue == "") ? 16 : currentValue);
    isProrationChanged = true;
});
$(document).on('change', '#meritChkBox', function () {
    if (this.checked) {
        isMerit = true;
        $("#isMerit").val("true");
    }
    else {
        isMerit = false;
        $("#isMerit").val("false");
    }
    var StartDate = $("#prorationStartDate").data("kendoDatePicker")._oldText;
    $("#processStartDateValue").val(StartDate);
    var EndDate = $("#prorationEndDate").data("kendoDatePicker")._oldText;
    $("#processEndDateValue").val(EndDate);
});

$(document).on("change", "[id$=txtAdjustedBudget]", function (e) {
    $("[id=Budgetpercent]").attr('disabled', 'disabled');
    $("[id=amt]").attr('disabled', 'disabled');
    $("[id=proratedBudgetpercent]").attr('disabled', 'disabled');
    $("[id=proratedAmt]").attr('disabled', 'disabled');
    $("#btnApplyBudgetPct").hide();
    $("#btnCreateGroup").show();
    setPageChanged(true);
    var grdBudgetPlan = $("#grdBudgetPlan").data("kendoGrid");
    var row = $(this).closest("tr");
    var rowData = grdBudgetPlan.dataItem(row);
    var rowIndex = $(row).index();
    var oldAdjustedAmt = rowData.AdjustedBudget;
    rowData.AdjustedBudget = Math.round(getNumberValue(this.value, preferredCulture));
    rowData.AdjustedBudgetPct = (((rowData.AdjustedBudget / rowData.BaseCurrentSalary) * 100));
    rowData.AdjustedBudgetRoundedPct = (((rowData.AdjustedBudget / rowData.BaseCurrentSalary) * 100)).toFixed(2);
    //rowData.AdjustedBudgetPct = (((getNumberValue(Math.round(this.value), preferredCulture) / rowData.BaseCurrentSalary) * 100)).toFixed(2);
    var newAdjustedAmt = rowData.AdjustedBudget;
    rowData.Balance = rowData.AdjustedBudget - rowData.Spent;
    rowData.dirty = true;
    grdBudgetPlan.refresh();
    if (oldAdjustedAmt < newAdjustedAmt) {
        var totalAdjustedBudget = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget + (newAdjustedAmt - oldAdjustedAmt);
        var adjustedPct = (totalAdjustedBudget / grdBudgetPlan.dataSource._data[0].TotalSalary) * 100;
        $("#adjustedBudgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#adjustedBudgetPct").text((formatGridValue((adjustedPct.toFixed(2)), 'BudgetPct', 'Percentage', preferredCulture) + "%"));
        $("#budgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#budgetPct").text(formatGridValue(Math.round(adjustedPct), 'BudgetPct', 'Percentage', preferredCulture) + "%");
    }
    if (oldAdjustedAmt > newAdjustedAmt) {
        var totalAdjustedBudget = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget - (oldAdjustedAmt - newAdjustedAmt);
        var adjustedPct = (totalAdjustedBudget / grdBudgetPlan.dataSource._data[0].TotalSalary) * 100;
        $("#adjustedBudgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#adjustedBudgetPct").text((formatGridValue((adjustedPct.toFixed(2)), 'BudgetPct', 'Percentage', preferredCulture) + "%"));
        $("#budgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#budgetPct").text(formatGridValue(Math.round(adjustedPct), 'BudgetPct', 'Percentage', preferredCulture) + "%");
    }
    Warningmessage(" Click on Distribute budget button to release the updated budget. ");
    objChangeFlag = true;
});
$(document).on("change", "[id$=txtAdjustedBudgetPCT]", function (e) {
    $("[id=Budgetpercent]").attr('disabled', 'disabled');
    $("[id=amt]").attr('disabled', 'disabled');
    $("[id=proratedBudgetpercent]").attr('disabled', 'disabled');
    $("[id=proratedAmt]").attr('disabled', 'disabled');
    $("#btnApplyBudgetPct").hide();
    $("#btnCreateGroup").show();

    setPageChanged(true);
    var grdBudgetPlan = $("#grdBudgetPlan").data("kendoGrid");
    var row = $(this).closest("tr");
    var rowData = grdBudgetPlan.dataItem(row);
    var oldAdjustedAmt = rowData.AdjustedBudget;
    rowData.AdjustedBudgetPct = +(((getNumberValue(this.value) == null) ? Number(this.value) : getNumberValue(this.value)).toFixed(2));// roundingRule(getNumberValue(this.value, preferredCulture), "BudgetPct", "Percentage");
    rowData.AdjustedBudgetRoundedPct = rowData.AdjustedBudgetPct;
    rowData.AdjustedBudget = (rowData.BaseCurrentSalary * rowData.AdjustedBudgetPct) / 100;
    var newAdjustedAmt = rowData.AdjustedBudget;
    rowData.dirty = true;
    rowData.Balance = rowData.AdjustedBudget - rowData.Spent;
    grdBudgetPlan.refresh();
    if (oldAdjustedAmt < newAdjustedAmt) {
        var totalAdjustedBudget = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget + (newAdjustedAmt - oldAdjustedAmt);
        //var totalAdjustedBudget = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget;
        var adjustedPct = (totalAdjustedBudget / grdBudgetPlan.dataSource._data[0].TotalSalary) * 100;
        $("#adjustedBudgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
       $("#adjustedBudgetPct").text((formatGridValue((adjustedPct.toFixed(2)), 'BudgetPct', 'Percentage', preferredCulture) + "%"));
        $("#budgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#budgetPct").text(formatGridValue(adjustedPct, 'BudgetPct', 'Percentage', preferredCulture) + "%");
    }
    if (oldAdjustedAmt > newAdjustedAmt) {
        var totalAdjustedBudget = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget - (oldAdjustedAmt - newAdjustedAmt);
        //var totalAdjustedBudget = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget;
        var adjustedPct = (totalAdjustedBudget / grdBudgetPlan.dataSource._data[0].TotalSalary) * 100;
        $("#adjustedBudgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#adjustedBudgetPct").text((formatGridValue((adjustedPct.toFixed(2)), 'BudgetPct', 'Percentage', preferredCulture) + "%"));
        $("#budgetAmt").text(formatGridValue(totalAdjustedBudget, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#budgetPct").text(formatGridValue(adjustedPct, 'BudgetPct', 'Percentage', preferredCulture) + "%");
    }
    Warningmessage(" Click on Distribute budget button to release the updated budget. ");
    objChangeFlag = true;
});
$("#btnFilter").click(function () {
    var wndFilterSort = $("#wndFilterSort").data("kendoWindow");
    wndFilterSort.options.gridName = "grdBudgetPlan";
    wndFilterSort.center().open();
});
$(document).on('click', '#prorateCheckBox', function (e) {

    if (!showSaveWarning(e)) return false;
    if (this.checked) {
        $.ajax({
            url: "../BudgetPlan/_ProrationPopUp",
            type: "Get",
            success: function (result) {
                $("#divProration").html(result);
                $("#divProration").modal('show');
            }
        });
      
    }
    else {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '../BudgetPlan/ClearProrationValues',
            type: "Post",
            data: {
                __RequestVerificationToken: token, isProration: "NO"
            },
            success: function (result) {
                $("#grdBudgetPlan").data("kendoGrid").dataSource.read();
                $("#idProrateTooltip").css("display", "none");
                $("#proratedPopUp").hide();
            }
        });
    }
});


$(document).on("click", "#prorationSave", function (event) {

    var token = $('input[name="__RequestVerificationToken"]').val();
    var prorateStartDate = $("#prorationStartDate").val();
    var prorateEndDate = $("#prorationEndDate").val();
    var prorationDuration = $("#txtProrationDuration").val();
    var prorationDays = $("#txtProrationDays").val();
    $.ajax({
        url: "../BudgetPlan/_Proration",
        type: "Post",
        data: {
            __RequestVerificationToken: token, prorateStartDate: prorateStartDate, prorateEndDate: prorateEndDate,
            prorationDuration: prorationDuration, prorationDays: prorationDays, prorationType: prorationType, isMerit: isMerit
        },
        success: function (result) {
            $("#divProration").modal('hide');
            isProrationVisible = "True";
            $("#idProrateTooltip").css("display", "inline");
            $("#proratedPopUp").show();
            $("#grdBudgetPlan").data("kendoGrid").dataSource.read();
            $("[id=txtAdjustedBudget]").attr('disabled', false);
            $("[id=txtAdjustedBudgetPCT]").attr('disabled', false);
            $("#prorateCheckBox").prop("checked", true);
           
        }
    });
});
$(document).on("click", "#budgetExport", function (event) {
    var currencyDrop = $("#ddlLocalCurrenciesBudget").data("kendoDropDownList");
    var selectedData = currencyDrop.dataItem($("#ddlLocalCurrenciesBudget").data("kendoDropDownList").select());
    selectedCurrencyCodeNum = $("#selectedCurrencyNum").val();
    selectedCurrencyCodeNum = (selectedCurrencyCodeNum == 0) ? selectedData.CurrencyNum : selectedCurrencyCodeNum
    var token = $('input[name="__RequestVerificationToken"]').val();
    var form = $("<form action='" + '../BudgetPlan/' + 'BudgetPlanExport' + "' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + token + "' />");
    form.append("<input type='text' name='selectedCurrencyNum' value='" + selectedCurrencyCodeNum + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();

});

//Functions 
function budgetPlanDataBound() {
    var grdBudgetPlan = $("#grdBudgetPlan").data("kendoGrid");
    var data = grdBudgetPlan.dataSource.view();
    if (data.length > 0) {
        var baseCurrencyCode = data[0].BaseCurrencyCode;
        var selectedCurrencyCode = data[0].SelectedCurrencyCode;
        var SelectedExchangeRate = data[0].SelectedExchangeRate;
        if (data[0].ProrationVisible === false) {
            grdBudgetPlan.hideColumn("ProratedBudget");
            grdBudgetPlan.hideColumn("ProratedBudgetPct");
            grdBudgetPlan.showColumn("Budget");
            grdBudgetPlan.showColumn("BudgetPct");
            $("#proratedAmtDiv").hide();
            $("#proratedPctDiv").hide();
            $("#budgetAmtDiv").show();
            $("#budgetPctDiv").show();
            isProrationVisible = "False";
            $("#prorateCheckBox").prop("checked", false);
            $("#idProrateTooltip").css("display", "none");
            $("#proratedPopUp").hide();

        }
        if (data[0].ProrationVisible === true) {
            grdBudgetPlan.hideColumn("Budget");
            grdBudgetPlan.hideColumn("BudgetPct");
            grdBudgetPlan.showColumn("ProratedBudget");
            grdBudgetPlan.showColumn("ProratedBudgetPct");
            $("#budgetAmtDiv").hide();
            $("#budgetPctDiv").hide();
            $("#proratedAmtDiv").show();
            $("#proratedPctDiv").show();
            isProrationVisible = "True";
            $("#prorateCheckBox").prop("checked", true);
            $("#idProrateTooltip").css("display", "inline");
            $("#proratedPopUp").show();
        }

        var rows = grdBudgetPlan.tbody.children('tr');
        var data = grdBudgetPlan.dataSource.view();
        //
        if (isFilter == false) {
            baseCurrency = data[0].BaseCurrency;
            totalSalary = grdBudgetPlan.dataSource._data[0].TotalSalary;
            prorateStartDateValue = data[0].ProrationStartDate;
            prorateEndDateValue = data[0].ProrationEndDate;
            startDateValue = data[0].StartDate;
            endDateValue = data[0].EndDate;
            budgetPercent = data[0].BudgetPercent;
            totalSalaryValue = grdBudgetPlan.dataSource._data[0].TotalSalary;
            budgetAmtValue = grdBudgetPlan.dataSource._data[0].TotalBudget;
            budgetPctValue = (totalSalaryValue==0)?0:(budgetAmtValue / totalSalaryValue) * 100;
            adjustBudgetValue = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget;
            adjustBudgetPct = (totalSalaryValue == 0) ? 0 : (adjustBudgetValue / totalSalaryValue) * 100;
            proratedBudgetValue = grdBudgetPlan.dataSource._data[0].TotalProratedBudget;
            proratedBudgetPct = (totalSalaryValue == 0) ? 0 : (proratedBudgetValue / totalSalaryValue) * 100;
            totalSpentValue = grdBudgetPlan.dataSource._data[0].TotalSpent;
            balanceValue = adjustBudgetValue - totalSpentValue;
            totalCountValue = grdBudgetPlan.dataSource._data[0].TotalEmployeeCount;
            meritSpentValue = grdBudgetPlan.dataSource._data[0].TotalMeritSpent;
            lumpsumSpentValue = grdBudgetPlan.dataSource._data[0].TotalLumpSumSpent;
            promotionSpentValue = grdBudgetPlan.dataSource._data[0].TotalPromotionSpent;
            adjustmentSpentValue = grdBudgetPlan.dataSource._data[0].TotalAdjustmentSpent;
            //
            budgetValuesRefresh();
            
            if (isEnableMerit == 'False')
                $("#lgtMerit").hide();
            if (isEnableLumpSum == 'False')
                $("#lgtLumpSum").hide();
            if (isEnablePromotion == 'False')
                $("#lgtPromotion").hide();
            if (isEnableAdjustment == 'False')
                $("#lgtAdjustment").hide();

            if ($("#grdBudgetPlan").data("kendoGrid").dataSource._filter != undefined) {
                $("#btnClearFilter").show();
            }
            if (baseCurrencyCode == selectedCurrencyCode) {
                $("#conversionRate").hide();
                // $("#conversionRate").text("");
            }
            else {
                $("#conversionRate").text("1 " + baseCurrencyCode + " = " + (1 * SelectedExchangeRate) + " " + selectedCurrencyCode);
                $("#conversionRate").show();
            }
        }
        else if (isFilter == true)
        {
            totalSalaryValue = grdBudgetPlan.dataSource._data[0].TotalSalary;
            adjustBudgetValue = grdBudgetPlan.dataSource._data[0].TotalAdjustedBudget;
            adjustBudgetPct = (totalSalaryValue == 0) ? 0 : (adjustBudgetValue / totalSalaryValue) * 100;
            averageBudgetPct = grdBudgetPlan.dataSource._data[0].AverageBudgetPct;
            $("#adjustedBudgetPct").text(((formatGridValue(((averageBudgetPct).toFixed(2)), 'BudgetPct', 'Percentage', preferredCulture) + "%")));
            $("#budgetPct").text(formatGridValue(adjustBudgetPct, 'BudgetPct', 'Percentage', preferredCulture) + "%");
        }
    }
    else if (data.length == 0) {
        var budgetPlanGrid = $("#grdBudgetPlan").data("kendoGrid");
        if (budgetPlanGrid.dataSource._view.length == 0) {
            var colCount = $("#grdBudgetPlan").find('th').length;
            $("#grdBudgetPlan").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center;background-color:#6686C4;color:white !important;">No Results Found!</td></tr>');           
        }
    }

}
function budgetValuesRefresh() {
    totalSalary = totalSalary;
    if (prorateStartDateValue != "")
        $("#idStartDate").text(startDateValue);
    else
        $("#idStartDate").text("");
    if (prorateEndDateValue != "")
        $("#idEndDate").text(endDateValue);
    else
        $("#idEndDate").text("");
    //$("#totalSpentAmt").text(formatGridValue(totalSpentValue, 'BudgetAmt', 'Dollar', preferredCulture));
    //$("#balanceAmt").text(formatGridValue(balanceValue, 'BudgetAmt', 'Dollar', preferredCulture));
    $("#totalSpentAmt").text(formatBudgetCurrency(totalSpentValue, 'c0', preferredCulture));
    $("#balanceAmt").text(formatBudgetCurrency(balanceValue, 'c0', preferredCulture));
    $("#totalCount").text(totalCountValue);
    // $("#totalSalary").text(formatGridValue(totalSalaryValue, 'BudgetAmt', 'Dollar', preferredCulture));

    $("#totalSalary").text(formatBudgetCurrency(totalSalaryValue, 'c0', preferredCulture));
    // $("#budgetAmt").text(formatGridValue(adjustBudgetValue, 'BudgetAmt', 'Dollar', preferredCulture));
    $("#budgetAmt").text(formatBudgetCurrency(adjustBudgetValue, 'c0', preferredCulture));
    $("#budgetPct").text(formatGridValue(adjustBudgetPct, 'BudgetPct', 'Percentage', preferredCulture) + "%");
    $("#amt").val(formatGridValue(budgetAmtValue, 'BudgetAmt', 'Dollar', preferredCulture));
    //$("#Budgetpercent").val(formatGridValue(budgetPercent, 'BudgetPct', 'Percentage', preferredCulture));
    $("#Budgetpercent").val((getNumberValue(budgetPercent, preferredCulture).toFixed(2)));
    $("#adjustedBudgetAmt").text(formatGridValue(adjustBudgetValue, 'BudgetAmt', 'Dollar', preferredCulture));
    $("#adjustedBudgetPct").text(((formatGridValue(((adjustBudgetPct).toFixed(2)), 'BudgetPct', 'Percentage', preferredCulture) + "%")));
    $("#proratedAmt").val(formatGridValue(proratedBudgetValue, 'BudgetAmt', 'Dollar', preferredCulture));
    $("#proratedBudgetpercent").val(formatGridValue(budgetPercent, 'BudgetPct', 'Percentage', preferredCulture));   
    $("#meritSpent").text(formatBudgetCurrency(meritSpentValue, 'c0', preferredCulture));
    $("#lumpsumSpent").text(formatBudgetCurrency(lumpsumSpentValue, 'c0', preferredCulture));
    $("#promotionSpent").text(formatBudgetCurrency(promotionSpentValue, 'c0', preferredCulture));
    $("#adjustmentSpent").text(formatBudgetCurrency(adjustmentSpentValue, 'c0', preferredCulture));
    createGauge();
}

function onUpdateRequestEnd(e) {
    if (e.type == "update") {
        // showMessage(e.response.Message);
        $("#grdBudgetPlan").data("kendoGrid").dataSource.read();
    }
}
function gridUser_sync() {
    var gridUser = $("#grdBudgetPlan").data("kendoGrid");
    gridUser.dataSource.read();
}

function BudgetGridData() {
    var selectedCurrencyCodeNum;
    var token = $('input[name="__RequestVerificationToken"]').val();
    var currencyDrop = $("#ddlLocalCurrenciesBudget").data("kendoDropDownList");
    var selectedData = currencyDrop.dataItem($("#ddlLocalCurrenciesBudget").data("kendoDropDownList").select());
    selectedCurrencyCodeNum = $("#selectedCurrencyNum").val();
    selectedCurrencyCodeNum=(selectedCurrencyCodeNum == 0) ? selectedData.CurrencyNum : selectedCurrencyCodeNum
    return { __RequestVerificationToken: token, selectedCurrencyNum: selectedCurrencyCodeNum   };
}


function formatGridValue(dataValue, keyValue, subcategory, employeeCulture) {
    if (dataValue == null) return '';

    if (subcategory == 'Percentage') {
        var cultureFormat = getCultureFormat(keyValue, subcategory);
        var result = formatValue(dataValue, cultureFormat, employeeCulture);
        result = result.replace(' ', '');
        return result;
    }
    else {
        var cultureFormat = getCultureFormat(keyValue, subcategory);
        var result = formatValue(dataValue, cultureFormat, employeeCulture);
        return result;
    }
}

function getCultureFormat(keyValue, subcategory) {
    if (subcategory == "Percentage") {
        if (keyValue == "BudgetPct") {
            var type = BudgetPlanConfiguration.RoundingBudgetPercentage.toLowerCase();
            var configValue = BudgetPlanConfiguration.DecimalBudgetPercentage;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('n' + noOfDecimalPlaces).toString();
        }
    }
    if (subcategory == "Dollar") {
        if (keyValue == "BudgetAmt") {
            var type = BudgetPlanConfiguration.RoundingBudgetDoller.toLowerCase();
            var configValue = BudgetPlanConfiguration.DecimalBudgetDoller;
            var noOfDecimalPlaces = getnoOfDecimalPlaces(configValue);
            return ('c' + noOfDecimalPlaces).toString();
        }
    }
}


function getnoOfDecimalPlaces(configValue) {
    var noOfDecimalPlaces = decimalPlaces(configValue);
    return noOfDecimalPlaces;
}


function roundingRule(dataValue, keyValue, subcategory) {
    if (subcategory == "Percentage") {
        if (keyValue == "BudgetPct") {
            var type = BudgetPlanConfiguration.RoundingBudgetPercentage.toLowerCase();
            var configValue = BudgetPlanConfiguration.DecimalBudgetPercentage;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN || isNaN(roundedValue)) ? 0 : roundedValue;
        }
    }
    if (subcategory == "Dollar") {
        if (keyValue == "BudgetAmt") {
            var type = BudgetPlanConfiguration.RoundingBudgetDoller.toLowerCase();
            var configValue = BudgetPlanConfiguration.DecimalBudgetDoller;
            var roundedValue = roundingNumbers(type, dataValue, configValue);
            return (roundedValue == undefined || roundedValue == null || roundedValue == "" || roundedValue == NaN || isNaN(roundedValue)) ? 0 : roundedValue;
        }
    }
}
function getdecimalPlaces(configValue) {
    var noOfDecimalPlaces = decimalPlaces(configValue);
    if (noOfDecimalPlaces <= 0) {
        var configValueLength = configValue.toString().replace(/[^0]/g, "").length;
        noOfDecimalPlaces = -Math.abs(configValueLength);
    }
    return noOfDecimalPlaces;
}

function decimalPlaces(configValue) {
    var regexp = ('' + configValue).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
    if (!regexp) {
        return 0;
    }

    return Math.max(0,
        // Number of digits right of decimal point.
       (regexp[1] ? regexp[1].length : 0)
       // Adjust for scientific notation.
       - (regexp[2] ? +regexp[2] : 0));
}

function roundingNumbers(type, dataValue, configValue) {
  
    var decimalPlaces = getdecimalPlaces(configValue);
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
    }
}
function getBalance(value, format, culture) {
    if (value < 0) {
        return "<span style='color:red'>" + formatGridValue(roundingRule(value, 'BudgetAmt', 'Dollar'), 'BudgetAmt', 'Dollar', preferredCulture) + "</span>";
    }
    else {
        return "<span>" + formatGridValue(roundingRule(value, 'BudgetAmt', 'Dollar'), 'BudgetAmt', 'Dollar', preferredCulture) + "</span>";
    }
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

function onChange() {
    var selectedData = $("#ddlLocalCurrenciesBudget").data("kendoDropDownList").dataItem($("#ddlLocalCurrenciesBudget").data("kendoDropDownList").select());
    $("#selectedCurrencyNum").val(selectedData.CurrencyNum);
    preferredCulture = selectedData.CultureCode;
    if (ddlcurrencyDropOldvalue != selectedData.CurrencyNum) {
        // onMultiCurrencyDropDownChange();
        ddlcurrencyDropOldvalue = selectedData.CurrencyNum;
        ddlCurrencyCulture = selectedData.CultureCode;
        ddlExchangeRate = selectedData.ExchangeRate;
        ddlCurrencyCodeNum = selectedData.CurrencyNum;
    }
    //  getBudgetData();
    $("#grdBudgetPlan").data("kendoGrid").dataSource.read();
    //
}
function dataBound() {
    var currencyDrop = $("#ddlLocalCurrenciesBudget").data("kendoDropDownList");
    if (DefaultCurrencyNum != '0')
        currencyDrop.value(DefaultCurrencyNum);
    else
        currencyDrop.value(0);

    ddlcurrencyDropOldvalue = currencyDrop.value();
    var selectedData = currencyDrop.dataItem($("#ddlLocalCurrenciesBudget").data("kendoDropDownList").select());

    if (selectedData != undefined) {
        ddlCurrencyCodeNum = selectedData.CurrencyNum;
        ddlCurrencyCulture = (selectedData.CultureCode != undefined) ? selectedData.CultureCode : 'en-US';
        ddlExchangeRate = (selectedData.ExchangeRate != undefined) ? selectedData.ExchangeRate : 1;
    }

}

function calculateBudgetAmt(dataValue) {
    var budgetamt = (dataValue / 100) * totalSalary;
    $("#amt").val(formatGridValue(budgetamt, 'BudgetAmt', 'Dollar', preferredCulture));
    //$("#Budgetpercent").val(formatGridValue(+(dataValue).toFixed(2), 'BudgetPct', 'Percentage', preferredCulture));
    $("#Budgetpercent").val(((getNumberValue(dataValue, preferredCulture)) == null) ? formatGridValue(dataValue, 'BudgetPct', 'Percentage', preferredCulture) : +(getNumberValue(dataValue, preferredCulture).toFixed(2)));
}
function calculateBudgetPct(dataValue) {

    var budgetPct = ((dataValue / totalSalary) * 100).toFixed(2);
    if (budgetPct > 999) {
        showAlert("Value exceeds the maximum limit");
        //$(this).val('');
    }
    else {
        $("#amt").val(formatGridValue(dataValue, 'BudgetAmt', 'Dollar', preferredCulture));
        //$("#Budgetpercent").val(formatGridValue(budgetPct, 'BudgetPct', 'Percentage', preferredCulture));
        $("#Budgetpercent").val(((getNumberValue(dataValue, preferredCulture)) == null) ? formatGridValue(budgetPct, 'BudgetPct', 'Percentage', preferredCulture) : +(getNumberValue(budgetPct, preferredCulture).toFixed(2)));
    }
}
function calculateProratedBudgetAmt(dataValue) {
    var proratedBudgetamt = (dataValue / 100) * totalSalary;
    $("#proratedAmt").val(formatGridValue(proratedBudgetamt, 'BudgetAmt', 'Dollar', preferredCulture));
    $("#proratedBudgetpercent").val(formatGridValue(dataValue.toFixed(2), 'BudgetPct', 'Percentage', preferredCulture));
}
function calculateProratedBudgetPct(dataValue) {
    var proratedbudgetPct = ((dataValue / totalSalary) * 100).toFixed(2);
    if (proratedbudgetPct > 999) {
        showAlert("Value exceeds the maximum limit");
        //$(this).val('');
    }
    else {
        $("#proratedAmt").val(formatGridValue(dataValue, 'BudgetAmt', 'Dollar', preferredCulture));
        $("#proratedBudgetpercent").val(formatGridValue(proratedbudgetPct, 'BudgetPct', 'Percentage', preferredCulture));
    }
}
function createGauge(labelPosition) {
    var a = ((meritSpentValue / adjustBudgetValue) * 100).toFixed(2);
    var b = ((lumpsumSpentValue / adjustBudgetValue) * 100).toFixed(2);
    var c = ((promotionSpentValue / adjustBudgetValue) * 100).toFixed(2);
    var d = parseFloat(a) + parseFloat(b);
    var e = parseFloat(a) + parseFloat(b) + parseFloat(c);
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
                position: labelPosition || "inside",
                visible: false
            },
            ranges: [
                {
                    from: 0,
                    to: a,
                    color: "#71c055"
                }, {
                    from: a,
                    to: d,
                    color: "#fec679"
                }, {
                    from: d,
                    to:e,
                    color: "#71c4ee"
                }
            ]
        }
    });
}

$("#Budgetpercent").on("keypress", function (evt) {
    var $txtBox = $(this);
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;
    else {
        var len = $txtBox.val().length;
        var index = $txtBox.val().indexOf('.');

        if (index > 0 && charCode == 46) {
            return false;
        }
        if (index > 0) {
            var charAfterdot = (len + 1) - index;
            if (charAfterdot > 3) {
                return false;
            }
        }
    }
    return $txtBox; //for chaining
});
function formatBudgetCurrency(value, format, culture) {
    if (value == null) return '';
    if (isNaN(value)) return '';
    var selectedData = $("#ddlLocalCurrenciesBudget").data("kendoDropDownList").dataItem($("#ddlLocalCurrenciesBudget").data("kendoDropDownList").select());
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
$(document).on("click", "#prorationCancel", function (e) {
    $("#divProration").modal('hide');
    if (isProrationVisible == "True") {
        $("#prorateCheckBox").prop("checked", true);
    }
    else {
        $("#prorateCheckBox").prop("checked", false);
        $("#idProrateTooltip").css("display", "none");
        $("#proratedPopUp").hide();
    }
});
function closeAfterProration() {
    $("#grdBudgetPlan").data("kendoGrid").dataSource.read();
    $("#divProration").modal('hide');
    Successmessage("Saved Successfully");
}

function onSavePromotion() {
    $("#idProrateTooltip").css("display", "inline");
}

   // PRORATION 

var startDateData;// =; Model.ProrateStartDate;
var endDateData;// = Model.ProrateEndDate;

var isMerit = false;
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
                placement: "right",
                content: value.message,
                template: "<div class=\"popover\"><div class=\"arrow\"></div><div class=\"popover-inner\"><div class=\"popover-content\"><span class=\"glyphicon glyphicon-hand-right\"></span><p></p></div></div></div>"
            });
            $(value.element).parent().addClass("has-error");
            _popover.data("bs.popover").options.content = value.message;
            return $(value.element).popover("show");
        });
    }
});


$(document).on("keydown", "#txtProrationDatesPerMonth", function (e) {
    if ($('input:radio[name=duration]:checked').val() == undefined || $('input:radio[name=duration]:checked').val() == "") {
        if (e.keyCode == 9)
            $(this).trigger('blur');
    }
});


$(document).on("keydown", "#txtProrationDuration", function (e) {
    if ($('input:radio[name=duration]:checked').val() == undefined || $('input:radio[name=duration]:checked').val() == "") {
        if (e.keyCode == 9)
            $(this).trigger('blur');
    }
});


$(document).on("click", "#txtProrationDuration", function (e) {
    if ($('input:radio[name=duration]:checked').val() == undefined || $('input:radio[name=duration]:checked').val() == "") {
        $(this).trigger('blur');
    }
    var StartDate = $("#prorationStartDate").data("kendoDatePicker")._oldText;
    $("#processStartDateValue").val(StartDate);
    var EndDate = $("#prorationEndDate").data("kendoDatePicker")._oldText;
    $("#processEndDateValue").val(EndDate);
});

$(document).on("click", "#txtProrationDatesPerMonth", function (e) {
    if ($('input:radio[name=duration]:checked').val() == undefined || $('input:radio[name=duration]:checked').val() == "") {
        $(this).trigger('blur');
    }
    var StartDate = $("#prorationStartDate").data("kendoDatePicker")._oldText;
    $("#processStartDateValue").val(StartDate);
    var EndDate = $("#prorationEndDate").data("kendoDatePicker")._oldText;
    $("#processEndDateValue").val(EndDate);
});

$(document).on("change", "#prorationDaily", function (e) {
    var StartDate = $("#prorationStartDate").data("kendoDatePicker")._oldText;
    $("#processStartDateValue").val(StartDate);
    var EndDate = $("#prorationEndDate").data("kendoDatePicker")._oldText;
    $("#processEndDateValue").val(EndDate);
});

$(document).on("show.bs.modal", "#divProration", function (e) {
    $.validator.unobtrusive.parse(document);
    setValidation();

    $("#txtProrationDatesPerMonth").keypress(function (e) {
        if (e.charCode >= 48 && e.charCode <= 57)
            $("#datePerMonth").html("");
        if (String.fromCharCode(e.keyCode).match(/[^0-9]/g)) return false;
    });

    $("#txtProrationDuration").keypress(function (e) {
        if (String.fromCharCode(e.keyCode).match(/[^0-9]/g)) return false;
    });
});

// Functions

function setValidation() {
    $(".input-validation-error").parent().removeClass('has-success').addClass("has-error");
    $("div.validation-summary-errors").has("li:visible").addClass("alert-block alert-danger");

    //$('form').data('validator').settings.onfocusout = function (element) {
    //    $(element).valid();
    //};
}
function prorationStartDateChange(e) {
    var a = e.sender._oldText;

    $("#processStartDateValue").val(a);
    var EndDate = $("#prorationEndDate").data("kendoDatePicker")._oldText;
    $("#processEndDateValue").val(EndDate);
    isProrationChanged = true;
}
function prorationEndDateChange(e) {
    var a = e.sender._oldText;
    $("#processEndDateValue").val(a);
    var StartDate = $("#prorationStartDate").data("kendoDatePicker")._oldText;
    $("#processStartDateValue").val(StartDate);
    isProrationChanged = true;
}

$("#form").submit(function (e) {
    if ($("#txtProrationDatesPerMonth").val() == "" && $("#prorationType").val() == "Monthly") {
        $("#datePerMonth").html("Please Enter Your Proration Dates Per Month");
        return false;
    }
});

