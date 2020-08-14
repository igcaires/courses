using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursesApi.Models;
using CoursesApi.Models.Context;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoursesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CoursesController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca todos os cursos
        /// </summary>       
        /// <returns>Lista de objetos contendo dados do curso</returns>
        [HttpGet]
        public IEnumerable<Course> GetCourses()
        {
            return _context.Courses;
        }

        /// <summary>
        /// Busca um curso especifico.
        /// </summary>
        /// <param name="id">id do curso</param>
        /// <returns>Objeto Curso de acordo com o Id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        /// <summary>
        /// Altera determinado curso.
        /// </summary>
        /// <param name="id">id do curso</param>
        /// <param name="course">Objeto Curso</param>
        /// <returns>Retorna 204 em caso de sucesso</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse([FromRoute] int id, [FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.Id)
            {
                return BadRequest();
            }

            var validate = validateCourse(course).Result;

            if (validate != "")
            {
                return BadRequest(new { 
                    msg = validate,
                    erro = StatusCodes.Status400BadRequest
                });
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Insere um novo curso.
        /// </summary>
        /// <param name="course">Objeto Curso</param>
        /// <returns>Retorna o Curso inserido com o novo Id</returns>
        [HttpPost]
        public async Task<IActionResult> PostCourse([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validate = validateCourse(course).Result;

            if (validate != "")
            {
                return BadRequest(new
                {
                    msg = validate,
                    erro = StatusCodes.Status400BadRequest
                });
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        private async Task<string> validateCourse(Course course)
        {
            var category = await _context.Categories.FindAsync(course.CategoryId);

            if (category == null)
            {
                return "Categoria não encontrada";
            }


            if (course.StartDate < DateTime.Now)
            {
                return "Data do curso menor do que hoje";
            }

            if (course.StartDate > course.EndDate)
            {
                return "Data inicial maior do que data final";
            }

            var courseSaved = _context.Courses.Where(e => e.Id != course.Id && ((course.StartDate >= e.StartDate && course.StartDate <= e.EndDate) || (course.EndDate >= e.StartDate && course.StartDate <= e.EndDate))).ToList();

            //var courseSaved = from e in _context.Courses where (e.StartDate <= course.StartDate && e.EndDate <= course.StartDate) || (e.StartDate <= course.EndDate && e.EndDate >= course.EndDate) select e;

            if (courseSaved != null && courseSaved.Count > 0)
            {
                return "Existe(m) curso(s) planejados(s) dentro do período informado";
            }

            return "";
        }

        /// <summary>
        /// Remove o curso.
        /// </summary>
        /// <param name="id">Id do curso para remover</param>
        /// <returns>Status 200 com o curso removido</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}