using Core.Entity;
using Core.Input;
using Core.Repository;
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
                return Ok(_usuarioRepository.ObterTodos());
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
                return Ok(_usuarioRepository.ObterPorId(id));
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Erro ao obter usuario.",
                    Error = e.Message,
                    inner = e.InnerException?.Message
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
            try
            {
                Email email;
                try
                {
                    email = new Email(input.Email);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message); 
                }

                Senha senha;
                try
                {
                    senha = new Senha(input.Senha);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }

                var usuario = new Usuario()
                {
                    Nome = input.Nome,
                    Email = input.Email,
                    Senha = input.Senha,
                    // Nivel = input.Nivel
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
                Email email;
                try
                {
                    email = new Email(input.Email);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message); 
                }

                Senha senha;
                try
                {
                    senha = new Senha(input.Senha);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }

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
                return BadRequest(e);
            }
        }
    }
}
