using Starcounter;
using System;

namespace HelloWorld {
    [Database]
    public class Likeable {
        public Quotation What;
        public Int64 LikesCount {
            get {
                return Db.SlowSQL<Int64>("SELECT COUNT(*) FROM HelloWorld.\"Like\" o WHERE o.ToWhat = ?", this).First;
            }
        }
        public string Key {
            get {
                return this.GetObjectID();
            }
        }

    }
}
