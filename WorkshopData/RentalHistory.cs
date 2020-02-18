using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace WorkshopData
{
    // use this model to display results of the inner join of tools, borrowers and rental
    // Create a view in database to avoid the complicated sqlite syntax in C#
    public class RentalHistory
    {
        public long Id { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime CheckIn { get; set; }
        public string BorrowerName { get; set; }
        public string Description{ get; set; }

        public static List<RentalHistory> Select()
        {
            string query = "SELECT * FROM rentalHistory";
            return Database.GetConnection().Query<RentalHistory>(query).AsList();
        }
    }
}
