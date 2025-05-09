using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Bookings.Mappings;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Domain;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingCommandHandler(
            IBookingRepository bookingRepository,
            IUnitOfWork unitOfWork
        )
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingDto> Handle(
            CreateBookingCommand request,
            CancellationToken cancellationToken
        )
        {
            if (await _bookingRepository.Exists(TimeSpan.Parse(request.BookingTime)))
            {
                throw new ConflictException($"Booking time {request.BookingTime} already exists.");
            }

            var booking = new Booking(request.Name, TimeSpan.Parse(request.BookingTime));
            await _bookingRepository.Create(booking);
            await _unitOfWork.SaveChanges(cancellationToken);
            return booking.ToDto();
        }
    }
}
