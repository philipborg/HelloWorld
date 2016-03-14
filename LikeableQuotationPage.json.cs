using Starcounter;

namespace HelloWorld {
    partial class LikeableQuotationPage : Partial, IBound<Likeable> {

        public Like GetLike(string token) {
            var existing = Db.SQL<Like>("SELECT o FROM \"Like\" o WHERE o.ToWhat = ? AND o.UserToken = ?", Data, token).First;
            return existing;
        }

        public bool _HasMyLike {
            get {
                var sessionId = Session.ToAsciiString();
                var myLike = GetLike(sessionId);
                return (myLike != null);
            }
        }

        void Handle(Input.ToggleLike action) {
            var sessionId = Session.ToAsciiString();
            var myLike = GetLike(sessionId);
            if (myLike == null) {
                Db.Transact(() => {
                    new Like() {
                        ToWhat = Data,
                        UserToken = sessionId
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
