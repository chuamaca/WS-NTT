namespace BiblioNET.Api.Dtos;

public sealed record BenchmarkResultadoDto(
    string Tecnologia,
    string Escenario,
    int Repeticiones,
    int Filas,
    double MinMs,
    double MedianaMs,
    double PromedioMs,
    double MaxMs,
    IReadOnlyList<double> MuestrasMs);
