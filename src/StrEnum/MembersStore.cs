using System;
using System.Collections.Concurrent;

namespace StrEnum
{
    /// <summary>
    /// Stores enum members to allow their lookup by a name or a value
    /// </summary>
    /// <typeparam name="TEnum">The type of the string enum</typeparam>
    internal class MembersStore<TEnum> where TEnum : StringEnum<TEnum>, new()
    {
        private static readonly ConcurrentDictionary<string, TEnum> OriginalCaseNameMembersStore = new();
        private static readonly ConcurrentDictionary<string, TEnum> OriginalCaseValueMembersStore = new();

        private static readonly ConcurrentDictionary<string, TEnum> UpperCaseNameMembersStore = new();
        private static readonly ConcurrentDictionary<string, TEnum> UpperCaseValueMembersStore = new();

        public bool TryAdd(TEnum member)
        {
            var name = member.ToString();
            var upperName = name.ToUpperInvariant();

            var value = (string)member;
            var upperValue = value.ToUpperInvariant();

            if (!OriginalCaseNameMembersStore.TryAdd(name, member))
                return false;

            OriginalCaseValueMembersStore.TryAdd(value, member);

            UpperCaseNameMembersStore.TryAdd(upperName, member);
            UpperCaseValueMembersStore.TryAdd(upperValue, member);

            return true;
        }

        public TEnum? Find(string nameOrValue, bool ignoreCase)
        {
            if (ignoreCase)
            {
                return FindCaseInsensitive(nameOrValue);
            }

            return FindCaseSensitive(nameOrValue);
        }

        private static TEnum? FindCaseSensitive(string nameOrValue)
        {
            if (OriginalCaseNameMembersStore.TryGetValue(nameOrValue, out var foundMember))
                return foundMember;

            if (OriginalCaseValueMembersStore.TryGetValue(nameOrValue, out foundMember))
                return foundMember;

            return null;
        }

        private static TEnum? FindCaseInsensitive(string nameOrValue)
        {
            var upperNameOrValue = nameOrValue.ToUpperInvariant();

            if (UpperCaseNameMembersStore.TryGetValue(upperNameOrValue, out var foundMember))
                return foundMember;

            if (OriginalCaseValueMembersStore.TryGetValue(upperNameOrValue, out foundMember))
                return foundMember;

            return null;
        }
    }
}