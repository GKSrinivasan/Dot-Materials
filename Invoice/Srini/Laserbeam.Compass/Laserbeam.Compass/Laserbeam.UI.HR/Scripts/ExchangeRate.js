$(document).ready(function () {
    $("#exchangeRateClearFilter").hide();
    var selecteddata;
    var text;
});

$(document).on("click", "#btnChangeBaseCurrency", function (event) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
                url: "../ExchangeRate/UpdateBaseCurrency",
                data: { __RequestVerificationToken:token, currencyCode: text, currencyCodeNum: selecteddata.CurrencyNum },
                type: "Post",
                success: function (result) {
                    $("#divExchangeRate").modal('hide');
                    $("#grdExchangeRate").data("kendoGrid").dataSource.read();
                    Successmessage("Exchange Rate Updated Successfully ");
                }
            });
});

$(document).on("click", "#btnCancelBaseCurrency", function (event) {
   
    $("#divExchangeRate").modal('hide');
    $("#CurrencyCodeComboBox1").data("kendoComboBox").value(baseCurrSelectedVal);
});

$(document).on("click", "#btnAddCurrency", function (event) {
    $.ajax({
        url: "../ExchangeRate/_ExchangeRatePopUp",
        type: "Get",
        success: function (result) {
            $("#divExchangeRate").html(result);
            $("#divExchangeRate").modal('show');
        }
    });
});

$(document).keypress(function (e) {
    if (e.delegateTarget.activeElement.name == "CurrencyCodeComboBox_input") {
        if ($("input[name=CurrencyCodeComboBox_input]").val().length < 3) {
            var regex = new RegExp("[a-zA-Z]");
            var key = String.fromCharCode(e.charCode ? e.which : e.charCode);
            if (!regex.test(key)) {
                e.preventDefault();
                return false;
            }
        }
        else {
            e.preventDefault();
            return false;
        }
    }
});

function closeAfterExchangeRateAdded() {
    $("#grdExchangeRate").data("kendoGrid").dataSource.read();
    $("#divExchangeRate").modal('hide');
    Successmessage("Modified Successfully");
}

function closeAfterExchangeRateAdded1() {    
    $("#divExchangeRate").modal('hide');    
}

function currencyCultureChange() {
    $("#CurrencyCode").val($("#CurrencyCodeComboBox").val() != undefined ? $("#CurrencyCodeComboBox").val() : $("#addCurrencyCode").val());
    var cultureCode = $("#cultureComboBox").val();
    if(cultureCode == "")
    {
        $("#mandate_CultureCode").html("Culture Code is missing");
    }
    else {
        $("#mandate_CultureCode").html("");
        $("#CultureCode").val($("#cultureComboBox").val());
    }
}

function currencyCodeChange() {
    $("#CultureCode").val($("#cultureComboBox").val());
    $("#CurrencyCode").val($("#CurrencyCodeComboBox").val());
}

function grdExchangeRateChange() {
    var grid = $("#grdExchangeRate").data("kendoGrid");
    var selectedItem = grid.dataItem($(this).closest("tr"));
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../ExchangeRate/_ExchangeRatePopUpModify",
        type: "Post",
        data: { __RequestVerificationToken:token, exchangeRateNum: selectedItem.CurrencyCodeNum, CurrencyCode: selectedItem.CurrencyCode, ExchangeRate: selectedItem.ExchangeRate, culture: selectedItem.CultureCode },
        success: function (result) {
            $("#divExchangeRate").html(result);
            $("#divExchangeRate").modal('show');
        }
    });
}

$(document).on("click", "[id$=cultureCode]", function () {

    exchangeRate($(this));
});

$(document).on("click", "[id$=exchangeRate]", function () {

    exchangeRate($(this));

});

$(document).on("click", "[id$=currencyCode]", function () {
    exchangeRate($(this));

});

$(document).on("click", "[id$=currencyCodeNum]", function () {
    exchangeRate($(this));

});
$(document).on("click", "[id$=conversionPreview]", function () {
    exchangeRate($(this));

});
$(document).on("click", "[id$=displayPreview]", function () {
    exchangeRate($(this));
});


function exchangeRate(data) {
    var grdMeritGrid = $("#grdExchangeRate").data("kendoGrid");
    var rowIndex = data.closest("tr").index();
    var row = data.closest("tr");
    var selectedItem = grdMeritGrid._data[rowIndex];
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../ExchangeRate/_ExchangeRatePopUpModify",
        type: "Post",
        data: { __RequestVerificationToken:token, exchangeRateNum: selectedItem.CurrencyCodeNum, CurrencyCode: selectedItem.CurrencyCode, ExchangeRate: selectedItem.ExchangeRate, cultureCode: selectedItem.CultureCode },
        success: function (result) {
            $("#divExchangeRate").html(result);
            $("#divExchangeRate").modal('show');
        }
    });

}

$(document).on("change", "#addCurrencyCode", function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var currencyCode = $("#addCurrencyCode").val();
    if (currencyCode == "") {
        $("#mandate_CurrencyCode").html("Currency Code is missing");
    }
    else {        
        $.ajax({
            url: "../ExchangeRate/ValidateCurrencyCode",
            type: "post",
            data: { __RequestVerificationToken:token, currencyCodeValue: currencyCode },
            success: function (result) {
                if (result == true) {
                    $("#mandate_CurrencyCode").html("Currency Code already exists");
                }
                else {
                    $("#mandate_CurrencyCode").html("");
                }
            }
        });
    }
});

function currencyCodeValidation()
{
    var currencyCode = $("#addCurrencyCode").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (currencyCode == "") {
        $("#mandate_CurrencyCode").html("Currency Code is missing");
    }
    else if(currencyCode.length > 3)
        $("#mandate_CurrencyCode").html("Currency Code should not exceed 3 characters");
    else {        
        $.ajax({
            url: "../ExchangeRate/ValidateCurrencyCode",
            type: "post",
            data: { __RequestVerificationToken:token, currencyCodeValue: currencyCode },
            success: function (result) {
                if (result == true) {
                    $("#mandate_CurrencyCode").html("Currency Code already exists");
                }
                else {
                    $("#mandate_CurrencyCode").html("");
                }
            }
        });
    }
}

function ExchangeRateValidation()
{
    var exchangeRate = $("#addExchangeRate").val();
    if (exchangeRate == 0) {
        $("#mandate_ExchangeRate").html("Exchange Rate is missing");
    }
    else {
        $("#mandate_ExchangeRate").html("");
    }
}

function ExchangeRateDataBound() {
    var grdExchangeRateGrid = $("#grdExchangeRate").data("kendoGrid");
    if (grdExchangeRateGrid.dataSource._view.length == 0) {
        var colCount = $("#grdExchangeRate").find('th').length;
        $("#grdExchangeRate").find('tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center;background-color:#6686C4;color:white !important;">No Results Found!</td></tr>');       
    }
}


$(document).on("change", "#addExchangeRate", function () {
    var exchangeRate = $("#addExchangeRate").val();
    if(exchangeRate == ""){
        $("#mandate_ExchangeRate").html("Exchange Rate is missing");
    }
    else {
        $("#mandate_ExchangeRate").html("");
    }
});

$(document).on("click", "#addExchangeRateBtn", function () {
    var currencyCode = $("#addCurrencyCode").val();
    var EditCurrencyCode = $("#CurrencyCodeComboBox").val();
    (EditCurrencyCode == undefined && currencyCode != undefined) ? currencyCodeValidation() : '';
    currencyCultureChange();
    ExchangeRateValidation();
    var errorMessage = "";
    $('[id^="mandate_"]').each(function () {
        errorMessage = errorMessage + ((this.textContent != "" && this.textContent != undefined && this.textContent != " ") ? (this.textContent).trim() + "<br/>" : "");
    });
    errorMessage = (errorMessage.trim() == "<br/>") ? "" : errorMessage.trim();
    if (errorMessage == "") {
        $("#AddinngExchangeRateForm").submit();
    }
});

$(document).on('click', '#exchangeRateFilter', function () {
    $.ajax({
        url: '../ExchangeRate/_ExchangeRateFilterSort',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        success: function (result) {
            $("#wndFilterSortPopup").html(result);
            var wndFilterSort = $("#wndFilterSortPopup").data("kendoWindow");
            wndFilterSort.options.gridName = "grdExchangeRate";
            wndFilterSort.center().open();
            $("#filterseperator").css("display", "inline");
            $("#exchangeRateFilter").css("display", "inline");
            $("#exchangeRateFilter").hide();
            $("#exchangeRateClearFilter").show();
        }
    });

});

$(document).on("click", "#exchangeRateClearFilter", function () {
        clearFilterSort(true);
        $("#exchangeRateClearFilter").hide();
        $("#exchangeRateFilter").show();
});

$(document).on("click", "#exchangeRateExport", function (event) {
    var form = $("<form action='../ExchangeRate/ExchangeRateExport' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + $('input[name="__RequestVerificationToken"]').val() + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});

function ValidateCurrencyCode(event) {
    var regex = new RegExp("[a-zA-Z]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}

function test()
{
   
}
$(document).on("keyup", "input[name=CurrencyCodeComboBox]", function () {
    var regex = new RegExp("[a-zA-Z]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});

$(document).on("keyup", "#CurrencyCodeComboBox", function () {
    var regex = new RegExp("[a-zA-Z]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});

$(document).on("keyup", "[id*=CurrencyCodeComboBox]", function () {
    var regex = new RegExp("[a-zA-Z]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});

function additionalParamInfo()
{
    return {
        __RequestVerificationToken : $('input[name="__RequestVerificationToken"]').val()
    }
}