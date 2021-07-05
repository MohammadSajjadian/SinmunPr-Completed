using cloudscribe.Pagination.Models;
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
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SinmunPr.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> SignInManager;
        DBsinmun db;

        public AccountController(UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager, DBsinmun _db)
        {
            userManager = _userManager;
            SignInManager = _signInManager;
            db = _db;
        }



        public IActionResult Register()
        {
            return View();
        }



        public async Task<IActionResult> RegisterConfirmlvlOne([FromServices] IMail mail, RegisterViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.username);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = model.username,
                    Email = model.email,
                };

                var status = await userManager.CreateAsync(user, model.password);
                if (status.Succeeded)
                {
                    try
                    {
                        #region CreateToken&Address

                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        var address = Url.Action("registerconfirmlvltwo", "account", new { userId = user.Id, token = token, password = model.password }
                        , "https");

                        #endregion

                        #region SendEmailConfirm&CreateTokenDateTime

                        mail.SendMail("تایید حساب کاربری", $"لطفا برای تایید حساب کاربری خود <b><a href='{address}'>اینجا</a></b> کلیک کنید.",
                            user.Email);

                        user.registerTokenCreationTime = DateTime.Now;
                        await userManager.UpdateAsync(user);

                        #endregion

                        TempData["msg"] = "ایمیل تایید حساب کاربری برای شما ارسال شد.";

                        return RedirectToAction("index", "home");
                    }
                    catch
                    {
                        TempData["msg"] = "خطا در ارسال ایمیل تایید. لطفا دوباره امتحان کنید.";

                        return RedirectToAction(nameof(Register));
                    }
                }
                else
                {
                    TempData["registerfail"] = "نام کاربری فقط میتواند شامل حروف بزرگ، حروف کوچک، اعداد، نماد های -_.@$ و خط فاصله باشد. " +
                        "رمز عبور باید شامل حروف بزرگ، حروف کوچک، اعداد و نماد باشد.";

                    return RedirectToAction(nameof(Register));
                }
            }
            else
            {
                TempData["userexist"] = "این نام کاربرِی درحال حاظر موجود است.";

                return RedirectToAction(nameof(Register));
            }
        }



        public async Task<IActionResult> RegisterConfirmlvlTwo(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (DateTime.Now.Subtract(user.registerTokenCreationTime) < TimeSpan.FromSeconds(20))
            {
                await userManager.ConfirmEmailAsync(user, token);

                await userManager.AddToRoleAsync(user, "user");

                TempData["msg"] = "حساب کاربری شما با موفقیت فعال شد. لطفا از طریق گزینه ورود، وارد شوید.";
            }
            else
            {
                await userManager.DeleteAsync(user);

                TempData["msg"] = "مدت زمان تایید حساب کاربری به اتمام رسیده است. لطفا دوباره تلاش کنید.";

            }

            return RedirectToAction("index", "home");
        }



        public IActionResult Login()
        {
            return View();
        }



        public async Task<IActionResult> LoginConfirm(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.username);

            if (user != null && user.EmailConfirmed == true)
            {
                var status = await SignInManager.PasswordSignInAsync(user, model.password, model.rememmberme, true);

                if (status.IsLockedOut)
                {
                    TempData["msg"] = "اکانت شما به مدت 1 دقیقه مسدود خواهد شد.";
                }

                if (status.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    TempData["loginfail"] = "نام کاربری یا رمزعبور اشتباه است.";

                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["usernotfound"] = "کاربر موجود نیست.";

                return RedirectToAction(nameof(Login));
            }
        }



        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }



        public async Task<IActionResult> RessPasslvlOne(string email, [FromServices] IMail mail)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                #region CreateToken,Address,Subject&Body

                string token = await userManager.GeneratePasswordResetTokenAsync(user);

                var address = Url.Action(nameof(RessPassBridge), "account", new { id = user.Id, token = token }, "https");

                string subject = "تغییر رمز عبور در وبسایت SinMun.ir";
                string body = $"برای تغییر رمز عبور خود <b><a href='{address}'>اینجا</a></b> کلیک کنید.";

                #endregion

                try
                {
                    #region SendEmail&UpdateUser

                    mail.SendMail(subject, body, user.Email);
                    user.ressPassTokenCreationTime = DateTime.Now;
                    await userManager.UpdateAsync(user);

                    #endregion

                    TempData["msg"] = "ایمیل تغییر رمزعبور برای شما ارسال شد";
                }
                catch
                {
                    TempData["msg"] = "خطا در ارسال ایمیل";
                }
            }
            else
            {
                TempData["msg"] = "کاربر مورد نظر موجود نمیباشد.";
            }

            return RedirectToAction(nameof(Login));
        }



        public IActionResult RessPassBridge(string id, string token)
        {
            TempData["id"] = id;
            TempData["token"] = token;

            return RedirectToAction(nameof(RessPasslvlTwo));
        }



        public async Task<IActionResult> RessPasslvlTwo()
        {
            #region Id&Token

            string id = TempData["id"].ToString();
            string token = TempData["token"].ToString();

            #endregion

            var user = await userManager.FindByIdAsync(id);
            if (DateTime.Now.Subtract(user.ressPassTokenCreationTime) < TimeSpan.FromHours(12))
            {
                #region SetSessions

                HttpContext.Session.SetString("id", id);
                HttpContext.Session.SetString("token", token);

                #endregion

                return View();
            }
            else
            {
                TempData["msg"] = "لینک تغییر رمز منقضی شده است. لطفا دوباره تلاش کنید.";

                return RedirectToAction(nameof(Login));
            }
        }



        public async Task<IActionResult> RessPasslvlThree(RegisterViewModel model)
        {
            #region GetSessions

            var id = HttpContext.Session.GetString("id");
            var token = HttpContext.Session.GetString("token");

            #endregion

            var user = await userManager.FindByIdAsync(id);

            var change = await userManager.ResetPasswordAsync(user, token, model.password);
            if (change.Succeeded)
            {
                TempData["msg"] = "رمزعبور شما با موفقیت تغییر یافت.";

                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["msg"] = "خطا در تغییر رمز عبور";

                return RedirectToAction(nameof(Login));
            }
        }



        public IActionResult ChangePassword()
        {
            return View();
        }



        public async Task<IActionResult> ChangePasswordConfirm(RegisterViewModel model, string currentPassword)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var status = await userManager.ChangePasswordAsync(user, currentPassword, model.password);
            if (status.Succeeded)
            {
                TempData["msg"] = "رمزعبور شما با موفقیت تغییر یافت.";

                return RedirectToAction("index", "home");
            }
            else
            {
                TempData["changepassfail"] = "تغییر رمز عبور با شکست مواجه شد.";

                return RedirectToAction(nameof(ChangePassword));
            }

        }



        public async Task<IActionResult> ShowProfile(string id, int pagenumber = 1)
        {
            HttpContext.Session.SetString("id", id);
            var user = await userManager.FindByIdAsync(id);

            return RedirectToAction(nameof(MainUser), new
            {
                username = user.UserName.Replace(" ", "-"),
                pagenumber
            });
        }



        [Route("user/{username}/{pagenumber}")]
        public async Task<IActionResult> MainUser(string username, int pagenumber)
        {
            db.posts.Include(x => x.category).Include(x => x.mainComments).ThenInclude(x => x.subComments).ThenInclude(x => x.applicationUser).ToList();

            string id = HttpContext.Session.GetString("id").ToString();

            var user = await userManager.FindByIdAsync(id);

            #region Pagination

            var mComments = db.mainComments.Where(x => x.userId == id && x.IsConfirm == true).OrderByDescending(x => x.id);

            ViewData["mainComments"] = await PagingList.CreateAsync(mComments, 20, pagenumber);

            var result = new PagedResult<MainComment>()
            {
                PageNumber = pagenumber,
                PageSize = 20,
                TotalItems = db.mainComments.Where(x => x.userId == user.Id).Count()
            };
            
            #endregion

            #region ViewBags

            ViewBag.PageNumber = result.PageNumber;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalItems = result.TotalItems;

            ViewBag.posts = db.posts.Where(x => x.userId == id).Count();
            ViewBag.mainCommentsCount = db.mainComments.Where(x => x.userId == id && x.IsConfirm == true).Count();
            ViewBag.subCommentsCount = db.subComments.Where(x => x.userId == id && x.IsConfirm == true).Count();
            
            #endregion

            ViewData["newNews"] = db.posts.OrderByDescending(x => x.id).Take(5).ToList();

            return View(user);
        }



        public async Task<IActionResult> ShowProfileQuotation(string id, int pagenumber = 1)
        {
            HttpContext.Session.SetString("id", id);
            var user = await userManager.FindByIdAsync(id);

            return RedirectToAction(nameof(MainUserQuotation), new { username = user.UserName.Replace(" ", "-"), pagenumber = pagenumber });
        }



        [Route("user/quotation/{username}/{pagenumber}")]
        public async Task<IActionResult> MainUserQuotation(string username, int pagenumber)
        {
            db.posts.Include(x => x.category).Include(x => x.mainComments).ThenInclude(x => x.subComments).ThenInclude(x => x.applicationUser).ToList();

            string id = HttpContext.Session.GetString("id").ToString();

            var user = await userManager.FindByIdAsync(id);

            #region Pagination

            var sComments = db.subComments.Where(x => x.userId == id && x.IsConfirm == true).OrderByDescending(x => x.id);

            ViewData["subComments"] = await PagingList.CreateAsync(sComments, 20, pagenumber);
            var result = new PagedResult<MainComment>()
            {
                PageNumber = pagenumber,
                PageSize = 20,
                TotalItems = db.mainComments.Where(x => x.userId == user.Id).Count()
            };
            #endregion

            #region ViewBags

            ViewBag.PageNumber = result.PageNumber;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalItems = result.TotalItems;

            ViewBag.posts = db.posts.Where(x => x.applicationUser.Id == id).Count();
            ViewBag.mainCommentsCount = db.mainComments.Where(x => x.userId == id && x.IsConfirm == true).Count();
            ViewBag.subCommentsCount = db.subComments.Where(x => x.userId == id && x.IsConfirm == true).Count();
            
            #endregion

            ViewData["newNews"] = db.posts.OrderByDescending(x => x.id).Take(5).ToList();

            return View(user);
        }



        public async Task<IActionResult> ShowProfilePosts(string id, int pagenumber = 1)
        {
            HttpContext.Session.SetString("id", id);
            var user = await userManager.FindByIdAsync(id);

            return RedirectToAction(nameof(MainUserPosts), new { username = user.UserName.Replace(" ", "-"), pagenumber = pagenumber });
        }



        [Route("user/posts/{username}/{pagenumber}")]
        public async Task<IActionResult> MainUserPosts(string username, int pagenumber)
        {
            #region Includes

            db.mainComments.Include(x => x.post).ThenInclude(x => x.applicationUser).ToList();
            db.posts.Include(x => x.category).Include(x => x.mainComments).ThenInclude(x => x.subComments).ToList();
            
            #endregion

            string id = HttpContext.Session.GetString("id").ToString();

            var user = await userManager.FindByIdAsync(id);

            #region Pagination

            var posts = db.posts.Where(x => x.applicationUser.Id == user.Id).OrderByDescending(x => x.id);

            ViewData["mPosts"] = await PagingList.CreateAsync(posts, 10, pagenumber);
            var result = new PagedResult<MainComment>()
            {
                PageNumber = pagenumber,
                PageSize = 10,
                TotalItems = db.posts.Where(x => x.applicationUser.Id == user.Id).Count()
            };
            #endregion

            #region ViewBags

            ViewBag.PageNumber = result.PageNumber;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalItems = result.TotalItems;

            ViewBag.posts = db.posts.Where(x => x.applicationUser.Id == id).Count();
            ViewBag.mainCommentsCount = db.mainComments.Where(x => x.userId == id && x.IsConfirm == true).Count();
            ViewBag.subCommentsCount = db.subComments.Where(x => x.userId == id && x.IsConfirm == true).Count();
            
            #endregion

            ViewData["newNews"] = db.posts.OrderByDescending(x => x.id).Take(5).ToList();

            return View(user);
        }



        public IActionResult EditProfile()
        {
            return View();
        }



        public async Task<IActionResult> EditProfileConfirm(EditProfileViewModel model, [FromServices] IResize resize)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            //Confirm UserName Format...
            if (model.username != null)
            {
                var anotherUser = await userManager.FindByNameAsync(model.username);
                if (anotherUser == null)
                {
                    if (Regex.IsMatch(model.username, "^(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$"))
                    {
                        user.UserName = model.username;
                    }
                    else
                    {
                        TempData["usernotvalid"] = "فرمت نام کاربری قابل قبول نیست.";

                        return RedirectToAction(nameof(EditProfile));
                    }
                }
                else
                {
                    TempData["useralreadyexist"] = "این نام کاربری درحال حاظر موجود میباشد.";

                    return RedirectToAction(nameof(EditProfile));
                }
            }

            //PhoneNumber only Numbers Confirmation...
            if (model.phonenumber != null)
            {
                if (Regex.IsMatch(model.phonenumber, "^[0-9]*$"))
                {
                    user.PhoneNumber = model.phonenumber;
                }
                else
                {
                    TempData["phonenumber"] = "برای شماره تلفن فقط اعداد مجاز است";

                    return RedirectToAction(nameof(EditProfile));
                }
            }

            if (model.bio != null)
            {
                user.bio = model.bio;
            }

            if (model.firstname != null)
            {
                user.firstName = model.firstname;
            }

            if (model.lastname != null)
            {
                user.lastName = model.lastname;
            }

            if (model.avatar != null)
            {
                if (model.avatar.Length <= 5 * Math.Pow(1024, 2))
                {
                    if (System.IO.Path.GetExtension(model.avatar.FileName) == ".jpg" || System.IO.Path.GetExtension(model.avatar.FileName) == ".png")
                    {
                        byte[] b = new byte[model.avatar.Length];

                        model.avatar.OpenReadStream().Read(b, 0, b.Length);

                        user.bigAvatar = resize.Resizer(b, 120, 120, ImageFormat.Jpeg);
                        user.smallAvatar = resize.Resizer(b, 50, 50, ImageFormat.Jpeg);
                    }
                }
                else
                {
                    TempData["avatarlengh"] = "حجم عکس باید کمتر از 5 مگابایت باشد.";
                }
            }

            try
            {
                await userManager.UpdateAsync(user);
                await SignInManager.RefreshSignInAsync(user);

                TempData["msg"] = "اطلاعات با موفقیت ویرایش شد.";

                return RedirectToAction("index", "home");
            }
            catch
            {
                TempData["msg"] = "ویرایش اطلاعات با شکست مواجه شد.";

                return RedirectToAction(nameof(EditProfile));
            }
        }



        public async Task<IActionResult> ConfirmedMainCommentBan(string userId, int postId, int pagenumber)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                user.EmailConfirmed = false;

                await userManager.UpdateAsync(user);
                db.SaveChanges();

                TempData["msg"] = "کاربر مورد نظر مسدود شد.";

                return RedirectToAction("PostShowBridge", "post", new { id = postId, pagenumber = pagenumber });
            }
            catch
            {
                TempData["msg"] = "مسدود سازی کاربر با شکست مواجه شد.";

                return RedirectToAction("PostShowBridge", "post", new { id = postId, pagenumber = pagenumber });
            }
        }



        public async Task<IActionResult> ConfirmedSubCommentBan(string userId, int postId, int pagenumber)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                user.EmailConfirmed = false;

                await userManager.UpdateAsync(user);
                db.SaveChanges();

                TempData["msg"] = "کاربر مورد نظر مسدود شد.";

                return RedirectToAction("PostShowBridge", "post", new { id = postId, pagenumber = pagenumber });
            }
            catch
            {
                TempData["msg"] = "مسدود سازی کاربر با شکست مواجه شد.";

                return RedirectToAction("PostShowBridge", "post", new { id = postId, pagenumber = pagenumber });
            }
        }



        public async Task<string> AddUserToRole(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);

            await userManager.AddToRoleAsync(user, roleName);

            return $"کاربر {user.UserName} به گروه {roleName} اضاف شد.";
        }



        public async Task<string> RemoveUserFromRole(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);

            await userManager.RemoveFromRoleAsync(user, roleName);

            return $"کاربر {user.UserName} از گروه {roleName} خارج شد.";
        }



        public async Task<IActionResult> BanUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.EmailConfirmed = false;

            await userManager.UpdateAsync(user);
            db.SaveChanges();

            return Json(new
            {
                msg = $"کاربر {user.UserName} مسدود شد.",
                activeUsers = userManager.Users.Where(x => x.EmailConfirmed == true).Count(),
                banUsers = userManager.Users.Where(x => x.EmailConfirmed == false).Count()
            });
        }



        public async Task<IActionResult> UnBanUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.EmailConfirmed = true;

            await userManager.UpdateAsync(user);
            db.SaveChanges();

            return Json(new
            {
                msg = $"اکانت کاربر {user.UserName} فعال شد.",
                activeUsers = userManager.Users.Where(x => x.EmailConfirmed == true).Count(),
                banUsers = userManager.Users.Where(x => x.EmailConfirmed == false).Count()
            });
        }
    }
}
