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
    /// aboutList 的摘要说明
    /// </summary>
    public class aboutList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            string action = context.Request["Action"];
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            if(isSave)
            {
                if(action == "Add")
                {
                    int i = 0;
                    string name_1 = context.Request["Name_1"];
                    string name_2 = context.Request["Name_2"];
                    string name_3 = context.Request["Name_3"];
                    string name_4 = context.Request["Name_4"];
                    string name_5 = context.Request["Name_5"];
                    string name_6 = context.Request["Name_6"];

                    if(name_1 == "" || name_2 == "" || name_3 == "" || name_4 == "" || name_5 == "" || name_6 == "" )
                    {
                        context.Response.Redirect("Error.ashx");
                    }

                    SqlHelper.ExecuteNonQuery("Insert into T_department(Name, isHui, isEdit) Values(@Name, @isHui, @isEdit)",
                        new SqlParameter("@Name", name_1),
                        new SqlParameter("@isHui", 1),
                        new SqlParameter("@isEdit", 1));
                    SqlHelper.ExecuteNonQuery("Insert into T_department(Name, isHui) Values(@Name, @isHui)",
                        new SqlParameter("@Name", name_2),
                        new SqlParameter("@isHui", i));
                    SqlHelper.ExecuteNonQuery("Insert into T_department(Name, isHui) Values(@Name, @isHui)",
                        new SqlParameter("@Name", name_3),
                        new SqlParameter("@isHui", i));
                    SqlHelper.ExecuteNonQuery("Insert into T_department(Name, isHui) Values(@Name, @isHui)",
                        new SqlParameter("@Name", name_4),
                        new SqlParameter("@isHui", i));
                    SqlHelper.ExecuteNonQuery("Insert into T_department(Name, isHui) Values(@Name, @isHui)",
                        new SqlParameter("@Name", name_5),
                        new SqlParameter("@isHui", i));
                    SqlHelper.ExecuteNonQuery("Insert into T_department(Name, isHui) Values(@Name, @isHui)",
                        new SqlParameter("@Name", name_6),
                        new SqlParameter("@isHui", i));

                    context.Response.Redirect("aboutList.ashx");

                }
                else if(action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_department where Id=@Id", new SqlParameter("@Id", id));
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
                        if (Convert.ToInt32(dt.Rows[0]["isHui"]) == 1)
                        {
                            string name = context.Request["Name"];
                            string member_hui = context.Request["member_hui"];
                            string member_fu = context.Request["member_fu"];
                            string msg = context.Request["Msg"];
                            string history = context.Request["history"];
                            string zhangcheng = context.Request["zhangcheng"];
                            string zhZh = context.Request["zhZh"];
                            HttpPostedFile pic = context.Request.Files["pic"];
                            if (name == "" || member_hui == "" || member_fu == "" || msg == "" || history == "" || zhangcheng == "" || zhZh == "")
                                context.Response.Redirect("Error.ashx");

                            SqlHelper.ExecuteNonQuery("Update T_department Set Name=@Name, member_hui=@member_hui, member_fu=@member_fu, Msg=@Msg, history=@history, zhangcheng=@zhangcheng, zhZh=@zhZh where Id=@Id",
                                new SqlParameter("@Name", name),
                                new SqlParameter("@member_hui", member_hui),
                                new SqlParameter("@member_fu", member_fu),
                                new SqlParameter("@Msg", msg),
                                new SqlParameter("@history", history),
                                new SqlParameter("@zhangcheng", zhangcheng),
                                new SqlParameter("@zhZh", zhZh),
                                new SqlParameter("@Id", id));
                            if (CommonHelper.HasFile(pic))
                            {
                                string picName = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic.FileName);
                                pic.SaveAs(context.Server.MapPath("~/uploadfile/" + picName));
                                SqlHelper.ExecuteNonQuery("Update T_department Set pic=@pic where Id=@Id",
                                    new SqlParameter("@pic", "/uploadfile/" + picName),
                                    new SqlParameter("@Id", id));
                            }

                            context.Response.Redirect("aboutList.ashx");
                        }
                        else
                        {
                            string name = context.Request["Name"];
                            string member_hui = context.Request["member_hui"];
                            string member_fu = context.Request["member_fu"];
                            string member_wei = context.Request["member_wei"];
                            string member_gan = context.Request["member_gan"];
                            string msg = context.Request["Msg"];
                            string zhZh = context.Request["zhZh"];
                            HttpPostedFile pic = context.Request.Files["pic"];
                            if (name == "" || member_hui == "" || member_fu == "" || msg == "" || member_wei == "" || member_gan == "" || zhZh == "")
                                context.Response.Redirect("Error.ashx");
                            SqlHelper.ExecuteNonQuery("Update T_department Set Name=@Name, member_hui=@member_hui, member_wei=@member_wei, member_gan=@member_gan, member_fu=@member_fu, Msg=@Msg, zhZh=@zhZh where Id=@Id",
                                new SqlParameter("@Name", name),
                                new SqlParameter("@member_hui", member_hui),
                                new SqlParameter("@member_fu", member_fu),
                                new SqlParameter("@member_wei", member_wei),
                                new SqlParameter("@member_gan", member_gan),
                                new SqlParameter("@Msg", msg),
                                new SqlParameter("@zhZh", zhZh),
                                new SqlParameter("@Id", id));
                            if (CommonHelper.HasFile(pic))
                            {
                                string picName = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic.FileName);
                                pic.SaveAs(context.Server.MapPath("~/uploadfile/" + picName));
                                SqlHelper.ExecuteNonQuery("Update T_department Set pic=@pic where Id=@Id",
                                    new SqlParameter("@pic", "/uploadfile/" + picName),
                                    new SqlParameter("@Id", id));
                            }

                            context.Response.Redirect("aboutList.ashx");
                        }
                    }
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
                    DataTable dt = SqlHelper.ExecuteDataTable("select value from T_Setting where Name=@Name", new SqlParameter("@Name", "CN_Name"));
                    DataTable dt_1 = SqlHelper.ExecuteDataTable("select * from T_department");
                    if(Convert.ToInt32(dt_1.Rows[0]["isEdit"]) == 1)
                    {
                        context.Response.Write("给你说了不能重复修改啊！");
                    }
                    else
                    {
                        context.Response.Write(CommonHelper.RenderHtml("Admin/aboutEdit.html", new { Title = "完善社团信息", Action = "Add", Name = dt.Rows[0]["Value"], settings = CommonHelper.GetSetting() }));
                    }
                }
                else if(action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_department where Id=@Id", 
                        new SqlParameter("@Id", id));
                    if(dt.Rows.Count >1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的数据！");
                    }
                    else if(dt.Rows.Count <= 0)
                    {
                        context.Response.Write("没有找到Id=" + id + "的数据！");
                    }
                    else
                    {
                        context.Response.Write(CommonHelper.RenderHtml("Admin/aboutEdit.html", new { Title = "编辑关于我们", Action = "Edit", about = dt.Rows[0], settings = CommonHelper.GetSetting() }));
                    }
                }
                else
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_department");
                    context.Response.Write(CommonHelper.RenderHtml("Admin/aboutList.html", new { Title = "关于我们列表", departments = dt.Rows, settings = CommonHelper.GetSetting() }));
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