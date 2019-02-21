using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MileageCup.Admin
{
    /// <summary>
    /// grbEdit 的摘要说明
    /// </summary>
    public class grbEdit : IHttpHandler, IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            string action = context.Request["Action"];
            if(isSave)
            {
                if (action == "Add")
                {
                    string name =  context.Request["Name"];
                    string msg = context.Request["Msg"];
                    HttpPostedFile pic = context.Request.Files["pic"];

                    if (name == "" || msg == "" || (CommonHelper.HasFile(pic) == false))
                        context.Response.Redirect("Error.ashx");
                    long id = Convert.ToInt64(SqlHelper.ExecuteScalar("Insert into T_grb(Name, Msg, createTime) values(@Name, @Msg, getdate()) select @@identity",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@Msg", msg)));

                    if (CommonHelper.HasFile(pic))
                    {
                        string picName = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic.FileName);
                        pic.SaveAs(context.Server.MapPath("~/uploadfile/" + picName));
                        SqlHelper.ExecuteNonQuery("Update T_grb Set pic=@pic where Id=@Id",
                            new SqlParameter("@pic", "/uploadfile/" + picName),
                            new SqlParameter("@Id", id));
                    }
                    context.Response.Redirect("grbList.ashx");
                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    string name = context.Request["Name"];
                    string msg = context.Request["Msg"];
                    HttpPostedFile pic = context.Request.Files["pic"];
                    if (name == "" || msg == "")
                        context.Response.Redirect("Error.ashx");

                    SqlHelper.ExecuteScalar("Update T_grb Set Name=@Name, Msg=@Msg where Id=@Id",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@Msg", msg),
                        new SqlParameter("@Id", id));

                    if (CommonHelper.HasFile(pic))
                    {
                        string picName = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic.FileName);
                        pic.SaveAs(context.Server.MapPath("~/uploadfile/" + picName));
                        SqlHelper.ExecuteNonQuery("Update T_grb Set pic=@pic where Id=@Id",
                            new SqlParameter("@pic", "/uploadfile/" + picName),
                            new SqlParameter("@Id", id));
                    }
                    context.Response.Redirect("grbList.ashx");
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
                    context.Response.Write(CommonHelper.RenderHtml("Admin/grbEdit.html", new { Title = "添加最新消息",Action = "Add", grb = new { Name = "", pic = "", Msg = "" }, settings = CommonHelper.GetSetting() }));
                }
                else if(action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_grb where Id=@Id", new SqlParameter("@Id", id));
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
                        context.Response.Write(CommonHelper.RenderHtml("Admin/grbEdit.html", new { Title = "编辑最新消息", Action = "Edit", grb = dt.Rows[0], settings = CommonHelper.GetSetting() }));
                    }
                }
                else if(action == "Delete")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_grb where Id=@Id", new SqlParameter("@Id", id));
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
                        SqlHelper.ExecuteNonQuery("Delete from T_grb where Id=@Id", new SqlParameter("@Id", id));
                        context.Response.Redirect("grbList.ashx");
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