using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreWebApi.Models;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly BookStoresDBContext _context;

        public PublishersController(BookStoresDBContext context)
        {
            _context = context;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }
        [HttpGet("GetPublisherDetail/{id}")]
        public async Task<ActionResult<Publisher>> GetPublisherDetail(int id)
        {
            var publisher =  await _context.Publishers
                .Include(pub => pub.Books)
                    .ThenInclude(book=>book.Sales)
                        .ThenInclude(sale=>sale.Store)
                .Include(pub=>pub.Users)
                .Where(pub=>pub.PubId==id).FirstOrDefaultAsync();

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }
        [HttpGet("PostPublisherDetail/")]
        public async Task<ActionResult<Publisher>> PostPublisherDetail()
        {
            var publisher = new Publisher();
            publisher.PublisherName = "Mega Publishing";
            publisher.City = "Addis Ababa";
            // publisher.State = "Amhara";
            var book1 = new Book();
            book1.Title = "New Moon Night - 1";
            book1.PublishedDate = DateTime.Now;
            var book2 = new Book();
            book2.Title = "Mulugeta Mehari Biography";
            book2.PublishedDate = DateTime.Now;
            Sale sale1 = new Sale();
            sale1.Quantity = 2;
            sale1.StoreId = "8042";
            sale1.OrderNum = "XYZ";
            sale1.PayTerms = "Net 30";
            sale1.OrderDate = DateTime.Now;

            Sale sale2 = new Sale();
            sale2.Quantity = 2;
            sale2.StoreId = "7131";
            sale2.OrderNum = "QA879.1";
            sale2.PayTerms = "Net 20";
            sale2.OrderDate = DateTime.Now;

            book1.Sales.Add(sale1);
            book2.Sales.Add(sale2);
            publisher.Books.Add(book1);
            publisher.Books.Add(book2);
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
             var publisherr =await _context.Publishers
                .Include(pub => pub.Books)
                    .ThenInclude(book => book.Sales)
                        .ThenInclude(sale => sale.Store)
                            .ThenInclude(store=>store.Sales)
                .Include(pub => pub.Users)
                .Where(pub => pub.PubId == publisher.PubId).FirstOrDefaultAsync();

            if (publisherr == null)
            {
                return NotFound();
            }

            return publisherr;
        }
        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers
                
                .FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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

        // POST: api/Publishers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return publisher;
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.PubId == id);
        }
    }
}
