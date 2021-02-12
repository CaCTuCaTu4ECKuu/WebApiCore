using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace WebApiCore.ComponentModel
{
    [DebuggerDisplay("{Name}")]
    [TypeConverter(typeof(Converters.StringEnumTypeConverter))]
    public abstract class StringEnum<T> where T : StringEnum<T>, IEquatable<T>
    {
        private static readonly Dictionary<string, T> _possibleValues;

        protected readonly string[] _values;

        public string Name => string.Join(",", _values);

        static StringEnum()
        {
            _possibleValues = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        }

        protected StringEnum(string name)
        {
            _values = new[] { name };
        }

        protected StringEnum(string[] names)
        {
            _values = names;
        }

        private const BindingFlags _createBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        private static T CreateInstance(object[] ctorProps)
        {
            return (T)Activator.CreateInstance(typeof(T), _createBindingFlags, null, ctorProps, null);
        }

        protected static T RegisterPossibleValue(string name)
        {
            if (_possibleValues.ContainsKey(name))
                throw new ArgumentException();

            var value = CreateInstance(new object[] { name });
            _possibleValues.Add(name, value);

            return value;
        }

        public static IEnumerable<T> GetAll() => _possibleValues.Values;

        public bool HasFlag(StringEnum<T> value)
        {
            if (value._values.Length == 1)
                return _values.Contains(value.Name);
            else if (value._values.Length == _values.Length)
            {
                foreach (var val in value._values)
                {
                    if (!_values.Contains(val))
                        return false;
                }
                return true;
            }

            return false;
        }

        public bool HasAny(StringEnum<T> flagsValue)
        {
            if (flagsValue == null)
                throw new ArgumentNullException();

            if (_values.Length > flagsValue._values.Length)
                return _values.Intersect(flagsValue._values).Any();
            else
                return flagsValue._values.Intersect(_values).Any();
        }

        public static bool IsValidValue(string key) =>  key != null && _possibleValues.ContainsKey(key);

        private static readonly char[] splitChars = new[] { ',' };
        
        public static bool TryParse(string key, out T result)
        {
            if (key != null)
            {
                key = key.Trim();
                if (key.IndexOf(',') < 0)
                {
                    if (_possibleValues.ContainsKey(key))
                    {
                        result = _possibleValues[key];
                        return true;
                    }
                }
                else
                {
                    string[] values = key.Split(splitChars).Select(e => e.Trim()).ToArray();
                    if (values.Count() > 0)
                    {
                        List<string> names = new List<string>();
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (_possibleValues.ContainsKey(values[i]))
                                names.Add(_possibleValues[values[i]].Name);
                            else
                            {
                                result = null;
                                return false;
                            }
                        }
                        if (names.Any())
                        {
                            var dNames = names.Distinct().ToArray();
                            if (dNames.Length == values.Length)
                            {
                                if (dNames.Count() > 1)
                                    result = CreateInstance(new object[] { dNames });
                                else
                                    result = _possibleValues[dNames[0]];
                                return true;
                            }
                        }
                    }
                }
            }

            result = null;
            return false;
        }

        public static T Parse(string key)
        {
            if (TryParse(key, out T res))
                return res;

            throw new ArgumentException();
        }

        public bool Equals(T other)
        {
            if (other == null)
                return false;

            return _values.Length == other._values.Length
                && _values.All(e => other._values.Any(x => string.Equals(e, x, StringComparison.OrdinalIgnoreCase)));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public override int GetHashCode()
        {
            var hashCode = 3487623;
            foreach (var val in _values)
                hashCode ^= val.GetHashCode();
            return hashCode;
        }

        public override string ToString() => Name;

        public static bool operator ==(StringEnum<T> left, StringEnum<T> right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(right, null) || ReferenceEquals(left, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(StringEnum<T> left, StringEnum<T> right)
        {
            return !(left == right);
        }

        public static T operator |(StringEnum<T> a, StringEnum<T> b)
        {
            if (a.Equals(b))
                return a as T;

            var dNames = a._values.Concat(b._values).Distinct().ToArray();

            return CreateInstance(new object[] { dNames });
        }

        public static implicit operator string(StringEnum<T> value) => value.Name;
    }
}
