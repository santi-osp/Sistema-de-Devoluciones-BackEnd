using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Proveedor : Usuario
{
    public Proveedor(string nombre, Email correo, string passwordHash, string? createdBy = null)
        : base(nombre, correo, passwordHash, createdBy)
    {
    }
}

