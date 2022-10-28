using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Security.Permissions;

namespace MainAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet("{id}")]
        public string[] GetPrice(int id)
        {
            string temp = id.ToString();
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("forparsing");
            var test = database.GetCollection<BsonDocument>("test");

            var filter = Builders<BsonDocument>.Filter.Eq("id", temp);

            var doc = test.Find(filter).FirstOrDefault();

            if (doc != null)
            {
                string fio = doc["fio"].ToString();
                string price = doc["price"].ToString();
                Console.WriteLine(price);
                string[] result = { fio, price };
                return result;
            }
            else
            {
                string[] result = { "Error", "No one ID" };
                return result;
            }
        }
        [HttpGet("{city}/{street}/{house}/{apart}")]
        public string[] GetID(string city, string street, string house, string apart)
        {
            string stringconn = "Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                "Initial Catalog=adresDB;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(stringconn);
            try
            {
                conn.Open();
            }
            catch (SqlException ex)
            {
                Console.Write(ex.Message);
            }

            string zxc = "qwe";
            SqlCommand command = new SqlCommand(zxc, conn);
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter cityParam = new SqlParameter
            {
                ParameterName = "@city",
                Value = city
            };
            command.Parameters.Add(cityParam);
            SqlParameter streetParam = new SqlParameter
            {
                ParameterName = "@street",
                Value = street
            };
            command.Parameters.Add(streetParam);
            SqlParameter houseParam = new SqlParameter
            {
                ParameterName = "@house",
                Value = house
            };
            command.Parameters.Add(houseParam);
            SqlParameter apartParam = new SqlParameter
            {
                ParameterName = "@apart",
                Value = apart
            };
            command.Parameters.Add(apartParam);

            SqlParameter result = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.VarChar,
                Size = 50
            };
            result.Direction = ParameterDirection.Output;
            command.Parameters.Add(result);

            command.ExecuteNonQuery();

            string aaa = command.Parameters["@id"].Value.ToString();

            conn.Close();



            string temp = aaa.ToString();



            int period = 519;
            
            
            //менять период на фронет и тут !


            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("forparsing");
            var test = database.GetCollection<BsonDocument>("test");

            var filter = Builders<BsonDocument>.Filter.Eq("id", temp);

            //var filter1 = Builders<BsonDocument>.Filter.Eq("period", period);

            var doc = test.Find(filter).FirstOrDefault();

            if (doc != null)
            {
                string fio = doc["fio"].ToString();
                string price = doc["price"].ToString();
                string pd = doc["period"].ToString();

                price = price.Replace(".", ",");
                price = string.Format("{0:N2}", Convert.ToDouble(price));
                string[] result1 = { fio, price, temp,  pd };
                return result1;
            }
            else
            {
                string[] result1 = { "Error", "No one ID" };
                return result1;
            }

        }
        [HttpGet("c/{city}")]
        public string[] getStreet(string city)
        {
            string[] allStreet = null;

            string sql = $"select distinct street from adres where city = '{city}'";
            string stringconn = "Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                "Initial Catalog=adresDB;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(sql, conn);
            using (var reader = command.ExecuteReader())
            {
                var list = new List<string>();
                while (reader.Read())
                    list.Add( reader.GetString(0).ToString());
                allStreet = list.ToArray();
                Array.Sort(allStreet);
            }
            conn.Close();
            return allStreet;
        }
        [HttpGet("c")]
        public string[] getCitis()
        {
            string[] allCitis = null;

            string sql = $"select distinct city from adres";
            string stringconn = "Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                "Initial Catalog=adresDB;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(sql, conn);
            using (var reader = command.ExecuteReader())
            {
                var list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetString(0).ToString());
                allCitis = list.ToArray();
                Array.Sort(allCitis);
            }
            conn.Close();
            return allCitis;
        }
        [HttpGet("cs/{city}/{street}")]
        public string[] getHouses(string city, string street)
        {
            string[] allHouses = null;

            string sql = $"select distinct house from adres where adres.city = '{city}' and adres.street = '{street}'";
            string stringconn = "Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                "Initial Catalog=adresDB;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(sql, conn);
            using (var reader = command.ExecuteReader())
            {
                var list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetString(0).ToString());
                allHouses = list.ToArray();
                Array.Sort(allHouses);
            }
            conn.Close();
            return allHouses;
        }
        [HttpGet("csh/{city}/{street}/{house}")]
        public string[] getApart (string city, string street, string house)
        {
            string[] allApart = null;

            string sql = $"select distinct apart from adres where city = '{city}' and street = '{street}' and house = '{house}'";
            string stringconn = "Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                "Initial Catalog=adresDB;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(sql, conn);
            using (var reader = command.ExecuteReader())
            {
                var list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetString(0).ToString());
                allApart = list.ToArray();
                Array.Sort(allApart);
            }
            conn.Close();
            return allApart;
        }
        
    }
}
