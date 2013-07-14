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
}