namespace GalaxyQuest.Entities
{
    /// <summary>
    /// Represents additional details about a problem that occurred during an HTTP request.
    /// </summary>
    public class ProblemDetails
    {
        /// <summary>
        /// Gets or sets the type of the problem.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets a short description of the problem.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code for the response.
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets a detailed description of the problem.
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Gets or sets a URI reference that identifies the specific occurrence of the problem.
        /// </summary>
        public string? Instance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemDetails"/> class.
        /// </summary>
        /// <param name="type">The type of the problem.</param>
        /// <param name="title">A short description of the problem.</param>
        /// <param name="status">The HTTP status code for the response.</param>
        /// <param name="detail">A detailed description of the problem.</param>
        /// <param name="instance">A URI reference that identifies the specific occurrence of the problem.</param>
        public ProblemDetails(string? type, string? title, int? status, string? detail, string? instance)
        {
            Type = type;
            Title = title;
            Status = status;
            Detail = detail;
            Instance = instance;
        }
    }
}
