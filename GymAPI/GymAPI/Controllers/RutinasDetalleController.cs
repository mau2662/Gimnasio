using GymAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using GymAPI.Utils;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RutinasDetalleController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string _connection;
        private IUtils _utils;

        public RutinasDetalleController(IConfiguration configuration, IUtils utils)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
            _utils = utils;
        }


        [HttpPost]
        [Route("AgregarRutinas")]
        public IActionResult AgregarRutinas(RutinasDetalleEnt entidad)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Execute("sp_AgregarRutinasDetalle", new
                    {
                        idRutina = entidad.idRutina,
                        idEjercicio = entidad.idEjercicio,
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(datos > 0);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("ObtenerRutinas")]
        public IActionResult ObtenerRutinas()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Query<RutinasDetalleEnt>("SELECT * FROM RutinasDetalle", commandType: CommandType.Text).ToList();
                    var response = datos ?? new List<RutinasDetalleEnt>();
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("EliminarRutina")]
        public IActionResult EliminarRutina(int id)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Query<RutinasDetalleEnt>("DELETE FROM RutinasDetalle WHERE Id = @id", new { id= id }, commandType: CommandType.Text).ToList();
                    var response = datos ?? new List<RutinasDetalleEnt>();
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
