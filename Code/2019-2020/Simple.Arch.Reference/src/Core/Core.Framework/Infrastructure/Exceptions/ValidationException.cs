using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("One or more validation failures have occurred.")
        {
        }

        public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures) : this()
        {
            this.Failures = failures.Select(x => (x.PropertyName, x.ErrorMessage)).ToList();
        }

        public List<(string,string)> Failures { get; }
    }
}
