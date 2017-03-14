using Starcounter;

namespace HelloWorld
{
    partial class SpenderPage : Json, IBound<Spender>
    {
        protected override void OnData()
        {
            //unless you comment the below line, the app crashes (Session == null)
            Session.CalculatePatchAndPushOnWebSocket();
        }

        [SpenderPage_json.Spendings]
        partial class SpendingsItem : Json, IBound<Expense>
        {
            protected override void OnData()
            {
                //unless you comment the below line, SpenderPage response is not wrapped in a namespace
                Session.CalculatePatchAndPushOnWebSocket();
            }
        }
    }

    
}
