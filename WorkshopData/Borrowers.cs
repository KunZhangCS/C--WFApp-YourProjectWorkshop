using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace WorkshopData
{
    public class Borrowers
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // assign this as "DisplayMember"
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        // or...
        //public override string ToString()
        //{
        //    return $"{FirstName} {LastName}";
        //}

        public static void Delete(Borrowers record)
        {
            string query = "DELETE FROM borrowers WHERE id = @id";
            Database.IUD(query, record);
        }

        public static void Insert(Borrowers record)
        {
            string query = "INSERT INTO borrowers (firstName, lastName, phone, Email) VALUES (@firstName, @lastName, @phone, @Email)";
            Database.IUD(query, record);
        }

        public static void Update(Borrowers record)
        {
            string query = "UPDATE borrowers SET firstName=@firstName, lastName=@lastName, phone=@phone, Email=@Email WHERE id = @id";
            Database.IUD(query, record);
        }

        public static Borrowers Find(Borrowers record)
        {
            string query = "SELECT * FROM borrowers WHERE id = @id";
            return Database.GetConnection().QuerySingle<Borrowers>(query, record);
        }

        public static List<Borrowers> SelectAll()
        {
            string query = "SELECT * FROM borrowers";
            return Database.GetConnection().Query<Borrowers>(query).AsList();
        }
    }
}
