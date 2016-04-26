using Starcounter;

namespace HelloWorld
{
    [Database]
    public class Person
    {
        public string FirstName;
        public string LastName;

        public QueryResultRows<Expense> Spendings
            => Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);

        public decimal CurrentBalance
            => Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
    }

    [Database]
    public class Expense
    {
        public Person Spender;
        public string Description;
        public decimal Amount;
    }

    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                var anyone = Db.SQL<Person>("SELECT p FROM Person p").First;
                if (anyone == null)
                {
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                }
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/HelloWorld", () =>
            {
                return Db.Scope(() =>
                {
                    var person = Db.SQL<Person>("SELECT p FROM Person p").First;
                    var json = new PersonJson()
                    {
                        Data = person
                    };

                    if (Session.Current == null)
                    {
                        Session.Current = new Session(SessionOptions.PatchVersioning);
                    }
                    json.Session = Session.Current;

                    if (person.Spendings != null)
                    {
                        json.RefreshExpenses(person.Spendings);
                    }

                    return json;
                });
            });

            Handle.GET("/HelloWorld/partial/expense/{?}", (string id) =>
            {
                var json = new ExpenseJson();
                json.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id));
                return json;
            });
        }
    }
}