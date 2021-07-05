using Microsoft.AspNetCore.Mvc;
using SinmunPr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Components
{
    public class ImmediateNewsViewComponent : ViewComponent
    {
        DBsinmun db;
        public ImmediateNewsViewComponent(DBsinmun _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            return View(db.posts.Where(x => x.IsInImmediate == true).OrderByDescending(x => x.id).Take(4).ToList());
        }

    }
}
