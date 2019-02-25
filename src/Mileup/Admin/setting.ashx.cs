using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mileup.Admin
{
    /// <summary>
    /// setting 的摘要说明
    /// </summary>
    public class setting : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            if(isSave)
            {
                HttpPostedFile huihui_pic = context.Request.Files["huihui_pic"];
                
                if(context.Request["CN_Name"] != "")
                    CommonHelper.WriteSetting("CN_Name", context.Request["CN_Name"]);
                if (context.Request["EN_Name"] != "")
                    CommonHelper.WriteSetting("EN_Name", context.Request["EN_Name"]);
                if (context.Request["Address"] != "")
                    CommonHelper.WriteSetting("Address", context.Request["Address"]);
                if (context.Request["postCode"] != "")
                    CommonHelper.WriteSetting("postCode", context.Request["postCode"]);
                if (context.Request["linkMan"] != "")
                    CommonHelper.WriteSetting("linkMan", context.Request["linkMan"]);
                if (context.Request["tel"] != "")
                    CommonHelper.WriteSetting("tel", context.Request["tel"]);
                if (context.Request["QQ"] != "")
                    CommonHelper.WriteSetting("QQ", context.Request["QQ"]);
                if (context.Request["Email"] != "")
                    CommonHelper.WriteSetting("Email", context.Request["Email"]);
                if (context.Request["weChat"] != "" || context.Request["QQ_qun"] != "" || context.Request["microBlog"] != "")
                {
                    CommonHelper.WriteSetting("weChat", context.Request["weChat"]);
                    CommonHelper.WriteSetting("QQ_qun", context.Request["QQ_qun"]);
                    CommonHelper.WriteSetting("microBlog", context.Request["microBlog"]);
                }
                if (context.Request["phone"] != "")
                    CommonHelper.WriteSetting("phone", context.Request["phone"]);
                CommonHelper.WriteSetting("guestbook", context.Request["guestbook"]);

                if (CommonHelper.HasFile(huihui_pic))
                {
                    string huihuiName = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(huihui_pic.FileName);
                    huihui_pic.SaveAs(context.Server.MapPath("~/uploadfile/" + huihuiName));
                    CommonHelper.WriteSetting("huihui_pic", "/uploadfile/" + huihuiName);
                }

                context.Response.Redirect("setting.ashx");
            }
            else
            {
                context.Response.Write(CommonHelper.RenderHtml("Admin/setting.html", new { Title = "网站配置", setting = CommonHelper.GetSetting(), settings = CommonHelper.GetSetting() }));
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