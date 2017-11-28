using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LydSemesterModtagBroadcast
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        private string ConnectionString =
                "Server=tcp:eventmserver.database.windows.net,1433;Initial Catalog=EMDatabase;Persist Security Info=False;User ID=Matias;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            ;

        public IList<Lyd> GetAllLyd()
        {
            const string selectAllLyde = "Select * from Lydmaling";

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllLyde, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())

                    {
                        List<Lyd> lydList = new List<Lyd>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string lyde = reader.GetString(1);
                            DateTime date = reader.GetDateTime(2);
                            Lyd l1 = new Lyd()
                            {
                                Id = id,
                                Lyde = lyde,
                                Date = date
                            };
                            lydList.Add(l1);
                        }
                        return lydList;
                    }
                }
            }
        }
    

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public int PostLydToList(string lyde)
        {
            const string postLyde = "insert into Lydmaling (Lyde, Date) values (@lyde, GETDATE())";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(postLyde, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@lyde", lyde);
                    
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected;

                }
            }
        }
    }
}
