using System;
using Starcounter;

namespace HelloWorld {
    class Program {

        static void Main() {

            //on the first run, create the initial Person object
            var person = Db.SQL<Person>("SELECT p FROM Person p").First;
            if (person == null) {
                Db.Transact(() => {
                    new Person() {
                        FirstName = "Martin"
                    };
                });
            }

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            //create HTTP handler
            Handle.GET("/HelloWorld", () => {
                return Db.Scope(() => {
                    var json = new PersonJson() {
                        Data = Db.SQL<Person>("SELECT p FROM Person p").First
                    };
                    json.NewExpense.Data = new Expense() {
                        Amount = 1
                    };
                    json.Session = new Session(SessionOptions.PatchVersioning);
                    return json;
                });
            });
        }
    }
}