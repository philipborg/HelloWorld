using System;
using Simplified.Ring1;
using Starcounter;
using System.Collections.Generic;

namespace DuplicateRemover
{
    partial class RemoveDuplicatesButton : Json
    {
        void Handle(Input.RemoveDuplicatesTrigger action)
        {
            QueryResultRows<Something> items = Db.SQL<Something>("SELECT s FROM Simplified.Ring1.Something s");

            FindDuplicates(items);
        }

        private void FindDuplicates(QueryResultRows<Something> items)
        {
            List<string> descriptions = new List<string>();
            foreach(Something item in items)
            {
                if (descriptions.Contains(item.Description)){
                    item.Delete();
                } else
                {
                    descriptions.Add(item.Description);
                }
            }
        }
    }
}
