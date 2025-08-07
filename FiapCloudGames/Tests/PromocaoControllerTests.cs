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


        [Fact]
        public void CTPR006_ExcluirPromocao_VinculadaPedido_NaoDevePermitir()
        {
            // Arrange
            int promocaoId = 10;

            var promocao = new Promocao("Promo Teste", 20, DateTime.Now.AddDays(10));
            promocao.Id = promocaoId;

            _mockRepo.Setup(r => r.ObterPorId(promocaoId)).Returns(promocao);
            // Simula que existe pedido vinculado à promoção
            _mockRepo.Setup(r => r.PromocaoTemPedidos(promocaoId)).Returns(true);

            // Act
            var result = _controller.Delete(promocaoId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var message = badRequest.Value as string;
            Assert.Equal("Promoção possui pedidos. Exclusão não permitida.", message);

        }


        [Fact]
        public void CTPR007_ExcluirPromocao_SemVinculo_DevePermitir()
        {
            int promoId = 11;

            // Setup para simular promoção existente
            _mockRepo.Setup(r => r.ObterPorId(promoId)).Returns(new Promocao("Promo Livre", 15, DateTime.Now.AddDays(15)) { Id = promoId });

            // Simula que não existe pedido vinculado a essa promoção
            _mockRepo.Setup(r => r.PromocaoTemPedidos(promoId)).Returns(false);

            // Setup para exclusão, apenas para verificação
            _mockRepo.Setup(r => r.Deletar(promoId)).Verifiable();

            var result = _controller.Delete(promoId);

            var ok = Assert.IsType<OkObjectResult>(result);
            var msg = ok.Value?.GetType().GetProperty("Message")?.GetValue(ok.Value, null) as string;

            Assert.Equal("Promoção excluída.", msg);

            _mockRepo.Verify(r => r.Deletar(promoId), Times.Once);
        }
        [Fact]
        public void CTPR008_ConsultarListaPromocoes_DeveRetornarListaComItensEsperados()
        {
            // Arrange
            var promocoes = new List<Promocao>
    {
        new Promocao("Promo1", 10, DateTime.Now.AddDays(10)) { Id = 1 },
        new Promocao("Promo2", 20, DateTime.Now.AddDays(20)) { Id = 2 },
    };

            _mockRepo.Setup(r => r.ObterTodos()).Returns(promocoes);

            // Act
            var resultado = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);

            var lista = Assert.IsAssignableFrom<IEnumerable<PromocaoDto>>(okResult.Value);
            Assert.NotEmpty(lista);
            Assert.Equal(2, lista.Count());

            var listaDto = lista.ToList();

            // Validação detalhada dos dados retornados
            Assert.Equal(1, listaDto[0].Id);
            Assert.Equal("Promo1", listaDto[0].Nome);
            Assert.Equal(10, listaDto[0].Desconto);

            Assert.Equal(2, listaDto[1].Id);
            Assert.Equal("Promo2", listaDto[1].Nome);
            Assert.Equal(20, listaDto[1].Desconto);
        }


        [Fact]
        public void CTPR009_ConsultarPromocaoPorId_DeveRetornarPromocao()
        {
            // Arrange
            int promoId = 1;
            var promocao = new Promocao("Promo1", 10, DateTime.Now.AddDays(10)) { Id = promoId };

            _mockRepo.Setup(r => r.ObterPorId(promoId)).Returns(promocao);

            // Act
            var resultado = _controller.Get(promoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);

            // Se o controller retorna um DTO, use PromocaoDto; senão, mantenha como Promocao
            var retorno = Assert.IsType<PromocaoDto>(okResult.Value);

            Assert.Equal(promoId, retorno.Id);
            Assert.Equal("Promo1", retorno.Nome);
            Assert.Equal(10, retorno.Desconto); // ajuste se for necessário
        }



    }
}
