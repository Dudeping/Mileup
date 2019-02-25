using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace Mileup.Admin
{
    /// <summary>
    /// guestbookList 的摘要说明
    /// </summary>
    public class guestbookList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            string action = context.Request["Action"];
            if(action == "Delete")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_guestbook where Id=@Id", new SqlParameter("@Id", id));
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
                    SqlHelper.ExecuteNonQuery("Delete from T_guestbook where Id=@Id", new SqlParameter("@Id", id));
                    context.Response.Redirect("guestbookList.ashx");
                }
            }
            else if(action == "Label")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                SqlHelper.ExecuteNonQuery("Update T_guestbook Set isRead=@isRead where Id=@Id",
                    new SqlParameter("@isRead", 1),
                    new SqlParameter("@Id", id));

                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_guestbook where Id=@Id", new SqlParameter("@Id", id));
                object[] comments = new object[1];
                DataRow row = dt.Rows[0];
                comments[0] = new { isRead = row["isRead"] };
                string json = new JavaScriptSerializer().Serialize(comments);
                context.Response.Write(json);
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
                    from T_guestbook p 
                ) as s
                where s.num between @Start and @End",
                        new SqlParameter("@Start", (pageNum - 1) * 5 + 1),
                        new SqlParameter("@End", pageNum * 5));

                int totalCount = (int)SqlHelper.ExecuteScalar("select count (*) from T_guestbook");
                int pageCount = (int)Math.Ceiling(totalCount / 5.0);
                object[] pageData = new object[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    pageData[i] = new { Href = "guestbookList.ashx?PageNum=" + (i + 1), Title = (i + 1) };
                }
                
                context.Response.Write(CommonHelper.RenderHtml("Admin/guestbookList.html", new { Title = "留言板列表", guestbooks = dt.Rows, Page = new { PageData = pageData, LastPageNum = pageNum - 1, NextPageNum = pageNum + 1, PageNum = pageNum, PageCount = pageCount }, settings = CommonHelper.GetSetting() }));
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