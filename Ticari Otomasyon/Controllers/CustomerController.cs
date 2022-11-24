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

using Ticari_Otomasyon.ServerSideProcess;

namespace Ticari_Otomasyon.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Cari

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CustomerList(JqDataTable model)
        {


             

            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " SELECT                                                                              " +
                                               
                                               "	C.CODE Code,																	  " +
                                               "	LEFT(C.DEFINITION_,50) CompanyName,														  " +



                                               "	CASE C.TAXNR WHEN '' THEN C.TCKNO ELSE C.TAXNR END IdentityNumber,				  " +
                                               "	C.INCHARGE Incharge,															  " +
                                               "	C.CITY City,																	  " +
                                               "	CASE WHEN A.BAKIYE>0 THEN CONVERT(VARCHAR,A.BAKIYE)+' (B)' 	  " +
                                               "	WHEN A.BAKIYE<0 THEN CONVERT(VARCHAR,ABS(A.BAKIYE))+' (A)' 	  " +
                                               "	ELSE CONVERT(VARCHAR,0) END Amount																  " +
                                               "																					  " +
                                               " FROM LG_"+ConfigManager.ConfigCompanyNo()+"_CLCARD C    	  " +
                                               " OUTER APPLY (SELECT DEBIT-CREDIT BAKIYE FROM LV_"+ConfigManager.ConfigCompanyNo()+"_"+ConfigManager.ConfigPeriodNo()+"_GNTOTCL WHERE CARDREF=C.LOGICALREF AND TOTTYP=1) A " +
                                               "WHERE C.CARDTYPE <> 22   AND C.ACTIVE = 0                                              ";
                
           
                var sqlQuery = context.Database.SqlQuery<Customer>(sqlString);
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
                    x.CompanyName.ToLower().Contains(searchValue.ToLower()) ||
                    x.IdentityNumber.ToLower().Contains(searchValue.ToLower()) ||
                    x.Incharge.ToLower().Contains(searchValue.ToLower()) ||
                    x.City.ToLower().Contains(searchValue.ToLower()) ||
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
        public ActionResult GetCustomer(string data)
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {
                string sqlString = " SELECT                                                                              " +
                                         "	C.LOGICALREF Logicalref,														  " +
                                         "	C.CODE Code,																	  " +
                                         "	C.DEFINITION_ CompanyName,														  " +
                                         "	C.DEFINITION2 CompanyName2,														  " +
                                         "	C.SPECODE SpecialCode,															  " +
                                         "	C.CYPHCODE CyphCode,															  " +
                                         "	C.TRADINGGRP TradingGrp,														  " +
                                         "	C.INCHARGE Incharge,															  " +
                                         "	C.CITY City,																	  " +
                                         "	C.TOWN Town,																	  " +
                                         "	C.COUNTRY Country,																  " +
                                         "	C.ADDR1 Adress,																	  " +
                                         "	C.TELNRS1 Phone,																	  " +
                                         "	C.EMAILADDR Email,																	  " +
                                         "	C.TAXOFFICE TaxOffice,															  " +
                                         "	CASE C.TAXNR WHEN '' THEN C.TCKNO ELSE C.TAXNR END IdentityNumber				  " +

                                         "																					  " +
                                         " FROM LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD C  	  " +
                                         " WHERE C.CARDTYPE<>22 AND C.ACTIVE=0 AND C.CODE='" + data + "'						  ";



                var sqlQuery = context.Database.SqlQuery<Customer>(sqlString);
                Customer query = sqlQuery.SingleOrDefault();

                return View(query);
            }

        }

        [HttpPost]
        public ActionResult UpdateCustomer(GetCustomer customer)
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {

               string   sqlCommand = " UPDATE LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD  SET CODE='" + customer.Code+"'," +
                                     " DEFINITION_='" + customer.CompanyName + "'," +
                                     " DEFINITION2='" + customer.CompanyName2 + "'," +
                                     " SPECODE='" + customer.SpecialCode + "'," +
                                     " CYPHCODE='" + customer.CyphCode + "'," +
                                     " TRADINGGRP='" + customer.TradingGrp + "'," +
                                     " INCHARGE='" + customer.Incharge + "'," +
                                     " CITY='" + customer.City + "'," +
                                     " TOWN='" + customer.Town + "'," +
                                     " COUNTRY='" + customer.Country + "'," +
                                     " ADDR1='" + customer.Adress + "'," +
                                     " TELNRS1='" + customer.Phone + "'," +
                                     " EMAILADDR='" + customer.Email + "'," +
                                     " TCKNO='" + customer.TckNo + "'," +
                                      " TAXNR='" + customer.Vkn + "'," +
                                     " TAXOFFICE='" + customer.TaxOffice + "'" +
                                     " WHERE LOGICALREF='" + customer.Logicalref + "'";
               

                var sqlQuery = context.Database.ExecuteSqlCommand(sqlCommand);


                return Json(new { success = true, message = "Cari Hesap Güncellendi" }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }

        }
        [HttpGet]
        public ActionResult UpdateCustomer(string data)
        {
            using (AutomationDbEntities context = new AutomationDbEntities())
            {



                string sqlString = " SELECT                                                                              " +
                                               "	C.LOGICALREF Logicalref,														  " +
                                               "	C.CODE Code,																	  " +
                                               "	C.DEFINITION_ CompanyName,														  " +
                                               "	C.DEFINITION2 CompanyName2,														  " +
                                               "	C.SPECODE SpecialCode,															  " +
                                               "	C.CYPHCODE CyphCode,															  " +
                                               "	C.TRADINGGRP TradingGrp,														  " +
                                               "	C.INCHARGE Incharge,															  " +
                                               "	C.CITY City,																	  " +
                                               "	C.TOWN Town,																	  " +
                                               "	C.COUNTRY Country,																  " +
                                               "	C.ADDR1 Adress,																	  " +
                                               "	C.TELNRS1 Phone,																	  " +
                                               "	C.EMAILADDR Email,																	  " +
                                               "	C.TAXOFFICE TaxOffice,															  " +
                                               "	C.TAXNR  Vkn,				  " +
                                                "	C.TCKNO  TckNo				  " +
                                               "																					  " +
                                               " FROM LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD C  	  " +
                                               " WHERE C.CARDTYPE<>22 AND C.ACTIVE=0 AND C.CODE='" + data + "'						  ";

                var sqlQuery = context.Database.SqlQuery<GetCustomer>(sqlString);
                GetCustomer query = sqlQuery.SingleOrDefault();

                return View(query);
            }

        }
        [HttpPost]
        public ActionResult ListSellTransactions(string data)
        {


            using (AutomationDbEntities context = new AutomationDbEntities())
            {
               string sqlString=" SELECT                                                                                       " +
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
                                "  FROM LG_"+ConfigManager.ConfigCompanyNo()+"_" + ConfigManager.ConfigPeriodNo() + "_ORFICHE O JOIN LG_" + ConfigManager.ConfigCompanyNo() + "_CLCARD C ON C.LOGICALREF=O.CLIENTREF				   " +
                                " OUTER APPLY(SELECT DEFINITION_ SALESMAN FROM LG_SLSMAN WHERE LOGICALREF=O.SALESMANREF AND FIRMNR='"+ConfigManager.ConfigCompanyNo()+"') SM	   " +
                                " WHERE O.TRCODE=1 AND C.CODE='"+data+"' ";


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
            ViewBag.temp = data;
            using (AutomationDbEntities context=new AutomationDbEntities())
            {
                string sqlString = "SELECT DEFINITION_ CompanyName FROM LG_"+ConfigManager.ConfigCompanyNo()+"_CLCARD WHERE CODE='" + data + "'";

                var sqlQuery = context.Database.SqlQuery<SellTransactionCustomerName>(sqlString);
                var query = sqlQuery.SingleOrDefault();
                ViewBag.companyName = query.CompanyName.ToString();
            }
            return View();
        }

    }

}