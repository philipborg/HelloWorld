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

        void Handle(Input.AddNewExpense action)
        {
            Transaction.Commit();

            var expense = new Expense()
            {
                Spender = (Person) this.Data,
                Amount = 1
            };
            var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
            this.Expenses.Add(expenseJson);
        }
    }
}