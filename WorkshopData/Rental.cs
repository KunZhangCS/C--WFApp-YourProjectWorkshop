using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace WorkshopData
{
    public class Rental
    {
        public long Id { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime CheckIn { get; set; }                
        public long Borrower { get; set; }
        public long Tool { get; set; }

        public static void Delete(Rental record)
        {
            string query = "DELETE FROM rental WHERE id = @id";
            Database.IUD(query, record);
        }

        // new record does not have the checkin values
        public static void Insert(Rental record)
        {
            string query = "INSERT INTO rental (checkOut, borrower, tool) VALUES (@checkOut, @borrower, @tool)";
            Database.IUD(query, record);
        }

        // Just have to update the checkin status
        public static void Update(Rental record)
        {
            string query = "UPDATE rental SET checkIn=@checkIn WHERE id = @id";
            Database.IUD(query, record);
        }

        public static Rental FindAll(Rental record)
        {
            string query = "SELECT * FROM rental WHERE id = @id";
            return Database.GetConnection().QuerySingle<Rental>(query, record);
        }

        // find all columns without checkIN in case it is null
        public static Rental FindNull(Rental record)
        {
            string query = "SELECT id, checkOut, borrower, tool FROM rental WHERE id = @id";
            return Database.GetConnection().QuerySingle<Rental>(query, record);
        }

        public static List<Rental> SelectAll()
        {
            string query = "SELECT * FROM rental";
            return Database.GetConnection().Query<Rental>(query).AsList();            
        }
    }
}
