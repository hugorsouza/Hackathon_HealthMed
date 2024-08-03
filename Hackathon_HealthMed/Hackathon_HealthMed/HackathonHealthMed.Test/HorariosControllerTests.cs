using Hackathon_HealthMed.Controllers;
using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Application.DTO;
using FluentAssertions;

namespace HackathonHealthMed.Test
{
    public class HorariosControllerTests
    {
        private readonly Mock<ILoginService> _mockLoginService;
        private readonly Mock<IHorarioDisponivelService> _mockHorarioDisponivelService;
        private readonly HorarioDisponivelController _controller;

        public HorariosControllerTests()
        {
            _mockLoginService = new Mock<ILoginService>();
            _mockHorarioDisponivelService = new Mock<IHorarioDisponivelService>();
            _controller = new HorarioDisponivelController(_mockHorarioDisponivelService.Object, _mockLoginService.Object);
        }

        [Fact]
        public async Task ObterHorariosPorMedico_UsuarioPaciente_DeveRetornarOkComHorarios()
        {
            // Arrange
            int medicoId = 1;
            string token = "valid-token";
            var user = new Usuario { perfil = EPerfil.Paciente };
            var horarios = new List<HorarioDisponivel>
        {
            new HorarioDisponivel
            {
                Id = 1,
                MedicoId = medicoId,
                DataHoraInicio = new DateTime(2024, 8, 1, 9, 0, 0),
                DataHoraFim = new DateTime(2024, 8, 1, 9, 30, 0),
                Horario = new DateTime(2024, 8, 1, 9, 0, 0),
                Nome = "Consulta 1",
                PacienteId = 101
            }
        };

            // Configuração do mock para o login
            _mockLoginService.Setup(s => s.IdentityUserAsync(token)).ReturnsAsync(user);

            // Configuração do mock para o serviço de horários
            _mockHorarioDisponivelService.Setup(s => s.ObterHorariosPorMedicoAsync(It.IsAny<int>())) .ReturnsAsync(horarios); // Correção: Passar a lista 'horarios'

            // Act
            var result = await _controller.ObterHorariosPorMedico(medicoId, token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsAssignableFrom<IEnumerable<HorarioDisponivel>>(okResult.Value);
            Assert.Equal(horarios, resultValue); // Verifica se o valor retornado é igual ao esperado
        }


        [Fact]
        public async Task ObterHorariosPorMedico_UsuarioNaoPaciente_DeveRetornarUnauthorized()
        {
            // Arrange
            int medicoId = 1;
            string token = "valid-token";
            var user = new Usuario { perfil = EPerfil.Medico };

            _mockLoginService.Setup(s => s.IdentityUserAsync(token)).ReturnsAsync(user);

            // Act
            var result = await _controller.ObterHorariosPorMedico(medicoId, token);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task ObterHorariosPorMedico_TokenInvalido_DeveRetornarUnauthorized()
        {
            // Arrange
            int medicoId = 9;
            string token = "Invalid-token";
            _mockLoginService.Setup(s => s.IdentityUserAsync(token)).ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.ObterHorariosPorMedico(medicoId, token);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task AtualizarHorario_UserValido_HorarioAtualizadoComSucesso()
        {
            // Arrange
            var horarioDisponivelDto = new HorarioDisponivelDto
            {
                // Inicialize conforme necessário
            };
            var token = "valid_token";
            var user = new Usuario
            {
                perfil = EPerfil.Medico
            };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.AtualizarHorarioAsync(horarioDisponivelDto))
                                          .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AtualizarHorario(horarioDisponivelDto, token);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task AtualizarHorario_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var horarioDisponivelDto = new HorarioDisponivelDto
            {
                // Inicialize conforme necessário
            };
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.AtualizarHorario(horarioDisponivelDto, token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task AtualizarHorario_HorarioNaoEncontrado_NotFound()
        {
            // Arrange
            var horarioDisponivelDto = new HorarioDisponivelDto
            {
                // Inicialize conforme necessário
            };
            var token = "valid_token";
            var user = new Usuario
            {
                perfil = EPerfil.Medico
            };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.AtualizarHorarioAsync(horarioDisponivelDto))
                                          .ThrowsAsync(new KeyNotFoundException("Horário não encontrado."));

            // Act
            var result = await _controller.AtualizarHorario(horarioDisponivelDto, token);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Horário não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public async Task AdicionarHorario_UserValido_HorarioAdicionadoComSucesso()
        {
            // Arrange
            var horarioDisponivelDto = new HorarioDisponivelDto
            {
                // Inicialize conforme necessário
            };
            var token = "valid_token";
            var user = new Usuario
            {
                perfil = EPerfil.Medico
            };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.AdicionarHorarioAsync(horarioDisponivelDto))
                                          .ReturnsAsync(true);

            // Act
            var result = await _controller.AdicionarHorario(horarioDisponivelDto, token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().Be("Horário adicionado!");
        }

        [Fact]
        public async Task AdicionarHorario_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var horarioDisponivelDto = new HorarioDisponivelDto
            {
                // Inicialize conforme necessário
            };
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.AdicionarHorario(horarioDisponivelDto, token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task AdicionarHorario_HorarioJaOcupado_Conflict()
        {
            // Arrange
            var horarioDisponivelDto = new HorarioDisponivelDto
            {
                // Inicialize conforme necessário
            };
            var token = "valid_token";
            var user = new Usuario
            {
                perfil = EPerfil.Medico
            };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.AdicionarHorarioAsync(horarioDisponivelDto))
                                          .ReturnsAsync(false);

            // Act
            var result = await _controller.AdicionarHorario(horarioDisponivelDto, token);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            conflictResult.Value.Should().Be("Horário já está ocupado.");
        }
        
        [Fact]
        public async Task ObterHorariosPorNomeMedico_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var nome = "Dr. Smith";
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.ObterHorariosPorNomeMedico(nome, token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task ObterMedicosComHorariosDisponiveis_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.ObterMedicosComHorariosDisponiveis(token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        
        [Fact]
        public async Task DeletarHorario_UserValido_HorarioDeletado()
        {
            // Arrange
            var id = 1;
            var token = "valid_token";
            var user = new Usuario { perfil = EPerfil.Medico };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.DeletarHorarioAsync(id))
                                          .ReturnsAsync(true);

            // Act
            var result = await _controller.DeletarHorario(id, token);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeletarHorario_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var id = 1;
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.DeletarHorario(id, token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task DeletarHorario_HorarioNaoEncontrado_NotFound()
        {
            // Arrange
            var id = 1;
            var token = "valid_token";
            var user = new Usuario { perfil = EPerfil.Medico };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.DeletarHorarioAsync(id))
                                          .ReturnsAsync(false);

            // Act
            var result = await _controller.DeletarHorario(id, token);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

       
        [Fact]
        public async Task AgendarConsulta_UserValido_ConsultaAgendada()
        {
            // Arrange
            var consultaDto = new HorarioDisponivelDto { /* Inicialize conforme necessário */ };
            var token = "valid_token";
            var user = new Usuario { perfil = EPerfil.Paciente };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.AgendarConsultaAsync(consultaDto))
                                          .ReturnsAsync(true);

            // Act
            var result = await _controller.AgendarConsulta(consultaDto, token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().Be("Consulta agendada com sucesso.");
        }

        [Fact]
        public async Task AgendarConsulta_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var consultaDto = new HorarioDisponivelDto { /* Inicialize conforme necessário */ };
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.AgendarConsulta(consultaDto, token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task AgendarConsulta_HorarioNaoDisponivel_BadRequest()
        {
            // Arrange
            var consultaDto = new HorarioDisponivelDto { /* Inicialize conforme necessário */ };
            var token = "valid_token";
            var user = new Usuario { perfil = EPerfil.Paciente };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.AgendarConsultaAsync(consultaDto))
                                          .ReturnsAsync(false);

            // Act
            var result = await _controller.AgendarConsulta(consultaDto, token);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Horário não disponível.");
        }

        [Fact]
        public async Task DesmarcarConsulta_UserValido_ConsultaDesmarcada()
        {
            // Arrange
            var id = 1;
            var pacienteId = 123;
            var token = "valid_token";
            var user = new Usuario { perfil = EPerfil.Paciente };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.DesmarcarConsultaAsync(id, pacienteId))
                                          .ReturnsAsync(true);

            // Act
            var result = await _controller.DesmarcarConsulta(id, pacienteId, token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().Be("Cancelamento realizado com sucesso.");
        }

        [Fact]
        public async Task DesmarcarConsulta_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var id = 1;
            var pacienteId = 123;
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.DesmarcarConsulta(id, pacienteId, token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task DesmarcarConsulta_ConsultaNaoLocalizada_BadRequest()
        {
            // Arrange
            var id = 1;
            var pacienteId = 123;
            var token = "valid_token";
            var user = new Usuario { perfil = EPerfil.Paciente };

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync(user);
            _mockHorarioDisponivelService.Setup(service => service.DesmarcarConsultaAsync(id, pacienteId))
                                          .ReturnsAsync(false);

            // Act
            var result = await _controller.DesmarcarConsulta(id, pacienteId, token);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Consulta não localizada não disponível.");
        }

        [Fact]
        public async Task ExibirConsultas_UsuarioInvalido_Unauthorized()
        {
            // Arrange
            var pacienteId = 123;
            var token = "invalid_token";

            _mockLoginService.Setup(service => service.IdentityUserAsync(token))
                             .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.ExibirConsultas(pacienteId, token);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}

