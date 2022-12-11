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
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

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
        [HttpGet("{id}/{name}/{date}/{documentNumber}/{shiftNumber}/{documentIndex}/{fsNumber}/{ofdinn}/{price}/{ls}/{fp}")]
        public string CreateCheque(string id, string name, 
            string date, string documentNumber, string shiftNumber, 
            string documentIndex, string fsNumber,
            string ofdinn, string price, string ls, string fp)
        {
            string stringconn = "Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
               "Initial Catalog=adresDB;" +
               "Integrated Security=True;" +
               "Connect Timeout=30;" +
               "Encrypt=False;" +
               "TrustServerCertificate=False;" +
               "ApplicationIntent=ReadWrite;" +
               "MultiSubnetFailover=False";
            string sql = $"INSERT INTO [dbo].[Cheque] (" +
                $"[id]," +
                $"[name]," +
                $"[date]," +
                $"[documentNumber]," +
                $"[shiftNumber]," +
                $"[documentIndex]," +
                $"[fsNumber]," +
                $"[ofdinn]," +
                $"[price]," +
                $"[ls]," +
                $"[fp])" +
                $" VALUES ('{id}', '{name}', '{date}', '{documentNumber}', '{shiftNumber}', '{documentIndex}', '{fsNumber}', '{ofdinn}', '{price}', '{ls}', '{fp}');";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            string result = "";
            var command = new SqlCommand(sql, conn);
            try
            {
                result = command.ExecuteNonQuery().ToString();
            }
            catch
            {
                result = "err";         
            }
            conn.Close();
            return result;
        }
        [HttpGet("qwe/{id}")]
        public string[] GetCheque(string id)
        {
            string[] Cheque = new string[10];
            string stringconn = $"Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                $"Initial Catalog=adresDB;Integrated Security=True;" +
                $"Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
                $"ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sql = $"SELECT * FROM Cheque WHERE id = '{id}'";

            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(sql,conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Cheque[0] = reader.GetString(0).ToString();
                Cheque[1] = reader.GetString(1).ToString();
                Cheque[2] = reader.GetString(2).ToString();
                Cheque[3] = reader.GetString(3).ToString();
                Cheque[4] = reader.GetString(4).ToString();
                Cheque[5] = reader.GetString(5).ToString();
                Cheque[6] = reader.GetString(6).ToString();
                Cheque[7] = reader.GetString(7).ToString();
                Cheque[8] = reader.GetString(8).ToString();
                Cheque[9] = reader.GetString(9).ToString();
            }

            conn.Close();
            return Cheque;
        }
        [HttpGet("{hash}/{status}/{pay_id}")]
        public bool regreg(string hash, string status, string pay_id)
        {
            string stringconn = $"Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                $"Initial Catalog=adresDB;Integrated Security=True;" +
                $"Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
                $"ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sql = $"INSERT INTO [dbo].[paypay] VALUES ('{hash}', '{status}', '{pay_id}')";
            string qq = $"update paypay set [status] = '{status}', [pay_id] = '{pay_id}' where [hash] = '{hash}'\r\n" +
                $"if @@ROWCOUNT = 0\r\nbegin\r\n\tinsert into paypay values ('{hash}', '{status}', '{pay_id}')\r\nend";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(qq, conn);
            command.ExecuteNonQuery();
            conn.Close();
            return true;
        }
        [HttpGet("qq/{hash}")]
        public IActionResult getPayId(string hash)
        {
            string stringconn = $"Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
                $"Initial Catalog=adresDB;Integrated Security=True;" +
                $"Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
                $"ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sql = $"SELECT [status], [pay_id] FROM [adresDB].[dbo].[paypay]\r\nwhere [hash] = '{hash}'";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(sql, conn);
            var list = new List<qwe>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var temp = new qwe(
                        reader.GetString(0).ToString(),
                        reader.GetString(1).ToString());
                    list.Add(temp);
                }
            }
            var json = JsonConvert.SerializeObject(list);

            conn.Close();
            return Ok(json);
        }
        [HttpGet("qqqq/{hash}/{status}")]
        public bool qqq (string hash, string status)
        {
            string stringconn = $"Data Source=LAPTOP-FPR118VN\\SQLEXPRESS;" +
               $"Initial Catalog=adresDB;Integrated Security=True;" +
               $"Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
               $"ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sql = $"update paypay set [status] = '{status}' where [hash] = '{hash}'";
            SqlConnection conn = new SqlConnection(stringconn);
            conn.Open();
            var command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            return true;
        }
        public class qwe
        {
            public string st;
            public string pay;
            public qwe(string st, string pay)
            {
                this.st = st;
                this.pay = pay;
            }
        }
    }
}
