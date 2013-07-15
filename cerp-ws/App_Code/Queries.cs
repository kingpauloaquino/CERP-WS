using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;

/// <summary>
/// Summary description for Queries
/// </summary>
public static class Queries
{
    static bool returnFlag;
    static string result;
    static string sql;
    static DataTable dt;

	static Queries()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    internal static bool IsExisting(string barcode, string type)
    {
        if (type == "MAT")
        {
            sql = "SELECT " +
                    "m.id " +
                "FROM materials AS m " +
                "WHERE m.bar_code = '" + barcode.Trim() + "'";
        }
        else
        {
            sql = "SELECT " +
                    "p.id " +
                "FROM products AS p " +
                "WHERE p.bar_code = '" + barcode.Trim() + "'";
        }
        dt = new DataTable();
        dt = Database.Query(sql);
        if (dt != null)
        {
            returnFlag = true;
        }
        return returnFlag;
    }

    internal static string GetDetails(string barcode, string type)
    {
        if (type == "MAT")
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
                "WHERE m.bar_code = '" + barcode.Trim() + "'";
        }
        else
        {
            sql = "SELECT " +
                    "p.id AS pid, p.product_code AS code, brand_models.brand_model AS brand, product_series.series, p.description, lookup_status.description AS status " +
                "FROM products AS p " +
                "JOIN brand_models ON brand_models.id = p.brand_model " +
                "JOIN product_series On product_series.id = p.series " +
                "JOIN lookup_status ON lookup_status.id = p.status " +
                "WHERE p.bar_code = '" + barcode.Trim() + "'";
        }
        dt = new DataTable();
        dt = Database.Query(sql);
        if (dt != null)
        {
            if (dt.Rows[0].ItemArray[0].ToString() == "-1")
            {
                result = dt.Rows[0].ItemArray[1].ToString();
            }
            else
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

    internal static string GetStock(string barcode, string type)
    {
        if (IsExisting(barcode.Trim(), type))
        {
            if (type == "MAT")
            {
                sql = "SELECT " +
                    "COALESCE(SUM(warehouse_inventories.qty),0) AS total_stock " +
                    //"SUM(COALESCE(warehouse_inventories.qty),0) AS total_stock" +
                "FROM warehouse_inventories " +
                "JOIN materials ON materials.id = warehouse_inventories.item_id " +
                "WHERE materials.bar_code = '" + barcode.Trim() + "'";
            }
            else
            {
                sql = "SELECT " +
                        "COALESCE(SUM(warehouse2_inventories.qty),0) AS total_stock " +
                    "FROM warehouse2_inventories " +
                    "JOIN products ON products.id = warehouse2_inventories.item_id " +
                    "WHERE products.bar_code = '" + barcode.Trim() + "'";
            }

            dt = new DataTable();
            dt = Database.Query(sql);
            if (dt.Rows[0].ItemArray[0].ToString() == "-1")
            {
                result = dt.Rows[0].ItemArray[1].ToString();
            }
            else
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

    internal static string GetStockItems(string barcode, string type)
    {
        if (IsExisting(barcode.Trim(), type))
        {
            if (type == "MAT")
            {
                sql = "SELECT " +
                "warehouse_inventories.id, warehouse_inventories.item_id, warehouse_inventories.invoice_no, warehouse_inventories.lot_no, " +
                "warehouse_inventories.qty, warehouse_inventories.remarks, lookups.description AS unit " +
                "FROM warehouse_inventories " +
                "JOIN materials ON materials.id = warehouse_inventories.item_id " +
                "INNER JOIN lookups ON lookups.id = materials.unit " +
                "WHERE materials.bar_code = '" + barcode.Trim() + "'";
            }
            else
            {
                sql = "SELECT " +
                "warehouse2_inventories.id, warehouse2_inventories.item_id, warehouse2_inventories.batch_no, warehouse2_inventories.prod_lot_no, " +
                "warehouse2_inventories.qty, warehouse2_inventories.remarks, lookups.description AS unit " +
                "FROM warehouse2_inventories " +
                "JOIN products ON products.id = warehouse2_inventories.item_id " +
                "INNER JOIN lookups ON lookups.id = products.unit " +
                "WHERE products.bar_code = '" + barcode.Trim() + "'";
            }

            dt = new DataTable();
            dt = Database.Query(sql);
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

    internal static string GetDeliveries()
    {
        sql = "SELECT " +
                "deliveries.id AS delivery_id, purchases.id AS purchase_id, purchases.po_number, deliveries.delivery_date, supplier_id, name AS supplier_name, " +
                "lookup_status.description AS status, lookup_status2.description AS completion_status " +
            "FROM deliveries " +
            "JOIN purchases ON purchases.id = deliveries.purchase_id " +
            "JOIN suppliers ON suppliers.id = supplier_id " +
            "JOIN lookup_status ON lookup_status.id = deliveries.status " +
            "JOIN lookup_status AS lookup_status2 ON lookup_status2.id = deliveries.completion_status " +
            "WHERE lookup_status.description = 'Open' AND lookup_status2.description != 'Complete' ";
        dt = new DataTable();
        dt = Database.Query(sql);
        if (dt != null)
        {
            if (dt.Rows[0].ItemArray[0].ToString() == "-1")
            {
                result = dt.Rows[0].ItemArray[1].ToString();
            }
            else
            {
                StringWriter sw = new StringWriter();
                dt.WriteXml(sw);
                result = sw.ToString();
            }
        }
        else
        {
            result = Functions.FormatReturn(0, "No items found");
        }
        return result;
    }

    internal static string GetDeliveryItems(int delivery_id)
    {
        sql = "SELECT " +
                "delivery_items.id, purchase_items.item_id AS mid, materials.material_code AS code, materials.description, delivery_items.invoice, delivery_items.remarks, " +
                "lookup_status.description AS status, lookups.description AS unit, COALESCE(delivery_items.received,0) AS received, COALESCE(purchase_items.quantity,0) AS po_qty, " +
                "COALESCE((" +
                "SELECT SUM(received) " +
                    "FROM delivery_items AS del " +
                    "INNER JOIN purchase_items AS pi ON pi.id = del.purchase_item_id " +
                    "INNER JOIN purchases AS p ON p.id = pi.purchase_id " +
                    "INNER JOIN materials AS m ON m.id = pi.item_id " +
                    "WHERE del.purchase_item_id = delivery_items.purchase_item_id " +
                "),0) AS delivered " +
            "FROM delivery_items " +
            "INNER JOIN purchase_items ON purchase_items.id = delivery_items.purchase_item_id " +
            "INNER JOIN purchases ON purchases.id = purchase_items.purchase_id " +
            "INNER JOIN materials ON materials.id = purchase_items.item_id " +
            "INNER JOIN item_costs ON item_costs.item_id = materials.id AND item_costs.item_type = 'MAT' AND item_costs.supplier = purchases.supplier_id " +
            "INNER JOIN lookups ON lookups.id = materials.unit " +
            "INNER JOIN lookup_status ON lookup_status.id = delivery_items.status " +
            "WHERE delivery_items.delivery_id=" + delivery_id;
        dt = new DataTable();
        dt = Database.Query(sql);
        if (dt != null)
        {
            if (dt.Rows[0].ItemArray[0].ToString() == "-1")
            {
                result = dt.Rows[0].ItemArray[1].ToString();
            }
            else
            {
                StringWriter sw = new StringWriter();
                dt.WriteXml(sw);
                result = sw.ToString();
            }
        }
        else
        {
            result = Functions.FormatReturn(0, "Delivery items not found");
        }
        return result;
    }
}