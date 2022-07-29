using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        // ReSharper disable once StaticMemberInGenericType
        private static int _runningOrder = -1;
        private static readonly ConcurrentDictionary<int, TEnum> OrderMemberStore = new();

        public bool TryAdd(TEnum member)
        {
            var name = member.ToString();
            var upperName = name.ToUpperInvariant();

            var value = (string)member;
            var upperValue = value.ToUpperInvariant();

            var order = Interlocked.Increment(ref _runningOrder);

            if (!OriginalCaseNameMembersStore.TryAdd(name, member))
                return false;

            OrderMemberStore.TryAdd(order, member);

            OriginalCaseValueMembersStore.TryAdd(value, member);

            UpperCaseNameMembersStore.TryAdd(upperName, member);
            UpperCaseValueMembersStore.TryAdd(upperValue, member);

            return true;
        }

        public IReadOnlyCollection<TEnum> List()
        {
            var orderedMembers = OrderMemberStore
                .OrderBy(o => o.Key)
                .Select(o => o.Value).ToArray();

            return Array.AsReadOnly(orderedMembers);
        }

        public TEnum? Find(string value, bool ignoreCase, MatchBy matchBy)
        {
            if (ignoreCase)
            {
                return FindCaseInsensitive(value, matchBy);
            }

            return FindCaseSensitive(value, matchBy);
        }

        private static TEnum? FindCaseSensitive(string value, MatchBy matchBy)
        {
            if (OriginalCaseValueMembersStore.TryGetValue(value, out var foundMember))
                return foundMember;

            if (matchBy == MatchBy.ValueOnly)
                return null;

            if (OriginalCaseNameMembersStore.TryGetValue(value, out foundMember))
                return foundMember;

            return null;
        }

        private static TEnum? FindCaseInsensitive(string value, MatchBy matchBy)
        {
            var upperValue = value.ToUpperInvariant();

            if (UpperCaseValueMembersStore.TryGetValue(upperValue, out var foundMember))
                return foundMember;

            if (matchBy == MatchBy.ValueOnly)
                return null;

            if (UpperCaseNameMembersStore.TryGetValue(upperValue, out foundMember))
                return foundMember;

            return null;
        }
    }
}