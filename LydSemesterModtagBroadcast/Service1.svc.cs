using System;
using System.Collections.Generic;
using System.Data;
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

        //Kigger på det øverste(eneste) felt i switch table for at se om den er true eller false
        public bool TjekStatus()
        {
            const string gemswitch = "SELECT TOP(1) OnOff from Switch";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(gemswitch, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool kontakt = (bool) reader["OnOff"];
                            if (kontakt == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        // Henter alle lyde fra databasen
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

        // Poster lyden fra udp reciever til databasen
        public int PostLydToList(string lyde)
        {
            int hentstedid = usestedid();
            const string postLyde = "insert into Lydmaling (Lyde, [FK IdSted]) values (@lyde, @sted)";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(postLyde, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@lyde", lyde);
                    insertCommand.Parameters.AddWithValue("@sted", hentstedid);
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected;

                }
            }

        }
        // sender string til db fra php
        public int SetIdSted(string stedstr)
        {
            int sted = Int32.Parse(stedstr);

            const string setid = "UPDATE StedID SET IdSted = @sted";

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand updateCommand = new SqlCommand(setid, databaseConnection))
                {
                    updateCommand.Parameters.AddWithValue("@sted", sted);
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
        // bruges til at vælge id fra sted (bruges i PostLydToList)
        public int usestedid()
        {
            const string kaldid = "SELECT * FROM StedID";


            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectIdCommand = new SqlCommand(kaldid, databaseConnection))
                {
                    using (SqlDataReader reader = selectIdCommand.ExecuteReader())
                    {
                        int stedid = 0;

                        while (reader.Read())
                        {
                            stedid = reader.GetInt32(0);
                        }
                        return stedid;
                    }
                }

            }
        }

       
        // Updatere switch table, så den kigger på tjekstatus metoden og ser om switch er false eller true og sætter den til det omvendte (eks. hvis tjekliste er sat til false vil Updat2 metoden ændre det til true
        public void Update()
        {
            bool status = TjekStatus();
            if (status == true)
            {
                bool value = false;
                const string update = "UPDATE Switch SET OnOff = @Value WHERE Id = 1 ";

                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand UpdateCommand = new SqlCommand(update, con);
                UpdateCommand.Parameters.AddWithValue("@Value", SqlDbType.Bit).Value = 0;
                con.Open();
                UpdateCommand.ExecuteNonQuery();
                con.Close();
            }
            else if (status == false)
            {
                bool value = true;
                const string update = "UPDATE Switch SET OnOff = @Value WHERE Id = 1 ";
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand UpdateCommand = new SqlCommand(update, con);
                UpdateCommand.Parameters.AddWithValue("@Value", SqlDbType.Bit).Value = 1;
                con.Open();
                UpdateCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        // Sorter listen af lyde efter lydniveau (descending) fra lavent til højest
        public IList<Lyd> GetAlllydSorted()
        {
            const string selectAllLyde =
                "SELECT Lydmaling.Lyde, Lydmaling.Dato, Steder.Sted FROM Lydmaling INNER JOIN Steder ON Lydmaling.[FK IdSted]=Steder.IdSted ORDER BY Lyde DESC";

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

                            string lyde = reader.GetString(0);
                            DateTime date = reader.GetDateTime(1);
                            string sted = reader.GetString(2);
                            Lyd l1 = new Lyd()
                            {

                                Lyde = lyde,
                                Date = date,
                                Sted = sted
                            };

                            lydList.Add(l1);
                        }

                        return lydList;
                    }
                }
            }
        }

        // Henter personale fra db (hardcoded ind for at vise ekspempel)
        public IList<Personale> GetAllPersonale()
        {
            {
                const string selectAllLyde = "Select * from Personale";

                using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
                {
                    databaseConnection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(selectAllLyde, databaseConnection))
                    {
                        using (SqlDataReader reader = selectCommand.ExecuteReader())

                        {
                            List<Personale> personaleList = new List<Personale>();
                            while (reader.Read())
                            {

                                string navn = reader.GetString(1);
                                int telf = reader.GetInt32(2);
                                string email = reader.GetString(3);
                                Personale l1 = new Personale()
                                {

                                    Navn = navn,
                                    Telf = telf,
                                    Email = email
                                };
                                personaleList.Add(l1);
                            }
                            return personaleList;
                        }
                    }
                }
            }
        }



        

        // Viser lyde og de steder lyd er sat til.
        public IList<Lyd> GetAllLydMedSted()
        {
            const string selectAllLyde = "SELECT Lydmaling.Lyde, Lydmaling.Dato, Steder.Sted FROM Lydmaling INNER JOIN Steder ON Lydmaling.[FK IdSted]=Steder.IdSted";

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
                            
                            string lyde = reader.GetString(0);
                            DateTime date = reader.GetDateTime(1);
                            string sted = reader.GetString(2);
                            Lyd l1 = new Lyd()
                            {                              
                                Lyde = lyde,
                                Date = date,
                                Sted = sted
                            };
                            lydList.Add(l1);
                        }
                        return lydList;
                    }
                }
            }
        }
        // sender max lyd (brugt til mail)
        public IList<Lyd> GetMaxLyd()
        {
            const string selectMaxLyde = "SELECT TOP 1 * FROM Lydmaling ORDER BY Lyde DESC";

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectMaxLyde, databaseConnection))
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

        // bruges til mail system - sender personale info 
        public IList<Personale> GetHvorHvem()
        {
            const string selectAllLyde =
                "SELECT Personale.Navn, Personale.Telf, Personale.Email, Steder.Sted FROM Personale INNER JOIN Steder ON Personale.[FK StedId]=Steder.IdSted";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllLyde, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())

                    {

                        List<Personale> personList = new List<Personale>();
                        while (reader.Read())
                        {

                            string personale = reader.GetString(0);
                            int telefon = reader.GetInt32(1);
                            string email = reader.GetString(2);
                            string sted = reader.GetString(3);
                            Personale l1 = new Personale()
                            {
                                Navn = personale,
                                Sted = sted,
                                Telf = telefon,
                                Email = email
                            };
                            personList.Add(l1);

                        }
                        return personList;
                    }
                }
            }
        }
    }
}

   


