using System.Reflection;

namespace coer91.Tools
{
    public static class Validations
    {
        #region IsPrimitiveOrDateTime

        /// <summary>
        /// Validates whether it is a primitive type or datetime.
        /// </summary>
        public static bool IsPrimitiveOrDateTime<T>(T obj)
            => obj is not null && IsPrimitiveOrDateTime(obj.GetType());


        /// <summary>
        /// Validates whether it is a primitive type or datetime.
        /// </summary>
        public static bool IsPrimitiveOrDateTime(PropertyInfo property)
            => IsPrimitiveOrDateTime(property.PropertyType);


        /// <summary>
        /// Validates whether it is a primitive type or datetime. 
        /// </summary>
        public static bool IsPrimitiveOrDateTime(Type type)
            => type == typeof(char)
            || type == typeof(string)
            || type == typeof(int)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(decimal)
            || type == typeof(bool)
            || type == typeof(DateOnly)
            || type == typeof(DateTime)
            || type == typeof(TimeOnly)
            || type == typeof(TimeSpan);

        #endregion


        #region IsString  

        /// <summary>
        /// Validate if it is of string type.
        /// </summary>
        public static bool IsString<T>(T obj)
           => obj is not null && IsString(obj.GetType());


        /// <summary>
        /// Validate if it is of string type. 
        /// </summary>
        public static bool IsString(PropertyInfo property)
           => IsString(property.PropertyType);


        /// <summary>
        /// Validate if it is of string type. 
        /// </summary>
        public static bool IsString(Type type)
           => type == typeof(string);

        #endregion


        #region IsCollection

        /// <summary>
        ///  
        /// </summary>
        public static bool IsCollection(object obj)
            => obj is System.Collections.IEnumerable && obj.GetType() != typeof(string);

        public static bool IsCollection<T>()
            => typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        #endregion


        #region GetProperties

        /// <summary>
        /// Gets an enumerable with the object's properties. 
        /// </summary>
        public static IEnumerable<string> GetProperties(object obj)
            => IsPrimitiveOrDateTime(obj) ? [] : obj?.GetType().GetProperties().Select(p => p.Name) ?? [];

        #endregion


        #region HasProperty

        /// <summary>
        /// Validates whether the object has the property. 
        /// </summary>
        public static bool HasProperty(string property, object obj, bool sensitive = true)
            => !IsPrimitiveOrDateTime(obj) && (sensitive ? GetProperties(obj).Contains(property) : GetProperties(obj).Contains(property, StringComparer.OrdinalIgnoreCase));

        #endregion
    }
}