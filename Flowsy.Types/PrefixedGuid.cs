using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Flowsy.Types.Resources;

namespace Flowsy.Types;

public sealed class PrefixedGuid : IComparable<PrefixedGuid>, IEquatable<PrefixedGuid>
{
    private static readonly Regex GuidExpression = 
        new ("^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
    
    private const string EmptyPrefix = "0";
    
    public static readonly PrefixedGuid Empty = new(EmptyPrefix, Guid.Empty);
    
    public PrefixedGuid(string prefix, Guid guid)
    {
        Prefix = !string.IsNullOrEmpty(prefix) ? prefix : EmptyPrefix;
        Guid = guid;
    }

    public string Prefix { get; }
    public Guid Guid { get; }
    
    public bool IsEmpty => this == Empty;
    
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Prefix.GetHashCode();
            hash = hash * 23 + Guid.GetHashCode();
            return hash;
        }
    }

    public override string ToString() => $"{Prefix}_{Guid}";

    public static PrefixedGuid New(string prefix)
        => new (prefix, Guid.NewGuid());
    
    public static PrefixedGuid Parse(string value)
    {
        if (ReferenceEquals(value, null))
            return Empty;
        
        var parts = value.Split('_');
        if (parts.Length != 2 || parts.Any(s => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)))
            throw new ArgumentException(string.Format(Strings.InvalidFormatForX, nameof(PrefixedGuid)));

        return new PrefixedGuid(parts[0], Guid.Parse(parts[1]));
    }
    
    public static bool TryParse(
        string stringValue,
        [MaybeNullWhen(false)] out PrefixedGuid prefixedGuid
    )
    {
        prefixedGuid = null;
        
        var parts = stringValue.Split('_');
        if (parts.Length != 2)
            return false;

        if (!Guid.TryParse(parts[1], out var guid))
            return false;
        
        prefixedGuid = new PrefixedGuid(parts[0], guid);
        return true;
    }

    public int CompareTo(PrefixedGuid? other)
        => other is not null
            ? string.Compare(ToString(), other.ToString(), StringComparison.Ordinal)
            : -1;

    public bool Equals(PrefixedGuid? other) => Equals(other as object);
    
    public override bool Equals(object? obj) 
        => obj is PrefixedGuid other && Prefix == other.Prefix && Guid == other.Guid;

    private static bool IsEmptyValue(string prefix, string guid)
        => IsEmptyValue(prefix, Guid.Parse(guid));

    private static bool IsEmptyValue(string prefix, Guid guid)
        => prefix == EmptyPrefix && guid == Guid.Empty;
    
    public static bool IsValid(string value)
    {
        var parts = value.Split('_');
        if (parts.Length != 2)
            return false;
        
        var prefix = parts[0];
        var guid = parts[1];

        if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(guid))
            return false;

        if (IsEmptyValue(prefix, guid))
            return true;

        return prefix.Length > 0 && GuidExpression.IsMatch(guid);
    }
    
    public static implicit operator string(PrefixedGuid value) => value.ToString();
    public static implicit operator PrefixedGuid(string value) => Parse(value);

    public static bool operator ==(PrefixedGuid? left, PrefixedGuid? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;
        
        return left.Equals(right);
    }

    public static bool operator !=(PrefixedGuid? left, PrefixedGuid? right)
    {
        return !(left == right);
    }
}