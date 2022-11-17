using Cooking_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cooking_WebApi.Controllers
{
    public class FeedbackController : ApiController
    {
        FoodReceipesEntities food = new FoodReceipesEntities();

        // GET api/<controller>
        public List<FeedBack> Get()
        {
            return food.FeedBacks.ToList();
        }

        // GET api/<controller>/5
        public FeedBack Get(string id)
        {
            return food.FeedBacks.ToList().Find(x => x.Email == id);
        }

        // POST api/<controller>
        public void Post([FromBody] FeedBack f)
        {
            food.FeedBacks.Add(f);
            food.SaveChanges();
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody] FeedBack f1)
        {
            FeedBack f = food.FeedBacks.ToList().Find(x => x.Email == id);
            food.FeedBacks.Remove(f);
            food.FeedBacks.Add(f);
            food.SaveChanges();
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            FeedBack f = food.FeedBacks.ToList().Find(x => x.Email == id);
            food.FeedBacks.Remove(f);
            food.SaveChanges();
        }
    }
}