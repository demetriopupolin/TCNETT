using Xunit;
using Xunit.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;

namespace Tests
{
    public class PromocaoControllerTests
    {
        private readonly Mock<IPromocaoRepository> _mockRepo;
        private readonly PromocaoController _controller;
        private readonly ITestOutputHelper _output;

        public PromocaoControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _mockRepo = new Mock<IPromocaoRepository>();
            _controller = new PromocaoController(_mockRepo.Object);
        }

        [Fact]
        public void Get_DeveRetornarListaDePromocoes()
        {
            var promocoes = new List<Promocao>
            {
                new Promocao
                {
                    Id = 1,
                    Nome = "Promoção 1",
                    Desconto = 10,
                    DataValidade = DateTime.Now.AddDays(10),
                    DataCriacao = DateTime.Now,
                    Pedidos = new List<Pedido>()
                }
            };

            _mockRepo.Setup(r => r.ObterTodos()).Returns(promocoes);

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsType<List<PromocaoDto>>(okResult.Value);

            Assert.Single(lista);
            Assert.Equal("Promoção 1", lista[0].Nome);

            _output.WriteLine($"Total promoções retornadas: {lista.Count}");
        }

        [Fact]
        public void Get_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.ObterTodos()).Throws(new Exception("Erro inesperado"));

            var result = _controller.Get();

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao obter todas as promoções", badRequest.Value.ToString());

            _output.WriteLine($"Erro retornado: {badRequest.Value}");
        }

        [Fact]
        public void GetPorId_DeveRetornarPromocao_QuandoEncontrar()
        {
            var promocao = new Promocao
            {
                Id = 1,
                Nome = "Promoção Teste",
                Desconto = 20,
                DataValidade = DateTime.Now.AddDays(5),
                DataCriacao = DateTime.Now,
                Pedidos = new List<Pedido>()
            };

            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(promocao);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PromocaoDto>(okResult.Value);

            Assert.Equal("Promoção Teste", dto.Nome);

            _output.WriteLine($"Promoção retornada: {dto.Id} - {dto.Nome} - Desconto: {dto.Desconto}");
        }

        [Fact]
        public void GetPorId_DeveRetornarNotFound_QuandoNaoEncontrar()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns((Promocao)null);

            var result = _controller.Get(999);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Promoção não encontrada.", notFound.Value);

            _output.WriteLine("Promoção não encontrada para o ID 999.");
        }

        [Fact]
        public void GetPorId_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Throws(new Exception("Erro inesperado"));

            var result = _controller.Get(1);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao obter o Promoção", badRequest.Value.ToString());

            _output.WriteLine($"Erro retornado: {badRequest.Value}");
        }

        [Fact]
        public void Post_DeveCadastrarPromocao_ComSucesso()
        {
            var input = new PromocaoInput
            {
                Nome = "Nova Promoção",
                Desconto = 25,
                DataValidade = DateTime.Now.AddDays(15)
            };

            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Promocao>()));

            var result = _controller.Post(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Promoção cadastrada", ok.Value.ToString());

            _mockRepo.Verify(r => r.Cadastrar(It.Is<Promocao>(p => p.Nome == input.Nome && p.Desconto == input.Desconto)), Times.Once);

            _output.WriteLine($"Promoção cadastrada: {input.Nome} com desconto {input.Desconto}%");
        }

        [Fact]
        public void Post_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Promocao>())).Throws(new Exception("Erro no cadastro"));

            var input = new PromocaoInput
            {
                Nome = "Promoção Erro",
                Desconto = 30,
                DataValidade = DateTime.Now.AddDays(10)
            };

            var result = _controller.Post(input);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao inserir promoção", badRequest.Value.ToString());

            _output.WriteLine($"Erro no cadastro da promoção: {badRequest.Value}");
        }

        [Fact]
        public void Put_DeveAlterarPromocao_QuandoExistir()
        {
            var promocaoExistente = new Promocao
            {
                Id = 1,
                Nome = "Promoção Existente",
                Desconto = 10,
                DataValidade = DateTime.Now.AddDays(5)
            };

            var input = new PromocaoUpdateInput
            {
                Id = 1,
                Desconto = 15,
                DataValidade = DateTime.Now.AddDays(20)
            };

            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(promocaoExistente);
            _mockRepo.Setup(r => r.Alterar(It.IsAny<Promocao>()));

            var result = _controller.Put(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Promoção alterada", ok.Value.ToString());

            _mockRepo.Verify(r => r.Alterar(It.Is<Promocao>(p => p.Id == input.Id && p.Desconto == input.Desconto)), Times.Once);

            _output.WriteLine($"Promoção alterada: ID {input.Id} com desconto {input.Desconto}%");
        }

        [Fact]
        public void Put_DeveRetornarNotFound_QuandoNaoExistir()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns((Promocao)null);

            var input = new PromocaoUpdateInput
            {
                Id = 999,
                Desconto = 50,
                DataValidade = DateTime.Now.AddDays(30)
            };

            var result = _controller.Put(input);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Promoção não encontrada.", notFound.Value);

            _output.WriteLine("Tentativa de alteração de promoção inexistente (ID 999).");
        }

        [Fact]
        public void Put_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            var promocaoExistente = new Promocao
            {
                Id = 1,
                Nome = "Promoção Existente",
                Desconto = 10,
                DataValidade = DateTime.Now.AddDays(5)
            };

            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(promocaoExistente);
            _mockRepo.Setup(r => r.Alterar(It.IsAny<Promocao>())).Throws(new Exception("Erro na alteração"));

            var input = new PromocaoUpdateInput
            {
                Id = 1,
                Desconto = 20,
                DataValidade = DateTime.Now.AddDays(25)
            };

            var result = _controller.Put(input);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao alterar promoção", badRequest.Value.ToString());

            _output.WriteLine($"Erro ao alterar promoção: {badRequest.Value}");
        }

        [Fact]
        public void Delete_DeveExcluirPromocao_QuandoNaoTiverPedidos()
        {
            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(new Promocao { Id = 1, Nome = "Promocao Nome", DataValidade = DateTime.Now.AddDays(365), Desconto = 10  });
            _mockRepo.Setup(r => r.PromocaoTemPedidos(1)).Returns(false);
            _mockRepo.Setup(r => r.Deletar(1));

            var result = _controller.Delete(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Promoção excluída", ok.Value.ToString());

            _mockRepo.Verify(r => r.Deletar(1), Times.Once);

            _output.WriteLine("Promoção excluída com sucesso: ID 1");
        }

        [Fact]
        public void Delete_DeveRetornarBadRequest_QuandoTiverPedidos()
        {
            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(new Promocao { Id = 1, Nome = "Promocao Nome", DataValidade = DateTime.Now.AddDays(365), Desconto = 10 });
            _mockRepo.Setup(r => r.PromocaoTemPedidos(1)).Returns(true);

            var result = _controller.Delete(1);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Promoção possui pedidos. Exclusão não permitida.", badRequest.Value);

            _output.WriteLine("Tentativa de exclusão de promoção com pedidos (ID 1).");
        }

        [Fact]
        public void Delete_DeveRetornarNotFound_QuandoNaoExistir()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns((Promocao)null);

            var result = _controller.Delete(999);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Promoção inexistente.", notFound.Value);

            _output.WriteLine("Tentativa de exclusão de promoção inexistente (ID 999).");
        }

        [Fact]
        public void Delete_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(new Promocao { Id = 1 , Nome = "Promocao Nome" , DataValidade = DateTime.Now.AddDays(365), Desconto = 10 });
            _mockRepo.Setup(r => r.PromocaoTemPedidos(1)).Returns(false);
            _mockRepo.Setup(r => r.Deletar(1)).Throws(new Exception("Erro no delete"));

            var result = _controller.Delete(1);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao deletar promoção", badRequest.Value.ToString());

            _output.WriteLine($"Erro ao deletar promoção: {badRequest.Value}");
        }

        [Fact]
        public void CadastroEmMassa_DeveCadastrarPromocoes_ComSucesso()
        {
            _mockRepo.Setup(r => r.CadastrarEmMassa(It.IsAny<List<Promocao>>()));

            var result = _controller.CadastroEmMassa();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Promoções cadastradas em massa", ok.Value.ToString());

            _mockRepo.Verify(r => r.CadastrarEmMassa(It.IsAny<List<Promocao>>()), Times.Once);

            _output.WriteLine("Promoções cadastradas em massa com sucesso.");
        }

        [Fact]
        public void CadastroEmMassa_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.CadastrarEmMassa(It.IsAny<List<Promocao>>())).Throws(new Exception("Erro na massa"));

            var result = _controller.CadastroEmMassa();

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro na massa", badRequest.Value.ToString());

            _output.WriteLine($"Erro no cadastro em massa: {badRequest.Value}");
        }
    }
}
