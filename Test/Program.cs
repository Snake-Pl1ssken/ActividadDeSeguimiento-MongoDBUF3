using MongoDB.Bson;
using MongoDB.Driver;

namespace Test
{
    internal class Program
    {
        static string connectionString = "mongodb://localhost:27017";
        static MongoClient connection;

        static IMongoDatabase database;
        static IMongoCollection<BsonDocument> collection;

        static void Main(string[] args)
        {
            connection = new MongoClient(connectionString);

            Console.WriteLine("Connected");

            List<string> names = connection.ListDatabaseNames().ToList<string>();

            foreach (string name in names)
            {
                Console.WriteLine("Found database: " + name);
            }

            database = connection.GetDatabase("HeroesDB");

            collection = database.GetCollection<BsonDocument>("Heroes");

            int option = -1;

            while (option != 0)
            {
                Console.WriteLine("1.- Listar");
                Console.WriteLine("2.- Añadir");
                Console.WriteLine("3.- Buscar");
                Console.WriteLine("4.- Eliminar");
                Console.WriteLine("5.- Eliminar");
                Console.WriteLine("0.- Salir");
                Console.WriteLine("?>");
                option = Int32.Parse(Console.ReadLine());

                if (option == 1)
                {
                    List<BsonDocument> Docs = collection.Find<BsonDocument>("{}").ToList<BsonDocument>();

                    foreach (BsonDocument Doc in Docs)
                    {
                        Console.WriteLine(Doc["_id"].ToString());

                        foreach (BsonElement e in Doc.Elements)
                        {
                            Console.WriteLine(e.Name + ":" + e.Value);
                        }
                        Console.WriteLine("--------------------------------");
                    }
                }
                else if (option == 2)
                {
                    var doc = new BsonDocument();

                    string nombre;
                    string publisher;

                    Console.WriteLine("Nombre?>");
                    nombre = Console.ReadLine();
                    Console.WriteLine("Publicador?>");
                    publisher = Console.ReadLine();

                    BsonElement e = new BsonElement("superhero", new BsonString(nombre));
                    doc.Add(e);

                    e = new BsonElement("publisher", new BsonString(publisher));
                    doc.Add(e);
                    collection.InsertOne(doc);
                }
                else if (option == 3)
                {
                    Console.WriteLine("Nombre?>");
                    string name = Console.ReadLine();

                    IFindFluent<BsonDocument, BsonDocument> findResult = collection.Find<BsonDocument>("{ superhero: '" + name + "'}");

                    List<BsonDocument> list = findResult.ToList<BsonDocument>();

                    foreach (BsonDocument Doc in list)
                    {
                        Console.WriteLine(Doc["_id"].ToString());

                        foreach (BsonElement e in Doc.Elements)
                        {
                            Console.WriteLine(e.Name + ":" + e.Value);
                        }
                        Console.WriteLine("--------------------------------");
                    }
                }
                else if (option == 4)
                {
                    Console.WriteLine("id?>");
                    string idText = Console.ReadLine();
                    ObjectId id = ObjectId.Parse(idText);

                    List<BsonDocument> list = collection.Find<BsonDocument>("{ _id: objectId('" + id.ToString() + "')}").ToList<BsonDocument>();

                    if (list.Count > 0)
                    {
                        BsonDocument doc = list.First();

                        collection.DeleteOne(doc);
                    }
                    else
                    {
                        Console.WriteLine("Not Found");
                    }
                }
                else if (option == 5)
                {
                    Console.WriteLine("id?>");
                    string idText = Console.ReadLine();
                    ObjectId id = ObjectId.Parse(idText);
                    string query = "{ _id: objectId('" + id.ToString() + "')}";
                    Console.WriteLine(query);

                    List<BsonDocument> list = collection.Find<BsonDocument>(query).ToList<BsonDocument>();

                    if (list.Count > 0)
                    {
                        BsonDocument doc = list.First();

                        Console.WriteLine("name?>");
                        string newNAME = Console.ReadLine();
                        Console.WriteLine("Publisher?>");
                        string newPUBLISHER = Console.ReadLine();

                        doc["superhero"] = newNAME;
                        doc["publlisher"] = newPUBLISHER;

                        collection.ReplaceOne(query, doc);
                    }
                    else
                    {
                        Console.WriteLine("Not Found");
                    }
                }
            }   
        }
    }
}

//reatrear en una coleccion todo el seguimiento de los usuarios osea cada que hace click en una accion hay que guardarlo en la base de datos
