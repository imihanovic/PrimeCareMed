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
            //var reservation = _mapper.Map<Reservation>(createReservationModel);

            var reservation = new Reservation();
            reservation.NumberOfPersons = createReservationModel.NumberOfPerson;
            reservation.StartTime = createReservationModel.Date.ToUniversalTime();
            reservation.Tables = createReservationModel.Tables;
            reservation.Status = Core.Enums.ReservationStatus.Reserved;

            reservation.EndTime = createReservationModel.Date.AddHours(2).ToUniversalTime();
            await _reservationRepository.AddAsync(reservation);
            return reservation;
        }

    }
}
