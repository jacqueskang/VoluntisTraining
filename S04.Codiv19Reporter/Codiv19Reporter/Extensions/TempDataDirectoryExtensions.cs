namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    public static class TempDataDirectoryExtensions
    {
        public static void SetInfoMessage(this ITempDataDictionary tempData, string message)
        {
            tempData["InfoMessage"] = message;
        }

        public static void SetErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData["ErrorMessage"] = message;
        }
    }
}
