using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Controllers
{
    public class couponController : Controller
    {
        // GET: Coupon
        public ActionResult Index(string ID)
        {
            ViewBag.CouponCode = ID;
            if (string.IsNullOrEmpty(ID))
            {
                if (Request.QueryString["utm_source"] != null)
                {
                    setCookie(Request.QueryString.Get("utm_source"));
                }
                return Redirect("/");
            }
            else if (!string.IsNullOrEmpty(ID))
            {
                if (Request.QueryString["utm_source"] != null)
                {
                    setCookie(Request.QueryString.Get("utm_source"));
                }
                return View();
            }
            else
            {
                return Redirect("/404");
            }
        }



        //public ActionResult Coupon(string ID)
        //{
        //    ViewBag.CouponCode = ID;
        //    //if (ID==null)
        //    //{
        //    //    return View(/);
        //    //}
        //    return View();
        //}


        public void setCookie(string sourceMedia)
        {
            int intSmedia;
            bool bNum = int.TryParse(sourceMedia, out intSmedia);
            sourceMedia = bNum ? intSmedia.ToString() : "1000";
            HttpCookie FMsMedia = new HttpCookie("FMsMediaIndia");
            FMsMedia["sMediaIndia"] = sourceMedia;
            FMsMedia.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(FMsMedia);
        }
    }
}