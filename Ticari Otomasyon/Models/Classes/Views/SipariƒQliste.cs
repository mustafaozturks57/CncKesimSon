using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticari_Otomasyon.Models.Classes.Views
{
    public class SiparişQliste
    {
 
        public string  SipID  { get; set; }

        public bool Onay_fl { get; set; }
        public bool Kesim_fl                                  { get; set; }
public int?  PaletNo                                 { get; set; }
public bool  Palet_fl                                { get; set; }
public bool Pres_fl                                 { get; set; }
public bool Paket_fl                                { get; set; }
public string  MusterıAdı                          { get; set; }
public string SiparisDurumu                       { get; set; }
public DateTime  Tarih                                  { get; set; }
public DateTime TerminTarihi                           { get; set; }
public string Aciklama                            { get; set; }
public string StokAdı                             { get; set; }
public string RenkAdı                             { get; set; }
public int  Adet                                    { get; set; }
public decimal  Fiyat                               { get; set; }

        public decimal m2 { get; set; }



    }
}