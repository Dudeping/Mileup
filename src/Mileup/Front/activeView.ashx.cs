using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MileageCup.Front
{
    /// <summary>
    /// activeView 的摘要说明
    /// </summary>
    public class activeView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            int isRead = 0;
            if (action == "PostComment")
            {
                string msg = context.Request["Msg"];
                long id = Convert.ToInt64(context.Request["Id"]);
                SqlHelper.ExecuteNonQuery("Insert into T_activeComments(activeId, Msg, isRead, CreateTime) values(@activeId, @Msg, @isRead, getdate())",
                    new SqlParameter("@activeId", id),
                    new SqlParameter("@Msg", msg),
                    new SqlParameter("@isRead", isRead));
                context.Response.Write("OK");
            }
            else if (action == "Load")
            {
                long id = Convert.ToInt64(context.Request["Id"]);

                int pageNum = 1;
                if (context.Request["PageNum"] != null)
                {
                    pageNum = Convert.ToInt32(context.Request["PageNum"]);
                }

                DataTable dt = SqlHelper.ExecuteDataTable(@"select * from
                (
                    select *,
                    row_number() over (order by p.Id desc) as num
                    from T_activeComments p where p.activeId=@Id
                ) as s
                where s.num between @Start and @End",
                        new SqlParameter("@Id", id),
                        new SqlParameter("@Start", (pageNum - 1) * 5 + 1),
                        new SqlParameter("@End", pageNum * 5));

                int totalCount = (int)SqlHelper.ExecuteScalar("select count (*) from T_activeComments where activeId=@Id",
                    new SqlParameter("@Id", id));
                int pageCount = (int)Math.Ceiling(totalCount / 5.0);
                
                object[] comments = new object[dt.Rows.Count];
                for (int i = 0; i < comments.Length; i++)
                {
                    DataRow row = dt.Rows[i];
                    DateTime createDT = (DateTime)row["CreateTime"];
                    if (pageNum >= pageCount)
                        comments[i] = new { Msg = row["Msg"], CreateTime = createDT.ToString(), PageNum = 0 };
                    else
                        comments[i] = new { Msg = row["Msg"], CreateTime = createDT.ToString(), PageNum = pageNum + 1 };
                }
                string json = new JavaScriptSerializer().Serialize(comments);
                context.Response.Write(json);
            }
            else
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_active where Id=@Id", new SqlParameter("@Id", id));
                if (dt.Rows.Count <= 0)
                {
                    context.Response.Write("没有找到Id=" + id + "的数据！");
                }
                else if (dt.Rows.Count > 1)
                {
                    context.Response.Write("找到多条Id=" + id + "的数据！");
                }
                else
                {
                    context.Response.Write(CommonHelper.RenderHtml("Front/activeView.html", new { Title = "活动详情", active = dt.Rows[0], settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
                }
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