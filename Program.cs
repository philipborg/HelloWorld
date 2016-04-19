using System;
using Starcounter;

namespace HelloWorld {
    class Program {

        static void Main() {

            //on the first run, create the initial Spender object
            var spender = Db.SQL<Spender>("SELECT s FROM Spender s").First;
            if (spender == null) {
                Db.Transact(() => {
                    new Spender() {
                        Name = "Martin's Expenses List"
                    };
                });
            }

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            //create HTTP handler
            Handle.GET("/HelloWorld", () => {
                return Db.Scope(() => {
                    var _spender = Db.SQL<Spender>("SELECT s FROM Spender s").First;
                    var json = new ExpensesList() {
                        Data = _spender
                    };
                    foreach(var expense in _spender.Expenses) {
                        json.ExpenseItems.Add(Self.GET("/HelloWorld/expense/" + expense.GetObjectID()));
                    }
                    json.NewExpense.Data = new Expense() {
                        Amount = 1
                    };
                    json.Session = new Session(SessionOptions.PatchVersioning);
                    return json;
                });
            });

            Handle.GET("/HelloWorld/expense/{?}", (string ObjectId) => {
                var expense = DbHelper.FromID(DbHelper.Base64DecodeObjectID(ObjectId));
                var json = new ExpenseItem() {
                    Data = expense
                };
                return json;
            });
        }
    }
}