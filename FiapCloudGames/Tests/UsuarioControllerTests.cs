using Xunit;
using Xunit.Abstractions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class UsuarioControllerTest
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly UsuarioController _controller;
        private readonly ITestOutputHelper _output;

        public UsuarioControllerTest(ITestOutputHelper output)
        {
            _output = output;
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _controller = new UsuarioController(_usuarioRepoMock.Object);
        }

        [Fact]
        public void Get_DeveRetornarUsuarios()
        {
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "Lúcio Martins", Email = "lucio@ex.com", Senha = "ABC123$#", Nivel = 'U', Pedidos = new List<Pedido>() }
            };
            _usuarioRepoMock.Setup(r => r.ObterTodos()).Returns(usuarios);

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<UsuarioDto>>(okResult.Value);
            Assert.Single(list);

            _output.WriteLine($"Total usuários retornados: {list.Count}");
            foreach (var u in list)
                _output.WriteLine($"Usuário: {u.Id} - {u.Nome} - {u.Email}");
        }

        [Fact]
        public void Get_DeveRetornarBadRequest_EmCasoDeErro()
        {
            _usuarioRepoMock.Setup(r => r.ObterTodos()).Throws(new Exception("Erro"));

            var result = _controller.Get();

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            _output.WriteLine($"Mensagem de erro: {badRequest.Value}");

            Assert.Contains("Erro ao obter todos os usuarios", badRequest.Value.ToString());
        }

        [Fact]
        public void GetById_DeveRetornarUsuario()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Clara Menezes",
                Email = "clara@ex.com",
                Senha = "ABC123$#",
                Pedidos = new List<Pedido>()
            };
            _usuarioRepoMock.Setup(r => r.ObterPorId(1)).Returns(usuario);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<UsuarioDto>(okResult.Value);

            _output.WriteLine($"Usuário retornado: {dto.Id} - {dto.Nome} - {dto.Email}");

            Assert.Equal("Clara Menezes", dto.Nome);
        }

        [Fact]
        public void GetById_DeveRetornarNotFound_SeNaoExistir()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorId(99)).Returns<Usuario>(null);

            var result = _controller.Get(99);

            _output.WriteLine("Usuário não encontrado (ID 99).");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Post_DeveCadastrarUsuario_ComSucesso()
        {
            var input = new UsuarioInput { Nome = "Valentina Costa", Email = "valentina@ex.com", Senha = "XZY456$%" };
            _usuarioRepoMock.Setup(r => r.ObterPorEmail(input.Email)).Returns<Usuario>(null);

            var result = _controller.Post(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            _output.WriteLine($"Cadastro realizado: {input.Nome} - {input.Email}");

            Assert.Contains("Usuario cadastrado", ok.Value.ToString());
        }

        [Fact]
        public void Post_DeveRetornarBadRequest_SeEmailJaExistir()
        {
            var input = new UsuarioInput { Nome = "Valentina Costa", Email = "valentina@ex.com", Senha = "XZY456$%" };
            _usuarioRepoMock.Setup(r => r.ObterPorEmail(input.Email)).Returns(new Usuario
            {
                Nome = input.Nome,
                Email = input.Email,
                Senha = input.Senha,
                Nivel = 'U',
                Pedidos = new List<Pedido>()
            });

            var result = _controller.Post(input);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            _output.WriteLine($"Falha ao cadastrar (email já existe): {input.Email}");

            Assert.Contains("já existe", badRequest.Value.ToString());
        }

        [Fact]
        public void CadastrarAdmin_DeveCadastrarAdmin_ComSucesso()
        {
            var input = new UsuarioInput { Nome = "Fernando Prado", Email = "fernando@ex.com", Senha = "DEF456$%" };
            _usuarioRepoMock.Setup(r => r.ObterPorEmail(input.Email)).Returns<Usuario>(null);

            var result = _controller.CadastrarAdmin(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            _output.WriteLine($"Admin cadastrado: {input.Nome} - {input.Email}");

            Assert.Contains("Usuario cadastrado", ok.Value.ToString());
        }

        [Fact]
        public void Put_DeveAlterarUsuario()
        {
            var usuario = new Usuario { Id = 1, Nome = "Lúcio Martins", Email = "lucio@ex.com", Senha = "ABC123$#", Pedidos = new List<Pedido>() };
            _usuarioRepoMock.Setup(r => r.ObterPorId(1)).Returns(usuario);

            var input = new UsuarioUpdateInput { Id = 1, Nome = "Bianca Furtado", Email = "bianca@ex.com", Senha = "NEW123$#" };
            var result = _controller.Put(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            _output.WriteLine($"Usuário alterado: ID {input.Id} - {input.Nome} - {input.Email}");

            Assert.Contains("Usuario alterado", ok.Value.ToString());
        }

        [Fact]
        public void Put_DeveRetornarBadRequest_EmErro()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorId(1)).Throws(new Exception("Erro"));
            var input = new UsuarioUpdateInput { Id = 1, Nome = "Bianca Furtado", Email = "bianca@ex.com", Senha = "NEW123$#" };

            var result = _controller.Put(input);

            _output.WriteLine("Erro ao alterar usuário com ID 1.");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Delete_DeveExcluirUsuario()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorId(1)).Returns(new Usuario
            {
                Id = 1,
                Nome = "Carlos Souza",
                Email = "carlos@ex.com",
                Senha = "PASS123$#",
                Nivel = 'U',
                Pedidos = new List<Pedido>()
            });
            _usuarioRepoMock.Setup(r => r.UsuarioTemPedidos(1)).Returns(false);

            var result = _controller.Put(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            _output.WriteLine("Usuário excluído: ID 1");

            Assert.Contains("Usuario excluído", ok.Value.ToString());
        }

        [Fact]
        public void Delete_DeveRetornarNotFound_SeNaoExistir()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorId(1)).Returns<Usuario>(null);

            var result = _controller.Put(1);

            _output.WriteLine("Tentativa de exclusão de usuário inexistente: ID 1");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Delete_DeveRetornarBadRequest_SeTiverPedidos()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorId(1)).Returns(new Usuario
            {
                Id = 1,
                Nome = "Carlos Souza",
                Email = "carlos@ex.com",
                Senha = "PASS123$#",
                Nivel = 'U',
                Pedidos = new List<Pedido>()
            });
            _usuarioRepoMock.Setup(r => r.UsuarioTemPedidos(1)).Returns(true);

            var result = _controller.Put(1);

            _output.WriteLine("Usuário com pedidos não pode ser excluído: ID 1");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CadastroEmMassa_DeveCadastrar_ComSucesso()
        {
            var result = _controller.CadastroEmMassa();
            var ok = Assert.IsType<OkObjectResult>(result);

            _output.WriteLine("Cadastro em massa realizado com sucesso.");

            Assert.Contains("Usuarios cadastrados em massa", ok.Value.ToString());
        }

        [Fact]
        public void CadastroEmMassa_DeveRetornarBadRequest_EmErro()
        {
            _usuarioRepoMock.Setup(r => r.CadastrarEmMassa(It.IsAny<List<Usuario>>())).Throws(new Exception("Erro"));

            var result = _controller.CadastroEmMassa();

            _output.WriteLine("Erro no cadastro em massa.");

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
