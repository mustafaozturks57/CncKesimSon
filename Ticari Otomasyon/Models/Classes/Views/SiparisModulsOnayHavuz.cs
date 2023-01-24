using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.Classes.Views
{
    public class SiparisModulsOnayHavuz
    {
     
        public string FisNo                      { get; set; }
        public string Müşteri_Adı               { get; set; }
        public DateTime  Tarih                        { get; set; }
        public DateTime Termin_Tarihi                { get; set; }
        public string Sipariş_Şekli             { get; set; }
        public string Sipariş_Durumu            { get; set; }
        public string BID                       { get; set; }
        public string Belge_No                  { get; set; }
        public string Aciklama                  { get; set; }
        public int  Adet                          { get; set; }
        public decimal  m2                        { get; set; }
        public string  Durum                     { get; set; }

        public bool Onay_fl { get; set; }

        public bool Teslim_fl { get; set; }

        
    }
}