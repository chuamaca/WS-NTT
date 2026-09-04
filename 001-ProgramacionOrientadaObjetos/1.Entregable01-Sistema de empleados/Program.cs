using ProgramacionOrientadoAObjetos;

List<Empleado> empleados = new()
{
    new Developer(nombre: "Cesar Huamani" , salarioBase: 3000m,aniosExperiencia: 2,proyectosCompletados: 5),
    new TeamLeader(nombre: "Carlos Ruiz",salarioBase:  4500m, aniosExperiencia : 5, tamanioEquipo: 8),
    new Manager(nombre: "Lucia Fernandez",salarioBase: 6000m, aniosExperiencia: 10, presupuestoGestionado: 50000m)
};

foreach (Empleado empleado in empleados)
{
    Console.WriteLine(empleado);
}
