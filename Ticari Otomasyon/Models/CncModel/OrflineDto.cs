namespace Ticari_Otomasyon.Models.CncModel
{
    public class OrflineDto

    {
       
        public string OrficheNo{ get; set; }
       
        public int MalzemeKodu { get; set; }
        public string MalzemeAdi { get; set; }
        public string Ozellik { get; set; }
        public double Boy { get; set; }
        public double En { get; set; }
        public int Adet { get; set; }
        public double M2 { get; set; }
        public double M2Fiyat { get; set; }
        public double Fiyat { get; set; }
    }
}