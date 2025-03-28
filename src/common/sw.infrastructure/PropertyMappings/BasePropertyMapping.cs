﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace sw.infrastructure.PropertyMappings {
    public abstract class BasePropertyMapping : IPropertyMappingService {
        private readonly IList<IPropertyMapping> _propertyMappings;

        protected BasePropertyMapping(IList<IPropertyMapping> propertyMappings) {
            this._propertyMappings = propertyMappings;
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>() {
            // get matching mapping
            var matchingMapping = this._propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping != null && matchingMapping.Any()) {
                return matchingMapping.First().MappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields) {
            var propertyMapping = this.GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields)) {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit) {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields
                // are coming from an orderBy string, this part must be
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName)) {
                    return false;
                }
            }
            return true;

        }
    }
}
