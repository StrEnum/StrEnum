using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrEnum
{
    /// <summary>
    /// Allows to create and use a string enum of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The class to represent the string enum.</typeparam>
    /// <example>
    /// Example:
    /// <code>
    /// public class Channel: StringEnum&lt;Channel&gt;
    /// {
    ///     public static readonly Channel Sms = Define("SMS");
    ///     public static readonly Channel Email = Define("Email");
    /// }
    /// </code>
    /// </example>
    public abstract class StringEnum<TEnum> where TEnum : StringEnum<TEnum>, new()
    {
        private string _name;
        private string _value;

        /// <summary>
        /// Defines a new member of the <typeparamref name="TEnum"/> enum with a given name and value.
        /// </summary>
        /// <param name="value">Value of the member</param>
        /// <param name="name">Name of the member. If omitted, will use the name of the property, field, or method.</param>
        /// <returns>The newly defined <typeparamref name="TEnum"/> member.</returns>
        /// <example>
        /// Example:
        /// <code>
        /// public static readonly <typeparamref name="TEnum"/> Name = Define("Value");
        /// </code>
        /// </example>
        protected static TEnum Define(string value, [CallerMemberName] string name = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var member = new TEnum()
            {
                _name =  name,
                _value = value
            };

            return member;
        }

        public override string ToString() => _value;
    }
}