using System.ComponentModel.DataAnnotations;

public class JogoInput
{
  
    [Required]
    public string Nome { get; set; }

    [Required]
    [Range(1980, int.MaxValue, ErrorMessage = "Ano de lançamento deve ser a partir de 1980.")]
    public int AnoLancamento { get; set; }

    [Required]
    [Range(0.01, 9999999999.99, ErrorMessage = "Preço deve ser maior que zero.")]
    public decimal PrecoBase { get; set; }
}