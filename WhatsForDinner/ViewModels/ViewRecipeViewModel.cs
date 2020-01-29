using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatsForDinner.Models;

namespace WhatsForDinner.ViewModels
{
    public class ViewRecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public IList<Recipe> Items { get; set; }
    }
}
