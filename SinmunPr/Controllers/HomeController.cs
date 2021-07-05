using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using SinmunPr.Data;
using SinmunPr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Controllers
{
    public class HomeController : Controller
    {
        DBsinmun db;
        public HomeController(DBsinmun _db)
        {
            db = _db;
        }

        public async Task<IActionResult> Index()
        {
            #region LazyLoading

            db.posts.Include(x => x.applicationUser).Include(x => x.category).Include(x => x.mainComments)
                .ThenInclude(x => x.subComments).ToList();

            #endregion

            #region ViewDatas

            ViewData["topslidebar"] = db.posts.Where(x => x.IsInTopSlideBar == true).OrderByDescending(x => x.id).Take(3).ToList();
            ViewData["editorchoice"] = db.posts.Where(x => x.IsInEditorsChoice == true).OrderByDescending(x => x.id).Take(6).ToList();
            ViewData["important"] = db.posts.Where(x => x.IsInImportant == true).OrderByDescending(x => x.id).Take(5).ToList();
            ViewData["dedicated"] = db.posts.Where(x => x.IsInDedicated == true).OrderByDescending(x => x.id).Take(4).ToList();
            ViewData["mostview"] = db.posts.Where(x => x.ePostCreation <= DateTime.Now && x.ePostCreation >= DateTime.Today.AddDays(-7))
                .OrderByDescending(x => x.views).Take(6).ToList();

            var categories = db.categories.Where(x => x.IsInMainPage == true).OrderByDescending(x => x.id).Take(3).ToList();

            List<Post> cPosts = new();
            foreach (var item in categories)
            {
                List<Post> oneCPosts = item.posts.OrderByDescending(x => x.id).Take(4).ToList();
                cPosts.AddRange(oneCPosts);
            }

            ViewData["categories"] = categories;
            ViewData["cPosts"] = cPosts;

            #endregion

            #region Pagination

            var posts = db.posts.AsNoTracking().Include(x => x.applicationUser).Include(x => x.category).Include(x => x.mainComments).ThenInclude(x => x.subComments).OrderByDescending(x => x.id);
            var postList = await PagingList.CreateAsync(posts, 14, 1);

            var result = new PagedResult<Post>()
            {
                Data = postList.ToList(),
                TotalItems = db.posts.Count(),
                PageNumber = 1,
                PageSize = 14
            };
            #endregion

            return View(result);
        }



        [Route("/page/{pagenumber}")]
        public async Task<IActionResult> Page(int pagenumber)
        {
            #region LazyLoading

            db.posts.Include(x => x.applicationUser).Include(x => x.category).Include(x => x.mainComments)
                .ThenInclude(x => x.subComments).ToList();

            #endregion

            #region ViewDatas

            ViewData["editorchoice"] = db.posts.Where(x => x.IsInEditorsChoice == true).OrderByDescending(x => x.id).Take(6).ToList();
            ViewData["important"] = db.posts.Where(x => x.IsInImportant == true).OrderByDescending(x => x.id).Take(5).ToList();
            ViewData["dedicated"] = db.posts.Where(x => x.IsInDedicated == true).OrderByDescending(x => x.id).Take(4).ToList();
            ViewData["mostview"] = db.posts.Where(x => x.ePostCreation <= DateTime.Now && x.ePostCreation >= DateTime.Today.AddDays(-7))
                .OrderByDescending(x => x.views).Take(6).ToList();

            var categories = db.categories.Where(x => x.IsInMainPage == true).OrderByDescending(x => x.id).Take(3).ToList();

            List<Post> cPosts = new();
            foreach (var item in categories)
            {
                var oneCPosts = item.posts.OrderByDescending(x => x.id).Take(4).ToList();
                cPosts.AddRange(oneCPosts);
            }

            ViewData["categories"] = categories;
            ViewData["cPosts"] = cPosts;

            #endregion

            #region Pagination

            var posts = db.posts.Include(x => x.applicationUser).Include(x => x.category).Include(x => x.mainComments).ThenInclude(x => x.subComments).AsNoTracking().OrderByDescending(x => x.id);
            var postList = await PagingList.CreateAsync(posts, 14, pagenumber);

            var result = new PagedResult<Post>()
            {
                Data = postList.ToList(),
                TotalItems = db.posts.Count(),
                PageNumber = pagenumber,
                PageSize = 14
            };
            #endregion

            return View(result);
        }



        [Route("/allcategories")]
        public IActionResult AllCategories()
        {
            #region LazyLoading

            db.posts.Include(x => x.applicationUser).Include(x => x.category).ToList();

            #endregion

            #region ViewData

            var categories = db.categories.OrderByDescending(x => x.id).ToList();

            List<Post> cPosts = new();
            foreach (var item in categories)
            {
                var oneCPost = item.posts.OrderByDescending(x => x.id).Take(6).ToList();
                cPosts.AddRange(oneCPost);
            }

            ViewData["cPosts"] = cPosts;

            #endregion

            return View(categories);
        }



        public IActionResult ShowCategoryBridge(int id, int pagenumber = 1)
        {
            var category = db.Find<Category>(id);

            return RedirectToAction(nameof(ShowCategory), new
            {
                id,
                categoryName = category.Ename.Replace(" ", "-"),
                pagenumber
            });
        }



        [Route("/category/{categoryName}/{id}/{pagenumber}")]
        public async Task<IActionResult> ShowCategory(int id, int pagenumber, string categoryName)
        {
            #region LazyLoading

            db.posts.Include(x => x.applicationUser).Include(x => x.category).Include(x => x.mainComments)
                    .ThenInclude(x => x.subComments).ToList();

            #endregion

            #region ViewData

            ViewData["editorchoice"] = db.posts.Where(x => x.IsInEditorsChoice == true).OrderByDescending(x => x.id).Take(6).ToList();
            ViewData["important"] = db.posts.Where(x => x.IsInImportant == true).OrderByDescending(x => x.id).Take(5).ToList();
            ViewData["dedicated"] = db.posts.Where(x => x.IsInDedicated == true).OrderByDescending(x => x.id).Take(4).ToList();
            ViewData["mostview"] = db.posts.Where(x => x.ePostCreation <= DateTime.Now && x.ePostCreation >= DateTime.Today.AddDays(-7))
                .OrderByDescending(x => x.views).Take(6).ToList();

            #endregion

            #region FindCategory&Pagination

            var category = db.Find<Category>(id);

            ViewBag.categoryPname = category.Pname;

            var posts = db.posts.Include(x => x.applicationUser).Include(x => x.category).Include(x => x.mainComments)
                .ThenInclude(x => x.subComments).Where(x => x.categoryId == id).OrderByDescending(x => x.id);

            var postsList = await PagingList.CreateAsync(posts, 14, pagenumber);

            var pagination = new PagedResult<Post>()
            {
                Data = postsList.ToList(),
                PageNumber = pagenumber,
                PageSize = 14,
                TotalItems = db.posts.Where(x => x.categoryId == id).Count()
            };

            #endregion

            return View(pagination);
        }



        public IActionResult ShowTagBridge(int id, int pagenumber = 1)
        {
            var tag = db.Find<Tag>(id);

            return RedirectToAction(nameof(ShowTag), new
            {
                id,
                name = tag.Ename.Replace(" ", "-"),
                pagenumber
            });
        }



        [Route("/tag/{name}/{id}/{pagenumber}")]
        public async Task<IActionResult> ShowTag(int id, string name, int pagenumber = 1)
        {
            #region LazyLoading

            db.posts.Include(x => x.applicationUser).Include(x => x.category).Include(x => x.postTagIds).ThenInclude(x => x.Tag)
                .Include(x => x.mainComments).ThenInclude(x => x.subComments).ToList();

            #endregion

            #region ViewData

            ViewData["editorchoice"] = db.posts.Where(x => x.IsInEditorsChoice == true).OrderByDescending(x => x.id).Take(6).ToList();
            ViewData["important"] = db.posts.Where(x => x.IsInImportant == true).OrderByDescending(x => x.id).Take(5).ToList();
            ViewData["dedicated"] = db.posts.Where(x => x.IsInDedicated == true).OrderByDescending(x => x.id).Take(4).ToList();
            ViewData["mostview"] = db.posts.Where(x => x.ePostCreation <= DateTime.Now && x.ePostCreation >= DateTime.Today.AddDays(-7))
                .OrderByDescending(x => x.views).Take(6).ToList();

            #endregion

            #region FindTag&Pagination

            var tag = db.Find<Tag>(id);
            ViewBag.Pname = tag.Pname;

            var posttag = db.postTagIds.Where(x => x.tagId == tag.id).OrderByDescending(x => x.id);

            var posts = await PagingList.CreateAsync(posttag, 14, pagenumber);

            var pagination = new PagedResult<PostTagId>()
            {
                Data = posts.ToList(),
                PageNumber = pagenumber,
                PageSize = 14,
                TotalItems = db.postTagIds.Where(x => x.tagId == tag.id).Count()
            };
            #endregion

            return View(pagination);
        }



        [Route("/aboutus")]
        public IActionResult AboutUs()
        {
            return View(db.Find<AboutUs>(1));
        }



        [Route("/contactus")]
        public IActionResult ContactUs()
        {
            return View();
        }



        public IActionResult Error()
        {
            return View();
        }
    }
}
