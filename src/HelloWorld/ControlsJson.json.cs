using Starcounter;

namespace HelloWorld
{
    partial class ControlsJson : Json
    {
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
                Spender = (this.Parent as PersonJson).Data as Spender,
                Amount = 1
            };
        }

        void Handle(Input.DeleteAllTrigger action)
        {
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Parent.Data);
            (this.Parent as PersonJson).Expenses.Clear();
        }
    }
}
