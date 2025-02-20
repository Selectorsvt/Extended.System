using System.Linq.Expressions;
using System.Reflection;

namespace Extended.System
{
    /// <summary>
    /// The expression extensions class.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the property name using the specified property lambda.
        /// </summary>
        /// <typeparam name="TSource">The source.</typeparam>
        /// <typeparam name="TProperty">The property.</typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var propertyInfo = GetPropertyInfo(propertyLambda);
            return propertyInfo.Name;
        }

        /// <summary>
        /// Gets the property info using the specified property lambda.
        /// </summary>
        /// <typeparam name="TSource">The source.</typeparam>
        /// <typeparam name="TProperty">The property.</typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <exception cref="ArgumentException">Not valid expression.</exception>
        /// <returns>The prop info.</returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            ObjectExtensions.CheckIsNull(propertyLambda);

            var body = propertyLambda.Body;

            if (body is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert)
            {
                body = unaryExpression.Operand;
            }

            if (body is not MemberExpression member)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));
            }

            if (member.Member is not PropertyInfo propInfo)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));
            }

            Type type = typeof(TSource);
            if (propInfo.ReflectedType != null && type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));
            }

            return propInfo;
        }
    }
}
