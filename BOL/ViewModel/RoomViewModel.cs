using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.ViewModel
{
    public class RoomViewModel
    {
        public string VideoCategory { get; set; }
        public int UserId { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string SessionId { get; set; }
        public string PublisherToken { get; set; }
        public string ConnectionId { get; set; }
        public double Latitude { get; set; }
        public double longitude { get; set; }
    }
}
