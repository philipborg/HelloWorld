using Starcounter;
using System;

namespace HelloWorld {
    [Database]
    public class Expense {
        public Person Spender;
        public string Description;
        public decimal Amount;
    }
}
