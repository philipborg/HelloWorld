using Starcounter;
using System;

namespace HelloWorld {
    [Database]
    public class Expense {
        public Spender Spender;
        public string Description;
        public decimal Amount;
    }
}
