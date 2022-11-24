using System;
namespace Ticari_Otomasyon.ServerSideProcess
{
    public class JqDataTable
    {
        public string sEcho { get; set; }

        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }

        public int? iColumns { get; set; }

        public int? iSortingCols { get; set; }
        public string sColumns { get; set; }

        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }

        public string sSearch_0 { get; set; }
        public string sSearch_1 { get; set; }
        public string sSearch_2 { get; set; }
        public string sSearch_3 { get; set; }

    }
}
