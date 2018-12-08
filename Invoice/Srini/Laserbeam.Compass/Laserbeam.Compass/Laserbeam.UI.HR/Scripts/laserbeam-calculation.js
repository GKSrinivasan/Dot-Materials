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

function isValueInvalid(value) {
    return (value == null || value == undefined || value == NaN);
}


function formatValue(value, format, culture) {
    if (value == null) return '';
    if ((arguments.length == 1) || format == undefined)
        return value.toString();
    else if (culture == undefined) {
        var val = kendo.toString(value, format);
        val = val.indexOf('-') > -1 ? '(' + val.replace('-', '') + ')' : val
        return val;
    }
    else {
        var kndValue = kendo.toString(value, format, culture);
        kndValue = kndValue.indexOf('-') > -1 ? '(' + kndValue.replace('-', '') + ')' : kndValue;
        return kndValue;
    }
}

var checkCurrency = (function (value) {
    if (!value.match(/(?:^|\s)([1-9](?:\d*|(?:\d{0,2})(?:,\d{3})*)(?:\.\d*[1-9])?|0?\.\d*[1-9]|0)(?:\s|$)/) && value.length > 0) {
        return false;
    }
    return true;
});

function checkDecimalInput(value) {
    if (value != null && value.length == 1 && (value == "." || value == "%")) {
        return false;
    }
    return true;
}
var initialValue = "";
function check_float(e, field) {
    var charCode = "";
    if (e.which == undefined)
        charCode = e.keyCode;
    else
        charCode = e.which;
    if (field.readOnly == true) return;

    if (charCode == 46) {
        var patt1 = new RegExp("\\.");
        var ch = patt1.exec(field.value);
        if (ch == ".") {
            showAlert("More than one decimal point not allowed");
            (window.Event) ? e.which : e.keyCode = 0;
            field.value = initialValue;
            return false;
        }
    }
    return true;
}

function refreshRow(grid, row) {
    var dataItem = grid.dataItem(row);

    var rowChildren = $(row).children('td[role="gridcell"]');
    var locked = 0;
    for (var i = 0; i < grid.columns.length; i++) {
        if (grid.columns.length > i + locked) {
            var columnAll = grid.columns[i];
            if (columnAll.locked == true) {
                locked = locked + 1;
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
}


function add(addend1, addend2, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(addend1) || isValueInvalid(addend2)) return null;
    result = (getNumberValue(addend1, culture) + getNumberValue(addend2, culture));
    if (arguments.length > 3)
        result = formatValue(result, format, culture);
    return result;
}

function addArray(addendArray, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(addendArray) || addendArray.length == 0) return null;
    $.each(addendArray, function () {
        if (result == undefined)
            result = getNumberValue(this, culture);
        else result += getNumberValue(this, culture);
    });
    if (arguments.length > 2)
        result = formatValue(result, format, culture);
    return result;
}

function subtract(minusend, subtrahend, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(minusend) || isValueInvalid(subtrahend)) return null;
    result = getNumberValue(minusend, culture) - getNumberValue(subtrahend, culture);
    if (arguments.length > 3)
        result = formatValue(result, format, culture);
    return result;
}

function multiply(value1, value2, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(value1) || isValueInvalid(value2)) return null;
    result = (getNumberValue(value1, culture) * getNumberValue(value2, culture));
    if (arguments.length > 3)
        result = formatValue(result, format, culture);
    return result;
}

function multiplyArray(arrayValue, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(arrayValue) || arrayValue.length == 0) return null;
    $.each(arrayValue, function () {
        if (result == undefined)
            result = getNumberValue(this, culture);
        else result = (result * getNumberValue(this, culture));
    });
    if (arguments.length > 2)
        result = formatValue(result, format, culture);
    return result;
}



function division(dividend, divisor, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(dividend) || isValueInvalid(divisor) || divisor == 0) return null;
    result = (getNumberValue(dividend, culture) / getNumberValue(divisor, culture));
    if (arguments.length > 3)
        result = formatValue(result, format, culture);
    return result;
}

function percent(baseValue, percentValue, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(baseValue) || isValueInvalid(percentValue)) return null;
    result = ((getNumberValue(percentValue, culture) / getNumberValue(baseValue, culture)) * 100);
    if (arguments.length > 3)
        result = formatValue(result, format, culture);
    return result;
}

function percentValue(baseValue, percent, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(baseValue) || isValueInvalid(percent)) return null;
    result = ((getNumberValue(baseValue, culture) * getNumberValue(percent, culture)) / 100);
    if (arguments.length > 3)
        result = formatValue(result, format, culture);
    return result;
}

function rounding(value, decimal, culture, format) {
    var result;
    if (arguments.length == 0 || isValueInvalid(value) || isValueInvalid(decimal)) return null;
    if (getNumberValue(value, culture) == null)
        result = null;
    else
        result = Number(getNumberValue(value, culture).toFixed(decimal));
    if (arguments.length > 3)
        result = formatValue(result, format, culture);
    return result;
}