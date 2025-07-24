namespace INDT.Viagens.Test;

using INDT.Viagens.Application.CommandHandler.ResultadoMelhorRotaCommand;
using INDT.Viagens.Application.CommandHandler.ResultadoMelhorRotaHandler;
using INDT.Viagens.Application.Interfaces;
using INDT.Viagens.Domain;
using NSubstitute;
using Xunit;
public class ResultadoMelhorRotaHandlerTests
{
    private readonly IRotaRepository _rotaRepository;
    private readonly ResultadoMelhorRotaHandler _handler;

    public ResultadoMelhorRotaHandlerTests()
    {
        _rotaRepository = Substitute.For<IRotaRepository>();
        _handler = new ResultadoMelhorRotaHandler(_rotaRepository);
    }

    [Fact]
    public async Task ExecutarHandle_DeveChamarLoadRotas_QuandoExecutado()
    {
        // Arrange
        var command = new ResultadoMelhorRotaCommand("SP", "RJ");
        var rotas = new List<Rota>();
        var expectedResult = new ResultadoMelhorRota();

        _rotaRepository.LoadRotas().Returns(rotas);
        _rotaRepository.EncontrarMelhorRota(rotas, command.Origem, command.Destino).Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rotaRepository.Received(1).LoadRotas();
    }

    [Fact]
    public async Task ExecutarHandle_DeveChamarEncontrarMelhorRota_ComParametrosCorretos()
    {
        // Arrange
        var command = new ResultadoMelhorRotaCommand("SP", "RJ");
        var rotas = new List<Rota>();
        var expectedResult = new ResultadoMelhorRota();

        _rotaRepository.LoadRotas().Returns(rotas);
        _rotaRepository.EncontrarMelhorRota(rotas, command.Origem, command.Destino).Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rotaRepository.Received(1).EncontrarMelhorRota(rotas, "SP", "RJ");
    }

    [Fact]
    public async Task ExecutarHandle_DeveRetornarResultadoCorreto_QuandoRotaEncontrada()
    {
        // Arrange
        var command = new ResultadoMelhorRotaCommand("SP", "RJ");
        var rotas = new List<Rota>
        {
            new Rota { Origem = "SP", Destino = "RJ", Valor = 100 }
        };
        var expectedResult = new ResultadoMelhorRota
        {
            Caminho = new List<string> { "SP", "RJ" },
            Custo = 100
        };

        _rotaRepository.LoadRotas().Returns(rotas);
        _rotaRepository.EncontrarMelhorRota(rotas, command.Origem, command.Destino).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedResult.Caminho, result.Caminho);
        Assert.Equal(expectedResult.Custo, result.Custo);
    }

    [Fact]
    public async Task ExecutarHandle_DeveRetornarResultadoVazio_QuandoNenhumaRotaEncontrada()
    {
        // Arrange
        var command = new ResultadoMelhorRotaCommand("SP", "RJ");
        var rotas = new List<Rota>();
        var expectedResult = new ResultadoMelhorRota
        {
            Caminho = new List<string>(),
            Custo = 0
        };

        _rotaRepository.LoadRotas().Returns(rotas);
        _rotaRepository.EncontrarMelhorRota(rotas, command.Origem, command.Destino).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult.Caminho, result.Caminho);
        Assert.Equal(expectedResult.Custo, result.Custo);
    }

    [Fact]
    public async Task ExecutarHandle_DeveTratarCancellationToken_QuandoFornecido()
    {
        // Arrange
        var command = new ResultadoMelhorRotaCommand("SP", "RJ");
        var rotas = new List<Rota>();
        var expectedResult = new ResultadoMelhorRota();
        var cancellationToken = new CancellationToken();

        _rotaRepository.LoadRotas().Returns(rotas);
        _rotaRepository.EncontrarMelhorRota(rotas, command.Origem, command.Destino).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.NotNull(result);
        _rotaRepository.Received(1).LoadRotas();
        _rotaRepository.Received(1).EncontrarMelhorRota(rotas, command.Origem, command.Destino);
    }

    [Fact]
    public async Task ExecutarHandle_DeveUsarCampoRepositoryCorretamente()
    {
        // Arrange
        var command = new ResultadoMelhorRotaCommand("SP", "RJ");
        var rotas = new List<Rota>();
        var expectedResult = new ResultadoMelhorRota();

        _rotaRepository.LoadRotas().Returns(rotas);
        _rotaRepository.EncontrarMelhorRota(Arg.Any<List<Rota>>(), Arg.Any<string>(), Arg.Any<string>()).Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _rotaRepository.Received(1).LoadRotas();
        _rotaRepository.Received(1).EncontrarMelhorRota(Arg.Any<List<Rota>>(), Arg.Any<string>(), Arg.Any<string>());
    }
}