using IdentityModel;
using Microsoft.EntityFrameworkCore;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;
using TennisCourt.Infra.Data.Context;
using TennisCourt.Infra.Data.Repositories.Base;

namespace TennisCourt.Infra.Data.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(TennisCourtContext context)
            : base(context)
        {
        }
    }
}
