using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.Classes
{
    public class Item
    {
        
        public int Logicalref { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        
        public string MainUnit { get; set; }
        public string SpecialCode { get; set; }
     
        public string Amount { get; set; }
     
    }
    public class GetItem
    {

        public int Logicalref { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string ItemName2 { get; set; }
        public string MainUnit { get; set; }
        public string SpecialCode { get; set; }
        public string CyphCode { get; set; }
        public string ProducerCode { get; set; }
        public double Vat { get; set; }
        public double SellVat { get; set; }
        public double ReturnVat { get; set; }
    
     

    }
    public class SellTransactionItemName
    {
        public string ItemName { get; set; }
    }
}