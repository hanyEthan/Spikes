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
            //Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures) : this()
        {
            this.Failures = failures.Select(x => (x.PropertyName, x.ErrorMessage)).ToList();

            //var failureGroups = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            //foreach (var failureGroup in failureGroups)
            //{
            //    var propertyName = failureGroup.Key;
            //    var propertyFailures = failureGroup.ToArray();

            //    Failures.Add(propertyName, propertyFailures);
            //}
        }

        //public IDictionary<string, string[]> Failures { get; }
        public List<(string,string)> Failures { get; }
    }
}
