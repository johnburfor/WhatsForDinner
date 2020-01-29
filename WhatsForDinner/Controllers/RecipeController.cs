﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsForDinner.Data;
using WhatsForDinner.Models;
using WhatsForDinner.ViewModels;

namespace WhatsForDinner.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext context;

        public RecipeController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }
        public IActionResult Index()
        {
            List<Recipe> Recipes = context.Recipes.ToList();

            return View(Recipes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddRecipeViewModel addRecipeViewModel = new AddRecipeViewModel();
            return View(addRecipeViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddRecipeViewModel addRecipeViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the recipe to my existing recipes
                Recipe newRecipe = new Recipe
                {
                    Name = addRecipeViewModel.Name,
                    Ingredients = addRecipeViewModel.Ingredients,
                    Directions = addRecipeViewModel.Directions
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
                .Where(cm => cm.ID == id)
                .ToList();

            ViewRecipeViewModel viewRecipeViewModel = new ViewRecipeViewModel
            {
                Recipe = recipe,
                Items = items
            };

            return View(viewRecipeViewModel);
        }
    }
}