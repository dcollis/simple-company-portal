using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.Services;
using Portal.Services.Blob;
using Portal.Website.Model;

namespace Portal.Website.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleService _peopleService;
        private readonly ProfilePhotoService _photoService;

        public PeopleController(PeopleService peopleService, ProfilePhotoService photoService)
        {
            this._peopleService = peopleService;
            _photoService = photoService;
        }

        [HttpGet("getpeople")]
        public async Task<PeopleVm> GetPeople()
        {
            var vm = await _peopleService.GetPeople();
            return new PeopleVm(vm.users, vm.nextLink);
        }

        [HttpGet("getmorepeople")]
        public async Task<PeopleVm> GetMorePeople(string url)
        {
            var vm = await _peopleService.GetMorePeople(url);

            return new PeopleVm(vm.users, vm.nextLink);
        }

        [HttpGet("getfilteredpeople")]
        public async Task<PeopleVm> GetPeople(string filter)
        {
            var vm = await _peopleService.GetPeople(filter);
            return new PeopleVm(vm.users, vm.nextLink);
        }

        [HttpGet("profilephoto")]
        public async Task<IActionResult> GetPhoto(string emailAddress)
        {
            var photoStream = await _photoService.GetPhoto(emailAddress);
            if (photoStream == null) return new EmptyResult();
            photoStream.Position = 0;

            var filename = emailAddress;
            if (filename.Contains("/"))
            {
                filename = filename.Substring(filename.LastIndexOf('/') + 1);
            }
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = filename + ".jpg",
                Inline = false

            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(photoStream, "image/jpeg", filename + ".jpg");
        }
    }
}