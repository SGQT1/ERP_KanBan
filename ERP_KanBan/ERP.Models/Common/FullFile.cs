using System.IO;

namespace ERP.Models.Common
{
    public class FullFile
    {
        public string FileName { get; set; }

        public Stream Stream { get; set; }
    }
}