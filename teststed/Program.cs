using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teststed
{
    class Program
    {
        private static string ConnectionString =
                "Server=tcp:eventmserver.database.windows.net,1433;Initial Catalog=EMDatabase;Persist Security Info=False;User ID=Matias;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            ;
        static void Main(string[] args)
        {
            int sted = 4;
            const string setid = "UPDATE StedID SET IdSted = @sted";

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand updateCommand = new SqlCommand(setid, databaseConnection))
                {
                    updateCommand.Parameters.AddWithValue("@sted", sted);
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected);
                    
                }
                
            }
            Console.ReadKey();
        }
    }
}
