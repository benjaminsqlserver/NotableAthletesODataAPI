using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;




namespace NotableAthletes.Controllers.ConData
{
  using Models;
  using Data;
  using Models.ConData;

  [Route("odata/ConData/Countries")]
  public partial class CountriesController : ODataController
  {
    private NotableAthletes.Data.ConDataContext context;

    public CountriesController(NotableAthletes.Data.ConDataContext context)
    {
      this.context = context;
    }
    // GET /odata/ConData/Countries
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.ConData.Country> GetCountries()
    {
      var items = this.context.Countries.AsQueryable<Models.ConData.Country>();
      this.OnCountriesRead(ref items);

      return items;
    }

    partial void OnCountriesRead(ref IQueryable<Models.ConData.Country> items);

    partial void OnCountryGet(ref SingleResult<Models.ConData.Country> item);

    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("/odata/ConData/Countries(CountryID={CountryID})")]
    public SingleResult<Country> GetCountry(int key)
    {
        var items = this.context.Countries.Where(i=>i.CountryID == key);
        var result = SingleResult.Create(items);

        OnCountryGet(ref result);

        return result;
    }
    partial void OnCountryDeleted(Models.ConData.Country item);
    partial void OnAfterCountryDeleted(Models.ConData.Country item);

    [HttpDelete("/odata/ConData/Countries(CountryID={CountryID})")]
    public IActionResult DeleteCountry(int key)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var item = this.context.Countries
                .Where(i => i.CountryID == key)
                .Include(i => i.Athletes)
                .FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            this.OnCountryDeleted(item);
            this.context.Countries.Remove(item);
            this.context.SaveChanges();
            this.OnAfterCountryDeleted(item);

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnCountryUpdated(Models.ConData.Country item);
    partial void OnAfterCountryUpdated(Models.ConData.Country item);

    [HttpPut("/odata/ConData/Countries(CountryID={CountryID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutCountry(int key, [FromBody]Models.ConData.Country newItem)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newItem == null || (newItem.CountryID != key))
            {
                return BadRequest();
            }

            this.OnCountryUpdated(newItem);
            this.context.Countries.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Countries.Where(i => i.CountryID == key);
            this.OnAfterCountryUpdated(newItem);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPatch("/odata/ConData/Countries(CountryID={CountryID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchCountry(int key, [FromBody]Delta<Models.ConData.Country> patch)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = this.context.Countries.Where(i => i.CountryID == key).FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            patch.Patch(item);

            this.OnCountryUpdated(item);
            this.context.Countries.Update(item);
            this.context.SaveChanges();

            var itemToReturn = this.context.Countries.Where(i => i.CountryID == key);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnCountryCreated(Models.ConData.Country item);
    partial void OnAfterCountryCreated(Models.ConData.Country item);

    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.ConData.Country item)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item == null)
            {
                return BadRequest();
            }

            this.OnCountryCreated(item);
            this.context.Countries.Add(item);
            this.context.SaveChanges();

        
            this.OnAfterCountryCreated(item);
            
            return Created($"odata/ConData/Countries/{item.CountryID}", item);
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
