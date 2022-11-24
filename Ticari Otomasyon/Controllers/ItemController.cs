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
    [_SessionControl]
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ItemList(JqDataTable model)
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
        [HttpGet]
        public ActionResult GetItem(string data)
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
                     " I.NAME2 ItemName2,																							" +
                     " U.NAME MainUnit,																							" +
                     " I.SPECODE SpecialCode,																					" +
                     " I.CYPHCODE CyphCode,																					" +
                     " I.PRODUCERCODE ProducerCode,																					" +
                     " I.VAT Vat,																					" +
                     " I.SELLVAT SellVat,																					" +
                     " I.RETURNVAT ReturnVat																			" +

                     " FROM LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS I JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_UNITSETL U ON U.UNITSETREF=I.UNITSETREF AND U.MAINUNIT=1					" +

                     " WHERE I.CARDTYPE NOT IN (22,20) AND I.ACTIVE=0 AND I.CODE='" + data + "'                                                                      " +
                     " ORDER BY 1 ";

                var sqlQuery = context.Database.SqlQuery<GetItem>(sqlString);
                GetItem query = sqlQuery.SingleOrDefault();

                return View(query);
            }

        }
        [HttpPost]
        public ActionResult UpdateItem(GetItem item)
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {

                string sqlCommand = " UPDATE LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS  SET CODE='" + item.Code + "'," +
                                      " NAME='" + item.ItemName + "'," +
                                      " NAME2='" + item.ItemName2 + "'," +
                                      " SPECODE='" + item.SpecialCode + "'," +
                                      " CYPHCODE='" + item.CyphCode + "'," +
                                      " PRODUCERCODE='" + item.ProducerCode + "'," +
                                      " VAT='" + item.Vat + "'," +
                                      " SELLVAT='" + item.SellVat + "'," +
                                      " RETURNVAT='" + item.ReturnVat + "'" +

                                      " WHERE LOGICALREF='" + item.Logicalref + "'";


                var sqlQuery = context.Database.ExecuteSqlCommand(sqlCommand);


                return Json(new { success = true, message = "Malzeme Kartı Güncellendi" }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public ActionResult UpdateItem(string data)
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
                     " I.NAME2 ItemName2,																							" +
                     " U.NAME MainUnit,																							" +
                     " I.SPECODE SpecialCode,																					" +
                     " I.CYPHCODE CyphCode,																					" +
                     " I.PRODUCERCODE ProducerCode,																					" +
                     " I.VAT Vat,																					" +
                     " I.SELLVAT SellVat,																					" +
                     " I.RETURNVAT ReturnVat																			" +

                     " FROM LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS I JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_UNITSETL U ON U.UNITSETREF=I.UNITSETREF AND U.MAINUNIT=1					" +

                     " WHERE I.CARDTYPE NOT IN (22,20) AND I.ACTIVE=0 AND I.CODE='" + data + "'                                                                      " +
                     " ORDER BY 1 ";

                var sqlQuery = context.Database.SqlQuery<GetItem>(sqlString);
                GetItem query = sqlQuery.SingleOrDefault();

                return View(query);
            }

        }

        [HttpPost]
        public ActionResult ListSellTransactions(string data)
        {
           using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " SELECT                                                                                       " +
                                 " O.LOGICALREF Logicalref,																	   " +
                                 " CONVERT(VARCHAR,O.DATE_,104) Date,														   " +
                                 " O.FICHENO Ficheno,																		   " +
                                 " C.DEFINITION_ CompanyName,																   " +
                                 " O.SPECODE SpecialCode,																	   " +
                                 " O.TRADINGGRP TradingGrp,																	   " +
                                 " CONVERT(VARCHAR,O.NETTOTAL) Amount,																		   " +
                                 " CONVERT(VARCHAR,O.SOURCEINDEX) SourceIndex,																   " +
                                 " CONVERT(VARCHAR,O.BRANCH) Branch,																			   " +
                                 " CONVERT(VARCHAR,O.DEPARTMENT) Department,																	   " +
                                 " ISNULL(SM.SALESMAN,'') SalesMan															   " +
                                 "  FROM LG_" + ConfigManager.ConfigCompanyNo() + "_" + ConfigManager.ConfigPeriodNo() + "_ORFICHE O JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD C ON C.LOGICALREF=O.CLIENTREF				   " +
                                 " OUTER APPLY(SELECT DEFINITION_ SALESMAN FROM LG_SLSMAN WHERE LOGICALREF=O.SALESMANREF AND FIRMNR='" + ConfigManager.ConfigCompanyNo() + "') SM	   " +
                                 " WHERE O.TRCODE=1 AND O.LOGICALREF IN (SELECT S.ORDFICHEREF FROM LG_" + ConfigManager.ConfigCompanyNo() + "_" + ConfigManager.ConfigPeriodNo() + "_ORFLINE S JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS I ON I.LOGICALREF=S.STOCKREF WHERE CODE='" + data + "')";


                var sqlQuery = context.Database.SqlQuery<SellTransactionCustomer>(sqlString);
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
                    query = query.Where(x => x.Date.ToLower().Contains(searchValue.ToLower()) ||
                    x.Ficheno.ToLower().Contains(searchValue.ToLower()) ||
                    x.CompanyName.ToLower().Contains(searchValue.ToLower()) ||
                    x.SpecialCode.ToLower().Contains(searchValue.ToLower()) ||
                    x.TradingGrp.ToLower().Contains(searchValue.ToLower()) ||
                    x.SalesMan.ToLower().Contains(searchValue.ToLower()) ||
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
        [HttpGet]
        public ActionResult SellTransactions(string data)
        {
            ViewBag.itemTemp = data;

            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = "SELECT NAME ItemName FROM LG_" + ConfigManager.ConfigCompanyNo() + "_ITEMS WHERE CODE='" + data + "'";

                var sqlQuery = context.Database.SqlQuery<SellTransactionItemName>(sqlString);
                var query = sqlQuery.SingleOrDefault();
                ViewBag.itemName = query.ItemName.ToString();
            }
            return View();
        }
    }
}