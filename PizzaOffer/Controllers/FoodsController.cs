using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaOffer.DataLayer.Context;
using PizzaOffer.DomainClasses;
using PizzaOffer.Services;
using PizzaOffer.Common;

namespace PizzaOffer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));
            //todo: don't use database directly in controller. create a foodsService class in  PizzaOffer.Services.
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return await _unitOfWork.Set<Food>().ToListAsync();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int id)
        {
            var food = await _unitOfWork.Set<Food>().FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // PUT: api/Foods/5
        [HttpPut("{id}")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.Editor)]
        public async Task<IActionResult> PutFood(int id, Food food)
        {
            if (id != food.Id)
            {
                return BadRequest();
            }
            _unitOfWork.Entry(food).State = EntityState.Modified;

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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

        // POST: api/Foods
        [HttpPost]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.Editor)]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {
            _unitOfWork.Set<Food>().Add(food);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetFood", new { id = food.Id }, food);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.Editor)]
        public async Task<ActionResult<Food>> DeleteFood(int id)
        {
            var food = await _unitOfWork.Set<Food>().FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _unitOfWork.Set<Food>().Remove(food);
            await _unitOfWork.SaveChangesAsync();

            return food;
        }

        private bool FoodExists(int id)
        {
            return _unitOfWork.Set<Food>().Any(e => e.Id == id);
        }
    }
}
