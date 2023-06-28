using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApiCore.ComponentModel
{
    /// <summary>
    /// Позволяет узнать были ли установлены свойства класса.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DeterminativePropertySet<T>
        where T : DeterminativePropertySet<T>
    {
        protected Dictionary<string, object> _properties = new Dictionary<string, object>();

        protected void Set<TValue>(TValue value, Expression<Func<T, object>> keyExpr)
        {
            _properties[PropName(keyExpr)] = value;
        }
        protected TValue Get<TValue>(Expression<Func<T, object>> keyExpr)
        {
            string key = PropName(keyExpr);
            if (_properties.ContainsKey(key))
                return (TValue)_properties[key];

            return default;
        }

        private string PropName(Expression<Func<T, object>> propExpr)
        {
            var expr = propExpr.Body;
            switch (expr.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expr).Member.Name;
                case ExpressionType.Convert:
                    expr = ((UnaryExpression)expr).Operand;
                    return ((MemberExpression)expr).Member.Name;
                default:
                    throw new Exception(string.Format("Could not determine member from {0}", expr));
            }
        }

        public bool IsDefined(Expression<Func<T, object>> propExpr)
        {
            return _properties.ContainsKey(PropName(propExpr));
        }

        public static implicit operator Dictionary<string, object>(DeterminativePropertySet<T> set)
        {
            return new Dictionary<string, object>(set._properties);
        }
    }
}
