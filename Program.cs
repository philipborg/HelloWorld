using System;
using Starcounter;

namespace HelloWorld {
    [Database]
    public class Person {
        public string FirstName;
        public string LastName;
    }

    class Program {
        static void Main() {
            var anyone = Db.SQL<Person>("SELECT p FROM Person p").First;
            if (anyone == null) {
                Db.Transact(() => {
                    new Person() {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                });
            }
        }
    }
}