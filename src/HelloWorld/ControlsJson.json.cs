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

        void Handle(Input.DeleteAllTrigger action)
        {
            (this.Parent as PersonJson).DeleteAllExpenses();
        }
    }
}
