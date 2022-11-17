using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Cooking_WebApi.Models;

namespace Cooking_WebApi.Controllers
{
    public class RecipeController : ApiController
    {
        
        FoodReceipesEntities food = new FoodReceipesEntities();

       
        // GET api/<controller>
        public List<Receipe> Get()
        {
            return food.Receipes.ToList();
        }

        // GET api/<controller>/5
        public Receipe Get(int id)
        {
            Receipe r = food.Receipes.ToList().Find(x => x.RId == id);
            return r;
        }

        // POST api/<controller>
        public void Post([FromBody] Receipe r)
        {
            food.Receipes.Add(r);
            food.SaveChanges();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] Receipe r)
        {
            Receipe r1 = food.Receipes.ToList().Find(x => x.RId == id);
            r1.RId = id;
            r1.RName = r.RName;
            r1.VNB = r.VNB;
            r1.Youtube = r.Youtube;
            r1.Photo = r.Photo;
            r1.HTM = r.HTM;
            r1.State = r.State;
            r1.Ingredient = r.Ingredient;

            food.SaveChanges();

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            Receipe r1 = food.Receipes.ToList().Find(x => x.RId == id);
            food.Receipes.Remove(r1);
            food.SaveChanges();
        }
    }
}