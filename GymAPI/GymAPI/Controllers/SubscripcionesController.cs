using GymAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using GymAPI.Utils;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscripcionesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _connection;
        private IUtils _utils;

        public SubscripcionesController(IConfiguration configuration, IUtils utils)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
            _utils = utils;

        }

        [HttpPost]
        [Route("AgregarSubscripcion")]
        public IActionResult AgregarSubscripcion(SubscripcionEnt entidad)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Execute("sp_procesar_subscripcion", new
                    {
                        IdUsuario = entidad.IdUsuario,
                        IdPaquete = entidad.idPaquete,
                        DescripcionPago = entidad.Descripcion,
                        MontoPago = entidad.MontoPago,
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(datos>1);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("ObtenerSubscripcion")]
        public IActionResult ObtenerSubscripcion(int idUsuario) {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.QuerySingleOrDefault<SubscripcionEnt>("sp_ObtenerSubscripcion", new
                    {
                        idUsuario = idUsuario
                    }, commandType: CommandType.StoredProcedure);
                    var response = datos ?? new SubscripcionEnt();
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest(ex.Message);
            }
        }
    }
}
