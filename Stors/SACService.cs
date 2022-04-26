using Google.Apis.Auth.OAuth2;

using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace ServiceAccountSample {
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
}
