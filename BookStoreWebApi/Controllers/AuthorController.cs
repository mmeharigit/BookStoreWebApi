using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        BookStoresDBContext _context;
        public AuthorController(BookStoresDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Author> Get()
        {


            //get all the authors
            //  return context.Author.ToList();
            //get authors by id
            //add new author 
            //var author = new Author();
            //author.FirstName = "Mulugeta";
            //author.LastName = "Mehari";
            //author.Phone = "0942212422";
            //author.City = "Addis Ababa";
            //author.EmailAddress = "mmeahriwoldeab@gmail.com";
            //context.Author.Add(author);
            //var author = context //update data
            //    .Author.Where(auth => auth.FirstName == "Mulugeta").FirstOrDefault();
            //author.FirstName = "Abiy";
            //author.LastName = "Belay";
            //author.Phone = "0913577623";
            //context.SaveChanges();
            //********** Delete Author ***********
            //var author = context //update data
            // .Author.Where(auth => auth.FirstName == "Abiy").FirstOrDefault();
            //         context.Author.Remove(author);
            //         context.SaveChanges();
            return _context.Authors.ToList();
                
                   
            }
        }
    }

