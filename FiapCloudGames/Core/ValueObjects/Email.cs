using System.Text.RegularExpressions;

public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (!Regex.IsMatch(value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            throw new ArgumentException("Email inválido");

        Value = value;
    }

    public override bool Equals(object obj) =>
        obj is Email other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
