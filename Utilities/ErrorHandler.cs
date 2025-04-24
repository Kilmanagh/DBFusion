using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFusion.Utilities
{
    using DBFusion.Models;
    using System;

    using System;

    public static class ErrorHandler
    {
        // Handle general exceptions
        public static void HandleException(Exception ex, string? context = null)
        {
            var message = context == null
                ? $"An error occurred: {ex.Message}"
                : $"An error occurred in {context}: {ex.Message}";

            // Log the error
            Logger.LogError(message, ex);

            // Optionally, write to console for visibility during development
            Console.WriteLine(message);
        }

        // Handle database-specific exceptions
        public static void HandleDatabaseException(Exception ex, string? query = null)
        {
            var message = query == null
                ? $"Database error: {ex.Message}"
                : $"Database error while executing query [{query}]: {ex.Message}";

            // Log the error
            Logger.LogError(message, ex);

            // Optionally, write to console for visibility during development
            Console.WriteLine(message);
        }

        // Handle argument exceptions (e.g., invalid parameters)
        public static void HandleArgumentException(ArgumentException ex, string? parameterName = null)
        {
            var message = parameterName == null
                ? $"Invalid argument: {ex.Message}"
                : $"Invalid argument [{parameterName}]: {ex.Message}";

            // Log the error
            Logger.LogWarning(message);
            Console.WriteLine(message); // Output warning during development
        }

        // Handle authentication-specific exceptions
        public static void HandleAuthException(Exception ex, DbAuth? auth)
        {
            var message = auth == null
                ? $"Authentication error: {ex.Message}"
                : $"Authentication error for user [{auth.Username}]: {ex.Message}";

            // Log the authentication error
            Logger.LogError(message, ex);
            Console.WriteLine(message);
        }

        // Handle custom application exceptions
        public static void HandleCustomException(string customMessage)
        {
            // Log the custom exception
            Logger.LogWarning($"Custom error: {customMessage}");
            Console.WriteLine($"Custom error: {customMessage}"); // Output custom message during development
        }

        // Handle unexpected errors gracefully
        public static void HandleUnexpectedException(Exception ex)
        {
            var message = $"An unexpected error occurred: {ex.Message}";

            // Log the unexpected error
            Logger.LogError(message, ex);
            Console.WriteLine(message);
        }

        // Handle resource-specific errors (e.g., missing files)
        public static void HandleResourceException(Exception ex, string resourceName)
        {
            var message = $"Error accessing resource [{resourceName}]: {ex.Message}";

            // Log the resource error
            Logger.LogError(message, ex);
            Console.WriteLine(message);
        }
    }
}