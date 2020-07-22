using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calculator.Models;

namespace Calculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationsController : ControllerBase
    {
        private readonly CalculationContext _context;

        public CalculationsController(CalculationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a history of calculations.
        /// </summary>
        /// <returns>All previous calculations.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calculation>>> GetCalculations()
        {
            return await _context.Calculations.ToListAsync();
        }

        /// <summary>
        /// Returns a calculation by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Calculation with specified Id.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Calculation>> GetCalculation(int id)
        {
            var calculation = await _context.Calculations.FindAsync(id);

            if (calculation == null)
            {
                return NotFound();
            }

            return calculation;
        }

        /// <summary>
        /// Replace existing calculation from history with a new one Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="calculation"></param>
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalculation(int id, Calculation calculation)
        {
            if (id != calculation.Id)
            {
                return BadRequest();
            }

            _context.Entry(calculation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalculationExists(id))
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
        /// Creates a new Calculation.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Calculation
        ///     {
        ///        "id": 0,
        ///        "firstValue": 20,
        ///        "secondValue": 10,
        ///        "operation": "Add",
        ///        "result": 0
        ///     }
        ///
        /// </remarks>
        /// <param name="calculation"></param>
        /// <returns>Calculation with updated result</returns>
        /// <response code="201">Returns the newly created item with result</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        public async Task<ActionResult<Calculation>> PostCalculation(Calculation calculation)
        {
            PerformCalculation( ref calculation);
            _context.Calculations.Add(calculation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCalculation), new { id = calculation.Id }, calculation);
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Calculation>> DeleteCalculation(int id)
        {
            var calculation = await _context.Calculations.FindAsync(id);
            if (calculation == null)
            {
                return NotFound();
            }

            _context.Calculations.Remove(calculation);
            await _context.SaveChangesAsync();

            return calculation;
        }

        private bool CalculationExists(int id)
        {
            return _context.Calculations.Any(e => e.Id == id);
        }

        private void PerformCalculation(ref Calculation calculation)
        {
            switch (calculation.Operation)
            {
                case Operations.Add:
                    try
                    {
                        calculation.Result = calculation.FirstValue + calculation.SecondValue;
                    }
                    catch (OverflowException)
                    {
                        throw new ArgumentException($"Overflow occurred when adding {calculation.FirstValue} to {calculation.SecondValue}.");
                    }
                    break;
                case Operations.Sub:
                    try
                    {
                        calculation.Result = calculation.FirstValue - calculation.SecondValue;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"Error occurred when subtracting {calculation.SecondValue} from {calculation.FirstValue}.");
                    }
                    break;
                case Operations.Mul:
                    try
                    {
                        calculation.Result = calculation.FirstValue * calculation.SecondValue;
                    }
                    catch (OverflowException)
                    {
                        throw new ArgumentException($"Overflow occurred when multiplying {calculation.FirstValue} on {calculation.SecondValue}.");
                    }
                    break;
                case Operations.Div:
                    try
                    {
                        calculation.Result = calculation.FirstValue / calculation.SecondValue;
                    }
                    catch (DivideByZeroException)
                    {
                        throw new ArgumentException($"Division by zero!");
                    }
                    break;
                case Operations.Rem:
                    try
                    {
                        calculation.Result = calculation.FirstValue % calculation.SecondValue;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"Error occurred when computing the remainder after dividing {calculation.FirstValue} by {calculation.SecondValue}.");
                    }
                    break;
                case Operations.Sqrt:
                    try
                    {
                        calculation.Result = Math.Sqrt(calculation.FirstValue);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"Error occurred when computing the square root of {calculation.FirstValue}.");
                    }
                    break;
                default:
                    // If Operation value is not correct, method assign null as an object`s value
                    calculation = null;
                    break;
            }
        }
    }
}
