using Starcounter;
using System.Collections.Generic;

namespace HelloWorld
{
    partial class PersonJson : Json, IBound<Person>
    {
        public string FullName => FirstName + " " + LastName;

        void Handle(Input.SaveTrigger action)
        {
            Transaction.Commit();
        }

        void Handle(Input.CancelTrigger action)
        {
            Transaction.Rollback();
            RefreshExpenses(this.Data.Spendings);
        }

        void Handle(Input.AddNewExpenseTrigger action)
        {
            var expense = new Expense()
            {
                Spender = (Person) this.Data,
                Amount = 1
            };
            AddExpense(expense);
        }

        void Handle(Input.DeleteAllTrigger action)
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