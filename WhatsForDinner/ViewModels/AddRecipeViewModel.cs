using GenFu.ValueGenerators.Cooking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsForDinner.ViewModels
{
    public class AddRecipeViewModel
    {
        [Display(Name = "Recipe Name")]
        public string Name { get; set; }
        [Display(Name = "Ingredients")]
        public string Ingredients { get; set; }
        [Display(Name = "Directions")]
        public string Directions { get; set; }
        [Display(Name = "Rating")]
        public int Rating { get; set; }
    }
}
