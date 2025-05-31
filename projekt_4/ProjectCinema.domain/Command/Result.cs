using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Command
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure { get; }
        public string Message { get; }
        public IEnumerable<Error> Errors { get; }

        private Result(bool isSuccess, string message, IEnumerable<Error> errors)
        { 
            this.IsSuccess = isSuccess;
            this.IsFailure = !isSuccess;
            this.Message = message;
            this.Errors = errors ?? Enumerable.Empty<Error>();
        }

        public static Result Ok() => new(true, string.Empty, Enumerable.Empty<Error>());
        public static Result Fail(string message) => new(false, message, Enumerable.Empty<Error>());
        public static Result Fail(ValidationResult validationResult) 
        {
            var errors = validationResult.Errors.Select(x => new Error(x.PropertyName, x.ErrorMessage));

            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            return new Result(false, message, errors);
        }

        public class Error 
        {
            public string PropertyName { get; }
            public string Message { get; }
            
            public Error(string propertyName, string message) 
            { 
                this.PropertyName = propertyName;
                this.Message = message;
            }
        }
    }
}
