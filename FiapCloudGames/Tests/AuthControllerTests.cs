using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Core.Input;
using Core.Entity;
using Core.Repository;
using FiapCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace Tests
{
    public class AuthControllerTests
    {
        private readonly ITestOutputHelper _output;

        public AuthControllerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Login_DeveRetornarToken_QuandoCredenciaisForemValidas()
        {
            // Arrange
            var email = "teste@email.com";
            var senha = "123456@A";
            var nome = "Teste";
            var mockRepo = new Mock<IUsuarioRepository>();
            mockRepo.Setup(r => r.ObterPorEmail(email)).Returns(new Usuario
            {
                Id = 1,
                Nome = nome,
                Email = email,
                Senha = senha,
                Nivel = 'A'
            });

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Jwt:Key"]).Returns("minha-chave-super-secretaaaaaaaaaaaaaaaaaaaaaaaaaa-1234567890abcd");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("FiapIssuer");

            var controller = new AuthController(mockConfig.Object, mockRepo.Object);

            var loginDto = new LoginDto
            {
                Email = email,
                Senha = senha
            };

            // Act
            var resultado = controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);
            _output.WriteLine($"Token gerado: {okResult.Value}");
        }

        [Fact]
        public void Login_DeveRetornarUnauthorized_QuandoSenhaForInvalida()
        {
            // Arrange
            var email = "teste@email.com";
            var senhaCorreta = "123456@A";
            var senhaIncorreta = "senhaErrada";
            var mockRepo = new Mock<IUsuarioRepository>();
            mockRepo.Setup(r => r.ObterPorEmail(email)).Returns(new Usuario
            {
                Id = 1,
                Nome = "Teste",
                Email = email,
                Senha = senhaCorreta,
                Nivel = 'A'
            });

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Jwt:Key"]).Returns("minha-chave-super-secretaaaaaaaaaaaaaaaaaaaaaaaaaa-1234567890abcd");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("FiapIssuer");

            var controller = new AuthController(mockConfig.Object, mockRepo.Object);

            var loginDto = new LoginDto
            {
                Email = email,
                Senha = senhaIncorreta
            };

            // Act
            var resultado = controller.Login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(resultado);
            Assert.Equal("Senha incorreta.", unauthorizedResult.Value);
            _output.WriteLine($"Mensagem retornada: {unauthorizedResult.Value}");
        }

        [Fact]
        public void Login_DeveRetornarUnauthorized_QuandoUsuarioNaoExistir()
        {
            // Arrange
            var email = "inexistente@email.com";
            var senha = "qualquerSenha";
            var mockRepo = new Mock<IUsuarioRepository>();
            mockRepo.Setup(r => r.ObterPorEmail(email)).Returns((Usuario)null);

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Jwt:Key"]).Returns("minha-chave-super-secretaaaaaaaaaaaaaaaaaaaaaaaaaa-1234567890abcd");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("FiapIssuer");

            var controller = new AuthController(mockConfig.Object, mockRepo.Object);

            var loginDto = new LoginDto
            {
                Email = email,
                Senha = senha
            };

            // Act
            var resultado = controller.Login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(resultado);
            Assert.Equal($"Usuário {email} não encontrado.", unauthorizedResult.Value);
            _output.WriteLine($"Mensagem retornada: {unauthorizedResult.Value}");
        }
    }
}
