using System;
using Starcounter;
using System.Diagnostics;

namespace HelloWorld {
    [Database]
    public class Person {
        public string FirstName;
        public string LastName;
        public Person Parent;
        public Int64 AncestorsCount {
            get {
                /*if (Parent != null) {
                    return Parent.AncestorsCount + 1;
                }
                return 0;*/

                Int64 count = 0;
                Person guy = Parent;
                while (guy != null) {
                    guy = guy.Parent;
                    count++;
                }

                return count;


            }
        }
    }

    class Program {
        static void Main() {
            Stopwatch timer = new Stopwatch();
            

            Db.Transact(() => {
                Db.SlowSQL("DELETE FROM Person");
            });


            //CREATE
            timer.Restart();
            var max = 1000000;
            Person lastGuy = null;
            for (var i = 0; i < max; i++) {
                Db.Transact(() => {
                    var guy = new Person() {
                        FirstName = "Marcin",
                        LastName = "Warpechowski",
                        Parent = lastGuy
                    };
                    lastGuy = guy;
                });
            }
            timer.Stop();
            Console.WriteLine("Create: " + timer.ElapsedMilliseconds + " ms");
            Console.WriteLine("That's " + Math.Abs(max * 1000 / timer.ElapsedMilliseconds) + " tps");

            //READ AGGREGATE
            Console.WriteLine("-----");
            timer.Restart();
            var number = Db.SlowSQL<Int64>("SELECT COUNT(*) FROM Person p").First;
            //var number = 0;
            timer.Stop();
            Console.WriteLine("Found " + number + " people");
            Console.WriteLine("Read aggregate: " + timer.ElapsedMilliseconds + " ms");

            //READ CALCULATED PROPERTY
            Console.WriteLine("-----");
            timer.Restart();
            Console.WriteLine("Last person has " + lastGuy.AncestorsCount + "  ancestors");
            timer.Stop();
            Console.WriteLine("Read calculated property: " + timer.ElapsedMilliseconds + " ms");


            
            //Console.WriteLine("Duration " + timer.ElapsedMilliseconds + " ms");
            
            

        }
    }
}