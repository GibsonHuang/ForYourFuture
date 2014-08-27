using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;

public partial class DBAccess_EventString : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //透過ajax丟參數
        String DeviceID = Request.Form["DeviceID"];
        String BeginDate = Request.Form["BeginDate"];
        String EndDate = Request.Form["EndDate"];
        String PageSize = Request.Form["PageSize"];
        String PageNum = Request.Form["PageNum"];
        //String SrcIPIncludeList = Request.Form["SrcIPIncludeList"];
        //String DestIPIncludeList = Request.Form["DestIPIncludeList"];
        //String SensorIPIncludeList = Request.Form["SensorIPIncludeList"];
        //String EventNameIncludeList = Request.Form["EventNameIncludeList"];

        //String DeviceID = "27898";
        //String BeginDate = "2010-12-24 10:00:00";
        //String EndDate = "2010-12-24 11:00:00";
        //String PageSize = "1000";
        //String PageNum = "1";
        String SrcIPIncludeList = "";
        String DestIPIncludeList = "";
        String SensorIPIncludeList = "";
        String EventNameIncludeList = "";

        //透過網址丟參數
        //String DeviceID = Request.QueryString["DeviceID"];
        //String BeginDate = Request.QueryString["BeginDate"];
        //String EndDate = Request.QueryString["EndDate"];
        //String PageSize = Request.QueryString["PageSize"];
        //String PageNum = Request.QueryString["PageNum"];

        //Request.TotalBytes;
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "192.168.128.80";
        builder.InitialCatalog = "UGuardDB";
        builder.UserID = "sa";
        builder.Password = "rd@qwertpoiuy";

        using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
        {
            try
            {
                conn.Open();

                //SqlCommand sqlcmd = new SqlCommand("select AssetID from Assets", conn);

                SqlCommand sqlcmd = new SqlCommand("dbo.x_up_EventDetailView", conn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandTimeout = 0;
                sqlcmd.Parameters.AddWithValue("@CallerID", 0);
                sqlcmd.Parameters.AddWithValue("@DeviceID", DeviceID);
                sqlcmd.Parameters.AddWithValue("@BeginDate", BeginDate);
                sqlcmd.Parameters.AddWithValue("@EndDate", EndDate);
                sqlcmd.Parameters.AddWithValue("@PageSize", PageSize);
                sqlcmd.Parameters.AddWithValue("@PageNum", PageNum);
                sqlcmd.Parameters.AddWithValue("@SrcIPIncludeList", SrcIPIncludeList);
                sqlcmd.Parameters.AddWithValue("@DestIPIncludeList", DestIPIncludeList);
                sqlcmd.Parameters.AddWithValue("@SensorIPIncludeList", SensorIPIncludeList);
                sqlcmd.Parameters.AddWithValue("@EventNameIncludeList", EventNameIncludeList);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                Response.Write (serializer.Serialize(rows));
            }

            catch (Exception ex)
            {
                string Error = ex.Message.Replace("'", "");
                Response.Write("<Script language='Javascript'>");
                Response.Write("alert('" + Error + "')");
                Response.Write("</" + "Script>");
                conn.Close();

            }
        }

    }
}