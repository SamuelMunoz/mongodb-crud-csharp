using System;
using MongoDB.Bson;

namespace MongoDBTest
{
    public class Usuarios
    {
        public ObjectId Id { get; set; }
        public String Usuario { get; set; }
        public String Password { get; set; }
    }
}
