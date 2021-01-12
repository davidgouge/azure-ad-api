# How to set up a protected API on Azure and call from a "Daemon" application.
### (A "Daemon" application being one that does not require user sign in)

## The API

- Create a new Azure AD Application
- Edit the Manifest for the application:
 ```
 "appRoles": [
		{
			"allowedMemberTypes": [
				"Application"
			],
			"description": "Accesses the Weather Forecast API as an application.",
			"displayName": "access_as_application",
			"id": "effddccf-fbce-46bd-86b1-dd24ee1fef3f",
			"isEnabled": true,
			"lang": null,
			"origin": "Application",
			"value": "access_as_application"
		}
	],
```
- In `ConfigureServices` in `Startup`, add the following:

```
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));
```
- in appsetting.json:
```
"AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "[The domain of the Azure AD]",
    "TenantId": "[The Azure AD Tenant ID]",
    "ClientId": "[The AD Application Client ID]"
  },
```
![Web API Registration Details](/screenshots/webapiregistration.png)

- In the API Controller, you can use `HttpContext.ValidateAppRole("access_as_application");` to validate the correct role.

## The Client Application (Deamon)

- Create a new Azure AD Application
- Create a Client Secret and make a note of it
- `API permissions` -> `Add a permission` -> `My APIs` -> Select the Web API application as created above.  Select `Application permissions`, tick `access_as_application` and click `Add permissions`.
- Go to `Enterprise applications`, select the Daemon app then `Permissions`. Click `Grant admin consent for Default Directory`

- To call the API from the Daemon Application:

```
var app = ConfidentialClientApplicationBuilder
                .Create("[The Client Id of the Daemon application]")
                .WithClientSecret("[The Client Secret of the Daemon application]")
                .WithAuthority("https://login.microsoftonline.com/[The Azure AD Tenant ID (will be the same as Tenant ID in the API config above)]")
                .Build();

            var authResult = app.AcquireTokenForClient(new[] {"api://[The Client ID of the Web API application (same as Client ID in the API config above)]/.default"}).ExecuteAsync().Result;
            
```