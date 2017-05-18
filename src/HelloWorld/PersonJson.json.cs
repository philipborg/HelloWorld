using Starcounter;

namespace HelloWorld
{
    partial class PersonJson : Json, IBound<Spender>
    {
        static PersonJson()
        {
            DefaultTemplate.Expenses.ElementType.InstanceType = typeof(ExpenseJson);
        }

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
        }
    }
}