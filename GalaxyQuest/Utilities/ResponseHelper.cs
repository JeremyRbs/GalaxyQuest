using Microsoft.AspNetCore.Mvc;

namespace GalaxyQuest.Utilities
{
    /// <summary>
    /// Helper class for handling responses in controllers.
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// Handles the response based on the result and optional messages.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="result">The result to be handled.</param>
        /// <param name="notFoundMessage">Optional message for not found response.</param>
        /// <param name="badRequestMessage">Optional message for bad request response.</param>
        /// <returns>IActionResult representing the handled response.</returns>
        public static IActionResult HandleResponse<T>(T result, string? notFoundMessage = null, string? badRequestMessage = null) =>
            result != null
                ? new JsonResult(result)
                : (!string.IsNullOrEmpty(notFoundMessage)
                    ? new NotFoundObjectResult(notFoundMessage)
                    : (!string.IsNullOrEmpty(badRequestMessage)
                        ? new BadRequestObjectResult(badRequestMessage)
                        : new NotFoundResult()));
    }
}
