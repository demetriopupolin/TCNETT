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
    public class JogoController : ControllerBase
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoController(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        /// <summary>
        /// Retorna todos os jogos
        /// </summary>
        /// <returns>Lista de jogos</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var jogos = _jogoRepository.ObterTodos()
                    .OrderBy(p => p.Id)
                    .ToList(); ;

                var jogosDto = new List<JogoDto>();
               
                foreach (var jogo in jogos)
                {
                        jogosDto.Add(new JogoDto()
                    {
                        Id = jogo.Id,
                        DataCriacao = jogo.DataCriacao,
                        Nome = jogo.Nome,
                        AnoLancamento = jogo.AnoLancamento,
                        PrecoBase = jogo.PrecoBase,
                        Pedidos = jogo.Pedidos.Select(pedido => new PedidoDto()
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
                    });
                }

                return Ok(jogosDto);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter todos os jogos.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Busca um jogo específico pelo ID
        /// </summary>
        /// <param name="id">ID do Jogo</param>
        /// <returns>Jogo encontrado</returns>
        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var jogo = _jogoRepository.ObterPorId(id);

                if (jogo == null)
                    return NotFound("Jogo não encontrado.");

                var jogoDto = new JogoDto
                {
                    Id = jogo.Id,
                    DataCriacao = jogo.DataCriacao,
                    Nome = jogo.Nome,
                    AnoLancamento = jogo.AnoLancamento,
                    PrecoBase = jogo.PrecoBase,
                    Pedidos = jogo.Pedidos.Select(pedido => new PedidoDto
                    {
                        Id = pedido.Id,
                        DataCriacao = pedido.DataCriacao,
                        UsuarioId = pedido.UsuarioId,
                        JogoId = pedido.Id,
                        PromocaoId = pedido.PromocaoId,
                        VlPedido = pedido.VlPedido,
                        VlDesconto = pedido.VlDesconto,
                        VlPago = pedido.VlPago
                    }).ToList()
                };

                return Ok(jogoDto);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter o jogo.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Cadastrar Jogo
        /// </summary>
        /// <param name="input">Objeto com os dados do Jogo</param>
        /// <returns>Jogo cadastrado com sucesso</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] JogoInput input)
        {
            try
            {
                // Use object initializer to set required properties explicitly
                var jogo = new Jogo
                {
                    Nome = input.Nome,
                    AnoLancamento = input.AnoLancamento,
                    PrecoBase = input.PrecoBase
                };

                _jogoRepository.Cadastrar(jogo);

                return Ok(new { Message = "Jogo cadastrado." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao inserir jogo.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Alterar Jogo
        /// </summary>
        /// <param name="input">Objeto com os dados do Jogo</param>
        /// <returns>Jogo alterado</returns>
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public IActionResult Put([FromBody] JogoUpdateInput input)
        {
            try
            {
                var jogo = _jogoRepository.ObterPorId(input.Id);
                
                if (jogo == null)
                    return NotFound("Jogo não encontrado.");

                jogo.Nome = input.Nome;
                jogo.AnoLancamento = input.AnoLancamento;
                jogo.PrecoBase = input.PrecoBase;

                _jogoRepository.Alterar(jogo);

                return Ok(new { Message = "Jogo alterado." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao alterar jogo.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Excluir um jogo específico pelo ID
        /// </summary>
        /// <param name="id">ID do Jogo</param>
        /// <returns>Excluir Jogo</returns>
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {

            if (_jogoRepository.ObterPorId(id) == null)
                return NotFound("Jogo inexistente.");

            if (_jogoRepository.JogoTemPedidos(id))
                return BadRequest("Jogo possui pedidos. Exclusão não permitida.");

            try
            {
                _jogoRepository.Deletar(id);
                return Ok(new { Message = "Jogo excluído." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao deletar jogo.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Cadastro em Massa de Jogos
        /// </summary>
        [Authorize(Policy = "Admin")]
        [HttpPost(("cadastro-em-massa"))]
        public IActionResult CadastroEmMassa()
        {

            try
            {
                var jogos = new List<Jogo>()
                {
                    new Jogo() { Nome = "SONIC"         , AnoLancamento = 2025, PrecoBase = 100.00M },
                    new Jogo() { Nome = "SUPER MARIO"   , AnoLancamento = 2025, PrecoBase = 200.00M },
                    new Jogo() { Nome = "MORTAL KOMBAT" , AnoLancamento = 2025, PrecoBase = 300.00M },
                };

                _jogoRepository.CadastrarEmMassa(jogos);
                return Ok(new { Message = "Jogos cadastrados em massa." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Erro ao cadastrar jogos em massa", Error = e.Message });

            }


        }

    }
}
