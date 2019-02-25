using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mileup.Admin
{
    /// <summary>
    /// zhaoxinList 的摘要说明
    /// </summary>
    public class zhaoxinList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_zhaoxin where createTime=@time", new System.Data.SqlClient.SqlParameter("@time", DateTime.Now.Year));
            context.Response.Write(CommonHelper.RenderHtml("Admin/zhaoxinList.html", new { Title = "招新模块列表", zhaoxin = dt.Rows, settings = CommonHelper.GetSetting() }));
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