using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;

namespace HelloWorld
{
    [Database]
    public class Spender : Person
    {
        public QueryResultRows<Expense> Expenses => Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);
        public decimal CurrentBalance => Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
    }

    [Database]
    public class Expense : Something
    {
        public Spender Spender;
        public decimal Amount;
    }

    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                var anyone = Db.SQL<Spender>("SELECT s FROM Spender s").First;
                if (anyone == null)
                {
                    new Spender()
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                }
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/HelloWorld/Controls", () => new ControlsJson());

            Handle.GET("/HelloWorld", () =>
            {
                return Db.Scope(() =>
                {
                    var person = Db.SQL<Spender>("SELECT s FROM Spender s").First;

                    if (Session.Current == null)
                    {
                        Session.Current = new Session(SessionOptions.PatchVersioning);
                    }

                    var json = new PersonJson()
                    {
                        Data = person,
                        Controls = Self.GET<ControlsJson>("/HelloWorld/Controls"),
                        Session = Session.Current
                    };

                    json.PopulateExpenses();

                    return json;
                });
            });

            Hook<Expense>.CommitDelete += (s, obj) =>
            {
                var json = Session.Current.Data as PersonJson;
                json.PopulateExpenses();
            };

            Handle.GET("/HelloWorld/partial/expense/{?}", (string id) => new ExpenseJson { Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id)) });

            Blender.MapUri<Something>("/HelloWorld/partial/expense/{?}");
            Blender.MapUri("/HelloWorld/Controls", "controls");
        }
    }
}