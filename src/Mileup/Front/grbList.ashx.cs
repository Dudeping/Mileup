using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MileageCup.Front
{
    /// <summary>
    /// grbList 的摘要说明
    /// </summary>
    public class grbList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            long id = Convert.ToInt64(context.Request["Id"]);
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_grb where Id=@Id", new SqlParameter("@Id", id));
            if(dt.Rows.Count <= 0)
            {
                context.Response.Write("没有找到Id=" + id + "的数据");
            }
            else if(dt.Rows.Count > 1)
            {
                context.Response.Write("找到多条Id=" + id + "的数据");
            }
            else
            {
                context.Response.Write(CommonHelper.RenderHtml("Front/grbList.html", new { Title = "最新消息详细内容", grb = dt.Rows[0], settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
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