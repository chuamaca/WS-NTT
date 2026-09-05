using System.Diagnostics;
using BiblioNET.Api.Dtos;

namespace BiblioNET.Api.Services;

public sealed class BenchmarkRunner
{
    public async Task<BenchmarkResultadoDto> EjecutarAsync(
        string tecnologia,
        string escenario,
        int repeticiones,
        Func<Task<IReadOnlyList<BenchmarkLibroDto>>> operacion)
    {
        repeticiones = Math.Clamp(repeticiones, 3, 15);

        // Warm-up: no se incluye en las métricas.
        await operacion();

        var muestras = new List<double>(repeticiones);
        var filas = 0;

        for (var i = 0; i < repeticiones; i++)
        {
            var sw = Stopwatch.StartNew();

            var resultado = await operacion();

            sw.Stop();

            filas = resultado.Count;
            muestras.Add(sw.Elapsed.TotalMilliseconds);
        }

        var ordenadas = muestras.OrderBy(x => x).ToArray();

        return new BenchmarkResultadoDto(
            Tecnologia: tecnologia,
            Escenario: escenario,
            Repeticiones: repeticiones,
            Filas: filas,
            MinMs: Math.Round(ordenadas.First(), 3),
            MedianaMs: Math.Round(CalcularMediana(ordenadas), 3),
            PromedioMs: Math.Round(muestras.Average(), 3),
            MaxMs: Math.Round(ordenadas.Last(), 3),
            MuestrasMs: muestras.Select(x => Math.Round(x, 3)).ToArray());
    }

    public async Task<BenchmarkConteoResultadoDto>
    EjecutarConteoAsync(
        string tecnologia,
        string escenario,
        int repeticiones,
        Func<Task<long>> operacion)
    {
        repeticiones = Math.Clamp(repeticiones, 3, 15);

        // Warm-up: no se mide.
        await operacion();

        var muestras = new List<double>(repeticiones);
        long coincidencias = 0;

        for (var i = 0; i < repeticiones; i++)
        {
            var sw = Stopwatch.StartNew();

            coincidencias = await operacion();

            sw.Stop();

            muestras.Add(sw.Elapsed.TotalMilliseconds);
        }

        var ordenadas = muestras
            .OrderBy(x => x)
            .ToArray();

        return new BenchmarkConteoResultadoDto(
            Tecnologia: tecnologia,
            Escenario: escenario,
            Repeticiones: repeticiones,
            Coincidencias: coincidencias,
            MinMs: Math.Round(ordenadas.First(), 3),
            MedianaMs: Math.Round(
                CalcularMediana(ordenadas), 3),
            PromedioMs: Math.Round(
                muestras.Average(), 3),
            MaxMs: Math.Round(
                ordenadas.Last(), 3),
            MuestrasMs: muestras
                .Select(x => Math.Round(x, 3))
                .ToArray());
    }

    private static double CalcularMediana(double[] values)
    {
        var middle = values.Length / 2;

        return values.Length % 2 == 0
            ? (values[middle - 1] + values[middle]) / 2d
            : values[middle];
    }
}
