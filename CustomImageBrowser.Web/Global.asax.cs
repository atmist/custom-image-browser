using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CustomImageBrowser.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Session_OnEnd(Object sender, EventArgs E)
        {

            foreach (var imageModel in ((List<ImageModel>)Session["images"]))
            {
                var fileName = imageModel.Name;
                var thumbnail = fileName.Replace(Path.GetExtension(fileName), "_thumb" + Path.GetExtension(fileName));
                var path = string.Format("{0}\\{1}", ImagesPath(), fileName);
                var pathThumbnail = string.Format("{0}\\{1}", ImagesPath(), thumbnail);
                try
                {
                    System.IO.File.Delete(path);
                    System.IO.File.Delete(pathThumbnail);

                    ((List<ImageModel>)Session["images"]).Remove(imageModel);
                }
                catch (Exception) { }
            }
        }

        private string ImagesPath()
        {
            var path = string.Empty;

#if DEBUG
            path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/uploads");
#else
            path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads");
#endif
            return path;
        }
    }
}