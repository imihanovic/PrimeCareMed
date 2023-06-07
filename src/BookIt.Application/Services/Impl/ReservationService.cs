using AutoMapper;
using BookIt.Application.Models.Reservation;
using BookIt.Application.Models.Restaurant;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories;
using System;

namespace BookIt.Application.Services.Impl
{
    public class ReservationService : IReservationService
    {
        private readonly IMapper _mapper;
        private readonly IReservationRepository _reservationRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IUserRepository _userRepository;

        public ReservationService(IMapper mapper,
            IReservationRepository reservationRepository,
            ITableRepository tableRepository,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _reservationRepository = reservationRepository;
            _tableRepository = tableRepository;
            _userRepository = userRepository;
        }
        public async Task<Reservation> AddAsync(ReservationModelForCreate createReservationModel)
        {
            var reservation = _mapper.Map<Reservation>(createReservationModel);

            reservation.StartTime = DateTime.UtcNow;
            reservation.EndTime = DateTime.UtcNow.AddHours(1).AddMinutes(30);
            reservation.Tables.Add(_tableRepository.GetAllTablesAsync().Result.First());
            await _reservationRepository.AddAsync(reservation);
            return reservation;
        }

    }
}
