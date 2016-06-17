using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowShare.API;

namespace Barly.Business
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsValid { get; set; }

        public Location()
        {

        }

        private static string GetResourceLink(Row row, string key)
        {
            if (row == null)
                return string.Empty;

            File file = new File((IDictionary<string, object>)row.Values["Photo"]);
            return string.Format(CultureInfo.CurrentCulture, "https://www.rowshare.com/blob/{0}4/1/{1}", row.Id.ToString().Replace("-", ""), file.FileName);
        }
        public Location(Row row)
        {
            Id = int.Parse(row.Values["Id"].ToString());
            Name = row.Values["Nom"].ToString();
            Schedule = row.Values["Horaires"].ToString();
            Picture = GetResourceLink(row, "Photo");
            Address = row.Values["Adresse"].ToString();
            ZipCode = row.Values["Code postal"].ToString();
            Latitude = decimal.Parse(row.Values["Latitude"].ToString());
            Longitude = decimal.Parse(row.Values["Longitude"].ToString());
            IsValid = bool.Parse(row.Values["Validé"].ToString());
        }
    }
}
