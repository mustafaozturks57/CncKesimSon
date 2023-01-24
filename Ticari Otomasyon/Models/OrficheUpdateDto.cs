using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticari_Otomasyon.Models.CncModel;

namespace Ticari_Otomasyon.Models
{
    public class OrficheUpdateDto
    {
        public Orfiche Orfiche { get; set; }
        public List<Orfline> Orflines { get; set; }
    }
}