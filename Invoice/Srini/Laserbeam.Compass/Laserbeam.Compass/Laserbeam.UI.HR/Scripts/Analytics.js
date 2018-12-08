function PayRangeChartRender()
{
    var employeeNum = $("#hndSelectedManagerNum").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var isSelectedRollup = ($("#hndAnalyticsIsSelectedRollup").val() == 1);
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '../Analytics/_PayRangeDistribution',
        type: "post",
        cache: false,
        data: { __RequestVerificationToken: token, employeeNum: employeeNum, isRollup: isRollup, isSelectedRollup: isSelectedRollup },
        success: function (result) {
            $('#chartRender1').html(result);
        }
    });
}
function ChartCollection() {
    var employeeNum = $("#hndSelectedManagerNum").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var isSelectedRollup = ($("#hndAnalyticsIsSelectedRollup").val() == 1);
    var token = $('input[name="__RequestVerificationToken"]').val();
    var currencyCodeNum = $("#ddlLocalCurrenciesAnalytics").data("kendoDropDownList")
    currencyCodeNum = currencyCodeNum == undefined ? 0 : currencyCodeNum._selectedValue;
    $.ajax({
        url: '../Analytics/_ChartPage',
        type: "post",
        cache: false,
        data: { __RequestVerificationToken: token, employeeNum: employeeNum, selectedCurrencyNum: currencyCodeNum, isRollup: isRollup, isSelectedRollup: isSelectedRollup },
        success: function (result) {
            $('#ChartCollection').html(result);
        }
    });
}

function SelectedChartType(selectedValue)
{
    $.ajax({
        url: '../Analytics/' + selectedValue,
        cache: false,
        success: function (result) {
            $('#chartRender').html(result);
        }
    });
}
function ddlAnalyticsManagerTreeOnSelect(e) {
    if (!showSaveWarning(e, "objChangeFlag")) {
        e._defaultPrevented = true;
        return false;
    }
    var node = e.sender.dataItem(e.node);
    CallManagerAction(node);
    $("span.k-in").parent().find('span').removeClass("highlight");
}

function setAnalyticsTreeViewDrodDownText(text) {
    $('#ddlAnalyticsManagerTreeView').find('.k-input').text(text);
}

function CallManagerAction(node, employeeJobNum, employeeNum) {
  
    var managerNum;
    if (node.ManagerName == "My Organization") {
        $("#hndIsRollup").val(1);
        managerNum = node.LoggedInEmployeeNum;
        $("#hndLoggedInEmployeeNum").val(node.LoggedInEmployeeNum);
        myOrgNode = node.ManagerName;
    }
    else {
        managerNum = node.ManagerNum;
        $("#hndIsRollup").val(0);
    }
    $("#hndSelectedManagerNum").val(managerNum);
    $("#hndMenuType").val(node.MenuType);
    var selectedValue = $("#ddlOption").data("kendoDropDownList").dataItem($("#ddlOption").data("kendoDropDownList").select()).Value;
    SelectedChartType(selectedValue);
    PayRangeChartRender();
    ChartCollection();
    $("#SelectedManagerName").text(node.ManagerName);
}
$(document).on('change', '#direct', function () {
    isSelectedRollup = false;
    $("#hndAnalyticsIsSelectedRollup").val(0);
    var selectedValue = $("#ddlOption").data("kendoDropDownList").dataItem($("#ddlOption").data("kendoDropDownList").select()).Value;
    SelectedChartType(selectedValue);
    PayRangeChartRender();
    ChartCollection();
     });

$(document).on('change', '#rollup', function () {
    isSelectedRollup = true;
    $("#hndAnalyticsIsSelectedRollup").val(1);
    var selectedValue = $("#ddlOption").data("kendoDropDownList").dataItem($("#ddlOption").data("kendoDropDownList").select()).Value;
    SelectedChartType(selectedValue);
    PayRangeChartRender();
    ChartCollection();
});
$(document).on('change', '#ddlLocalCurrenciesAnalytics', function () {
    var currencyValue = $("#ddlLocalCurrenciesAnalytics").val();
    if (currencyValue != "") {
        ChartCollection();
        var selectedValue = $("#ddlOption").data("kendoDropDownList").dataItem($("#ddlOption").data("kendoDropDownList").select()).Value;
        SelectedChartType(selectedValue);
    }
    else {
        return false;
    }
});
function ReportLnk(actionName)
{
    var employeeNum = $("#hndSelectedManagerNum").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var isSelectedRollup = ($("#hndAnalyticsIsSelectedRollup").val() == 1);
    CallActionName(actionName, employeeNum, isRollup, isSelectedRollup);
}
function ExportLnk(exportActionName) {
    var employeeNum = $("#hndSelectedManagerNum").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var isSelectedRollup = ($("#hndAnalyticsIsSelectedRollup").val() == 1);
    var groupValue = $("#ddlOption").data("kendoDropDownList").dataItem($("#ddlOption").data("kendoDropDownList").select()).Text;
    var token = $('input[name="__RequestVerificationToken"]').val();
    var form = $("<form action='" + '../Analytics/' + exportActionName + "' method='post'></form>")
     form.append("<input type='text' name='__RequestVerificationToken' value='" + token + "' />");
    form.append("<input type='text' name='employeeNum' value='" + employeeNum + "' />");
    form.append("<input type='text' name='groupBy' value='" + groupValue + "' />");
    form.append("<input type='text' name='isRollup' value='" + isRollup + "' />");
    form.append("<input type='text' name='isSelectedRollup' value='" + isSelectedRollup + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}
function CallActionName(actionName, employeeNum, isRollup, isSelectedRollup)
{
    var form = $("<form action='" + '../Analytics/' + actionName + "' method='post'></form>")
    var token = $('input[name="__RequestVerificationToken"]').val();
    
    form.append("<input type='text' name='__RequestVerificationToken' value='" + token + "' />");
    form.append("<input type='text' name='selectedEmployeeNum' value='" + employeeNum + "' />");
    form.append("<input type='text' name='isRollup' value='" + isRollup + "' />");
    form.append("<input type='text' name='isSelectedRollup' value='" + isSelectedRollup + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}
function DataParams() {
    var employeeNum = $("#hndSelectedManagerNum").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var isSelectedRollup = ($("#hndAnalyticsIsSelectedRollup").val() == 1);
    var groupValue = $("#ddlOption").data("kendoDropDownList").dataItem($("#ddlOption").data("kendoDropDownList").select()).Text;
    var currencyCodeNum = $("#ddlLocalCurrenciesAnalytics").data("kendoDropDownList")
    currencyCodeNum = currencyCodeNum == undefined ? 0 : currencyCodeNum._selectedValue;
    var token = $('input[name="__RequestVerificationToken"]').val();
    return {
        __RequestVerificationToken: token,
        employeeNum: employeeNum,
        isRollup: isRollup,
        isSelectedRollup: isSelectedRollup,
        groupBy: groupValue,
        currencyCodeNum: currencyCodeNum
    }
}
var theme = {
    color: [
        '#26B99A', '#34495E', '#BDC3C7', '#3498DB',
        '#9B59B6', '#8abb6f', '#759c6a', '#bfd3b7'
    ],

    title: {
        itemGap: 8,
        textStyle: {
            fontWeight: 'normal',
            color: '#408829'
        }
    },

    dataRange: {
        color: ['#1f610a', '#97b58d']
    },

    toolbox: {
        color: ['#408829', '#408829', '#408829', '#408829']
    },

    tooltip: {
        backgroundColor: 'rgba(0,0,0,0.5)',
        axisPointer: {
            type: 'line',
            lineStyle: {
                color: '#408829',
                type: 'dashed'
            },
            crossStyle: {
                color: '#408829'
            },
            shadowStyle: {
                color: 'rgba(200,200,200,0.3)'
            }
        }
    },

    dataZoom: {
        dataBackgroundColor: '#eee',
        fillerColor: 'rgba(64,136,41,0.2)',
        handleColor: '#408829'
    },
    grid: {
        borderWidth: 0
    },

    categoryAxis: {
        axisLine: {
            lineStyle: {
                color: '#408829'
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        }
    },

    valueAxis: {
        axisLine: {
            lineStyle: {
                color: '#408829'
            }
        },
        splitArea: {
            show: true,
            areaStyle: {
                color: ['rgba(250,250,250,0.1)', 'rgba(200,200,200,0.1)']
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        }
    },
    timeline: {
        lineStyle: {
            color: '#408829'
        },
        controlStyle: {
            normal: { color: '#408829' },
            emphasis: { color: '#408829' }
        }
    },

    k: {
        itemStyle: {
            normal: {
                color: '#68a54a',
                color0: '#a9cba2',
                lineStyle: {
                    width: 1,
                    color: '#408829',
                    color0: '#86b379'
                }
            }
        }
    },
    map: {
        itemStyle: {
            normal: {
                areaStyle: {
                    color: '#ddd'
                },
                label: {
                    textStyle: {
                        color: '#c12e34'
                    }
                }
            },
            emphasis: {
                areaStyle: {
                    color: '#99d2dd'
                },
                label: {
                    textStyle: {
                        color: '#c12e34'
                    }
                }
            }
        }
    },
    force: {
        itemStyle: {
            normal: {
                linkStyle: {
                    strokeColor: '#408829'
                }
            }
        }
    },
    chord: {
        padding: 4,
        itemStyle: {
            normal: {
                lineStyle: {
                    width: 1,
                    color: 'rgba(128, 128, 128, 0.5)'
                },
                chordStyle: {
                    lineStyle: {
                        width: 1,
                        color: 'rgba(128, 128, 128, 0.5)'
                    }
                }
            },
            emphasis: {
                lineStyle: {
                    width: 1,
                    color: 'rgba(128, 128, 128, 0.5)'
                },
                chordStyle: {
                    lineStyle: {
                        width: 1,
                        color: 'rgba(128, 128, 128, 0.5)'
                    }
                }
            }
        }
    },
    gauge: {
        startAngle: 225,
        endAngle: -45,
        axisLine: {
            show: true,
            lineStyle: {
                color: [[0.2, '#86b379'], [0.8, '#68a54a'], [1, '#408829']],
                width: 8
            }
        },
        axisTick: {
            splitNumber: 10,
            length: 12,
            lineStyle: {
                color: 'auto'
            }
        },
        axisLabel: {
            textStyle: {
                color: 'auto'
            }
        },
        splitLine: {
            length: 18,
            lineStyle: {
                color: 'auto'
            }
        },
        pointer: {
            length: '90%',
            color: 'auto'
        },
        title: {
            textStyle: {
                color: '#333'
            }
        },
        detail: {
            textStyle: {
                color: 'auto'
            }
        }
    },
    textStyle: {
        fontFamily: 'Segoe UI'
    }
};

function DepartmentGridDataBound(result) {
    $("#chartGroup").show();
    var echartPieCollapse = echarts.init(document.getElementById('IncreaseByDivision'), theme);
    echartPieCollapse.setOption({
        tooltip: {
            trigger: 'item',
            formatter: "{b} : {c}%"
        },
        toolbox: {
            show: true,
            feature: {
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                }

            }
        },
        legend: {
            enabled: false
        },
        series: [{
            type: 'pie',
            radius: [25, 90],
            center: ['50%', 170],
            roseType: 'area',
            x: '50%',
            max: 40,
            sort: 'ascending',
            data: result.sender._data
        }]

    });
}
function GenderGridDataBound(result) {
    $("#chartGroup").show();
    var echartPieCollapse = echarts.init(document.getElementById('IncreaseByDivision'), theme);
    echartPieCollapse.setOption({
        tooltip: {
            trigger: 'item',
            formatter: "{b} : {c}%"
        },

        toolbox: {
            show: true,
            feature: {
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                }

            }
        },
        legend: {
            enabled: false
        },
        series: [{
            type: 'pie',
            radius: [25, 90],
            center: ['50%', 170],
            roseType: 'area',
            x: '50%',
            max: 40,
            sort: 'ascending',
            data: result.sender._data
        }]
    });


}
function GradeGridDataBound(result) {
    $("#chartGroup").show();
    var echartPieCollapse = echarts.init(document.getElementById('IncreaseByDivision'), theme);
    echartPieCollapse.setOption({
        tooltip: {
            trigger: 'item',
            formatter: "{b} : {c}%"
        },

        toolbox: {
            show: true,
            feature: {
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                }

            }
        },
        legend: {
            enabled: false
        },
        series: [{
            type: 'pie',
            radius: [25, 90],
            center: ['50%', 170],
            roseType: 'area',
            x: '50%',
            max: 40,
            sort: 'ascending',
            data: result.sender._data
        }]
    });


}
function PayRangeDataParams() {
    $("#chartGroup").hide();
    var employeeNum = $("#hndSelectedManagerNum").val();
    var isRollup = ($("#hndIsRollup").val() == 1);
    var isSelectedRollup = ($("#hndAnalyticsIsSelectedRollup").val() == 1);
    var groupValue = $("#ddlOption").data("kendoDropDownList").dataItem($("#ddlOption").data("kendoDropDownList").select()).Text;
    return {
        employeeNum: employeeNum,
        isRollup: isRollup,
        isSelectedRollup: isSelectedRollup,
        groupBy: groupValue
    }
}
function BindAnalyticsTreeView() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../Analytics/GetAnalyticsManagerTree",
        data: { __RequestVerificationToken: token, managerNum: $("#hndSelectedManagerNum").val() },
        type: "POST",
        cache: false,
        success: function (dataValue) {
            var datasource = getTreeViewHierarchialDataSource(dataValue, "ManagerNum", "ReportingManagerNum", "IsTreeTop", "RowNumber");
            var treeView = $("#ddlAnalyticsManagerTreeView").kendoExtDropDownTreeView({
                treeview: {
                    template: '#=ShowStatusAnalyticsManagerTree(item)#',
                    dataTextField: "ManagerName",
                    dataValueField: "ManagerNum",
                    loadOnDemand: false,
                    dataSource: datasource,
                    select: ddlAnalyticsManagerTreeOnSelect
                }
            }).data("kendoExtDropDownTreeView");
            var managerNum = $("#hndSelectedManagerNum").val();
            var managerLineage = $("#hndManagerLineage").val();
            var MenuType = $("#hndMenuType").val();
            var rowNumber = $("#hndRowNumber").val();
            if (treeView != undefined) {
                treeView._treeview.expand(".k-item");
                if (rowNumber == undefined || rowNumber == null || rowNumber == "") {
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
                                    nodeSelect = treeView._treeview.findByUid(treeView._treeview.dataSource._data[0].uid);
                                    $("#hndSelectedManagerNum").val(treeView._treeview.dataSource._data[0].ManagerNum)
                                    $("#hndMenuType").val(treeView._treeview.dataSource._data[0].MenuType)
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
                        setAnalyticsTreeViewDrodDownText(nodeText);
                        treeView._treeview.select(nodeSelect);
                        if (treeView._treeview.dataSource._data.length != 0) {
                            $("#SelectedManagerName").text(treeView._treeview.dataSource._data[0].ManagerName);
                        }
                }
            }
        }
    });
}

function ShowStatusAnalyticsManagerTree(item) {
    //var finalImg = getMeritTreeViewStatusImage(item);
    var Class = "";
    if (item.OverFlow) {
        Class = " class='OverFlow'";
    }

    var finalTitle = "";
    if (item.IsOverSpent == true)
        finalTitle = "<span style='color:red' id=" + item.ManagerNum + "_" + item.ManagerNum + Class + ">" + item.ManagerName + " (<span id=" + item.ManagerNum + ">" + item.CompCompletedCount + "</span>/" + item.ReporteeCount + ")" + "</span> ";// + finalImg;
    else
        finalTitle = "<span id=" + item.ManagerNum + "_" + item.ManagerNum + Class + ">" + item.ManagerName + " (<span id=" + item.ManagerNum + ">" + item.CompCompletedCount + "</span>/" + item.ReporteeCount + ")" + "</span> ";//+ finalImg;
    return finalTitle;
}
