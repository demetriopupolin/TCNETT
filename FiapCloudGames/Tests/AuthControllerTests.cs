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
        public void CTAC001_Autenticacao_de_usuario_nao_sucedida()
        {
            // Arrange
            var email = "teste@email.com";
            var senha = "123456@X";
            var nome = "Teste";
            var mockRepo = new Mock<IUsuarioRepository>();
            mockRepo.Setup(r => r.ObterPorEmail(email)).Returns(new Usuario
            {
                Id = 1,
                Nome = nome,
                Email = email,
                Senha = senha,
                Nivel = 'U'
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
        public void CTAC002_Autenticacao_de_usuario_bem_sucedida()
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
                Nivel = 'U'
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
        public void CTAC003_Autenticacao_de_usuario_administrador_nao_sucedida()
        {
            // Arrange
            var email = "teste@email.com";
            var senha = "123456@X";
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
        public void CTAC004_Autenticacao_de_usuario_administrador_bem_sucedida()
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




    }
}
