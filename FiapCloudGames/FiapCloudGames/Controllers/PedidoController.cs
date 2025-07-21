using Core.Entity;
using Core.Input;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FiapCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;

        private readonly IPromocaoRepository _promocaoRepository;

        private readonly IUsuarioRepository _usuarioRepository;

        private readonly IJogoRepository _jogoRepository;


        public PedidoController(IPedidoRepository pedidoRepository, IPromocaoRepository promocaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _promocaoRepository = promocaoRepository;
        }

        /// <summary>
        /// Retorna todos os pedidos
        /// </summary>
        /// <returns>Lista de pedidos</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var pedidos = _pedidoRepository.ObterTodos()
                     .OrderBy(p => p.Id)
                     .ToList();

                var pedidosDto = new List<PedidoDto>();
                
                foreach (var pedido in pedidos)
                {
                    pedidosDto.Add(new PedidoDto()
                    {
                        Id = pedido.Id,
                        DataCriacao = pedido.DataCriacao,
                        UsuarioId = pedido.UsuarioId,
                        JogoId = pedido.JogoId,
                        PromocaoId = pedido.PromocaoId
                        
                    });
                }

                return Ok(pedidosDto);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter todos os pedidos.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Busca um pedido específico pelo ID
        /// </summary>
        /// <param name="id">ID da Pedido</param>
        /// <returns>Pedido encontrado</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var pedido = _pedidoRepository.ObterPorId(id);
                if (pedido == null)
                    return NotFound("Pedido não encontrado.");

                return Ok(pedido);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter o pedido.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Busca pedidos de um usuário autenticado.
        /// </summary>
        /// <param name="id">ID da Pedido.</param>
        /// <returns>Pedido encontrado.</returns>
        [Authorize]
        [HttpGet("pedidos-do-usuario")]
        public IActionResult GetPedidosPorUsuario()
        {
            try
            {
                
                int usuarioid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var pedidos = _pedidoRepository.ObterPedidosPorUsuario(usuarioid);

                var pedidosDto = pedidos.Select(p => new PedidoDto()
                {
                   Id = p.Id,
                  DataCriacao = p.DataCriacao,
                 UsuarioId = p.UsuarioId,
                JogoId = p.JogoId,
                PromocaoId = p.PromocaoId
               }).ToList();

                return Ok(pedidosDto);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter pedidos do usuário.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }






        private IActionResult CadastrarPedido(PedidoInput input, int usuarioId)
        {
            try
            {
                if (input.PromocaoId <= 0)
                    input.PromocaoId = null;

                if (input.PromocaoId != null)
                {
                    var promocao = _promocaoRepository.ObterPorId(input.PromocaoId.Value);

                    if (promocao == null)
                        return BadRequest($"Promoção não encontrada");

                    if (!promocao.EhValida())
                        return BadRequest($"Promoção válida para pedidos até {promocao.DataValidade:dd/MM/yyyy HH:mm:ss}");
                }

                var usuario = _usuarioRepository.ObterPorId(usuarioId);
                if (usuario == null)
                    return BadRequest($"Usuario não encontrado");

                var jogo = _jogoRepository.ObterPorId(input.JogoId);
                if (jogo == null)
                    return BadRequest($"Jogo não encontrado");

                var pedido = new Pedido
                {
                    UsuarioId = usuarioId,
                    JogoId = input.JogoId,
                    PromocaoId = input.PromocaoId
                };

                _pedidoRepository.Cadastrar(pedido);

                return Ok(new { Message = "Pedido cadastrado." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao inserir pedido.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }









        /// <summary>
        /// Cadastrar Pedido do usuário autenticado
        /// </summary>
        /// <param name="input">Objeto com os dados do Pedido</param>
        /// <returns>Pedido cadastrado com sucesso</returns>
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] PedidoInput input)
        {
            var usuarioIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!int.TryParse(usuarioIdStr, out var usuarioId) || usuarioId <= 0)
                return BadRequest("Usuário inválido.");

            return CadastrarPedido(input, usuarioId);
        }

        /// <summary>
        /// Cadastrar Pedido para um usuário específico
        /// </summary>
        /// <param name="input">Objeto com os dados do Pedido</param>
        /// <returns>Pedido cadastrado com sucesso</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("cadastrar-por-usuario")]
        public IActionResult PostPorUsuario([FromBody] PedidoInput input)
        {
            if (input.UsuarioId <= 0)
                return BadRequest("Id de usuário inválido.");

            return CadastrarPedido(input, input.UsuarioId);
        }




        /// <summary>
        /// Alterar Pedido
        /// </summary>
        /// <param name="input">Objeto com os dados do Pedido</param>
        /// <returns>Pedido alterado</returns>
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public IActionResult Put([FromBody] PedidoUpdateInput input)
        {
            try
            {
                // busca e valida pedido
                var pedido = _pedidoRepository.ObterPorId(input.Id);
                
                if (pedido == null)
                    return NotFound("Pedido não encontrado.");

                var usuario = _usuarioRepository.ObterPorId(input.UsuarioId);

                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                // valida promocao na data de criacao
                if (input.PromocaoId != null)
                {
                    var promocao = _promocaoRepository.ObterPorId(input.PromocaoId.Value);

                    if (!promocao.EhValidaNaData(pedido.DataCriacao))
                    {
                        return BadRequest($"Promoção expirada na data de criação do pedido. " +
                                          $"Data criação: {pedido.DataCriacao:dd/MM/yyyy}, " +
                                          $"Data validade promoção: {promocao.DataValidade:dd/MM/yyyy}");
                    }
                    
                }

                // tudo ok
                pedido.UsuarioId = input.UsuarioId;
                pedido.JogoId = input.JogoId;
                pedido.PromocaoId = input.PromocaoId;
                
                _pedidoRepository.Alterar(pedido);

                return Ok(new { Message = "Pedido alterado." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao alterar pedido.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Excluir um Pedido específico pelo ID
        /// </summary>
        /// <param name="id">ID do Pedido</param>
        /// <returns>Excluir Pedido</returns>
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _pedidoRepository.Deletar(id);
                return Ok(new { Message = "Pedido excluído." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao deletar pedido.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Cadastro em Massa de Pedidos
        /// </summary>
        [Authorize(Policy = "Authenticated")]
        [HttpPost("cadastro-em-massa")]
        public IActionResult CadastroEmMassa()
        {
            try
            {
                var pedidos = new List<Pedido>()
                {
                    new Pedido() { UsuarioId = 1, JogoId = 2, PromocaoId = 3, },
                    new Pedido() { UsuarioId = 2, JogoId = 3, PromocaoId = 1  },
                    new Pedido() { UsuarioId = 3, JogoId = 1, PromocaoId = 2, }
                };

                _pedidoRepository.CadastrarEmMassa(pedidos);
                return Ok(new { Message = "Pedidos cadastrados em massa." });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
