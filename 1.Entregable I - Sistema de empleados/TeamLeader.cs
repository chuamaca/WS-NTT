namespace ProgramacionOrientadoAObjetos;

public class TeamLeader : Empleado
{
    public int TamanioEquipo { get; private set; }

    public TeamLeader(string nombre, decimal salarioBase, int aniosExperiencia, int tamanioEquipo)
        : base(nombre, salarioBase, aniosExperiencia)
    {
        TamanioEquipo = tamanioEquipo;
    }

    public override decimal CalcularBono()
    {
        decimal bono = SalarioBase * 0.15m;
        bono += TamanioEquipo * 100m;
        return bono;
    }
}
