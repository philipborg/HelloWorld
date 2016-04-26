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
                    FirstName = "John",
                    LastName = "Doe"
                };
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/HelloWorld", () => {
                var json = new PersonJson();
                json.Data = Db.SQL<Person>("SELECT p FROM Person p WHERE p.LastName = ?", "Doe").First;
                json.Session = new Session(SessionOptions.PatchVersioning);
                return json;
            });
        }
    }
}