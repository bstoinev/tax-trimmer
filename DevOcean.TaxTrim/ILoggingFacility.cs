using System;

namespace DevOcean.TaxTrim
{
    public interface ILoggingFacility<TSource> where TSource: class
    {
        void Debug(string message);

        void Error(Exception ex);

        void Error(string message);

        void Info(string message);

        void Trace(string message);

        void Warning(string message);
    }
}
