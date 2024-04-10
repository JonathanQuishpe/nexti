using backend.Context;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        public readonly NextiContext _nextiContext;

        public EventsController(NextiContext nextiContext)
        {
            _nextiContext = nextiContext;
        }


        // GET: api/<EventsController>
        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _nextiContext.Events.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                pageNumber = pageNumber < 1 ? 1 : pageNumber;
                pageSize = pageSize <= 0 ? 10 : pageSize;

                var data = await _nextiContext.Events
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();

                var response = new
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    Data = data
                };

                return Ok(response);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        // POST api/<EventsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Event value)
        {
            try
            {
                _nextiContext.Events.Add(value);

                await _nextiContext.SaveChangesAsync();

                return Ok(new {message="Evento guardado"});

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<EventsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Event value)
        {

            try
            {
                Event evt = _nextiContext.Events.Find(id);

                if (evt == null)
                {
                    return BadRequest("Evento no encontrado");
                }

                evt.Date = value.Date;
                evt.Location = value.Location;
                evt.Description = value.Description;
                evt.Price = value.Price;

                _nextiContext.Events.Update(evt);

                await _nextiContext.SaveChangesAsync();

                return Ok(new { message = "Evento actualizado" });

            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        // DELETE api/<EventsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evt = await _nextiContext.Events.FindAsync(id);

                if (evt == null)
                {
                    return BadRequest("Evento no encontrado");
                }

                _nextiContext.Events.Remove(evt);
                await _nextiContext.SaveChangesAsync();

                return Ok(new { message = "Evento eliminado" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
