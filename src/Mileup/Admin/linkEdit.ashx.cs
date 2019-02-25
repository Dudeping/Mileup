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
    /// linkEdit 的摘要说明
    /// </summary>
    public class linkEdit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            if(isSave)
            {
                string action = context.Request["Action"];
                if (action == "Add")
                {
                    string linkName = context.Request["linkName"];
                    string link = context.Request["link"];
                    if (linkName == "" || link == "")
                        context.Response.Redirect("Error.ashx");
                    SqlHelper.ExecuteNonQuery("Insert into T_link(linkName, link) Values(@linkName, @link)",
                        new SqlParameter("@linkName", linkName),
                        new SqlParameter("@link", link));

                    context.Response.Redirect("/Admin/linkList.ashx");
                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_link where Id=@Id", new SqlParameter("@Id", id));
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
                        string linkName = context.Request["linkName"];
                        string link = context.Request["link"];
                        if (linkName == "" || link == "")
                            context.Response.Redirect("Error.ashx");
                        SqlHelper.ExecuteNonQuery("Update T_link set linkName=@linkName, link=@link where Id=@Id",
                            new SqlParameter("@linkName", linkName),
                            new SqlParameter("@link", link),
                            new SqlParameter("@Id", id));
                        context.Response.Redirect("/Admin/linkList.ashx");
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
                    context.Response.Write(CommonHelper.RenderHtml("Admin/linkEdit.html", new { Title = "添加友情链接", Action = action, links = new { linkName = "", link = "" }, settings = CommonHelper.GetSetting() }));
                }
                else if(action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_link where Id=@Id", new SqlParameter("@Id", id));
                    
                    if(dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到Id=" + id + "的数据！");
                    }
                    else if(dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的数据！");
                    }
                    else
                    {
                        context.Response.Write(CommonHelper.RenderHtml("Admin/linkEdit.html", new { Title = "编辑友情链接", Action = action, links = dt.Rows[0], settings = CommonHelper.GetSetting() }));
                    }
                }
                else if(action == "Delete")
                {
                    string id = context.Request["Id"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_link where Id=@Id", new SqlParameter("@Id", id));

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
                        SqlHelper.ExecuteNonQuery("Delete from T_link where Id=@Id", new SqlParameter("@Id", id));
                        context.Response.Redirect("/Admin/linkList.ashx");
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