using System;
using System.Text.RegularExpressions;

public class Senha
{
    public string Value { get; }

    public Senha(string value)
    {
        if (!EhSegura(value))
            throw new ArgumentException("Senha deve ter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.");

        Value = value;
    }

    private bool EhSegura(string senha)
    {
        if (senha.Length < 8)
            return false;

        bool temLetra = Regex.IsMatch(senha, @"[a-zA-Z]");
        bool temNumero = Regex.IsMatch(senha, @"\d");
        bool temEspecial = Regex.IsMatch(senha, @"[\W_]"); // não alfanuméricos

        return temLetra && temNumero && temEspecial;
    }

    public override bool Equals(object obj) =>
        obj is Senha other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
