using AutoMapper;
using Bogus;
using Moq;
using System.Threading.Tasks;
using TennisCourt.Application.AutoMapper;
using TennisCourt.Application.DTO;
using TennisCourt.Application.Services;
using TennisCourt.Domain.Enums;
using TennisCourt.Domain.Interfaces.Services;
using TennisCourt.Domain.Models;
using Xunit;

namespace TennisCourt.Unit.Tests.Application
{
    public class ReservationAppServiceTests
    {
        private readonly Mock<IReservationService> _reservationService;
        private readonly IMapper _mapper;

        public ReservationAppServiceTests()
        {
            _reservationService = new Mock<IReservationService>();

            var mapperConfig = new MapperConfiguration(config => { config.AddProfile<DtoToDomainMappingProfile>(); });
            _mapper = new Mapper(mapperConfig);
        }

        [Fact]
        public async Task CancelReservationAsync_WhenCancelling_ExpectCancelledReservation()
        {
            // Assamble
            var reservation = new Faker<Reservation>()
                                        .RuleFor(o => o.Id, f => f.Random.Guid())
                                        .RuleFor(o => o.RefundValue, f => f.Random.Decimal())
                                        .RuleFor(o => o.ReservationStatus, ReservationStatusEnum.Cancelled)
                                        .Generate();

            var reservationInput = new Faker<CancelReservationDto>()
                                        .RuleFor(o => o.Id, reservation.Id)
                                        .Generate();

            _reservationService.Setup(x => x.CancelReservationAsync(It.IsAny<Reservation>()))
                               .ReturnsAsync(reservation);

            var service = new ReservationAppService(_reservationService.Object, _mapper);

            // Act
            var actualResult = await service.CancelReservationAsync(reservationInput);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Null(actualResult.ReservationDate);
            Assert.Equal(reservation.Id, actualResult.Id);
            Assert.Equal(ReservationStatusEnum.Cancelled, actualResult.ReservationStatus);
            Assert.Equal(0m, actualResult.Value);
            Assert.Equal(reservation.RefundValue, actualResult.RefundValue);
        }

        [Fact]
        public async Task GetReservationAsync_WhenValidId_ExpectReservation()
        {
            // Assamble
            var reservation = new Faker<Reservation>()
                                        .RuleFor(o => o.Id, f => f.Random.Guid())
                                        .RuleFor(o => o.RefundValue, f => f.Random.Decimal())
                                        .RuleFor(o => o.ReservationStatus, f => f.Random.Enum<ReservationStatusEnum>())
                                        .Generate();

            _reservationService.Setup(x => x.GetReservationAsync(reservation.Id))
                               .ReturnsAsync(reservation);

            var service = new ReservationAppService(_reservationService.Object, _mapper);

            // Act
            var actualResult = await service.GetReservationAsync(reservation.Id);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(reservation.Id, actualResult.Id);
            Assert.Equal(reservation.RefundValue, actualResult.RefundValue);
            Assert.Equal(reservation.ReservationStatus, actualResult.ReservationStatus);
        }
    }
}
