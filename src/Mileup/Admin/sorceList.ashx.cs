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
    /// sorceList 的摘要说明
    /// </summary>
    public class sorceList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            int pageNum = 1;
            if (context.Request["PageNum"] != null)
            {
                pageNum = Convert.ToInt32(context.Request["PageNum"]);
            }

            DataTable dt = SqlHelper.ExecuteDataTable(@"select * from
                (
                    select *,
                    row_number() over (order by p.Id desc) as num
                    from T_sorce p 
                ) as s
                where s.num between @Start and @End",
                    new SqlParameter("@Start", (pageNum - 1) * 10 + 1),
                    new SqlParameter("@End", pageNum * 10));

            int totalCount = (int)SqlHelper.ExecuteScalar("select count (*) from T_sorce");
            int pageCount = (int)Math.Ceiling(totalCount / 10.0);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "linkLiat.ashx?PageNum=" + (i + 1), Title = (i + 1) };
            }
            
            context.Response.Write(CommonHelper.RenderHtml("Admin/sorceList.html", new { Title = "资源列表", sorces = dt.Rows, Page = new { PageData = pageData, LastPageNum = pageNum - 1, NextPageNum = pageNum + 1, PageNum = pageNum, PageCount = pageCount }, settings = CommonHelper.GetSetting() }));
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