using System;

namespace sw.infrastructure.Exceptions.Repositories.NHibernate
{
    public class NhInfrastructureException : Exception
    {
        public string Details { get; private set; }

        public override string Message => "NHibernate Session Factory failed to be initialized.";

        public NhInfrastructureException(string strDetails)
        {
            Details = strDetails;
        }
    }
}
