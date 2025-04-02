using DotNetInterview.API.Domain;
using DotNetInterview.API.DTOs;
using DotNetInterview.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetInterview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PricingService _pricingService;

        public ItemsController(DataContext context, PricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
        {
            var items = await _context.Items
                .Include(i => i.Variations)
                .ToListAsync();

            return items.Select(MapToDto).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(Guid id)
        {
            var item = await _context.Items
                .Include(i => i.Variations)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();
            return MapToDto(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(Item item)
        {
            if (item.Price <= 0)
                return BadRequest("Price must be greater than 0");

            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, MapToDto(item));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, Item item)
        {
            if (id != item.Id) return BadRequest();
            if (item.Price <= 0) return BadRequest("Price must be greater than 0");

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();
            
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private ItemDto MapToDto(Item item)
        {
            var totalQuantity = item.Variations.Sum(v => v.Quantity);
            var discount = _pricingService.GetApplicableDiscount(totalQuantity);
            
            return new ItemDto
            {
                Id = item.Id,
                Reference = item.Reference,
                Name = item.Name,
                OriginalPrice = item.Price,
                DiscountedPrice = _pricingService.ApplyDiscounts(item.Price, totalQuantity),
                DiscountPercentage = discount,
                Variations = item.Variations.Select(v => new VariationDto
                {
                    Id = v.Id,
                    Size = v.Size,
                    Quantity = v.Quantity
                }).ToList()
            };
        }
    }
}