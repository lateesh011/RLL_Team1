using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Cooking_App.Models
{
    public class Img
    {
        [Key]
        public int RId { get; set; }
        [DisplayName("Receipe Name")]
        public string RName { get; set; }
        [DisplayName("Add Image")]
        public string Photo { get; set; }
        [DisplayName("Youtube Link")]
        public string Youtube { get; set; }
        [DisplayName("Ingredients")]
        public string Ingredient { get; set; }
        [DisplayName("How to Make")]
        public string HTM { get; set; }
        public string VNB { get; set; }
        public string State { get; set; }
    

        public HttpPostedFileBase ImageFile { get; set; }
    }
}