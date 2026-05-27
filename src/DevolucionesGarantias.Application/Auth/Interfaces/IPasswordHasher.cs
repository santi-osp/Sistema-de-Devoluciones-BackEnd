namespace DevolucionesGarantias.Application.Auth.Interfaces;

public interface IPasswordHasher
{
    bool Verify(string password, string passwordHash);
}
