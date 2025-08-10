using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using Xunit;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tests
{
    public class JogoControllerTests
    {
        private readonly Mock<IJogoRepository> _mockRepo;
        private readonly JogoController _controller;
        private readonly ITestOutputHelper _output;

        public JogoControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _mockRepo = new Mock<IJogoRepository>();
            _controller = new JogoController(_mockRepo.Object);
        }
        
        [Fact]
        public void CTJO001_Cadastro_de_jogo_com_nome_nao_informado()
        {
            var input = new JogoInput
            {
                // Nome não informado (proposital)
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 200m
            };

            var result = _controller.Post(input);

            // Valida se o retorno foi BadRequest com mensagem
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var mensagemErro = badRequest.Value?.ToString().ToLower();

            Assert.Contains("nome", mensagemErro);

            // Garante que o método de cadastro não foi chamado
            _mockRepo.Verify(r => r.Cadastrar(It.IsAny<Jogo>()), Times.Never);

            _output.WriteLine($"Resultado do teste: {badRequest.Value}");
        }


        [Fact]
        public void CTJO002_Cadastro_de_jogo_com_preco_zero_nulo_ou_negativo()
        {
            var input = new JogoInput
            {
                Nome = "SPACE",
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 0 // inválido
            };

            var result = _controller.Post(input);

            // Espera retorno BadRequest com mensagem
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var mensagemErro = badRequest.Value?.ToString().ToLower();

            // Verifica que o erro é sobre o preço
            Assert.Contains("preço", mensagemErro);

            // Garante que não houve chamada para o repositório
            _mockRepo.Verify(r => r.Cadastrar(It.IsAny<Jogo>()), Times.Never);

            _output.WriteLine($"Resultado do teste: {badRequest.Value}");
        }

        [Fact]
        public void CTJO003_Cadastro_de_jogo_com_ano_lancamento_maior_que_ano_atual()
        {
            var input = new JogoInput
            {
                Nome = "Novo Jogo",
                AnoLancamento = DateTime.Now.Year + 1,  // coloca ano futuro para dar erro 
                PrecoBase = 200m // inválido
            };

            var result = _controller.Post(input);

            // Espera retorno BadRequest com mensagem
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var mensagemErro = badRequest.Value?.ToString().ToLower();

            // Verifica que o erro é sobre ano
            Assert.Contains("ano", mensagemErro);

            // Garante que não houve chamada para o repositório
            _mockRepo.Verify(r => r.Cadastrar(It.IsAny<Jogo>()), Times.Never);

            _output.WriteLine($"Resultado do teste: {badRequest.Value}");
        }

        

        [Fact]
        public void CTJO004_Cadastro_de_jogo_com_dados_validos()
        {
            var input = new JogoInput
            {
                Nome = "Novo Jogo",
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 200m
            };

            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Jogo>()));

            var result = _controller.Post(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Jogo cadastrado", ok.Value.ToString());

            _mockRepo.Verify(r => r.Cadastrar(It.Is<Jogo>(j => j.Nome == input.Nome && j.PrecoBase == input.PrecoBase)), Times.Once);

            _output.WriteLine($"Jogo cadastrado com sucesso: {input.Nome} - Preço: {input.PrecoBase}");
        }







        [Fact]
        public void CTJO005_Excluir_jogo_vinculado_a_pedido()
        {
            int jogoId = 1;
            _mockRepo.Setup(r => r.ObterPorId(jogoId)).Returns(new Jogo { Id = jogoId });
            _mockRepo.Setup(r => r.JogoTemPedidos(jogoId)).Returns(true);

            var result = _controller.Delete(jogoId);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("não permitida", badRequest.Value.ToString().ToLower());

            _mockRepo.Verify(r => r.Deletar(It.IsAny<int>()), Times.Never);

            _output.WriteLine($"Resultado do teste: {badRequest.Value}");
        }



        [Fact]
        public void CTJO006_Excluir_jogo_sem_vinculo_a_pedido()
        {
            int jogoId = 2;
            _mockRepo.Setup(r => r.ObterPorId(jogoId)).Returns(new Jogo { Id = jogoId });
            _mockRepo.Setup(r => r.JogoTemPedidos(jogoId)).Returns(false);
            _mockRepo.Setup(r => r.Deletar(jogoId));

            var result = _controller.Delete(jogoId);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("excluído", ok.Value.ToString().ToLower());

            _mockRepo.Verify(r => r.Deletar(jogoId), Times.Once);

            _output.WriteLine($"Resultado do teste: {ok.Value}");
        }


        [Fact]
        public void CTJO007_Usuario_consultar_lista_de_jogos()
        {
            var jogos = new List<Jogo>
    {
        new Jogo { Id = 1, Nome = "Game A", AnoLancamento = DateTime.Now.Year, PrecoBase = 100m, DataCriacao = DateTime.Now },
        new Jogo { Id = 2, Nome = "Game B", AnoLancamento = DateTime.Now.Year, PrecoBase = 150m, DataCriacao = DateTime.Now }
    };

            _mockRepo.Setup(r => r.ObterTodos()).Returns(jogos);

            var controllerSemAdmin = new JogoController(_mockRepo.Object);

            // ✅ Simula um usuário comum no controller correto
            controllerSemAdmin.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()) // Não é admin
                }
            };

            var result = controllerSemAdmin.Get();

            var ok = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsAssignableFrom<IEnumerable<JogoDtoUsuario>>(ok.Value);

            Assert.Equal(2, lista.Count());

            _output.WriteLine("Jogos retornados ao usuário comum.");
        }

        [Fact]
        public void CTJO008_Usuario_consultar_jogo_por_id()
        {
            // Arrange
            var jogo = new Jogo
            {
                Id = 1,
                Nome = "Jogo Teste",
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 150m,
                DataCriacao = DateTime.Now,
                Pedidos = new List<Pedido>()
            };

            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(jogo);

            // Simula um usuário comum (não admin)
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            // Act
            var result = _controller.Get(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var jogoDto = Assert.IsType<JogoDtoUsuario>(ok.Value);

            Assert.Equal(jogo.Id, jogoDto.Id);
            Assert.Equal(jogo.Nome, jogoDto.Nome);
        }


        [Fact]
        public void CTJO009_Admin_consultar_lista_de_jogos()
        {
            var jogos = new List<Jogo>
    {
        new Jogo { Id = 1, Nome = "Admin Game", AnoLancamento = DateTime.Now.Year, PrecoBase = 199m, DataCriacao = DateTime.Now, Pedidos = new List<Pedido>() }
    };

            _mockRepo.Setup(r => r.ObterTodos()).Returns(jogos);

            var controllerComAdmin = new JogoController(_mockRepo.Object);
            var userMock = new Mock<System.Security.Claims.ClaimsPrincipal>();
            userMock.Setup(x => x.IsInRole("Admin")).Returns(true);

            controllerComAdmin.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            var result = controllerComAdmin.Get();

            var ok = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);

            Assert.Single(lista);

            _output.WriteLine("Jogos retornados para admin.");
        }


        [Fact]
        public void CTJO010_Admin_consultar_jogo_por_id()
        {
            var jogo = new Jogo
            {
                Id = 99,
                Nome = "Admin Test Game",
                AnoLancamento = 2025,
                PrecoBase = 120m,
                DataCriacao = DateTime.Now,
                Pedidos = new List<Pedido>()
            };

            _mockRepo.Setup(r => r.ObterPorId(jogo.Id)).Returns(jogo);

            var controllerComAdmin = new JogoController(_mockRepo.Object);
            var userMock = new Mock<System.Security.Claims.ClaimsPrincipal>();
            userMock.Setup(x => x.IsInRole("Admin")).Returns(true);

            controllerComAdmin.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            var result = controllerComAdmin.Get(jogo.Id);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<JogoDto>(ok.Value);

            Assert.Equal(jogo.Id, dto.Id);

            _output.WriteLine($"Admin recebeu jogo: {dto.Nome}");
        }

    }
}
