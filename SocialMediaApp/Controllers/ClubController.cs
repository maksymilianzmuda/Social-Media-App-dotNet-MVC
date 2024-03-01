﻿//using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Models;
using SocialMediaApp.Repository;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        public ClubController( IClubRepository clubRepository, IPhotoService photoService)
        {
            
            _clubRepository = clubRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public async Task<IActionResult> Create()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                        Street = clubVM.Address.Street,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);  
        }

        public async Task<IActionResult>Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
                return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVm);
            }
            //to prevent an error
            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVm);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVm.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVm.Title,
                    Description = clubVm.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVm.AddressId,
                    Address = clubVm.Address
                };

                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVm);
            }
        }

    }
}
