using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using SinmunPr.Areas.Identity.Data;
using SinmunPr.Data;
using SinmunPr.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Controllers
{
    [Authorize(policy: "adminPolicy")]
    public class AdminController : Controller
    {
        DBsinmun db;
        UserManager<ApplicationUser> userManager;
        public AdminController(DBsinmun _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }



        public IActionResult CategoryOptions()
        {
            return View(db.categories.ToList());
        }



        public IActionResult PostInsert()
        {
            #region ViewDatas

            ViewData["categories"] = db.categories.ToList();
            ViewData["tags"] = db.tags.ToList();

            #endregion

            return View();
        }



        [Route("/admin/postoptions/{pagenumber}")]
        public async Task<IActionResult> PostOptions(int pagenumber = 1)
        {
            #region Pagination

            var posts = db.posts.Include(x => x.applicationUser).OrderByDescending(x => x.id);
            var shortPosts = await PagingList.CreateAsync(posts, 20, pagenumber);

            var result = new PagedResult<Post>()
            {
                Data = shortPosts.ToList(),
                PageNumber = pagenumber,
                PageSize = 20,
                TotalItems = db.posts.Count(),
            };
            #endregion

            return View(result);
        }



        public IActionResult PostUpdate(int id)
        {
            db.posts.Include(x => x.category).Include(x => x.sources).Include(x => x.postTagIds).ToList();

            #region ViewDatas

            ViewData["categories"] = db.categories.ToList();
            ViewData["tags"] = db.tags.ToList();
            ViewData["sources"] = db.sources.Where(x => x.postId == id).ToList();
            ViewData["posttagid"] = db.postTagIds.Where(x => x.postId == id).ToList();
            
            #endregion

            return View(db.Find<Post>(id));
        }



        [Route("/admin/tagoptions/{pagenumber}")]
        public async Task<IActionResult> TagOptions(int pagenumber = 1)
        {
            #region Pagination

            var tags = db.tags.OrderByDescending(x => x.id);
            var pTags = await PagingList.CreateAsync(tags, 20, pagenumber);
            var result = new PagedResult<Tag>()
            {
                Data = pTags,
                PageNumber = pagenumber,
                PageSize = 20,
                TotalItems = db.tags.Count(),
            };
            #endregion

            return View(result);
        }



        public IActionResult CategoryUpdate(int id)
        {
            return View(db.Find<Category>(id));
        }



        public IActionResult TagUpdate(int id)
        {
            return View(db.Find<Tag>(id));
        }



        [Route("/admin/managecomments/{pagenumber}")]
        public IActionResult ManageComments()
        {
            db.mainComments.Include(x => x.applicationUser).Include(x => x.post).Include(x => x.subComments).ThenInclude(x => x.applicationUser).ToList();

            var subComments = db.subComments.Where(x => x.IsConfirm == false).OrderByDescending(x => x.id).ToList();

            #region ViewBag&ViewData

            ViewData["mainCommentss"] = db.mainComments.Where(x => x.IsConfirm == false).OrderByDescending(x => x.id).ToList();
            ViewBag.mainComments = db.mainComments.Where(x => x.IsConfirm == false).Count();
            
            #endregion

            return View(subComments);
        }



        [Route("/admin/manageusers/{Pagenumber}")]
        public async Task<IActionResult> ManageUsers(int pagenumber)
        {
            #region Pagination
            
            var users = userManager.Users.OrderByDescending(x => x.accountCrationTime);

            var usersList = await PagingList.CreateAsync(users, 50, pagenumber);

            var usersPagination = new PagedResult<ApplicationUser>()
            {
                Data = usersList.ToList(),
                PageNumber = pagenumber,
                PageSize = 50,
                TotalItems = userManager.Users.Count()
            };

            #endregion

            return View(usersPagination);
        }



        [Route("/admin/sensitivewordsoption/{pagenumber}")]
        public async Task<IActionResult> SensitiveWordsOption(int pagenumber)
        {
            #region PaginationArea
            var sWords = db.sensitiveWords.OrderByDescending(x => x.id);

            var sWordList = await PagingList.CreateAsync(sWords, 20, pagenumber);

            var sWordPagination = new PagedResult<SensitiveWord>()
            {
                Data = sWordList,
                PageSize = 20,
                PageNumber = pagenumber,
                TotalItems = db.sensitiveWords.Count()
            };

            ViewBag.sWordCount = sWordList.Count();
            #endregion

            return View(sWordPagination);
        }



        public IActionResult AboutusChange()
        {
            return View(db.Find<AboutUs>(1));
        }



        [Route("/admin/inbox/{pagenumber}")]
        public async Task<IActionResult> Inbox(int pagenumber)
        {
            #region Pagination

            var inboxes = db.contactUs.OrderByDescending(x => x.id);

            var inboxList = await PagingList.CreateAsync(inboxes, 10, pagenumber);

            var pagination = new PagedResult<ContactUs>()
            {
                Data = inboxList.ToList(),
                PageNumber = pagenumber,
                PageSize = 10,
                TotalItems = db.contactUs.Count()
            };
            #endregion

            return View(pagination);
        }



        public async Task<IActionResult> SearchUser(string username)
        {
            return View(await userManager.FindByNameAsync(username));
        }
    }
}
