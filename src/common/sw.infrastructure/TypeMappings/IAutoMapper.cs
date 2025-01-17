﻿namespace sw.infrastructure.TypeMappings
{
    public interface IAutoMapper
    {
        T Map<T>(object objectToMap);
        TDest Map<TSource, TDest>(TSource objectSource, TDest objectDest);
    }
}