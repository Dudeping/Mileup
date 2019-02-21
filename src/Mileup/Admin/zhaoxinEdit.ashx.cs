using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MileageCup.Admin
{
    /// <summary>
    /// zhaoxinEdit 的摘要说明
    /// </summary>
    public class zhaoxinEdit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            string action = context.Request["Action"];
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            if(isSave)
            {
                if (action == "Add")
                {
                    string name = context.Request["Name"];
                    string post = context.Request["post"];
                    string num = context.Request["num"];
                    string place = context.Request["place"];
                    string time = context.Request["time"];
                    string linkMan = context.Request["linkMan"];
                    string contact = context.Request["contact"];
                    string msg = context.Request["Msg"];
                    string data = context.Request["data"];
                    string note = context.Request["note"];
                    if (name == "" || post == "" || num == "" || place == "" || time == "" || linkMan == "" || contact == "" || msg == "")
                        context.Response.Redirect("Error.ashx");
                    int createTime = DateTime.Now.Year;
                    SqlHelper.ExecuteNonQuery("Insert into T_zhaoxin(Name, post, num, place, time, linkMan, contact, Msg, data, note, createTime) values(@Name, @post, @num, @place, @time, @linkMan, @contact, @Msg, @data, @note, @createTime)",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@post", post),
                        new SqlParameter("@num", num),
                        new SqlParameter("@place", place),
                        new SqlParameter("@time", time),
                        new SqlParameter("@linkMan", linkMan),
                        new SqlParameter("@contact", contact),
                        new SqlParameter("@Msg", msg),
                        new SqlParameter("@note", note),
                        new SqlParameter("@data", data),
                        new SqlParameter("@createTime", createTime));

                    context.Response.Redirect("zhaoxinList.ashx");
                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    string name = context.Request["Name"];
                    string post = context.Request["post"];
                    string num = context.Request["num"];
                    string place = context.Request["place"];
                    string time = context.Request["time"];
                    string linkMan = context.Request["linkMan"];
                    string contact = context.Request["contact"];
                    string msg = context.Request["Msg"];
                    string data = context.Request["data"];
                    string note = context.Request["note"];
                    if (name == "" || post == "" || num == "" || place == "" || time == "" || linkMan == "" || contact == "" || msg == "")
                        context.Response.Redirect("Error.ashx");
                    int createTime = DateTime.Now.Year;
                    SqlHelper.ExecuteNonQuery("Update T_zhaoxin Set Name=@Name, post=@post, num=@num, place=@place, time=@time, linkMan=@linkMan, contact=@contact, Msg=@Msg, data=@data, note=@note, createTime=@createTime where Id=@Id",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@post", post),
                        new SqlParameter("@num", num),
                        new SqlParameter("@place", place),
                        new SqlParameter("@time", time),
                        new SqlParameter("@linkMan", linkMan),
                        new SqlParameter("@contact", contact),
                        new SqlParameter("@Msg", msg),
                        new SqlParameter("@note", note),
                        new SqlParameter("@data", data),
                        new SqlParameter("@createTime", createTime),
                        new SqlParameter("@Id", id));

                    context.Response.Redirect("zhaoxinList.ashx");
                }
                else
                {
                    context.Response.Write("参数错误！");
                }
            }
            else
            {
                if(action == "Add")
                {
                    context.Response.Write(CommonHelper.RenderHtml("Admin/zhaoxinEdit.html", new { Title = "编辑招新栏目", Action = "Add", zhaoxin = new { Name = "", post = "", num = "", place = "", time = "", linkMan = "", contact = "", Msg = "", data = "", note = "" }, settings = CommonHelper.GetSetting() }));
                }
                else if(action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_zhaoxin where Id=@Id", new SqlParameter("@Id", id));
                    if(dt.Rows.Count <= 0)
                    {
                        context.Response.Write("没有找到Id=" + id + "的数据!");
                    }
                    else if(dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的数据!");
                    }
                    else
                    {
                        context.Response.Write(CommonHelper.RenderHtml("Admin/zhaoxinEdit.html", new { Title = "编辑招新栏目", Action = "Edit", zhaoxin = dt.Rows[0], settings = CommonHelper.GetSetting() }));
                    }
                }
                else if(action == "Delete")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_zhaoxin where Id=@Id", new SqlParameter("@Id", id));
                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("没有找到Id=" + id + "的数据!");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的数据!");
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery("Delete from T_zhaoxin where Id=@Id", new SqlParameter("@Id", id));
                        context.Response.Redirect("zhaoxinList.ashx");
                    }
                }
                else
                {
                    context.Response.Write("参数错误！");
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