using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models.Classes.Views;
using Ticari_Otomasyon.Models.CncModel;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class CncPaletController : Controller
    {
        // GET: CncPalet
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();
        public async Task<ActionResult> Index()
        {


            var Views = await db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where Kesim_fl=1  and Palet_fl=0 and and Onay_fl=1 ").ToListAsync();

            return View(Views);
        }








        [HttpGet]



        public ActionResult PressIndex(string PaletNo)
        {


            if (PaletNo != null)

            {
                string sorgumuz = "select * from SıparısAppPoolView  where Kesim_fl=1 and palet_fl=1 and PaletNo={0}";

                var find = db.Database.SqlQuery<SiparişQliste>(sorgumuz, PaletNo).ToList();

                if (find.Count == 0)
                {
                    string query = "select * from SıparısAppPoolView   where Kesim_fl=1 and palet_fl=1 and PaletNo={0}";



                    var Views = db.Database.SqlQuery<SiparişQliste>(query, PaletNo).ToList();
                    return View(Views);
                }
                else
                {
                    string query = "select * from SıparısAppPoolView   where Kesim_fl=1 and palet_fl=1 and PaletNo={0}";
                    string quert2tablo = "select * from SıparısAppPoolView   where   RenkAdı={0}";
                    var findRenk = db.Database.SqlQuery<SiparişQliste>(query, PaletNo).FirstOrDefault();
                    var views2tablo = db.Database.SqlQuery<SiparişQliste>(quert2tablo, findRenk.RenkAdı).ToList();
                    ViewBag.alwayas = views2tablo;
                    ViewBag.sıp = findRenk.PaletNo;
                    ViewBag.renk = findRenk.RenkAdı;


                    var Views = db.Database.SqlQuery<SiparişQliste>(query, PaletNo).ToList();
                    return View(Views);
                }
            }

            else
            {


                string queryTable = "select * from SıparısAppPoolView where  Kesim_fl=1 and Onay_fl=1 and   palet_fl=1 and PaletNo>0 "; ;



                var Viewscol = db.Database.SqlQuery<SiparişQliste>(queryTable).ToList();
                return View(Viewscol);
            }


        }


        [HttpPost]
        public JsonResult PaletOnay(string sipno, string paletno)
        {

            var kayıt = db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where Kesim_fl=1 and Palet_fl = 0 and SipID = {0} ", sipno).ToList();

            if (kayıt.Count == 0)
            {
                return Json("Üzgünüm Sipariş Bulunamadı");

            }
            else
            {





                string query = "update SıparısAppPool set  PaletNo='" + paletno + "' where SipID = '" + sipno + "' and Kesim_fl=1  and Palet_fl=0";

                db.Database.ExecuteSqlCommand(query);

                string alert = sipno + " Numaralı Siparişe Palet Numarası Tanımlanmıştır , Palet No :  " + paletno;

                return Json(alert);
            }

        }
        [HttpPost]
        public JsonResult PaletYazdır(string sipno)
        {

            var kayıt = db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where Kesim_fl=1 and PaletNo>0 and SipID = '"+sipno+"' ").ToList();

            if (kayıt.Count == 0)
            {
                return Json("Üzgünüm Sipariş onaylanmamıştır, Sipariş Mevcut Olmayabilir veya PAlet numarası Verilmemiş olabilir lütfen kontrol ediniz");

            }
            else
            {


                string query = "update SıparısAppPool set   Palet_fl=1  where SipID = '" + sipno + "' and Kesim_fl=1  and Palet_fl=0";

                db.Database.ExecuteSqlCommand(query);

                string alert = sipno + " Numaralı Siparişe Palet Onaylanmıştır ";

                return Json(alert);
            }

        }

        [HttpPost]
        public JsonResult PaletYazdırAndOnay(string sipno,string paletno)
        {

            var kayıt = db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where Kesim_fl=1   and SipID = '" + sipno + "' ").ToList();

            if (kayıt.Count == 0)
            {
                return Json("Üzgünüm Sipariş onaylanmamıştır. Bir Hata Oluştur");

            }
            else
            {


                string query = "update SıparısAppPool set   Palet_fl=1,PaletNo='"+paletno+"'  where SipID = '" + sipno + "' and Kesim_fl=1  and Palet_fl=0";

                db.Database.ExecuteSqlCommand(query);

                string alert = sipno + " Numaralı Sipariş Onaylanmıştır ve Palet Numarası Verilmiştir Palet No : "+paletno;

                return Json(alert);
            }

        }

        [HttpPost]
        public JsonResult CncKesimKısayol(string sipno)
        {
            var find = db.SıparısAppPool.Where(x => x.SipID == sipno).ToList();

            if (find.Count == 0)
            {
                return Json("Üzgünüm Sipariş Bulunamadı");
            }
            else
            {


                string query = "update SıparısAppPool set Kesim_fl=1 where SipID=" + sipno;

                db.Database.ExecuteSqlCommand(query);

                string alert = "Sipariş Kesim Yapılmıştır Sipariş Numarası  : " + sipno;
                return Json(alert);
            }

        }

        [HttpPost]
        public JsonResult PaleteEkle(string sipno, string paletno)
        {
            var find = db.SıparısAppPool.Where(x => x.SipID == sipno).ToList();

            if (find.Count == 0)
            {
                return Json("Üzgünüm Sipariş Bulunamadı");
            }
            else
            {


                string query = "update SıparısAppPool set Palet_fl=1,PaletNo=" + paletno + " where SipID=" + sipno;

                db.Database.ExecuteSqlCommand(query);

                string alert = "Sipariş Palete Yüklenmiştir Sipariş Numarası  : " + sipno + "  Palet Numarası :" + paletno;
                return Json(alert);
            }

        }
        [HttpPost]

        public JsonResult xx(string sipno)
        {
            var find = db.SıparısAppPool.Where(x => x.SipID == sipno).ToList();

            if (find.Count == 0)
            {
                return Json("Üzgünüm Sipariş Bulunamadı");
            }
            else
            {


                string query = "update SıparısAppPool set Kesim_fl=0,Palet_fl=0,PaletNo=0  where SipID=" + sipno;

                db.Database.ExecuteSqlCommand(query);

                string alert = "Sipariş Kesim İptal Edilmiştir , Sipariş Numarası  : " + sipno;
                return Json(alert);
            }

        }
        [HttpPost]
        public JsonResult Kesimİptal(string sipno)
        {
            var find = db.SıparısAppPool.Where(x => x.SipID == sipno).ToList();

            if (find.Count == 0)
            {
                return Json("Üzgünüm Sipariş Bulunamadı");
            }
            else
            {


                string query = "update SıparısAppPool set Kesim_fl=0,Palet_fl=0,PaletNo=0  where SipID=" + sipno;

                db.Database.ExecuteSqlCommand(query);

                string alert = "Sipariş Kesim İptal Edilmiştir , Sipariş Numarası  : " + sipno;
                return Json(alert);
            }

        }


        [HttpPost]
        public JsonResult KesimVePaletNo(string sipno, string paletno)
        {
            var find = db.SıparısAppPool.Where(x => x.SipID == sipno).ToList();

            if (find.Count == 0)
            {
                return Json("Üzgünüm Sipariş Bulunamadı");
            }
            else
            {


                string query = "update SıparısAppPool set Kesim_fl=1,Palet_fl=1,PaletNo=" + paletno + "  where SipID=" + sipno;

                db.Database.ExecuteSqlCommand(query);

                string alert = "Sipariş Kesim İptal Edilmiştir , Sipariş Numarası  : " + sipno;
                return Json(alert);
            }

        }



        [HttpPost]
        public JsonResult PaletNoBul(string renk)
        {
            var rengım = db.Database.SqlQuery<SiparişQliste>("select * from SıparısAppPoolView where SipID='"+renk+"'").FirstOrDefault();

           string querys = "SELECT PaletNo FROM SıparısAppPoolView where palet_fl=1 and PaletNo IS NOT NULL AND RenkAdı='"+rengım.RenkAdı+"' GROUP BY PaletNo";
           var Sayı =  db.Database.SqlQuery<int>(querys, renk).ToList();

            if (Sayı.Count==0)
            {
                return Json("Renk e ait paletno bulunamadı ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Sayı, JsonRequestBehavior.AllowGet);
            }

            



        }









    }
}