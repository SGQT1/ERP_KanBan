using System;
using System.Collections.Generic;

namespace ERP.Models.Views.Projects
{
    public class ProjectGridView
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ArticleNo { get; set; }
        public string ProjectType { get; set; }
        public decimal ProjectTypeId { get; set; }
        public DateTime OpenDate { get; set; }
        public string ProjectOutsole { get; set; }
        public int? ProjectOutsoleId { get; set; }
        public string ProjectLast { get; set; }
        public int? ProjectLastId { get; set; }
        public string ShoeName { get; set; }
    }
}
