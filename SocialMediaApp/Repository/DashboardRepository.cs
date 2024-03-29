﻿using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Models;

namespace SocialMediaApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly IHttpContextAccessor _contextAccessor;
        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>>GetAllUserClubs()
        {
            var curUser = _contextAccessor.HttpContext?.User.GetUserId();
            var userClubs =  _context.Clubs.Where(x => x.AppUser.Id == curUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _contextAccessor.HttpContext?.User.GetUserId();
            var userRaces =  _context.Races.Where(x => x.AppUser.Id == curUser);
            return userRaces.ToList();
        }

        public async Task<AppUser>GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

       public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();

        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
        public bool Save()
        {
          var saved =  _context.SaveChanges();
          return saved > 0? true: false ;
        }
    }
}
