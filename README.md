# Sample: How to access AdminAPI.DirectoryService using Service Account, .Net Framework 4.8 (C#)

Google APIs can act on behalf of your application without accessing user information. 
In these situations your application needs to prove its own identity to the API, but no user consent is necessary. 
Similarly, in enterprise scenarios, your application can request delegated access to some resources.

For these types of server-to-server interactions you need a service account, 
which is an account that belongs to your application instead of to an individual end-user.
Your application calls Google APIs on behalf of the service account, and user consent is not required. 

Complete the steps described in the rest of this page to create a simple .NET console application (.NET Framework)
that makes requests to the Admin SDK (Google.Apis.Admin.Directory) APIs.

## Prerequisites:
  + Visual Studio 2013 or later
  
  + A Google Cloud Platform project with the API enabled.
    
    To create a project and enable an API, refer to [Create a project and enable the API](https://developers.google.com/workspace/guides/create-project)
    
    > Note: For this example, you are enabling the `Admin SDK API`, accessing data scope of `/admin.directory.user.readonly`.

  + A Google Workspace domain with [API access enabled](https://support.google.com/a/answer/7281227?visit_id=637865874764605082-823144595&rd=1)

    [Create private key](https://cloud.google.com/iam/docs/creating-managing-service-account-keys) (X.509 certificate - P12 format) for Service Account
    
    Downloads a file that contains the private key. Store the file securely because this key cannot be recovered if lost.

    > Note: Service account keys could pose a security risk if compromised.
    > Note: The public key in a Google-managed key pair is publicly accessible, so that anyone can verify signatures that are created with the private key.
    > Note: By default, service account keys never expire

  + A Google account in that domain with administrator privileges.

## 1. Step 1: Prepare the project - Visual Stodio
  + Create a new Visual C# Console Application (.NET Framework) project in Visual Studio.

  + Open the NuGet Package Manager Console, select the package source nuget.org, and run the following commands:
     + `Install-Package Google.Apis.Auth`
     + `Install-Package Google.Apis.Admin.Directory.directory_v1`

## Step 2: Set up the sample
```
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

## 3. Initilize DirectoryService and Execute API request. 

#### List top 10 users alias from Google Workspace Domain
```
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
```

## Support / Donate 



## References

  + [Creating and managing service account keys](https://cloud.google.com/iam/docs/creating-managing-service-account-keys#iam-service-account-keys-create-csharp)