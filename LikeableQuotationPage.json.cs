using Starcounter;

namespace HelloWorld {
    partial class LikeableQuotationPage : Partial, IBound<Likeable> {
        public string UserToken;

        public Like GetLike(string token) {
            var existing = Db.SQL<Like>("SELECT o FROM \"Like\" o WHERE o.ToWhat = ? AND o.UserToken = ?", Data, token).First;
            return existing;
        }

        public bool _HasMyLike {
            get {
                var myLike = GetLike(UserToken);
                return (myLike != null);
            }
        }

        void Handle(Input.ToggleLike action) {
            var myLike = GetLike(UserToken);
            if (myLike == null) {
                Db.Transact(() => {
                    new Like() {
                        ToWhat = Data,
                        UserToken = UserToken
                    };
                });
            }
            else {
                Db.Transact(() => {
                    myLike.Delete();
                });
            }
        }
    }
}
