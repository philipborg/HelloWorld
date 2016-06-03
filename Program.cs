using System;
using Starcounter;
using System.Collections.Generic;

namespace HelloWorld {
    [Database]
    public class Person {
        public string FirstName;
        public string LastName;
        public QueryResultRows<Expense> Spendings => Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);
        public decimal CurrentBalance => Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
    }

    [Database]
    public class Expense {
        public Person Spender;
        public string Description;
        public decimal Amount;
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

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/HelloWorld", () => {
                return Db.Scope(() => {
                    var person = Db.SQL<Person>("SELECT p FROM Person p").First;
                    var json = new PersonJson() {
                        Data = person
                    };

                    

                    json.Session = new Session(SessionOptions.PatchVersioning);
                    return json;
                });
            });

            Handle.GET("/HelloWorld/partial/expense/{?}", (string id) => {
                var json = new ExpenseJson();
                json.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id));
                return json;
            });
        }
    }
}