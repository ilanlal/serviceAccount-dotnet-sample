namespace ServiceAccountSample {
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
}
