using System;
using Starcounter;

namespace HelloWorld {
    partial class InjectionJson : Page
    {
        void Handle(Input.Announce Action)
        {
            var currentPage = Session.Current.Data;
            if (currentPage == null)
            {
                throw new Exception("Empty current session data");
            }
        }
    }
}
