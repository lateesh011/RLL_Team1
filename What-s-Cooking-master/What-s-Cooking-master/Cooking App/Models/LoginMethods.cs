using Cooking_App.Models;
using Cooking_WebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooking_App
{
    public class LoginMethods
    {
        FoodReceipesEntities food = null;
        public LoginMethods()
        {
                food = new FoodReceipesEntities();
        }
       

        // Find Name Query
        public Login GetName(string email, string pass)
        {
            var list = food.Logins.ToList();
            Login u= list.Find(x => x.Email == email && x.Password == pass);
            return u;
        }

        //logged methods

        //add username and pass to logged table
        public int Temporary(string Email, string Password)
        {
            Logged l = new Logged();
            l.Name = Email;
            l.Password = Password;
            try
            {
                food.Loggeds.Add(l);
                food.SaveChanges();
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        // store vnb in logged table
        public int Temporaryvnb(string Email, string vnb)
        {
            int res = 0;
            try
            {
                var list = food.Loggeds.ToList();
                Logged l = list.First(x => x.Name == Email);
                if (l != null)
                {
                    
                    l.Name = Email;
                    l.Vnb = vnb;

                    food.SaveChanges();
                    res= 1;
                }
                
                
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }

        //store state in logged table
        public int Temporarystate(string Email, string sname)
        {

            int res = 0;
            try
            {
                var list = food.Loggeds.ToList();
                Logged l = list.First(x => x.Name == Email);
                if (l != null)
                {

                    l.Name = Email;
                    l.Sname = sname;

                    food.SaveChanges();
                    res = 1;
                }


            }
            catch (Exception)
            {

                throw;
            }
            return res;        
           
        }
        //taking the details from logged table //login user details
        public Logged TempName()
        {
          
            try
            {
                return food.Loggeds.First();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteLogged()
        {
          
            food.Loggeds.RemoveRange(food.Loggeds);
            food.SaveChanges();
        }
       
    }

}

