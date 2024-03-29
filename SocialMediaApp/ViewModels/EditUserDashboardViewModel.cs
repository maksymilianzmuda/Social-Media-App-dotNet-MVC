﻿namespace SocialMediaApp.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public int? Pace { get; set; }  
        public int? Distance { get; set; }
        public string? ProfileImageUrl { get;set; }
        public string City { get; set; }
        public string State {get; set; }
        public string? Street { get; set; }
        public IFormFile Image { get; set; }

    }
}
