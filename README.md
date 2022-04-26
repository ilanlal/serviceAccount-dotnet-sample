# Sample: How to access AdminAPI.DirectoryService using Service Account, .Net Framework 4.8 (C#)
  A service account is a special kind of account used by an application, rather than a person. 

  You can use a service account to access data or perform actions by the robot account itself, 
  or to access data on behalf of Google Workspace or Cloud Identity users.
  > In any server-to-server interactions you must have a service account

  ## Prerequisites:
  + ### Visual Studio 2013 or later 
  + ### A Google Cloud Platform project with the Admin SDK API enabled service account with domain-wide delegation. 
  + ### A Google Workspace domain with API access enabled and granted domain-wide delegation to your project

## Step 1: Set up the Google Cloud Platform project
+ ### Create Google Cloud project
  A Google Cloud project is required to use Google Workspace APIs and build Google Workspace add-ons or apps
  To create a project refer to: [How to create a Google Cloud project](https://developers.google.com/workspace/guides/create-project)

+ ### Enable Google Workspace APIs
  Before using Google APIs, you need to enable them in a Google Cloud project. 
  You can enable one or more APIs in a single Google Cloud project. 
  If you don't already have a Google Cloud project,
  To Enable Google Workspace APIs refer to: [How to Enable Google Workspace APIs](https://developers.google.com/workspace/guides/enable-apis)
  
  > Note: 
  > * For this example, you are enabling the `Admin SDK API`, 
  > * For this example, you are enabling accessing data scope of `/auth/admin.directory.user.readonly`.
    
+ ### Create Service Account with domain-wide delegation
  Use to authenticate on behalf of super user, through domain-wide delegation.

  To create service account refer to: [How to create service account?](https://developers.google.com/workspace/guides/create-credentials#create_a_service_account)
  
  To enable domain-wide delegation refer to: [How to enable domain-wide delegation](https://developers.google.com/identity/protocols/oauth2/service-account#delegatingauthority) for servvice account.
  
  > Note:
  > * Download Service Account private key (p12 format)
  > * Download p12 file [contains the private key](https://cloud.google.com/iam/docs/creating-managing-service-account-keys) for your Service Account.
  
## Step 2: Set up the Google Workspace 
+ ### A Google Workspace domain with [API access enabled](https://support.google.com/a/answer/7281227?visit_id=637865874764605082-823144595&rd=1)

+ ### Granted domain-wide delegation in your Google Workspace domain
  To call APIs on behalf of users in a Google Workspace organization, your service account needs to be granted domain-wide delegation of authority in the Google Workspace Admin console by a super administrator account

+ ### A Google account in that domain with administrator privileges.



## Step 3: Prepare Visual Stodio project - 
  + ### Create a new Visual C# Console Application (.NET Framework) project in Visual Studio.
  + ### Open the NuGet Package Manager Console, select the package source nuget.org, and run the following commands:
     + #### `Install-Package Google.Apis.Auth`
     + #### `Install-Package Google.Apis.Admin.Directory.directory_v1`

## Step 4: Set up credentials parameters

### How to set up [Service Account](#Service%20Account) in Google Cloud Platform project 
### 

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

## Step 3: Initilize DirectoryService and Execute API request. 

#### List top 10 users alias from Google Workspace Domain
```csharp
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

## References

  + [Creating and managing service account keys](https://cloud.google.com/iam/docs/creating-managing-service-account-keys#iam-service-account-keys-create-csharp)