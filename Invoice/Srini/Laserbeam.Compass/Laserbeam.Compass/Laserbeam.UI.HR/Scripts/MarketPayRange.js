/* Javascript file for Market pay range*/
/*region Market Pay Range functions*/
var selectedMarketPayRange;
var MarketPayRangeConstants = [];
function GetReporteesMarketPayRangeParam(e) {
    return { selectedMarketPayRange: selectedMarketPayRange }
}
function onMarketPayRangeDataBound(e) {    
    var grid = $('#MarketPayRangeReporteeGrid').data('kendoGrid'); 
    if (selectedMarketPayRange.toLowerCase() == "jobcode") {
        grid.hideColumn("GradeCode");
        grid.hideColumn("EmployeeID");
        //grid.hideColumn("EmployeeName");

        grid.showColumn("JobCode");
    }
    else if (selectedMarketPayRange.toLowerCase() == "grade") {
        grid.hideColumn("JobCode");
        grid.hideColumn("EmployeeID");
        //grid.hideColumn("EmployeeName");

        grid.showColumn("GradeCode");
    }
    else if (selectedMarketPayRange.toLowerCase() == "byemployee") {
        grid.hideColumn("JobCode");
        grid.hideColumn("GradeCode");

        grid.showColumn("EmployeeID");
        //grid.showColumn("EmployeeName");
    }    
}
$(document).on("click", "input[name=MarketPayRangeToggle]", function (e) {
    if (selectedMarketPayRange != $('input[name=MarketPayRangeToggle]:checked').val()) {
        var con = showConfirm(e,"If you switch over the toggle, previous data will erase. Are you sure to proceed?");
        if (con) {
            selectedMarketPayRange = $('input[name=MarketPayRangeToggle]:checked').val();
            $("#btnAddMarketPayRange").show();
            $.ajax({
                url: '../MarketPayRange/_Reportees',
                type: "POST",
                async: true,
                cache: false,
                data: { selectedMarketPayRange: selectedMarketPayRange, isToggleSwitch: true },
                success: function (result) {
                    if (selectedMarketPayRange.toLowerCase() == "byemployee")
                        $("#btnAddMarketPayRange").hide();
                    if ($("#liMarketPayRangeList").hasClass("active")) {
                        var grdMarketPayRangeReportees = $("#MarketPayRangeReporteeGrid").data("kendoGrid");
                        grdMarketPayRangeReportees.dataSource.read();
                    }
                }
            });
        }
        else {
            return false;
        }
    }
});

function OnBeginMarketPayRange() {
    if ($("#idMarPayRangeJob").val() && $("#idMarPayRangeGrade").val()) {
        //alert("Enter Header");
        showAlert("Please enter header");
        return false;
    }
    else {
        return true;
    }
}
function onSuccessUpdateMarketPayRange(result) {
    if (result.toLowerCase() == "success") {        
        $("#AddMarketpayrange").modal('hide');
        Successmessage("Saved Successfully");
        var grdMarketPayRangeReportees = $("#MarketPayRangeReporteeGrid").data("kendoGrid");
        grdMarketPayRangeReportees.dataSource.read();
    }
    else {
        var selectedText = $("#SelectedMarketPayRange").val();
        $("#valdationErrorMsg").html(selectedText + " " + result);
        $("#valdationErrorMsg").fadeIn("slow");
        $("#valdationErrorMsg").fadeOut(3000);
    }

}
function onAddMarketRangeFunc(selectedValue, marketPayRangeNum, isEdit) {
    $.ajax({
        url: '../MarketPayRange/_AddMarketPayRange',
        type: "GET",
        async: true,
        cache: false,
        data: { selectedValue: selectedValue, marketPayRangeNum: marketPayRangeNum, isEdit: isEdit },
        success: function (result) {
            $("#divAddMarketPayRangeModel").html(result);
            $("#AddMarketpayrange").modal('show');
        }
    });
}

function allNumbersOnly(e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
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
function allowDecimalsRange(e,$this,decimalRange)
{
    var value = parseFloat($($this).val());
    if (!isNaN(value)) {
        $($this).val(parseFloat(value).toFixed(decimalRange));
    }
    
}
function refreshMarketPayRangeGrid()
{
    var grdMarketPayRangeReportees = $("#MarketPayRangeReporteeGrid").data("kendoGrid");
    grdMarketPayRangeReportees.dataSource.read();
}
$(document).on("click", "#btnAddMarketPayRange", function (e) {
    onAddMarketRangeFunc("", 0,false)
});
$(document).on("keydown", ".market-numbers-annual, .market-numbers-hourly", function (e) {   
    allNumbersOnly(e);    
});
$(document).on("blur", ".market-numbers-annual", function (e) {
    allowDecimalsRange(e, this, 2);
});
$(document).on("blur", ".market-numbers-hourly", function (e) {
    allowDecimalsRange(e, this, 5);
});
//$(document).on("click", "#lnkMarketPayRangeJobCode, #lnkMarketPayRangeGradeCode, #lnkMarketPayRangeEmployeeID", function (e) {
//    var selectedValue = $.trim($(this).attr("data-value"));
//    var marketPayRangeNum = $.trim($(this).attr("data-marketpayrange-num"));
//    onAddMarketRangeFunc(selectedValue, marketPayRangeNum,true);
//});
$(document).on("click", "#liMarketPayRangeList, #liMarketPayRangeData", function (e) {
    refreshMarketPayRangeGrid();
});
function Grid_OnRowSelect(e) {
    var data = this.dataItem(this.select());
    var datauid;
    if (selectedMarketPayRange.toLowerCase() == "jobcode") 
        datauid = $("[data-uid=" + data.uid + "]").find("#lnkMarketPayRangeJobCode")
    else if (selectedMarketPayRange.toLowerCase() == "grade")
        datauid = $("[data-uid=" + data.uid + "]").find("#lnkMarketPayRangeGradeCode")
    else if (selectedMarketPayRange.toLowerCase() == "byemployee")
        datauid = $("[data-uid=" + data.uid + "]").find("#lnkMarketPayRangeEmployeeID")
    
    var selectedValue = $.trim($(datauid).attr("data-value"));
    var marketPayRangeNum = $.trim($(datauid).attr("data-marketpayrange-num"));
    onAddMarketRangeFunc(selectedValue, marketPayRangeNum, true);
    //window.location("ViewName/YourRowID");
}
$(document).on("click", "#btnFilterMarketPayRange", function (e) {    
    $.ajax({
        url: '../MarketPayRange/_FilterSort/',
        type: 'GET',
        cache: false,
        data:{selectedMarketPayRange:selectedMarketPayRange},
        success: function (result) {                        
            $("#MarketPayRangeFilterPopup").html(result);
            $("#MarketPayRangeFilterPopup").data("kendoWindow").center().open().title("Comment");
        }
    });
});
$(document).on("click", "#btnClearFilterMarketPayRange", function (event) {

    clearFilterSort(true);
    $("#btnClearFilterMarketPayRange").hide();
    $("#btnFilterMarketPayRange").show();
    isFilter = false;
});
/*endregion  Market Pay Range functions*/