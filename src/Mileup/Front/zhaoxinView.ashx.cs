using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Mileup.Front
{
    /// <summary>
    /// zhaoxinView 的摘要说明
    /// </summary>
    public class zhaoxinView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            long id = Convert.ToInt64(context.Request["Id"]);
            string name = context.Request["Name"];
            string msg;
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_zhaoxin where Id=@Id",
                new SqlParameter("@Id", id));
            DataTable dt_Msg = SqlHelper.ExecuteDataTable("select * from T_department where Name=@Name",
                new SqlParameter("@Name", name));
            if(dt_Msg.Rows.Count == 1)
            {
                msg = dt_Msg.Rows[0]["Msg"].ToString();
            }
            else
            {
                msg = "加载数据出错！";
            }
            if(dt.Rows.Count <= 0)
            {
                context.Response.Write("没有找到Id=" + id + "的数据！");
            }
            else if(dt.Rows.Count > 1)
            {
                context.Response.Write("找到多条Id=" + id + "的数据！");
            }
            else
            {
                context.Response.Write(CommonHelper.RenderHtml("Front/zhaoxinView.html", new { Title = "招新详细信息", zhaoxin = dt.Rows[0], Msg = msg, settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
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