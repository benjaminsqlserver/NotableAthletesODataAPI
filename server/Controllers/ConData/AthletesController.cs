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

  [Route("odata/ConData/Athletes")]
  public partial class AthletesController : ODataController
  {
    private NotableAthletes.Data.ConDataContext context;

    public AthletesController(NotableAthletes.Data.ConDataContext context)
    {
      this.context = context;
    }
    // GET /odata/ConData/Athletes
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.ConData.Athlete> GetAthletes()
    {
      var items = this.context.Athletes.AsQueryable<Models.ConData.Athlete>();
      this.OnAthletesRead(ref items);

      return items;
    }

    partial void OnAthletesRead(ref IQueryable<Models.ConData.Athlete> items);

    partial void OnAthleteGet(ref SingleResult<Models.ConData.Athlete> item);

    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("/odata/ConData/Athletes(AthleteID={AthleteID})")]
    public SingleResult<Athlete> GetAthlete(Int64 key)
    {
        var items = this.context.Athletes.Where(i=>i.AthleteID == key);
        var result = SingleResult.Create(items);

        OnAthleteGet(ref result);

        return result;
    }
    partial void OnAthleteDeleted(Models.ConData.Athlete item);
    partial void OnAfterAthleteDeleted(Models.ConData.Athlete item);

    [HttpDelete("/odata/ConData/Athletes(AthleteID={AthleteID})")]
    public IActionResult DeleteAthlete(Int64 key)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var item = this.context.Athletes
                .Where(i => i.AthleteID == key)
                .FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            this.OnAthleteDeleted(item);
            this.context.Athletes.Remove(item);
            this.context.SaveChanges();
            this.OnAfterAthleteDeleted(item);

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnAthleteUpdated(Models.ConData.Athlete item);
    partial void OnAfterAthleteUpdated(Models.ConData.Athlete item);

    [HttpPut("/odata/ConData/Athletes(AthleteID={AthleteID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutAthlete(Int64 key, [FromBody]Models.ConData.Athlete newItem)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newItem == null || (newItem.AthleteID != key))
            {
                return BadRequest();
            }

            this.OnAthleteUpdated(newItem);
            this.context.Athletes.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Athletes.Where(i => i.AthleteID == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Country");
            this.OnAfterAthleteUpdated(newItem);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPatch("/odata/ConData/Athletes(AthleteID={AthleteID})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchAthlete(Int64 key, [FromBody]Delta<Models.ConData.Athlete> patch)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = this.context.Athletes.Where(i => i.AthleteID == key).FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            patch.Patch(item);

            this.OnAthleteUpdated(item);
            this.context.Athletes.Update(item);
            this.context.SaveChanges();

            var itemToReturn = this.context.Athletes.Where(i => i.AthleteID == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Country");
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnAthleteCreated(Models.ConData.Athlete item);
    partial void OnAfterAthleteCreated(Models.ConData.Athlete item);

    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.ConData.Athlete item)
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

            this.OnAthleteCreated(item);
            this.context.Athletes.Add(item);
            this.context.SaveChanges();

            var key = item.AthleteID;

            var itemToReturn = this.context.Athletes.Where(i => i.AthleteID == key);

            Request.QueryString = Request.QueryString.Add("$expand", "Country");

            this.OnAfterAthleteCreated(item);

            return new ObjectResult(SingleResult.Create(itemToReturn))
            {
                StatusCode = 201
            };
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
