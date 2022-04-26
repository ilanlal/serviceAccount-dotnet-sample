namespace ServiceAccountSample {
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
    public SACInitializeParameters(string serviceAccountId, string x509CertificateFilePath, string impersonateEmail, string[] scopes) {
      this.ServiceAccountId = serviceAccountId;
      this.ImpersonateEmail = impersonateEmail;
      this.Scopes = scopes;
      this.X509CertificateFilePath = x509CertificateFilePath;
    }

    override public string ToString() {
      return Newtonsoft.Json.JsonConvert.SerializeObject(this);
    }
  }
}
