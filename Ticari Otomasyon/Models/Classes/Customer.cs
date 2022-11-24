using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.Classes
{
    public class Customer
    {
       
        public int Logicalref { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string CompanyName2 { get; set; }
        public string SpecialCode { get; set; }
        public string CyphCode { get; set; }
        public string TradingGrp { get; set; }
        public string Incharge { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TaxOffice { get; set; }
        public string IdentityNumber { get; set; }
        public string Amount { get; set; }
      

    }
    public class GetCustomer
    {
        public int Logicalref { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string CompanyName2 { get; set; }
        public string SpecialCode { get; set; }
        public string CyphCode { get; set; }
        public string TradingGrp { get; set; }
        public string Incharge { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TaxOffice { get; set; }
        public string Vkn { get; set; }
        public string TckNo { get; set; }
    }
    public class SellTransactionCustomer
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
    public class SellTransactionCustomerName
    {        
        public string CompanyName { get; set; }      
    }
}