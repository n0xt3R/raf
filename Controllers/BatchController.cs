using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bfws.Data;
using bfws.Models.DBModels;
using bfws.Models;
using Microsoft.AspNetCore.Identity;

namespace bfws.Controllers
{
    [Produces("application/json")]
    [Route("api/Batch")]
    public class BatchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BatchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("SearchResults")]
        public IActionResult GetSearchResults([FromForm]string FirstName, [FromForm]string LastName, [FromForm]string District, [FromForm]string City)
        {
            //Note FromForm - xxxurlencoded and formdata [formBody]

            var query = _context.Users.Include(m => m.District).Include(m => m.City).Where(d => d.Id != null);

            if (Convert.ToInt32(City) > 0)
            {
                query = query.Where(m => m.CityId.Equals(Convert.ToInt32(City)));
            }
            if (Convert.ToInt32(District) > 0)
                query = query.Where(m => m.DistrictId.Equals(Convert.ToInt32(District)));

            if (!String.IsNullOrEmpty(FirstName))
                query = query.Where(m => m.FirstName.Contains(FirstName));

            if (!String.IsNullOrEmpty(LastName))
                query = query.Where(m => m.LastName.Contains(LastName));

            return Ok(query.Select(m => new { Id = m.Id, firstName = m.FirstName, lastName = m.LastName, jobTitle = m.JobTitle, district = m.District.Name, city = m.City.Name, userName = m.UserName }).ToList());
        }
    }
}