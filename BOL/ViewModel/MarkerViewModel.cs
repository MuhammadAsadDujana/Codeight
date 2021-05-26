using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.ViewModel
{
    public class MarkerViewModel
    {
        public int LocationId { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<double> Distance { get; set; }
        public Nullable<int> UserId { get; set; }
        public string FCMDeviceToken { get; set; }
        public double Range { get; set; }
    }
}
