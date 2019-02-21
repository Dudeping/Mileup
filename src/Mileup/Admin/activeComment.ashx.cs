using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace MileageCup.Admin
{
    /// <summary>
    /// activeComment 的摘要说明
    /// </summary>
    public class activeComment : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            string action = context.Request["Action"];
            if(action == "Delete")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_activeComments where Id=@Id", 
                    new SqlParameter("@Id", id));
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
                    SqlHelper.ExecuteNonQuery("Delete from T_activeComments where Id=@Id", new SqlParameter("@Id", id));
                    context.Response.Redirect("activeComment.ashx");
                }
            }
            else if (action == "Label")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                SqlHelper.ExecuteNonQuery("Update T_activeComments Set isRead=@isRead where Id=@Id",
                    new SqlParameter("@isRead", 1),
                    new SqlParameter("@Id", id));

                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_activeComments where Id=@Id", 
                    new SqlParameter("@Id", id));
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
                    from T_activeComments p 
                ) as s
                where s.num between @Start and @End",
                        new SqlParameter("@Start", (pageNum - 1) * 5 + 1),
                        new SqlParameter("@End", pageNum * 5));

                int totalCount = (int)SqlHelper.ExecuteScalar("select count (*) from T_activeComments");
                int pageCount = (int)Math.Ceiling(totalCount / 5.0);
                object[] pageData = new object[pageCount];
                for (int i = 0; i < pageCount; i++)
                {
                    pageData[i] = new { Href = "activeComment.ashx?PageNum=" + (i + 1), Title = (i + 1) };
                }

                DataTable active = SqlHelper.ExecuteDataTable("select * from T_active");
                context.Response.Write(CommonHelper.RenderHtml("Admin /activeComment.html", new { Title = "活动评论列表", Comments = dt.Rows, actives = active.Rows, Page = new { PageData = pageData, LastPageNum = pageNum - 1, NextPageNum = pageNum + 1, PageNum = pageNum, PageCount = pageCount }, settings = CommonHelper.GetSetting() }));
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