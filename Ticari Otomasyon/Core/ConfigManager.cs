using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Ticari_Otomasyon.Core
{
    public static class ConfigManager
    {
        public static string ConfigCompanyNo()
        {
            return ConfigurationManager.AppSettings["CompanyNo"].ToString();
        }
        public static string ConfigPeriodNo()
        {
            return ConfigurationManager.AppSettings["PeriodNo"].ToString();
        }
    }
}