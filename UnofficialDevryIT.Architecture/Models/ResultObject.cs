using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace UnofficialDevryIT.Architecture.Models
{
    public class ResultObject
    {
        public bool Succeeded => !Errors.Any();
        public IReadOnlyCollection<string> Errors { get; }

        internal ResultObject(IEnumerable<string> errors)
        {
            Errors = errors.ToImmutableArray();
        }

        public static ResultObject Success()
        {
            return new (new string[] { });
        }

        public static ResultObject Failure(params string[] errors)
        {
            return new (errors);
        }

        public override string ToString()
        {
            return string.Join(",", Errors);
        }
    }
}