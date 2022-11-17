using Cooking_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cooking_WebApi.Controllers
{
    public class UserController : ApiController
    {
        FoodReceipesEntities food = new FoodReceipesEntities();
        // GET api/<controller>
        public List<Login> Get()
        {
            return food.Logins.ToList();
        }

        // GET api/<controller>/5
        public Login Get(int id)
        {
            return food.Logins.ToList().Find(x => x.UserId == id);
        }

        // POST api/<controller>
        public void Post([FromBody] Login l)
        {
            food.Logins.Add(l);
            food.SaveChanges();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] Login l1)
        {
            Login l = food.Logins.ToList().Find(x => x.UserId == id);
            l.UserId = id;
            l.UserName = l1.UserName;
            l.Email = l1.Email;
            l.DOB = l1.DOB;
            l.MobileNumber = l1.MobileNumber;
            l.Password = l1.Password;
            l.Gender = l1.Gender;
            l.Profession = l1.Profession;
            l.City = l1.City;

            food.SaveChanges();

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            Login l = food.Logins.ToList().Find(x => x.UserId == id);
            food.Logins.Remove(l);
            food.SaveChanges();
        }
    }
}