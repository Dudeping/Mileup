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
    /// activeList 的摘要说明
    /// </summary>
    public class activeList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            string action = context.Request["Action"];
            if(action == "Delete")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_active where Id=@Id",
                    new SqlParameter("@Id", id));
                if(dt.Rows.Count <=0)
                {
                    context.Response.Write("找不到Id=" + id + "的活动");
                }
                else if(dt.Rows.Count >1)
                {
                    context.Response.Write("找到多条Id=" + id + "的活动");
                }
                else
                {
                    SqlHelper.ExecuteNonQuery("Delete from T_active where Id=@Id",
                        new SqlParameter("@Id", id));
                    context.Response.Redirect("activeList.ashx");
                }

                context.Response.Redirect("activeList.ashx");
            }
            else
            {
                int pageNum = 1;
                if (context.Request["PageNum"] != null)
                {
                    pageNum = Convert.ToInt32(context.Request["PageNum"]);
                }

                DataTable dt = SqlHelper.ExecuteDataTable(@"select * from
                (
                    select *,
                    row_number() over (order by p.Id desc) as num
                    from T_active p 
                ) as s
                where s.num between @Start and @End",
                        new SqlParameter("@Start", (pageNum - 1) * 10 + 1),
                        new SqlParameter("@End", pageNum * 10));

                int totalCount = (int)SqlHelper.ExecuteScalar("select count (*) from T_active");
                int pageCount = (int)Math.Ceiling(totalCount / 10.0);
                object[] pageData = new object[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    pageData[i] = new { Href = "activeList.ashx?PageNum=" + (i + 1), Title = (i + 1) };
                }
                
                context.Response.Write(CommonHelper.RenderHtml("Admin/activeList.html", new { Title = "活动列表", actives = dt.Rows, Page = new { PageData = pageData, LastPageNum = pageNum - 1, NextPageNum = pageNum + 1, PageNum = pageNum, PageCount = pageCount }, settings = CommonHelper.GetSetting() }));
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