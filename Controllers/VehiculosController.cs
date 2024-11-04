using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RoundTripAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public VehiculosController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("categoria")]
        public IActionResult ObtenerVehiculoDisponible([FromQuery] String categoria )
        {
            if (categoria == null)
            {
                return BadRequest("Debe proporcionar una categoria");
            }

            string storedProcedure = "SP_Muestra_Vehiculo_Disponible"; // Cambiar por el nombre real del SP

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                // Añadir los parámetros de fecha al stored procedure
                command.Parameters.AddWithValue("@categoria", categoria);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    var vehiculos = new List<Vehiculos>(); // Asumiendo que tienes un DTO llamado PagoDto

                    while (reader.Read())
                    {
                        var vehiculo = new Vehiculos
                        {
                            Marca = reader["Marca"].ToString(),
                            Modelo = reader["Modelo"].ToString(),
                            PrecioDia = (decimal)reader["PrecioDia"],
                            Imagen = reader["Imagen"].ToString()
                        };
                        vehiculos.Add(vehiculo);
                    }

                    reader.Close();

                    if (vehiculos.Count > 0)
                    {
                        return Ok(vehiculos); // Retorna la lista de pagos
                    }
                    else
                    {
                        return NotFound("No hay vehiculos disponibles.");
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    return StatusCode(500, $"Error interno del servidor: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
