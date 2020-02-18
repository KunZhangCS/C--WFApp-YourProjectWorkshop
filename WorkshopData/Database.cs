using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Data.SQLite;


namespace WorkshopData
{
    static class Database
    {      

        public static SQLiteConnection GetConnection()
        {
            // doesn't work when used by another assembly
            //the connectionStrings must be int app.config of the startup project
            //string cn = System.Configuration.ConfigurationManager.ConnectionStrings["YourProjectRecord"].ConnectionString.ToString();
            //return new SQLiteConnection(cn);
            return new SQLiteConnection("Data Source=YourProjectRecord.db; Foreign Keys=True;");
        }      

        public static void Error(Exception ex)
        {
            MessageBox.Show($"Exception: {ex}");
        }

        // Method for Insert, Delete and Update queries
        public static void IUD(string query, object record)
        {
            var db = GetConnection().OpenAndReturn();            
            var trans = db.BeginTransaction();
            try
            {                
                db.Execute(query, record, trans);
                trans.Commit();
            }
            catch(Exception ex)
            {
                Error(ex);
                trans.Rollback();
            }
            db.Close();
        }
    }
}
