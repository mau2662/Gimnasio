using GymAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using GymAPI.Utils;
using Microsoft.AspNetCore.Authorization;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EjercicioController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string _connection;
        private IHostEnvironment _hostingEnvironment;
        private IUtils _utils;

        public EjercicioController(IConfiguration configuration, IHostEnvironment hostingEnvironment, IUtils utils)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
            _hostingEnvironment = hostingEnvironment;
            _utils = utils;
        }


        [HttpPost]
        [Route("CrearEjercicio")]
        public IActionResult CrearEjercicio(EjercicioEnt entidad)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Execute("CrearEjercicio", new
                    {
                        entidad.NombreEjercicio,
                        entidad.DescripcionEjercicio,
                        entidad.VideoEjercicio
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(datos);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest($"Error al registrar en la bitácora: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("ConsultarEjercicio")]
        public IActionResult ConsultarEjercicio()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Query<EjercicioEnt>("ConsultarEjercicio",
                        new { },
                        commandType: CommandType.StoredProcedure).ToList();

                    return Ok(datos);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest($"Error al Consultar Ejercicio en la bitácora: {ex.Message}");
            }
        }

        
        [HttpDelete]
        [Route("EliminarEjercicio")]
        public IActionResult EliminarEjercicio(long q)
        {
            try
            {
                long IdEjercicio = q;

                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Execute("EliminarEjercicio",
                        new { IdEjercicio },
                        commandType: CommandType.StoredProcedure);

                    return Ok(datos);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest($"Error al Eliminar Ejercicio en la bitácora: {ex.Message}");
            }
        }

    }
}
