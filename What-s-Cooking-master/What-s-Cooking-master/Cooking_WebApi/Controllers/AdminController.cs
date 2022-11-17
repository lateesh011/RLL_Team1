using Cooking_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cooking_WebApi.Controllers
{
    public class AdminController : ApiController
    {
        FoodReceipesEntities food = new FoodReceipesEntities();
        // GET api/<controller>
        public List<Admin> Get()
        {
            return food.Admins.ToList();
        }

        // GET api/<controller>/5
        public Admin Get(int id)
        {
            List<Admin> list = food.Admins.ToList();
            Admin a=  list.Find(x => x.Adminid == id);
            return a;
        }

        // POST api/<controller>
        public void Post([FromBody] Admin a)
        {
            food.Admins.Add(a);
            food.SaveChanges();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] Admin a)
        {
            Admin a1= food.Admins.ToList().Find(x => x.Adminid == id);
            food.Admins.Remove(a1);
            food.Admins.Add(a);
            food.SaveChanges();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            Admin a1 = food.Admins.ToList().Find(x => x.Adminid == id);
            food.Admins.Remove(a1);
            food.SaveChanges();
        }
    }
}