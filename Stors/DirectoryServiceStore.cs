namespace ServiceAccountSample {
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
}
