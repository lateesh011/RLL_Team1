using Cooking_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cooking_WebApi.Controllers
{
    public class CommentController : ApiController
    {
        FoodReceipesEntities food = new FoodReceipesEntities();
        // GET api/<controller>
        public List<Comments> Get()
        {
            return food.Commentable.ToList();
        }

        // GET api/<controller>/5
        public Comments Get(int id)
        {
            return food.Commentable.ToList().Find(x => x.CommentID == id);
        }

        // POST api/<controller>
        public void Post([FromBody] Comments comment)
        {
            food.Commentable.Add(comment);
            food.SaveChanges();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] Comments comment)
        {
           Comments c= food.Commentable.ToList().Find(x => x.CommentID == id);
            food.Commentable.Remove(c);
            food.Commentable.Add(comment);
            food.SaveChanges();


        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            Comments c = food.Commentable.ToList().Find(x => x.CommentID == id);
            food.Commentable.Remove(c);
            food.SaveChanges();
        }
    }
}