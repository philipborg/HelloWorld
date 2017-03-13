using Starcounter;
using System.Collections.Generic;

namespace HelloWorld
{
    partial class PersonJson : Json, IBound<Spender>
    {
        public string FullName => FirstName + " " + LastName;

        void Handle(Input.Save action)
        {
            

            Transaction.Commit();
            CurrentPage = Self.GET("/helloworld/personwithexpenses");


        }
    }
}