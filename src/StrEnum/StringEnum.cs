using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

// ReSharper disable ExplicitCallerInfoArgument

namespace StrEnum
{
    /// <summary>
    ///     Allows to create a string enum of type <typeparamref name="TEnum" />.
    /// </summary>
    /// <typeparam name="TEnum">The class to represent the string enum.</typeparam>
    /// <example>
    ///     Example:
    ///     <code>
    /// public class Season: StringEnum&lt;Season&gt;
    /// {
    ///     public static readonly Season Spring = Define("SPRING");
    ///     public static readonly Season Summer = Define("SUMMER");
    ///     public static readonly Season Autumn = Define("AUTUMN");
    ///     public static readonly Season Winter = Define("WINTER");
    /// }
    /// </code>
    /// </example>
    public abstract class StringEnum<TEnum> where TEnum : StringEnum<TEnum>, new()
    {
        static StringEnum()
        {
            // Initialize the TEnums's static fields upon the enum creation
            RuntimeHelpers.RunClassConstructor(typeof(TEnum).TypeHandle);
        }

        private string _name;
        private string _value;

        private static readonly MembersStore<TEnum> Members = new();

        /// <summary>
        ///     Defines a new member of the <typeparamref name="TEnum" /> enum with a given name and value.
        /// </summary>
        /// <param name="value">Value of the member</param>
        /// <param name="name">Name of the member. If omitted, will use the name of the property, field, or method that calls the Define method.</param>
        /// <param name="callerMemberName">The name of the property, field, or method that calls the Define method. The value is passed automatically.</param>
        /// <returns>The newly defined <typeparamref name="TEnum" /> member.</returns>
        /// <example>
        ///     Example:
        /// <code>
        /// public static readonly <typeparamref name="TEnum" /> Name = Define("Value");
        /// </code>
        /// </example>
        protected static TEnum Define(string value, string? name = null, [CallerMemberName] string? callerMemberName = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            name ??= callerMemberName;

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var member = new TEnum
            {
                _name = name,
                _value = value
            };

            var added = Members.TryAdd(member);

            if (!added)
                throw new ArgumentException($"A member '{name}' is already defined.");
            
            return member;
        }

        public override string ToString()
        {
            return _name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            var otherEnum = (StringEnum<TEnum>)obj;

            return _value.Equals(otherEnum._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Retrieves a collection of the members of a given string enum in the order of definition.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyCollection<TEnum> GetMembers()
        {
            return Members.List();
        }

        /// <summary>Converts the string representation of the name or value of a member to an equivalent string enum member object.</summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">
        /// <see langword="true" /> to ignore case; <see langword="false" /> to regard case.</param>
        /// <param name="matchBy">
        /// <see langword="MatchBy.NameOrValue" /> to match the members by name or value; <see langword="MatchBy.ValueOnly" /> to match the members by value only.</param>
        public static TEnum Parse(string value, bool ignoreCase = false, MatchBy matchBy = MatchBy.NameOrValue)
        {
            if (!TryParse(value, out var member, ignoreCase, matchBy))
                throw new ArgumentException($"Requested name or value '{value}' was not found.");
            
            return member!;
        }

        /// <summary>Converts the string representation of the name or value of a member to an equivalent string enum member object. The return value indicates whether the member was found.</summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="member">A found member or <see langword="null" />.</param>
        /// <param name="ignoreCase">
        /// <see langword="true" /> to ignore case; <see langword="false" /> to regard case.</param>
        /// <param name="matchBy">
        /// <see langword="MatchBy.NameOrValue" /> to match the members by name or value; <see langword="MatchBy.ValueOnly" /> to match the members by value only.</param>
        public static bool TryParse(string value, out TEnum? member, bool ignoreCase = false, MatchBy matchBy = MatchBy.NameOrValue)
        {
            member = Members.Find(value, ignoreCase, matchBy);

            return member != null;
        }
        
        public static bool operator ==(StringEnum<TEnum>? a, StringEnum<TEnum>? b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(StringEnum<TEnum>? a, StringEnum<TEnum>? b)
        {
            return !(a == b);
        }

        public static explicit operator string(StringEnum<TEnum> @enum)
        {
            return @enum._value;
        }
    }
}