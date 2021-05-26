using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.ViewModel
{
    public class VideoManagementViewModel
    {
        public Guid ArchiveId { get; set; }
        public TimeSpan VideoDuration { get; set; }
        public string VideoSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string VideoURL { get; set; }
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
