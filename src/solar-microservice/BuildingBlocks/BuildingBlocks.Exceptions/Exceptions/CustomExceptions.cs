using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions.Exceptions
{
    public class CustomExceptions
    {

    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string msg):base(msg)
        {
            
        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {

        }
    }

    public class AlreadyExists : Exception
    {
        public AlreadyExists(string msg):base(msg)
        {
            
        }
    }

    //public class ValidationException : Exception
    //{
    //    public IEnumerable<string> Errors { get; }

    //    public ValidationException(IEnumerable<string> errors)
    //        : base("Validation Error")
    //    {
    //        Errors = errors;
    //    }
    //}

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string msg):base(msg)
        {
            
        }
    }

}
