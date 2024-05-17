using ApiPersonajesAWS.Models;
using ApiPersonajesAWS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPersonajesAWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private RepositoryPersonajes repo;
        public PersonajesController(RepositoryPersonajes repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Personaje>>> GetPersonajes()
        {
            return await this.repo.GetPersonajesAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Personaje>> Get(int id)
        {
            return await this.repo.FindPersonajeAsync(id);
        }        
        [HttpPost]
        public async Task<ActionResult> Create(Personaje personaje)
        {
            await this.repo.CreatePersonajeAsync(personaje.Nombre, 
                personaje.Imagen);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> Update(Personaje personaje)
        {
            await this.repo.UpdatePersonajeAsync(personaje.IdPersonaje, personaje.Nombre, 
                personaje.Imagen);
            return Ok();
        }
    }
}
