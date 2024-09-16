using System;

namespace sw.infrastructure.Exceptions.Repositories.NHibernate;

public class NHibernateSessionTransactionNotExistException : Exception
{
    public string Details { get; }

    public NHibernateSessionTransactionNotExistException(string details)
    {
        Details = details;
    }

    public override string Message => "NHibernate Session Not Exists.\nDetails:" + Details;
}