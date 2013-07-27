using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;

/// <summary>
/// Summary description for Functions
/// </summary>
public static class Functions
{

    public static string FormatReturn(int _flag, string _result)
    {
        string result;
        using (DataTable dt = new DataTable("table"))
        {
            dt.Columns.Add("flag");
            dt.Columns.Add("result");
            dt.Rows.Add(_flag, _result);
            StringWriter sw = new StringWriter();
            dt.WriteXml(sw);
            result = sw.ToString();
            sw.Dispose();
        }

        return result;
    }

    public static DataTable FormatDTReturn(int _flag, string _result)
    {
        using (DataTable dt = new DataTable("table"))
        {
            dt.Columns.Add("flag");
            dt.Columns.Add("result");
            dt.Rows.Add(_flag, _result);
            return dt;
        }
    }

    public static DataTable StringParser(string ws_result)
    {
        StringReader sr = new StringReader(ws_result);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("table");
        ds.ReadXml(sr);
        dt = ds.Tables[0];
        return dt;
    }


    public static string DefaultDateFormat(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd");
    }

    public static string DefaultDateTimeFormat(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string IfError(string result)
    {
        DataTable dt = new DataTable();
        dt = StringParser(result);
        if (dt.Rows[0].ItemArray[0].ToString() == "-1")
        {
            return "ERROR: " + dt.Rows[0].ItemArray[1].ToString();
        }
        else
        {
            return "";
        }
    }

    public enum status
    { 
        pending = 19,
        incomplete = 22,
        complete = 21,
        open = 13,
        close = 14,

    }
}