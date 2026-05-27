using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Rol : IEntity
{
    public Rol(RolUsuario tipo)
    {
        Id = Guid.NewGuid();
        Tipo = tipo;
        Nombre = tipo.ToString();
    }

    public Guid Id { get; private set; }
    public RolUsuario Tipo { get; private set; }
    public string Nombre { get; private set; }
}

