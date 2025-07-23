using System.ComponentModel.DataAnnotations;

public class JogoInput
{
  
    [Required]
    public string Nome { get; set; }

    [Required]
    public int AnoLancamento { get; set; }

    [Required]
    [Range(0.01, 9999999999.99, ErrorMessage = "Preço deve ser maior que zero.")]
    public decimal PrecoBase { get; set; }
}