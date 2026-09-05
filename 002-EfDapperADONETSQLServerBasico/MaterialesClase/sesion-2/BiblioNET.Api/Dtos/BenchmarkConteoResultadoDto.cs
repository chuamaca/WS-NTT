namespace BiblioNET.Api.Dtos;

public sealed record BenchmarkConteoResultadoDto(
    string Tecnologia,
    string Escenario,
    int Repeticiones,
    long Coincidencias,
    double MinMs,
    double MedianaMs,
    double PromedioMs,
    double MaxMs,
    IReadOnlyList<double> MuestrasMs);