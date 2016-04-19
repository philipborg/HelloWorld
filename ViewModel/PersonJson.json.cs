using Starcounter;

namespace HelloWorld {
    partial class PersonJson : Json, IBound<Person> {
        void Handle(Input.ClearAll action) {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Data);
            Transaction.Commit();
        }

        void Handle(Input.Add action) {
            if (this.NewExpense.Amount > 0) {
                this.NewExpense.Data.Spender = this.Data;
                Transaction.Commit();
                this.NewExpense.Data = new Expense() { Amount = 1 };
            }
        }
    }

    [PersonJson_json.NewExpense]
    partial class NewExpenseJson : Json, IBound<Expense> {

    }
}
