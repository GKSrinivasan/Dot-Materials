jQuery.expr[':'].icontains = function (a, i, m) {
    return jQuery(a).text().toUpperCase()
        .indexOf(m[3].toUpperCase()) >= 0;
};

function showSaveWarning(event, name, message) {

    if ((objChangeFlag && name == undefined) || (name != undefined && window[name] == true)) {

        swal({
            title: "Warning!",
            html: message || globalPageConstants.GlobalWarningMessage,
            type: "",
            showCancelButton: true,
            padding: 20,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#3085d6',
            confirmButtonClass: 'confirm-class',
            cancelButtonClass: 'cancel-class',
            confirmButtonText: "Continue Without Saving",
            cancelButtonText: "Got It",
            allowOutsideClick: false,
            closeOnConfirm: true,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {

                if (name == undefined) objChangeFlag = false;
                else window[name] = false;
                var switchParament = (event.type != undefined) ? event.type : event.sender.element.data().role;
                switch (switchParament) {
                    case "click":
                        event.target.click(event);
                        break;
                    case "change":
                        event.target.change(event);
                        break;
                    case "combobox":
                        event.sender.select(event.item.index());
                        event.sender.trigger("change");
                        break;
                    case "dropdownlist":
                        event.sender.select(event.item.index());
                        event.sender.trigger("change");
                        break;
                    case "treeview":
                        event.sender.select(event.sender._clickTarget);
                        event.sender.trigger("select", { node: event.sender._clickTarget });
                        if (event.sender.wrapper) {
                            event.sender.wrapper.removeClass("k-custom-visible");
                        }
                        break;

                    case "read":
                        event.sender.query({ page: event.sender._page, pageSize: event.sender._pageSize, filter: event.sender._filter, sort: event.sender._sort });
                        break;
                    case "hide":
                        $(event.target).modal('hide');
                        break;
                }
            }
        });
    }
    return (name == undefined) ? !objChangeFlag : !window[name];
}
function showAlert(message) {
    swal({
        title: "Alert!",
        html: message,
        allowOutsideClick: false,
        confirmButtonColor: '#00abf0',
        width: 350,
    });
}
var isAllowConfirm = false;
function showConfirm(event, message) {
   
    if (isAllowConfirm == true) {
        isAllowConfirm = false;
        return true;
    }
    swal({
        title: "Confirm!",
        html: message,
        type: "",
        showCancelButton: true,
        padding: 20,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#3085d6',
        confirmButtonClass: 'confirm-class',
        cancelButtonClass: 'cancel-class',
        confirmButtonText: "Cancel",
        cancelButtonText: "Ok",
        allowOutsideClick: false,
        closeOnConfirm: true,
        closeOnCancel: true,
        allowEscapeKey:false
    }, function (isConfirm) {
        if (!isConfirm) {
                isAllowConfirm = true;
            event.target.click(event);
        }
    });
    return false;
}

function successClose() {
    var id = document.getElementById("successMessageDiv");
    id.style.display = "none";
}

function infoClose() {
    var id = document.getElementById("infoMessageDiv");
    id.style.display = "none";
}

function warningClose() {
    var id = document.getElementById("warningMessageDiv");
    id.style.display = "none";
}

function errorClose() {
    var id = document.getElementById("errorMessageDiv");
    id.style.display = "none";
}



$(document).ready(function () {

    $("#successMessageDiv").hide();
    $("#infoMessageDiv").hide();
    $("#warningMessageDiv").hide();
    $("#errorMessageDiv").hide();

    $("#lnkMyNotification [data-toggle=Notificationpopover1]").popover({
        html: true, container: 'body',
        content: function () {
            return $('#notificationPopoverContent')[0].innerHTML;
        }
    });

    $("#lnkHelpDocuments [data-toggle=helpPopover]").popover({
        html: true, container: 'body',
        content: function () {
            return $('#helpPopoverContent')[0].innerHTML;
        }
    });
    $("#idHomeLink,#idLogoutLink").click(function (event) {
        if (!showSaveWarning(event)) return false;
    });



});


