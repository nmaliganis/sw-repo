using System;

namespace sw.infrastructure.Exceptions.Repositories.NHibernate;

public class NHibernateSessionTransactionNotActiveException : Exception
{
    public string Details { get; }

    public NHibernateSessionTransactionNotActiveException(string details)
    {
        Details = details;
    }

    public override string Message => "NHibernate Session Not Active.\nDetails:" + Details;
}