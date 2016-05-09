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
            Application.Current.Use(new JsonAutoSessions());

            Handle.GET("/HelloWorld", () => {
                return Db.Scope(() => {
                    var person = Db.SQL<Person>("SELECT p FROM Person p WHERE p.LastName = ?", "Doe").First;
                    var json = new PersonJson() {
                        Data = person
                    };
                    //json.Session = new Session(SessionOptions.PatchVersioning);
                    return json;
                });
            });
        }
    }
}