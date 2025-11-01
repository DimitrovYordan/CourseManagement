using System.Net;

namespace CourseManagement.Middlewares
{
    public class CourseException : Exception
    {
        public abstract class AppException : Exception
        {
            public int StatusCode { get; }

            protected AppException(string message, int statusCode) : base(message)
            {
                StatusCode = statusCode;
            }
        }

        public class NotFoundException : AppException
        {
            public NotFoundException(string message)
                : base(message, (int)HttpStatusCode.NotFound) { }
        }

        public class ConflictException : AppException
        {
            public ConflictException(string message)
                : base(message, (int)HttpStatusCode.Conflict) { }
        }

        public class BadRequestException : AppException
        {
            public BadRequestException(string message)
                : base(message, (int)HttpStatusCode.BadRequest) { }
        }
    }
}
