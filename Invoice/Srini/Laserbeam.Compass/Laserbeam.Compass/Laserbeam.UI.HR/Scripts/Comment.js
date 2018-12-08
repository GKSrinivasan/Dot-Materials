var meritCommetCount = 0;
var promotionCommetCount = 0;
var adjustmentCommetCount = 0;
var lastEditedCommentNum = 0;
$(document).ready(function () {

    $(document).on("click", "#btnCommentRefresh", function (e) {
        if (!showSaveWarning(event, "inputChanged")) return false;
        $("#EmpCommentNum").val(0);
        $("#CompensationTypeNum").val(0);
        $("#IsEditItem").val(false);
        $("#comment").text('');
        $("#btnCommentRefresh").hide();
        return false;
    });
});
var inputChanged = false;
function ClearCommentChangeFlag() {
    inputChanged = false;
}
function SetCommentChangeFlag() {
    inputChanged = true;
}
function closeAfterSaveSlide() {
    ClearCommentChangeFlag();
    isPopupEdited = true;
    $("#divComment").modal('hide');
}
function Change(e) {
    inputChanged = true;
}
var defaultTools = kendo.ui.Editor.defaultTools;
defaultTools["insertLineBreak"].options.shift = false;
delete defaultTools["insertParagraph"].options;
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
        showAlert("Exceeds characters Limit");
        $(countID)[0].value = CharacterCount(id);
    }
}
function Select(e) {
    this.focus();
}
function editClick(empCommentNum, num) {
    var ItemCommentValue = $("#btnEdit_" + empCommentNum).attr('data-myitemid');
    $("#editBox" + num + "_" + empCommentNum).text(ItemCommentValue);
    $("#comment").css("display", "none");
    $("#btnCommentSlideSave").css("display", "none");
    $("#saveBtn" + num + "_" + empCommentNum).show();
    $("#closeBtn" + num + "_" + empCommentNum).show();
    var isCollapsed = $("#editBox" + num + "_" + empCommentNum).is(":visible");
    if (!isCollapsed) {
        $("#generalText" + num + "_" + empCommentNum).css("display", "none");
        $("#btnEdit_" + empCommentNum).hide();
    }
    else {
        $("#btnEdit_" + empCommentNum).show();
        $("#generalText" + num + "_" + empCommentNum).show();
        $("#comment").show();
        $("#btnCommentSlideSave").show();
    }

    if (lastEditedCommentNum != empCommentNum) {
        if (lastEditedCommentNum != 0) {
            var isCollapsed = $("#editBox" + num + "_" + lastEditedCommentNum).is(":visible");
            if (isCollapsed) {
                $("#generalText" + num + "_" + lastEditedCommentNum).show();
                $("#collapseedit" + num + "_" + lastEditedCommentNum).removeClass("in");
            }
            //else
            //$("#generalText" + num + "_" + lastEditedCommentNum).css("display", "none");
        }
        lastEditedCommentNum = empCommentNum;
    }
}


function saveComment(empCommentNum, CompensationTypeNum, num) {
    var commentType = $("#editBox" + num + "_" + empCommentNum).attr('data-commentLabel');
    $("#IsEditItem").val(true);
    var ItemCommentValue = $("#editBox"+num+"_"+empCommentNum).val();
    $("#EmpCommentNum").val(empCommentNum);
    if (commentType == "General") {
        $("#commentType").val('Compensation');
    }
    else if (commentType == "Merit") {
        $("#commentType").val('CompensationMeritMandit');
    }
    else {
        $("#commentType").val(commentType);
    }

    $("#comment").text(ItemCommentValue)
    $("#CompensationTypeNum").val(CompensationTypeNum);
    $("#btnEdit_" + empCommentNum).show();
    $("#frmComment").submit();
}

function cancelComment(empCommentNum, CompensationTypeNum, num) {
    //var  existingComment = $('p').text();
    //$("#comment").text(existingComment)
    $("#generalText" + num + "_" + empCommentNum).show();
    //$("#editBox" + num + "_" + empCommentNum).css("display", "none");
    $("#saveBtn" + num + "_" + empCommentNum).hide();
    $("#closeBtn" + num + "_" + empCommentNum).hide();
    $("#collapseedit" + num + "_" + empCommentNum).removeClass("in");
    $("#collapseedit" + num + "_" + empCommentNum).addClass("collapse");
    $("#comment").show();
    $("#btnCommentSlideSave").show();
    $("#btnEdit_" + empCommentNum).show();
    //$("#closeDiv").hide();
}
