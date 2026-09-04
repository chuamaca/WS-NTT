namespace ProgramacionOrientadoAObjetos;

public class Developer : Empleado
{
    public int ProyectosCompletados { get; private set; }

    public Developer(string nombre, decimal salarioBase, int aniosExperiencia, int proyectosCompletados)
        : base(nombre, salarioBase, aniosExperiencia)
    {
        ProyectosCompletados = proyectosCompletados;
    }

    public override decimal CalcularBono()
    {
        decimal bono = SalarioBase * 0.10m;
        bono += ProyectosCompletados * 50m;
        return bono;
    }
}
