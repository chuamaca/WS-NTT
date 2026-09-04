namespace ProgramacionOrientadoAObjetos;

public class Manager : Empleado
{
    public decimal PresupuestoGestionado { get; private set; }

    public Manager(string nombre, decimal salarioBase, int aniosExperiencia, decimal presupuestoGestionado)
        : base(nombre, salarioBase, aniosExperiencia)
    {
        PresupuestoGestionado = presupuestoGestionado;
    }

    public override decimal CalcularBono()
    {
        decimal bono = SalarioBase * 0.20m;
        bono += PresupuestoGestionado * 0.01m;
        return bono;
    }
}
