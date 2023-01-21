using KarcagS.Common.Demo.DTOs;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenderController
    {
        private readonly IGenderService genderService;

        public GenderController(IGenderService genderService)
        {
            this.genderService = genderService;
        }

        [HttpGet]
        public List<GenderDTO> GetAll([FromQuery] string? orderBy, [FromQuery] string? orderDirection)
        {
            if (ObjectHelper.IsNotNull(orderBy) && ObjectHelper.IsNotNull(orderDirection))
            {
                return genderService.GetMappedOrderedList<GenderDTO>((x) => x.Id > 1, orderBy, orderDirection).ToList();
            }

            return genderService.GetAllMapped<GenderDTO>().ToList();
        }
    }
}
