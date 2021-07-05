using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using SinmunPr.Areas.Identity.Data;
using SinmunPr.Data;
using SinmunPr.Models;
using SinmunPr.Services;
using SinmunPr.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Controllers
{
    [Authorize(policy: "adminPolicy")]
    public class PostController : Controller
    {
        DBsinmun db;
        UserManager<ApplicationUser> userManager;
        public PostController(DBsinmun _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }


        [AllowAnonymous]
        public IActionResult PostShowBridge(int id, int pagenumber = 1)
        {
            var post = db.Find<Post>(id);
            db.posts.Include(x => x.category).ToList();

            #region PostViewCount
            var IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var IsInPostIp = db.postIps.Any(x => x.postId == id && x.IP == IP);

            if (IsInPostIp == false)
            {
                PostIp postIp = new() { postId = id, IP = IP };

                post.views++;

                db.Add(postIp);
                db.Update(post);
                db.SaveChanges();
            }
            #endregion

            return RedirectToAction(nameof(PostShow), new
            {
                id = post.id,
                category = post.category.Ename.Replace(" ", "-"),
                topic = post.topic.Replace(" ", "-"),
                pagenumber = pagenumber
            });
        }



        [AllowAnonymous]
        [Route("{category}/{id}/{topic}/{pagenumber}")]
        public async Task<IActionResult> PostShow(int id, string category, string topic, int pagenumber)
        {
            #region LazyLoading

            db.posts.Include(x => x.category).Include(x => x.sources).Include(x => x.postTagIds)
                .ThenInclude(x => x.Tag).Include(x => x.applicationUser).Include(x => x.mainComments)
                .ThenInclude(x => x.subComments).ThenInclude(x => x.applicationUser).ToList();
            db.mainComments.Include(x => x.applicationUser).Include(x => x.subComments).ToList();

            #endregion

            #region ViewData

            var mainComments = db.mainComments.Where(x => x.postId == id && x.IsConfirm == true).OrderByDescending(x => x.id);
            ViewData["sources"] = db.sources.Where(x => x.postId == id).ToList();
            ViewData["tags"] = db.postTagIds.Where(x => x.postId == id).ToList();
            ViewData["subComments"] = db.subComments.Where(x => x.MainComment.post.id == id && x.IsConfirm == true).ToList();
            var newestpost = db.posts.OrderByDescending(x => x.id).Take(6).ToList();
            newestpost.Remove(db.Find<Post>(id));
            ViewData["newestpost"] = newestpost;

            #endregion

            #region Pagination
            ViewData["mainComments"] = await PagingList.CreateAsync(mainComments, 20, pagenumber);

            var PagingNumbers = new PagedResult<MainComment>()
            {
                PageNumber = pagenumber,
                PageSize = 20,
                TotalItems = db.mainComments.Where(x => x.postId == id).Count(),
            };
            #endregion

            #region ViewBag

            ViewBag.previous = db.posts.Where(x => x.id < id).OrderByDescending(x => x.id).Select(x => x.id).FirstOrDefault();
            ViewBag.next = db.posts.Where(x => x.id > id).OrderBy(x => x.id).Select(x => x.id).FirstOrDefault();
            ViewBag.PageNumber = PagingNumbers.PageNumber;
            ViewBag.PageSize = PagingNumbers.PageSize;
            ViewBag.TotalItems = PagingNumbers.TotalItems;

            #endregion

            return View(db.Find<Post>(id));
        }



        public async Task<IActionResult> PostInsertConfirm(PostViewModel model, [FromServices] IResize resize)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            #region GiveAttributeToPost

            Post post = new()
            {
                topic = model.topic,
                shortDes = model.shortDes,
                content = model.content,
                ePostCreation = DateTime.Now,
                IsInImmediate = model.IsInImmediate,
                IsInTopSlideBar = model.IsInTopSlideBar,
                IsInEditorsChoice = model.IsInEditorsChoice,
                IsInDedicated = model.IsInDedicated,
                IsInImportant = model.IsInImportant,
                categoryId = model.categoryId,
                userId = user.Id,
                sources = new List<Source>(),
                postTagIds = new List<PostTagId>()
            };

            PersianCalendar persian = new();
            post.pPostCreation = $"{persian.GetYear(DateTime.Now)}/{persian.GetMonth(DateTime.Now)}/" +
                $"{persian.GetDayOfMonth(DateTime.Now)} - {persian.GetHour(DateTime.Now)}:{persian.GetMinute(DateTime.Now)}";

            #endregion

            if (model.mainImg != null && model.mainImg.Length <= 5 * Math.Pow(1024, 2))
            {
                if (System.IO.Path.GetExtension(model.mainImg.FileName) == ".jpg" || System.IO.Path.GetExtension(model.mainImg.FileName) == ".png")
                {
                    byte[] b = new byte[model.mainImg.Length];

                    model.mainImg.OpenReadStream().Read(b, 0, b.Length);

                    post.mainImg = resize.Resizer(b, 479, 359, ImageFormat.Jpeg);
                    post.bigSlideBar = resize.Resizer(b, 479, 240, ImageFormat.Jpeg);
                    post.dedicated = resize.Resizer(b, 459, 344, ImageFormat.Jpeg);
                    post.footerImg = resize.Resizer(b, 95, 112, ImageFormat.Jpeg);

                }
            }

            if (model.sourceName != null && model.sourceLink != null)
            {
                int i = 1;
                model.sourceName.Select(x => new { id = i++, name = x }).ToList().ForEach(x =>
                {
                    if (x.name != null)
                    {
                        Source source = new();
                        source.name = x.name;

                        int j = 1;
                        model.sourceLink.Select(y => new { id = j++, link = y }).ToList().ForEach(y =>
                        {
                            if (y.link != null && y.id == x.id)
                            {
                                source.link = y.link;
                            }
                        });
                        post.sources.Add(source);
                    }
                });
            }

            if (model.tags != null)
            {
                model.tags.ForEach(x =>
                {
                    if (x != -1)
                    {
                        PostTagId postTagId = new PostTagId()
                        {
                            postId = post.id,
                            tagId = x
                        };

                        post.postTagIds.Add(postTagId);
                    }
                });
            }

            try
            {
                db.Add(post);
                db.SaveChanges();

                TempData["msg"] = "مقاله مورد نظر با موفقیت ثبت شد.";
            }
            catch
            {
                TempData["msg"] = "ثبت مقاله با خطا مواجه شد.";
            }

            return RedirectToAction("postinsert", "admin");
        }



        public IActionResult PostDelete(int id)
        {
            try
            {
                var post = db.Find<Post>(id);

                db.Remove(post);
                db.SaveChanges();

                TempData["msg"] = "پست مورد نظر با موفقیت حذف شد.";

                return RedirectToAction("postoptions", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "حذف کردن پست با شکست مواجه شد.";

                return RedirectToAction("postoptions", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult PostUpdateConfirm(int id, int categoryId, IFormFile mainImg, string topic, string shortDes,
            string content, List<string> sourceLink, List<string> sourceName, List<int> tags, string IsInImmediate,
            string IsInEditorsChoice, string IsInImportant, string IsInTopSlideBar, string IsInDedicated, [FromServices] IResize resize)
        {
            #region FindPost&Equality
            var post = db.Find<Post>(id);

            post.categoryId = categoryId;
            post.topic = topic;
            post.shortDes = shortDes;
            post.content = content;
            post.IsInImmediate = IsInImmediate == "on" ? true : false;
            post.IsInEditorsChoice = IsInEditorsChoice == "on" ? true : false;
            post.IsInImportant = IsInImportant == "on" ? true : false;
            post.IsInTopSlideBar = IsInTopSlideBar == "on" ? true : false;
            post.IsInDedicated = IsInDedicated == "on" ? true : false;

            #endregion

            #region ImageProtocol
            if (mainImg != null && mainImg.Length <= 5 * Math.Pow(1024, 2))
            {
                if (System.IO.Path.GetExtension(mainImg.FileName) == ".jpg" || System.IO.Path.GetExtension(mainImg.FileName) == ".png")
                {
                    byte[] b = new byte[mainImg.Length];

                    mainImg.OpenReadStream().Read(b, 0, b.Length);

                    post.mainImg = resize.Resizer(b, 479, 359, ImageFormat.Jpeg);
                    post.bigSlideBar = resize.Resizer(b, 479, 240, ImageFormat.Jpeg);
                    post.dedicated = resize.Resizer(b, 459, 344, ImageFormat.Jpeg);
                    post.footerImg = resize.Resizer(b, 95, 112, ImageFormat.Jpeg);
                }
            }

            if (sourceName != null && sourceLink != null)
            {
                int i = 1;
                sourceName.Select(x => new { id = i++, name = x }).ToList().ForEach(x =>
                {
                    if (x.name != null)
                    {
                        Source source = new Source();
                        source.postId = post.id;
                        source.name = x.name;

                        int j = 1;
                        sourceLink.Select(y => new { id = j++, link = y }).ToList().ForEach(y =>
                        {
                            if (y.link != null && y.id == x.id)
                            {
                                source.link = y.link;
                            }
                        });
                        db.Add(source);
                    }
                });
            }

            if (tags != null)
            {
                tags.ForEach(x =>
                {
                    if (x != -1)
                    {
                        PostTagId postTagId = new PostTagId()
                        {
                            postId = post.id,
                            tagId = x
                        };

                        db.Add(postTagId);
                    }
                });
            }
            #endregion

            #region TryCatch
            try
            {
                db.Update(post);
                db.SaveChanges();

                TempData["msg"] = "پست مورد نظر با موفقت ویرایش شد.";

                return RedirectToAction("postoptions", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "ویرایش پست با شکست مواجه شد.";

                return RedirectToAction("postoptions", "admin", new { pagenumber = 1 });
            }
            #endregion
        }



        public IActionResult CategoryInsert(string Ename, string Pname)
        {
            try
            {
                if (Ename != null && Pname != null)
                {
                    Category category = new() { Ename = Ename, Pname = Pname, IsInMainPage = false };

                    db.Add(category);
                    db.SaveChanges();

                    TempData["msg"] = "دسته بندی با موفقیت اضاف شد.";
                }
                return RedirectToAction("categoryoptions", "admin");
            }
            catch
            {
                TempData["msg"] = "اضاف کردن دسته بندی با شکست مواجه شد.";

                return RedirectToAction("categoryoptions", "admin");
            }
        }



        public IActionResult CategoryDelete(int id)
        {
            try
            {
                var category = db.Find<Category>(id);

                db.Remove(category);
                db.SaveChanges();

                TempData["msg"] = "بسته بندی با موفقیت حذف شد.";

                return RedirectToAction("categoryoptions", "admin");
            }
            catch
            {
                TempData["msg"] = "حذف دسته بندی با شکست مواجه شد.";

                return RedirectToAction("categoryoptions", "admin");
            }
        }



        public IActionResult TagInsert(string Pname, string Ename)
        {
            try
            {
                if (Pname != null && Ename != null)
                {
                    Tag tag = new() { Pname = Pname, Ename = Ename };

                    db.Add(tag);
                    db.SaveChanges();

                    TempData["msg"] = "برچسب با موفقیت اضاف شد.";
                }
                return RedirectToAction("tagoptions", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "اضاف کردن برچسب با شکست مواجه شد.";

                return RedirectToAction("tagoptions", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult CategoryUpdateConfirm(int id, string Pname, string Ename, string IsInMainPage)
        {
            try
            {
                #region FindCategory&Equalize

                var category = db.Find<Category>(id);

                category.Pname = Pname;
                category.Ename = Ename;
                category.IsInMainPage = IsInMainPage == "on" ? true : false;

                #endregion

                db.Update(category);
                db.SaveChanges();

                TempData["msg"] = "دسته بندی با موفقیت ویرایش شد.";

                return RedirectToAction("categoryoptions", "admin");
            }
            catch
            {
                TempData["msg"] = "ویرایش کردن دسته بندی با شکست مواجه شد.";

                return RedirectToAction("categoryoptions", "admin");
            }
        }



        public IActionResult TagDelete(int id)
        {
            try
            {
                var tag = db.Find<Tag>(id);

                db.Remove(tag);
                db.SaveChanges();

                TempData["msg"] = "برچسب با موفقیت حذف شد.";

                return RedirectToAction("tagoptions", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "حذف کردن برچسب با شکست مواجه شد.";

                return RedirectToAction("tagoptions", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult TagUpdateConfirm(int id, string Pname, string Ename)
        {
            try
            {
                #region FindTag&Equalize

                var tag = db.Find<Tag>(id);

                tag.Pname = Pname;
                tag.Ename = Ename;

                #endregion

                db.Update(tag);
                db.SaveChanges();

                TempData["msg"] = "برچسب با موفقیت ویرایش شد.";

                return RedirectToAction("tagoptions", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "ویرایش کردن برچسب با شکست مواجه شد.";

                return RedirectToAction("tagoptions", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult SourceDelete(int sourceId, int postId)
        {
            try
            {
                var SourceId = db.Find<Source>(sourceId);

                db.Remove(SourceId);
                db.SaveChanges();

                TempData["msg"] = "منبع مورد نظر حذف شد.";

                return RedirectToAction("postupdate", "admin", new { id = postId });
            }
            catch
            {
                TempData["msg"] = "حذف منبع مورد نظر با شکست مواجه شد.";

                return RedirectToAction("postupdate", "admin", new { id = postId });
            }
        }



        public IActionResult TagPostDelete(int tagId, int postId)
        {
            try
            {
                var posttagId = db.Find<PostTagId>(tagId);

                db.Remove(posttagId);
                db.SaveChanges();

                TempData["msg"] = "برچسب با موفقیت حذف شد.";

                return RedirectToAction("postupdate", "admin", new { id = postId });
            }
            catch
            {
                TempData["msg"] = "حذف برچسب با شکست مواجه شد.";

                return RedirectToAction("postupdate", "admin", new { id = postId });
            }
        }



        [AllowAnonymous]
        public async Task<IActionResult> CommentConfirm(int postId, string content)
        {
            #region FindMainComment&User
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var mainComment = new MainComment()
            {
                postId = postId,
                userId = user.Id,
                content = content,
                creationTime = DateTime.Now,
            };
            #endregion

            #region FilteringSystem

            var sensitiveWords = db.sensitiveWords.ToList();
            if (sensitiveWords.Count() != 0)
            {
                foreach (var item in sensitiveWords)
                {
                    if (content.Contains(item.word) == false) mainComment.IsConfirm = true;

                    else
                    {
                        mainComment.IsConfirm = false;
                        break;
                    }
                }

                if (await userManager.IsInRoleAsync(user, "admin")) mainComment.IsConfirm = true;
            }
            else mainComment.IsConfirm = true;

            #endregion

            #region AddMainComment
            if (mainComment.IsConfirm == true)
            {
                db.Add(mainComment);
                db.SaveChanges();

                TempData["msg"] = "نظر شما با موفقیت ثبت شد.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId });
            }
            else
            {
                db.Add(mainComment);
                db.SaveChanges();

                TempData["msg"] = "نظر شما با موفقیت ثبت و پس از تایید در وبسایت قرار خواهد گرفت.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId });
            }
            #endregion
        }



        [AllowAnonymous]
        public async Task<IActionResult> CommentReplyConfirm(int mainCommentId, int postId, string content)
        {
            #region FindMainComment&SubComment&User&LazyLoading

            db.mainComments.Include(x => x.applicationUser).ToList();

            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var mainComment = db.Find<MainComment>(mainCommentId);

            var subComment = new SubComment()
            {
                mainCommentId = mainCommentId,
                userId = user.Id,
                creationTime = DateTime.Now,
            };

            if (mainComment.applicationUser.firstName != null && mainComment.applicationUser.lastName != null)
            {
                subComment.content = $"@{mainComment.applicationUser.firstName} {mainComment.applicationUser.lastName}\n{content.Replace("<br>", "\r\n")}";
            }
            else
            {
                subComment.content = $"@{mainComment.applicationUser.UserName}\n{content.Replace("<br>", "\r\n")}";
            }
            #endregion

            #region FilteringSystem
            var sensitiveWords = db.sensitiveWords.ToList();

            if (sensitiveWords.Count() != 0)
            {
                foreach (var item in sensitiveWords)
                {
                    if (content.Contains(item.word) == false)
                    {
                        subComment.IsConfirm = true;
                    }
                    else
                    {
                        subComment.IsConfirm = false;
                        break;
                    }
                }
                if (await userManager.IsInRoleAsync(user, "admin")) subComment.IsConfirm = true;
            }
            else subComment.IsConfirm = true;
            #endregion

            #region AddMainComment

            if (subComment.IsConfirm == true)
            {
                db.Add(subComment);
                db.SaveChanges();

                TempData["msg"] = "نظر شما با موفقیت ثبت شد.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId });
            }
            else
            {
                db.Add(subComment);
                db.SaveChanges();

                TempData["msg"] = "نظر شما با موفقیت ثبت و پس از تایید در وبسایت قرار خواهد گرفت.";

                return RedirectToAction(nameof(PostShowBridge), new
                {
                    id = postId
                });
            }
            #endregion
        }



        [AllowAnonymous]
        public async Task<IActionResult> ReplyToReplyConfirm(int postId, int mainCommentId, int subCommentId, string content)
        {
            #region FindSubComment&User&AddSubComment&LazyLoading

            db.subComments.Include(x => x.applicationUser).ToList();

            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var subCommentClass = new SubComment()
            {
                mainCommentId = mainCommentId,
                userId = user.Id,
                creationTime = DateTime.Now,
            };

            var subComment = db.Find<SubComment>(subCommentId);
            if (subComment.applicationUser.firstName != null && subComment.applicationUser.lastName != null)
            {
                subCommentClass.content = $"@{subComment.applicationUser.firstName} {subComment.applicationUser.lastName}\n{content.Replace("<br>", "\r\n")}";
            }
            else
            {
                subCommentClass.content = $"@{subComment.applicationUser.UserName}\n{content.Replace("<br>", "\r\n")}";
            }
            #endregion

            #region FilteringSystem
            var sensitiveWords = db.sensitiveWords.ToList();

            if (sensitiveWords.Count != 0)
            {
                foreach (var item in sensitiveWords)
                {
                    if (content.Contains(item.word) == false)
                    {
                        subCommentClass.IsConfirm = true;
                    }
                    else
                    {
                        subCommentClass.IsConfirm = false;
                        break;
                    }
                }
                if (await userManager.IsInRoleAsync(user, "admin")) subCommentClass.IsConfirm = true;
            }
            else subCommentClass.IsConfirm = true;

            #endregion

            #region AddMainComment

            if (subCommentClass.IsConfirm == true)
            {
                db.Add(subCommentClass);
                db.SaveChanges();

                TempData["msg"] = "نظر شما با موفقیت ثبت شد.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId });
            }
            else
            {
                db.Add(subCommentClass);
                db.SaveChanges();

                TempData["msg"] = "نظر شما با موفقیت ثبت و پس از تایید در وبسایت قرار خواهد گرفت.";

                return RedirectToAction(nameof(PostShowBridge), new
                {
                    id = postId
                });
            }
            #endregion
        }



        public IActionResult MainCommentConfirm(int id)
        {
            try
            {
                #region FindMainComment&Confirmation

                var mainComment = db.Find<MainComment>(id);

                mainComment.IsConfirm = true;
                mainComment.creationTime = DateTime.Now;

                #endregion

                db.Update(mainComment);
                db.SaveChanges();

                TempData["msg"] = "کامنت مورد نظر تایید شد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "تایید کامنت با شکست مواجه شد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult MainCommentDelete(int id)
        {
            try
            {
                var maincomment = db.Find<MainComment>(id);

                db.Remove(maincomment);
                db.SaveChanges();

                TempData["msg"] = "کامنت مورد نظر تایید نشد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "حذف کامنت با شکست مواجه شد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult SubCommentConfirm(int id)
        {
            try
            {
                #region FindSubComment&Confirmation

                var subComment = db.Find<SubComment>(id);

                subComment.IsConfirm = true;
                subComment.creationTime = DateTime.Now;

                #endregion

                db.Update(subComment);
                db.SaveChanges();

                TempData["msg"] = "کامنت مورد نظر تایید شد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "تایید کامنت با شکست مواجه شد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult SubCommentDelete(int id)
        {
            try
            {
                var subComment = db.Find<SubComment>(id);

                db.Remove(subComment);
                db.SaveChanges();

                TempData["msg"] = "کامنت مورد نظر تایید نشد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "حذف کامنت با شکست مواجه شد.";

                return RedirectToAction("managecomments", "admin", new { pagenumber = 1 });
            }
        }



        public IActionResult ConfirmedMainCommentDelete(int mainCommentId, int postId, int pagenumber)
        {
            try
            {
                var mainComment = db.Find<MainComment>(mainCommentId);

                db.Remove(mainComment);
                db.SaveChanges();

                TempData["msg"] = "کامنت مورد نظر با موفقیت حذف شد.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId, pagenumber = pagenumber });
            }
            catch
            {
                TempData["msg"] = "حذف کامنت با خطا رو به رو شد.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId, pagenumber = pagenumber });
            }
        }



        public IActionResult ConfirmedSubCommentDelete(int subCommentId, int postId, int pagenumber)
        {
            try
            {
                var subComment = db.Find<SubComment>(subCommentId);

                db.Remove(subComment);
                db.SaveChanges();

                TempData["msg"] = "کامنت مورد نظر با موفقیت حذف شد.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId, pagenumber = pagenumber });
            }
            catch
            {
                TempData["msg"] = "حذف کامنت با خطا رو به رو شد.";

                return RedirectToAction(nameof(PostShowBridge), new { id = postId, pagenumber = pagenumber });
            }
        }



        public IActionResult SensitiveWordInsert(string word, int pagenumber)
        {
            try
            {
                var sWord = new SensitiveWord() { word = word };

                db.Add(sWord);
                db.SaveChanges();

                TempData["msg"] = "کلمه مورد نظر ثبت شد.";

                return RedirectToAction("sensitivewordsoption", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "ثبت کامنت با شکست مواجه شد.";

                return RedirectToAction("sensitivewordsoption", "admin", new { pagenumber = 1 });
            }

        }



        public IActionResult SensitiveWordDelete(int id)
        {
            try
            {
                var word = db.Find<SensitiveWord>(id);

                db.Remove(word);
                db.SaveChanges();

                TempData["msg"] = "کلمه مورد نظر با موفقیت حذف شد.";

                return RedirectToAction("sensitivewordsoption", "admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData["msg"] = "حذف کامنت با شکست مواجه شد.";

                return RedirectToAction("sensitivewordsoption", "admin", new { pagenumber = 1 });
            }
        }



        [AllowAnonymous]
        public IActionResult SearchBridge(string search, int pagenumber = 1)
        {
            return RedirectToAction(nameof(search), new
            {
                search = search,
                pagenumber = pagenumber
            });
        }



        [AllowAnonymous]
        [Route("/search/{search}/{pagenumber}")]
        public async Task<IActionResult> Search(string search, int pagenumber)
        {
            #region LazyLoading

            db.posts.Include(x => x.applicationUser).Include(x => x.category).Include(x => x.mainComments)
                    .ThenInclude(x => x.subComments).ToList();

            #endregion

            #region ViewData

            ViewData["editorchoice"] = db.posts.Where(x => x.IsInEditorsChoice == true).OrderByDescending(x => x.id).Take(6).ToList();
            ViewData["important"] = db.posts.Where(x => x.IsInImportant == true).OrderByDescending(x => x.id).Take(5).ToList();
            ViewData["dedicated"] = db.posts.Where(x => x.IsInDedicated == true).OrderByDescending(x => x.id).Take(4).ToList();
            ViewData["hotposts"] = db.posts.Where(x => x.ePostCreation <= DateTime.Now && x.ePostCreation >= DateTime.Today.AddDays(-7))
                .OrderByDescending(x => x.mainComments.Count).Take(6).ToList();

            #endregion

            #region Pagination

            var posts = db.posts.Where(x => x.topic.Contains(search)).OrderByDescending(x => x.id);

            var postList = await PagingList.CreateAsync(posts, 14, pagenumber);

            var pagination = new PagedResult<Post>()
            {
                Data = postList.ToList(),
                PageNumber = pagenumber,
                PageSize = 14,
                TotalItems = db.posts.Where(x => x.topic.Contains(search)).Count()
            };
            #endregion

            ViewBag.search = search;

            return View(pagination);
        }



        public IActionResult AboutUsConfirm(int id, string shortAbout, string longAbout)
        {
            try
            {
                #region UpdateAboutUs

                var aboutUs = db.Find<AboutUs>(id);

                aboutUs.shortAbout = shortAbout;
                aboutUs.longAbout = longAbout;
                
                #endregion

                db.Update(aboutUs);
                db.SaveChanges();

                TempData["msg"] = "تغییرات با موفقیت ثبت شد.";

                return RedirectToAction("aboutuschange", "admin");
            }
            catch
            {
                TempData["msg"] = "ثبت تغییرات با شکست مواجه شد.";

                return RedirectToAction("aboutuschanges", "admin");
            }
        }



        public IActionResult ContactUsConfirm(ContactUsViewModel model)
        {
            try
            {
                var contactUs = new ContactUs()
                {
                    name = model.name,
                    email = model.email,
                    subject = model.subject,
                    content = model.content,
                    createTime = DateTime.Now
                };

                db.Add(contactUs);
                db.SaveChanges();

                TempData["msg"] = "پیام شما با موفقیت ثبت شد.";

                return RedirectToAction("contactus", "home");
            }
            catch
            {
                TempData["msg"] = "ثبت پیام با شکست مواجه شد.";

                return RedirectToAction("contactus", "home");
            }
        }



        public IActionResult InboxDelete(int id, int pagenumber)
        {
            try
            {
                var inbox = db.Find<ContactUs>(id);

                db.Remove(inbox);
                db.SaveChanges();

                TempData["msg"] = "پیغام مورد نظر با موفقیت حذف شد.";

                return RedirectToAction("inbox", "admin", new { pagenumber = pagenumber });
            }
            catch
            {
                TempData["msg"] = "حذف پیغام با شکست مواجه شد شد.";

                return RedirectToAction("inbox", "admin", new { pagenumber = pagenumber });
            }
        }
    }
}
