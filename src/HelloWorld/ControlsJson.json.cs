using Starcounter;

namespace HelloWorld
{
    partial class ControlsJson : Json
    {
        private PersonJson parentJson => this.Parent as PersonJson;

        void Handle(Input.SaveTrigger action)
        {
            Transaction.Commit();
        }

        void Handle(Input.CancelTrigger action)
        {
            Transaction.Rollback();
        }

        void Handle(Input.NewExpenseTrigger action)
        {
            new Expense()
            {
                Spender = parentJson.Data as Spender,
                Amount = 1
            };
        }

        void Handle(Input.DeleteAllTrigger action)
        {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", parentJson.Data);
            parentJson.Expenses.Clear();
        }
    }
}
