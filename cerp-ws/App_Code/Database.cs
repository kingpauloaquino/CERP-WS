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
public static class Database
{
    static DataTable dt;

    public static string defaultConnectionString
    {
        get;
        private set;
    }

    static Database()
    {
        Database.defaultConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["str_con"].ConnectionString;
    }

    private static MySqlConnection Create()
    {
        return new MySqlConnection(Database.defaultConnectionString);
    }

    // returns DataTable type collection
    public static DataTable Query(string sql)
    {
        if (TestCon())
        {
            using (var connection = Database.Create())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (var da = new MySqlDataAdapter(command.CommandText, connection))
                    {
                        try
                        {
                            dt = new DataTable("table");
                            da.Fill(dt);
                            dt = (dt.Rows.Count > 0) ? dt : null;
                        }
                        catch (Exception ex)
                        {
                            return Functions.FormatDTReturn(-1, ex.Message);
                        }
                    }
                }
            }
        }
        else
        {
            return Functions.FormatDTReturn(-1, "Unable to connect to database.");
        }
        return dt;
    }

    // returns new identity seed if returnSeed==True
    public static object InsertRecord(string sql, bool returnSeed)
    {
        if (TestCon())
        {
            using (var connection = Database.Create())
            {
                using (var cmd = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        if (returnSeed)
                        {
                            return cmd.ExecuteScalar();
                        }
                        else
                        {
                            cmd.ExecuteNonQuery();
                            return "";
                        }
                        // return (returnSeed) ? cmd.ExecuteScalar() : cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return Functions.FormatReturn(-1, ex.Message);
                    }
                }
            }
        }
        else
        {
            return Functions.FormatReturn(-1, "Unable to connect to database.");
        }
    }

    // returns number of updated record
    public static string UpdateRecord(string sql)
    {
        if (TestCon())
        {
            using (var connection = Database.Create())
            {
                using (var cmd = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        return Functions.FormatReturn(cmd.ExecuteNonQuery(), "Update successful.");
                    }
                    catch (Exception ex)
                    {
                        return Functions.FormatReturn(-1, ex.Message);
                    }
                }
            }
        }
        else
        {
            return Functions.FormatReturn(-1, "Unable to connect to database.");
        }
    }

    public static bool TestCon()
    {
        using (var con = new MySqlConnection(Database.defaultConnectionString))
        {
            try
            {
                con.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}