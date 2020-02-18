using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace WorkshopData
{
    // Only certain columns are needed in the reports
    // use this model to define the selected columns in Tools
    public class Report
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long AssetNumber { get; set; }
        public string Brand { get; set; }
        public string Comments { get; set; }

        public static List<Report> Select(string query, object record)
        {
            return Database.GetConnection().Query<Report>(query, record).AsList();
        }
    }
}
