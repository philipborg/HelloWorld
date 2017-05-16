using Starcounter;

namespace HelloWorld
{
    partial class PersonJson : Json
    {
        static PersonJson()
        {
            DefaultTemplate.Expenses.ElementType.InstanceType = typeof(ExpenseJson);
        }

        public string FullName => FirstName + " " + LastName;
    }
}