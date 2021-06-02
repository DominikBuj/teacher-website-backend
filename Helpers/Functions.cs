using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TeacherWebsiteBackEnd.Helpers
{
    public static class Functions
    {
        public static bool CorrectValue<TOne, TTwo>(TOne value)
        {
            foreach (PropertyInfo propertyInfo in typeof(TTwo).GetProperties())
            {
                var correctValue = propertyInfo.GetValue(null);
                if (value.Equals(correctValue)) return true;
            }
            return false;
        }

        public static object ChangeType(object value, Type type)
        {
            var _type = type;

            if (_type.IsGenericType && _type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) return null;
                _type = Nullable.GetUnderlyingType(_type);
            }
            
            return Convert.ChangeType(value, _type);
        }

        public static EntityEntry<T> AddIfNotExists<T>(this DbSet<T> set, T entity, Expression<Func<T, bool>> predicate) where T : class
        {
            return (set.Any(predicate)) ? null : set.Add(entity);
        }
    }
}
