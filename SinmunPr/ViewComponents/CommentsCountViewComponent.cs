using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinmunPr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewComponents
{
    public class CommentsCountViewComponent : ViewComponent
    {
        DBsinmun db;
        public CommentsCountViewComponent(DBsinmun _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            db.mainComments.Include(x => x.subComments).ToList();

            ViewBag.CommentCount = db.mainComments.Where(x => x.IsConfirm == false).Count() + db.subComments.Where(x => x.IsConfirm == false).Count();

            return View();
        }
    }
}
