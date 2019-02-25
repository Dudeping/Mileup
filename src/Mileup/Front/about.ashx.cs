using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Mileup.Front
{
    /// <summary>
    /// about 的摘要说明
    /// </summary>
    public class about : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            if(action == "jiagou")
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_department");
                context.Response.Write(CommonHelper.RenderHtml("Front/jiagou.html", new { Title = "社团架构", jiagous = dt.Rows, settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
            }
            else if(action == "history")
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_department where isHui=1");
                context.Response.Write(CommonHelper.RenderHtml("Front/history.html", new { Title = "社团历史", historys = dt.Rows[0], settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
            }
            else if(action == "gaikuang")
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_department");
                context.Response.Write(CommonHelper.RenderHtml("Front/gaikuang.html", new { Title = "社团概况", gaikuangs = dt.Rows, settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
            }
            else
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_department where isHui=1");
                context.Response.Write(CommonHelper.RenderHtml("Front/about.html", new { Title = "关于我们", abouts = dt.Rows[0], settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
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