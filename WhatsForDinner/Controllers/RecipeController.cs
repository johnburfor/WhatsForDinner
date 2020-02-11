using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsForDinner.Data;
using WhatsForDinner.Models;
using WhatsForDinner.ViewModels;
using PagedList;

namespace WhatsForDinner.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext context;

        public RecipeController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        //public IActionResult Index()
        //{
        //    List<Recipe> Recipes = context.Recipes.ToList();

        //    return View(Recipes);
        //}

        public async Task<ViewResult> IndexAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RatingSortParm = sortOrder == "Rating" ? "rating_desc" : "Rating";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var recipes = from r in context.Recipes
                           select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(r => r.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    recipes = recipes.OrderByDescending(r => r.Name);
                    break;
                case "Date":
                    recipes = recipes.OrderBy(r => r.Rating);
                    break;
                case "rating_desc":
                    recipes = recipes.OrderByDescending(r => r.Rating);
                    break;
                default:
                    recipes = recipes.OrderBy(r => r.Name);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Recipe>.CreateAsync(recipes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            AddRecipeViewModel addRecipeViewModel = new AddRecipeViewModel();
            return View(addRecipeViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddRecipeViewModel addRecipeViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the recipe to my existing recipes
                Recipe newRecipe = new Recipe
                {
                    Name = addRecipeViewModel.Name,
                    Ingredients = addRecipeViewModel.Ingredients,
                    Directions = addRecipeViewModel.Directions,
                    Rating = addRecipeViewModel.Rating
                };

                context.Recipes.Add(newRecipe);
                context.SaveChanges();

                return Redirect("/Recipe");
            }

            return View(addRecipeViewModel);
        }

        public IActionResult ViewRecipe(int id)
        {
            Recipe recipe = context.Recipes.Single(m => m.ID == id);

            List<Recipe> items = context
                .Recipes
               // .Include(item => item.Name)
               // .Include(item => item.Ingredients)
               // .Include(item => item.Directions)
               // .Include(item => itme.Rating)
                .Where(cm => cm.ID == id)
                .ToList();

            ViewRecipeViewModel viewRecipeViewModel = new ViewRecipeViewModel
            {
                Recipe = recipe,
                Items = items
            };

            return View(viewRecipeViewModel);
        }

        [Authorize]
        public IActionResult Remove()
        {
            ViewBag.title = "Remove Recipes";
            ViewBag.recipes = context.Recipes.ToList();
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Remove(int[] recipeIds)
        {
            foreach (int recipeId in recipeIds)
            {
                Recipe theRecipe = context.Recipes.Single(c => c.ID == recipeId);
                context.Recipes.Remove(theRecipe);
            }

            context.SaveChanges();

            return Redirect("/Recipe");
        }

        [HttpGet][Authorize]
        public IActionResult Edit(int id)
        {
            Recipe recipe = context.Recipes.Find(id);
            return View(recipe);
        }

        [HttpPost][Authorize]
        public IActionResult Edit(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                context.Entry(recipe).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recipe);
        }
    }
}