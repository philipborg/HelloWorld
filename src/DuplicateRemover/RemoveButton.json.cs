using Simplified.Ring1;
using Starcounter;
using System.Linq;

namespace DuplicateRemover
{
    partial class RemoveButton : Json
    {
        void Handle(Input.RemoveTrigger action)
        {
            Something item = Db.SQL<Something>("SELECT s FROM Simplified.Ring1.Something s WHERE s.ObjectId = ?", this.Key).FirstOrDefault();

            Db.Transact(() => item.Delete());

        }
    }
}
