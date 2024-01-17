using GymAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using GymAPI.Utils;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutinasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _connection;
        private IUtils _utils;

        public RutinasController(IConfiguration configuration, IUtils utils)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
            _utils = utils;

        }

        [HttpPost]
        [Route("AgregarRutinas")]
        [Authorize]
        public IActionResult AgregarRutinas(RutinasEnt entidad)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Execute("sp_AgregarRutinas", new
                    {
                        nombre = entidad.Nombre,
                        intensidad = entidad.Intensidad,
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
        [Authorize]
        public IActionResult ObtenerRutinas()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Query<RutinasEnt>("SELECT * FROM Rutinas", commandType: CommandType.Text).ToList();
                    var response = datos ?? new List<RutinasEnt>();
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _utils.RegistrarError(ex.Message, "API");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerRutinasTable")]
        [Authorize]
        public IActionResult ObtenerRutinasTable()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Query<RutinasEnt>("sp_ObtenerEjercicios_X_Rutina", commandType: CommandType.StoredProcedure).ToList();
                    var response = datos ?? new List<RutinasEnt>();
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
