//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ticari_Otomasyon.Models.CncModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orfline
    {
        public int Id { get; set; }
        public string OrficheNo { get; set; }
        public string MalzemeKodu { get; set; }
        public string Model { get; set; }
        public string Ozellik { get; set; }
        public Nullable<decimal> Boy { get; set; }
        public Nullable<decimal> En { get; set; }
        public Nullable<int> Adet { get; set; }
        public Nullable<decimal> M2 { get; set; }
        public Nullable<decimal> M2Fiyat { get; set; }
        public Nullable<decimal> Fiyat { get; set; }
    }
}