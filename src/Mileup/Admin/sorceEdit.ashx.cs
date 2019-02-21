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
    /// sorceEdit 的摘要说明
    /// </summary>
    public class sorceEdit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            if (isSave)
            {
                string action = context.Request["Action"];
                if (action == "Add")
                {
                    string sorceName = context.Request["sorceName"];
                    string sorce = context.Request["sorce"];
                    if (sorceName == "" || sorce == "")
                        context.Response.Redirect("Error.ashx");

                    string sorceNote = context.Request["sorceNote"];
                    SqlHelper.ExecuteNonQuery("Insert into T_sorce(sorceName, sorceLink, sorceNote, createTime) Values(@sorceName, @sorceLink, @sorceNote, getdate())",
                        new SqlParameter("@sorceName", sorceName),
                        new SqlParameter("@sorceLink", sorce),
                        new SqlParameter("@sorceNote", sorceNote));

                    context.Response.Redirect("/Admin/sorceList.ashx");
                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_sorce where Id=@Id", new SqlParameter("@Id", id));
                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到Id=" + id + "的数据！");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的数据！");
                    }
                    else
                    {
                        string sorceName = context.Request["sorceName"];
                        string sorce = context.Request["sorce"];
                        if (sorceName == "" || sorce == "")
                            context.Response.Redirect("Error.ashx");
                        string sorceNote = context.Request["sorceNote"];
                        SqlHelper.ExecuteNonQuery("Update T_sorce set sorceName=@sorceName, sorceLink=@sorceLink, sorceNote=@sorceNote where Id=@Id",
                            new SqlParameter("@sorceName", sorceName),
                            new SqlParameter("@sorceLink", sorce),
                            new SqlParameter("@sorceNote", sorceNote),
                            new SqlParameter("@Id", id));
                        context.Response.Redirect("/Admin/sorceList.ashx");
                    }
                }
                else
                {
                    context.Response.Write("参数错误!!!");
                }
            }
            else
            {
                string action = context.Request["Action"];
                if (action == "Add")
                {
                    context.Response.Write(CommonHelper.RenderHtml("Admin/sorceEdit.html", new { Title = "添加资源", Action = action, sorces = new { sorceName = "", sorceLink = "", sorceNote = "" }, settings = CommonHelper.GetSetting() }));
                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_sorce where Id=@Id", new SqlParameter("@Id", id));

                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到Id=" + id + "的数据！");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的数据！");
                    }
                    else
                    {
                        context.Response.Write(CommonHelper.RenderHtml("Admin/sorceEdit.html", new { Title = "编辑资源", Action = action, sorces = dt.Rows[0], settings = CommonHelper.GetSetting() }));
                    }
                }
                else if(action == "Delete")
                {
                    string id = context.Request["Id"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_sorce where Id=@Id", new SqlParameter("@Id", id));

                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到Id=" + id + "的数据！");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的数据！");
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery("Delete from T_sorce where Id=@Id", new SqlParameter("@Id", id));
                        context.Response.Redirect("/Admin/sorceList.ashx");
                    }
                }
                else
                {
                    context.Response.Write("参数错误!!!");
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