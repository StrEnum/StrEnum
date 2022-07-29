namespace StrEnum
{

    /// <summary>
    /// Specifies the way StrEnum matches members to the input string when performing parsing.
    /// </summary>
    public enum MatchBy
    {
        /// <summary>
        /// The member is a match if either its name or value is equal to the input string.
        /// </summary>
        NameOrValue,
        /// <summary>
        /// The member is a match if its value is equal to the input string.
        /// </summary>
        ValueOnly
    }
}