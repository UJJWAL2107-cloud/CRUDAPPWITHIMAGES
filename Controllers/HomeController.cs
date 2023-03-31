using CRUDAPPWITHIMAGES.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDAPPWITHIMAGES.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        ExampleDBEntities db = new ExampleDBEntities();
        public ActionResult Index()
        {
            var data = db.Products.ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product P)
        {
            if(ModelState.IsValid == true)
            {
                string FileName = Path.GetFileNameWithoutExtension(P.ImageFile.FileName);
                string Extension = Path.GetExtension(P.ImageFile.FileName);

                HttpPostedFileBase postedfile = P.ImageFile;
                int length = postedfile.ContentLength;

                if(Extension.ToLower() == ".jpg" || Extension.ToLower() == ".jpeg" || Extension.ToLower() == ".png")
                {
                    if(length <= 1000000)
                    {
                        FileName = FileName + Extension;
                        P.Image = "~/Images" + FileName;
                        FileName = Path.Combine(Server.MapPath("~/Images/"), FileName);
                        P.ImageFile.SaveAs(FileName);
                        db.Products.Add(P);
                        int a = db.SaveChanges();
                        if(a > 0)
                        {
                            TempData["SuccessMessage"] = "<script>alert('Data inserted Succesfully')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index","Home");
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "<script>alert('Data Not Inserted Succesfully')</script>";
                        }
                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image Size Exceeds 1 MB')</script>";
                    }
                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('Format Not Supported')</script>";
                }
            }
            return View();
        }
    }
}