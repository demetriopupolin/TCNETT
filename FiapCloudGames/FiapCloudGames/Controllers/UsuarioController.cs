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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Retorna todos os Usuários
        /// </summary>
        /// <returns>Lista de Usuários</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {

                var usuarios = _usuarioRepository.ObterTodos()
                    .OrderBy(p => p.Id)
                    .ToList();

                var usuarioDto = new List<UsuarioDto>();

                foreach (var usuario in usuarios)
                {
                    usuarioDto.Add(new UsuarioDto()
                    {
                        Id = usuario.Id,
                        DataCriacao = usuario.DataCriacao,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Nivel = usuario.Nivel,
                        Senha = usuario.Senha,
                        Pedidos = usuario.Pedidos.Select(pedido => new PedidoDto()
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

                return Ok(usuarioDto);


            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter todos os usuarios.",
                    Error = e.Message,
                    inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Busca um Usuário específico pelo ID
        /// </summary>
        /// <param name="id">ID do Usuário</param>
        /// <returns>Usuário encontrado</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var usuario = _usuarioRepository.ObterPorId(id);

                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id,
                    DataCriacao = usuario.DataCriacao,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Pedidos = usuario.Pedidos.Select(pedido => new PedidoDto
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

                return Ok(usuarioDto);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter o usuário.",
                    Error = e.Message,
                    Inner = e.InnerException?.Message
                });
            }
        }



        /// <summary>
        /// Cadastrar Usuário
        /// </summary>
        /// <param name="input">Objeto com os dados do Usuário</param>
        /// <returns>Usuário cadastrado com sucesso</returns>
        [HttpPost]
        public IActionResult Post([FromBody] UsuarioInput input)
        {
            return CadastrarUsuario(input, 'U'); // 'U' para usuário comum
        }

        /// <summary>
        /// Cadastrar Usuário Admin
        /// </summary>
        /// <param name="input">Objeto com os dados do Usuário Admin</param>
        /// <returns>Usuário Admin cadastrado com sucesso</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("cadastrar-admin")]
        public IActionResult CadastrarAdmin([FromBody] UsuarioInput input)
        {
            return CadastrarUsuario(input, 'A'); // 'A' para administrador
        }

        // Cadastro Generico de Usuario
        private IActionResult CadastrarUsuario(UsuarioInput input, char nivel)
        {
            try
            {
                if (_usuarioRepository.ObterPorEmail(input.Email) != null)
                {
                    return BadRequest($"Email {input.Email} já existe.");
                }

                var usuario = new Usuario()
                {
                    Nome = input.Nome,
                    Email = input.Email,
                    Senha = input.Senha,
                    Nivel = nivel
                };

                _usuarioRepository.Cadastrar(usuario);

                return Ok(new { Message = "Usuario cadastrado." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao inserir usuário.",
                    Error = e.Message,
                    inner = e.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Alterar Usuário
        /// </summary>
        /// <param name="input">Objeto com os dados do Usuário</param>
        /// <returns>Usuário alterado.</returns>
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public IActionResult Put([FromBody] UsuarioUpdateInput input)
        {
            try
            {
                var usuario = _usuarioRepository.ObterPorId(input.Id);

                usuario.Nome = input.Nome;
                usuario.Email = input.Email;
                usuario.Senha = input.Senha; 

                _usuarioRepository.Alterar(usuario);

                return Ok(new { Message = "Usuario alterado." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao alterar usuário.",
                    Error = e.Message,
                    inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Excluir um Usuário específico pelo ID
        /// </summary>
        /// <param name="id">ID do Usuário</param>
        /// <returns>Excluir Usuário</returns>
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Put([FromRoute] int id)
        {

            if (_usuarioRepository.ObterPorId(id) == null)
                return NotFound("Usuário inexistente.");

            if (_usuarioRepository.UsuarioTemPedidos(id))
                return BadRequest("Usuário possui pedidos. Exclusão não permitida.");
            
            try
            {
                _usuarioRepository.Deletar(id);
                return Ok(new { Message = "Usuario excluído." });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao apagar usuário.",
                    Error = e.Message,
                    inner = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Cadastro em Massa de Usuários
        /// </summary>
        [Authorize(Policy = "Admin")]
        [HttpPost(("cadastro-em-massa"))]
        public IActionResult CadastroEmMassa()
        {

            try
            {
                var usuarios = new List<Usuario>()
                {
                    new Usuario() { Nome = "DAVI DA SILVA"     , Email = "davi@uol.com.br"    , Senha = "123456$A", Nivel = 'U' },
                    new Usuario() { Nome = "MURILO DA SILVA"   , Email = "murilo@uol.com.br"  , Senha = "123456$B", Nivel = 'U' },
                    new Usuario() { Nome = "NATHALIE DA SILVA" , Email = "nathalie@uol.com.br", Senha = "123456$C", Nivel = 'U' },
                };

                _usuarioRepository.CadastrarEmMassa(usuarios);

                return Ok(new { Message = "Usuarios cadastrados em massa." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Erro ao cadastrar usuarios em massa", Error = e.Message });
            }
        }
    }
}
