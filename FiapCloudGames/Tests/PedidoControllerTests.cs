using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Security.Claims;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PedidosControllerTests
    {
        private readonly Mock<IPedidoRepository> _mockPedidoRepo;
        private readonly Mock<IPromocaoRepository> _mockPromocaoRepo;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepo;
        private readonly Mock<IJogoRepository> _mockJogoRepo;
        private readonly PedidoController _controller;
        private readonly ITestOutputHelper _output;

        public PedidosControllerTests()
        {
            _mockPedidoRepo = new Mock<IPedidoRepository>();
            _mockPromocaoRepo = new Mock<IPromocaoRepository>();
            _mockUsuarioRepo = new Mock<IUsuarioRepository>();
            _mockJogoRepo = new Mock<IJogoRepository>();

            _controller = new PedidoController(
                _mockPedidoRepo.Object,
                _mockPromocaoRepo.Object,
                _mockUsuarioRepo.Object,
                _mockJogoRepo.Object);
        }

        [Fact]
        public void CTPD001_CadastroSemJogo_DeveRetornarErro()
        {


            // Simula usuário autenticado com Claim de NameIdentifier
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "mock"))
                }
            };


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });

            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });

            var input = new PedidoInputUsuario
            {
                JogoId = null,
                PromocaoId = null,
            };

            var result = _controller.Post(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Jogo não encontrado", badRequest.Value);
        }

        [Fact]
        public void CTPD002_CadastroSemPromocao_DeveRetornarSucesso()
        {

            // Simula usuário autenticado com Claim de NameIdentifier
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "mock"))
                }
            };


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });


            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });

            var input = new PedidoInputUsuario
            {
                JogoId = 8,
                PromocaoId = null,
            };

            var result = _controller.Post(input);
            var ok = Assert.IsType<OkObjectResult>(result);
            var mensagem = ok.Value?.GetType().GetProperty("Message")?.GetValue(ok.Value, null) as string;

            Assert.Equal("Pedido cadastrado.", mensagem);
        }

        [Fact]
        public void CTPD003_CadastroComPromocaoExpirada_DeveRetornarErro()
        {

            // Simula usuário autenticado com Claim de NameIdentifier
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "mock"))
                }
            };


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });

            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });

            // Mock para retornar promocao valida
            Promocao promo = new Promocao("Promo Teste", 20, DateTime.Now.AddDays(365));
            promo.Id = 10;
            promo.DataCriacao = DateTime.Now.AddDays(-5);
            promo.AtualizarValidade(DateTime.Now.AddDays(-1));

            _mockPromocaoRepo.Setup(repo => repo.ObterPorId(10))
                .Returns(promo);

            var dto = new PedidoInputUsuario
            {
                JogoId = 8,
                PromocaoId = 10
            };

            var result = _controller.Post(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var erro = badRequest.Value?.GetType().GetProperty("Error")?.GetValue(badRequest.Value, null) as string;

            Assert.Equal("Promoção expirada para a data do pedido.", erro);
        }

        [Fact]
        public void CTPD004_CadastroComPromocaoValida_DeveRetornarSucesso()
        {

            // Simula usuário autenticado com Claim de NameIdentifier
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "mock"))
                }
            };


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });


            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });


            // Mock para retornar promocao valida
            Promocao promo = new Promocao("Promo Teste", 20, DateTime.Now.AddDays(365));
            promo.Id = 10;
            _mockPromocaoRepo.Setup(repo => repo.ObterPorId(10))
                .Returns(promo);

            var input = new PedidoInputUsuario
            {
                JogoId = 8,
                PromocaoId = 10
            };

            var result = _controller.Post(input);
            var ok = Assert.IsType<OkObjectResult>(result);
            var mensagem = ok.Value?.GetType().GetProperty("Message")?.GetValue(ok.Value, null) as string;

            Assert.Equal("Pedido cadastrado.", mensagem);
        }


        [Fact]
        public void CTPD005_CadastroSemUsuario_AdministradorAutenticado_DeveRetornarErro()
        {


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });

            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });

            var input = new PedidoInput
            {
                JogoId = 8,
                PromocaoId = null,
                UsuarioId = null
            };

            var result = _controller.PostPorUsuario(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuario não encontrado", badRequest.Value);
        }


        [Fact]
        public void CTPD006_CadastroSemJogo_AdministradorAutenticado_DeveRetornarErro()
        {


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });

            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });

            var input = new PedidoInput
            {
                JogoId = null,
                PromocaoId = null,
                UsuarioId = 123
            };

            var result = _controller.PostPorUsuario(input);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Jogo não encontrado", badRequest.Value);
        }






        [Fact]
        public void CTPD007_PedidoSemPromocao_AdministradorAutenticado_DeveRetornarSucesso()
        {


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });

            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });

            var input = new PedidoInput
            {
                JogoId = 8,
                PromocaoId = null,
                UsuarioId = 123
            };

            var result = _controller.PostPorUsuario(input);
            var ok = Assert.IsType<OkObjectResult>(result);
            var mensagem = ok.Value?.GetType().GetProperty("Message")?.GetValue(ok.Value, null) as string;

            Assert.Equal("Pedido cadastrado.", mensagem);
        }








        [Fact]
        public void CTPD008_CadastroComPromocaoExpirada_AdministratorExpirado_DeveRetornarErro()
        {

            // Simula usuário autenticado com Claim de NameIdentifier
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "mock"))
                }
            };


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });

            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });

            // Mock para retornar promocao valida
            Promocao promo = new Promocao("Promo Teste", 20, DateTime.Now.AddDays(365));
            promo.Id = 10;
            promo.DataCriacao = DateTime.Now.AddDays(-5);
            promo.AtualizarValidade(DateTime.Now.AddDays(-1));

            _mockPromocaoRepo.Setup(repo => repo.ObterPorId(10))
                .Returns(promo);

            var dto = new PedidoInput
            {
                JogoId = 8,
                PromocaoId = 10,
                UsuarioId = 123
            };

            var result = _controller.PostPorUsuario(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var erro = badRequest.Value?.GetType().GetProperty("Error")?.GetValue(badRequest.Value, null) as string;

            Assert.Equal("Promoção expirada para a data do pedido.", erro);
        }






        [Fact]
        public void CTPD009_PedidoComPromocaoValida_AdministradorAutenticado_DeveRetornarSucesso()
        {


            // Mock para retornar usuário válido
            _mockUsuarioRepo.Setup(repo => repo.ObterPorId(123))
                .Returns(new Usuario { Id = 123, Nome = "Usuário Teste", Nivel = 'U' });

            // Mock para retornar jogo válido
            _mockJogoRepo.Setup(repo => repo.ObterPorId(8))
                .Returns(new Jogo { Id = 8, Nome = "Jogo Teste", PrecoBase = 100 });
            
            // Mock promocao
            Promocao promo = new Promocao("Promo Teste", 20, DateTime.Now.AddDays(365));
            promo.Id = 10;
            _mockPromocaoRepo.Setup(repo => repo.ObterPorId(10))
                .Returns(promo);


            var input = new PedidoInput
            {
                JogoId = 8,
                PromocaoId = 10,
                UsuarioId = 123
            };

            var result = _controller.PostPorUsuario(input);
            var ok = Assert.IsType<OkObjectResult>(result);
            var mensagem = ok.Value?.GetType().GetProperty("Message")?.GetValue(ok.Value, null) as string;

            Assert.Equal("Pedido cadastrado.", mensagem);
        }



    }
}
