using System;
using Starcounter;

namespace HelloWorldMapper
{
    class Program
    {
        static void Main()
        {
            UriMapping.OntologyMap<HelloWorld.Expense>("/HelloWorld/partial/expense/{?}");

            UriMapping.OntologyMap<HelloWorld.Expense>("/images/concept/{?}");
        }
    }
}