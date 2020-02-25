using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WhatsForDinner.Models
{
    public class LoginClass
    { 
        public int ID { get; set; }
    
        [Required(ErrorMessage ="Please Enter Your Username!")]
        [Display(Name ="Enter Username:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password!")]
        [Display(Name = "Enter Password:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
