using Xunit;
using Xunit.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Input;
using Core.Repository;
using FiapCloudGamesApi.Controllers;

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
        public void Get_DeveRetornarListaDeJogos()
        {
            _output.WriteLine("Testando Get - sucesso");

            var jogos = new List<Jogo>
            {
                new Jogo
                {
                    Id = 1,
                    Nome = "Jogo 1",
                    AnoLancamento = DateTime.Now.Year,
                    DataCriacao = DateTime.Now,
                    PrecoBase = 100m,
                    Pedidos = new List<Pedido>()
                }
            };

            _mockRepo.Setup(r => r.ObterTodos()).Returns(jogos);

            var result = _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsType<List<JogoDto>>(okResult.Value);

            Assert.Single(lista);
            Assert.Equal("Jogo 1", lista[0].Nome);

            _output.WriteLine($"Total jogos retornados: {lista.Count}");
            foreach (var j in lista)
                _output.WriteLine($"Jogo: {j.Id} - {j.Nome} - Preço: {j.PrecoBase}");
        }

        [Fact]
        public void Get_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.ObterTodos()).Throws(new Exception("Erro inesperado"));

            var result = _controller.Get();

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao obter todos os jogos", badRequest.Value.ToString());

            _output.WriteLine($"Erro retornado: {badRequest.Value}");
        }

        [Fact]
        public void GetPorId_DeveRetornarJogo_QuandoEncontrar()
        {
            var jogo = new Jogo
            {
                Id = 1,
                Nome = "Jogo Teste",
                AnoLancamento = DateTime.Now.Year,
                DataCriacao = DateTime.Now,
                PrecoBase = 150m,
                Pedidos = new List<Pedido>()
            };

            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(jogo);

            var result = _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<JogoDto>(okResult.Value);

            Assert.Equal("Jogo Teste", dto.Nome);

            _output.WriteLine($"Jogo retornado: {dto.Id} - {dto.Nome} - Preço: {dto.PrecoBase}");
        }

        [Fact]
        public void GetPorId_DeveRetornarNotFound_QuandoNaoEncontrar()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns((Jogo)null);

            var result = _controller.Get(999);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Jogo não encontrado.", notFound.Value);

            _output.WriteLine("Jogo não encontrado para o ID 999.");
        }

        [Fact]
        public void GetPorId_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Throws(new Exception("Erro inesperado"));

            var result = _controller.Get(1);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao obter o jogo", badRequest.Value.ToString());

            _output.WriteLine($"Erro retornado: {badRequest.Value}");
        }

        [Fact]
        public void Post_DeveCadastrarJogo_ComSucesso()
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
        public void Post_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.Cadastrar(It.IsAny<Jogo>())).Throws(new Exception("Erro no cadastro"));

            var input = new JogoInput
            {
                Nome = "Erro Jogo",
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 300m
            };

            var result = _controller.Post(input);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao inserir jogo", badRequest.Value.ToString());

            _output.WriteLine($"Falha no cadastro do jogo: {input.Nome}, erro: {badRequest.Value}");
        }

        [Fact]
        public void Put_DeveAlterarJogo_QuandoExistir()
        {
            var jogoExistente = new Jogo
            {
                Id = 1,
                Nome = "Jogo Existente",
                AnoLancamento = DateTime.Now.Year,
                DataCriacao = DateTime.Now,
                PrecoBase = 150m
            };

            var input = new JogoUpdateInput
            {
                Id = 1,
                Nome = "Jogo Alterado",
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 180m
            };

            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(jogoExistente);
            _mockRepo.Setup(r => r.Alterar(It.IsAny<Jogo>()));

            var result = _controller.Put(input);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Jogo alterado", ok.Value.ToString());

            _mockRepo.Verify(r => r.Alterar(It.Is<Jogo>(j => j.Nome == input.Nome && j.PrecoBase == input.PrecoBase)), Times.Once);

            _output.WriteLine($"Jogo alterado com sucesso: ID {input.Id} - {input.Nome} - Preço: {input.PrecoBase}");
        }

        [Fact]
        public void Put_DeveRetornarNotFound_QuandoNaoExistir()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns((Jogo)null);

            var input = new JogoUpdateInput
            {
                Id = 999,
                Nome = "Jogo Fake",
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 100m
            };

            var result = _controller.Put(input);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Jogo não encontrado.", notFound.Value);

            _output.WriteLine("Tentativa de alteração de jogo inexistente (ID 999).");
        }

        [Fact]
        public void Put_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            var jogoExistente = new Jogo
            {
                Id = 1,
                Nome = "Jogo Existente",
                AnoLancamento = DateTime.Now.Year,
                DataCriacao = DateTime.Now,
                PrecoBase = 150m
            };

            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(jogoExistente);
            _mockRepo.Setup(r => r.Alterar(It.IsAny<Jogo>())).Throws(new Exception("Erro na alteração"));

            var input = new JogoUpdateInput
            {
                Id = 1,
                Nome = "Jogo Alterado",
                AnoLancamento = DateTime.Now.Year,
                PrecoBase = 180m
            };

            var result = _controller.Put(input);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao alterar jogo", badRequest.Value.ToString());

            _output.WriteLine($"Erro ao alterar jogo: {badRequest.Value}");
        }

        [Fact]
        public void Delete_DeveExcluirJogo_QuandoNaoTiverPedidos()
        {
            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(new Jogo
            {
                Id = 1,
                AnoLancamento = DateTime.Now.Year,
                DataCriacao = DateTime.Now
            });
            _mockRepo.Setup(r => r.JogoTemPedidos(1)).Returns(false);
            _mockRepo.Setup(r => r.Deletar(1));

            var result = _controller.Delete(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Jogo excluído", ok.Value.ToString());

            _mockRepo.Verify(r => r.Deletar(1), Times.Once);

            _output.WriteLine("Jogo excluído com sucesso: ID 1");
        }

        [Fact]
        public void Delete_DeveRetornarBadRequest_QuandoTiverPedidos()
        {
            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(new Jogo { Id = 1 });
            _mockRepo.Setup(r => r.JogoTemPedidos(1)).Returns(true);

            var result = _controller.Delete(1);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Jogo possui pedidos. Exclusão não permitida.", badRequest.Value);

            _output.WriteLine("Tentativa de exclusão de jogo com pedidos (ID 1).");
        }

        [Fact]
        public void Delete_DeveRetornarNotFound_QuandoNaoExistir()
        {
            _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns((Jogo)null);

            var result = _controller.Delete(999);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Jogo inexistente.", notFound.Value);

            _output.WriteLine("Tentativa de exclusão de jogo inexistente (ID 999).");
        }

        [Fact]
        public void Delete_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.ObterPorId(1)).Returns(new Jogo
            {
                Id = 1,
                AnoLancamento = DateTime.Now.Year,
                DataCriacao = DateTime.Now
            });
            _mockRepo.Setup(r => r.JogoTemPedidos(1)).Returns(false);
            _mockRepo.Setup(r => r.Deletar(1)).Throws(new Exception("Erro no delete"));

            var result = _controller.Delete(1);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao deletar jogo", badRequest.Value.ToString());

            _output.WriteLine($"Erro ao deletar jogo: {badRequest.Value}");
        }

        [Fact]
        public void CadastroEmMassa_DeveCadastrarJogos_ComSucesso()
        {
            _mockRepo.Setup(r => r.CadastrarEmMassa(It.IsAny<List<Jogo>>()));

            var result = _controller.CadastroEmMassa();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Jogos cadastrados em massa", ok.Value.ToString());

            _mockRepo.Verify(r => r.CadastrarEmMassa(It.IsAny<List<Jogo>>()), Times.Once);

            _output.WriteLine("Jogos cadastrados em massa com sucesso.");
        }

        [Fact]
        public void CadastroEmMassa_DeveRetornarBadRequest_QuandoOcorreErro()
        {
            _mockRepo.Setup(r => r.CadastrarEmMassa(It.IsAny<List<Jogo>>())).Throws(new Exception("Erro na massa"));

            var result = _controller.CadastroEmMassa();

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Erro ao cadastrar jogos em massa", badRequest.Value.ToString());

            _output.WriteLine($"Erro no cadastro em massa: {badRequest.Value}");
        }
    }
}
