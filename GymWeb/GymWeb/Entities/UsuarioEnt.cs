using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace GymWeb.Entities
{
    public class UsuarioEnt
    {
        public int IdUsuario { get; set; }
        public string? Identificacion { get; set; }
        public string? NombreCompleto { get; set; }
        public int IdRol { get; set; }
        public string? NombreRol { get; set; }

        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Por favor, introduce un correo electrónico válido")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string? contrasenna { get; set; }
        public int Telefono { get; set; }
        public bool? ClaveTemporal { get; set; }
        public DateTime? FechaCreacionUsuario { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public bool Estado { get; set; }
        public byte[]? FotoPerfil { get; set; }
        public string? Token { get; set; }

        public string? contrasennaTemporal { get; set; } 

    }

    public class UsuarioEntRespuesta
    {
        public int Codigo { get; set; }
        public string? Mensaje { get; set; } 
        public UsuarioEnt? Objeto { get; set; }
        public List<UsuarioEnt>? Objetos { get; set; } 
        public bool Resultado { get; set; }
    }


}
