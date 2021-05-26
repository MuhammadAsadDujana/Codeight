using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.ViewModel
{
    public class MapNearByListViewModel
    {
        public string SessionId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string VideoCategory { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public string Role { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> TokenIssueDate { get; set; }
        public double Range { get; set; }
    }
}
