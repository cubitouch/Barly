using System;
using System.Collections.ObjectModel;
using System.Globalization;
using CodeFluent.Runtime.Utilities;

namespace RowShare.Api
{
    public class List
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int ColumnCount { get; set; }
        public DateTime LastUpdateDateUtc { get; set; }
        public Folder Folder { get; set; }
        public Collection<Column> Columns { get; private set; }
        public Collection<Row> Rows { get; set; }

        public static List GetListById(string id)
        {
            string url = string.Format(CultureInfo.CurrentCulture, "list/load/{0}", id);
            string json = RowShareCommunication.GetData(url);

            return JsonUtilities.Deserialize<List>(json);
        }

        public void LoadRows()
        {
            Rows = Row.GetRowsByList(this);
        }

        public void LoadRows(string json)
        {
            Rows = JsonUtilities.Deserialize<Collection<Row>>(json, ~JsonSerializationOptions.AutoParseDateTime);
        }

        public void LoadColumns()
        {
            Columns = Column.GetColumnsByList(this);
        }

        public void CacheRowsToFS(string cacheFilename)
        {
            System.IO.File.WriteAllText(cacheFilename, JsonUtilities.Serialize(Rows), System.Text.Encoding.UTF8);
        }
        public void ClearCache(string cacheFilename, DateTime lastUpdate)
        {
            string cacheDirectory = System.IO.Path.GetDirectoryName(cacheFilename);
            foreach (string filename in System.IO.Directory.EnumerateFiles(cacheDirectory))
            {
                string cacheName = System.IO.Path.GetFileNameWithoutExtension(filename);
                long ticks = 0;
                long.TryParse(cacheName, out ticks);
                if (ticks > 0)
                {
                    DateTime cacheDate = new DateTime(ticks);
                    if (cacheDate < lastUpdate)
                    {
                        System.IO.File.Delete(filename);
                    }
                }
            }
        }

        public static void DeleteList(string id)
        {
            var data = GetListById(id);
            DeleteList(data);
        }

        public static void DeleteList(List data)
        {
            string url = string.Format(CultureInfo.CurrentCulture, "/row/delete/");
            RowShareCommunication.DeleteData(url, JsonUtilities.Serialize(data));
        }
    }
}
