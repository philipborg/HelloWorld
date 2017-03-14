using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;

namespace HelloWorld
{
    [Database]
    public class Spender : Person
    {
        public QueryResultRows<Expense> Spendings
            => Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);

        public decimal CurrentBalance
            => Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
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
                    anyone = new Spender()
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };

                    new Expense()
                    {
                        Spender = anyone,
                        Description = "Milk",
                        Amount = 10m
                    };
                }
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/HelloWorld", () =>
            {
                var json = new MasterPage() {};

                if (Session.Current == null)
                {
                    Session.Current = new Session(SessionOptions.PatchVersioning);
                }
                json.Session = Session.Current;
                return json;
            });

            Handle.GET("/HelloWorld/subpage", () =>
            {
                var json = new SpenderPage();
                var person = Db.SQL<Spender>("SELECT s FROM Spender s").First;
                json.Data = person;
                return json;
            });
        }
    }
}