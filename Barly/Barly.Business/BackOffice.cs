using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void LoadLocations()
        {
            List locationTable = List.GetListById(_rowshareTableId);
            locationTable.LoadRows();

            _locations = new List<Location>();
            foreach (Row row in locationTable.Rows)
            {
                _locations.Add(new Location(row));
            }
        }
    }
}
