using Starcounter;

namespace HelloWorld {
    partial class PersonJson : Json {
        void Handle(Input.Save action) {
            Transaction.Commit();
        }

        void Handle(Input.Cancel action) {
            Transaction.Rollback();
        }

        void Handle(Input.AddNewExpense action) {
            Transaction.Commit();

            var expense = new Expense() {
                Spender = (Person)this.Data,
                Amount = 1
            };
            var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
            this.Expenses.Add(expenseJson);
        }

        void Handle(Input.DeleteAll action) {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Data);
            Transaction.Commit();
            this.Expenses.Clear();
        }
    }
}
