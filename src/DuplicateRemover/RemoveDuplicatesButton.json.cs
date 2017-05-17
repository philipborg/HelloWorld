using System;
using Simplified.Ring1;
using Starcounter;
using System.Collections.Generic;
using System.Linq;

namespace DuplicateRemover
{
    partial class RemoveDuplicatesButton : Json
    {
        void Handle(Input.RemoveDuplicatesTrigger action)
        {
            QueryResultRows<Something> items = Db.SQL<Something>("SELECT s FROM Simplified.Ring1.Something s");

            DeleteDuplicates(items);
        }

        private void DeleteDuplicates(QueryResultRows<Something> items)
        {
            List<string> descriptions = new List<string>();
            foreach (Something item in items)
            {
                if (item.GetType().Name == "Expense")
                {
                    if (descriptions.Contains(item.Description))
                    {
                        if (Transaction != null)
                        {
                            item.Delete();
                        }
                        else
                        {
                            Db.Transact(() => item.Delete());
                        }
                    }
                    else
                    {
                        descriptions.Add(item.Description);
                    }
                }
            }
        }
    }
}
