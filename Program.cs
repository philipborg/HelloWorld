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
            Db.Transact(() => {
                new Person() {
                    FirstName = "Johnny",
                    LastName = "Doe"
                };
            });
        }
    }
}