using Starcounter;

namespace HelloWorld
{
    partial class PersonJson : Json
    {
        void Handle(Input.Save action)
        {
            Transaction.Commit();
        }
    }
}