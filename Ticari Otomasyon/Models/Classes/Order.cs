using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.Classes
{
    public class Order
    {
        public int Logicalref { get; set; }
        public string Date { get; set; }
        public string Ficheno { get; set; }
        public string CompanyName { get; set; }
        public string SpecialCode { get; set; }
        public string TradingGrp { get; set; }
        public string Amount { get; set; }
        public string SourceIndex { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string SalesMan { get; set; }
    }
    public class GetOrderView
    {
        public GetOrderTop GetOrderTop_ { get; set; }
        public List<GetOrderEnd> GetOrderEnd_ { get; set; }
    }
    public class GetOrderEnd
    {
        public string barcode { get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string amount { get; set; }
        public string mainUnit { get; set; }
        public string price { get; set; }
        public string vat { get; set; }
        public string Ind1 { get; set; }
        public string Ind2 { get; set; }
        public string Ind3 { get; set; }
        public string Ind4 { get; set; }
        public string Ind5 { get; set; }
        public string total { get; set; }
        public string netTotal { get; set; }

    }
    public class OrderCapiDept
    {
        public int Nr { get; set; }
        public string Name { get; set; }
    }
    public class OrderCapiDiv
    {
        public int Nr { get; set; }
        public string Name { get; set; }
    }
    public class OrderCapiWhouse
    {
        public int Nr { get; set; }
        public string Name { get; set; }
    }
    public class OrderSalesMan
    {

        public int Nr { get; set; }
        public string Name { get; set; }
    }
    public class OrderTradingGrp
    {

        public int Nr { get; set; }
        public string Name { get; set; }
    }
    public class OrderCustomerCode
    {
        public int Nr { get; set; }
        public string Code { get; set; }
    }
    public class OrderCustomerDefinition
    {
        public int Nr { get; set; }
        public string Definition { get; set; }
    }
    public class OrderCodeToDefinition
    {
        public string Code { get; set; }
        public string Definition { get; set; }
    }
    public class OrderGetItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Vat { get; set; }
        public string MainUnit { get; set; }
        public string Price { get; set; }
    }
    public class OrderUnitset
    {
        public int Lineref { get; set; }
        public int Unitsetref { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
    public class OrderFicheNo
    {
        public string FicheNo { get; set; }
    }
}