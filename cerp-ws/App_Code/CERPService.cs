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

    public CERPService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool DBConTest()
    {
        return Database.TestCon();
    }

    private bool IsExisting(string barcode, string type)
    {
        return Queries.IsExisting(barcode, type);
    }

    [WebMethod(Description = "Return Material/Product Details")]
    public string GetDetails(string barcode, string type)
    {
        return Queries.GetDetails(barcode, type);
    }

    [WebMethod(Description = "Return Material/Product Total Stock")]
    public string GetStock(string barcode, string type)
    {
        return Queries.GetStock(barcode, type);
    }

    [WebMethod(Description = "Return All Material/Product Stock Details")]
    public string GetStockItems(string barcode, string type)
    {
        return Queries.GetStockItems(barcode, type);
    }

    [WebMethod(Description = "Return All Open Deliveries")]
    public string GetDeliveries()
    {
        return Queries.GetDeliveries();
    }

    [WebMethod(Description = "Return Delivery Items")]
    public string GetDeliveryItems(int delivery_id)
    {
        return Queries.GetDeliveryItems(delivery_id);
    }

    [WebMethod(Description = "Receive Deliveries")]
    public string ReceiveDelivery(int delivery_id, string invoice, string receipt, string lot_no)
    {
        // set delivery_items to 22 (incomplete)


        return "";
    }

    [WebMethod(Description = "Test")]
    public string Tester()
    {
        return Database.InsertRecord("INSERT INTO devices (device_code) VALUES ('tester'); SELECT LAST_INSERT_ID();", true).ToString();
    }
}