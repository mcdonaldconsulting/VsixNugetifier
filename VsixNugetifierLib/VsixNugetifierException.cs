namespace NugetTemplateTools
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class VsixNugetifierException : Exception
    {
        public VsixNugetifierException()
            : base()
        {
        }

        public VsixNugetifierException(string message)
            : base(message)
        {
        }

        public VsixNugetifierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected VsixNugetifierException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
