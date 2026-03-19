using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackLabelGroup
    {
        public PackLabel PackLabel { get; set; }
        public IEnumerable<PackLabelEdition> PackLabelEdition { get; set; }
        public IEnumerable<PackLabelItem> PackLabelItem { get; set; }
    }
}