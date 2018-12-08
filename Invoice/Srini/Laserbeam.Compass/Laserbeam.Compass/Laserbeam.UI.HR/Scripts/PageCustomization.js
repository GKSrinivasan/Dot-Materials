var checkAll;
var changed;
var gridPages;
var checkboxflag = false;
var checkBoxCheckFlag = false;
$(document).ready(function () {
    $("#mainemptydiv").hide();
    $("div.bhoechie-tab-menu>div.list-group>a").click(function (e) {
        e.preventDefault();
        $(this).siblings('a.active').removeClass("active");
        $(this).addClass("active");
        var index = $(this).index();
        $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
        $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");
    });
    $("#lnkGridDisplay").trigger("click");

    var html = jQuery('html');
    html.css('overflow', 'auto');
    $("#btnClearFilter").hide();
});
$(document).on("click", "#Griddisplay", function (e) {
    $("#mainemptydiv").show();
});

$(document).on("click", ".noemptydiv", function (e) {
    $("#mainemptydiv").hide();
});
$(document).on("click", "#lnkGridDisplay", function (e) {
    $("#mainemptydiv").hide();
    $.ajax({
        url: '../PageCustomization/_GridDisplay',
        data: null,
        type: 'get',
        cache: false,
        success: function (result) {
            $("#PageCustomization").html(result);
        }
    });
});
$(document).on("click", "#lnkExportDisplay", function (e) {
    $("#mainemptydiv").hide();
    $.ajax({
        url: '../PageCustomization/_ExportDisplay',
        data: null,
        type: 'get',
        cache: false,
        success: function (result) {
            $("#PageCustomization").html(result);
        }
    });
}); $(document).on("click", "#lnkPopupDisplay", function (e) {
    $("#mainemptydiv").show();
    $.ajax({
        url: '../PageCustomization/_PopupDisplay',
        data: null,
        type: 'get',
        cache: false,
        success: function (result) {
            $("#PageCustomization").html(result);
        }
    });
});
$(document).on("click", ".noemptydiv", function (e) {
    $("#mainemptydiv").hide();
});
$(document).on("click", "#lnkFilterDisplay", function (e) {
    $("#mainemptydiv").hide();
    $.ajax({
        url: '../PageCustomization/_FilterDisplay',
        data: null,
        type: 'get',
        cache: false,
        success: function (result) {
            $("#PageCustomization").html(result);
        }
    });
});
function setRowChanged(rowData) {
    rowData.IsChanged = true;
}
var showMessage = (function (message, preventPost) {
    alert(message);
    return !preventPost;
});

function checkAll(ele) {
    var inputName = document.getElementById(ele.id).name;
    var checkboxes = document.getElementsByName(inputName);
    var grid = $("#grdPageCustomization").data("kendoGrid");
    var rowItem = $(this).closest("tr");
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
        for (var k = 0; k < grid._data.length; k++) {
            if (checkboxes[k].type == 'checkbox') {
                var rowIndex = ($(rowItem).t = k);
                var rowData = grid._data[rowIndex];
                swi(inputName);
            }
        }
        checkboxflag = true;
    }

    else {
        for (i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
        for (var k = 0; k < grid._data.length; k++) {
            if (checkboxes[k].type == 'checkbox') {
                var rowIndex = ($(rowItem).t = k);
                var rowData = grid._data[rowIndex];
                swi(inputName);
            }
        }
    }
    function swi(inputName) {
        switch (inputName) {
            case "PopupDisplay":
                rowData.PopupDisplay = checkboxes[k].checked;
                setRowChanged(rowData);
                break;
            case "ExportDisplay":
                rowData.ExportDisplay = checkboxes[k].checked;
                setRowChanged(rowData);
                break;
            case "FilterDisplay":
                rowData.FilterDisplay = checkboxes[k].checked;
                setRowChanged(rowData);
                break;
            case "GridDisplayGroupFields":
                rowData.GridDisplay = checkboxes[k].checked;
                setRowChanged(rowData);
                break;
        }
    }
}
$(document).on("click", "#btnPageCustomizationSave", function (e) {
    var flag = false;
    $("#chkSelectAllPopupDisplay").prop("checked", false);
    $("#chkSelectAllExportDisplay").prop("checked", false);
    $("#chkSelectAllFilterDisplay").prop("checked", false);
    $("#chkSelectAllGridDisplay").prop("checked", false);
    var grid = $("#grdPageCustomization").data("kendoGrid");
    var gridData = grid._data;
    var postData = [];
    $(gridData).each(function () {
        changed = this.IsChanged;
        if (this.IsChanged) {
            postData.push(this);
        }
    });
    if (postData.length > 0) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var jsonString =JSON.stringify(postData); 
        var jsonData = JSON.parse(jsonString)
        $.ajax({
            url: '../PageCustomization/UpdatePageCustomization',
            type: "post",
            data: ({ __RequestVerificationToken: token, pageCustomizationDetails: jsonData }),
            success: function (message) {
                var grid = $("#grdPageCustomization").data("kendoGrid");
                Successmessage(message);
                grid.refresh();
            }
        });
        postData.length = 0;
    }

    ///inorder to handle the sstatus of ischanged to handle postback


    $(gridData).each(function (e) {
        var changed = this.IsChanged;
        this.IsChanged = false;
    });
    objChangeFlag = false;
});

$(document).on("change", "[id$=txtAliasName]", function (e) { 
      var functionalGroup = [];
    var val = 0;
    var count = [];
    var isDuplicate = false;
    var fieldName;
    $('.k-grouping-row').each(function (e) {
        var rowIndexValues = this.rowIndex;
        functionalGroup.push(rowIndexValues);
    });
    var grid = $("#grdPageCustomization").data("kendoGrid");
    var rowItem = $(this).closest("tr[role='row']");
    var rowIndexVal = rowItem[0].rowIndex;
    functionalGroup.forEach(function (e) {
        if (functionalGroup[val] < rowIndexVal) {
            count.push(functionalGroup[val])
        }
        val++
    })
    var rowIndex = rowIndexVal - count.length;
    var rowData = grid._data[rowIndex];
    for (i = 0; i < grid._data.length; i++) {
      
       // if (e.target.value.toLowerCase().trim() == grid._data[i].AliasName.toLowerCase().trim() && rowData.ColumnName.trim() != grid._data[i].ColumnName)
        if (e.target.value.toLowerCase().trim() == grid._data[i].AliasName.toLowerCase().trim() && rowData.ColumnName.trim() != grid._data[i].ColumnName)
        {
            fieldName = grid._data[i].ColumnName;
            isDuplicate = true;
            break;
        }
    }
    if (isDuplicate == true) {
        var row = $(this).closest("tr");
        showConfirmWithOk(e, "<div style='text-align:left;'>It looks like you have given  <span style='color:orange;'>" + e.target.value + "</span> as display name for the following fields <br/> <span style='font-size:14px;font-weight:bold'>1. " + fieldName + " <br/> 2. " + rowData.ColumnName + "</span> </br> You cannot have fields with duplicate display name. Please correct. </div>");
        rowData.AliasName = rowData.AliasName;
        refreshRow(grid, row);

    }
    else {
        rowData.AliasName = e.target.value;
        setRowChanged(rowData);
    }
});

$(document).on("change", "[id$=chkPopupDisplay]", function (e) {
    var checkBoxUnCheckFlag = false;
    var functionalGroup = [];
    var val = 0;
    var count = [];
    $('.k-grouping-row').each(function (e) {
        var rowIndexValues = this.rowIndex;
        functionalGroup.push(rowIndexValues);
    });
    var grid = $("#grdPageCustomization").data("kendoGrid");
    var rowItem = $(this).closest("tr[role='row']");
    var rowIndexVal = rowItem[0].rowIndex;
    functionalGroup.forEach(function (e) {           //get the functional group
        if (functionalGroup[val] < rowIndexVal) {
            count.push(functionalGroup[val])
        }
        val++
    })
    var rowIndex = rowIndexVal - count.length;
    var rowData = grid._data[rowIndex];
    rowData.PopupDisplay = e.target.checked;
    setRowChanged(rowData);
    var inputName = e.target.name;
    var checkboxes = document.getElementsByName(inputName);
    var lists = [];
    for (var i = 1; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox') {
            lists.push(checkboxes[i].checked);
        }
    }
    for (var item in lists) {               //checkbox if all true set flag
        if (lists[item] == true) {
            checkBoxCheckFlag = true;
        }
        else {
            checkBoxUnCheckFlag = true;
        }

    }
    if (rowData.IsChanged == true && checkboxflag == true && e.target.id != "chkSelectAllPopupDisplay") {
        $("#chkSelectAllPopupDisplay").prop("checked", false);
    }
    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag != true && e.target.id != "chkSelectAllPopupDisplay") {
        $("#chkSelectAllPopupDisplay").prop("checked", true);//check all checkbox set to true if child nodes are checked
    }
    if (checkBoxCheckFlag != true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllPopupDisplay") {
        $("#chkSelectAllPopupDisplay").prop("checked", false);
    }
    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllPopupDisplay") {
        $("#chkSelectAllPopupDisplay").prop("checked", false);
    }
    checkBoxUnCheckFlag = false;
    checkBoxCheckFlag = false;
    //checkBoxFlag = false;
});
$(document).on("change", "[id$=chkFilterDisplay]", function (e) {
    var checkBoxUnCheckFlag = false;
    var functionalGroup = [];
    var val = 0;
    var count = [];
    $('.k-grouping-row').each(function (e) {
        var rowIndexValues = this.rowIndex;
        functionalGroup.push(rowIndexValues);
    });
    var grid = $("#grdPageCustomization").data("kendoGrid");
    var rowItem = $(this).closest("tr[role='row']");
    var rowIndexVal = rowItem[0].rowIndex;
    functionalGroup.forEach(function (e) {
        if (functionalGroup[val] < rowIndexVal) {
            count.push(functionalGroup[val])
        }
        val++
    })
    var rowIndex = rowIndexVal - count.length;
    var rowData = grid._data[rowIndex];
    rowData.FilterDisplay = e.target.checked;
    setRowChanged(rowData);
    var inputName = e.target.name;
    var checkboxes = document.getElementsByName(inputName);
    var lists = [];
    for (var i = 1; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox') {
            lists.push(checkboxes[i].checked);
        }
    }
    for (var item in lists) {
        if (lists[item] == true) {
            checkBoxCheckFlag = true;
        }
        else {
            checkBoxUnCheckFlag = true;
        }
    }

    if (rowData.IsChanged == true && checkboxflag == true && e.target.id != "chkSelectAllFilterDisplay") {
        $("#chkSelectAllFilterDisplay").prop("checked", false);
    }

    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag != true && e.target.id != "chkSelectAllFilterDisplay") {
        $("#chkSelectAllFilterDisplay").prop("checked", true);//check all checkbox set to true if child nodes are checked
    }
    if (checkBoxCheckFlag != true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllFilterDisplay") {
        $("#chkSelectAllFilterDisplay").prop("checked", false);
    }
    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllFilterDisplay") {
        $("#chkSelectAllFilterDisplay").prop("checked", false);
    }

    checkBoxUnCheckFlag = false;
    checkBoxCheckFlag = false;
});
$(document).on("change", "[id$=chkGridDisplayGroupFields]", function (e) {
    var checkBoxUnCheckFlag = false;
    var functionalGroup = [];
    var val = 0;
    var count = [];
    $('.k-grouping-row').each(function (e) {
        var rowIndexValues = this.rowIndex;
        functionalGroup.push(rowIndexValues);
    });
    var grid = $("#grdPageCustomization").data("kendoGrid");
    var rowItem = $(this).closest("tr[role='row']");
    var rowIndexVal = rowItem[0].rowIndex;
    functionalGroup.forEach(function (e) {
        if (functionalGroup[val] < rowIndexVal) {
            count.push(functionalGroup[val])
        }
        val++
    })
    var rowIndex = rowIndexVal - count.length;
    var rowData = grid._data[rowIndex];
    rowData.GridDisplay = e.target.checked;
    setRowChanged(rowData);
    var inputName = e.target.name;
    var checkboxes = document.getElementsByName(inputName);
    var lists = [];
    for (var i = 1; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox') {
            lists.push(checkboxes[i].checked);
        }
    }
    for (var item in lists) {
        if (lists[item] == true) {
            checkBoxCheckFlag = true;
        }
        else {
            checkBoxUnCheckFlag = true;
        }
    }
    grid.refresh();
    if (rowData.IsChanged == true && checkboxflag == true && e.target.id != "chkSelectAllGridDisplay") {
        $("#chkSelectAllGridDisplay").prop("checked", false);
    }
    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag != true && e.target.id != "chkSelectAllGridDisplay") {
        $("#chkSelectAllGridDisplay").prop("checked", true);//check all checkbox set to true if child nodes are checked
    }
    if (checkBoxCheckFlag != true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllGridDisplay") {
        $("#chkSelectAllGridDisplay").prop("checked", false);
    }
    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllGridDisplay") {
        $("#chkSelectAllGridDisplay").prop("checked", false);
    }
    checkBoxUnCheckFlag = false;
    checkBoxCheckFlag = false;
});
$(document).on("click", "#btnFilter", function (e) {
    $.ajax({
        url: '../PageCustomization/_FilterSort',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        success: function (result) {
            $("#wndFilterSort").html(result);
            var wndFilterSort = $("#wndFilterSort").data("kendoWindow");
            wndFilterSort.options.gridName = "grdPageCustomization";
            wndFilterSort.center().open();
            $("#filterseperator").css("display", "inline");
            isFilter = true;
        }
    });
});

$(document).on("change", "#grdPageCustomization input[type='text'], input[type='checkbox']", function (e) {
    objChangeFlag = true;
});
$(document).on("change", "[id$=chkExportDisplay]", function (e) {
    var checkBoxUnCheckFlag = false;
    var functionalGroup = [];
    var val = 0;
    var count = [];
    $('.k-grouping-row').each(function (e) {
        var rowIndexValues = this.rowIndex;
        functionalGroup.push(rowIndexValues);
    });
    var grid = $("#grdPageCustomization").data("kendoGrid");
    var rowItem = $(this).closest("tr[role='row']");
    var rowIndexVal = rowItem[0].rowIndex;
    functionalGroup.forEach(function (e) {
        if (functionalGroup[val] < rowIndexVal) {
            count.push(functionalGroup[val])
        }
        val++
    })
    var rowIndex = rowIndexVal - count.length;
    var rowData = grid._data[rowIndex];
    rowData.ExportDisplay = e.target.checked;
    setRowChanged(rowData);
    var inputName = e.target.name;
    var checkboxes = document.getElementsByName(inputName);
    var lists = [];
    for (var i = 1; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox') {
            lists.push(checkboxes[i].checked);
        }
    }
    for (var item in lists) {
        if (lists[item] == true) {
            checkBoxCheckFlag = true;
        }
        else {
            checkBoxUnCheckFlag = true;
        }
    }
    if (rowData.IsChanged == true && checkboxflag == true && e.target.id != "chkSelectAllExportDisplay") {
        $("#chkSelectAllExportDisplay").prop("checked", false);
    }
    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag != true && e.target.id != "chkSelectAllExportDisplay") {
        $("#chkSelectAllExportDisplay").prop("checked", true);//check all checkbox set to true if child nodes are checked
    }
    if (checkBoxCheckFlag != true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllExportDisplay") {
        $("#chkSelectAllExportDisplay").prop("checked", false);
    }
    if (checkBoxCheckFlag == true && checkBoxUnCheckFlag == true && e.target.id != "chkSelectAllExportDisplay") {
        $("#chkSelectAllExportDisplay").prop("checked", false);
    }
    checkBoxUnCheckFlag = false;
    checkBoxCheckFlag = false;
});

$(document).on("click", "#btnClearFilter", function (event) {
    clearFilterSort(true);
    $("#btnClearFilter").hide();
    $("#filter").show();
});


$(document).on("click", "#btnPageCustomizationReset", function () {
     var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../PageCustomization/ResetAction',
        type: 'Post',
        data:({__RequestVerificationToken:token }),
        success: function (message) {
            var grid = $("#grdPageCustomization").data("kendoGrid");
            Successmessage(message);
            grid.dataSource.read();
        }
    });
})

var prevPage = 1;
function requestStartPageCustomization(e) {

}

function showDisplay(status) {
    var commentImage;
    switch (status) {
        case true: commentImage = "<i class='fa fa-check displayColor'></i>";
            break;

        case false: commentImage = "<i></i>";
            break;


        default: commentImage = "<i></i>";
    }
    return commentImage;
}


function pageCustomizationFunGrpNames() {
    var groupNames = $("#grdPageCustomization .k-grouping-row").find('.k-reset');
    for (var i = 0; i < groupNames.length; i++) {
        var actualGroupValue = groupNames[i].innerText;
        var suffix = actualGroupValue.substring(19);
        groupNames[i].innerHTML = '<a tabindex=\"-1\" class=\"k-icon k-i-collapse\" href=\"#\"></a>' + suffix;
    }
}

function additionalParamInfo()
{
        var token = $('input[name="__RequestVerificationToken"]').val();
        return { __RequestVerificationToken: token};
}

function PopupDisplayParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    return { __RequestVerificationToken: token, display: "Popup" };
}

function GridDisplayParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    return { __RequestVerificationToken: token, display: "Grid" };
}

function ExportDisplayParam() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    return { __RequestVerificationToken: token, display: "Export" };
}
function showConfirmWithOk(event, message) {

    if (isAllowConfirm == true) {
        isAllowConfirm = false;
        return true;
    }
    swal({
        title: "Oops...",
        html: message,
        type: "",
        showCancelButton: false,
        showConfirmWithButton:true,
        padding: 40,
        width:700,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#3085d6',
        confirmButtonClass: 'cancel-class',
        cancelButtonClass: 'cancel-class',
        confirmButtonText: "Ok",
        cancelButtonText: "Cancel",
        allowOutsideClick: false,
        closeOnConfirm: true,
        closeOnCancel: true,
        allowEscapeKey: false
    }, function (isConfirm) {
        
       
    });
    return false;
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