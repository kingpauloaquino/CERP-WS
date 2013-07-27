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

    [WebMethod(Description = "Adjust material's current warehouse stock quantity")]
    public string AdjustStockItem(int inventory_id, double qty, string remarks)
    {
        return Queries.AdjustStockItem(inventory_id, qty, remarks);
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
    public string ReceiveDelivery(int delivery_id, string invoice, string receipt, string lot_no, string receiving_remarks, string items)
    {
        return Queries.ReceiveDelivery(delivery_id, invoice, receipt, lot_no, receiving_remarks, items);
    }





    //[WebMethod(Description = "Sample WS Consumer, single record")]
    public string Consumer1()
    {
        string barcode = "ASSYCOM-075A";
        string type = "MAT";

        string ws_result = Queries.GetDetails(barcode, type);

        DataTable dt = new DataTable();
        dt = Functions.StringParser(ws_result);

        string parsed_result="";
        if (dt.Rows[0].ItemArray[0].ToString() == "0")
        {
            parsed_result = "ERROR: " + dt.Rows[0].ItemArray[1].ToString();
        }
        else
        {
            parsed_result = dt.Rows[0].ItemArray[0].ToString(); // material ID
            parsed_result = dt.Rows[0].ItemArray[1].ToString(); // material code
            parsed_result = dt.Rows[0].ItemArray[2].ToString(); // material type, so on..
        }
        return parsed_result;
    }

    //[WebMethod(Description = "Sample WS Consumer, multiple records")]
    public string Consumer2()
    {
        string barcode = "ASSYCOM-075A";
        string type = "MAT";

        string ws_result = Queries.GetStockItems(barcode, type);

        DataTable dt = new DataTable();
        dt = Functions.StringParser(ws_result);

        string parsed_result="";
        if (dt.Rows[0].ItemArray[0].ToString() == "0")
        {
            parsed_result = "ERROR: " + dt.Rows[0].ItemArray[1].ToString();
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                parsed_result += dt.Rows[i].ItemArray[0].ToString() + "-"; // ID
                parsed_result += dt.Rows[i].ItemArray[1].ToString() + "-"; // item ID
                parsed_result += dt.Rows[i].ItemArray[2].ToString() + "-"; // invoice #, so on..
            }
        }
        // nakaloop lang tapos concatenate yung string, pero syempre di naman ganyan gagawin

        return parsed_result;
    }

    //[WebMethod(Description = "Test")]
    public string Tester()
    {
        return "tester";
    }
}