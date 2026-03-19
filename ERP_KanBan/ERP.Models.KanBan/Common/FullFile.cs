using System.IO;

namespace ERP.Models.KanBan.Common
{
    public class FullFile
    {
        public string FileName { get; set; }

        public Stream Stream { get; set; }
    }
}