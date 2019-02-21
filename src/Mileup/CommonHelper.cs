using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;

namespace MileageCup
{
    public class CommonHelper
    {
        /// <summary>
        /// 用data数据填充templateName模板，渲染生成html返回
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RenderHtml(string templateName, object data)
        {
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/templates"));//模板文件所在的文件夹
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("Data", data);//设置参数，在模板中可以通过$Data来引用

            Template vltTemplate = vltEngine.GetTemplate(templateName);
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);

            string html = vltWriter.GetStringBuilder().ToString();
            return html;
        }

        /// <summary>
        /// 判断文件是否为空
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool HasFile(HttpPostedFile file)
        {
            if(file == null)
            {
                return false;
            }
            else
            {
                return file.ContentLength > 0;
            }
        }
        /// <summary>
        /// /读取配置项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ReadSetting(string name)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select value from T_Setting where Name=@Name", new SqlParameter("@Name", name));
            if(dt.Rows.Count <= 0)
            {
                throw new Exception("找不到Name=" + name + "的配置项");
            }
            else if(dt.Rows.Count > 1)
            {
                throw new Exception("找到多条Name=" + name + "的配置项");
            }
            return (string)dt.Rows[0]["Value"];
        }

        /// <summary>
        /// 写入配置项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void WriteSetting(string name, string value)
        {
            SqlHelper.ExecuteNonQuery("Update T_Setting set Value=@Value where Name=@Name",
                new SqlParameter("@Value", value),
                new SqlParameter("@Name", name));
        }
         
        /// <summary>
        /// 获得所有的配置项
        /// </summary>
        /// <returns></returns>
        public static object GetSetting()
        {
            return new{
                        CN_Name = ReadSetting("CN_Name"),
                        EN_Name = ReadSetting("EN_Name"),
                        Address = ReadSetting("Address"),
                        huihui_pic = ReadSetting("huihui_pic"),
                        postCode = ReadSetting("postCode"),
                        linkMan = ReadSetting("linkMan"),
                        tel = ReadSetting("tel"),
                        Email = ReadSetting("Email"),
                        QQ = ReadSetting("QQ"),
                        weChat = ReadSetting("weChat"),
                        QQ_qun = ReadSetting("QQ_qun"),
                        microBlog = ReadSetting("microBlog"),
                        phone = ReadSetting("phone"),
                        guestbook = ReadSetting("guestbook"),
                        time = DateTime.Now.Year
            };
        }

        /// <summary>
        /// 判断后台是否登录
        /// </summary>
        public static void IsLogin()
        {
            HttpContext context = HttpContext.Current;

            string pwd = (string)context.Session["LoginUserPassWord"];
            if(String.IsNullOrEmpty(pwd))
            {
                context.Response.Redirect("Index.ashx");
            }
            else
            {
                int num = (int)SqlHelper.ExecuteScalar("select count(*) from T_sysUser where password=@pwd", new SqlParameter("@pwd", pwd));
                if (num != 1)
                {
                    context.Response.Redirect("Index.ashx");
                }
            }
        }

        public static object readLink()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_link");
            return dt.Rows;
        }

        /// <summary>
        /// MD5散列算法
        /// </summary>
        /// <param name="sDataIn"></param>
        /// <returns></returns>
        public static string GetMD5(string sDataIn)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }
    }
}