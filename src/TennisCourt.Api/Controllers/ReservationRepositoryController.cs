using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;

namespace TennisCourt.Api.Controllers
{
    [Route("Data/[controller]")]
    [ApiController]
    public class ReservationRepositoryController : ControllerBase
    {
        private readonly IReservationRepository _repository;

        public ReservationRepositoryController(IReservationRepository repository)
        {
            _repository= repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("GetAll")]
        public IAsyncEnumerable<Reservation> GetAll()
        {
           return _repository.GetAllQuery.AsAsyncEnumerable();
        }
    }
}