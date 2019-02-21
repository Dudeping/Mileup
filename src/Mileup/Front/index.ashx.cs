using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MileageCup
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            DataTable abouts = SqlHelper.ExecuteDataTable("select * from T_department where isHui=@isHui",
                new SqlParameter("@isHui", 1));

            DataTable grb = SqlHelper.ExecuteDataTable(@"select * from
                (
                    select *,
                    row_number() over (order by p.Id desc) as num
                    from T_grb p 
                ) as s
                where s.num between 1 and 4");

            DataTable reactive = SqlHelper.ExecuteDataTable("select * from T_active where IsRecommend=1");
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6 };

            DataTable active = SqlHelper.ExecuteDataTable(@"select * from
                (
                    select *,
                    row_number() over (order by p.Id desc) as num
                    from T_active p 
                ) as s
                where s.num between 1 and 6");

            context.Response.Write(CommonHelper.RenderHtml("Front/index.html", 
                new {Title = "四川农业大学校学生治安服务队", aboutUs = abouts.Rows[0], settings = CommonHelper.GetSetting(), reactives = reactive.Rows, actives = active.Rows, grbs = grb.Rows, times = numbers, links = CommonHelper.readLink() }));
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