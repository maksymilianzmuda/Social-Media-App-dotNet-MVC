﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Models;
using SocialMediaApp.Repository;
using SocialMediaApp.Services;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _photoService = photoService;
            _raceRepository = raceRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                        Street = raceVM.Address.Street,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null)
                return View("Error");
            var raceVm = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel raceVm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVm);
            }
            //to prevent an error
            var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);
            if (userRace != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVm);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceVm.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceVm.Title,
                    Description = raceVm.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVm.AddressId,
                    Address = raceVm.Address
                };

                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVm);
            }
        }
    }
}
