var objChangeFlag = false;
var isSubmitForApproval = false;
var headerMessage = "";
var searchedEmpRowNum = 0;
var CounterValue = 0;
var isPopupEdited = false;

$.fn.center = (function () {
    this.css("position", "absolute");
    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
                                                $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
                                                $(window).scrollLeft()) + "px");
    return this;
});



var setCookie = (function (cookieName, cookieValue, expirationDays) {
    var date = new Date();
    var cookie = $("<div></div>").html($("<div></div>").html(cookieValue).text()).text();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + date.toUTCString();
    document.cookie = cookieName + "=" + cookie + "; " + expires;
});

var getCookie = (function (cname) {

    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
});

var deleteCookie = (function (name) {
    if (getCookie(name)) {
        document.cookie = name + ";expires=Thu, 01 Jan 2015 00:00:01 GMT";
    }
});
var eraseCookie = (function (name) {
    setCookie(name, "", -1);
});

var Showmessage = (function (message) {
    setTimeout(function () {
        setmessage(headerMessage);
    }, 5000);
    setmessage(message, true);
});


function setmessage(message, notStaticHeader) {
    $('#divmessage').html(message);
    if (notStaticHeader == null) {
        headerMessage = message;
    }
}
function getmessage() {
    return $('#divmessage').html();
}

var Successmessage = (function (message) {
    setTimeout(function () {
        setsuccessmessage(headerMessage);
        $("#successMessageDiv").slideUp();

    }, 5000);
    $("#successMessageDiv").slideDown();
    setsuccessmessage(message, true);
});

function closeSaveWarning(e) {
    $("#successMessageDiv").hide();
}

function closeWarning(e) {
    $("#warningMessageDiv").hide();
}

function closeerrorWarning(e) {
    $("#errorMessageDiv").hide();
    $("#successMessageDiv").hide();
}

function closeInfo(e) {
    $("#infoMessageDiv").hide();
}

var WorkforceMessage = (function () {
    setTimeout(function () {
       // setsuccessmessage(headerMessage);
        $("#WorkforceMessageDiv").slideUp();

    }, 5000);
    $("#WorkforceMessageDiv").slideDown();
    //setsuccessmessage(message, true);
});
function setsuccessmessage(message, notStaticHeader) {

    $('#idSuccessMessage').html(message);
    headerMessage = message;
    if (notStaticHeader == null) {
        headerMessage = message;
    }
}

var Infomessage = (function (message) {
    $("#infoMessageDiv").slideDown();
    setinfomessage(message, true);
});

function setinfomessage(message, notStaticHeader) {

    $('#idinfoMessage').html(message);
    if (notStaticHeader == null) {
        headerMessage = message;
    }
}

var Warningmessage = (function (message) {
    setTimeout(function () {
        setwarningmessage(headerMessage);
        $("#warningMessageDiv").slideUp();
    }, 5000);
    $("#warningMessageDiv").slideDown();
    setwarningmessage(message, true);
});

function setwarningmessage(message, notStaticHeader) {

    $('#idwarningMessage').html(message);
    if (notStaticHeader == null) {
        headerMessage = message;
    }
}


var Errormessage = (function (message) {
    setTimeout(function () {
        seterrormessage(headerMessage);
        $("#errorMessageDiv").slideUp();
    }, 5000);
    $("#errorMessageDiv").slideDown();
    seterrormessage(message, true);
});

function seterrormessage(message, notStaticHeader) {

    $('#iderrorMessage').html(message);
    if (notStaticHeader == null) {
        headerMessage = message;
    }
}

function handleError(func, argumentArray) {
    try {
        return func.apply(this, argumentArray);
    } catch (exception) {
        showAlert(exception);
        return '';
    }
}

function cancelUnSavedChanges() {
    if (objChangeFlag) {
        objChangeFlag = confirm(globalPageConstants.GlobalWarningMessage);
    }
    return !objChangeFlag;
}
function setPageChanged(isPageChanged) {
    objChangeFlag = isPageChanged;
}

function cmb_UserDropdownListChange(e) {
    var UserDropDownList = $("#UserDropDownList").data("kendoComboBox");
    var dataItem = UserDropDownList.dataItem(UserDropDownList.select());
    if (dataItem != null) {
        $.ajax({
            url: '../Dashboard/SetDashboardSession',
            type:"get",
            data: { userNum: dataItem.Value, userText: dataItem.Text },
            success: function () {
                location.href = '../Dashboard/DashboardView'
            }
        });
    }
}

function appendChildNode(dataSource, keyColumn, parentKeyColumn, node) {
    if (node["MenuType"] == undefined || node["MenuType"] == 1) {
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
    }
    return node;
}


function getTreeViewHierarchialDataSource(dataSource, keyColumn, parentKeyColumn, treeTopColumn, idColumn) {
    var topNode = [];
    var idText = (idColumn) ? idColumn : keyColumn;
    if (dataSource.length > 0) {
        $(dataSource).each(function (index, item) {
            if (item[treeTopColumn] == true)
                topNode.push(appendChildNode(dataSource, keyColumn, parentKeyColumn, item));
        });
    }
    return new kendo.data.HierarchicalDataSource({
        data: topNode,
        schema: {
            model: {
                id: idText,
                children: "ReporteeManagers"
            }
        }
    });
}

function cmb_UserDropdownListSelect(e) {
    e._defaultPrevented = !showSaveWarning(e);
}
$(function () {
    kendo.ui.Window.fn._keydown = function (originalFn) {
        var KEY_ESC = 27;
        return function (e) {
            if (e.which !== KEY_ESC) {
                originalFn.call(this, e);
            }
        };
    }(kendo.ui.Window.fn._keydown);
});


$(document).ready(function () {
    $("input").click(function (event) {
        if ($(this).is("[readonly]")) {
            this.blur();
            return false;
        }
        return true;
    });
    function validateAndRedirectToLogin(data) {
        if (data.Redirect)
            window.location = data.Redirect;
    }

    $(document).on("ajaxStart", function (e) {

        var loadingPanel = $("#ajaxLoadingPanel");
        loadingPanel.height($(window).height());
        loadingPanel.width($(window).width());
        loadingPanel.css('top', $(window).scrollTop());
        loadingPanel.css('left', $(window).scrollLeft());
        loadingPanel.show();
    }).on("ajaxStop", function (e) {
        $("#ajaxLoadingPanel").hide();
        isBrowserClose = true;
    }).on("mousewheel", '.k-window', function (e) {
        e.preventDefault();
    }).on("scroll", function (e) {
        $(".k-window").filter(':visible').center();
    }).on("ajaxSuccess", function (e, request, setting, data) {
        validateAndRedirectToLogin(data);
    }).on("ajaxError", function (e, request, setting, data) {
        if (request.responseText == undefined) return;
        if (request.responseText.match("errorBody") == "errorBody") {
            $("html").html(request.responseText);
        } else if (request.responseText.match("ErrorMessage") == "ErrorMessage") {
            var error = $.parseJSON(request.responseText);
            Showmessage(error.ErrorMessage, false);
        }
    });
});


