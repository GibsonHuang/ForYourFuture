using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

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
        String SrcIPIncludeList = Request.Form["SrcIPIncludeList"];
        String DestIPIncludeList = Request.Form["DestIPIncludeList"];
        String SensorIPIncludeList = Request.Form["SensorIPIncludeList"];
        String EventNameIncludeList = Request.Form["EventNameIncludeList"];

        //String DeviceID = "27898";
        //String BeginDate = "2014/6/9 00:00:00";
        //String EndDate = "2014/6/10 00:00:00";
        //String PageSize = "1000";
        //String PageNum = "1";

        ////透過網址丟參數
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


                
                //將Table寫成json格式的字串
                string EventString="",RowString="";
                int RID = 1;

//kai的Json格式
                using (SqlDataReader EventReader = sqlcmd.ExecuteReader())
                {
                    while(EventReader.Read())
                    {
                        RowString = "";

                        for(int i=0;i<EventReader.FieldCount;i++)
                        {
                            RowString = RowString + "TD" + i.ToString() + ":{Value:\"" + EventReader.GetValue(i).ToString() + "\"},";
                        }

                        //EventString = EventString + "R" + RID.ToString() + ":{"+ RowString.Substring(0,RowString.Length-1) +"},"+"</br>" ;
                        EventString = EventString + "R" + RID.ToString() + ":{" + RowString.Substring(0, RowString.Length - 1) + "},";

                        RID++;
                    }
                }
 
                if (RowString=="")
                {
                    EventString = "";
                }
                else{
                    EventString = "{" + EventString.Substring(0, EventString.Length - 1) + "}"; 
                };

                Response.Write(EventString);

                conn.Close();

/*//Json陣列格式              
                                using (SqlDataReader EventReader = sqlcmd.ExecuteReader())
                                {
                                    while (EventReader.Read())
                                    {
                                        RowString = "";

                                        for (int i = 0; i < EventReader.FieldCount; i++)
                                        {
                                            RowString = RowString + "TD" + i.ToString() + ":{Value:\"" + EventReader.GetValue(i).ToString() + "\"},";
                                        }

                                        EventString = EventString + "{" + RowString.Substring(0, RowString.Length - 1) + "},";

                                    }
                                }

                                EventString = "[" + EventString.Substring(0, EventString.Length - 1) + "]";

                                Response.Write(EventString);

                                conn.Close();
                */
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