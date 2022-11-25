using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.CncModel
{
    public class OrficheDto
    {
        public int MusteriId { get; set; }
        public DateTime Tarih { get; set; }
        public string FisNo { get; set; }
        public string BarkodNo { get; set; }
        public string SiparisSekli { get; set; }
        public string BID { get; set; }
        public string Aciklama { get; set; }
        public DateTime TerminTarihi { get; set; }
        public string BelgeNo { get; set; }
        public int OzelKod { get; set; }
        public int Bölüm { get; set; }
        public int Departman { get; set; }
        public int Isyeri { get; set; }
        public int SiparisDurumu { get; set; }
    }
}