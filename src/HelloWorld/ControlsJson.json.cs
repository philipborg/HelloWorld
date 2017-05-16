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
            parentJson.AddNewExpense();
        }

        void Handle(Input.DeleteAllTrigger action)
        {
            parentJson.DeleteAllExpenses();
        }
    }
}
