using System.Collections.Generic;
using System.Web.Hosting;
using RowShare.Api;

namespace Barly.Business
{
    public class BackOffice
    {
        private const string _rowshareTableId = "40ac0e1156c14f46aa996779e464fe37";

        private IList<Location> _locations;
        public IList<Location> Locations
        {
            get
            {
                if (_locations == null)
                {
                    LoadLocations();
                }
                return _locations;
            }
        }

        private object cacheLock = new object();
        private void LoadLocations()
        {
            List locationTable = new List();
            bool hasNewCache = false;

            try
            {
                locationTable = List.GetListById(_rowshareTableId);
                string cacheFilename = HostingEnvironment.MapPath(string.Format("~/App_Data/locations/{0}.json", locationTable.LastUpdateDateUtc.Ticks.ToString()));
                
                if (System.IO.File.Exists(cacheFilename))
                {
                    lock (cacheLock)
                    {
                        locationTable.LoadRows(System.IO.File.ReadAllText(cacheFilename));
                    }
                }
                else
                {
                    locationTable.LoadRows();
                    hasNewCache = true;
                }
                if (hasNewCache)
                {
                    locationTable.CacheRowsToFS(cacheFilename);
                    locationTable.ClearCache(cacheFilename, locationTable.LastUpdateDateUtc);
                }
            }
            catch (System.Exception e)
            {
                string cacheDirectory = HostingEnvironment.MapPath(string.Format("~/App_Data/locations"));
                foreach (string cacheFile in System.IO.Directory.GetFiles(cacheDirectory))
                {
                    lock (cacheLock)
                    {
                        locationTable.LoadRows(System.IO.File.ReadAllText(cacheFile));
                    }
                }
            }

            _locations = new List<Location>();
            foreach (Row row in locationTable.Rows)
            {
                _locations.Add(new Location(row));
            }
        }
    }
}
