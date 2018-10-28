using CefSharp.WinForms;
using HelseIdSampleApp.BrowserLogic;
using HelseIdSampleApp.Configuration;
using HelseIdSampleApp.Logic;
using HelseIdSampleApp.OpenIdConnect.DCR.Api;
using HelseIdSampleApp.StructureMap;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelseIdSampleApp
{
    public partial class HelseIdEnabler : Form, IBrowserOperations
    {
        EmbeddedBrowser IBrowserOperations.WebBrowser => webBrowser == null ? webBrowser = new EmbeddedBrowser(chromiumWebBrowser, configurations.HelseIdRedirectUrl) : webBrowser;

        private readonly IConfigurations configurations;
        private readonly IAuthenticationOperations authenticationOperations;

        private EmbeddedBrowser webBrowser;
        private ChromiumWebBrowser chromiumWebBrowser;

        public HelseIdEnabler()
        {
            InitializeComponent();
            configurations = Bootstrapper.GetInstance<IConfigurations>();
            authenticationOperations = Bootstrapper.GetInstance<IAuthenticationOperations>();

            // "Fix" textbox 
            txtResult.BorderStyle = 0;
            txtResult.BackColor = pnlBottom.BackColor;
            
            DoubleBuffered = true;

            var browserCache = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.Combine("HelseIdSampleApp", "cache"));

            if (!Directory.Exists(browserCache))
            {
                Directory.CreateDirectory(browserCache);
            }

            CefSharp.Cef.Initialize(new CefSettings { CachePath = browserCache }, performDependencyCheck: false, browserProcessHandler: null);

            chromiumWebBrowser = new ChromiumWebBrowser("")
            {
                Dock = DockStyle.Fill
            };

            pnlControls.Controls.Add(chromiumWebBrowser);

            
        }

        public void Navigate(string url)
        {
            chromiumWebBrowser.Load(url);
        }

        private void btStoreClient_Click(object sender, EventArgs e)
        {
            btStoreClient.Enabled = false;
            UpdateResult("Performing user authentication...");
            
            Task.Run(() =>
            {
                try
                {
                    // Fetch a new HelseId Access Token using the authorization code grant type to access secured resources
                    var accessToken = authenticationOperations.IsAuthenticated
                        ? authenticationOperations.AccessToken
                        : authenticationOperations.InitAuthorizationCodeGrant();

                    if (string.IsNullOrWhiteSpace(accessToken))
                    {
                        throw new ArgumentException("No access token.");
                    }

                    UpdateResult("Fetched new access token, calling HelseId Dcr API to register new client...");

                    // Create a new HelseId client configuration 
                    var dcrService = new DcrService(new DcrServiceSettings { DcrApi = configurations.HelseIdApiEndpoint });

                    // Register new client
                    var client = dcrService.CreateClient(accessToken, DcrServiceSettings.DefaultGrantTypes, configurations.HelseIdRedirectUrl, null,
                        configurations.HelseIdScope.Split(' '), configurations.HelseIdOrganizationId, configurations.HelseIdOrganizationName);

                    var result = client.GetAwaiter().GetResult();

                    if (!string.IsNullOrWhiteSpace(result?.ClientId))
                    {
                        UpdateResult("New client registered, calling HelseId to set Org number...");

                        // Create association between organization and newly created client configuration
                        dcrService.SetOrgNumber(accessToken, result.ClientId, configurations.HelseIdOrganizationId);

                        UpdateResult($"Stored new client with id: {result.ClientId}");
                    }
                    else if (!string.IsNullOrWhiteSpace(result?.Error))
                    {
                        throw new Exception(result.Error);
                    }
                    else
                    {
                        UpdateResult($"Store new client failed");
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex.ToString());
                }
                finally
                {
                    btStoreClient.Invoke((MethodInvoker)delegate
                    {
                        btStoreClient.Enabled = true;
                    });
                }
            });
        }

        private void ShowError(string error)
        {
            txtResult.Invoke((MethodInvoker)delegate {
                MessageBox.Show(error);
            });
        }

        private void UpdateResult(string result)
        {
            txtResult.Invoke((MethodInvoker)delegate {
                txtResult.Text = result;
                Application.DoEvents();
            });
        }
    }
}
