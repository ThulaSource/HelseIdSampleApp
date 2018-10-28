####
HelseId sample application.
v1.0 - 2018-10-11
####


This sample application contains example code on how to generate the initial configuration of Helse-id clients.
The application contains a form (HelseIdEnabler) with a browser (CefSharp - https://github.com/cefsharp/CefSharp) and a button. 
Clicking the button will create a new client registration (store client) using the settings from the appsettings.json file.


The following is a brief explanation on the project organization:

-- Browser logic: Contains logic associated with handling browser requests
-- Configuration: Contains logic to handle the appSettings configuration file
-- OpenIdConnect: Contains data models and helper classes to handle OpenId communication with HelseId DCR API
-- StructureMap: StructureMap related classes

appSettings.json

    "HelseId.EnablerClientId": The HelseId client identifier that is allowed to register new clients 
    "HelseId.EnablerRedirectUrl": A valid redirect Url of the client that is allowed to register new clients 
    "HelseId.EnablerCertificateThumbprint": The organization certificate,
    "HelseId.EnablerOrgEnhId": The organization Enh-Id,
    "HelseId.EnablerOrgName": The organization name,
    "HelseId.EnablerScope":  These are the scopes needed to register new clients with access to Kjernejournal
    "HelseId.Scope": These are the scopes the new client is allowed
    "HelseId.Endpoint": HelseId endpoint Url 
    "HelseId.ApiEndpoint": HelseId API endpoint Url 


When registering a new client three things need to happen:

- Fetch a valid access token from HelseId
- Use the token and call HelseId API to create a new client
- Use the token and call HelseId API to associate the registered client with the organization identifier (Enh-Id)