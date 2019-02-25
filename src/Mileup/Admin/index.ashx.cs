using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mileup.Admin
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            if(isSave)
            {
                string userName = context.Request["userName"];
                string password = context.Request["password"];
                int count = (int)SqlHelper.ExecuteScalar("select count(*) from T_sysUser where userName=@UserName and password = @Password",
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@Password", CommonHelper.GetMD5(CommonHelper.GetMD5(password) + "dpp@xx??")));
                if(count == 1)
                {
                    if(context.Request["memory"] == "on")
                    {
                        HttpCookie cookieuser = new HttpCookie("userName", userName);
                        cookieuser.Expires = DateTime.Now.AddDays(9999);
                        context.Response.SetCookie(cookieuser);

                        HttpCookie cookiepwd = new HttpCookie("password", password);
                        cookiepwd.Expires = DateTime.Now.AddDays(9999);
                        context.Response.SetCookie(cookiepwd);
                    }
                    context.Session["LoginUserPassWord"] = CommonHelper.GetMD5(CommonHelper.GetMD5(password) + "dpp@xx??");
                    context.Response.Redirect("index.ashx");
                }
                else
                {
                    context.Response.Write("用户名或密码错误！");
                }
            }
            else
            {
                string pwd = (string)context.Session["LoginUserPassWord"];
                if (!String.IsNullOrEmpty(pwd))
                {
                    int num = (int)SqlHelper.ExecuteScalar("select count(*) from T_sysUser where password=@pwd", new SqlParameter("@pwd", pwd));
                    if (num != 1)
                    {
                        string userName, password;
                        HttpCookie cookieuser = context.Request.Cookies["userName"];
                        if (cookieuser != null)
                        {
                            userName = cookieuser.Value;
                        }
                        else
                        {
                            userName = "";
                        }
                        HttpCookie cookiepwd = context.Request.Cookies["password"];
                        if (cookiepwd != null)
                        {
                            password = cookiepwd.Value;
                        }
                        else
                        {
                            password = "";
                        }
                        context.Response.Write(CommonHelper.RenderHtml("Admin/index.html", new { Title = "后台主页", userName = userName, password = password, isLogin = 0, settings = CommonHelper.GetSetting() }));
                    }
                    else
                    {
                        int isRead = 0;
                        int guestbooks = (int)SqlHelper.ExecuteScalar("select count(*) from T_guestbook where isRead=@isRead",
                            new SqlParameter("@isRead", isRead));
                        int activeComments = (int)SqlHelper.ExecuteScalar("select count(*) from T_activeComments where isRead=@isRead",
                            new SqlParameter("@isRead", isRead));
                        context.Response.Write(CommonHelper.RenderHtml("Admin/index.html", new { Title = "后台主页", guestbooks = guestbooks, Comments = activeComments, isLogin = 1, settings = CommonHelper.GetSetting() }));
                    }
                }
                else
                {
                    string userName, password;
                    HttpCookie cookieuser = context.Request.Cookies["userName"];
                    if (cookieuser != null)
                    {
                        userName = cookieuser.Value;
                    }
                    else
                    {
                        userName = "";
                    }
                    HttpCookie cookiepwd = context.Request.Cookies["password"];
                    if (cookiepwd != null)
                    {
                        password = cookiepwd.Value;
                    }
                    else
                    {
                        password = "";
                    }
                    context.Response.Write(CommonHelper.RenderHtml("Admin/index.html", new { Title = "后台主页", userName = userName, password = password, isLogin = 0, settings = CommonHelper.GetSetting() }));
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}