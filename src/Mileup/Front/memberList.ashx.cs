using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MileageCup.Front
{
    /// <summary>
    /// memberList 的摘要说明
    /// </summary>
    public class memberList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Write(CommonHelper.RenderHtml("Front/memberList.html", new { Title = "成员详细信息", settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
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