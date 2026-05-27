using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Cliente : Usuario
{
    public Cliente(string nombre, Email correo, string passwordHash, string? createdBy = null)
        : base(nombre, correo, passwordHash, createdBy)
    {
    }
}

