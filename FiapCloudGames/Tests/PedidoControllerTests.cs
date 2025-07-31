using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PedidoControllerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepoMock;
        private readonly Mock<IPromocaoRepository> _promocaoRepoMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<IJogoRepository> _jogoRepoMock;
        private readonly PedidoController _controller;
        private readonly ITestOutputHelper _output;

        public PedidoControllerTests(ITestOutputHelper output)
        {
            _pedidoRepoMock = new Mock<IPedidoRepository>();
            _promocaoRepoMock = new Mock<IPromocaoRepository>();
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _jogoRepoMock = new Mock<IJogoRepository>();

            _controller = new PedidoController(
                _pedidoRepoMock.Object,
                _promocaoRepoMock.Object,
                _usuarioRepoMock.Object,
                _jogoRepoMock.Object
            );

            _output = output;
        }

        [Fact]
        public void Get_DeveRetornarTodosOsPedidos()
        {
            var pedidos = new List<Pedido> {
                new Pedido {
                    Id = 1,
                    DataCriacao = DateTime.Now,
                    UsuarioId = 1,
                    JogoId = 1,
                    PromocaoId = 1
                }
            };

            _pedidoRepoMock.Setup(r => r.ObterTodos()).Returns(pedidos);

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsAssignableFrom<IEnumerable<Pedido>>(okResult.Value);
            Assert.Single(lista);
        }

        

        [Fact]
        public void GetById_DeveRetornarNotFound_SeNaoEncontrar()
        {
            _pedidoRepoMock.Setup(r => r.ObterPorId(99)).Returns<Pedido>(null);

            var result = _controller.Get(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Post_DeveCadastrarPedidoComSucesso()
        {
            var input = new PedidoInput
            {
                UsuarioId = 1,
                JogoId = 1,
                PromocaoId = 1
            };

            // Mock para usuário existir
            _usuarioRepoMock.Setup(r => r.ObterPorId(1)).Returns(new Usuario { Id = 1, Nome = "Teste" });
            // Mock para jogo existir
            _jogoRepoMock.Setup(r => r.ObterPorId(1)).Returns(new Jogo { Id = 1, Nome = "JogoTeste", PrecoBase = 100 });
            // Mock para promoção (se necessário)
            _promocaoRepoMock.Setup(r => r.ObterPorId(1)).Returns(new Promocao("Promo Nome", 20, DateTime.Now.AddDays(365)));

            _pedidoRepoMock.Setup(r => r.Cadastrar(It.IsAny<Pedido>()));

            var result = _controller.PostPorUsuario(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Pedido cadastrado", ok.Value.ToString());

            _pedidoRepoMock.Verify(r => r.Cadastrar(It.Is<Pedido>(p =>
                p.UsuarioId == input.UsuarioId &&
                p.JogoId == input.JogoId &&
                p.PromocaoId == input.PromocaoId
            )), Times.Once);
        }

        [Fact]
        public void Post_DeveRetornarBadRequest_EmErro()
        {
            var input = new PedidoInputUsuario
            {
                JogoId = 1,
                PromocaoId = 1
            };

            // Mock ClaimsPrincipal com claim de usuário
            var userId = "42"; // exemplo de id do usuário
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
               new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Mock para usuário existir
            _usuarioRepoMock.Setup(r => r.ObterPorId(int.Parse(userId))).Returns(new Usuario { Id = int.Parse(userId), Nome = "Teste" });
            // Mock para jogo existir
            _jogoRepoMock.Setup(r => r.ObterPorId(1)).Returns(new Jogo { Id = 1, Nome = "JogoTeste", PrecoBase = 100 });
            // Mock para promoção (se necessário)
            _promocaoRepoMock.Setup(r => r.ObterPorId(1)).Returns(new Promocao ("Promo Nome", 20, DateTime.Now.AddDays(365)));

            _pedidoRepoMock.Setup(r => r.Cadastrar(It.IsAny<Pedido>())).Throws(new Exception("Erro"));

            var result = _controller.Post(input);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao cadastrar pedido", bad.Value.ToString());
        }

        [Fact]
        public void Delete_DeveExcluirPedidoComSucesso()
        {
            var pedido = new Pedido { Id = 1, UsuarioId = 1, JogoId = 1 };
            _pedidoRepoMock.Setup(r => r.ObterPorId(1)).Returns(pedido);

            var result = _controller.Delete(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Pedido excluído", ok.Value.ToString());
        }

        [Fact]
        public void Delete_DeveRetornarNotFound_SeNaoEncontrar()
        {
            _pedidoRepoMock.Setup(r => r.ObterPorId(1)).Returns<Pedido>(null);

            var result = _controller.Delete(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

    }
}
