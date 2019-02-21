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
    /// guestbook 的摘要说明
    /// </summary>
    public class guestbook : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            if(action == "Add")
            {
                int isRead = 0;
                string name;
                if (context.Request["niming"] == "on")
                    name = "匿名网友";
                else
                    name = context.Request["Name"];
                
                string email = context.Request["Email"];
                string phone = context.Request["phone"];
                string qq = context.Request["QQ"];
                string college = context.Request["College"];
                string major = context.Request["major"];
                string message = context.Request["Message"];

                if(name == "" || message == "" || (email == "" && phone == "" && qq == ""))
                    context.Response.Write("数据错误！");

                SqlHelper.ExecuteNonQuery("Insert into T_guestbook(Name, Email, isRead, phone, QQ, College, major, Message, CreateTime) values (@Name, @Email, @isRead, @phone, @QQ, @College, @major, @Message, getdate())",
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@isRead", isRead),
                    new SqlParameter("@phone", phone),
                    new SqlParameter("@QQ", qq),
                    new SqlParameter("@College", college),
                    new SqlParameter("@major", major),
                    new SqlParameter("@Message", message));

                context.Response.Redirect("guestbook.ashx");
            }
            else if(action == "Load")
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
                        new SqlParameter("@Start", (pageNum - 1) * 6 + 1),
                        new SqlParameter("@End", pageNum * 6));

                int totalCount = (int)SqlHelper.ExecuteScalar("select count (*) from T_guestbook");
                int pageCount = (int)Math.Ceiling(totalCount / 6.0);

                object[] comments = new object[dt.Rows.Count];
                for (int i = 0; i < comments.Length; i++)
                {
                    DataRow row = dt.Rows[i];
                    DateTime createDT = (DateTime)row["CreateTime"];
                    if (pageNum >= pageCount)
                        comments[i] = new { Msg = row["Message"], CreateTime = createDT.ToString(), Name = row["Name"], PageNum = 0 };
                    else
                        comments[i] = new { Msg = row["Message"], CreateTime = createDT.ToString(), Name = row["Name"], PageNum = pageNum + 1 };
                }
                string json = new JavaScriptSerializer().Serialize(comments);
                context.Response.Write(json);
            }
            else
            {
                context.Response.Write(CommonHelper.RenderHtml("Front/guestbook.html", new { Title = "留言板", Msg = CommonHelper.ReadSetting("guestbook"), settings = CommonHelper.GetSetting(), links = CommonHelper.readLink() }));
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