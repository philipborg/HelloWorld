using Starcounter;

namespace HelloWorldPart {
    class Program {
        static void Main() {
            Handle.GET("/helloworldpart/partials/helloworldpartannouncment/{?}", (string objectId) =>
            {
                var page = new JsonWithSubPage();
                page.SubPage = Self.GET("/helloworldpart/partials/helloworldpart/" + objectId);
                return page;
            });

            Handle.GET("/helloworldpart/partials/helloworldpart/{?}", (string objectId) => {
                return new Page();
            });

            UriMapping.OntologyMap("/helloworldpart/partials/helloworldpartannouncment/@w", "simplified.ring1.Illustration");
            UriMapping.OntologyMap("/helloworldpart/partials/helloworldpart/@w", "simplified.ring1.Somebody");
        }
    }
}