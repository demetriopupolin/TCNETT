using Core.Entity;
using Core.Input;
using Core.Repository;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PromocaoController : ControllerBase
    {
        private readonly IPromocaoRepository _promocaoRepository;

        public PromocaoController(IPromocaoRepository promocaoRepository)
        {
            _promocaoRepository = promocaoRepository;
        }

        /// <summary>
        /// Retorna todas as promoções
        /// </summary>
        /// <returns>Lista de promoções</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {

                var promocoes = _promocaoRepository.ObterTodos()
                    .OrderBy(p => p.Id)
                    .ToList();

                var promocoesDto = new List<PromocaoDto>();

                foreach (var promocao in promocoes)
                {
                    promocoesDto.Add(new PromocaoDto()
                    {
                        Id = promocao.Id,
                        DataCriacao = promocao.DataCriacao,
                        Nome = promocao.Nome,
                        Desconto = promocao.Desconto,
                        DataValidade = promocao.DataValidade,
                        Pedidos = promocao.Pedidos.Select(pedido => new PedidoDto()
                        {
                            Id = pedido.Id,
                            DataCriacao = pedido.DataCriacao,
                            UsuarioId = pedido.UsuarioId,
                            JogoId = pedido.JogoId,
                            PromocaoId = pedido.PromocaoId
                        }).ToList()
                    });
                }

                return Ok(promocoesDto);

            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter todas as promoções.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Busca uma promoção específico pelo ID
        /// </summary>
        /// <param name="id">ID da Promoção</param>
        /// <returns>Promoção encontrada</returns>
        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var promocao = _promocaoRepository.ObterPorId(id);

                if (promocao == null)
                    return NotFound("Promoção não encontrada.");

                var promocaoDto = new PromocaoDto
                {
                    Id = promocao.Id,
                    DataCriacao = promocao.DataCriacao,
                    Nome = promocao.Nome,
                    DataValidade = promocao.DataValidade,
                    Desconto = promocao.Desconto,
                    Pedidos = promocao.Pedidos.Select(pedido => new PedidoDto
                    {
                        Id = pedido.Id,
                        DataCriacao = pedido.DataCriacao,
                        UsuarioId = pedido.UsuarioId,
                        JogoId = pedido.JogoId,
                        PromocaoId = pedido.PromocaoId,
                        VlPedido = pedido.VlPedido,
                        VlDesconto = pedido.VlDesconto,
                        VlPago = pedido.VlPago
                    }).ToList()
                };

                return Ok(promocaoDto);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter o Promoção.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Cadastrar Promoção
        /// </summary>
        /// <param name="input">Objeto com os dados da Promoção</param>
        /// <returns>Promoção cadastrada com sucesso</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] PromocaoInput input)
        {
            try
            {
                var promocao = new Promocao
                {
                    Nome = input.Nome,
                    Desconto = input.Desconto,
                    DataValidade = input.DataValidade
                };

                _promocaoRepository.Cadastrar(promocao);

                return Ok(new { Message = "Promoção cadastrada." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao inserir promoção.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Alterar Promoção
        /// </summary>
        /// <param name="input">Objeto com os dados da Promoção</param>
        /// <returns>Promoção alterada</returns>
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public IActionResult Put([FromBody] PromocaoUpdateInput input)
        {
            try
            {
                var promocao = _promocaoRepository.ObterPorId(input.Id);

                if (promocao == null)
                    return NotFound("Promoção não encontrada.");

                promocao.Desconto = input.Desconto;
                promocao.DataValidade = input.DataValidade;

                _promocaoRepository.Alterar(promocao);

                return Ok(new { Message = "Promoção alterada." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao alterar promoção.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Excluir uma Promoção específica pelo ID
        /// </summary>
        /// <param name="id">ID da Promoção</param>
        /// <returns>Excluir Promoção</returns>
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {

            if (_promocaoRepository.ObterPorId(id) == null)
                return NotFound("Promoção inexistente.");

            if (_promocaoRepository.PromocaoTemPedidos(id))
                return BadRequest("Promoção possui pedidos. Exclusão não permitida.");
            
            try
            {
                _promocaoRepository.Deletar(id);
                return Ok(new { Message = "Promoção excluída." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao deletar promoção.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Cadastro em Massa de Promoções
        /// </summary>
        [Authorize(Policy = "Admin")]
        [HttpPost("cadastro-em-massa")]
        public IActionResult CadastroEmMassa()
        {
            try
            {
                var promocoes = new List<Promocao>()
                {
                    new Promocao() { Nome = "PROMOCAO NATAL"  , Desconto = 10, DataValidade = DateTime.Now.AddDays(30) },
                    new Promocao() { Nome = "BLACK FRIDAY"    , Desconto = 15, DataValidade = DateTime.Now.AddDays(20) },
                    new Promocao() { Nome = "DIA DAS CRIANCAS", Desconto = 20, DataValidade = DateTime.Now.AddDays(10) }
                };

                _promocaoRepository.CadastrarEmMassa(promocoes);
                return Ok(new { Message = "Promoções cadastradas em massa." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Erro ao cadastrar promocoes em massa", Error = e.Message });
            }
        }
    }
}
