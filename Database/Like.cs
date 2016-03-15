using Starcounter;
using System;

namespace HelloWorld {
    [Database]
    public class Like {
        public Likeable ToWhat;
        public string UserToken;
    }
}
