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
    /// linkList 的摘要说明
    /// </summary>
    public class linkList : IHttpHandler, IRequiresSessionState
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
                    from T_link p 
                ) as s
                where s.num between @Start and @End",
                    new SqlParameter("@Start", (pageNum - 1) * 12 + 1),
                    new SqlParameter("@End", pageNum * 12));

            int totalCount = (int)SqlHelper.ExecuteScalar("select count (*) from T_link");
            int pageCount = (int)Math.Ceiling(totalCount / 12.0);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "linkLiat.ashx?PageNum=" + (i + 1), Title = (i + 1) };
            }
            
            context.Response.Write(CommonHelper.RenderHtml("Admin/linkList.html", new { Title = "友情链接", links = dt.Rows, Page = new { PageData = pageData, LastPageNum = pageNum - 1, NextPageNum = pageNum + 1, PageNum = pageNum, PageCount = pageCount }, settings = CommonHelper.GetSetting() }));
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