﻿using BattleCards.Data;
using BattleCards.ViewModels;
using BattleCards.ViewModels;
using Microsoft.EntityFrameworkCore;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        // GET /cards/add
       public HttpResponse Add()
        {
            return this.View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd()
        {
            var dbContext = new ApplicationDbContext();

            if (this.Request.FormData["name"].Length < 5)
            {
                return this.Error("Name should be at least 5 characters long.");
            }

            dbContext.Cards.Add(new Card
            {
                Attack = int.Parse(this.Request.FormData["attack"]),
                Health = int.Parse(this.Request.FormData["health"]),
                Description = this.Request.FormData["description"],
                Name = this.Request.FormData["name"],
                ImageUrl = this.Request.FormData["image"],
                Keyword = this.Request.FormData["keyword"],
            });
            dbContext.SaveChanges();

            return this.Redirect("/");
        }

        // /cards/all
        public HttpResponse All()
        {
            var db = new ApplicationDbContext();
            var cardsViewModel = db.Cards.Select(x => new CardViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Attack = x.Attack,
                Health = x.Health,
                ImageUrl = x.ImageUrl,
                Type = x.Keyword,
            }).ToList();

            return this.View(new AllCardsViewModel { Cards = cardsViewModel });
        }

        // /cards/collection
        public HttpResponse Collection()
        {
            var db = new ApplicationDbContext();
            var cardsViewModel = db.Cards.Select(x => new CardViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Attack = x.Attack,
                Health = x.Health,
                ImageUrl = x.ImageUrl,
                Type = x.Keyword,
            }).ToList();

            return this.View(new AllCardsViewModel { Cards = cardsViewModel });
        }

        [HttpPost("/Cards/SelectCard")]
        public HttpResponse DoSelectCard()
        {
            var health = int.Parse(this.Request.FormData["health"]);
            Console.WriteLine(health);
            var db = new ApplicationDbContext();
            var cards = db.Cards.Where(x => x.Health > health).Select(x => new CardViewModel()
            {
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Description = x.Description,
                Type = x.Keyword,
                Attack = x.Attack,
                Health = x.Health
            }).ToList();

            //var selectedCards = new List<CardViewModel>();
            //foreach (var card in cards)
            //{
            //    var currentCard = new CardViewModel()
            //    {
            //        Name = card.Name,
            //        ImageUrl = card.ImageUrl,
            //        Description = card.Description,
            //        Type = card.Keyword,
            //        Attack = card.Attack,
            //        Health = card.Health
            //    };
            //    selectedCards.Add(currentCard);
            //}
            
            return this.Redirect("/Cards/DoSelectedCard");
        }

        public HttpResponse SelectCard()
        {
            
            return this.View();
        }
    }
}
