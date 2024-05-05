using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Controllers;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Tests.ControllerTest
{
    public class ClubControllerTest
    {
        private ClubController _clubController;
        private  IClubRepository _clubRepository;
        private  IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;

        //best thing firest is to start mocking dependencies
        //we use mocking to return fake values  
        public ClubControllerTest()
        {
            //Dependencies
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();

            //SUT - system under test
            _clubController = new ClubController(_clubRepository, _photoService, _httpContextAccessor);

        }

        //u cant unit test static functions so u should be using static functions a lot because u cant unit test them

        [Fact]
        public void ClubController_Index_ReturnsSuccess()
        {
            //Arrange 
            var clubs = A.Fake<IEnumerable<Club>>();

            A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);
            //Act
            var result = _clubController.Index();
            //Assert - object check actions, test viewmodels and what it returns
            result.Should().BeOfType<Task<IActionResult>>();
            
        }

        [Fact]
        public void ClubController_Detail_ReturnsSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);

            //Act
            var result = _clubController.Detail(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    
    }
}
