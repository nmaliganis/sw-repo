﻿using System.Collections.Generic;

namespace sw.infrastructure.PropertyMappings;

public interface IPropertyMappingService
{
  bool ValidMappingExistsFor<TSource, TDestination>(string fields);

  Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
}