using Starcounter;

namespace HelloWorld {
    [Database]
    public class Spender {
        public string Name;

        public QueryResultRows<Expense> Expenses {
            get {
                return Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);
            }
        }

        public decimal Total {
            get {
                return Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
            }
        }
    }
}
