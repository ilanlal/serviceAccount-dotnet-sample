# Sample: How to access AdminAPI.DirectoryService using Service Account, .Net Framework 4.8 (C#)
A service account is a special kind of account used by an application, rather than a person. 

You can use a service account to access data or perform actions by the robot account itself, or to access data on behalf of Google Workspace or Cloud Identity users.

## Prerequisites:
  + ### A Google Cloud Platform project 
    With the Admin SDK API enabled service account with domain-wide delegation. 
  + ### A Google Workspace domain.
    With account in that domain with administrator privileges.
  + ### Visual Studio 2013 or later 

## Step 1: Set up the Google Cloud Platform project
+ ### Create Google Cloud project
  A Google Cloud project is required to use Google Workspace APIs and build Google Workspace add-ons or apps.
  If you don't already have a Google Cloud project, refer to: [How to create a Google Cloud project](https://developers.google.com/workspace/guides/create-project)

+ ### Enable Google Workspace APIs
  Before using Google APIs, you need to enable them in a Google Cloud project. 
    
  To Enable Google Workspace APIs refer to: [How to Enable Google Workspace APIs](https://developers.google.com/workspace/guides/enable-apis)
  
  For this example you are enabling the the [Admin SDK Directory API](https://developers.google.com/admin-sdk/directory)
  with the data scope `/auth/admin.directory.user.readonly`.
 
    
+ ### Create Service Account with domain-wide delegation
  To create service account refer to: [How to create service account?](https://developers.google.com/workspace/guides/create-credentials#create_a_service_account)
  
  In the `Domain wide delegation` pane, select `Manage Domain Wide Delegation`.

+ ### Download Service Account private key (p12 format)
  Download p12 file [contains the private key](https://cloud.google.com/iam/docs/creating-managing-service-account-keys) for your Service Account.
  
## Step 2: Set up the Google Workspace 
+ ### Enable API access in the Google Workspace domain with
  To enable API access in Google Workspace domain, refer to: [how to enable API access](https://support.google.com/a/answer/7281227?visit_id=637865874764605082-823144595&rd=1)
+ ### Delegating domain-wide authority to the service account
  To call APIs on behalf of users in a Google Workspace organization, your service account needs to be granted domain-wide delegation of authority in the Google Workspace Admin console __by a super administrator account__
  
  To delegating domain-wide authority in Google Workspace domain, refer to: [How to Delegating domain-wide authority](https://developers.google.com/identity/protocols/oauth2/service-account#delegatingauthority) to the service account

## Step 3: Prepare Visual Stodio project - 
+ ### Create a new Visual C# Console Application (.NET Framework) project in Visual Studio.
+ ### Open the NuGet Package Manager Console, select the package source nuget.org, and run the following commands:
  + #### `Install-Package Google.Apis.Auth`
  + #### `Install-Package Google.Apis.Admin.Directory.directory_v1`

## Step 4: Add code

### `SACService.cs`
```csharp
  using Google.Apis.Auth.OAuth2;

  using System;
  using System.Security.Cryptography.X509Certificates;
  using System.Threading;

  /// <summary>
  //     Google OAuth 2.0 credential for accessing protected resources using an access
  //     token. The Google OAuth 2.0 Authorization Server supports server-to-server interactions
  //     such as those between a web application and Google Cloud Storage. The requesting
  //     application has to prove its own identity to gain access to an API, and an end-user
  //     doesn't have to be involved.
  /// </summary>
  public static class SACService {
    /// <summary>
    /// Constructs a new service account credential using the given initializer.
    /// </summary>
    /// <returns>ServiceAccountCredential</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static ServiceAccountCredential CreateServiceAccountCredential(SACInitializeParameters parameters) {
      using (var x509Certificate2 = new X509Certificate2(
       parameters.X509CertificateFilePath,
       "notasecret",
       X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable)) {

        var credential = new ServiceAccountCredential(
          new ServiceAccountCredential.Initializer(parameters.ServiceAccountId) {
            User = parameters.ImpersonateEmail,
            Scopes = parameters.Scopes
          }.FromCertificate(x509Certificate2));

        
        if (credential.RequestAccessTokenAsync(CancellationToken.None).Result) {
          return credential;
        }
        else {
          throw new InvalidOperationException($"Request for access token failed. {parameters.ToString()} ");
        }
      }
    }
  }
```

### `DirectoryServiceStore.cs`
```csharp
  using Google.Apis.Services;
  using AdminAPIs = Google.Apis.Admin.Directory.directory_v1;

  public static class DirectoryServiceFactory {
    public static AdminAPIs.DirectoryService CreateDirectoryService(SACInitializeParameters parameters) {
      return new AdminAPIs.DirectoryService(
        new BaseClientService.Initializer() {
          HttpClientInitializer = SACService.CreateServiceAccountCredential(parameters)
        });
    }
  }
```

### `SACInitializeParameters.cs`
```csharp
  /// <summary>
  /// Initializer paramters to constructs any new google service account credential
  /// </summary>
  public class SACInitializeParameters {
    /// <summary>
    /// The service account ID (typically an e-mail address like: *@*iam.gserviceaccount.com)
    /// </summary>
    public string ServiceAccountId;
    /// <summary>
    /// The full path; name of a certificate file.
    /// </summary>
    public string X509CertificateFilePath;
    /// <summary>
    /// The email address of the user the application trying to impersonate
    /// </summary>
    public string ImpersonateEmail;
    /// <summary>
    /// The scopes which indicate API access your application is requesting
    /// </summary>
    public string[] Scopes;

    /// <summary>
    /// Constructs initialize paramters for google service account credential
    /// </summary>
    /// <param name="serviceAccountId">The service account ID</param>
    /// <param name="x509CertificateFilePath">The full path; name of a certificate file.</param>
    /// <param name="impersonateEmail">The email address of the user the application trying to impersonate</param>
    /// <param name="scopes">The scopes which indicate API access your application is requesting</param>
    public SACInitializeParameters(
      string serviceAccountId, string x509CertificateFilePath, string impersonateEmail, string[] scopes) {
      this.ServiceAccountId = serviceAccountId;
      this.ImpersonateEmail = impersonateEmail;
      this.Scopes = scopes;
      this.X509CertificateFilePath = x509CertificateFilePath;
    }

    override public string ToString() {
      return Newtonsoft.Json.JsonConvert.SerializeObject(this);
    }
  }
```

### Set up credentials parameters

```csharp
  var _paramters = new SACInitializeParameters(
    
  // The service account ID (typically an e-mail address like: *@*iam.gserviceaccount.com)
  serviceAccountId: "[Service Account ID]",
    
  // The full path; name of a certificate file
  x509CertificateFilePath: "[X509 Certificate File]",
    
  // The email address of the user you trying to impersonate
  impersonateEmail: "[User Email]",
    
  // The scopes which indicate API access your application is requesting
  scopes: _scopes);
```

### List top 10 users alias from Google Workspace Domain
```csharp
  /// <summary>
  /// Example how to list all users from google workspace domain, using a service account (user impersonation).
  /// </summary>
  internal class Program {
    static void Main(string[] args) {
      // Scope for only retrieving users or user aliases.
      string[] _scopes = {
        "https://www.googleapis.com/auth/admin.directory.user.readonly"
      };

      var _paramters = new SACInitializeParameters(
        // The service account ID (typically an e-mail address like: *@*iam.gserviceaccount.com)
        serviceAccountId: "[Service Account ID]",
        // The full path; name of a certificate file
        x509CertificateFilePath: "[X509 Certificate File]",
        // The email address of the user you trying to impersonate
        impersonateEmail: "[User Email]",
        // The scopes which indicate API access your application is requesting
        scopes: _scopes);


      using (var directoryService = DirectoryServiceFactory.CreateDirectoryService(_paramters)) {
        // Retrieves a paginated list of either deleted users or all users in a domain.
        var request = directoryService.Users.List();
        // The unique ID for the customer's Google Workspace account
        // the `my_customer` alias represent current identety account's
        request.Customer = "my_customer";
        request.MaxResults = 10;
        var response = request.Execute();

        foreach (var user in response.UsersValue) {
          System.Console.WriteLine($"{user.Name.FullName}, {user.PrimaryEmail}, {user.Id}");
        }
      }
    }
  }
```

## References

  + [Creating and managing service account keys](https://cloud.google.com/iam/docs/creating-managing-service-account-keys#iam-service-account-keys-create-csharp)