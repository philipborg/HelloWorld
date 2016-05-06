using Starcounter;

namespace HelloWorld {
    partial class InjectionJson : Page
    {
        void Handle(Input.Announce Action)
        {
            var session = Session.Current;
        }
    }
}
