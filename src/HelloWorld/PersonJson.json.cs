using Starcounter;
using System.Collections.Generic;

namespace HelloWorld
{
    partial class PersonJson : Json, IBound<Spender>
    {
        public string FullName => FirstName + " " + LastName;

        void Handle(Input.Save action)
        {
            Transaction.Commit();
        }

        void Handle(Input.Cancel action)
        {
            Transaction.Rollback();
            RefreshExpenses(this.Data.Spendings);
        }

        void Handle(Input.AddNewExpense action)
        {
            var expense = new Expense()
            {
                Spender = (Spender) this.Data,
                Amount = 1
            };
            AddExpense(expense);
        }

        void Handle(Input.DeleteAll action)
        {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Data);
            this.Expenses.Clear();
        }

        public void AddExpense(Expense expense)
        {
            var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
            this.Expenses.Add(expenseJson);
        }

        public void RefreshExpenses(IEnumerable<Expense> expenses)
        {
            this.Expenses.Clear();
            foreach (Expense expense in expenses)
            {
                AddExpense(expense);
            }
        }
    }
}