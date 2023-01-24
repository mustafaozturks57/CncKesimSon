using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.Classes.HomeClass
{
    public class Top6Siparis
    {

        public string MusterıAdı { get; set; }

        public string SipID { get; set; }

        public decimal m2 { get; set; }

        public bool Kesim_fl { get; set; }
        public bool Palet_fl { get; set; }
        public bool Pres_fl { get; set; }
        public bool Paket_fl { get; set; }
        public bool Teslim_fl { get; set; }
        
    }
}