using Cooking_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CookingApp.Test
{
    public class ReceipeMethods
    {
        FoodReceipesEntities food = new FoodReceipesEntities();
        //For Nunit Testing

        public bool UserExists(string email)
        {
           var list= food.Logins.ToList();
           return list.Any(x => x.Email == email);
        }


        public bool Insert(Receipe m)
        {

            try
            {
                food.Receipes.Add(m);
                food.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        
        public bool RecipeExists(int id)
        {
            var list = food.Receipes.ToList();
            Receipe receipe = list.Find(x => x.RId == id);
            if (receipe !=null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool Delete(int id)
        {
            try
            {
                var list = food.Receipes.ToList();
                Receipe receipe = list.Find(x => x.RId == id);

                food.Receipes.Remove(receipe);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool Update(Receipe m)
        {
            try
            {
                var list = food.Receipes.ToList();
                Receipe found = list.Find(x => x.RId == m.RId);
                found.RId = m.RId;
                found.RName = m.RName;
                found.State = m.State;
                found.VNB = m.VNB;
                found.Youtube = m.Youtube;
                found.HTM = m.HTM;
                found.Photo = m.Photo;
                found.Ingredient = m.Ingredient;
                food.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }    
        }
        
    }
}

