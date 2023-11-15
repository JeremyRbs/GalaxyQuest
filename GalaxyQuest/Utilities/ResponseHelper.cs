using Microsoft.AspNetCore.Mvc;

namespace GalaxyQuest.Utilities
{
    public static class ResponseHelper
    {
        public static IActionResult HandleResponse<T>(T result, string? notFoundMessage = null)
        {
            return result != null ? new JsonResult(result) : (!string.IsNullOrEmpty(notFoundMessage) ? new NotFoundObjectResult(notFoundMessage) : new NotFoundResult());
        }
    }

}
