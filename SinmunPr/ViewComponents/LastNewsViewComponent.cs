using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinmunPr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewComponents
{
    public class LastNewsViewComponent : ViewComponent
    {
        DBsinmun db;
        public LastNewsViewComponent(DBsinmun _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            db.posts.Include(x => x.mainComments).ThenInclude(x => x.subComments).ToList();

            return View(db.posts.OrderByDescending(x => x.id).Take(3).ToList());
        }
    }
}
