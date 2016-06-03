using Starcounter;

namespace HelloWorld {
    partial class PersonJson : Json {
        public string _FullName => FirstName + " " + LastName;

        protected override void OnData() {
            base.OnData();
            this.RefreshExpenses();
        }

        protected void RefreshExpenses() {
            Person person = this.Data as Person;

            this.Expenses.Clear();

            if (person == null) {
                return;
            }

            var expenses = person.Spendings;

            foreach (var expense in expenses) {
                var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
                this.Expenses.Add(expenseJson);
            }
        }

        void Handle(Input.Save action) {
            Transaction.Commit();
        }

        void Handle(Input.Cancel action) {
            Transaction.Rollback();
            this.RefreshExpenses();
        }

        void Handle(Input.AddNewExpense action) {
            var expense = new Expense() {
                Spender = (Person)this.Data,
                Amount = 1
            };
            var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
            this.Expenses.Add(expenseJson);
        }

        void Handle(Input.DeleteAll action) {
            this.Expenses.Clear();
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Data);
        }
    }
}
