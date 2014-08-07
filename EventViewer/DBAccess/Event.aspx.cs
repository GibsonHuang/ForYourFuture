using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class DBAccess_Event : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        String DeviceID = Request.Form["DeviceID"];
        String BeginDate = Request.Form["BeginDate"];
        String EndDate = Request.Form["EndDate"];
        String PageSize = Request.Form["PageSize"];
        String PageNum = Request.Form["PageNum"];

        if (DeviceID == null) { DeviceID = Request.QueryString["DeviceID"]; }
        if (BeginDate == null) { BeginDate = Request.QueryString["BeginDate"]; }
        if (EndDate == null) { EndDate = this.Request.QueryString["EndDate"]; }


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

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlcmd;

                DataTable dt = new DataTable();
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();

                conn.Close();
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