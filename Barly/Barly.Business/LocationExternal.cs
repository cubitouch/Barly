using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barly.Business
{
    public class LocationExternal
    {
        public FoursquareVenue Foursquare { get; set; }
        public GoogleNearby Google { get; set; }

        public LocationExternal(FoursquareVenue foursquare, GoogleNearby google)
        {
            Foursquare = foursquare;
            Google = google;
        }
    }
}
