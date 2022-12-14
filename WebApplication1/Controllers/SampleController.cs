using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;

namespace WebApplication1.Controllers
{
   
   
   // [EnableCors(origins: "http://localhost:4200", headers: " Access-Control-Allow-Origin ", methods: "*")]
  // [EnableCors("AllowOrigin","*","*")]
    public class SampleController : ApiController
    {
        // GET: api/Sample
        [HttpGet]
        public string Get(string value1 , string value2)
        {
            string value3 = DateTime.Now.ToString();
            String[] message = new string[] { value1, value2, value3 };
            string valid = Validation(value1, value2);
            try
            {
                if (valid == "Success")
                {
                    WriteToFile(message);
                    Sample samplemsg = new Sample()
                    {
                        Name = message[0],
                        Msg = message[1],
                        CurrentTime = DateTime.Now
                    };
                    var client = new MongoClient("mongodb://localhost:27017");
                    var database = client.GetDatabase("newdb");
                    var collection = database.GetCollection<Sample>("Sample");
                    collection.InsertOne(samplemsg);
                    
                }
            }
            catch(Exception ex) { }
            return valid;
        }

        // GET: api/Sample/5
        public string Get(int id)
        {
            return "Siva";
        }

        // POST: api/Sample
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Sample/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sample/5
        public void Delete(int id)
        {
        }
        [Route("api/Sample/Get")]
        //  [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
      
        public List<Sample> GetAll()
        {
            List<Sample> showlist = new List<Sample>();
            try {
                
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("newdb");
                var collection = database.GetCollection<Sample>("Sample");
                showlist = collection.Find(name => true).ToList();
            }
            catch(Exception ex) {
                string[] ErrorMessage = new string[] { ex.Message };
                WriteToErrorLog(ErrorMessage);
            }
                
                return showlist;
              
        }
        private string Validation(string str1, string str2)
        {
            string val = "Success";
            string val1 = string.Empty;
            string val2 = string.Empty;
            // Regex objNotNaturalPattern = new Regex("[^0-9]");
            if (!Regex.Match(str1, @"^\d*[a-zA-Z]{1,}\d*$").Success)
            {
                val1 = "Invalid Client Name";
            }
            if (!Regex.Match(str2.Replace("<EOF>", ""), @"^[a-zA-Z]+$").Success)
            {
                val2 = "Invaid Message";
            }

            if (val1.Length > 0 || val2.Length > 0)
            {
                val = val1 + "  " + val2;
            }

            return val;
        }
        public void WriteToErrorLog(String[] Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Database";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Database\\Error Log" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("Error Message:  " + Message[0]);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("Error Message:  " + Message[0]);
                }
            }
        }
        public void WriteToFile(String[] Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Database";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Database\\Data Storage" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("Client Name: " + Message[0]);
                    sw.WriteLine("Message: " + Message[1]);
                    sw.WriteLine("Date: " + Message[2]);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("Client Name: " + Message[0]);
                    sw.WriteLine("Message: " + Message[1]);
                    sw.WriteLine("Date: " + Message[2]);
                }
            }
        }
    }
}
