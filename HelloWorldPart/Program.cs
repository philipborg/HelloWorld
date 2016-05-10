using System;
using Starcounter;

namespace HelloWorldPart {
    class Program {
        static void Main() {
            Handle.GET("/helloworldpart/partials/helloworldpartannouncment/{?}", (string objectId) =>
            {
                var page = Self.GET("/helloworldpart/partials/helloworldpart/" + objectId);
                return page;
            });

            Handle.GET("/helloworldpart/partials/helloworldpart/{?}", (string objectId) => null);

            UriMapping.OntologyMap("/helloworldpart/partials/helloworldpartannouncment/@w", "simplified.ring1.Illustration", null, null);
            UriMapping.OntologyMap("/helloworldpart/partials/helloworldpart/@w", "simplified.ring1.Somebody", objectId => objectId, objectId => null);
        }
    }
}