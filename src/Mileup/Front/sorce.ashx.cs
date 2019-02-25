using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Mileup.Front
{
    /// <summary>
    /// sorce 的摘要说明
    /// </summary>
    public class sorce : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
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
                pageData[i] = new { Href = "sorce.ashx?PageNum=" + (i + 1), Title = (i + 1) };
            }
            context.Response.Write(CommonHelper.RenderHtml("Front/sorce.html", new { Title = "资源下载", sorces = dt.Rows, settings = CommonHelper.GetSetting(), links = CommonHelper.readLink(), Page = new { PageData = pageData, LastPageNum = pageNum - 1, NextPageNum = pageNum + 1, PageNum = pageNum, PageCount = pageCount } }));
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