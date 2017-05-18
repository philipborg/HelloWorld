using System;
using Starcounter;
using System.Linq;
using System.Collections.Generic;

namespace HelloWorld
{
    partial class PersonJson : Json, IBound<Spender>
    {
        public string FullName => FirstName + " " + LastName;

        public void DeleteAllExpenses()
        {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Data);
            this.Expenses.Clear();
        }

        void Handle(Input.NewExpenseTrigger action)
        {
            new Expense()
            {
                Spender = this.Data,
                Amount = 1
            };

            this.PopulateExpenses();
        }

        public void PopulateExpenses()
        {
            IEnumerable<ExpenseJson> expenseJson = this.Data.Expenses.Select(x => Self.GET<ExpenseJson>("/HelloWorld/partial/expense/" + x.GetObjectID()));
            this.Expenses.Clear();
            expenseJson.ToList().ForEach(x => this.Expenses.Add(x));
        }
    }
}