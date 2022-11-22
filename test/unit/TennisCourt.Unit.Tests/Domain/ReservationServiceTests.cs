using Bogus;
using Moq;
using System;
using System.Threading.Tasks;
using TennisCourt.Domain.Enums;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;
using TennisCourt.Domain.Services;
using Xunit;

namespace TennisCourt.Unit.Tests.Domain
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _repository;

        public ReservationServiceTests()
        {
            _repository = new Mock<IReservationRepository>();
        }

        [Fact]
        public async Task RescheduleReservationAsync_WhenReservationDateHasValue_ExpectRescheduledReservation()
        {
            // Assamble
            var reservation = new Faker<Reservation>()
                                        .RuleFor(o => o.Id, f => f.Random.Guid())
                                        .RuleFor(o => o.Value, f => f.Random.Decimal())
                                        .RuleFor(o => o.ReservationStatus, ReservationStatusEnum.Active)
                                        .Generate();

            var reservationInput = new Faker<Reservation>()
                                        .RuleFor(o => o.Id, reservation.Id)
                                        .RuleFor(o => o.ReservationDate, f => f.Date.Future())
                                        .Generate();

            _repository.Setup(x => x.GetByIdAsync(reservation.Id)).ReturnsAsync(reservation);
            var service = new ReservationService(_repository.Object);

            // Act
            var actualResult = await service.RescheduleReservationAsync(reservationInput);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(reservation.Id, actualResult.Id);
            Assert.Equal(ReservationStatusEnum.Active, actualResult.ReservationStatus);
            Assert.Equal(0m, actualResult.RefundValue);
            Assert.Equal(reservationInput.ReservationDate, actualResult.ReservationDate);
        }

        [Fact]
        public async Task CancelReservationAsync_WhenValidId_ExpectCancelledReservation()
        {
            // Assamble
            var reservation = new Faker<Reservation>()
                                        .RuleFor(o => o.Id, f => f.Random.Guid())
                                        .RuleFor(o => o.Value, f => f.Random.Decimal())
                                        .RuleFor(o => o.ReservationStatus, f => ReservationStatusEnum.Active)
                                        .RuleFor(o => o.ReservationDate, f => f.Date.Future())
                                        .Generate();

            var expectedRefund = reservation.Value;

            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(reservation);
            var service = new ReservationService(_repository.Object);

            // Act
            var actualResult = await service.CancelReservationAsync(reservation);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Null(actualResult.ReservationDate);
            Assert.Equal(reservation.Id, actualResult.Id);
            Assert.Equal(ReservationStatusEnum.Cancelled, actualResult.ReservationStatus);
            Assert.Equal(0m, actualResult.Value);
            Assert.Equal(expectedRefund, actualResult.RefundValue);
        }

        [Fact]
        public async Task GetReservationAsync_WhenValidId_ExpectValidReservation()
        {
            // Assamble
            var expectedResult = new Faker<Reservation>()
                                        .RuleFor(o => o.Id, f => f.Random.Guid())
                                        .RuleFor(o => o.Value, f => f.Random.Decimal())
                                        .RuleFor(o => o.ReservationStatus, f => f.Random.Enum<ReservationStatusEnum>())
                                        .RuleFor(o => o.ReservationDate, f => f.Date.Future())
                                        .Generate();
            
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expectedResult);
            var service = new ReservationService(_repository.Object);

            // Act
            var actualResult = await service.GetReservationAsync(Guid.NewGuid());

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ProcessReservationAsync_WhenValidReservation_ExpectValidReservation()
        {
            // Assamble
            var reservationInput = new Faker<Reservation>()
                              .RuleFor(o => o.Id, f => f.Random.Guid())
                              .RuleFor(o => o.Value, f => f.Random.Decimal())
                              .RuleFor(o => o.ReservationDate, f => f.Date.Future())
                              .Generate();

            var reservationDb = new Faker<Reservation>()
                            .RuleFor(o => o.Id, reservationInput.Id)
                            .RuleFor(o => o.Value, reservationInput.Value)
                            .RuleFor(o => o.ReservationDate, reservationInput.ReservationDate)
                            .Generate();

            _repository.Setup(x => x.AddAsync(reservationInput)).ReturnsAsync(reservationInput);
            var service = new ReservationService(_repository.Object);

            // Act
            var actualResult = await service.ProcessReservationAsync(reservationInput);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(ReservationStatusEnum.Active, actualResult.ReservationStatus);
        }
    }
}