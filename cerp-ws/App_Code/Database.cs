using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;

/// <summary>
/// Summary description for Database
/// </summary>
public class Database
{
    public string sql = "";
    public string table = "";

	public Database()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private MySqlConnection Connect()
    {
        string conn = System.Configuration.ConfigurationManager.ConnectionStrings["str_con"].ConnectionString;
        MySqlConnection con = new MySqlConnection(conn);
        return con;
    }

    public DataTable Query(string _sql)
    {
        MySqlConnection con = Connect();
        MySqlDataAdapter da = new MySqlDataAdapter(_sql, con);
        DataTable dt = new DataTable("table");
        try
        {
            con.Open();
            da.Fill(dt);
            dt = (dt.Rows.Count > 0) ? dt : null;
            con.Close();
        }
        catch (Exception ex)
        {

        }
        finally
        { 
        
        }
        return dt;
    }
}