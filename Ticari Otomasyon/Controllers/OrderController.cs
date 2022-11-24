using JQDT.MVC;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Ticari_Otomasyon.Core;
using Ticari_Otomasyon.Models;
using Ticari_Otomasyon.Models.Classes;
using Ticari_Otomasyon.Roles;
using Ticari_Otomasyon.ServerSideProcess;

namespace Ticari_Otomasyon.Controllers
{
    [AllowAnonymous]
    public class OrderController : Controller
    {
        // GET: Order
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult OrderList(string year)
        {
       
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " SELECT Left(Date,10) Date, " +
                    " FicheNo FicheNo, " +
                    " LEFT(C.CustomerDefinition,30) CustomerDefinition, " +
                    " SpecialCode SpecialCode, " +
                    " ISNULL(TR.GCODE,'') TradingGrp,  " +
                    " Department Department, " +
                    " CapiDiv CapiDiv," +
                    " Whouse Whouse," +
                    " CONVERT(VARCHAR,NetTotal) NetTotal " +
                    " FROM TB_ORFICHE T " +
                     " OUTER APPLY(SELECT DEFINITION_ CustomerDefinition FROM LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD WHERE T.CustomerCode=LOGICALREF) C  " +
                     " OUTER APPLY(SELECT GCODE GCODE FROM L_TRADGRP WHERE T.TradingGrp=LOGICALREF) TR " +
                     " WHERE CONVERT(VARCHAR,YEAR(CONVERT(DATE,T.Date,104)))='" + year+"'";
                     //" ORDER BY T.Id ASC ";


                var sqlQuery = context.Database.SqlQuery<OrderListModel>(sqlString);
                var query = sqlQuery.OrderBy(X => X.FicheNo).ToList();

                int totalRows = query.Count;
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortColumnDirection = Request["order[0][dir]"];

                ////Filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => x.FicheNo.ToLower().Contains(searchValue.ToLower()) ||
                    x.Date.ToLower().Contains(searchValue.ToLower()) ||
                    x.SpecialCode.ToLower().Contains(searchValue.ToLower()) ||
                    x.TradingGrp.ToLower().Contains(searchValue.ToLower()) ||
                    x.CustomerDefinition.ToLower().Contains(searchValue.ToLower())).ToList();

                }

                int totalRowsAfterFiltering = query.Count;
                //Sorting


                query = query.OrderBy(sortColumnName + " " + sortColumnDirection).ToList();

                //Paging
                query = query.Skip(start).Take(length).ToList();


                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;


                return Json(new { data = query, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering },
                    JsonRequestBehavior.AllowGet);



            };
        }
        [HttpGet]
        public ActionResult GetOrder(string data)
        {
            GetOrderView getOrderView = new GetOrderView();


            ViewBag.ficheNo = data;
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " SELECT                                                                                                                               " +
                                    "   FicheNo FicheNo, 																												   " +
                                    " 	Left(Date,10) Date,  																											   " +
                                    " 	Docode Docode,																													   " +
                                    " 	C.CustomerCode CustomerCode,																									   " +
                                    " 	C.CustomerDefinition CustomerDefinition,																						   " +
                                    " 	CyphCode CyphCode,																												   " +
                                    " 	Department.Name Department, 																									   " +
                                    " 	CapiDiv.Name CapiDiv,																											   " +
                                    " 	Whouse.Name Whouse,																												   " +
                                    " 	SpecialCode SpecialCode, 																										   " +
                                    " 	ISNULL(TR.GCODE,'') TradingGrp,  																								   " +
                                    " 	SLS.Name SalesMan,																												   " +
                                    " 	CONVERT(VARCHAR,TotalDiscount) TotalDiscount,																					   " +
                                    " 	CONVERT(VARCHAR,TotalVat) TotalVat,																								   " +
                                    " 	CONVERT(VARCHAR,Total) Total,																									   " +
                                    " 	CONVERT(VARCHAR,NetTotal) NetTotal 																								   " +
                                    " 																																	   " +
                                    " FROM TB_ORFICHE T 																												   " +
                                    " OUTER APPLY(SELECT DEFINITION_ CustomerDefinition,CODE CustomerCode FROM LG_010_CLCARD WHERE T.CustomerCode=LOGICALREF) C  		   " +
                                    " OUTER APPLY(SELECT GCODE GCODE FROM L_TRADGRP WHERE T.TradingGrp=LOGICALREF) TR													   " +
                                    " OUTER APPLY(SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIDEPT WHERE FIRMNR=10 AND T.Department=NR) Department					   " +
                                    " OUTER APPLY(SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIDIV WHERE FIRMNR=10 AND T.Department=NR) CapiDiv						   " +
                                    " OUTER APPLY(SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIWHOUSE WHERE FIRMNR=10 AND T.Department=NR) Whouse						   " +
                                    " OUTER APPLY(SELECT Convert(INT,LOGICALREF) Nr,CODE Name FROM LG_SLSMAN WHERE FIRMNR=10 AND  ACTIVE=0 AND T.SalesMan=LOGICALREF) SLS " +
                                    " WHERE T.Ficheno='"+data+"'";

                var sqlQuery = context.Database.SqlQuery<GetOrderTop>(sqlString);
                getOrderView.GetOrderTop_ = sqlQuery.First();


                string sqlString2 = " SELECT                            " +
                                    " 	barcode barcode,				" +
                                    " 	itemCode itemCode,				" +
                                    " 	itemName itemName,				" +
                                    " 	CONVERT(VARCHAR,amount)  amount,					" +
                                    " 	mainUnit mainUnit,				" +
                                    " 	CONVERT(VARCHAR,price) price,					" +
                                    " 	CONVERT(VARCHAR,vat) vat,						" +
                                    " 	CONVERT(VARCHAR,Ind1) Ind1,						" +
                                    " 	CONVERT(VARCHAR,Ind2) Ind2,						" +
                                    " 	CONVERT(VARCHAR,Ind3) Ind3,						" +
                                    " 	CONVERT(VARCHAR,Ind4) Ind4,						" +
                                    " 	CONVERT(VARCHAR,Ind5) Ind5,						" +
                                    " 	CONVERT(VARCHAR,total) total,					" +
                                    " 	CONVERT(VARCHAR,netTotal) netTotal				" +
                                    " FROM TB_ORFLINE WHERE Ficheno='"+data+"'";

                var sqlQuery2 = context.Database.SqlQuery<GetOrderEnd>(sqlString2);
                List<GetOrderEnd> getOrderEnds = sqlQuery2.ToList();
                getOrderView.GetOrderEnd_ = getOrderEnds;

                return View(getOrderView);
            }

        }
        [HttpGet]
        public ActionResult AddOrder()
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlFicheNo = "DECLARE @FICHENO VARCHAR(100)='ENT'+CONVERT(VARCHAR,YEAR(GETDATE())); SELECT TOP 1 @FICHENO+RIGHT('000000'+CONVERT(VARCHAR,((CONVERT(INT,RIGHT(Ficheno,6)))+1)),6) FicheNo FROM TB_ORFICHE ORDER BY 1 DESC ";



                var ficheNoSql = context.Database.SqlQuery<OrderFicheNo>(sqlFicheNo).ToList();
                var value = ficheNoSql.SingleOrDefault();

                ViewBag.ficheNo = value.FicheNo;
                //-----------------------------------------------------------------------//
                string sqlString = "SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIDEPT WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery = context.Database.SqlQuery<OrderCapiDept>(sqlString).ToList();

                List<SelectListItem> capiDeptList = (from i in sqlQuery
                                                     select new SelectListItem
                                                     {
                                                         Text = i.Name,
                                                         Value = i.Nr.ToString()

                                                     }).ToList();


                ViewBag.capiDept = capiDeptList;
                //---------------------------------------------//
                string sqlString2 = "SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIDIV WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery2 = context.Database.SqlQuery<OrderCapiDiv>(sqlString2).ToList();

                List<SelectListItem> capiDivList = (from i in sqlQuery2
                                                    select new SelectListItem
                                                    {
                                                        Text = i.Name,
                                                        Value = i.Nr.ToString()

                                                    }).ToList();


                ViewBag.capiDiv = capiDivList;
                //---------------------------------------------//
                string sqlString3 = "SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIWHOUSE WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery3 = context.Database.SqlQuery<OrderCapiDiv>(sqlString3).ToList();

                List<SelectListItem> capiWhouseList = (from i in sqlQuery3
                                                       select new SelectListItem
                                                       {
                                                           Text = i.Name,
                                                           Value = i.Nr.ToString()

                                                       }).ToList();


                ViewBag.capiWhouse = capiWhouseList;
                //---------------------------------------------//
                string sqlString4 = "SELECT CONVERT(INT,LOGICALREF) NR,DEFINITION_ NAME FROM LG_SLSMAN WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery4 = context.Database.SqlQuery<OrderSalesMan>(sqlString4).ToList();

                List<SelectListItem> capiSalesman = (from i in sqlQuery4
                                                     select new SelectListItem
                                                     {
                                                         Text = i.Name,
                                                         Value = i.Nr.ToString()

                                                     }).ToList();
                capiSalesman.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""
                });

                ViewBag.salesMan = capiSalesman;

                //---------------------------------------------//
                string sqlString5 = "SELECT CONVERT(INT,LOGICALREF) NR,GCODE NAME FROM L_TRADGRP WHERE ACTIVE=0";



                var sqlQuery5 = context.Database.SqlQuery<OrderTradingGrp>(sqlString5).ToList();

                List<SelectListItem> tradingGrps = (from i in sqlQuery5
                                                    select new SelectListItem
                                                    {
                                                        Text = i.Name,
                                                        Value = i.Nr.ToString()

                                                    }).ToList();
                tradingGrps.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""
                });

                ViewBag.tradingGrp = tradingGrps;
                //---------------------------------------------//
                string sqlString6 = "SELECT CONVERT(INT,LOGICALREF) Nr,CODE Code FROM LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD WHERE  ACTIVE=0 AND CARDTYPE<>22";



                var sqlQuery6 = context.Database.SqlQuery<OrderCustomerCode>(sqlString6).ToList();

                List<SelectListItem> customersCode = (from i in sqlQuery6
                                                      select new SelectListItem
                                                      {
                                                          Text = i.Code,
                                                          Value = i.Nr.ToString()

                                                      }).ToList();
                customersCode.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""
                });

                ViewBag.customerCode = customersCode;





                string sqlString7 = "SELECT CONVERT(INT,LOGICALREF) Nr,LEFT(DEFINITION_,20) Definition FROM LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD WHERE ACTIVE=0 AND CARDTYPE<>22";



                var sqlQuery7 = context.Database.SqlQuery<OrderCustomerDefinition>(sqlString7).ToList();

                List<SelectListItem> customersDefs = (from i in sqlQuery7
                                                      select new SelectListItem
                                                      {
                                                          Text = i.Definition,
                                                          Value = i.Nr.ToString()

                                                      }).ToList();
                customersDefs.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""

                });

                ViewBag.customerDefs = customersDefs;
            }

            return View();
        }
        [HttpPost]
        public ActionResult AddOrder(GetOrderTop getOrder, GetOrderEnd[] kalemler)
        {
           
            if (TableValidate())
            {
                var date_ = DateTime.Now.ToShortDateString();
                var orfiche = InsertOrfiche(getOrder, date_);
                if (orfiche)
                {
                    var orfline = InsertOrfline(kalemler, getOrder.FicheNo);


                    if (orfline)
                    {
                        ViewBag.Succes = "Sipariş Kaydedildi.";
                    }
                }
                else
                {
                    ViewBag.Error = "Sipariş Kaydedilemedi.";
                }
                
            }



            return Json("");
        }
        public ActionResult GetItems(string barcode)

        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {

                if (string.IsNullOrEmpty(barcode) != true)
                {
                    string sqlString = "SELECT I.CODE Code,I.NAME Name,CONVERT(VARCHAR,I.Vat) Vat,U.NAME MainUnit,CONVERT(VARCHAR,P.PRICE) Price FROM LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS I  LEFT JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_UNITBARCODE B ON B.ITEMREF=I.LOGICALREF " +
                        " LEFT JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_UNITSETL U ON U.UNITSETREF=I.UNITSETREF AND MAINUNIT=1 LEFT JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_PRCLIST P ON P.CARDREF=I.LOGICALREF AND P.PTYPE=2 AND P.ACTIVE=0 " +
                        "WHERE B.BARCODE = '" + barcode + "'";


                    var sqlQuery = context.Database.SqlQuery<OrderGetItem>(sqlString);
                    var a = sqlQuery.FirstOrDefault();

                    return Json(a, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }
      
        [HttpPost]
        public JsonResult ItemList()
        {

            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " SELECT                                                                                                    " +
                                    " I.LOGICALREF Logicalref,																					" +
                                    " CASE I.CARDTYPE WHEN 1 THEN 'TM' 																			" +
                                    " WHEN 2 THEN 'KK' 																							" +
                                    " WHEN 3 THEN 'DM' 																							" +
                                    " WHEN 4 THEN 'SK'																							" +
                                    " WHEN 10 THEN 'HM'																							" +
                                    " WHEN 11 THEN 'YM'																							" +
                                    " WHEN 12 THEN 'MM'																							" +
                                    " WHEN 13 THEN 'TK'																							" +
                                    " END Type,																									" +
                                    " I.CODE Code,																								" +
                                    " I.NAME ItemName,																							" +
                                    " U.NAME MainUnit,																							" +
                                    " I.SPECODE SpecialCode,																					" +
                                    " CONVERT(VARCHAR,ISNULL(T.STOK,0)) Amount																							" +
                                    //" WHERE I.CARDTYPE NOT IN(20,22) AND I.ACTIVE=0                                                                       " +
                                    " FROM LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS I JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_UNITSETL U ON U.UNITSETREF=I.UNITSETREF AND U.MAINUNIT=1					" +
                                    " OUTER APPLY(SELECT ONHAND STOK FROM LV_" + ConfigManager.ConfigCompanyNo() + "_" + ConfigManager.ConfigPeriodNo() + "_GNTOTST WHERE STOCKREF=I.LOGICALREF AND INVENNO=-1) T		" +
                                    " WHERE I.CARDTYPE NOT IN(20,22,4) AND I.ACTIVE=0                                                                       " +
                                    " ORDER BY 1 ";









                var sqlQuery = context.Database.SqlQuery<Item>(sqlString);
                var query = sqlQuery.ToList();

                int totalRows = query.Count;
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortColumnDirection = Request["order[0][dir]"];

                //Filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => x.Code.ToLower().Contains(searchValue.ToLower()) ||
                    x.Code.ToLower().Contains(searchValue.ToLower()) ||
                    x.ItemName.ToLower().Contains(searchValue.ToLower()) ||
                    x.MainUnit.ToLower().Contains(searchValue.ToLower()) ||
                    x.SpecialCode.ToLower().Contains(searchValue.ToLower()) ||
                    x.Amount.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalRowsAfterFiltering = query.Count;
                //Sorting


                query = query.OrderBy(sortColumnName + " " + sortColumnDirection).ToList();

                //Paging
                query = query.Skip(start).Take(length).ToList();


                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;


                return Json(new { data = query, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering },
                    JsonRequestBehavior.AllowGet);



            };

        }
        [HttpPost]
        public JsonResult UnitsetList(string code)
        {

            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " SELECT LOGICALREF Lineref,UNITSETREF Unitsetref,Code CODE,NAME Name FROM LG_" + ConfigManager.ConfigCompanyNo() + "_UNITSETL " +
                                   " WHERE UNITSETREF IN(SELECT UNITSETREF FROM LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS WHERE CARDTYPE <> 22 AND CODE = '" + code+"') ";








                var sqlQuery = context.Database.SqlQuery<OrderUnitset>(sqlString);
                var query = sqlQuery.ToList();

                int totalRows = query.Count;
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortColumnDirection = Request["order[0][dir]"];

                //Filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => x.Code.ToLower().Contains(searchValue.ToLower()) ||
                    x.Code.ToLower().Contains(searchValue.ToLower()) ||
                    x.Name.ToLower().Contains(searchValue.ToLower()) 
                    ).ToList();
                }

                int totalRowsAfterFiltering = query.Count;
                //Sorting


                query = query.OrderBy(sortColumnName + " " + sortColumnDirection).ToList();

                //Paging
                query = query.Skip(start).Take(length).ToList();


                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;


                return Json(new { data = query, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering },
                    JsonRequestBehavior.AllowGet);



            };

        }
        public bool TableValidate()
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " IF OBJECT_ID('TB_ORFICHE') IS NULL BEGIN " +
                                   " CREATE TABLE TB_ORFICHE(Id INT IDENTITY(1,1) PRIMARY KEY,Ficheno VARCHAR(100),Date VARCHAR(100),Docode VARCHAR(100),CustomerCode VARCHAR(100), " +
                                   " CustomerDefinition VARCHAR(100),CyphCode VARCHAR(100),Department VARCHAR(100),CapiDiv VARCHAR(100),Whouse VARCHAR(100),SpecialCode VARCHAR(100), " +
                                   " TradingGrp VARCHAR(100),SalesMan VARCHAR(100),TotalDiscount float, TotalVat float, Total float, NetTotal float ) " +
                                   " END; ";

                var sqlQuery = context.Database.ExecuteSqlCommand(sqlString);

                string sqlString2 =" IF OBJECT_ID('TB_ORFLINE') IS NULL BEGIN " +
                                   " CREATE TABLE TB_ORFLINE(Id INT IDENTITY(1,1) PRIMARY KEY,Ficheno VARCHAR(100),barcode VARCHAR(100),itemCode VARCHAR(100),itemName VARCHAR(100),amount float,mainUnit VARCHAR(100), " +
                                   " price float,vat float,Ind1 float,Ind2 float,Ind3 float,Ind4 float,Ind5 float,total float,netTotal float) " +
                                   " END; ";

                var sqlQuery2 = context.Database.ExecuteSqlCommand(sqlString2);
                return true;
            }
        }
        public bool InsertOrfiche(GetOrderTop getOrder,string date_)
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
               
                string sqlString = "INSERT INTO TB_ORFICHE VALUES ('" + getOrder.FicheNo + "'," +
                    "'" + date_+ "'," +
                    "'" + getOrder.Docode + "'," +
                    "'" + getOrder.CustomerCode + "'," +
                    "'" + getOrder.CustomerDefinition + "'," +
                    "'" + getOrder.CyphCode + "'," +
                    "'" + getOrder.Department + "'," +
                    "'" + getOrder.CapiDiv + "'," +
                    "'" + getOrder.Whouse + "'," +
                    "'" + getOrder.SpecialCode + "'," +
                    "'" + getOrder.TradingGrp + "'," +
                    "'" + getOrder.SalesMan + "'," +
                    "REPLACE('" + getOrder.TotalDiscount + "',',','.')," +
                    "REPLACE('" + getOrder.TotalVat + "',',','.')," +
                    "REPLACE('" + getOrder.Total + "',',','.')," +
                    "REPLACE('" + getOrder.NetTotal + "',',','.')," +
                    "REPLACE('" + getOrder.DiscountGenel + "',',','.'))";

                var sqlQuery = context.Database.ExecuteSqlCommand(sqlString);

            }
            return true;
        }
        public bool InsertOrfline(GetOrderEnd[] kalemler,string ficheNo)
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                if (kalemler!=null)
                {
                    foreach (var line in kalemler)
                    {
                        if (!string.IsNullOrEmpty(line.itemCode))
                        {
                            string sqlString = "INSERT INTO TB_ORFLINE VALUES ('" + ficheNo + "','" + line.barcode + "','" + line.itemCode + "','" + line.itemName + "',REPLACE('" + line.amount + "',',','.'),'" + line.mainUnit + "',REPLACE('" + line.price + "',',','.'),REPLACE('" + line.vat + "',',','.'),REPLACE('" + line.Ind1 + "',',','.'),REPLACE('" + line.Ind2 + "',',','.'),REPLACE('" + line.Ind3 + "',',','.'),REPLACE('" + line.Ind4 + "',',','.'),REPLACE('" + line.Ind5 + "',',','.'),REPLACE('" + line.total + "',',','.'),REPLACE('" + line.netTotal + "',',','.'))";

                            var sqlQuery = context.Database.ExecuteSqlCommand(sqlString);
                        }

                    }
                }
                else
                {
                    return false;
                }




                return true;

            }
           
        }
        public bool DeleteOrficheAndOrfline(string ficheNo)
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {

                string sqlString = "DELETE FROM TB_ORFICHE WHERE Ficheno='"+ ficheNo + "';DELETE FROM TB_ORFLINE WHERE Ficheno='" + ficheNo + "';";

                var sqlQuery = context.Database.ExecuteSqlCommand(sqlString);





            }
            return true;
        }
        [HttpGet]
        public ActionResult UpdateOrder(string data)
        {

            GetOrderView getOrderView = new GetOrderView();


            ViewBag.ficheNo = data;
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                //string sqlFicheNo = "DECLARE @FICHENO VARCHAR(100)='ENT'+CONVERT(VARCHAR,YEAR(GETDATE())); SELECT TOP 1 @FICHENO+RIGHT('000000'+CONVERT(VARCHAR,((CONVERT(INT,RIGHT(Ficheno,6)))+1)),6) FicheNo FROM TB_ORFICHE ORDER BY 1 DESC ";



                //var ficheNoSql = context.Database.SqlQuery<OrderFicheNo>(sqlFicheNo).ToList();
                //var value = ficheNoSql.SingleOrDefault();

                //ViewBag.ficheNo = value.FicheNo;
                //-----------------------------------------------------------------------//
                string sqlString = "SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIDEPT WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery = context.Database.SqlQuery<OrderCapiDept>(sqlString).ToList();

                List<SelectListItem> capiDeptList = (from i in sqlQuery
                                                     select new SelectListItem
                                                     {
                                                         Text = i.Name,
                                                         Value = i.Nr.ToString()

                                                     }).ToList();


                ViewBag.capiDept = capiDeptList;
                //---------------------------------------------//
                string sqlString2 = "SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIDIV WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery2 = context.Database.SqlQuery<OrderCapiDiv>(sqlString2).ToList();

                List<SelectListItem> capiDivList = (from i in sqlQuery2
                                                    select new SelectListItem
                                                    {
                                                        Text = i.Name,
                                                        Value = i.Nr.ToString()

                                                    }).ToList();


                ViewBag.capiDiv = capiDivList;
                //---------------------------------------------//
                string sqlString3 = "SELECT Convert(INT,NR) Nr,NAME Name FROM L_CAPIWHOUSE WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery3 = context.Database.SqlQuery<OrderCapiDiv>(sqlString3).ToList();

                List<SelectListItem> capiWhouseList = (from i in sqlQuery3
                                                       select new SelectListItem
                                                       {
                                                           Text = i.Name,
                                                           Value = i.Nr.ToString()

                                                       }).ToList();


                ViewBag.capiWhouse = capiWhouseList;
                //---------------------------------------------//
                string sqlString4 = "SELECT CONVERT(INT,LOGICALREF) NR,DEFINITION_ NAME FROM LG_SLSMAN WHERE FIRMNR='" + ConfigManager.ConfigCompanyNo() + "'";



                var sqlQuery4 = context.Database.SqlQuery<OrderSalesMan>(sqlString4).ToList();

                List<SelectListItem> capiSalesman = (from i in sqlQuery4
                                                     select new SelectListItem
                                                     {
                                                         Text = i.Name,
                                                         Value = i.Nr.ToString()

                                                     }).ToList();
                capiSalesman.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""
                });

                ViewBag.salesMan = capiSalesman;

                //---------------------------------------------//
                string sqlString5 = "SELECT CONVERT(INT,LOGICALREF) NR,GCODE NAME FROM L_TRADGRP WHERE ACTIVE=0";



                var sqlQuery5 = context.Database.SqlQuery<OrderTradingGrp>(sqlString5).ToList();

                List<SelectListItem> tradingGrps = (from i in sqlQuery5
                                                    select new SelectListItem
                                                    {
                                                        Text = i.Name,
                                                        Value = i.Nr.ToString()

                                                    }).ToList();
                tradingGrps.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""
                });

                ViewBag.tradingGrp = tradingGrps;
                //---------------------------------------------//
                string sqlString6 = "SELECT CONVERT(INT,LOGICALREF) Nr,CODE Code FROM LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD WHERE  ACTIVE=0 AND CARDTYPE<>22";



                var sqlQuery6 = context.Database.SqlQuery<OrderCustomerCode>(sqlString6).ToList();

                List<SelectListItem> customersCode = (from i in sqlQuery6
                                                      select new SelectListItem
                                                      {
                                                          Text = i.Code,
                                                          Value = i.Nr.ToString()

                                                      }).ToList();
                customersCode.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""
                });

                ViewBag.customerCode = customersCode;





                string sqlString7 = "SELECT CONVERT(INT,LOGICALREF) Nr,LEFT(DEFINITION_,20) Definition FROM LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD WHERE ACTIVE=0 AND CARDTYPE<>22";



                var sqlQuery7 = context.Database.SqlQuery<OrderCustomerDefinition>(sqlString7).ToList();

                List<SelectListItem> customersDefs = (from i in sqlQuery7
                                                      select new SelectListItem
                                                      {
                                                          Text = i.Definition,
                                                          Value = i.Nr.ToString()

                                                      }).ToList();
                customersDefs.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = ""

                });

                ViewBag.customerDefs = customersDefs;


                string sqlStringTop = " SELECT                                                                                                                               " +
                                    "   FicheNo FicheNo, 																												   " +
                                    " 	Date Date,  																											   " +
                                    " 	Docode Docode,																													   " +
                                    " 	T.CustomerCode CustomerCode,																									   " +
                                    " 	T.CustomerDefinition CustomerDefinition,																						   " +
                                    " 	CyphCode CyphCode,																												   " +
                                    " 	Department Department, 																									   " +
                                    " 	CapiDiv CapiDiv,																											   " +
                                    " 	Whouse Whouse,																												   " +
                                    " 	SpecialCode SpecialCode, 																										   " +
                                    " 	ISNULL(T.TradingGrp,'') TradingGrp,  																								   " +
                                    " 	ISNULL(T.SalesMan,'') SalesMan,																												   " +
                                    " 	CONVERT(VARCHAR,TotalDiscount) TotalDiscount,																					   " +
                                    " 	CONVERT(VARCHAR,TotalVat) TotalVat,																								   " +
                                    " 	CONVERT(VARCHAR,Total) Total,																									   " +
                                    " 	CONVERT(VARCHAR,NetTotal) NetTotal 																								   " +
                                    " 																																	   " +
                                    " FROM TB_ORFICHE T 																												   " +
                                    //" OUTER APPLY(SELECT DEFINITION_ CustomerDefinition,CODE CustomerCode FROM LG_010_CLCARD WHERE T.CustomerCode=LOGICALREF) C  		   " +
                                  
                                    " WHERE T.Ficheno='" + data + "'";
                
                var sqlQueryTop = context.Database.SqlQuery<GetOrderTop>(sqlStringTop);
               
                getOrderView.GetOrderTop_ = sqlQueryTop.First();
                ViewBag.date_ = Convert.ToDateTime(getOrderView.GetOrderTop_.Date).ToString("yyyy-MM-dd");

                string sqlStringEnd = " SELECT                            " +
                                    " 	barcode barcode,				" +
                                    " 	itemCode itemCode,				" +
                                    " 	itemName itemName,				" +
                                    " 	CONVERT(VARCHAR,amount)  amount,					" +
                                    " 	mainUnit mainUnit,				" +
                                    " 	CONVERT(VARCHAR,price) price,					" +
                                    " 	CONVERT(VARCHAR,vat) vat,						" +
                                    " 	CONVERT(VARCHAR,Ind1) Ind1,						" +
                                    " 	CONVERT(VARCHAR,Ind2) Ind2,						" +
                                    " 	CONVERT(VARCHAR,Ind3) Ind3,						" +
                                    " 	CONVERT(VARCHAR,Ind4) Ind4,						" +
                                    " 	CONVERT(VARCHAR,Ind5) Ind5,						" +
                                    " 	CONVERT(VARCHAR,total) total,					" +
                                    " 	CONVERT(VARCHAR,netTotal) netTotal				" +
                                    " FROM TB_ORFLINE WHERE Ficheno='" + data + "'";

                var sqlQueryEnd = context.Database.SqlQuery<GetOrderEnd>(sqlStringEnd);
                List<GetOrderEnd> getOrderEnds = sqlQueryEnd.ToList();
                getOrderView.GetOrderEnd_ = getOrderEnds;

                return View(getOrderView);
            }
        }
        [HttpPost]
        public JsonResult UpdateOrder(GetOrderTop getOrder, GetOrderEnd[] kalemler)
        {
            bool status = DeleteOrficheAndOrfline(getOrder.FicheNo);
            if (status)
            {
                var orfiche = InsertOrfiche(getOrder, getOrder.Date);
                if (orfiche)
                {
                    var orfline = InsertOrfline(kalemler, getOrder.FicheNo);


                    if (orfline)
                    {
                        ViewBag.Succes = "Sipariş Kaydedildi.";
                    }
                }
                else
                {
                    ViewBag.Error = "Sipariş Kaydedilemedi.";
                }
            }
            return Json("");
        }
        [HttpPost]
        public JsonResult DeleteOrfiche(string data)
        {
          DeleteOrficheAndOrfline(data);
            return Json("");
        }
        public ActionResult PrintOrder(string data)
        {
            GetOrderView getOrderView = new GetOrderView();
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlStringTop = " SELECT                                                                                                                               " +
                                              "   FicheNo FicheNo, 																												   " +
                                              " 	Date Date,  																											   " +
                                              " 	Docode Docode,																													   " +
                                              " 	C.CustomerCode CustomerCode,																									   " +
                                              " 	C.CustomerDefinition CustomerDefinition,																						   " +
                                              " 	CyphCode CyphCode,																												   " +
                                              " 	Department Department, 																									   " +
                                              " 	CapiDiv CapiDiv,																											   " +
                                              " 	Whouse Whouse,																												   " +
                                              " 	SpecialCode SpecialCode, 																										   " +
                                              " 	ISNULL(T.TradingGrp,'') TradingGrp,  																								   " +
                                              " 	ISNULL(T.SalesMan,'') SalesMan,																												   " +
                                              " 	CONVERT(VARCHAR,TotalDiscount) TotalDiscount,																					   " +
                                              " 	CONVERT(VARCHAR,TotalVat) TotalVat,																								   " +
                                              " 	CONVERT(VARCHAR,Total) Total,																									   " +
                                              " 	CONVERT(VARCHAR,NetTotal) NetTotal 																								   " +
                                              " 																																	   " +
                                              " FROM TB_ORFICHE T 																												   " +
                                              " OUTER APPLY(SELECT DEFINITION_ CustomerDefinition,CODE CustomerCode FROM LG_"+ConfigManager.ConfigCompanyNo()+"_CLCARD WHERE T.CustomerCode=LOGICALREF) C  		   " +

                                              " WHERE T.Ficheno='" + data + "'";

                var sqlQueryTop = context.Database.SqlQuery<GetOrderTop>(sqlStringTop);

                getOrderView.GetOrderTop_ = sqlQueryTop.First();
                //ViewBag.date_ = Convert.ToDateTime(getOrderView.GetOrderTop_.Date).ToString("yyyy-MM-dd");


                string sqlStringEnd = " SELECT                            " +
                                    " 	barcode barcode,				" +
                                    " 	itemCode itemCode,				" +
                                    " 	itemName itemName,				" +
                                    " 	CONVERT(VARCHAR,amount)  amount,					" +
                                    " 	mainUnit mainUnit,				" +
                                    " 	CONVERT(VARCHAR,price) price,					" +
                                    " 	CONVERT(VARCHAR,vat) vat,						" +
                                    " 	CONVERT(VARCHAR,Ind1) Ind1,						" +
                                    " 	CONVERT(VARCHAR,Ind2) Ind2,						" +
                                    " 	CONVERT(VARCHAR,Ind3) Ind3,						" +
                                    " 	CONVERT(VARCHAR,Ind4) Ind4,						" +
                                    " 	CONVERT(VARCHAR,Ind5) Ind5,						" +
                                    " 	CONVERT(VARCHAR,total) total,					" +
                                    " 	CONVERT(VARCHAR,netTotal) netTotal				" +
                                    " FROM TB_ORFLINE WHERE Ficheno='" + data + "'";

                var sqlQueryEnd = context.Database.SqlQuery<GetOrderEnd>(sqlStringEnd);
                List<GetOrderEnd> getOrderEnds = sqlQueryEnd.ToList();
                getOrderView.GetOrderEnd_ = getOrderEnds;

                return View(getOrderView);
            }
          

            
        }
    }
}
