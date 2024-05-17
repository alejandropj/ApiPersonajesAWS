using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;
        private readonly IConfiguration _configuration;
        public RepositoryPersonajes(PersonajesContext context, IConfiguration configuration)
        {
            this.context = context;
            this._configuration = configuration;
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }
        public async Task<Personaje> FindPersonajeAsync(int idpersonaje)
        {
            return await this.context.Personajes.FirstOrDefaultAsync
                (x => x.IdPersonaje == idpersonaje);
        }
        private async Task<int> GetMaxIdPersonajeAsync()
        {
            return await this.context.Personajes.MaxAsync(x => x.IdPersonaje) + 1;
        }

        public async Task CreatePersonajeAsync(string nombre, string imagen)
        {
            Personaje personaje = new Personaje
            {
                IdPersonaje = await this.GetMaxIdPersonajeAsync(),
                Nombre = nombre,
                Imagen = imagen
            };
            this.context.Personajes.Add(personaje);
            await this.context.SaveChangesAsync();
        }
        #region PROCEDIMIENTOS ALMACENADOS
/*      DELIMITER //
        CREATE PROCEDURE ActualizarPersonaje(
            IN p_id INT,
            IN p_nombre VARCHAR(60),
            IN p_imagen  VARCHAR(250)
        )
        BEGIN
            UPDATE PERSONAJES
            SET
                PERSONAJE = p_nombre,
                IMAGEN = p_imagen
            WHERE IDPERSONAJE = p_id;
        END //
        DELIMITER;*/
        #endregion
        public async Task UpdatePersonajeAsync(int id, string nombre, string imagen)
        {
            string connectionString = _configuration.GetConnectionString("MySqlTelevision");
            using (MySqlConnection connection = new MySqlConnection(connectionString)) 
            { 
                connection.Open(); 
                using (MySqlCommand command = new MySqlCommand("ActualizarPersonaje", connection)) 
                { 
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("p_id", id); 
                    command.Parameters.AddWithValue("p_nombre", nombre); 
                    command.Parameters.AddWithValue("p_imagen", imagen); 
                    command.ExecuteNonQuery(); 
                } 
            }
        }
        
/*        public async Task UpdatePersonajeAsync(int id, string nombre, string imagen)
        {
            Personaje personaje = await this.FindPersonajeAsync(id);
            personaje.Nombre = nombre;
            personaje.Imagen = imagen;

            await this.context.SaveChangesAsync();
        }*/
    }
}
