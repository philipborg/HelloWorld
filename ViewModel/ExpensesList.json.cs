using Starcounter;

namespace HelloWorld {
    partial class ExpensesList : Json, IBound<Spender> {
        void Handle(Input.ClearAll action) {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", Data);
            Transaction.Commit();
        }
    }

    [ExpensesList_json.NewExpense]
    partial class ExpensesListNewExpense : Json, IBound<Expense> {
        void Handle(Input.Add action) {
            if (Amount > 0) {
                Spender spender = (Parent as ExpensesList).Data;
                Data.Spender = spender;
                Transaction.Commit();
                Data = new Expense() {
                    Amount = 1
                };
            }
        }
    }
}
