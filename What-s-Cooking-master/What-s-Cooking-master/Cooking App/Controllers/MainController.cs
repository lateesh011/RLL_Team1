using Cooking_App.Models;
using Cooking_WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Cooking_App.Controllers
{
    public class MainController : Controller
    {
        // GET: Main

        Uri baseAddress = new Uri("https://localhost:44322/api/");

        HttpClient client;
        LoginMethods lmethods;
        public MainController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            lmethods = new LoginMethods();
          
        }
       

    public ActionResult MainPage()
        {
            List<Login> list = new List<Login >();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User").Result;
            if (response.IsSuccessStatusCode)
            {
                String Data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<Login>>(Data);
            }

            List<Receipe> rlist = new List<Receipe>();
            HttpResponseMessage response1 = client.GetAsync(client.BaseAddress + "/Recipe").Result;
            if (response1.IsSuccessStatusCode)
            {
                String Data = response1.Content.ReadAsStringAsync().Result;
                rlist = JsonConvert.DeserializeObject<List<Receipe>>(Data);
            }
            List<FeedBack> flist = new List<FeedBack>();
            HttpResponseMessage response2 = client.GetAsync(client.BaseAddress + "/Feedback").Result;
            if (response2.IsSuccessStatusCode)
            {
                String Data = response2.Content.ReadAsStringAsync().Result;
                flist = JsonConvert.DeserializeObject<List<FeedBack>>(Data);
            }

            try
            {
                ViewBag.Id = list.Count();
                ViewBag.Rid = rlist.Count();
                TempData["T1"] = null;
                TempData["M1"] = "MainPage";
                lmethods.DeleteLogged();
                ViewBag.FeedList = flist.Take(3);
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public ActionResult MainPage(string name, string email, string msg)
        {
            try
            {
                TempData["M1"] = "MainPage";
                FeedBack f = new FeedBack();
                f.Name = name;
                f.Email = email;
                f.Msg = msg;


                List<Login> list = new List<Login>();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User").Result;
                if (response.IsSuccessStatusCode)
                {
                    String Data = response.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<Login>>(Data);
                }
                bool ans = list.Any(x => x.Email == f.Email);
                Login u = list.FirstOrDefault(x => x.Email == f.Email);
                if (ans)
                {
                    f.UserId = u.UserId;
                    string data = JsonConvert.SerializeObject(f);

                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response1 = client.PostAsync(client.BaseAddress + "/Feedback", content).Result;
                    if (response1.IsSuccessStatusCode)
                    {
                        List<FeedBack> flist = new List<FeedBack>();
                        HttpResponseMessage response2 = client.GetAsync(client.BaseAddress + "/Feedback").Result;
                        if (response2.IsSuccessStatusCode)
                        {
                            String Data = response2.Content.ReadAsStringAsync().Result;
                            flist = JsonConvert.DeserializeObject<List<FeedBack>>(Data);
                        }
                        ViewBag.FeedList = flist.Take(3);
                        return View();
                    }

                }

                else
                {
                    List<FeedBack> flist = new List<FeedBack>();
                    HttpResponseMessage response2 = client.GetAsync(client.BaseAddress + "/Feedback").Result;
                    if (response2.IsSuccessStatusCode)
                    {
                        String Data = response2.Content.ReadAsStringAsync().Result;
                        flist = JsonConvert.DeserializeObject<List<FeedBack>>(Data);
                    }
                    ViewBag.FeedList = flist.Take(3);

                    ViewBag.Error = "Yor are Not a User";
                    return View();

                }

                return View();
            }
            catch (Exception)
            {

                throw;
            }
            

        }


        public ActionResult FeedbackPage()
        {
            List<FeedBack> flist = new List<FeedBack>();
            HttpResponseMessage response2 = client.GetAsync(client.BaseAddress + "/Feedback").Result;
            if (response2.IsSuccessStatusCode)
            {
                String Data = response2.Content.ReadAsStringAsync().Result;
                flist = JsonConvert.DeserializeObject<List<FeedBack>>(Data);
            }
            ViewBag.FeedList = flist;
            return View();
        }


        public ActionResult LoginPage()
        {
            TempData["M1"] = null;
            lmethods.DeleteLogged();
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage( FormCollection form)
        {
            List<Login> list = new List<Login>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User").Result;
            if (response.IsSuccessStatusCode)
            {
                String Data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<Login>>(Data);
            }

            Login l = new Login();
            l.Email = form["email"];
            l.Password = form["password"];
            bool ans = list.Any(x => x.Email == l.Email && x.Password == l.Password);

            if (ans)
            {
           
                Login u = list.Find(x => x.Email == l.Email && x.Password == l.Password);
                TempData["T1"] = u.UserName;
                lmethods.Temporary(l.Email, l.Password); ///to store the email ans pass in looged table..  
                TempData["sucess"] = "success";
                return RedirectToAction("VNBMenu");
            }
            else
            {
                ViewBag.Message = "Invalid Username or password";
                return View();
            }
       
        }

        public ActionResult SignupPage()
        {
            TempData["M1"] = null;
            lmethods.DeleteLogged();
            return View();
        }


        [HttpPost]
        public ActionResult SignupPage(FormCollection form)
        {
            Login u = new Login();
            string fname = form["fname"];
            string lname = form["lname"];
            string FullName = null;
            if (fname != null)
            {
                FullName = fname + " " + lname;
            }

            FullName = fname;
            u.UserName = FullName;
            u.Email = form["email"];
            u.DOB = Convert.ToDateTime(form["dob"]);
            u.MobileNumber = form["no"];
            u.Password = form["password"];
            string s = form["confirm password"];
            if (s == u.Password)
            {
                string data = JsonConvert.SerializeObject(u);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/User",content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LoginPage");
                }
                return View();
            }
            else
            {
                ViewBag.pass = "Confirm password and password should be same";
                return View();
            }


            return View();
        }
        public ActionResult ForgotPassword()
        {
            TempData["M1"] = null;
            lmethods.DeleteLogged();
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(FormCollection form)
        {
            Login l = new Login();
            l.Email = form["email"];
            l.DOB = Convert.ToDateTime(form["date"]);
            l.Password = form["password"];

            List<Login> list = new List<Login>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User").Result;
            if (response.IsSuccessStatusCode)
            {
                String Data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<Login>>(Data);
            }
            Login u= list.Find(x=>x.Email==l.Email && x.DOB==l.DOB);
            if (u!=null)
            {
                u.Password = l.Password;

                string data = JsonConvert.SerializeObject(u);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = client.PutAsync(baseAddress + "/User/" + u.UserId, content).Result;
                if (response1.IsSuccessStatusCode)
                {
                    ViewBag.Msg1 = "Password Changed Sucessfully";
                    return View();
                }
                else
                {
                    ViewBag.Msg2 = "Given input is not valid";
                    return View();
                }

            }
            return View();
            
           
          
        }

        public ActionResult LogOut()
        {
            lmethods.DeleteLogged();
            return RedirectToAction("MainPage");
        }


        public ActionResult Profile()
        {
            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            int id = u.UserId;

            Login p = new Login();

            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                String Data = response.Content.ReadAsStringAsync().Result;
                p = JsonConvert.DeserializeObject<Login>(Data);
            }

            return View(p);
        }
        [HttpPost]
        public ActionResult Profile(Login l)
        {

            string data = JsonConvert.SerializeObject(l);
            StringContent Content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(baseAddress + "/User/" + l.UserId, Content).Result;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.profile = "Profile Updated Successfully";
                return RedirectToAction("VNBMenu");
            }



            return View();
        }




       // ====================================================================================================

        //Recipe View for User
        public ActionResult VNBMenu()
        {

            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            return View();

        }

        public ActionResult StateWiseMenu(string id)
        {
            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            ViewBag.Id = u.UserId;
            lmethods.Temporaryvnb(l.Name, id);

            try
            {
                List<Receipe> rlist = new List<Receipe>();
                HttpResponseMessage response1 = client.GetAsync(client.BaseAddress + "/Recipe").Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response1.Content.ReadAsStringAsync().Result;
                    rlist = JsonConvert.DeserializeObject<List<Receipe>>(Data);
                }

                List<String> list = rlist.Where(x => x.VNB == id).Select(m => m.State).Distinct().ToList();
                List<State> slist = new List<State>();
                foreach (var item in list)
                {
                    slist.Add(new State { Sname = item });
                }
            

                return View(slist);
            }
            catch (Exception)
            {

                throw;
            }
            
        }


        public ActionResult MainMenu(string id)
        {
            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            lmethods.Temporarystate(l.Name, id);
            TempData["sname"] = l.Sname;

            Receipe m = new Receipe();
            ViewBag.Id = u.UserId;
            ViewBag.Image = m.Photo;
            ViewBag.Vnb = l.Vnb;

            try
            {
                List<Receipe> rlist = new List<Receipe>();
                HttpResponseMessage response1 = client.GetAsync(client.BaseAddress + "/Recipe").Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response1.Content.ReadAsStringAsync().Result;
                    rlist = JsonConvert.DeserializeObject<List<Receipe>>(Data);
                }

                List<Receipe> list1 = rlist.FindAll(x => x.State == id && x.VNB == l.Vnb);

             

                return View(list1);
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        public ActionResult Info(int id)
        {
            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            ViewBag.uid = u.UserId;
            
            if (TempData["T1"] !=null)
            {
                Receipe m = new Receipe();
                HttpResponseMessage response1 = client.GetAsync(client.BaseAddress + "/Recipe/" + id).Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response1.Content.ReadAsStringAsync().Result;
                    m = JsonConvert.DeserializeObject<Receipe>(Data);
                    TempData["rid"] = m.RId;
                    ViewBag.Rname = m.RName;    //RName = Recipe Name
                    ViewBag.Vnb = m.VNB;        //VNB = Veg Non-Veg Beverage
                    ViewBag.State = m.State;
                    ViewBag.Photo = m.Photo;
                    ViewBag.Youtube = m.Youtube;
                    ViewBag.Ingredient = m.Ingredient;
                    ViewBag.Htm = m.HTM;
                }
                
                List<Comments> list = new List<Comments>();
                HttpResponseMessage response2 = client.GetAsync(client.BaseAddress + "/Comment").Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response2.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<Comments>>(Data);

                    List<Comments> clist= list.Where(x=>x.RId==m.RId).ToList();
                    ViewBag.CList = clist;                
                }
             
                return View();
            }

            return View();

        }

        [HttpPost]
        public ActionResult Info(Comments comments)
        {
            Logged l = lmethods.TempName();
            Login  u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            Comments c = new Comments();
            c.RId = Convert.ToInt32( TempData["rid"]);
            c.UserId = u.UserId;
            c.Comment = comments.Comment;
            c.UserName = u.UserName;
            c.DateofCreation = DateTime.Now;

            string data = JsonConvert.SerializeObject(c);

            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Comment", content).Result;
            if (response.IsSuccessStatusCode)
            {

                Receipe m = new Receipe();
                HttpResponseMessage response1 = client.GetAsync(client.BaseAddress + "/Recipe/" + c.RId).Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response1.Content.ReadAsStringAsync().Result;
                    m = JsonConvert.DeserializeObject<Receipe>(Data);
                    TempData["rid"] = m.RId;
                    ViewBag.Rname = m.RName;    //RName = Recipe Name
                    ViewBag.Vnb = m.VNB;        //VNB = Veg Non-Veg Beverage
                    ViewBag.State = m.State;
                    ViewBag.Photo = m.Photo;
                    ViewBag.Youtube = m.Youtube;
                    ViewBag.Ingredient = m.Ingredient;
                    ViewBag.Htm = m.HTM;
                }

                List<Comments> list = new List<Comments>();
                HttpResponseMessage response2 = client.GetAsync(client.BaseAddress + "/Comment").Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response2.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<Comments>>(Data);

                    List<Comments> clist = list.Where(x => x.RId == m.RId).ToList();
                    ViewBag.CList = clist;
                   
                    return View();
                }

            }
                return View();
        }


        //================================================================================================

        //Beverage
        
        public ActionResult BeverageMenu(string id)
        {
            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            ViewBag.Id = u.UserId;
            lmethods.Temporaryvnb(l.Name, id);

            try
            {
                List<Receipe> rlist = new List<Receipe>();
                HttpResponseMessage response1 = client.GetAsync(client.BaseAddress + "/Recipe").Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response1.Content.ReadAsStringAsync().Result;
                    rlist = JsonConvert.DeserializeObject<List<Receipe>>(Data);
                }

                List<Receipe> list1 = rlist.FindAll(x =>  x.VNB == id);

                //ViewBag.List = list1;

                return View(list1);
            }
            catch (Exception)
            {

                throw;
            }

         
        }


        //=======================================================================================

        //Find Recipe

        public ActionResult GetList()
        {
            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            if (TempData["T1"]!=null)
            {
                List<Receipe> list = new List<Receipe>();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Recipe").Result;
                if (response.IsSuccessStatusCode)
                {
                    String Data = response.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<Receipe>>(Data);
                }
                return View(list);
            }

            return View();

        }
        [HttpPost]
        public ActionResult GetList(string searchstring)
        {
            Login u = new Login();
            Logged l = lmethods.TempName();
            u = lmethods.GetName(l.Name, l.Password);
            TempData["T1"] = u.UserName;
            if (searchstring != null)
            {
                List<Receipe> list = new List<Receipe>();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Recipe").Result;
                if (response.IsSuccessStatusCode)
                {
                    String Data = response.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<Receipe>>(Data);
                }
                List<Receipe> rlist = list.Where(x => x.RName.Contains(searchstring)).ToList();

                return View(rlist);
            }
            return View();
        }


    }
}