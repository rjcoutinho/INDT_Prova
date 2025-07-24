using INDT.Viagens.Application.CommandHandler.ViagemCommand;
using INDT.Viagens.Application.CommandHandler.ViagemHandler;
using INDT.Viagens.Application.Interfaces;
using INDT.Viagens.Domain;
using NSubstitute;

namespace INDT.Viagens.Application.Tests.CommandHandler;

public class ViagemHandlerTests
{
    private readonly IRotaRepository _rotaRepository;
    private readonly ViagemHandler _handler;
    private readonly List<Rota> _rotas;

    public ViagemHandlerTests()
    {
        _rotaRepository = Substitute.For<IRotaRepository>();
        _handler = new ViagemHandler(_rotaRepository);
        _rotas = new List<Rota>
        {
            new Rota { Origem = "SP", Destino = "RJ", Valor = 100 },
            new Rota { Origem = "RJ", Destino = "BH", Valor = 200 }
        };
    }

    [Fact]
    public async Task Handle_CriarRota_DeveAdicionarNovaRotaERetornarRota()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("SP", "BH", 150, TipoOperacao.CriarRota);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SP", result.Origem);
        Assert.Equal("BH", result.Destino);
        Assert.Equal(150, result.Valor);
        _rotaRepository.Received(1).SaveRotas(Arg.Any<List<Rota>>());
    }

    [Fact]
    public async Task Handle_AlterarRota_ComRotaExistente_DeveAlterarERetornarRota()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("SP", "RJ", 120, TipoOperacao.AlterarRota);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SP", result.Origem);
        Assert.Equal("RJ", result.Destino);
        Assert.Equal(120, result.Valor);
        _rotaRepository.Received(1).SaveRotas(Arg.Any<List<Rota>>());
    }

    [Fact]
    public async Task Handle_AlterarRota_ComRotaInexistente_DeveRetornarNull()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("BH", "SP", 120, TipoOperacao.AlterarRota);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _rotaRepository.DidNotReceive().SaveRotas(Arg.Any<List<Rota>>());
    }

    [Fact]
    public async Task Handle_DeletarRota_ComRotaExistente_DeveRemoverERetornarRota()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("SP", "RJ", 0, TipoOperacao.DeletarRota);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SP", result.Origem);
        Assert.Equal("RJ", result.Destino);
        _rotaRepository.Received(1).SaveRotas(Arg.Any<List<Rota>>());
    }

    [Fact]
    public async Task Handle_DeletarRota_ComRotaInexistente_DeveRetornarNull()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("BH", "SP", 0, TipoOperacao.DeletarRota);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _rotaRepository.DidNotReceive().SaveRotas(Arg.Any<List<Rota>>());
    }

    [Fact]
    public async Task Handle_ConsultarRotas_ComRotaExistente_DeveRetornarPrimeiraRota()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("SP", "RJ", 0, TipoOperacao.ConsultarRotas);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SP", result.Origem);
        Assert.Equal("RJ", result.Destino);
        Assert.Equal(100, result.Valor);
        _rotaRepository.DidNotReceive().SaveRotas(Arg.Any<List<Rota>>());
    }

    [Fact]
    public async Task Handle_ConsultarRotas_ComRotaInexistente_DeveRetornarNull()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("BH", "SP", 0, TipoOperacao.ConsultarRotas);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _rotaRepository.DidNotReceive().SaveRotas(Arg.Any<List<Rota>>());
    }

    [Fact]
    public async Task Handle_ConsultarRotas_SemRotasEncontradas_DeveRetornarNull()
    {
        // Arrange
        var rotasVazias = new List<Rota>();
        _rotaRepository.LoadRotas().Returns(rotasVazias);
        var command = new ViagemCommand("SP", "RJ", 0, TipoOperacao.ConsultarRotas);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_TipoOperacaoInvalido_DeveRetornarNull()
    {
        // Arrange
        _rotaRepository.LoadRotas().Returns(_rotas);
        var command = new ViagemCommand("SP", "RJ", 100, TipoOperacao.MelhorRota);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _rotaRepository.DidNotReceive().SaveRotas(Arg.Any<List<Rota>>());
    }
}
