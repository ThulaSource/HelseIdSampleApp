
namespace HelseIdSampleApp.BrowserLogic
{
    public interface IBrowserOperations
    {
        EmbeddedBrowser WebBrowser { get; }

        void Navigate(string url);
    }
}