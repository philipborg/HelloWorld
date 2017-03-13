using Starcounter;

namespace HelloWorld
{
    partial class PersonWithExpenses : Json, IBound<Spender>
    {
        [PersonWithExpenses_json.Spendings]
        partial class PersonWithExpensesItem : Json, IBound<Expense>
        {
            protected override void OnData()
            {
                Session.CalculatePatchAndPushOnWebSocket();
                
            }
        }
    }

    
}
