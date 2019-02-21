using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MileageCup.Admin
{
    /// <summary>
    /// Error 的摘要说明
    /// </summary>
    public class Error : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Write(CommonHelper.RenderHtml("Admin/Error.html", new { Title = "出错页面", settings = CommonHelper.GetSetting() }));
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