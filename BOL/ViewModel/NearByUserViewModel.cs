using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.ViewModel
{
    public class NearByUserViewModel
    {
        public string VideoURL { get; set; }
        public Guid ArchiveId { get; set; }
        public string Status { get; set; }
        public TimeSpan VideoDuration { get; set; }
        public string SessionId { get; set; }
        public string PublisherToken { get; set; }
        public string VideoCategory { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
        public string roomCreatedLat { get; set; }
        public string roomCreatedLng { get; set; }

    }
}
