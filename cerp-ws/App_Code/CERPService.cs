using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Text;
using System.Data;
using System.IO;

[WebService(Namespace = "http://cerp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class CERPService : System.Web.Services.WebService
{
    Database db = new Database();
    DataTable dt;
    string sql = "";
    string result = "";
    bool returnFlag = false;

    public CERPService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string DBConTest()
    {
        string conn = System.Configuration.ConfigurationManager.ConnectionStrings["str_con"].ConnectionString;
        MySqlConnection con = new MySqlConnection(conn);
        try
        {
            con.Open();
            con.Close();
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private bool IsExisting(string barcode, string type)
    {
        sql = "SELECT " +
                "m.id " +
            "FROM materials AS m " +
            "WHERE m.bar_code = '" + barcode + "'";
        dt = new DataTable();
        dt = db.Query(sql);
        if (dt != null)
        {
            returnFlag = true;
        }
        return returnFlag;
    }

    [WebMethod(Description = "Return Material or Product Details")]
    public string GetDetails(string barcode, string type)
    {
        sql = "SELECT " +
                "m.id AS mid, m.material_code AS code, lookups.description AS type, item_classifications.classification AS classification, " +
                "m.description AS description, brand_models.brand_model AS model, CONCAT(users.first_name, ' ', users.last_name) AS pic, " +
                "lookups2.description AS unit, m.defect_rate, m.sorting_percentage, m.msq, lookup_status.description AS status " +
            "FROM materials AS m " +
            "JOIN lookups ON lookups.id = m.material_type " +
            "JOIN item_classifications ON item_classifications.id = m.material_classification " +
            "JOIN brand_models ON brand_models.id = m.brand_model " +
            "JOIN users ON users.id = m.person_in_charge " +
            "JOIN lookups AS lookups2 ON lookups2.id = m.unit " +
            "JOIN lookup_status ON lookup_status.id = m.status " +
            "WHERE m.bar_code = '" + barcode + "'";
        dt = new DataTable();
        dt = db.Query(sql);
        if (dt != null)
        {
            StringWriter sw = new StringWriter();
            dt.WriteXml(sw);
            result = sw.ToString();
        }
        else
        {
            result = Functions.FormatReturn(0, "Barcode not found");
        }

        return result;
    }

    [WebMethod(Description = "Return Material or Product Stock")]
    public string GetStock(string barcode, string type)
    {
        if (IsExisting(barcode, type))
        {
            sql = "SELECT " +
                "COALESCE(SUM(warehouse_inventories.qty),0) AS total_stock " +
                //"SUM(COALESCE(warehouse_inventories.qty),0) AS total_stock" +
            "FROM warehouse_inventories " +
            "JOIN materials ON materials.id = warehouse_inventories.item_id " +
            "WHERE materials.material_code = '" + barcode + "'";
            dt = new DataTable();
            dt = db.Query(sql);
            if (dt != null)
            {
                StringWriter sw = new StringWriter();
                dt.WriteXml(sw);
                result = sw.ToString();
            }
        }
        else
        {
            result = Functions.FormatReturn(0, "Barcode not found");
        }
        return result;
    }

    [WebMethod(Description = "Return Material or Product Stock Details")]
    public string GetStockDetails(string barcode, string type)
    {
        if (IsExisting(barcode, type))
        {
            sql = "SELECT " +
                "warehouse_inventories.id, warehouse_inventories.item_id, warehouse_inventories.invoice_no, warehouse_inventories.lot_no, " +
                "warehouse_inventories.qty, warehouse_inventories.remarks, lookups.description AS unit " +
            "FROM warehouse_inventories " +
            "JOIN materials ON materials.id = warehouse_inventories.item_id " +
            "INNER JOIN lookups ON lookups.id = materials.unit " +
            "WHERE materials.bar_code = '" + barcode + "'";
            dt = new DataTable();
            dt = db.Query(sql);
            if (dt != null)
            {
                StringWriter sw = new StringWriter();
                dt.WriteXml(sw);
                result = sw.ToString();
            }
            else
            {
                result = Functions.FormatReturn(0, "No items found");
            }
        }
        else
        {
            result = Functions.FormatReturn(0, "Barcode not found");
        }

        return result;
    }
}