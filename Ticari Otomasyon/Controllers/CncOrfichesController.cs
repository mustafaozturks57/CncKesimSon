using Antlr.Runtime.Misc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Models;
using Ticari_Otomasyon.Models.Classes;
using Ticari_Otomasyon.Models.Classes.Views;
using Ticari_Otomasyon.Models.CncModel;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class CncOrfichesController : Controller
    {
        private CncTicariOtomasyonEntities db = new CncTicariOtomasyonEntities();

        // GET: CncOrfiches
        public async Task<ActionResult> Index()
        {

            var _db = await db.Database.SqlQuery<SiparisViewsModul>("select * from SiparisModuls ORDER BY Tarih DESC ").ToListAsync();
            return View(_db);
        }

        // GET: CncOrfiches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orfiche orfiche = db.Orfiches.Find(id);
            if (orfiche == null)
            {
                return HttpNotFound();
            }
            return View(orfiche);
        }

        
        public  JsonResult ItemsGet()
       {

           var query = db.Items.ToList();

            int totalRows = query.Count;
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortColumnDirection = Request["order[0][dir]"];

            //Filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.MODEL.ToLower().Contains(searchValue.ToLower())
                
              ).ToList();
            }

            int totalRowsAfterFiltering = query.Count;
            //Sorting

           
           // query = query.OrderBy(sortColumnName + " " + sortColumnDirection).ToList();

            //Paging
            query = query.Skip(start).Take(length).ToList();


            //int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //int skip = start != null ? Convert.ToInt32(start) : 0;


            return Json(new { data = query, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering },
                JsonRequestBehavior.AllowGet);


        }

        public JsonResult ItemsColorGet()


        {
            var query = db.ItemsColors.ToList();

            int totalRows = query.Count;
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortColumnDirection = Request["order[0][dir]"];

            //Filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.MALZEME.ToLower().Contains(searchValue.ToLower())

              ).ToList();
            }

            int totalRowsAfterFiltering = query.Count;
            //Sorting


            // query = query.OrderBy(sortColumnName + " " + sortColumnDirection).ToList();

            //Paging
            query = query.Skip(start).Take(length).ToList();


            //int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //int skip = start != null ? Convert.ToInt32(start) : 0;


            return Json(new { data = query, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering },
                JsonRequestBehavior.AllowGet);

        }
        

        [HttpGet]
        public JsonResult ItemsPrice(int id)


        {

            var  ıtems = db.Items.Where(x => x.ID == id).FirstOrDefault();




            return Json(ıtems, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ItemsPriceColor(int model )

           
        {

            string malzeme ="";


            if (malzeme.Replace("HG", null).Length>0)
            {
                var ıtems = db.Items.Where(x => x.ID == model).FirstOrDefault();




                return Json(ıtems.FIYATHG, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var ıtems = db.Items.Where(x => x.ID == model).FirstOrDefault();




                return Json(ıtems.FIYATMAT, JsonRequestBehavior.AllowGet);
            }


         

        }
      




         
        public ActionResult Delete(string id)
        {
            if (id != null)
            {

         

           DeleteOrficheAndOrfline(id);
                ViewBag.mesaj = "Kayıt Başarı İle Silinmiştir";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.mesaj = "Kayıt silerken hata alınmıştır";
                return RedirectToAction("Index");
            }
        }
        // GET: CncOrfiches/Create
        public ActionResult Create()

        {

            int SonNo = db.Database.SqlQuery<int>("select top 1  count(FisNo)+1 from Orfiche  ").FirstOrDefault();
            ViewBag.SonNo = SonNo;
            #region Dropdownlar

            

            List<SelectListItem> getCustomer = (from db in db.Customers.ToList()
                                        select new SelectListItem
                                        {
                                            Text = db.Fırma_Adı.ToString(),
                                            Value = db.id.ToString(),
                                        }

                                          ).ToList();
            ViewBag.Musteri = getCustomer;


            List<SelectListItem> getSıpDurum = (from db in db.SıpDurumu.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = db.SiparisDurumu.ToString(),
                                                    Value = db.id.ToString(),
                                                }

                                          ).ToList();
            ViewBag.getSıpDurum = getSıpDurum;

            List<SelectListItem> GetBolum = (from db in db.Bolumlers.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = db.BolumName.ToString(),
                                                    Value = db.id.ToString(),
                                                }

                                         ).ToList();
            ViewBag.GetBolum = GetBolum;

            List<SelectListItem> GetDepartman = (from db in db.Departments.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = db.Name.ToString(),
                                                 Value = db.Id.ToString(),
                                             }

                                        ).ToList();
            ViewBag.GetDepartman = GetDepartman;

            List<SelectListItem> GetISyeri = (from db in db.Isyeris.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = db.IsyeriName.ToString(),
                                                     Value = db.id.ToString(),
                                                 }

                                       ).ToList();
            ViewBag.GetISyeri = GetISyeri;

            List<SelectListItem> ozelkod = (from db in db.SpecialCodes.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = db.Name.ToString(),
                                                  Value = db.Id.ToString(),
                                              }

                                      ).ToList();
            ViewBag.ozelkod = ozelkod;


            


            #endregion

            return View();
        }

        // POST: CncOrfiches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(OrficheDto orficheDto, OrflineDto[] orflines)
        {
            var orficheInsert = InsertOrfiche(orficheDto);
            if (orficheInsert)
            {
                var orfline = InsertOrfline(orflines, orficheDto.FisNo);
                

                if (orfline)
                {
                    string query = "GetFarklıRenklerProc @sipno='"+orficheDto.FisNo+"' ,@pTranType=5";

                   var message =  db.Database.SqlQuery<string>(query).FirstOrDefault();

                    return Json(message, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                ViewBag.Error = "Sipariş Kaydedilemedi.";
            }

            return Json("Sipariş Kayıt Ederken Sorun Oluştu", JsonRequestBehavior.AllowGet);
        }

        // GET: CncOrfiches/Edit/5
        public ActionResult Edit(string id)
        {
            #region Dropdownlar

            #region Mertİleİncele multicolumndropdown Kullanılabilir
            //var test2 = db.Items.Join(db.ItemsColors, x => x.ID, Y => Y.ItemsID, (c, p) => 
            //new
            //{
            //    ProductName = p.Items,
            //    CategoryName = c.ItemsColor.ColorName

            //});


            //var test = from p in db.Items
            //           join c in db.ItemsColors on p.ID equals c.ItemsID
            //             select new
            //             {
            //                 ProductName = db.Items,
            //                 CategoryName = c.ColorName
            //             };


            //ViewBag.GetCombo = test;

#endregion


            List<SelectListItem> getCustomer = (from db in db.Customers.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = db.Fırma_Adı.ToString(),
                                                    Value = db.id.ToString(),
                                                }

                                          ).ToList();
            ViewBag.Musteri = getCustomer;


            List<SelectListItem> getSıpDurum = (from db in db.SıpDurumu.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = db.SiparisDurumu.ToString(),
                                                    Value = db.id.ToString(),
                                                }

                                          ).ToList();
            ViewBag.getSıpDurum = getSıpDurum;

            List<SelectListItem> GetBolum = (from db in db.Bolumlers.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = db.BolumName.ToString(),
                                                 Value = db.id.ToString(),
                                             }

                                         ).ToList();
            ViewBag.GetBolum = GetBolum;

            List<SelectListItem> GetDepartman = (from db in db.Departments.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = db.Name.ToString(),
                                                     Value = db.Id.ToString(),
                                                 }

                                        ).ToList();
            ViewBag.GetDepartman = GetDepartman;

            List<SelectListItem> GetISyeri = (from db in db.Isyeris.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = db.IsyeriName.ToString(),
                                                  Value = db.id.ToString(),
                                              }

                                       ).ToList();
            ViewBag.GetISyeri = GetISyeri;

            List<SelectListItem> ozelkod = (from db in db.SpecialCodes.ToList()
                                            select new SelectListItem
                                            {
                                                Text = db.Name.ToString(),
                                                Value = db.Id.ToString(),
                                            }

                                      ).ToList();
            ViewBag.ozelkod = ozelkod;

            List<SelectListItem> Items = (from db in db.Items.ToList()
                                            select new SelectListItem
                                            {
                                                Text = db.MODEL.ToString(),
                                                Value = db.MODEL.ToString(),
                                            }

                                     ).ToList();
            ViewBag.Items = Items;


            List<SelectListItem> ItemsColor = (from db in db.ItemsColors.ToList()
                                          select new SelectListItem
                                          {
                                              Text = db.MALZEME.ToString(),
                                              Value = db.MALZEME.ToString(),
                                          }

                                     ).ToList();
            ViewBag.ItemsColor = ItemsColor;





            #endregion

            OrficheUpdateDto orficheUpdateDto = new OrficheUpdateDto();

            var a = db.Orfiches.Where(x => x.FisNo == id).First();
            orficheUpdateDto.Orfiche = a;
            
         
            ViewBag.TerminTarihi = Convert.ToDateTime(a.TerminTarihi).ToString("yyyy-MM-dd");
            ViewBag.Tarih = Convert.ToDateTime(a.Tarih).ToString("yyyy-MM-dd");

            var b = db.Orflines.Where(x => x.OrficheNo == id);
            orficheUpdateDto.Orflines = b.ToList();

            return View(orficheUpdateDto);
        }

       
        [HttpPost]        
        public ActionResult Edit( Orfiche orfiche, Orfline[] orflines)
     {
            var deleteFicheStatus = DeleteOrficheAndOrfline(orfiche.FisNo);
            if (deleteFicheStatus)
            {
                var orficheInsert = EditOrfiche(orfiche);
                if (orficheInsert)
                {
                    var orfline = EditOrfline(orflines, orfiche.FisNo);


                    if (orfline)
                    {
                        string query = "GetFarklıRenklerProc @sipno='" + orfiche.FisNo + "' ,@pTranType=5";

                        db.Database.ExecuteSqlCommand(query);
                        return Json("Kayıt Başarılı bir şekilde Değiştirilmiştir",JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Bir Sorun Oluştu Sistem Yöneticisine Başvurunuz", JsonRequestBehavior.AllowGet);
                }
            }


            return Json("Bir Sorun Oluştu Sistem Yöneticisine Başvurunuz", JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public bool InsertOrfiche(OrficheDto orficheDto)
        {

            Orfiche Aktarım = new Orfiche();

            Aktarım.MusteriId = orficheDto.MusteriId;
         Aktarım.Tarih   =           orficheDto.Tarih                 ;
         Aktarım.FisNo            =   orficheDto.FisNo                ;
         Aktarım.BarkodNo         =   orficheDto.BarkodNo             ;
         Aktarım.SiparisSekli     =   orficheDto.SiparisSekli         ;
         Aktarım.BID              =   orficheDto.BID                  ;
         Aktarım.Aciklama         =   orficheDto.Aciklama             ;
         Aktarım.TerminTarihi     =   orficheDto.TerminTarihi         ;
         Aktarım.BelgeNo          =   orficheDto.BelgeNo              ;
         Aktarım.OzelKod          =   orficheDto.OzelKod              ;
         Aktarım.Bölüm            =   orficheDto.Bölüm                ;
         Aktarım.Departman        =   orficheDto.Departman            ;
         Aktarım.Isyeri           =   orficheDto.Isyeri               ;
         Aktarım.SiparisDurumu    =   orficheDto.SiparisDurumu        ;
         Aktarım.ToplamIndirim    =   orficheDto.ToplamIndirim        ;
         Aktarım.TasarimTutari    =   orficheDto.TasarimTutari        ;
         Aktarım.KargoTutari      =   orficheDto.KargoTutari          ;
         Aktarım.Toplam           =   orficheDto.Toplam               ;
            Aktarım.NetTutar = orficheDto.NetTutar;

            db.Orfiches.Add(Aktarım);



            SıparısAppPool app = new SıparısAppPool();
            
            app.SipID = orficheDto.FisNo;
            app.PaletNo = null;
            app.Palet_fl = false;
            app.Pres_fl = false;
            app.Paket_fl = false;
            app.Toplamİndirim = Convert.ToDecimal( orficheDto.ToplamIndirim);
            app.TasarimTutari = Convert.ToDecimal(orficheDto.TasarimTutari);
            app.KargoTutari = Convert.ToDecimal(orficheDto.KargoTutari);
            app.NetTutar = Convert.ToDecimal(orficheDto.NetTutar);
            app.Kesim_fl = false;
            app.Teslim_fl = false;
            app.Onay_fl = false;
            db.SıparısAppPool.Add(app);
            db.SaveChanges();


            return true;

        }
        public bool EditOrfiche(Orfiche orfiche)
        {


            Orfiche Aktarım = new Orfiche();

            Aktarım.MusteriId = orfiche.MusteriId;
            Aktarım.Tarih = orfiche.Tarih;
            Aktarım.FisNo = orfiche.FisNo;
            Aktarım.BarkodNo = orfiche.BarkodNo;
            Aktarım.SiparisSekli = orfiche.SiparisSekli;
            Aktarım.BID = orfiche.BID;   
            Aktarım.Aciklama = orfiche.Aciklama;
            Aktarım.TerminTarihi = orfiche.TerminTarihi;
            Aktarım.BelgeNo = orfiche.BelgeNo;
            Aktarım.OzelKod = orfiche.OzelKod;
            Aktarım.Bölüm = orfiche.Bölüm;
            Aktarım.Departman = orfiche.Departman;
            Aktarım.Isyeri = orfiche.Isyeri;
            Aktarım.SiparisDurumu = orfiche.SiparisDurumu;
            Aktarım.ToplamIndirim = orfiche.ToplamIndirim;
            Aktarım.TasarimTutari = orfiche.TasarimTutari;
            Aktarım.KargoTutari = orfiche.KargoTutari;
            Aktarım.Toplam = orfiche.Toplam;
            Aktarım.NetTutar = orfiche.NetTutar;

            db.Orfiches.Add(Aktarım);

            SıparısAppPool app = new SıparısAppPool();

            app.SipID = orfiche.FisNo;
            app.PaletNo = null;
            app.Palet_fl = false;
            app.Pres_fl = false;
            app.Paket_fl = false;
            app.Toplamİndirim = Convert.ToDecimal(orfiche.ToplamIndirim);
            app.TasarimTutari = Convert.ToDecimal(orfiche.TasarimTutari);
            app.KargoTutari = Convert.ToDecimal(orfiche.KargoTutari);
            app.NetTutar = Convert.ToDecimal(orfiche.NetTutar);
            app.Kesim_fl = false;
            app.Teslim_fl = false;
            db.SıparısAppPool.Add(app);
            db.SaveChanges();



            db.SaveChanges();


            return true;

        }
        public bool InsertOrfline(OrflineDto[] orflines, string orficheId)
        {

            if (orflines != null)
            {


                foreach (var line in orflines)
                {
                    if (line.MalzemeKodu !=null)
                    {

                        Orfline offs = new Orfline();
                        offs.OrficheNo=orficheId;
                        offs.MalzemeKodu=line.MalzemeKodu;
                        offs.Model = line.Model;
                        offs.Ozellik = line.Ozellik;
                        offs.Boy = (decimal?)line.Boy;
                        offs.En = (decimal?)line.En;
                        offs.Adet = line.Adet;
                        offs.M2 = (decimal?)line.M2;
                        offs.M2Fiyat = (decimal?)line.M2Fiyat;
                        offs.Fiyat = (decimal?)line.Fiyat;

                        db.Orflines.Add(offs);
                        db.SaveChanges();
                        //string sqlString = "INSERT INTO Orfline VALUES ('" + orficheId + "','" + Convert.ToDecimal(line.MalzemeKodu) + "','" + Convert.ToDecimal(line.Model) + "','" + Convert.ToDecimal(line.Ozellik) + "','" + Convert.ToDecimal(line.Boy) + "','" + Convert.ToDecimal(line.En) + "','" + Convert.ToDecimal(line.Adet) + "','" + Convert.ToDecimal(line.M2) + "','" + Convert.ToDecimal(line.M2Fiyat) + "','" + Convert.ToDecimal(line.Fiyat) + "')";

                        //var sqlQuery = db.Database.ExecuteSqlCommand(sqlString);


                    }
                }

                return true;
            }
            return false;
        }

        public bool EditOrfline(Orfline[] orflines, string orficheId)
        {

            if (orflines != null)
            {
                foreach (var line in orflines)
                {
                    if (line.MalzemeKodu != null)
                    {

                        Orfline offs = new Orfline();
                        offs.OrficheNo = orficheId;
                        offs.MalzemeKodu = line.MalzemeKodu;
                        offs.Model = line.Model;
                        offs.Ozellik = line.Ozellik;
                        offs.Boy = (decimal?)line.Boy;
                        offs.En = (decimal?)line.En;
                        offs.Adet = line.Adet;
                        offs.M2 = (decimal?)line.M2;
                        offs.M2Fiyat = (decimal?)line.M2Fiyat;
                        offs.Fiyat = (decimal?)line.Fiyat;

                        db.Orflines.Add(offs);
                        db.SaveChanges();
                        //string sqlString = "INSERT INTO Orfline VALUES ('" + orficheId + "','" + Convert.ToDecimal(line.MalzemeKodu) + "','" + Convert.ToDecimal(line.Model) + "','" + Convert.ToDecimal(line.Ozellik) + "','" + Convert.ToDecimal(line.Boy) + "','" + Convert.ToDecimal(line.En) + "','" + Convert.ToDecimal(line.Adet) + "','" + Convert.ToDecimal(line.M2) + "','" + Convert.ToDecimal(line.M2Fiyat) + "','" + Convert.ToDecimal(line.Fiyat) + "')";

                        //var sqlQuery = db.Database.ExecuteSqlCommand(sqlString);


                    }
                }

                return true;
            }
            return false;
        }
        public bool DeleteOrficheAndOrfline(string ficheNo)
        {


            string sqlString = "DELETE FROM Orfiche WHERE FisNo='" + ficheNo + "';DELETE FROM Orfline WHERE OrficheNo='" + ficheNo + "';DELETE FROM SıparısAppPool WHERE SipID='" + ficheNo +"';";


            var sqlQuery = db.Database.ExecuteSqlCommand(sqlString);

          



            return true;
        }

        public JsonResult OnlyOnay(string id)
        {

            var find = db.Orfiches.Where(x => x.FisNo == id).ToList();
            if (find.Count==0)
            {
                return Json("Bu Evrak Kayıt Edilmemeiştir Lütfen Önce Kayıt Ediniz.", JsonRequestBehavior.AllowGet);

            }
            else
            {
                string quer = "update SıparısAppPool SET Onay_fl=1 where SipID='"+id+"'";
                db.Database.ExecuteSqlCommand(quer);
                return Json("Onaylama Başarılı Kesim İşlemine Devam Edebilir siniz .", JsonRequestBehavior.AllowGet);

            }


           
        }
    }
}
