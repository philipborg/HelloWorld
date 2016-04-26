using Starcounter;

namespace HelloWorld
{
    partial class PersonJson : Json
    {
        public string FullName => FirstName + " " + LastName;

        void Handle(Input.SaveTrigger action)
        {
            Transaction.Commit();
        }

        void Handle(Input.AddNewExpenseTrigger action)
        {
            Transaction.Commit();

            var expense = new Expense()
            {
                Spender = (Person) this.Data,
                Amount = 1
            };
            AddExpense(expense);
        }

        public void AddExpense(Expense expense)
        {
            var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
            this.Expenses.Add(expenseJson);
        }
    }
}