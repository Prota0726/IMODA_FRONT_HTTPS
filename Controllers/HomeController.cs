using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using IMODA_FRONT_HTTPS.Models;
using IMODA_FRONT_HTTPS.ViewModels;
using static IMODA_FRONT_HTTPS.Models.fb_registerModelViews;
using System.Collections.Specialized;
using System.Text;
using Newtonsoft.Json;

namespace IMODA_FRONT_HTTPS.Controllers
{
    public class HomeController : Controller
    {

        private test1Entities db = new test1Entities();
        public ActionResult Index()
        {
            Session["language"] = "en";
            ViewModels.IndexViewModel viewModel = new ViewModels.IndexViewModel();
            var banner_set = db.banner.OrderBy(m => m.id).ToList();
            viewModel.banner = banner_set;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult qna()
        {
            QnaViewModels viewModel = new QnaViewModels();
            var question_set = db.question_set.OrderBy(m => m.id).ToList();
            var question_category = db.question_category.OrderBy(m => m.id).ToList();
            viewModel.question_category_data = question_category;
            viewModel.question_set_data = question_set;
            return View(viewModel);
        }
        public ActionResult login()
        {
            if (Session["member_id"] != null)
            {
                Response.Redirect("member");
            }


            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult login(FormCollection post)
        {
            if (Session["member_id"] != null)
            {
                Response.Redirect("member");
            }

            if (post["add_account"] != null)
            {
                string name = post["name"];
                string sex = post["sex"];
                string birthday = post["birthday"];
                string email = post["account"];
                string password = post["password"];
                string new_password = post["new_password"];
                string tel = post["tel"];

                var db_email = db.member
                    .Where(b => b.email == email)
                    .FirstOrDefault();
                if ((db_email != null) || (password != new_password))
                {
                    TempData["message"] = "error";
                    return RedirectToAction("login", "Home");
                }
                member member = new member();
                member.c_id = 0;
                member.c_time = DateTime.Now;
                member.u_id = 0;
                member.u_time = DateTime.Now;
                member.account = email;
                member.email = email;
                member.password = password;
                member.name = name;
                member.birthday = DateTime.ParseExact(birthday, "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                member.sex = Convert.ToInt32(sex);
                //member.tel = tel;
                member.phone = tel;
                member.level = 1;
                member.level_start_time = DateTime.Now;
                member.level_end_time = DateTime.Now.AddYears(1);
                member.feedback = 0;
                member.shopping_gold = 0;
                db.member.Add(member);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }


                Response.Redirect("member");

            }
            return View();
        }

        public ActionResult member()
        {
            if (Session["member_id"] == null)
            {
                Response.Redirect("index");
            }
            var today = DateTime.Now;
            int ordersum = 0;

            MemberViewModels viewModel = new MemberViewModels();
            var memberid = Session["member_id"].ToString();
            var int32memberid = Int32.Parse(Session["member_id"].ToString());
            var time_limt = DateTime.Now.AddDays(-30);
            var nearly_3_month = DateTime.Now.AddDays(-90);
            var member = db.member.Where(b => b.id == int32memberid).ToList();
            var order = db.order_.Where(b => b.m_id == int32memberid && b.c_time >= time_limt).ToList();
            var member_level = db.member_level.OrderBy(m => m.id).ToList();
            var ordersum_list = db.order_.Where(b => b.m_id == int32memberid && b.pay_status == 1).Select(c => c.sum_).ToList();
            var shopping_count = db.order_.Where(b => b.m_id == int32memberid && b.c_time >= nearly_3_month).ToList().Count();
            List<string> ob_orderstatus = new List<string>();
            if (ordersum_list.Count == 0)
            {
                ordersum = 0;
            }
            else
            {
                foreach (var item in ordersum_list)
                {
                    ordersum += item;
                }
            }
            foreach (var item in order)
            {
                var temp = db.ob_order.Where(b => b.order_no == item.order_no).Select(b => b.status).ToString();
                ob_orderstatus.Add(temp);
            }
            viewModel.ob_order_status = ob_orderstatus;
            viewModel.member_data = member;
            viewModel.order_data = order;
            viewModel.member_level_data = member_level;
            viewModel.ordersum = ordersum;
            viewModel.wait_shopping_count = shopping_count;
            viewModel.shopping_count = shopping_count;

            //var dateString2 = DateTime.Now.ToString("yyyy-MM-dd");
            //var dateString1 = member[0].birthday.ToString("yyyy-MM-dd");

            return View(viewModel);


        }
        [HttpPost]
        public ActionResult member(FormCollection post)
        {
            if (post["birthday"] != null)
            {
                var id = Int32.Parse(Session["member_id"].ToString());
                var old_member = db.member
                    .Where(b => b.id == id)
                    .FirstOrDefault();
                old_member.name = post["name"];
                old_member.phone = post["phone"];
                old_member.email = post["email"];
                old_member.birthday = DateTime.ParseExact(post["birthday"], "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                db.Entry(old_member).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                TempData["message"] = "ok";
                return RedirectToAction("index", "Home");
            }
            if (post["password"] != null && (post["password"] == post["new_password"]))
            {
                var id = Int32.Parse(Session["member_id"].ToString());
                var old_member = db.member
                    .Where(b => b.id == id)
                    .FirstOrDefault();
                if (post["old_password"] == old_member.password)
                {
                    old_member.password = post["password"];
                    db.Entry(old_member).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    TempData["message"] = "ok";
                    return RedirectToAction("index", "Home");
                }


            }

            TempData["message"] = "error";
            return RedirectToAction("index", "Home");

        }
        [HttpPost]
        public ActionResult social_login(fb_registerModelViews facebook_Account)
        {
            var member = db.member
                    .Where(b => b.fb_id == facebook_Account.id)
                    .FirstOrDefault();
            if (member == null)
            {
                return Content("fail");
            }
            if ((member != null))
            {
                TempData["message"] = "login success !";
                Session["username"] = member.name;

                Session["member_id"] = member.id;
                Session["level"] = member.level;
                //Response.Redirect("index");
            }


            return Content("success");
        }
        public ActionResult fb_register()
        {


            return View();
        }
        [HttpPost]
        public ActionResult fb_register(FormCollection post)
        {
            var fb_id = post["fb_id"];
            var sex = post["sex"];
            var name = post["name"];
            var date = post["birthday"];
            var account = post["account"];
            var tel = post["tel"];
            if (fb_id != null && account != null)
            {
                var old_member = db.member
                    .Where(b => b.email == account)
                    .FirstOrDefault();
                if (old_member == null)
                {
                    member member = new member();
                    member.c_id = 0;
                    member.c_time = DateTime.Now;
                    member.u_id = 0;
                    member.u_time = DateTime.Now;
                    member.account = account;
                    member.email = account;
                    member.password = fb_id;
                    member.name = name;
                    member.birthday = DateTime.ParseExact(date, "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                    member.sex = Convert.ToInt32(sex);
                    //member.tel = tel;
                    member.phone = tel;
                    member.level = 1;
                    member.level_start_time = DateTime.Now;
                    member.level_end_time = DateTime.Now.AddYears(1);
                    member.feedback = 0;
                    member.shopping_gold = 0;
                    member.fb_id = fb_id;
                    db.member.Add(member);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    Session["member_id"] = member.id;
                    Session["level"] = member.level;

                    Response.Redirect("member");
                }
                else
                {
                    old_member.fb_id = fb_id;
                    old_member.name = name;
                    old_member.birthday = DateTime.ParseExact(date, "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                    db.Entry(old_member).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    Session["member_id"] = old_member.id;
                    Session["level"] = old_member.level;
                    Response.Redirect("member");
                }


            }




            return View();
        }
        public ActionResult line_login()
        {
            string response_type = "code";
            string client_id = "1654660526";
            string redirect_uri = HttpUtility.UrlEncode("https://line-login-starter-20200811.herokuapp.com/auth");
            string state = "aaa";
            string LineLoginUrl = string.Format("https://access.line.me/oauth2/v2.1/authorize?response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope=openid%20profile&nonce=09876xyz",
                response_type,
                client_id,
                redirect_uri,
                state
                );
            return Redirect(LineLoginUrl);

        }
        public class LineLoginToken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string id_token { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }

        public class LineUserProfile
        {
            public string userId { get; set; }
            public string displayName { get; set; }
            public string pictureUrl { get; set; }
            public string statusMessage { get; set; }
            public string email { get; set; }
        }
        //以下自行修改從WebConfig讀取
        string redirect_uri = HttpUtility.UrlEncode("https://localhost:44384/Home/AfterLineLogin");
        string client_id = "1654660526";
        string client_secret = "7c82a8d2dd3c0fa465e66ae90abaeac6";
        public ActionResult GetLineLoginUrl()
        {
            //if (Request.IsAjaxRequest() == false)
            //{
            //    return Content("");
            //}
            //只讓本機Ajax讀取LineLoginUrl

            //state使用隨機字串比較安全
            //每次Ajax Request都產生不同的state字串，避免駭客拿固定的state字串將網址掛載自己的釣魚網站獲取用戶的Line個資授權(CSRF攻擊)
            string state = Guid.NewGuid().ToString();
            TempData["state"] = state;//利用TempData被取出資料後即消失的特性，來防禦CSRF攻擊
            //如果是ASP.net Form，就改成放入Session或Cookie，之後取出資料時再把Session或Cookie設為null刪除資料
            string LineLoginUrl =
             $@"https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={client_id}&redirect_uri={redirect_uri}&state={state}&scope={HttpUtility.UrlEncode("openid profile email")}";
            //scope給openid是程式為了抓id_token用，設email則為了id_token的Payload裡才會有用戶的email資訊
            return Redirect(LineLoginUrl);

        }

        public ActionResult AfterLineLogin(string state, string code, string error, string error_description)
        {
            if (!string.IsNullOrEmpty(error))
            {//用戶沒授權你的LineApp
                ViewBag.error = error;
                ViewBag.error_description = error_description;
                return View();
            }

            if (TempData["state"] == null)
            {//可能使用者停留Line登入頁面太久

                return Content("頁面逾期");

            }

            if (Convert.ToString(TempData["state"]) != state)
            {//使用者原先Request QueryString的TempData["state"]和Line導頁回來夾帶的state Querystring不一樣，可能是parameter tampering或CSRF攻擊

                return Content("state驗證失敗");

            }

            if (Convert.ToString(TempData["state"]) == state)
            {//state字串驗證通過

                //取得id_token和access_token:https://developers.line.biz/en/docs/line-login/web/integrate-line-login/#spy-getting-an-access-token
                string issue_token_url = "https://api.line.me/oauth2/v2.1/token";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(issue_token_url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
                NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
                postParams.Add("grant_type", "authorization_code");
                postParams.Add("code", code);
                postParams.Add("redirect_uri", "https://localhost:44384/Home/AfterLineLogin");
                postParams.Add("client_id", "1654660526");
                postParams.Add("client_secret", "7c82a8d2dd3c0fa465e66ae90abaeac6");
                string para = postParams.ToString();

                //要發送的字串轉為byte[] 
                byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }//end using

                //API回傳的字串
                string responseStr = "";
                //發出Request
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            responseStr = sr.ReadToEnd();
                        }//end using  
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }

                LineLoginToken tokenObj = JsonConvert.DeserializeObject<LineLoginToken>(responseStr);
                string id_token = tokenObj.id_token;

                //方案總管>參考>右鍵>管理Nuget套件 搜尋 System.IdentityModel.Tokens.Jwt 來安裝
                var jst = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(id_token);
                LineUserProfile user = new LineUserProfile();
                //↓自行決定要從id_token的Payload中抓什麼user資料
                user.userId = jst.Payload.Sub;
                user.displayName = jst.Payload["name"].ToString();
                user.pictureUrl = jst.Payload["picture"].ToString();
                if (jst.Payload.ContainsKey("email") && !string.IsNullOrEmpty(Convert.ToString(jst.Payload["email"])))
                {//有包含email，使用者有授權email個資存取，並且用戶的email有值
                    user.email = jst.Payload["email"].ToString();
                }


                string access_token = tokenObj.access_token;
                ViewBag.access_token = access_token;
                #region 接下來是為了抓用戶的statusMessage狀態消息，如果你不想要可以省略不發出下面的Request

                //Social API v2.1 Getting user profiles
                //https://developers.line.biz/en/docs/social-api/getting-user-profiles/
                //取回User Profile
                string profile_url = "https://api.line.me/v2/profile";


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(profile_url);
                req.Headers.Add("Authorization", "Bearer " + access_token);
                req.Method = "GET";
                //API回傳的字串
                string resStr = "";
                //發出Request
                using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                    {
                        resStr = sr.ReadToEnd();
                    }//end using  
                }



                LineUserProfile userProfile = JsonConvert.DeserializeObject<LineUserProfile>(resStr);
                user.statusMessage = userProfile.statusMessage;//補上狀態訊息

                #endregion

                //ViewBag.user = JsonConvert.SerializeObject(user, new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //    Formatting = Formatting.Indented
                //});

                var member = db.member
                    .Where(b => b.line_id == user.userId)
                    .FirstOrDefault();
                if (member == null)
                {
                    return RedirectToAction("line_register", "Home", user);
                }
                if ((member != null))
                {
                    TempData["message"] = "login success !";
                    Session["username"] = member.name;

                    Session["member_id"] = member.id;
                    Session["level"] = member.level;
                    Response.Redirect("member");
                }


            }//end if 

            return View();


        }
        public ActionResult line_register()
        {


            return View();
        }
        [HttpPost]
        public ActionResult line_register(FormCollection post)
        {
            var line_id = post["line_id"];
            var sex = post["sex"];
            var name = post["name"];
            var date = post["birthday"];
            var account = post["account"];
            var tel = post["tel"];
            if (line_id != null && account != null)
            {
                var old_member = db.member
                    .Where(b => b.email == account)
                    .FirstOrDefault();
                if (old_member == null)
                {
                    member member = new member();
                    member.c_id = 0;
                    member.c_time = DateTime.Now;
                    member.u_id = 0;
                    member.u_time = DateTime.Now;
                    member.account = account;
                    member.email = account;
                    member.password = line_id;
                    member.name = name;
                    member.birthday = DateTime.ParseExact(date, "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                    member.sex = Convert.ToInt32(sex);
                    //member.tel = tel;
                    member.phone = tel;
                    member.level = 1;
                    member.level_start_time = DateTime.Now;
                    member.level_end_time = DateTime.Now.AddYears(1);
                    member.feedback = 0;
                    member.shopping_gold = 0;
                    member.line_id = line_id;
                    db.member.Add(member);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    Session["member_id"] = member.id;
                    Session["level"] = member.level;

                    Response.Redirect("member");
                }
                else
                {
                    old_member.line_id = line_id;
                    old_member.name = name;
                    old_member.birthday = DateTime.ParseExact(date, "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                    db.Entry(old_member).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    Session["member_id"] = old_member.id;
                    Session["level"] = old_member.level;
                    Response.Redirect("member");
                }


            }




            return View();
        }
        public ActionResult logout()
        {
            Session.RemoveAll();
            return RedirectToAction("index", "Home");
        }

    }
}