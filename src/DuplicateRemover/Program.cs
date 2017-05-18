using Starcounter;
using Simplified.Ring1;

namespace DuplicateRemover
{
    class Program
    {
        static void Main()
        {
            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/DuplicateRemover/partial/RemoveDuplicatesButton", () =>
            {
                return new RemoveDuplicatesButton();
            });

            Handle.GET("/DuplicateRemover/partial/RemoveButton/{?}", (string objectId) =>
            {
                return new RemoveButton() { ObjectKey = objectId };
            });

            Blender.MapUri<Something>("/DuplicateRemover/partial/RemoveButton/{?}");
            Blender.MapUri("/DuplicateRemover/partial/RemoveDuplicatesButton", "controls");
        }
    }
}