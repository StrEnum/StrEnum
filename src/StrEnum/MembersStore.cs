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

        public bool TryAdd(TEnum member)
        {
            var name = member.ToString();
            var value = (string)member;

            if (!OriginalCaseNameMembersStore.TryAdd(name, member))
                return false;

            OriginalCaseValueMembersStore.TryAdd(value, member);
            
            return true;
        }

        public TEnum? Find(string nameOrValue)
        {
            if (OriginalCaseNameMembersStore.TryGetValue(nameOrValue, out var foundMember))
                return foundMember;

            if (OriginalCaseValueMembersStore.TryGetValue(nameOrValue, out foundMember))
                return foundMember;

            return null;
        }
    }
}