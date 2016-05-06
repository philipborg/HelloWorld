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

            Handle.GET("/helloworld/partials/master", () => {
                var json = new MasterPage();
                json.Session = new Session(SessionOptions.PatchVersioning);
                return json;
            });

            Handle.GET("/helloworld/partials/helloworld/{?}", (string objectId) => {
                var json = new PersonJson();
                json.Data = Db.SQL<Person>("SELECT p FROM Person p WHERE p.LastName = ?", "Doe").First;
                return json;
            });

            Handle.GET("/HelloWorld", () =>
            {
                var master = (MasterPage)Self.GET("/helloworld/partials/master");
                master.CurrentPage = Self.GET("/helloworld/partials/helloworld/test");
                return master;
            });

            Handle.GET("/helloworld/partials/injectionpage/{?}", (string objectId) => {
                var json = new InjectionJson();
                return json;
            });

            UriMapping.OntologyMap("/helloworld/partials/helloworld/@w", "simplified.ring1.Illustration", null, null);
            UriMapping.OntologyMap("/helloworld/partials/injectionpage/@w", "simplified.ring1.Somebody", null, null);
        }
    }
}