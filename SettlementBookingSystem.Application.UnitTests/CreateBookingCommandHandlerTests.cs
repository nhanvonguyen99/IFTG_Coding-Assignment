using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Domain;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly IRequestHandler<CreateBookingCommand, BookingDto> _handler;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public CreateBookingCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateBookingCommandHandler(
                _bookingRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateBooking_WhenBookingTimeIsAvailable()
        {
            // Arrange
            var command = new CreateBookingCommand { Name = "Test Booking", BookingTime = "10:00" };

            _bookingRepositoryMock
                .Setup(x => x.CheckTimeOverlap(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            _bookingRepositoryMock.Verify(x => x.Create(It.IsAny<Booking>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChanges(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenBookingTimeIsNotAvailable()
        {
            // Arrange
            var command = new CreateBookingCommand { Name = "Test Booking", BookingTime = "10:00" };

            _bookingRepositoryMock
                .Setup(x => x.CheckTimeOverlap(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage($"Booking time {command.BookingTime} already exists.");
            _bookingRepositoryMock.Verify(x => x.Create(It.IsAny<Booking>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChanges(CancellationToken.None), Times.Never);
        }
    }
}
