using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Business.Contracts
{
    public class OperationResult
    {
        public Result Result { get; set; }

        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public bool IsSuccess => Result.Equals(Result.Succeed);

        public static OperationResult Succeed()
            => new OperationResult { Result = Result.Succeed };

        public static OperationResult Error(string error)
            => new OperationResult { Result = Result.Error, Errors = new List<string> { error } };
    }

    public enum Result
    {
        Succeed,
        Error
    }
}