using Azure;
using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PromocaoControllerTests
    {
        private readonly Mock<IPromocaoRepository> _mockRepo;
        private readonly PromocaoController _controller;
        private readonly ITestOutputHelper _output;

        public PromocaoControllerTests(ITestOutputHelper output)
        {
            _mockRepo = new Mock<IPromocaoRepository>();
            _controller = new PromocaoController(_mockRepo.Object);
            _output = output;
        }

        [Fact]
        public void CTPR001_CadastroSemNome_DeveRetornarErroNomeInvalido()
        {
            var dto = new PromocaoInput
            {
                Nome = null,
                Desconto = 20,
                DataValidade = DateTime.Today.AddDays(1)
            };

            var result = _controller.Post(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var erro = badRequest.Value?.GetType().GetProperty("Error")?.GetValue(badRequest.Value, null) as string;

            Assert.Equal("Nome é obrigatório.", erro);
        }

        [Fact]
        public void CTPR002_CadastroNomeDuplicado_DeveRetornarErroNomeDuplicado()
        {
            var dto = new PromocaoInput
            {
                Nome = "Black Friday",
                Desconto = 20,
                DataValidade = DateTime.Today.AddDays(1)
            };

            _mockRepo.Setup(r => r.ObterPorNome(dto.Nome)).Returns(new Promocao(dto.Nome, dto.Desconto, dto.DataValidade));

            var result = _controller.Post(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var erro = badRequest.Value?.GetType().GetProperty("Error")?.GetValue(badRequest.Value, null) as string;

            Assert.Equal("Erro de nome de promoção duplicado.", erro);
        }

        [Fact]
        public void CTPR003_DescontoForaDaFaixa_DeveRetornarErroDesconto()
        {
            var dto = new PromocaoInput
            {
                Nome = "Promo Teste",
                Desconto = 5, // fora da faixa permitida
                DataValidade = DateTime.Today.AddDays(1)
            };

            var result = _controller.Post(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var erro = badRequest.Value?.GetType().GetProperty("Error")?.GetValue(badRequest.Value, null) as string;

            Assert.Equal("Desconto deve estar entre 10% e 90%.", erro);
        }

        [Fact]
        public void CTPR004_DataValidadeAnterior_DeveRetornarErroDataValidade()
        {
            var dto = new PromocaoInput
            {
                Nome = "Promo Teste",
                Desconto = 20,
                DataValidade = DateTime.Today.AddDays(-1)
            };

            var result = _controller.Post(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var erro = badRequest.Value?.GetType().GetProperty("Error")?.GetValue(badRequest.Value, null) as string;

            Assert.Equal("Data de validade deve ser posterior à data atual.", erro);

        }

        [Fact]
        public void CTPR005_CadastroValido_DeveRetornarSucesso()
        {
            var input = new PromocaoInput
            {
                Nome = "PROMOÇÃO KIDS",
                Desconto = 15,
                DataValidade = new DateTime(2025, 9, 30)
            };

            _mockRepo.Setup(r => r.ObterPorNome(input.Nome)).Returns((Promocao)null);
            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Promocao>()));

            var result = _controller.Post(input);
            var ok = Assert.IsType<OkObjectResult>(result);

            var mensagem = ok.Value?.GetType().GetProperty("Message")?.GetValue(ok.Value, null) as string;
            Assert.Equal("Promoção cadastrada.", mensagem);

            _mockRepo.Verify(r => r.Cadastrar(It.Is<Promocao>(
                p => p.Nome == input.Nome && p.Desconto == input.Desconto
            )), Times.Once);

            _output.WriteLine($"Promoção cadastrada com sucesso: {input.Nome} - Desconto: {input.Desconto}%");
        }

    }
}
