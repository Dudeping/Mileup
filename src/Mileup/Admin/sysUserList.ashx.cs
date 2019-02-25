using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mileup.Admin
{
    /// <summary>
    /// sysUserList 的摘要说明
    /// </summary>
    public class sysUserList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            string action = context.Request["Action"];
            if (action == "Edit")
            {
                string pwd = context.Request["pwd"];
                string newPwd = context.Request["newPwd"];
                string rePwd = context.Request["rePwd"];
                if (newPwd == "" || newPwd == "" || rePwd == "" || (newPwd != rePwd))
                {
                    context.Response.Redirect("Error.ashx");
                }

                int num = (int)SqlHelper.ExecuteScalar("select count(*) from T_sysUser where password=@pwd", new SqlParameter("@pwd", CommonHelper.GetMD5(CommonHelper.GetMD5(pwd) + "dpp@xx??")));
                if(num != 1)
                {
                    context.Response.Write("原始密码错误！");
                }
                else
                {
                    SqlHelper.ExecuteNonQuery("Update T_sysUser Set password=@pwd",
                        new SqlParameter("@pwd", CommonHelper.GetMD5(CommonHelper.GetMD5(newPwd) +"dpp@xx??")));
                    context.Response.Redirect("sysUserList.ashx");
                }
            }
            else
            {
                context.Response.Write(CommonHelper.RenderHtml("Admin/sysUserList.html", new { Title = "后台管理员列表", msg="", settings = CommonHelper.GetSetting() }));
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