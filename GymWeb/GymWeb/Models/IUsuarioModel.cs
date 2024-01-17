using GymWeb.Entities;

namespace GymWeb.Models
{

    public interface IUsuarioModel
    {
        public UsuarioEntRespuesta? InicioSesion(UsuarioEnt entidad);
        public int RegistrarUsuario(UsuarioEnt entidad);
        public int RecuperarContrasenna(UsuarioEnt entidad);

        public int CambiarClaveCuenta(UsuarioEnt entidad);

       
        public int AgregarFotoPerfil(byte[] img, long id);
        public UsuarioEnt ModificarInfoPerfil(UsuarioEnt entidad);

        public UsuarioEntRespuesta? ConsultarUsuarios();
        public Task<int> ActualizarEstadoUsuario(int IdUsuario, bool estado);
        public UsuarioEntRespuesta? ConsultarUsuario(long q);
        public UsuarioEntRespuesta? ModificarUsuario(UsuarioEnt entidad);

        public Task<int> EliminarUsuario(int IdUsuario);

        public UsuarioEntRespuesta? ConsultarRoles();




    }
}

