using Microsoft.AspNetCore.Mvc;
using SinmunPr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        DBsinmun db;
        public CategoryViewComponent(DBsinmun _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            return View(db.categories.ToList());
        }
    }
}
