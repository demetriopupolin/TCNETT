using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
        public void CTJO004_Cadastro_de_jogo_com_ano_lancamento_menor_que_ano_da_data_de_criacao()
        {
            var input = new JogoInput
            {
                Nome = "Novo Jogo",
                AnoLancamento = DateTime.Now.Year - 1, // Ano anterior ao atual
                PrecoBase = 200m
            };

            var result = _controller.Post(input);

            // Espera retorno BadRequest com mensagem
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var mensagemErro = badRequest.Value?.ToString().ToLower();

            // Verifica que o erro é sobre o ano
            Assert.Contains("ano", mensagemErro);

            // Garante que o método de cadastro não foi chamado
            _mockRepo.Verify(r => r.Cadastrar(It.IsAny<Jogo>()), Times.Never);

            _output.WriteLine($"Resultado do teste: {badRequest.Value}");
        }



        [Fact]
        public void CTJO005_Cadastro_de_jogo_com_dados_validos()
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

    }
}
