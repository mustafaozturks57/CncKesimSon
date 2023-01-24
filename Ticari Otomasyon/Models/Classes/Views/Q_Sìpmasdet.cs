using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.CncModel
{
    public class Q_Sıpmasdet
    {

         public string FisNo              { get; set; }
         public string Fırma_Adı          { get; set; }
         public DateTime Tarih              { get; set; }
         public DateTime TerminTarihi       { get; set; }
         public string BarkodNo           { get; set; }
         public string SiparisSekli       { get; set; }
         public int SiparisDurumu      { get; set; }
         public double Fiyat              { get; set; }
         public double M2FIYAT            { get; set; }
         public int ToplamAdet         { get; set; }



        public int MalzemeKodu { get; set; }

        public string Ozellik    { get; set; }



 

    }
}