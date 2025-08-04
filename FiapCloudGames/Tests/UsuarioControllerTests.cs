using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepo;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _mockRepo = new Mock<IUsuarioRepository>();
            _controller = new UsuarioController(_mockRepo.Object);
        }

        [Fact]
        public void CTUS001_CadastroSemNome_DeveRetornarErroNomeInvalido()
        {
            var input = new UsuarioInput
            {
                Nome = null,
                Email = "teste@email.com",
                Senha = "Senha123!"
            };

            var result = _controller.Post(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Nome é obrigatório.", badRequest.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS002_CadastroEmailInvalido_DeveRetornarErroEmail()
        {
            var input = new UsuarioInput
            {
                Nome = "José",
                Email = "josedominio.com.br",
                Senha = "Senha123!"
            };

            var result = _controller.Post(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("E-mail inválido", badRequest.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS003_CadastroSenhaCurta_DeveRetornarErroSenhaCurta()
        {
            var input = new UsuarioInput
            {
                Nome = "Ana",
                Email = "ana@email.com",
                Senha = "R$1A"
            };

            var result = _controller.Post(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("A senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.", badRequest.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS004_CadastroSenhaSemEspecialOuNumero_DeveRetornarErro()
        {
            var input = new UsuarioInput
            {
                Nome = "Bruno",
                Email = "bruno@email.com",
                Senha = "abcdefgh"
            };

            var result = _controller.Post(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("A senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.", badRequest.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS005_CadastroValido_DeveRetornarSucesso()
        {
            var input = new UsuarioInput
            {
                Nome = "João Silva",
                Email = "joao@email.com",
                Senha = "Senha123!"
            };

            _mockRepo.Setup(r => r.ObterPorEmail(input.Email)).Returns((Usuario)null);
            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Usuario>()));

            var result = _controller.Post(input);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Usuario cadastrado", ok.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS006_CadastroEmailJaExistente_DeveRetornarErro()
        {
            var input = new UsuarioInput
            {
                Nome = "Maria da Silva",
                Email = "joao@email.com",
                Senha = "Maria123!"
            };
            _mockRepo.Setup(r => r.ObterPorEmail(input.Email))
                     .Returns(new Usuario
                     {
                         Nome = input.Nome,
                         Email = input.Email,
                         Senha = input.Senha,
                         Nivel = 'U'
                     });


            var result = _controller.Post(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("já existe", badRequest.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS007_CadastroAdminValido_DeveRetornarSucesso()
        {
            var input = new UsuarioInput
            {
                Nome = "Pedro Paulo",
                Email = "pedro@admin.com",
                Senha = "Abcd123!"
            };

            _mockRepo.Setup(r => r.ObterPorEmail(input.Email)).Returns((Usuario)null);
            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Usuario>()));

            var result = _controller.CadastrarAdmin(input);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Usuario cadastrado", ok.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS008_ExcluirUsuarioSemPedidos_DeveRetornarSucesso()
        {
            int userId = 10;

            _mockRepo.Setup(r => r.ObterPorId(userId)).Returns(new Usuario
            {
                Nome = "Teste",
                Email = "teste@email.com",
                Senha = "Senha123!",
                Nivel = 'U',
                Id = userId,
                Pedidos = new List<Pedido>() // importante evitar null
            });

            _mockRepo.Setup(r => r.UsuarioTemPedidos(userId)).Returns(false);
            _mockRepo.Setup(r => r.Deletar(userId));

            var result = _controller.Put(userId);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Usuario excluído", ok.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CTUS009_ExcluirUsuarioComPedidos_DeveRetornarErro()
        {
            int userId = 5;

            var usuario = new Usuario
            {
                Id = userId,
                Nome = "Carlos Teste",
                Email = "carlos@teste.com",
                Senha = "Senha123!",
                Nivel = 'U',
                Pedidos = new List<Pedido>()
            };

            var jogo = new Jogo
            {
                Id = 1,
                Nome = "Jogo Teste",
                PrecoBase = 100,
                DataCriacao = DateTime.Now
            };

            var pedido = new Pedido
            {
                Id = 1,
                UsuarioId = usuario.Id,
                JogoId = jogo.Id,
                DataCriacao = DateTime.Now
            };

            pedido.ValidarECalcularPedido(usuario, jogo); // calcula VlPedido, VlDesconto e VlPago

            usuario.Pedidos.Add(pedido);

            _mockRepo.Setup(r => r.ObterPorId(userId)).Returns(usuario);
            _mockRepo.Setup(r => r.UsuarioTemPedidos(userId)).Returns(true);

            var result = _controller.Put(userId);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Contains("Exclusão não permitida", badRequest.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }



    }
}
