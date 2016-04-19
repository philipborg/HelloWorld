using Starcounter;
using System.Collections.Generic;

namespace HelloWorld {
    [Database]
    public class Person {
        public string FirstName;
        public string LastName;

        public IEnumerable<Expense> Expenses => Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);

        public decimal Total => Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
    }
}
