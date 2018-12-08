var stpcount = 1;
var donutClass;
var oldValue;
$(document).ready(function () {
    if (stepCount == 0)
        stepCount=1
    //$('html, body').animate({ scrollTop: '0px' }, 800);
    switch (stepCount + 1) {
        case 1:
            $("#step1").trigger("click");
            break;
        case 2:
            $("#step2").trigger("click");
            break;
        case 3:
            $("#step3").trigger("click");
            break;
        case 4:
            $("#step4").trigger("click");
            break;
        case 5:
            $("#step5").trigger("click");
            break;
        //case 6:
        //    Wizard();
        //    break;
        default:
            $("#step1").trigger("click");
    }
});

$(document).on("click", "#btnLoadData", function (e) {
    
    var form = $("<form action='../Workforce/GetExportTrainingData' method='post'></form>")
    //form.append("<input type='text' name='xmlProcessNum' value='" + xmlProcessNum + "' />");
    form.appendTo("body");
    form.submit();
    form.remove();
});

$(document).on("click", "#btnSampleData", function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../Wizard/UpdateStepCount",
        type: "post",
        data: {
            __RequestVerificationToken: token,
            Step: 5
        },
        success: function (result) {
            window.location.href = "../Dashboard/DashboardView"

        }
    });
   
});
$(document).on("click", "#btnBackToBudget", function (e) {
    $("#step4").trigger("click");
});
$(document).on("click", "#btnBack", function (e) {
    $("#step2").trigger("click");
});
$(document).on("click", "#btnNext", function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    WorkForceSave();
    $.ajax({
        url: "../Wizard/UpdateStepCount",
        type: "post",
        data: {
            __RequestVerificationToken : token,
            Step: 3
        },
        success: function (result) {
            $("#step4").trigger("click");

        }
    });
});
$(document).on("click", "#btnBackToWelcome", function (e) {
    $("#step1").trigger("click");
});
$(document).on("click", "#btnNextToFields", function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var value = saveRules();
    if (value == true) {
        $.ajax({
            url: "../Wizard/UpdateStepCount",
            type: "post",
            data: {
                __RequestVerificationToken:token,
                Step: 3
            },
            success: function (result) {
                //$("#step3").trigger("click");
            }
        });
    }
});

$(document).on("keydown", "#budgetPct", function (e) {
    allowWizardDecimalNumberOnlyInput(e);
});

$(document).on("keypress", "#budgetPct", function (e) {
    var txt = $("#budgetPct").val();
    if (txt.indexOf(".") > -1 && txt.split(".")[1].length > 1) {
        var substr = txt.split(".")[1].substring(0, 1);
        $("#budgetPct").val(txt.split(".")[0] + "." + substr);
    }
});

$(document).on("change", "#budgetPct", function (e) {
    var dataValue = getNumberValue($(e.target).val());
    $("#wizardBudgetPctChart").removeClass("p" + $("#wizardBudgetPct")[0].innerHTML.replace("%", ""));
    $("#wizardBudgetPct")[0].innerHTML = (dataValue != null) ? dataValue + "%" : "";
    $("#wizardBudgetPctChart").removeClass(donutClass);
    $("#wizardBudgetPctChart").addClass("p" + Math.round((dataValue != null) ? (dataValue > 100) ? 100 : dataValue : 0));
    donutClass = "p" + Math.round((dataValue != null) ? (dataValue > 100) ? 100 : dataValue : 0);
    oldValue = (dataValue < 999) ? dataValue : oldValue;
    if (dataValue > 999) {
        showAlert("Value exceeds the maximum limit");
        $(this).val(oldValue);
        $("#wizardBudgetPct")[0].innerHTML = (oldValue != null) ? oldValue + "%" : "";
        $("#wizardBudgetPctChart").removeClass(donutClass);
        $("#wizardBudgetPctChart").addClass("p" + Math.round((oldValue != null) ? (oldValue > 100) ? 100 : oldValue : 0));
        donutClass = "p" + Math.round((oldValue != null) ? (oldValue > 100) ? 100 : oldValue : 0);
    }
});
//
$(document).on("click", "#btnApplyBudgetPct", function (e) {
    
    var budgetPct = $("#budgetPct").val();
    if (budgetPct != "")
    {
        $("#Budgetpercent").text(budgetPct + "%");
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: "../BudgetPlan/PutBudgetPct",
            type: "post",
            data: {
                __RequestVerificationToken: token, BudgetPercent: budgetPct, isProration: false,filterEmployee:""
            },
            success: function (result) {
                Successmessage("Great! You have successfully defined the budget.");
                objChangeFlag = false;

            }
        });
    }    
    else
    {
        Warningmessage("Please enter the budget pct and continue");
        e.preventDefault();
    }
});
//
$(document).on("click", "#btnChooseFields", function (event) {
    $("#step3").trigger("click");
});
$(document).on("click", "#btnComplete", function (event) {
    var budgetPct = $("#budgetPct").val();
    if (budgetPct != "")
    {
    $("#Budgetpercent").text(budgetPct + "%");
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../BudgetPlan/PutBudgetPct",
        type: "post",
        data: {
            __RequestVerificationToken: token, BudgetPercent: budgetPct, isProration: false,filterEmployee: ""
        },
        success: function (result) {
            Successmessage("Great! You have successfully defined the budget.");
            completeUpdate();

        }
    });
    }
    else {
        Warningmessage("Please enter the budget pct and continue");
        event.preventDefault();
    }
});

$(document).on("click", "#btnStart", function (event) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../Wizard/UpdateStepCount",
        type: "post",
        data: {
            __RequestVerificationToken:token,
            Step: 1
        },
        success: function (result) {
            $("#step2").trigger("click");

        }
    });

});
$(document).on("click", "#btnPrevious", function (event) {
    if (stpcount == 5)
        $("#step4").trigger("click");
    else if (stpcount == 4)
        $("#step2").trigger("click");
});

$(document).on("click", "#btnNext1", function (event) {
    if (stpcount == 2)
        $("#step4").trigger("click");
    else if (stpcount == 4)
        $("#step5").trigger("click");
});

$(document).on("click", "#btnskip", function (event) {
        stpcount = 0;
        $("#step5").trigger("click");
});
$(document).on("click", "#btnModifyComponents ,#btnModifyRules", function (event) {
    stpcount = 0;
    $("#step2").trigger("click");
});
$(document).on("click", "#btnModifyBudget", function (event) {
    stpcount = 0;
    $("#step4").trigger("click");
});
$(document).on("click", "#step1", function (event) {

    Welcome();
    $("#progressBar").css({ "width": "20%" });
    $("#step1").css({ "border": "3px solid #1079BD" });
    $("#step2").css({ "background": "orange" });
    $("#step3").css({ "background": "orange" });
    $("#step4").css({ "background": "orange" });
    $("#step5").css({ "background": "orange" });
});
$(document).on("click", "#step2", function (event) {

    enableFeatures();
    $(".f1-progress-line").css({ "width": "0%" });
    $("#step2 .f1-step-icon").css({ "background": "#f35b3f", "width": "48px", "height": "48px","border": "1px solid #f35b3f", "font-size": "22px", "line-height": "48px", "color": "white" });
    $("#step2 p").css({ "color": "#f35b3f" });
    $("#step5 .f1-step-icon").css({ "background": "#ccc", "color": "#ccc", "border": "1px solid #ccc", "width": "40px", "height": "40px", "line-height": "40px", "font-size": "16px" });
    $("#step5 p").css({ "color": "#ccc" });
    $("#step4 .f1-step-icon").css({ "background": "#ccc", "color": "#fff", "border": "1px solid #ccc", "width": "40px", "height": "40px", "line-height": "40px" });
    $("#step4 p").css({ "color": "#ccc" });
    $("#btnPrevious").css({ "background": "#ccc", "color": "#fff" }).prop('disabled', true);
    $("#btnNext1").css({ "background": "#06b3a2", "color": "#fff" }).prop('disabled', false);
    stpcount = 2;
    $("#btnskip").show();
    //$("#step2").css({ "border": "4px solid orange", "background": "#fff", "width": "50px", "height": "50px" });

    //$("#step1").css({ "background": "#ccc", "color": "#fff" });
    //$("#step3").css({ "background": "#ccc", "color": "#fff" });
    //$("#step4").css({ "background": "#ccc", "color": "#fff" });
    //$("#step5").css({ "background": "#ccc", "color": "#fff" });


});
$(document).on("click", "#step3", function (event) {
    chooseFields();
    $(".f1-progress-line").css({ "width": "50%" });
    //$("#step3").css({ "border": "4px solid orange", "background": "#fff","color":"orange" });
    //$("#step2").css({ "background": "#ccc","color":"#fff" });
    //$("#step1").css({ "background": "#ccc", "color": "#fff" });
    //$("#step4").css({ "background": "#ccc", "color": "#fff" });
    //$("#step5").css({ "background": "#ccc", "color": "#fff" });
});
$(document).on("click", "#step4", function (event) {
    if (stpcount != 0)
    $("#btnNextToFields").trigger("click");
    defineBudget();  
    $(".f1-progress-line").css({ "width": "50%" });
    $("#step4 .f1-step-icon").css({ "background": "#f35b3f", "width": "48px", "height": "48px","border": "1px solid #f35b3f", "font-size": "22px", "line-height": "48px", "color": "white" });
    $("#step4 p").css({ "color": "#f35b3f" });
    $("#step5 .f1-step-icon").css({ "background": "#ccc", "color": "#ccc", "border": "1px solid #ccc", "width": "40px", "height": "40px", "line-height": "40px", "font-size": "16px" });
    $("#step5 p").css({ "color": "#ccc" });
    $("#step2 .f1-step-icon").css({ "background": "#fff", "color": "#f35b3f", "border": "1px solid #f35b3f", "width": "40px", "height": "40px", "line-height": "40px" });
    $("#step2 p").css({ "color": "#f35b3f" });
    $("#btnPrevious").css({ "background": "#06b3a2","color":"#fff" }).prop('disabled', false);
    $("#btnNext1").css({ "background": "#06b3a2", "color": "#fff" }).prop('disabled', false);
    stpcount = 4;
    $("#btnskip").show();
    //$("#step4").css({ "border": "4px solid orange", "background": "#fff" });
    //$("#step5").css({ "background": "#ccc", "color": "#fff" });
    //$("#step3").css({ "background": "#ccc", "color": "#fff" });
    //$("#step2").css({ "background": "#ccc", "color": "#fff" });
    //$("#step1").css({ "background": "#ccc", "color": "#fff" });
    
});
$(document).on("click", "#step5", function (event) {
    if (stpcount != 0)
    $("#btnComplete").trigger("click");
    complete();
    $(".f1-progress-line").css({ "width": "100%" });
    $("#step5 .f1-step-icon").css({ "background": "#f35b3f", "width": "48px", "height": "48px","border": "1px solid #f35b3f", "font-size": "22px", "line-height": "48px" });
    $("#step5 p").css({ "color": "#f35b3f" });
    $("#step4 .f1-step-icon").css({ "background": "#fff", "color": "#f35b3f", "border": "1px solid #f35b3f", "width": "40px", "height": "40px", "line-height": "40px" });
    $("#step4 p").css({ "color": "#f35b3f" });
    $("#step2 .f1-step-icon").css({ "background": "#fff", "color": "#f35b3f", "border": "1px solid #f35b3f", "width": "40px", "height": "40px", "line-height": "40px" });
    $("#step2 p").css({ "color": "#f35b3f" });
    $("#btnPrevious").css({ "background": "#06b3a2", "color": "#fff" }).prop('disabled', false);
    $("#btnNext1").css({ "background": "#ccc" }).prop('disabled', true);
    $('.fa-chevron-right').css({ "color": "white" })
    stpcount = 5;
    $("#btnskip").hide();
    //$("#step5").css({ "border": "4px solid orange", "background": "#fff" });
    //$("#step4").css({ "background": "#ccc", "color": "#fff" });
    //$("#step3").css({ "background": "#ccc", "color": "#fff" });
    //$("#step2").css({ "background": "#ccc", "color": "#fff" });
    //$("#step1").css({ "background": "#ccc", "color": "#fff" });
});

function defineBudget() {
    $.ajax({
        url: "../Wizard/_DefineBudget",
        type: "Get",
        success: function (result) {
            $("#divDataBind").html(result);
            $("#btnLevels").hide();
        }
    });

}
function complete() {
    $.ajax({
        url: "../Wizard/_Complete",
        type: "Get",
        success: function (result) {
            $("#divDataBind").html(result);
            $("#btnLevels").hide();
        }
    });

}
function Welcome() {
    $.ajax({
        url: "../Wizard/_Welcome",
        type: "Get",
        success: function (result) {
            $("#divDataBind").html(result);
            $("#btnLevels").hide();
        }
    });

}
function chooseFields() {
    $.ajax({
        url: "../Workforce/_ChooseFields",
        data: { isWizard: true },
        type: "Get",
        success: function (result) {
            $("#divDataBind").html(result);
            $("#btnLevels").hide();

        }
    });

}

function enableFeatures() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../ManageRules/_SetRules",
        type: "post",
        data: { __RequestVerificationToken:token, isWizard: true },
        success: function (result) {
            $("#divDataBind").html(result);
            $("#btnLevels").hide();
        }
    });

}

function allowWizardDecimalNumberOnlyInput(e, control) {
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

function Wizard() {
    window.location.href = "../Workforce/Home"
}

function completeUpdate() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "../Wizard/UpdateStepCount",
        type: "post",
        data: {
            __RequestVerificationToken:token,
            Step: 4
        },
        success: function (result) {
            $("#step5").trigger("click");

        }
    });
}

//rajitha new wizard scripts//

function scroll_to_class(element_class, removed_height) {
    var scroll_to = $(element_class).offset().top - removed_height;
    if ($(window).scrollTop() != scroll_to) {
        $('html, body').stop().animate({ scrollTop: scroll_to }, 0);
    }
}

function bar_progress(progress_line_object, direction) {
    var number_of_steps = progress_line_object.data('number-of-steps');
    var now_value = progress_line_object.data('now-value');
    var new_value = 0;
    if (direction == 'right') {
        new_value = now_value + (100 / number_of_steps);
    }
    else if (direction == 'left') {
        new_value = now_value - (100 / number_of_steps);
    }
    progress_line_object.attr('style', 'width: ' + new_value + '%;').data('now-value', new_value);
}

jQuery(document).ready(function () {

    /*
        Fullscreen background
    */
    $.backstretch("assets/img/backgrounds/1.jpg");

    $('#top-navbar-1').on('shown.bs.collapse', function () {
        $.backstretch("resize");
    });
    $('#top-navbar-1').on('hidden.bs.collapse', function () {
        $.backstretch("resize");
    });

    /*
        Form
    */
    $('.f1 fieldset:first').fadeIn('slow');

    $('.f1 input[type="text"], .f1 input[type="password"], .f1 textarea').on('focus', function () {
        $(this).removeClass('input-error');
    });

    // next step
    $('.f1 .btn-next').on('click', function () {
        var parent_fieldset = $(this).parents('fieldset');
        var next_step = true;
        // navigation steps / progress steps
        var current_active_step = $(this).parents('.f1').find('.f1-step.active');
        var progress_line = $(this).parents('.f1').find('.f1-progress-line');

        // fields validation
        parent_fieldset.find('input[type="text"], input[type="password"], textarea').each(function () {
            if ($(this).val() == "") {
                $(this).addClass('input-error');
                next_step = false;
            }
            else {
                $(this).removeClass('input-error');
            }
        });
        // fields validation

        if (next_step) {
            parent_fieldset.fadeOut(400, function () {
                // change icons
                current_active_step.removeClass('active').addClass('activated').next().addClass('active');
                // progress bar
                bar_progress(progress_line, 'right');
                // show next step
                $(this).next().fadeIn();
                // scroll window to beginning of the form
                scroll_to_class($('.f1'), 20);
            });
        }

    });

    // previous step
    $('.f1 .btn-previous').on('click', function () {
        // navigation steps / progress steps
        var current_active_step = $(this).parents('.f1').find('.f1-step.active');
        var progress_line = $(this).parents('.f1').find('.f1-progress-line');

        $(this).parents('fieldset').fadeOut(400, function () {
            // change icons
            current_active_step.removeClass('active').prev().removeClass('activated').addClass('active');
            // progress bar
            bar_progress(progress_line, 'left');
            // show previous step
            $(this).prev().fadeIn();
            // scroll window to beginning of the form
            scroll_to_class($('.f1'), 20);
        });
    });

    // submit
    $('.f1').on('submit', function (e) {

        // fields validation
        $(this).find('input[type="text"], input[type="password"], textarea').each(function () {
            if ($(this).val() == "") {
                e.preventDefault();
                $(this).addClass('input-error');
            }
            else {
                $(this).removeClass('input-error');
            }
        });
        // fields validation

    });


});

(function (a, d, p) { a.fn.backstretch = function (c, b) { (c === p || 0 === c.length) && a.error("No images were supplied for Backstretch"); 0 === a(d).scrollTop() && d.scrollTo(0, 0); return this.each(function () { var d = a(this), g = d.data("backstretch"); if (g) { if ("string" == typeof c && "function" == typeof g[c]) { g[c](b); return } b = a.extend(g.options, b); g.destroy(!0) } g = new q(this, c, b); d.data("backstretch", g) }) }; a.backstretch = function (c, b) { return a("body").backstretch(c, b).data("backstretch") }; a.expr[":"].backstretch = function (c) { return a(c).data("backstretch") !== p }; a.fn.backstretch.defaults = { centeredX: !0, centeredY: !0, duration: 5E3, fade: 0 }; var r = { left: 0, top: 0, overflow: "hidden", margin: 0, padding: 0, height: "100%", width: "100%", zIndex: -999999 }, s = { position: "absolute", display: "none", margin: 0, padding: 0, border: "none", width: "auto", height: "auto", maxHeight: "none", maxWidth: "none", zIndex: -999999 }, q = function (c, b, e) { this.options = a.extend({}, a.fn.backstretch.defaults, e || {}); this.images = a.isArray(b) ? b : [b]; a.each(this.images, function () { a("<img />")[0].src = this }); this.isBody = c === document.body; this.$container = a(c); this.$root = this.isBody ? l ? a(d) : a(document) : this.$container; c = this.$container.children(".backstretch").first(); this.$wrap = c.length ? c : a('<div class="backstretch"></div>').css(r).appendTo(this.$container); this.isBody || (c = this.$container.css("position"), b = this.$container.css("zIndex"), this.$container.css({ position: "static" === c ? "relative" : c, zIndex: "auto" === b ? 0 : b, background: "none" }), this.$wrap.css({ zIndex: -999998 })); this.$wrap.css({ position: this.isBody && l ? "fixed" : "absolute" }); this.index = 0; this.show(this.index); a(d).on("resize.backstretch", a.proxy(this.resize, this)).on("orientationchange.backstretch", a.proxy(function () { this.isBody && 0 === d.pageYOffset && (d.scrollTo(0, 1), this.resize()) }, this)) }; q.prototype = { resize: function () { try { var a = { left: 0, top: 0 }, b = this.isBody ? this.$root.width() : this.$root.innerWidth(), e = b, g = this.isBody ? d.innerHeight ? d.innerHeight : this.$root.height() : this.$root.innerHeight(), j = e / this.$img.data("ratio"), f; j >= g ? (f = (j - g) / 2, this.options.centeredY && (a.top = "-" + f + "px")) : (j = g, e = j * this.$img.data("ratio"), f = (e - b) / 2, this.options.centeredX && (a.left = "-" + f + "px")); this.$wrap.css({ width: b, height: g }).find("img:not(.deleteable)").css({ width: e, height: j }).css(a) } catch (h) { } return this }, show: function (c) { if (!(Math.abs(c) > this.images.length - 1)) { var b = this, e = b.$wrap.find("img").addClass("deleteable"), d = { relatedTarget: b.$container[0] }; b.$container.trigger(a.Event("backstretch.before", d), [b, c]); this.index = c; clearInterval(b.interval); b.$img = a("<img />").css(s).bind("load", function (f) { var h = this.width || a(f.target).width(); f = this.height || a(f.target).height(); a(this).data("ratio", h / f); a(this).fadeIn(b.options.speed || b.options.fade, function () { e.remove(); b.paused || b.cycle(); a(["after", "show"]).each(function () { b.$container.trigger(a.Event("backstretch." + this, d), [b, c]) }) }); b.resize() }).appendTo(b.$wrap); b.$img.attr("src", b.images[c]); return b } }, next: function () { return this.show(this.index < this.images.length - 1 ? this.index + 1 : 0) }, prev: function () { return this.show(0 === this.index ? this.images.length - 1 : this.index - 1) }, pause: function () { this.paused = !0; return this }, resume: function () { this.paused = !1; this.next(); return this }, cycle: function () { 1 < this.images.length && (clearInterval(this.interval), this.interval = setInterval(a.proxy(function () { this.paused || this.next() }, this), this.options.duration)); return this }, destroy: function (c) { a(d).off("resize.backstretch orientationchange.backstretch"); clearInterval(this.interval); c || this.$wrap.remove(); this.$container.removeData("backstretch") } }; var l, f = navigator.userAgent, m = navigator.platform, e = f.match(/AppleWebKit\/([0-9]+)/), e = !!e && e[1], h = f.match(/Fennec\/([0-9]+)/), h = !!h && h[1], n = f.match(/Opera Mobi\/([0-9]+)/), t = !!n && n[1], k = f.match(/MSIE ([0-9]+)/), k = !!k && k[1]; l = !((-1 < m.indexOf("iPhone") || -1 < m.indexOf("iPad") || -1 < m.indexOf("iPod")) && e && 534 > e || d.operamini && "[object OperaMini]" === {}.toString.call(d.operamini) || n && 7458 > t || -1 < f.indexOf("Android") && e && 533 > e || h && 6 > h || "palmGetResource" in d && e && 534 > e || -1 < f.indexOf("MeeGo") && -1 < f.indexOf("NokiaBrowser/8.5.0") || k && 6 >= k) })(jQuery, window);