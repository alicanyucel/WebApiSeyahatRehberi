using AliCanSeyahatRehberi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliCanSeyahatRehberi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetValues()
        {
          var values =await _context.Values.ToListAsync();
           return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(V => V.Id == id);
            return Ok(value);
            
        }
        [HttpPost]
        
        public void Post([FromBody] string value)
        {

        }
        [HttpPut("{id}")]
        
        public void Put(int id,[FromBody] string value)
        {

        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
