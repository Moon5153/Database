using System;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;

namespace TestProj1
{
    public class Fruit
    {
        //By convention, a property named Id or <type name>Id will be  
        //configured as the key of an entity.
        public int FruitId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class Planet
    {
        //By convention, a property named Id or <type name>Id will be  
        //configured as the key of an entity.
        public int PlanetId { get; set; }
        public string Name2 { get; set; }
        public string Color2 { get; set; }
    }

    public class MyDbContext : DbContext
    {
        //Fruit is an entity type (see documentation for DbSet)
        public DbSet<Fruit> FruitDbSet { get; set; }

        //Planet is any Entity type (see documentation for DbSet)
        public DbSet<Planet> PlanetDbSet { get; set; }

        public MyDbContext() : base() { }


    }
    class CodeToDatabaseToCode
    {
        public static void Main(string[] args)
        {
            for (; ;)
            {
                Console.WriteLine("Options:");
                Console.WriteLine("0: quit");
                Console.WriteLine("1: create Database");
                Console.WriteLine("2: add Records");
                Console.WriteLine("3: intro to DataSource Example QSyntax");
                Console.WriteLine("4: projection Example QSyntax");
                Console.WriteLine("5: cross Join Example QSyntax");
                Console.WriteLine("6: print record with KeyValue 2 From FruitDbSet");
                Console.WriteLine("7: delete record with KeyValue 2 From FruitDbSet");
                Console.WriteLine("8: delete records with Color Green From FruitDbSet");
                Console.WriteLine("9: add one record to FruitDbSet");
                Console.WriteLine("10: delete all records From FruitDbSet");
                Console.Write("Enter option: ");
                int i = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("");
                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                        { createDatabase(); }
                        break;
                    case 2:
                        { addRecords(); }
                        break;
                    case 3:
                        { introDataSourceExampleQSyntax(); Console.WriteLine(""); }
                        break;
                    case 4:
                        { projectionExampleQSyntax(); Console.WriteLine(""); }
                        break;
                    case 5:
                        { crossJoinExampleQSyntax(); Console.WriteLine(""); }
                        break;
                    case 6:
                        {
                            printRecordWithKeyValue2FromFruitDbSet();
                            Console.WriteLine("");
                        }
                        break;
                    case 7:
                        {
                            deleteRecordWithKeyValue2FromFruitDbSet();
                            Console.WriteLine("");
                        }
                        break;
                    case 8:
                        {
                            deleteRecordsWithColorGreenFromFruitDbSet();
                            Console.WriteLine("");
                        }
                        break;
                    case 9:
                        { addOneRecordToFruitDbSet(); Console.WriteLine(""); }
                        break;
                    case 10:
                        { deleteAllRecordsFromFruitDbSet(); Console.WriteLine(""); }
                        break;

                    default:
                        break;
                }
                if (0 == i)
                    break; //break from for loop
            }          
        }

        private static void createDatabase()
        {
            //When database is created, its "by-convention" name 
            //is the full name (namespace + class name) 
            //of the derived context class.
            //For exampple, in our case the name should be
            //TestProj1.MyDbContext
            using (var context = new MyDbContext())
            {
                bool created = context.Database.CreateIfNotExists();
                if (false == created)
                    Console.WriteLine("Database already exists...");
            }
        }

        private static void addRecords()
        {
            Console.WriteLine(nameof(addRecords));

            using (var context = new MyDbContext())
            {
                Fruit[] fruitArray =
                {
                    new Fruit { Name = "Kiwi",     Color = "Green" },
                    new Fruit { Name = "Dates",    Color = "Pink" },
                    new Fruit { Name = "Grapes",   Color = "Green" },
                    new Fruit { Name = "Eggplant", Color = "Pink" },
                    new Fruit { Name = "Parsley",  Color = "Green" },
                    new Fruit { Name = "Celery",   Color = "Pink" }
                };
                Planet[] planetArray =
                {
                    new Planet { Name2 = "Earth",   Color2 = "Green" },
                    new Planet { Name2 = "Jupiter", Color2 = "Pink" }
                };
                //Begin writing to the database
                context.FruitDbSet.AddRange(fruitArray);
                context.PlanetDbSet.AddRange(planetArray);
                int records = context.SaveChanges(); //this line is important
                //End writing to the database
                Console.WriteLine(records + " records added");
                Console.WriteLine();
            }
        }

        /** LINQ example below - through Query Syntax
         */
        private static void introDataSourceExampleQSyntax()
        {
            using (var context = new MyDbContext())
            {
                //selects all data. The result type is IEnumerable<Fruit>
                var query = from f in context.FruitDbSet
                            select f;

                Console.WriteLine("introDataSourceExampleQSyntax: ");
                foreach (var item in query)
                {
                    Console.WriteLine(item.FruitId + " " +
                                      item.Name + " " +
                                      item.Color);
                }
            }
        }

        /** LINQ example below - through Query Syntax
         */
        private static void projectionExampleQSyntax()
        {
            using (var context = new MyDbContext())
            {
                //selects the data corresponding to Name attribute. 
                //The result type is IEnumerable<string>
                var query = from f in context.FruitDbSet
                            select f.Name;

                Console.WriteLine("projectionExampleQSyntax: ");
                foreach (var item in query)
                {
                    Console.WriteLine(item + " ");
                }
            }
        }

        private static void crossJoinExampleQSyntax()
        {
            using (var context = new MyDbContext())
            {
                //Cross Join: This returns the Cartesian Product of the two sets. 
                //So if the first collection contains 6 items and the second 
                //collection contains 2 items then the resulting join will have 
                //6 x 2 = 12 items.
                var query = from f in context.FruitDbSet
                            from p in context.PlanetDbSet
                            select new { f.Name, f.Color, p.Name2, p.Color2 };
                Console.WriteLine("crossJoinExampleQSyntax: ");
                foreach (var item in query)
                    Console.WriteLine(item.Name + " " +
                                      item.Color + " " +
                                      item.Name2 + " " +
                                      item.Color2);
            }
        }

        //find the record (i.e. an instance of entity Fruit) with 
        //Key Value 2 From FruitDbSet
        private static void printRecordWithKeyValue2FromFruitDbSet()
        {
            using (var context = new MyDbContext())
            {
                var fruitList = context.FruitDbSet.ToList();
                foreach (var f in fruitList)
                {
                    if (f.FruitId == 2)
                    {
                        Console.WriteLine(f.FruitId + " " +
                                          f.Name + " " +
                                          f.Color);
                        break;
                    }
                }
            }
        }

        private static void deleteRecordWithKeyValue2FromFruitDbSet()
        {
            using (var context = new MyDbContext())
            {
                var fruitList = context.FruitDbSet.ToList();
                foreach (Fruit f in fruitList)
                {
                    if (f.FruitId == 2)
                    {
                        Console.WriteLine("Record to be deleted from FruitDbSet:");
                        Console.WriteLine(f.FruitId + " " +
                                          f.Name + " " +
                                          f.Color);

                        context.FruitDbSet.Remove(f);
                        context.SaveChanges(); //this line is important
                        break;
                    }
                }
            }
        }

        private static void deleteRecordsWithColorGreenFromFruitDbSet()
        {
            using (var context = new MyDbContext())
            {
                var fruitList = (from f in context.FruitDbSet
                                 where f.Color == "Green"
                                 select f).ToList();

                Console.WriteLine("Records to be deleted from FruitDbSet:");
                foreach (var f in fruitList)
                {
                    Console.WriteLine(f.FruitId + " " +
                                  f.Name + " " +
                                  f.Color);
                }
                context.FruitDbSet.RemoveRange(fruitList);
                context.SaveChanges(); //this line is important
            }
        }

        private static void addOneRecordToFruitDbSet()
        {
            using (var context = new MyDbContext())
            {
                var fruit = new Fruit { Name = "Guava", Color = "Pink" };
                Console.WriteLine("Record to be added to FruitDbSet is:");
                Console.WriteLine(fruit.Name + " " +
                                  fruit.Color);
                context.FruitDbSet.Add(fruit);
                context.SaveChanges();
            }
        }

        private static void deleteAllRecordsFromFruitDbSet()
        {
            using (var context = new MyDbContext())
            {
                var fruitList = (from f in context.FruitDbSet
                                 select f).ToList();

                Console.WriteLine("All existing records to be deleted from FruitDbSet are:");
                foreach (var f in fruitList)
                {
                    Console.WriteLine(f.FruitId + " " +
                                  f.Name + " " +
                                  f.Color);
                }
                context.FruitDbSet.RemoveRange(fruitList);
                context.SaveChanges();
            }
        }
    }   
}
