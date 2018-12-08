using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Laserbeam.UI.HR.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.IgnoreList.Clear();
            bundles.Add(new StyleBundle("~/Content/Common").Include(
                "~/Content/jquery-ui.min.css", 
                "~/Content/sweetalert2.min.css",
                 "~/Content/bootstrap.css"
                 , "~/Content/font-awesome.css"
                 , "~/Content/Site.css"
                 , "~/Content/laserbeam-theme.css"                 
                ));

            bundles.Add(new StyleBundle("~/Content/SiteCommon").Include(
                "~/Content/Kendo/kendo.common.min.css"
                , "~/Content/Kendo/kendo.metrolight.min.css"
                , "~/Content/Kendo/kendo.ext.css"                
            ));

            bundles.Add(new ScriptBundle("~/Scripts/Common").Include(
                "~/Scripts/Jquery/jquery-{version}.js"
                , "~/Scripts/Jquery/jquery.unobtrusive-ajax.js"
                ,"~/Scripts/Jquery/jquery.validate.*"
                , "~/Scripts/Jquery/jquery-migrate-*"
                , "~/Scripts/Jquery/jquery-ui.js"
                ,"~/Scripts/BootStrap 3.3.6/bootstrap.min.js"
                
             
             ));

            bundles.Add(new ScriptBundle("~/Scripts/SiteCommon").Include(
               "~/Scripts/Kendo/kendo.all.min.js"
               , "~/Scripts/Kendo/kendo.aspnetmvc.min.js"
               , "~/Scripts/Kendo/kendo.core.min.js"
               , "~/Scripts/Kendo/cultures/*.js"
               , "~/Scripts/Kendo/kendo.web.ext.js"
               , "~/Scripts/Site.js"
               , "~/Scripts/laserbeam-calculation.min.js"
               , "~/Scripts/laserbeam-ui-validation.min.js"
               ,   "~/Scripts/sweetalert2.min.js"
               , "~/Scripts/plupload.full.min.js"
               ));
        }
    }
}