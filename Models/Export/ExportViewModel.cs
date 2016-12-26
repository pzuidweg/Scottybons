using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScottybonsMVC.Models.Export
{
    public class ExportViewModel
    {
        [AllowHtml]
        public string Csv { get; set; }
    }
}