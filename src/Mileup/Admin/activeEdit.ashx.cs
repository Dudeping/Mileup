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
    /// activeViewEdit 的摘要说明
    /// </summary>
    public class activeEdit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            CommonHelper.IsLogin();
            string action = context.Request["Action"];
            bool isSave = !String.IsNullOrEmpty(context.Request["save"]);
            if (isSave)
            {
                if (action == "Add")
                {
                    string name = context.Request["Name"];
                    string introduct = context.Request["introduct"];
                    string recommend = context.Request["recommend"];
                    HttpPostedFile pic_1 = context.Request.Files["pic_1"];
                    if ((CommonHelper.HasFile(pic_1) == false) || name == "" || introduct == "")
                        context.Response.Redirect("Error.ashx");

                    int isRecommend;
                    if(recommend == "on")
                    {
                        isRecommend = 1;
                    }
                    else
                    {
                        isRecommend = 0;
                    }

                    
                    string pic_1Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_1.FileName);
                    pic_1.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_1Name));

                    long id = Convert.ToInt64(SqlHelper.ExecuteScalar("Insert into T_active(Name, introduct, IsRecommend, pic_1, createTime) values(@Name, @introduct, @IsRecommend, @pic_1, getdate()) select @@identity",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@introduct", introduct),
                        new SqlParameter("@IsRecommend", isRecommend),
                        new SqlParameter("@pic_1", "/uploadfile/" + pic_1Name)));

                    HttpPostedFile pic_2 = context.Request.Files["pic_2"];
                    if (CommonHelper.HasFile(pic_2))
                    {
                        string pic_2Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_2.FileName);
                        pic_2.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_2Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_2=@pic_2 where Id=@Id",
                            new SqlParameter("@pic_2", "/uploadfile/" + pic_2Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_3 = context.Request.Files["pic_3"];
                    if (CommonHelper.HasFile(pic_3))
                    {
                        string pic_3Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_3.FileName);
                        pic_3.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_3Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_3=@pic_3 where Id=@Id",
                            new SqlParameter("@pic_3", "/uploadfile/" + pic_3Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_4 = context.Request.Files["pic_4"];
                    if (CommonHelper.HasFile(pic_4))
                    {
                        string pic_4Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_4.FileName);
                        pic_4.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_4Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_4=@pic_4 where Id=@Id",
                            new SqlParameter("@pic_4", "/uploadfile/" + pic_4Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_5 = context.Request.Files["pic_5"];
                    if (CommonHelper.HasFile(pic_5))
                    {
                        string pic_5Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_5.FileName);
                        pic_5.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_5Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_5=@pic_5 where Id=@Id",
                            new SqlParameter("@pic_5", "/uploadfile/" + pic_5Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_6 = context.Request.Files["pic_6"];
                    if (CommonHelper.HasFile(pic_6))
                    {
                        string pic_6Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_6.FileName);
                        pic_6.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_6Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_6=@pic_6 where Id=@Id",
                            new SqlParameter("@pic_6", "/uploadfile/" + pic_6Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_7 = context.Request.Files["pic_7"];
                    if (CommonHelper.HasFile(pic_7))
                    {
                        string pic_7Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_7.FileName);
                        pic_7.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_7Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_7=@pic_7 where Id=@Id",
                            new SqlParameter("@pic_7", "/uploadfile/" + pic_7Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_8 = context.Request.Files["pic_8"];
                    if (CommonHelper.HasFile(pic_8))
                    {
                        string pic_8Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_8.FileName);
                        pic_8.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_8Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_8=@pic_8 where Id=@Id",
                            new SqlParameter("@pic_8", "/uploadfile/" + pic_8Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_9 = context.Request.Files["pic_9"];
                    if (CommonHelper.HasFile(pic_9))
                    {
                        string pic_9Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_9.FileName);
                        pic_9.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_9Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_9=@pic_9 where Id=@Id",
                            new SqlParameter("@pic_9", "/uploadfile/" + pic_9Name),
                            new SqlParameter("@Id", id));
                    }

                    context.Response.Redirect("activeList.ashx");

                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    string name = context.Request["Name"];
                    string introduct = context.Request["introduct"];
                    string recommend = context.Request["recommend"];

                    if (name == "" || introduct == "")
                        context.Response.Redirect("Error.ashx");

                    int isRecommend;
                    if (recommend == "on")
                    {
                        isRecommend = 1;
                    }
                    else
                    {
                        isRecommend = 0;
                    }
                    SqlHelper.ExecuteNonQuery("Update T_active Set Name=@Name, introduct=@introduct, isRecommend=@isRecommend where Id=@Id",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@introduct", introduct),
                        new SqlParameter("@isRecommend", isRecommend),
                        new SqlParameter("@Id", id));

                    HttpPostedFile pic_1 = context.Request.Files["pic_1"];
                    if (CommonHelper.HasFile(pic_1))
                    {
                        string pic_1Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_1.FileName);
                        pic_1.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_1Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_1=@pic_1 where Id=@Id",
                            new SqlParameter("@pic_1", "/uploadfile/" + pic_1Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_2 = context.Request.Files["pic_2"];
                    if (CommonHelper.HasFile(pic_2))
                    {
                        string pic_2Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_2.FileName);
                        pic_2.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_2Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_2=@pic_2 where Id=@Id",
                            new SqlParameter("@pic_2", "/uploadfile/" + pic_2Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_3 = context.Request.Files["pic_3"];
                    if (CommonHelper.HasFile(pic_3))
                    {
                        string pic_3Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_3.FileName);
                        pic_3.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_3Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_3=@pic_3 where Id=@Id",
                            new SqlParameter("@pic_3", "/uploadfile/" + pic_3Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_4 = context.Request.Files["pic_4"];
                    if (CommonHelper.HasFile(pic_4))
                    {
                        string pic_4Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_4.FileName);
                        pic_4.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_4Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_4=@pic_4 where Id=@Id",
                            new SqlParameter("@pic_4", "/uploadfile/" + pic_4Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_5 = context.Request.Files["pic_5"];
                    if (CommonHelper.HasFile(pic_5))
                    {
                        string pic_5Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_5.FileName);
                        pic_5.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_5Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_5=@pic_5 where Id=@Id",
                            new SqlParameter("@pic_5", "/uploadfile/" + pic_5Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_6 = context.Request.Files["pic_6"];
                    if (CommonHelper.HasFile(pic_6))
                    {
                        string pic_6Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_6.FileName);
                        pic_6.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_6Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_6=@pic_6 where Id=@Id",
                            new SqlParameter("@pic_6", "/uploadfile/" + pic_6Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_7 = context.Request.Files["pic_7"];
                    if (CommonHelper.HasFile(pic_7))
                    {
                        string pic_7Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_7.FileName);
                        pic_7.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_7Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_7=@pic_7 where Id=@Id",
                            new SqlParameter("@pic_7", "/uploadfile/" + pic_7Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_8 = context.Request.Files["pic_8"];
                    if (CommonHelper.HasFile(pic_8))
                    {
                        string pic_8Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_8.FileName);
                        pic_8.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_8Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_8=@pic_8 where Id=@Id",
                            new SqlParameter("@pic_8", "/uploadfile/" + pic_8Name),
                            new SqlParameter("@Id", id));
                    }
                    HttpPostedFile pic_9 = context.Request.Files["pic_9"];
                    if (CommonHelper.HasFile(pic_9))
                    {
                        string pic_9Name = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(pic_9.FileName);
                        pic_9.SaveAs(context.Server.MapPath("~/uploadfile/" + pic_9Name));
                        SqlHelper.ExecuteNonQuery("Update T_active Set pic_9=@pic_9 where Id=@Id",
                            new SqlParameter("@pic_9", "/uploadfile/" + pic_9Name),
                            new SqlParameter("@Id", id));
                    }

                    context.Response.Redirect("activeList.ashx");
                }
                else
                {
                    context.Response.Write("参数错误！");
                }
            }
            else
            {
                if (action == "Add")
                {
                    context.Response.Write(CommonHelper.RenderHtml("Admin/activeEdit.html", new { Title = "添加活动", Action = "Add" , active = new { Id = 0, Name = "", isRecommend=0, introduct = ""}, settings = CommonHelper.GetSetting() }));
                }
                else if (action == "Edit")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_active where Id=@Id",
                        new SqlParameter("@Id", id));
                    if(dt.Rows.Count <=0)
                    {
                        context.Response.Write("找不到Id=" + id + "的活动！");
                    }
                    else if(dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id  + "的活动！");
                    }
                    else
                    {
                        context.Response.Write(CommonHelper.RenderHtml("Admin/activeEdit.html", new { Title = "活动编辑", Action = "Edit", active = dt.Rows[0], settings = CommonHelper.GetSetting() }));
                    }
                }
                else if (action == "Delete")
                {
                    long id = Convert.ToInt64(context.Request["Id"]);
                    string name = context.Request["picName"];
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_active where Id=@Id",
                        new SqlParameter("@Id", id));
                    if (dt.Rows.Count <= 0)
                    {
                        context.Response.Write("找不到Id=" + id + "的活动！");
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        context.Response.Write("找到多条Id=" + id + "的活动！");
                    }
                    else
                    {
                        if (name == "pic_2")
                        {
                            //string filename = context.Request["pic_2"];
                            //if (File.Exists(context.Server.MapPath("~/uploadfile/") + filename))
                            //    context.Response.Write("");
                            //    File.Delete(context.Server.MapPath("~/uploadfile/") + filename);

                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_2=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                        if (name == "pic_3")
                        {
                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_3=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                        if (name == "pic_4")
                        {
                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_4=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                        if (name == "pic_5")
                        {
                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_5=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                        if (name == "pic_6")
                        {
                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_6=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                        if (name == "pic_7")
                        {
                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_7=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                        if (name == "pic_8")
                        {
                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_8=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                        if (name == "pic_9")
                        {
                            SqlHelper.ExecuteNonQuery("Update T_active Set pic_9=@pic where Id=@Id",
                            new SqlParameter("@pic", ""),
                            new SqlParameter("@Id", id));
                        }
                    }
                    context.Response.Redirect("activeEdit.ashx?Action=Edit&Id=" + id);
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