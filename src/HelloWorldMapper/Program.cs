using System;
using Starcounter;
using Starcounter.Internal;

namespace HelloWorldMapper
{
    class Program
    {
        static void Main()
        {
            UriMapping.OntologyMap<HelloWorld.Expense>("/HelloWorld/partial/expense/{?}");

            StarcounterEnvironment.RunWithinApplication("Images", () => {
                Handle.GET("/images/partials/concept-expense/{?}", (string objectId) => {
                    return Self.GET("/images/partials/somethings-edit/" + objectId);
                });

                UriMapping.OntologyMap<HelloWorld.Expense>("/images/partials/concept-expense/{?}");
            });


        }
    }
}