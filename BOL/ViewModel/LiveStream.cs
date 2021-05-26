using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.ViewModel
{
    public class LiveStream
    {
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public Guid ArchiveId { get; set; }
        public string VideoCategory { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public TimeSpan VideoDuration { get; set; }
        public string Status { get; set; }
        public string VideoURL { get; set; }
    }
}
