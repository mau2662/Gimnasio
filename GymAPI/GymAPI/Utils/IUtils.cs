using System.Security.Claims;

namespace GymAPI.Utils
{
    public interface IUtils
    {
        public string GenerarToken(string idUsuario, string conRol);
        public long ObtenerUsuario(IEnumerable<Claim> valores);
        public string Encrypt(string texto);
        public string Decrypt(string texto);
        public bool RegistrarError(string descripcion, string tipo);
    }
}
