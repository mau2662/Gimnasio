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
    public class PagosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _connection;
        private IUtils _utils;

        public PagosController(IConfiguration configuration, IUtils utils)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
            _utils = utils;

        }

        [HttpGet]
        [Route("ObtenerPagos")]
        [Authorize]
        public IActionResult ObtenerPagos(int idUsuario)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var datos = context.Query<PagosEnt>("sp_Obtener_Pagos", new
                    {
                        idUsuario = idUsuario
                    }, commandType: CommandType.StoredProcedure).ToList();
                    return Ok(datos);
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
