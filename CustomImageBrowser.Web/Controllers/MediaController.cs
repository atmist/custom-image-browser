using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomImageBrowser.Web.Controllers
{
    public class MediaController : Controller
    {
        public ActionResult ImageBrowser()
        {
            var model = new ImageBrowserModel();
            model.Images = new List<ImageModel>();
            return PartialView("ImageBrowser", model);
        }

        public ActionResult Image(string fileName)
        {
            var path = string.Format("{0}\\{1}", ImagesPath(), fileName);
            if (System.IO.File.Exists(path))
            {
                return File(path, MimeMapping.GetMimeMapping(fileName));
            }
            return HttpNotFound();
        }

        public ActionResult Thumbnail(string fileName)
        {
            fileName = fileName.Replace(Path.GetExtension(fileName), "_thumb" + Path.GetExtension(fileName));
            var path = string.Format("{0}\\{1}", ImagesPath(), fileName);
            if (System.IO.File.Exists(path))
            {
                return File(path, MimeMapping.GetMimeMapping(fileName));
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Upload(string qqfile)
        {
            //Retrieve the stream and the file name
            var file = String.Empty;
            var stream = Request.InputStream;
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                //IE
                HttpPostedFileBase postedFile = Request.Files[0];
                stream = postedFile.InputStream;
                file = System.IO.Path.GetFileName(Request.Files[0].FileName);
            }
            else
            {
                //Webkit, Mozilla
                file = qqfile;
            }

            String imageName = String.Empty;
            if (SaveImage(file, stream, ImagesPath(), out imageName))
            {
                var imageModel = new ImageModel
                {
                    OriginalName = qqfile,
                    Id = imageName,
                    Url = Url.Action("Image", "Media", new { fileName = imageName }),
                    ThumbUrl = Url.Action("Thumbnail", "Media", new { fileName = imageName })
                };

                return Json(new { success = true, data = PartialViewToString("_Image", imageModel) }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult Download(string fileName)
        {
            var path = string.Format("{0}\\{1}", ImagesPath(), fileName);
            if (System.IO.File.Exists(path))
            {
                var result = File(path, MimeMapping.GetMimeMapping(fileName), fileName);
                return result;
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Delete(ImageModel model)
        {
            var fileName = model.Id;

            var thumbnail = fileName.Replace(Path.GetExtension(fileName), "_thumb" + Path.GetExtension(fileName));
            var path = string.Format("{0}\\{1}", ImagesPath(), fileName);
            var pathThumbnail = string.Format("{0}\\{1}", ImagesPath(), thumbnail);
            try
            {
                System.IO.File.Delete(path);
                System.IO.File.Delete(pathThumbnail);
                return Json(new { success = true, data = fileName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public string PartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;

            using (StringWriter stringWriter = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        #region Private Methods

        private bool SaveImage(string fileName, Stream inputStream, string location, out string name)
        {
            //Get the file's extension
            var extension = Path.GetExtension(fileName);

            name = string.Format("{0}{1}", Guid.NewGuid().ToString(), extension);
            fileName = "\\" + name;

            var path = location + fileName;

            try
            {
                var stream = new MemoryStream(); ;
                inputStream.CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                SaveThumbnail(path, stream);

                inputStream.Seek(0, SeekOrigin.Begin);
                ImageResizer.ImageJob imageJob = new ImageResizer.ImageJob(inputStream, path,
                    new ImageResizer.Instructions { AutoRotate = true });
                imageJob.CreateParentDirectory = true;
                imageJob.Build();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SaveThumbnail(string path, Stream inputStream)
        {
            var thumbnailSize = 300;
            path = path.Replace(Path.GetExtension(path), "_thumb" + Path.GetExtension(path));

            var settings = new ImageResizer.Instructions
            {
                Width = thumbnailSize,
                Height = thumbnailSize,
                Mode = ImageResizer.FitMode.Crop,
                AutoRotate = true
            };

            ImageResizer.ImageJob img = new ImageResizer.ImageJob(inputStream, path, settings);
            img.CreateParentDirectory = true;
            img.Build();
        }

        private string ImagesPath()
        {
            var path = string.Empty;

#if DEBUG
            path = HttpContext.ApplicationInstance.Server.MapPath("~/App_Data/uploads");
#else
            path = HttpContext.ApplicationInstance.Server.MapPath("~/uploads");
#endif
            return path;
        }

        #endregion
    }
}
