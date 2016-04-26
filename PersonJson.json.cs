using Starcounter;

namespace HelloWorld {
    partial class PersonJson : Json {
        public string _FullName => FirstName + " " + LastName;

        void Handle(Input.Save action) {
            Transaction.Commit();
        }
    }
}
