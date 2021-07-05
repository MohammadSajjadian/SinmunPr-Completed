using Microsoft.AspNetCore.Mvc;
using SinmunPr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewComponents
{
    public class InboxViewComponent : ViewComponent
    {
        DBsinmun db;
        public InboxViewComponent(DBsinmun _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Count = db.contactUs.Count();
            return View();
        }
    }
}
