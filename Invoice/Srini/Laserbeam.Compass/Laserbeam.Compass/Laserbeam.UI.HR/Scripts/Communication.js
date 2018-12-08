
var managerTemplateId = 0;
var objChangeFlag = false;
$(document).on("click", "#dashboardmsg", function (e) {
    if (!showSaveWarning(e, "objChangeFlag")) {
        e._defaultPrevented = true;
        return false;
    }
    else {
        managerTemplateId = 0;
        $("#mainemptydiv").show();
        $.ajax({
            url: '../Communication/DashboardMessage',
            data: null,
            type: 'get',
            cache: false,
            success: function (result) {
                $("#divDataBind").html(result);
            }
        });
    }
});

$(document).on("click", ".noemptydiv", function (e) {
    $("#mainemptydiv").hide();
});

$(document).ready(function () {
    $("#lnkCommunicationTemplate").trigger("click");
    $("#mainemptydiv").hide();
    $("#btnClearFilter").hide();
    $('#divddlUserGrp').hide();
    $('#dashboardEmailUserGrid').hide();
    $("#btnCommunicationClear").hide();
    $("#yetToLoginUserList").hide();
    $("div.bhoechie-tab-menu>div.list-group>a").click(function (e) {
        if (!showSaveWarning(e, "objChangeFlag")) {
            e._defaultPrevented = true;
            return false;
        }
        e.preventDefault();
        $(this).siblings('a.active').removeClass("active");
        $(this).addClass("active");
        var index = $(this).index();
        $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
        $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");
        objChangeFlag = false;
    });
});

function editorChanged() {
    objChangeFlag = true;
}


var appEmailTemplateID = 0;
var appEmailTemplateKey = "";
var appMessageId = 0;

function ddlGetManageCommunicationTreeViewOnSelect(e) {
    $('#txtEmailNotificationKey').show();
    $('#editorRegion').show();
    $('#subjectName').show();


    var node = e.sender.dataItem(e.node);
    if (node.IsTreeTop == true || !showSaveWarning(e, "objChangeFlag")) {
        appEmailTemplateID = 0;
        appEmailTemplateKey = "";
        $('#divddlUserGrp').hide();
        e._defaultPrevented = true;
        return false;
    }

    else {
        appEmailId = node.AppEmailId;
        $('#divddlUserGrp').show();
        $('#dashboardEmailUserGrid').show();
        $("#txtEmailNotificationKey")[0].value = node.EmailSubject;
        $('#txtEmailNotificationKey').attr('readonly', 'true');
        var editor= $('#emailnotificationEditor').data("kendoEditor");
        editor.body.contentEditable = false;
        $(".k-editor-toolbar").hide();
        $("#emailnotificationEditor").data("kendoEditor").value(node.EmailBody);
    }
}

function ddlGetManagerTemplateTreeViewOnSelect(e) {
    var node = e.sender.dataItem(e.node);
    if (node.IsTreeTop == true || !showSaveWarning(e, "objChangeFlag")) {
        e._defaultPrevented = true;
        return false;
    }    
    else {
        managerTemplateId = node.AppEmailId;
        $("#txtTemplateEmail").val(node.EmailSubject);
        $("#managerTemplateEditor").data("kendoEditor").value(node.EmailBody);
        $(".k-in").addClass("k-state-selected");
        if (node.ParentMenuNum == 555555)
            $("#btnManagerTemplateDelete").hide();
        else
            $("#btnManagerTemplateDelete").show();
    }
}


function appendManageCommunicationChildNode(dataSource, keyColumn, parentKeyColumn, node) {
    var childNodes = dataSource.filter(function (item) {
        if (item[parentKeyColumn] == node[keyColumn])
            return item;
    });
    $(childNodes).each(function (index, item) {
        appendManageCommunicationChildNode(dataSource, keyColumn, parentKeyColumn, item);
    });
    if (childNodes.length > 0) {
        node.EmailParent = childNodes;
    }
    return node;
}


function appendManagerTemplateChildNode(dataSource, keyColumn, parentKeyColumn, node) {
    var childNodes = dataSource.filter(function (item) {
        if (item[parentKeyColumn] == node[keyColumn])
            return item;
    });
    $(childNodes).each(function (index, item) {
        appendManageCommunicationChildNode(dataSource, keyColumn, parentKeyColumn, item);
    });
    if (childNodes.length > 0) {
        node.EmailParent = childNodes;
    }
    return node;
}

function getManageCommunicationTreeViewDataSource(dataSource, keyColumn, parentKeyColumn, treeTopColumn) {
    var topNode = [];
    if (dataSource.length > 0) {
        $(dataSource).each(function (index, item) {
            if (item[treeTopColumn] == true)
                topNode.push(appendManageCommunicationChildNode(dataSource, keyColumn, parentKeyColumn, item));
            else return false;
        });
    }
    return new kendo.data.HierarchicalDataSource({
        data: topNode,
        schema: {
            model: {
                id: keyColumn,
                children: "EmailParent"
            }
        }
    });
}


function getManagerTemplateTreeViewDataSource(dataSource, keyColumn, parentKeyColumn, treeTopColumn) {
    var topNode = [];
    if (dataSource.length > 0) {
        $(dataSource).each(function (index, item) {
            if (item[treeTopColumn] == true)
                topNode.push(appendManagerTemplateChildNode(dataSource, keyColumn, parentKeyColumn, item));
            else return false;
        });
    }
    return new kendo.data.HierarchicalDataSource({
        data: topNode,
        schema: {
            model: {
                id: keyColumn,
                children: "EmailParent"
            }
        }
    });
}

function BindManageCommunicationTemplateTreeView() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("../Communication/BindManageCommunicationTemplateTreeView", { __RequestVerificationToken: token}, function (datavalue) {
        var datasource = getManagerTemplateTreeViewDataSource(datavalue, "AppEmailId", "ParentMenuNum", "IsTreeTop");

        var treeView = $("#ddlbtnManagerTemplateTree").kendoExtDropDownTreeView({
            treeview: {
                template: '#=item.EmailSubject#',
                dataTextField: "EmailSubject",
                dataValueField: "AppEmailId",
                loadOnDemand: false,
                dataSource: datasource,
                select: ddlGetManagerTemplateTreeViewOnSelect
            }
        }).data("kendoExtDropDownTreeView");
        treeView._treeview.expand(".k-item");

        var optionLabelOfTree = "-Select-";
        $('#ddlbtnManagerTemplateTree').find('.k-input').text(optionLabelOfTree);
    });
}


$("#btnMessageCommunicationSave").on("click", function () {
    var emailTempList = $("#ddlMessageCommunicationTemplateTree").data("kendoExtDropDownTreeView")._treeview.dataSource.data();
    var ddlSubject = $("#txtEmailKey")[0].value;
    var bodytext = getEmailBody();
    var email_Arr = [];
    var token = $('input[name="__RequestVerificationToken"]').val();


    for (var i = 0; i < emailTempList.length; i++) {
        for (var j = 0; j < emailTempList[i].EmailParent.length; j++) {
            email_Arr.push(emailTempList[i].EmailParent[j].EmailSubject.toLowerCase().trim());
        }
    }

    if (ddlSubject == "" && bodytext == "") {
        alert("Please provide email Subject and Body"); return false;
    }
    else if (ddlSubject == "" && bodytext != "") {
        alert("Please provide email Subject"); return false;
    }
    else if (bodytext == "" && ddlSubject != "") {
        alert("Please provide email Body"); return false;
    }
    else {
        if (appEmailId == 0) { action = "Save"; } else { action = "Update"; }
        var sub = ddlSubject.toLowerCase();
        var index = $.inArray(sub, email_Arr);
        if (index > -1 && appEmailId == 0) {
            Successmessage("Email Subject already exists,Error");
            return false;
        }
        else {
            $.ajax({
                url: '../Communication/SaveAndDeleteCommunicationEmail',
                cache: false,
                type: "POST",
                data: JSON.stringify({ __RequestVerificationToken:token,EmailSubject: ddlSubject, EmailBody: bodytext, AppEmailID: appEmailId }),
                dataType: 'html',
                contentType: "application/json; charset=utf-8",
                success: function (message) {
                    objChangeFlag = false;
                    $('#renderCommunicationTemplate').html(message);
                    if (action == "Save") { Successmessage("Email Template Saved Successfully," + "Success"); }
                    else { Successmessage("Email Template Updated Successfully," + "Success"); }

                }
            });
        }
    }
});


$("#chkBoxSelectAll").on("click", function (e) {
    if (this.checked) {
        $("#checkBx input[type=checkbox]").each(function () {
            this.checked = true;
        });
    }
    else {
        $("#checkBx input[type=checkbox]").each(function () {
            this.checked = false;
        });
    }
});

function getEmailBody() {
    var kendoeditor = $("#editor").data("kendoEditor");
    var editor = kendoeditor.value();
    return editor;
};

//function getEmailTemplateBody() {
//    var kendoeditor = $("#managerTemplateEditor").data("kendoEditor");
//    var editor = kendoeditor.value();
//    return editor;
//};

$(document).on("change", "#txtTemplateEmail", function (e) {
    objChangeFlag = true;
});

$(document).on("click", "#btnManagerTemplateDelete", function (e) {
    if (managerTemplateId == 0) {
        return false
    }
    else {
        if (!showConfirm(e,"Are you sure to delete?")) return false;
        else {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: '../Communication/_ManagerCommunicationTemplate',
                data: { __RequestVerificationToken: token, AppEmailID: managerTemplateId },
                type: "POST",
                success: function (message) {
                    objChangeFlag = false;
                    $('#rendermanagertemplatetree').html(message);
                    $("#txtTemplateEmail").val('');
                    var kendoeditor = $("#managerTemplateEditor").data("kendoEditor");
                    kendoeditor.value('');
                    $('#ddlbtnManagerTemplateTree').find('.k-input').text("-Select-");
                    Successmessage("Email Template Deleted Successfully");
                    managerTemplateId = 0;
                }
            });
        }
    }
});

function GetParamValue() {
    var selectedtypeNum = $("#hndselectedtype").val();
    var a = $("#hndvalue").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    return {
        __RequestVerificationToken:token,
        checkRole: selectedtypeNum == 1 ? "true" : "false",
        userRoleNum: a
    }
}

$(document).on("click", "#btnManagerTemplateSave", function (e) {
    var emailTempList = $("#ddlbtnManagerTemplateTree").data("kendoExtDropDownTreeView")._treeview.dataSource.data();
    var ddlSubject = $("#txtTemplateEmail")[0].value;    
    var bodytext = $("#managerTemplateEditor")[0].value;//getEmailTemplateBody();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (ddlSubject == "" && bodytext == "") {
        alert("Please provide email Subject and Body"); return false;
    }
    else if (ddlSubject == "" && bodytext != "") {
        alert("Please provide email Subject"); return false;
    }
    else if (bodytext == "" && ddlSubject != "") {
        alert("Please provide email Body"); return false;
    }
    else if (ddlSubject.length > 500) {
        alert("The Email Subject Range should not exceed 500 characters")
    }
    else {
        (managerTemplateId == 0)?action = "Save":action = "Update";
        if (objChangeFlag == false) {
            return false;
        }
        else {
            $.ajax({
                url: "../Communication/_ManagerCommunicationTemplate",
                type: "POST",
                data: {__RequestVerificationToken:token, EmailSubject: ddlSubject, EmailBody: bodytext, AppEmailID: managerTemplateId },
                success: function (message) {
                    $('#rendermanagertemplatetree').html(message);
                    objChangeFlag = false;
                    if (action == "Save") { Successmessage("Email Template Saved Successfully"); }
                    else { Successmessage("Email Template updated Successfully"); }
                }
            });
        }
    }

});

$(document).on("click", "#btnDashboardDelete", function (e) {
    var appMessageID = $("#ddlDashboardMessage").data("kendoDropDownList")._selectedValue;
    if (appMessageID == "") {
        return false
    }
    else {
        if (!showConfirm(e,"Are you sure to delete?")) return false;
        else {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: '../Communication/DeleteDashboardMessage',
                type:'Post',
                cache: false,
                data: {
                    __RequestVerificationToken:token,
                    AppMessageID: appMessageID,
                    messageId: $("#ddlDashboardMessage").data("kendoDropDownList")._selectedValue
                },
                type: "POST",
                async: "true",
                success: function (result) {
                    $("#ddlDashboardMessage").data("kendoDropDownList").value(0);
                    $("#ddlDashboardMessage").data("kendoDropDownList").dataSource.read();
                    $("#ddlDashboardMessage").data("kendoDropDownList").text("-Configured email list-");
                    $("#dashboardSubject")[0].value = " ";
                    $("#dashBoardMessageEditor").data("kendoEditor").value("");
                    Successmessage("Message Template Deleted Successfully," + "Success");
                    $("#yetToLoginUserList").hide();
                    $("#hideUserGrid").hide();
                }
            });
        }
    }
});


$("#checkkBoxSelectAll").on("click", function (e) {
    if (this.checked) {
        $("#chkBox input[type=checkbox]").each(function () {
            this.checked = true;
        });
    }
    else {
        $("#chkBox input[type=checkbox]").each(function () {
            this.checked = false;
        });
    }
});

var isFilter = false;

$(document).on("click", "#lnkCommunicationTemplate", function (e) {
    if (!showSaveWarning(e, "objChangeFlag")) {
        e._defaultPrevented = true;
        return false;
    }
    else {
        $.ajax({
            url: '../Communication/_ManagerCommunicationTemplate',
            data: null,
            type: 'get',
            cache: false,
            success: function (result) {
                $("#divDataBind").html(result);
            }
        });
    }
});

$(document).on("click", "#lnkEmailCommunication", function (e) {
    if (!showSaveWarning(e, "objChangeFlag")) {
        e._defaultPrevented = true;
        return false;
    }
    else {
        managerTemplateId = 0;
        $.ajax({
            url: '../Communication/EmailCommunication',
            data: null,
            type: 'get',
            cache: false,
            success: function (result) {
                $("#divDataBind").html(result);
            }
        });
    }
});


$(document).on("click", "#btnClearFilter", function (event) {
    clearFilterSort(true);
    $("#btnClearFilter").hide();
    $("#filter").show();
});

$(document).on("click", "#btnDashboardMessageFilter", function () {
    $("body").css("overflow", "hidden");
    $.ajax({
        url: '../Communication/_DashboardMessageFilterSort',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        success: function (result) {
            $("#communicationFilter").html(result);
            var wndFilterSort = $("#communicationFilter").data("kendoWindow");
            wndFilterSort.options.gridName = "dashboardMessageUserGrid";
            wndFilterSort.center().open();
            isFilter = true;
        }
    });
});


$(document).on("click", "#filter", function () {
    $("body").css("overflow", "hidden");
    $.ajax({
        url: '../Communication/_EmailNotificationFilterSort',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        success: function (result) {
            $("#emailcommunicationFilter").html(result);
            var wndFilterSort = $("#emailcommunicationFilter").data("kendoWindow");
            wndFilterSort.options.gridName = "dashboardEmailUserGrid";
            wndFilterSort.center().open();
            isFilter = true;
        }
    });
});

function emailNotificationExport() {
    var value = appEmailId;
    var token = $('input[name="__RequestVerificationToken"]').val();
    var form = $("<form action='../Communication/GetEmailNotificationExportData' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + token + "' />");
    form.append("<input type='text' name='appEmailID' value='" + value + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

function BindManageCommunicationTreeView() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("../Communication/BindManageCommunicationTreeView", { __RequestVerificationToken: token, }, function (datavalue) {
        var datasource = getManageCommunicationTreeViewDataSource(datavalue, "AppEmailId", "ParentMenuNum", "IsTreeTop");

        var treeView = $("#ddlMessageCommunicationTemplateTree").kendoExtDropDownTreeView({
            treeview: {
                template: '#=item.EmailSubject#',
                dataTextField: "EmailSubject",
                dataValueField: "AppEmailId",
                loadOnDemand: false,
                dataSource: datasource,
                select: ddlGetManageCommunicationTreeViewOnSelect
            }
        }).data("kendoExtDropDownTreeView");
        treeView._treeview.expand(".k-item");

        var optionLabelOfTree = "-Select-";
        $('#ddlMessageCommunicationTemplateTree').find('.k-input').text(optionLabelOfTree);
    });
}

function BindEmailNotificationTreeview() {
    var token = $('input[name="__RequestVerificationToken"]').val();
 
    $.post("../Communication/_SendEmailNotificationTree",{__RequestVerificationToken: token}, function (dataValue) {
        var datasource = getEmailNotificationHierarchialDataSource(dataValue, "UserRoleNum", "ReportingManagerNum", "IsTreeTop");
        var treeView = $("#ddlEmailNotificationTreeview").kendoExtDropDownTreeView({
            treeview: {
                template: '#= item.UserRoleName#',
                dataTextField: "UserRoleName",
                dataValueField: "UserRoleNum",
                loadOnDemand: true,
                dataSource: datasource,
                select: BindEmailNotificationTreeviewOnSelect
            }
        }).data("kendoExtDropDownTreeView");
        treeView._treeview.expand(".k-item");
        var nodeText = "";
        nodeText = "-Select- ";
        treeView.bind("select", function (e) {
            if ($("#hndIsNodeSelected").val() == 1) {
                $('#ddlEmailNotificationTreeview').find('.k-input').text($("#hndSelectedTreeText").val());
            }
            else {
                var droptext = $(e.node).children("div").text();
                $('#ddlEmailNotificationTreeview').find('.k-input').text(droptext);
            }
        });

        $('#ddlEmailNotificationTreeview').find('.k-input').text(nodeText);
    });
}

function getEmailNotificationHierarchialDataSource(dataSource, keyColumn, parentKeyColumn, treeTopColumn) {
    var topNode = [];
    if (dataSource.length > 0) {
        $(dataSource).each(function (index, item) {
            if (item[treeTopColumn] == true)
                topNode.push(appendChildNode(dataSource, keyColumn, parentKeyColumn, item));
            else return false;
        });
    }
    return new kendo.data.HierarchicalDataSource({
        data: topNode,
        schema: {
            model: {
                id: keyColumn,
                children: "ReporteeManagers"
            }
        }
    });
}

function appendChildNode(dataSource, keyColumn, parentKeyColumn, node) {
    var childNodes = dataSource.filter(function (item) {
        if (item[parentKeyColumn] == node[keyColumn])
            return item;
    });
    $(childNodes).each(function (index, item) {
        appendChildNode(dataSource, keyColumn, parentKeyColumn, item);
    });
    if (childNodes.length > 0) {
        node.ReporteeManagers = childNodes;
    }
    return node;
}

function BindEmailNotificationTreeviewOnSelect(e) {
    var node = e.sender.dataItem(e.node);
    var nodes = e.sender;
    $("#hndIsNodeSelected").val(node.UserRoleNum);
    $("#hndvalue").val(node.UserRoleNum);
    $("#hndtext").val(node.UserRoleName);
    $("#hndselectedtype").val(node.SelectedType);
    var gridData = $("#dashboardEmailUserGrid").data("kendoGrid");
    gridData.dataSource.read();
    var root = $(".k-item");
    if ((node.IsTreeTop == true) || !showSaveWarning) {
        nodes.select(root);
        $("#hndIsNodeSelected").val(1);
        e._defaultPrevented = true;
        return false;
    }
    else {
        $("#hndIsNodeSelected").val(0);
    }
}

function dashboardMessagesExport(){
    var appMessageId = $("#ddlDashboardMessage").data("kendoDropDownList")._selectedValue;
    var value = appMessageId;
    var token = $('input[name="__RequestVerificationToken"]').val();
    var form = $("<form action='../Communication/GetDashboardMessageExportData' method='post'></form>")
    form.append("<input type='text' name='__RequestVerificationToken' value='" + token + "' />");
    form.append("<input type='text' name='appMessageId' value='" + value + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
}

$(document).on("click", "#btnCommunicationClear", function (event) {
    clearFilterSort(true);
    $("#btnCommunicationClear").hide();
    $("#btnDashboardMessageFilter").show();
});
function BindDashboardMessageSearchCriteriaTreeView() {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.post("../Communication/SendDashboardMessageSearchCriteriaTree", { __RequestVerificationToken: token}, function (dataValue) {
        var datasource = getDashboardMessageTreeViewDataSource(dataValue, "UserRoleNum", "ReportingManagerNum", "IsTreeTop");
        var treeView = $("#ddlDashboardMessageTreeView").kendoExtDropDownTreeView({
            treeview: {
                template: '#= item.UserRoleName#',
                dataTextField: "UserRoleName",
                dataValueField: "UserRoleNum",
                loadOnDemand: true,
                dataSource: datasource,
                select: ddlDashboardMessageTreeViewOnSelect
            }
        }).data("kendoExtDropDownTreeView");
        treeView._treeview.expand(".k-item");
        var nodeText = "";
        nodeText = "-Select- ";
        treeView.bind("select", function (e) {

            if ($("#hndIsNodeSelected").val() == 1) {
                $('#ddlDashboardMessageTreeView').find('.k-input').text($("#hndSelectedTreeText").val());
            }
            else {
                var droptext = $(e.node).children("div").text();
                $('#ddlDashboardMessageTreeView').find('.k-input').text(droptext);
            }
        });
        $('#ddlDashboardMessageTreeView').find('.k-input').text(nodeText);
    });
}

function getDashboardMessageTreeViewDataSource(dataSource, keyColumn, parentKeyColumn, treeTopColumn) {
    var topNode = [];
    if (dataSource.length > 0) {
        $(dataSource).each(function (index, item) {
            if (item[treeTopColumn] == true)
                topNode.push(appendChildNode(dataSource, keyColumn, parentKeyColumn, item));
            else return false;
        });
    }
    return new kendo.data.HierarchicalDataSource({
        data: topNode,
        schema: {
            model: {
                id: keyColumn,
                children: "ReporteeManagers"
            }
        }
    });
}








