﻿var tour = new Tour({
    storage: false,
    onEnd: function (tour) {
        $("#mgrTreetourid").css("border", "white solid 2px")
        $("#mgrTreetourid").css({ "padding-right": "0px" })
        $("#divBudget").css("border", "white solid 2px")
        $("#lnkEmployeeName").css("border", "white solid 2px")
        $("#txtMeritPCT").css("border", "white solid 2px")
        $("#lnkNewTitle").css("border", "white solid 2px")
        $("#txtPromotionPctLocal").css("border", "white solid 2px")
        $("#txtBonusPCT").css("border", "white solid 2px")
        $("#lnkComment").css("border", "white solid 2px")
        $("#txtLumpSumPct").css("border", "white solid 2px")
    },
});
tour.addSteps([
      {

          element: "#tour-go",
          title: "Welcome to the Compensation Tool tour!",
          content: "This tour will introduce you to the tool and the steps you will take to make proposals for your employees.",
          placement: 'left',
          smartPlacement: true,
          animation: true,
          backdrop: false,
          backdropContainer: 'body',
          duration: 10000,
          onShown: function (tour) {
              $("#mgrTreetourid").css("border", "white solid 2px")
              $("#mgrTreetourid").css({ "padding-right": "0px" })


          },

      },
    {

        element: "#mgrTree",
        title: "Select a manager",
        content: "Here you will be able to select any manager reporting to you to see the proposals they are making for their direct reports.",
        placement: 'left',
        smartPlacement: true,
        animation: true,
        backdrop: false,
        backdropContainer: 'body',
        duration: 10000,
        onShown: function (tour) {
            $("#mgrTreetourid").css("border", "#C0C0C0 solid 2px")
            $("#mgrTreetourid").css({ "padding-right": "25px" })
            $("#divBudget").css("border", "white solid 2px")



        },
    },
{
    element: "#divBudgetData",
    title: "Budget section",
    content: "Here you will see the budget and spend you have available to propose for your direct reports.  You can also see your rollup budget for your total area of responsibility as well as to see the budget in different currencies by clicking on the appropriate buttons.  Please remain within your budget guidelines overall.",
    placement: "bottom",
    smartPlacement: true,
    animation: true,
    backdrop: false,
    backdropContainer: 'body',
    duration: 10000,
    onShown: function (tour) {
        $("#mgrTreetourid").css("border", "white solid 2px")
        $("#mgrTreetourid").css({ "padding-right": "0px" })
        $("#divBudget").css("border", "#C0C0C0 solid 2px")

    },

}, {

    element: ".iconClosed",
    title: "Spend charts",
    content: "If you would like to see your ‘spent distribution’ in a chart form, click the arrow on the left.",
    placement: 'right',
    smartPlacement: true,
    animation: true,
    backdrop: false,
    backdropContainer: 'body',
    duration: 10000,
    onShown: function (tour) {
        // $("#ReporteeGrid").css("border", "white solid 2px")

    },
    onNext: function (tour) {
        $(".iconClosed").trigger("click");
    },

},
{

    element: "#chartTourId",
    title: "You see the manager's spent distribution",
    content: "Simply click the arrow again to collapse the chart.",
    placement: 'bottom',
    smartPlacement: true,
    animation: true,
    backdrop: false,
    backdropContainer: 'body',
    duration: 10000,
    onShown: function (tour) {
        // $("#ReporteeGrid").css("border", "white solid 2px")

    },
    onPrev: function (tour) {
        $(".iconOpen").trigger("click");
    },
    onNext: function (tour) {
        $(".iconOpen").trigger("click");
    },
},
{
    element: "#ReporteeGrid",
    title: "Viewing employee information",
    content: "In this section you will see your direct report information.  Please review the list to ensure all your direct reports are captured (contact your HR Representative if you find any discrepancies).",
    placement: 'top',
    smartPlacement: true,
    animation: true,
    backdrop: false,
    backdropContainer: 'body',
    duration: 10000,
    onShown: function (tour) {
        $(window).scrollTop(0);
        $("#divBudget").css("border", "white solid 2px")
        // $("#ReporteeGrid").css("border", "#C0C0C0 solid 2px")
        $("#lnkEmployeeName").css("border", "white solid 2px")


    },

},
   {

       element: "#lnkEmployeeName",
       title: "Additional employee information",
       content: "Click on the employees name to find other information pertaining to the individual that may assist with your proposals.  To close the box simply click outside of the pop-up.",
       smartPlacement: true,
       animation: true,
       backdrop: false,
       backdropContainer: 'body',
       duration: 10000,
       onShown: function (tour) {

           $("#lnkEmployeeName").css("border", "#C0C0C0 solid 2px")
           $("#txtMeritPCT").css("border", "white solid 2px")
       },
       onNext: function (tour) {
           var grid = $("#ReporteeGrid").data("kendoGrid");
           grid.element.find(".k-grid-content").animate({
               scrollLeft: 900
           }, 1000);

           $("#divEmployeeInfo").trigger("click");

       },
       onPrev: function (tour) {

           $("#divEmployeeInfo").trigger("click");

       },
   },

   {

       element: "#txtMeritPCT",
       title: "Entering merit percentages",
       content: "Enter the merit percentage you would like to propose in the box.  The budget for the individual will be shown to the left under country budget.  Amounts proposed for employees should be based on performance and should remain within budget for your overall team.",
       smartPlacement: true,
       animation: true,
       backdrop: false,
       backdropContainer: 'body',
       duration: 10000,
       delay: 1000,
       onShown: function (tour) {
           $("#lnkEmployeeName").css("border", "white solid 2px")
           $("#txtMeritPCT").css("border", "#C0C0C0 solid 2px")
           $("#lnkNewTitle").css("border", "white solid 2px")


       },

   },
    {

        element: "#lnkNewTitle",
        title: "Promoting an employee",
        content: "If you are promoting an employee, click on the Promote link.  This will open a box for you to enter a new title and reason for the promotion.  Click “Add comment” to close the box.",
        placement: 'left',
        smartPlacement: true,
        animation: true,
        backdrop: false,
        backdropContainer: 'body',
        duration: 10000,
        onShown: function (tour) {
            $("#txtMeritPCT").css("border", "white solid 2px")
            $("#lnkNewTitle").css("border", "#C0C0C0 solid 2px")
            $("#txtPromotionPctLocal").css("border", "white solid 2px")

        }, onNext: function (tour) {

            $("#divPromotion").trigger("click");

        },
        onPrev: function (tour) {

            $("#divPromotion").trigger("click");

        },

    },
        {

            element: "#txtPromotionPctLocal",
            title: "Entering a promotion amount",
            content: "Enter the promotion percentage in the box.  Promotion amounts will be deducted from your overall budget.",
            placement: 'left',
            smartPlacement: true,
            animation: true,
            backdrop: false,
            backdropContainer: 'body',
            duration: 10000,
            onShown: function (tour) {
                $("#lnkNewTitle").css("border", "white solid 2px")
                $("#txtPromotionPctLocal").css("border", "#C0C0C0 solid 2px")
                $("#txtBonusPCT").css("border", "white solid 2px")
            },



        },
         {
             element: "#txtLumpSumPct",
             title: "Lump sum proposals",
             content: "In instances where an employee may already be compensated very high relative to other peers and/or market, a lower end increase or possibly lump sum merit in lieu of a salary increase may be appropriate.  Enter the lump sum percentage in the box.  This will be deducted from the overall budget but not added to the employees’ salary.",
             placement: 'left',
             smartPlacement: true,
             animation: true,
             backdrop: false,
             backdropContainer: 'body',
             duration: 10000,
             onShown: function (tour) {
                 $("#txtPromotionPctLocal").css("border", "white solid 2px")
                 $("#txtLumpSumPct").css("border", "#C0C0C0 solid 2px")
                 $("#txtBonusPCT").css("border", "white solid 2px")
             },
             onNext: function (tour) {
                 var grid = $("#ReporteeGrid").data("kendoGrid");
                 grid.element.find(".k-grid-content").animate({
                     scrollLeft: 2700
                 }, 2700);
             },


         },
         {

             element: "#txtBonusPCT",
             title: "Bonus amounts",
             content: "Under the Employee Performance Score section, managers need to enter a proposal for the employees based on the bonus plan they are in.  Your HR Representatives will provide information on the scores that can be given based on the different bonus plans.",
             placement: 'left',
             smartPlacement: true,
             animation: true,
             backdrop: false,
             backdropContainer: 'body',
             duration: 10000,
             delay: 2000,
             onShown: function (tour) {
                 $("#txtPromotionPctLocal").css("border", "white solid 2px")
                 $("#txtLumpSumPct").css("border", "white solid 2px")
                 $("#txtBonusPCT").css("border", "#C0C0C0 solid 2px")
                 $("#lnkComment").css("border", "white solid 2px")
             },

         },
          {

              element: "#lnkComment",
              title: "Comments",
              content: "Scroll to the right and you will find a comment link.  If you would like to make any comments regarding any of the proposals you have made, click on the link and a box will pop-up for your comments.",
              placement: 'left',
              smartPlacement: true,
              animation: true,
              backdrop: false,
              backdropContainer: 'body',
              duration: 10000,
              onShown: function (tour) {
                  $("#txtBonusPCT").css("border", "white solid 2px")
                  $("#lnkComment").css("border", "#C0C0C0 solid 2px")


              },
              onNext: function (tour) {

                  $("#divComment").trigger("click");

              },
              onPrev: function (tour) {

                  $("#divComment").trigger("click");

              },

          },
           {

               element: "#btnCompensationSave",
               title: "Saving your work",
               content: "When you complete your proposals you will hit the Save button before submitting to the approving manager.",
               placement: 'left',
               smartPlacement: true,
               animation: true,
               backdrop: false,
               backdropContainer: 'body',
               duration: 10000,
               onShown: function (tour) {
                   $("#lnkComment").css("border", "white solid 2px")


               },

           },
            {

                element: "#ddlActionsTourId",
                title: "More actions",
                content: "Click on this button for additional options in viewing your employee information.",
                placement: 'bottom',
                smartPlacement: true,
                animation: true,
                backdrop: false,
                backdropContainer: 'body',
                duration: 10000,
                onShown: function (tour) {


                },

            },
             {

                 element: "#btnSubmit",
                 title: "Completed your reviews?",
                 content: "Don’t forget to submit your review to your next level manager to share your decisions.",
                 smartPlacement: true,
                 animation: true,
                 backdrop: false,
                 backdropContainer: 'body',
                 duration: 10000,
                 onShown: function (tour) {


                 },

             },
            {

                element: ".logOut",
                title: "Log out",
                content: "Click the Log out button when your work is completed.",
                smartPlacement: true,
                animation: true,
                backdrop: false,
                backdropContainer: 'body',
                duration: 10000,
                onShown: function (tour) {

                },

            },

]);
$(document).ready(function () {

    $('#tour-go').click(function (e) {
        tour.init();
        tour.restart();
        e.preventDefault();
    });
});

