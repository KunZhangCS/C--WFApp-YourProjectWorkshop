using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace WorkshopData
{
    public class Tools
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long AssetNumber { get; set; }
        public string Brand { get; set; }
        public string Comments { get; set; }
        public bool Active { get; set; }
        public bool Available { get; set; }
        
        public static void Delete(Tools record)
        {
            string query = "DELETE FROM tools WHERE id = @id";
            Database.IUD(query, record);
        }

        public static void Insert(Tools record)
        {
            string query = "INSERT INTO tools (description, assetNumber, brand, comments, active, available) VALUES (@description, @assetNumber, @brand, @comments, @active, @available)";
            Database.IUD(query, record);
        }

        public static void Update(Tools record)
        {
            string query = "UPDATE tools SET description = @description, assetNumber = @assetNumber, brand=@brand, comments=@comments, active=@active, available=@available WHERE id = @id";
            Database.IUD(query, record);
        }

        // Update the available status of tool when Rental Info changed
        public static void UpdateRental(Tools record)
        {
            string query = "UPDATE tools SET available=@available WHERE id = @id";
            Database.IUD(query, record);
        }

        public static Tools Find(Tools record)
        {
            string query = "SELECT * FROM tools WHERE id = @id";
            return Database.GetConnection().QuerySingle<Tools>(query, record);    
        }

        public static List<Tools> Select(string query)
        {            
            return Database.GetConnection().Query<Tools>(query).AsList();
        }
    }
}
