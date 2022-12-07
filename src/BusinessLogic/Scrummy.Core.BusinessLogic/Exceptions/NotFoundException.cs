using System.Runtime.Serialization;

namespace Scrummy.Core.BusinessLogic.Exceptions
{
    public class NotFoundException<T> : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(int id)
        {
            Id = id;
        }

        public NotFoundException(string? message)
            : base(message)
        {
        }

        public NotFoundException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public int Id { get; }
    }
}
