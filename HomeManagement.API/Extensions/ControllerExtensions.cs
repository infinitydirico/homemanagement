using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Extensions
{
    public static class ControllerExtensions
    {
        public static FileContentResult CreateFile(this Controller controller, byte[] rawData, string filename, string contentType)
        {
            controller.HttpContext.Response.ContentType = contentType;

            controller.HttpContext.Response.Headers.Add("Filename", filename);

            FileContentResult result = new FileContentResult(rawData, contentType)
            {
                FileDownloadName = filename
            };

            return result;
        }

        public static FileContentResult CreateCsvFile(this Controller controller, byte[] rawData, string filename)
        {
            return CreateFile(controller, rawData, filename, "text/csv");
        }
    }
}
