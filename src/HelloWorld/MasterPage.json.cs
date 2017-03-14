using Starcounter;

namespace HelloWorld
{
    partial class MasterPage : Json
    {
        void Handle(Input.ShowSubpage action)
        {
            CurrentPage = Self.GET("/HelloWorld/subpage");
        }
    }
}