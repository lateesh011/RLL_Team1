using Cooking_WebApi.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingApp.Test
{

    [TestFixture]
    public class ReceipeTest
    {
        ReceipeMethods methods = new ReceipeMethods();

        [TestCase("salini@gmail.com",ExpectedResult =true)]
        [TestCase("lateesh@gmail.com",ExpectedResult =true)]
        [TestCase("admin@gmail.com",ExpectedResult =false)]
        public bool UserCheck(string email)
        {
           return methods.UserExists(email);
        }



        [TestCase(11,ExpectedResult =false)]
        [TestCase(17,ExpectedResult =true)]
        public bool CheckRecipeExist(int id)
        {
            bool ans = methods.RecipeExists(id);

            return ans;
        }

        [TestCase]
        public void AddRecipe()
        {
            Receipe r = new Receipe();
            r.RName = "Chicken Tikka";
            r.VNB = "Non Veg";
            r.State = "Kerala";
            r.Youtube = "https://www.youtube.com/embed/6No7g2GptXY";
            r.Ingredient = "Chicken,Tomato,Ginger";
            r.HTM = "Follow the steps in Youtube Link";
            r.Photo = "../RecipeImg/dbc01052-2005-462d-adca-e02d34e225bb_Card2";

           bool ans= methods.Insert(r);
            Assert.AreEqual(true, ans);
        }

        [TestCase]
        public void UpdateRecipe()
        {
            Receipe r = new Receipe();
            r.RId = 21;
            r.RName = "Chicken Tikka";
            r.VNB = "Non Veg";
            r.State = "TamilNadu";
            r.Youtube = "https://www.youtube.com/embed/6No7g2GptXY";
            r.Ingredient = "Chicken,Tomato,Ginger";
            r.HTM = "Follow the steps in Youtube Link";
            r.Photo = "../RecipeImg/dbc01052-2005-462d-adca-e02d34e225bb_Card2";

            bool ans = methods.Update(r);

            Assert.AreEqual(true, ans);
        }

        [TestCase]
        public void DeleteRecipe()
        {
           bool ans= methods.Delete(21);
           Assert.AreEqual(true, ans);
        }


    }
    
}
