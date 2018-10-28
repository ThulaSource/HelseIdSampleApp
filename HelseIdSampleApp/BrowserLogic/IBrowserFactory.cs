using System;
using System.Windows.Forms;

namespace HelseIdSampleApp.BrowserLogic
{
    public interface IBrowserFactory
    {
        IBrowserOperations Browser { get; }
    }

    public class BrowserFactory : IBrowserFactory
    {
        private IBrowserOperations browser;
        public IBrowserOperations Browser
        {
            get
            {
                if (browser != null)
                {
                    return browser;
                }
                HelseIdEnabler authForm = null;
                foreach (var openForm in Application.OpenForms)
                {
                    if (openForm is HelseIdEnabler)
                    {
                        authForm = (HelseIdEnabler)openForm;
                        break;
                    }
                }

                return browser = authForm;
            }
        }
    }
}