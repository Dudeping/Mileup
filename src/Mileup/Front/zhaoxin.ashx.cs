using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MileageCup.Front
{
    /// <summary>
    /// zhaoxin 的摘要说明
    /// </summary>
    public class zhaoxin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string msg;
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_zhaoxin where createTime=@createTime", 
                new SqlParameter("@createTime", DateTime.Now.Year));
            DataTable dt_Msg = SqlHelper.ExecuteDataTable("select * from T_department");
            if (dt_Msg.Rows.Count == 1)
            {
                msg = dt_Msg.Rows[0]["Msg"].ToString();
            }
            else
            {
                msg = "加载数据出错！";
            }
            context.Response.Write(CommonHelper.RenderHtml("Front/zhaoxin.html", new { Title = "招新", zhaoxins = dt.Rows, Msg = dt_Msg.Rows[0]["Msg"], settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
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