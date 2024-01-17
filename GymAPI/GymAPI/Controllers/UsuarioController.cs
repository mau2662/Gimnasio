using GymAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using GymAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Text;

namespace GymAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UsuarioController : Controller
    {

        private readonly IConfiguration _configuration;
        private string _connection;
        private readonly IUtils _utils;
        private readonly IHostEnvironment _hostingEnvironment;

        public UsuarioController(IConfiguration configuration, IUtils utils, IHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
            _utils = utils;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("ConsultarUsuarios")]
        public IActionResult ConsultarUsuarios()
        {
            var resultado = new List<UsuarioEnt>();
            var respuesta = new UsuarioEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    resultado = connection.Query<UsuarioEnt>("ConsultarUsuarios",
                        new { },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de los usuarios";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Información consultada correctamente";
                    respuesta.Objetos = resultado;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un incoveninete.";
                respuesta.Objetos = resultado;
                return Ok(respuesta);
            }
        }



        [HttpGet]
        [Route("ConsultarUsuario")]
        public IActionResult ConsultarUsuario(long q)
        {
            var resultado = new UsuarioEnt();
            var respuesta = new UsuarioEntRespuesta();
            long IdUsuario = q;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    resultado = connection.Query<UsuarioEnt>("ObtenerUsuario",
                        new { IdUsuario },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información del usuario";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Información consultada correctamente";
                    respuesta.Objeto = resultado;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un incoveninete.";
                respuesta.Objeto = resultado;
                return Ok(respuesta);
            }
        }


        [HttpGet]
        [Route("ConsultarRoles")]
        public IActionResult ConsultarRoles()
        {
            var resultado = new List<UsuarioEnt>();
            var respuesta = new UsuarioEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    resultado = connection.Query<UsuarioEnt>("ConsultarRoles",
                        new { },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de los roles";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Información de roles consultada correctamente";
                    respuesta.Objetos = resultado;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un incoveninete en la consulta de roles.";
                respuesta.Objetos = resultado;
                return Ok(respuesta);
            }
        }


        [HttpPost]
        [Route("InicioSesion")]
        [AllowAnonymous]
        public IActionResult InicioSesion(UsuarioEnt entidad)
        {
            var resultado = new UsuarioEnt();
            var respuesta = new UsuarioEntRespuesta();
            var encryptPassword = _utils.Encrypt(entidad.contrasenna);
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    resultado = connection.Query<UsuarioEnt>("ConsultarUsuario",
                        new { 
                            entidad.Correo, 
                      entidad.contrasenna
                        },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de su usuario";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su usuario fue validado correctamente";
                    resultado.Token = _utils.GenerarToken(resultado.IdUsuario.ToString(), resultado.IdRol.ToString());
                    respuesta.Objeto = resultado;
                    return Ok(respuesta);
                }

            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente";
                return Ok(respuesta);
            }
        }


        [HttpPost]
        [Route("RegistrarUsuario")]
        [AllowAnonymous]
        public IActionResult RegistrarUsuario(UsuarioEnt entidad)
        {
            try
            {
                var encryptPassword = _utils.Encrypt(entidad.contrasenna);
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Execute("RegistrarUsuario", new
                    {
                        entidad.Identificacion,
                        entidad.NombreCompleto,
                        entidad.Telefono,
                        entidad.Correo,
                        contrasenna = encryptPassword
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(datos);
                }
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
                return BadRequest($"Error al Registrar Usuario en la bitácora: {ex.Message}");
            }
        }

        [HttpPatch]
        [Route("AgregarFotoPerfil")]
        public IActionResult AgregarFotoPerfil([FromForm] string foto, long id)
        {
            try
            {
                var img = Convert.FromBase64String(foto);
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Execute("AgregarFotoPerfil", new
                    {
                        foto = img,
                        id = id,
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(datos);
                }
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
                return BadRequest($"Error al Agregar FotoPerfil en la bitácora: {ex.Message}");
            }

        }

        [HttpPut]
        [Route("ModificarPerfil")]
        
        public IActionResult ModificarPerfil(UsuarioEnt entidad) {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.QuerySingleOrDefault("ActualizarPerfil", new
                    {
                        nombre = entidad.NombreCompleto,
                        telefono = entidad.Telefono,
                        email = entidad.Correo,
                        id = entidad.IdUsuario
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(datos);
                }
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
                return BadRequest("Ha ocurrido un error al intentar modificar el usuario");
            }
        }

        [HttpPut]
        [Route("ActualizarUsuario")]
        public IActionResult ModificarUsuario(UsuarioEnt entidad)
        {
            var respuesta = new UsuarioEntRespuesta();
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    int confirmacion = connection.Execute("ActualizarUsuario",
                        new
                        {
                            entidad.Identificacion,
                            entidad.NombreCompleto,
                            entidad.Telefono,
                            entidad.Correo,
                            entidad.IdUsuario,
                            entidad.IdRol
                        },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se actualizó la información del usuario";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Se actualizó la información correctamente";
                    respuesta.Resultado = true;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente.";
                return Ok(respuesta);
            }


        }

        [HttpPut]
        [Route("ActualizarEstadoUsuario")]
        public IActionResult ActualizarEstadoUsuario(UsuarioEnt entidad)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var filasAfectadas = context.Execute("ActualizarEstadoUsuario", new
                    {
                        IdUsuario = entidad.IdUsuario,
                        Estado = entidad.Estado
                    }, commandType: CommandType.StoredProcedure);


                    if (filasAfectadas > 0)
                    {
                        return Ok(1);
                    }
                    else
                    {
                        return Ok(0);
                    }
                }
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("EliminarUsuario")]
        public IActionResult EliminarUsuario(UsuarioEnt entidad)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var filasAfectadas = context.Execute("EliminarUsuario", new {IdUsuario = entidad.IdUsuario}, commandType: CommandType.StoredProcedure);

                    if (filasAfectadas > 0)
                        return Ok(1);
                    return Ok(0);
                }
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("RecuperarCuenta")]
        public IActionResult RecuperarCuenta(UsuarioEnt entidad)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Query<UsuarioEnt>("RecuperarContrasenna", new { entidad.Correo },commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (datos != null)
                    {
                        string contrasennaTemporal = GenerarCodigo();
                        string contenido = ArmarHTML(datos, contrasennaTemporal);

                        context.Execute("ActualizarClaveTemporal",new { datos.IdUsuario, contrasennaTemporal },commandType: CommandType.StoredProcedure);

                        EnviarCorreo(datos.Correo, "Restaurar Contraseña", contenido);
                        return Ok(1);
                    }
                    return Ok(0);
                }
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
                return BadRequest($"Error al Recuperar Cuenta en la bitácora: {ex.Message}");
            }
        }


        [HttpPut]
        [Route("CambiarClaveCuenta")]
        public IActionResult CambiarClaveCuenta(UsuarioEnt entidad)
        {
            try
            {
                entidad.Estado = true;
                var encryptPassword = _utils.Encrypt(entidad.contrasenna);

                using (var context = new SqlConnection(_connection))

                {
                    var datos = context.Execute("CambiarClaveCuenta", new
                    {
                        entidad.IdUsuario,
                        contrasenna = encryptPassword,
                        entidad.contrasennaTemporal
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(datos);
                }
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
                return BadRequest($"Error al Cambiar Clave Cuenta en la bitácora: {ex.Message}");
            }

        }


        private string ArmarHTML(UsuarioEnt datos, string contrasennaTemporal)
        {
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "CorreosTemplate\\Contrasenna.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@Nombre", datos.NombreCompleto);
            htmlArchivo = htmlArchivo.Replace("@@ClaveTemporal", contrasennaTemporal);
            htmlArchivo = htmlArchivo.Replace("@@Link", "https://localhost:7214/Login/CambiarClaveCuenta?q=" + datos.IdUsuario.ToString());

            return htmlArchivo;
        }


        private void EnviarCorreo(string Destinatario, string Asunto, string Mensaje)
        {
            try
            {
                string correoSMTP = _configuration.GetSection("Llaves:correoSMTP").Value;
                string claveSMTP = _configuration.GetSection("Llaves:claveSMTP").Value;

                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(Destinatario));
                msg.From = new MailAddress(correoSMTP);
                msg.Subject = Asunto;
                msg.Body = Mensaje;
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(correoSMTP, claveSMTP);
                client.Port = 587;
                client.Host = "smtp.office365.com";
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                var r = _utils.RegistrarError(ex.Message, "API");
            }
        }

        private string GenerarCodigo()
        {
            int length = 4;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

    }
}