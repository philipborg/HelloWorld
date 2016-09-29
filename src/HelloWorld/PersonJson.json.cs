using Starcounter;

namespace HelloWorld
{
    partial class PersonJson : Json
    {
        public string FullName => FirstName + " " + LastName;

        void Handle(Input.Save action)
        {
            Transaction.Commit();
        }

        void Handle(Input.Cancel action)
        {
            Transaction.Rollback();
        }

        void Handle(Input.AddNewExpense action)
        {
            var expense = new Expense()
            {
                Spender = (Spender) this.Data,
                Amount = 1
            };
            var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
            this.Expenses.Add(expenseJson);
        }

        void Handle(Input.DeleteAll action)
        {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Data);
            this.Expenses.Clear();
        }
    }
}