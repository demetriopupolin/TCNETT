using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Xunit.Abstractions;

namespace tests
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepo;
        private readonly UsuarioController _controller;
        private readonly ITestOutputHelper _output;

        public UsuarioControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _mockRepo = new Mock<IUsuarioRepository>();
            _controller = new UsuarioController(_mockRepo.Object);
        }

        [Fact]
        public void Get_DeveRetornarListaUsuarios()
        {
            // Arrange
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "User1", Email = "u1@test.com", Nivel = 'U', Senha = "123456#A", Pedidos = new List<Pedido>() },
                new Usuario { Id = 2, Nome = "User2", Email = "u2@test.com", Nivel = 'A', Senha = "654321$B", Pedidos = new List<Pedido>() }
            };
            _mockRepo.Setup(r => r.ObterTodos()).Returns(usuarios);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsType<List<UsuarioDto>>(okResult.Value);
            Assert.Equal(2, lista.Count);
            Assert.Equal("User1", lista[0].Nome);
            _output.WriteLine($"Usuarios retornados: {string.Join(", ", lista.Select(u => u.Nome))}");
        }

        [Fact]
        public void GetPorId_QuandoUsuarioExiste_DeveRetornarOkComUsuario()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nome = "User1", Email = "u1@test.com", Nivel = 'U', Senha = "123456#A", Pedidos = new List<Pedido>() };
            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(usuario);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var usuarioDto = Assert.IsType<UsuarioDto>(okResult.Value);
            Assert.Equal("User1", usuarioDto.Nome);
            _output.WriteLine($"Usuário retornado: {usuarioDto.Nome}, Email: {usuarioDto.Email}");
        }

        [Fact]
        public void GetPorId_QuandoUsuarioNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.ObterPorId(99)).Returns((Usuario)null);

            // Act
            var result = _controller.Get(99);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Usuário não encontrado.", notFound.Value);
            _output.WriteLine($"Resultado retornado: {notFound.Value}");
        }

        [Fact]
        public void Post_DeveCadastrarUsuarioComNivelU()
        {
            // Arrange
            var input = new UsuarioInput { Nome = "Novo", Email = "novo@test.com", Senha = "123456#A" };
            _mockRepo.Setup(r => r.ObterPorEmail(input.Email)).Returns((Usuario)null);
            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Usuario>()));

            // Act
            var result = _controller.Post(input);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Usuario cadastrado", ok.Value.ToString());
            _mockRepo.Verify(r => r.Cadastrar(It.Is<Usuario>(u => u.Nivel == 'U')), Times.Once);
            _output.WriteLine($"Resultado retornado: {ok.Value}");
        }

        [Fact]
        public void Post_DeveRetornarBadRequest_SeEmailExistir()
        {
            // Arrange
            var input = new UsuarioInput { Nome = "Novo", Email = "existe@test.com", Senha = "123456#A" };
            _mockRepo.Setup(r => r.ObterPorEmail(input.Email)).Returns(new Usuario
            {
                Nome = "Usuario Existente",
                Email = input.Email,
                Senha = "887766&A",
                Nivel = 'U'
            });

            // Act
            var result = _controller.Post(input);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Email {input.Email} já existe.", badRequest.Value);
            _output.WriteLine($"Resultado retornado: {badRequest.Value}");
        }
    }
}
