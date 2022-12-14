using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Sample
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } 
        [BsonElement("msg")]
        public string Msg { get; set; } 
        [BsonElement("currentTime")]
        public DateTime CurrentTime { get; set; }
    }
}