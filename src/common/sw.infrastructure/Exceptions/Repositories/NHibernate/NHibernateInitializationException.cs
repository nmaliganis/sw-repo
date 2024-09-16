﻿using System;

namespace sw.infrastructure.Exceptions.Repositories.NHibernate
{
    public class NHibernateInitializationException : Exception
    {
        public string Details { get; }
        public string InnerExceptionDetails { get; set; }

        public NHibernateInitializationException(string details)
        {
            Details = details;
        }

        public NHibernateInitializationException(string details, string innerExceptionDetails)
        {
            Details = details;
            InnerExceptionDetails = innerExceptionDetails;
        }

        public override string Message => "NHibernate initialization failed.\nDetails:" + Details + InnerExceptionDetails;
    }
}
