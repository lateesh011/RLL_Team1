using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Cooking_App.Models;
using Cooking_WebApi.Controllers;
using Cooking_WebApi.Models;
using Newtonsoft.Json;

namespace Cooking_App.Controllers
{
    public class AdminController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44322/api/");
     
        HttpClient client;
        LoginMethods lmethods;
        public AdminController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            lmethods = new LoginMethods();
        }

        

        public ActionResult LoginPage()
        {
            TempData["M1"] = null;
            lmethods.DeleteLogged();
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(FormCollection form)
        {
            List<Admin> list = new List<Admin>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Admin").Result;
            if (response.IsSuccessStatusCode)
            {
                String Data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<Admin>>(Data);
            }
            Admin l = new Admin();
            l.Email = form["email"];
            l.Password = form["password"];

            bool ans = list.Any(x => x.Email == l.Email && x.Password == l.Password);
            Admin u = list.FirstOrDefault(x => x.Email == l.Email && x.Password == l.Password);
            if (ans)
            {            
                TempData["A1"] = u.Username;
                lmethods.Temporary(u.Username, u.Password);
                TempData["sucess"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Invalid Username or password";
                return View();
            }

        }
        public ActionResult LogOut()
        {
            lmethods.DeleteLogged();
            return RedirectToAction("MainPage","Main");
        }

        // GET: Admin
        public ActionResult Index()
        {
            List<Receipe> list = new List<Receipe>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Recipe").Result;
            if (response.IsSuccessStatusCode)
            {
                String Data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<Receipe>>(Data);
            }
            Logged l= lmethods.TempName();
            TempData["A1"] = l.Name;
            if (TempData["A1"] !=null)
            {
                return View(list);
            }
            else
            {
                return View("LoginPage");
            }
            
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            Logged l = lmethods.TempName();
            TempData["A1"] = l.Name;
            if (TempData["A1"] != null)
            {
                Receipe m = new Receipe();
                HttpResponseMessage response1 = client.GetAsync(client.BaseAddress + "/Recipe/" + id).Result;
                if (response1.IsSuccessStatusCode)
                {
                    String Data = response1.Content.ReadAsStringAsync().Result;
                    m = JsonConvert.DeserializeObject<Receipe>(Data);
                    ViewBag.Rid = m.RId;
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
                }

                return View();

            }
            return View();

        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            Logged l = lmethods.TempName();
            TempData["A1"] = l.Name;
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Img receipe)
        {
            string FileName = Path.GetFileNameWithoutExtension(receipe.ImageFile.FileName);
 
            string FileExtension = Path.GetExtension(receipe.ImageFile.FileName);
 
            FileName = FileName+ DateTime.Now.ToString("yymmssfff") + FileExtension;
            receipe.Photo = "../RecipeImg/" + FileName;
            FileName = Path.Combine(Server.MapPath("../RecipeImg/"), FileName);

            
            receipe.ImageFile.SaveAs(FileName);

            Receipe r = new Receipe();
            r.RName = receipe.RName;
            r.Photo = receipe.Photo;
            string youtube = receipe.Youtube;
            youtube = youtube.Replace("watch?v=", "embed/");
            r.Youtube = youtube;
            r.HTM = receipe.HTM;
            r.Ingredient = receipe.Ingredient;
            r.State = receipe.State;
            r.VNB = receipe.VNB;


            string data = JsonConvert.SerializeObject(r);

            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Recipe", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            else
            {
                return View();
            }
      
           
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            Logged l = lmethods.TempName();
            TempData["A1"] = l.Name;
            if (TempData["A1"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Receipe receipe = new Receipe();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Recipe/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    String Data = response.Content.ReadAsStringAsync().Result;
                    receipe = JsonConvert.DeserializeObject<Receipe>(Data);
                }
                if (receipe == null)
                {
                    return HttpNotFound();

                }
                TempData["Photo"] = receipe.Photo;
                return View(receipe);
            }
            return View();


        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Receipe receipe, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);

                string FileExtension = Path.GetExtension(ImageFile.FileName);
                FileName = FileName + DateTime.Now.ToString("yymmssfff") + FileExtension;
                receipe.Photo = "../RecipeImg/" + FileName;
                FileName = Path.Combine(Server.MapPath(@"C:\Users\BALAJI\Downloads\What-s-Cooking-master\What-s-Cooking-master\Cooking App\RecipeImg\"), receipe.Photo);


                ImageFile.SaveAs(FileName);

                Receipe r = new Receipe();
                r.RName = receipe.RName;
                r.Photo = receipe.Photo;
                string youtube = receipe.Youtube;
                youtube = youtube.Replace("watch?v=", "embed/");
                r.Youtube = youtube;
                r.HTM = receipe.HTM;
                r.Ingredient = receipe.Ingredient;
                r.State = receipe.State;
                r.VNB = receipe.VNB;

                string data = JsonConvert.SerializeObject(r);
                StringContent Content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(baseAddress + "/Recipe/" + receipe.RId, Content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(receipe);
            }
            else
            {
                Receipe r = new Receipe();
                r.RName = receipe.RName;
                r.Photo = TempData["Photo"].ToString();
                string youtube = receipe.Youtube;
                youtube = youtube.Replace("watch?v=", "embed/");
                r.Youtube = youtube;
                r.HTM = receipe.HTM;
                r.Ingredient = receipe.Ingredient;
                r.State = receipe.State;
                r.VNB = receipe.VNB;

                string data = JsonConvert.SerializeObject(r);
                StringContent Content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(baseAddress + "/Recipe/" + receipe.RId, Content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(receipe);
            }

        }



        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            Receipe r = new Receipe();
            HttpResponseMessage response = client.DeleteAsync(baseAddress + "/Recipe/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult FindRecipe(string searchstring)
        {
            List<Receipe> list = new List<Receipe>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Recipe").Result;
            if (response.IsSuccessStatusCode)
            {
                String Data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<Receipe>>(Data);
            }
            List<Receipe> rlist = list.Where(x => x.RName.Contains(searchstring) || x.Ingredient.Contains(searchstring) || x.HTM.Contains(searchstring)).ToList();

            return View(rlist);

        }


      



    }
}
