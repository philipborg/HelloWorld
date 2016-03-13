using Starcounter;
using System;

namespace HelloWorld {
    [Database]
    public class Like {
        public Likeable ToWhat;
        public string UserToken;

        public bool HasLike(Likeable subject, string token) {
            var existing = Db.SQL("SELECT o FROM \"Like\" o WHERE o.ToWhat = ? AND o.UserToken = ?", subject, token).First;
            if (existing == null) {
                return false;
            }
            return true;
        }
    }
}
