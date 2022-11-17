using Cooking_WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Cooking_WebApi.Models
{

    public class Admin
    {
        [Key]
        public int Adminid { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class Login
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Profession { get; set; }
        public string City { get; set; }

        public  ICollection<FeedBack> feedBacks { get; set; }
        public  ICollection<Comments> Comments { get; set; }
    }

    public partial class Receipe
    {
        [Key]
        public int RId { get; set; }
        public string RName { get; set; }
        public string Photo { get; set; }
        public string Youtube { get; set; }
        public string Ingredient { get; set; }
        public string HTM { get; set; }
        public string VNB { get; set; }
        public string State { get; set; }

        public  ICollection<Comments> Comments { get; set; }

    }


    public  class FeedBack
    {
        [Key]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Msg { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Login logins { get; set; }
    }

    public class Logged
    {
        [Key]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Vnb { get; set; }
        public string Sname { get; set; }
    }
    public class Comments
    {
        [Key]
        public int CommentID { get; set; }
        public string Comment { get; set; }
        public DateTime DateofCreation { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Login Logins { get; set; }

        public int RId { get; set; }
        [ForeignKey("RId")]
        public Receipe Receipe { get; set; }
    }

    public class FoodReceipesEntities : DbContext
    {
        public FoodReceipesEntities() : base("FoodReceipesEntities")
        {
            Database.SetInitializer(new SeedMethod());
        }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Comments> Commentable { get; set; }
        public virtual DbSet<Logged> Loggeds { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Receipe> Receipes { get; set; }
    }

    public class SeedMethod : DropCreateDatabaseIfModelChanges<FoodReceipesEntities>
    {
        protected override void Seed(FoodReceipesEntities context)
        {
            List<Admin> admins = new List<Admin>();
            admins.Add(new Admin { Email = "admin@gmail.com",Username="Admin", Password = "Admin@123" });
            admins.Add(new Admin { Email = "abc@gmail.com",Username="Abc", Password = "ABC@123" });
            context.Admins.AddRange(admins);

            context.SaveChanges();
            base.Seed(context);
        }
    }

}