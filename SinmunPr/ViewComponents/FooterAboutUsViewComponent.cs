using Microsoft.AspNetCore.Mvc;
using SinmunPr.Data;
using SinmunPr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewComponents
{
    public class FooterAboutUsViewComponent : ViewComponent
    {
        DBsinmun db;
        public FooterAboutUsViewComponent(DBsinmun _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            return View(db.Find<AboutUs>(1));
        }
    }
}
