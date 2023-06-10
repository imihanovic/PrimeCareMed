using AutoMapper;
using BookIt.Application.Models.Reservation;
using BookIt.Application.Models.Restaurant;
using BookIt.Application.Models.Table;
using BookIt.Application.Models.User;
using BookIt.Core.Entities;
using BookIt.DataAccess.Repositories;
using BookIt.DataAccess.Repositories.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace BookIt.Application.Services.Impl
{
    public class ReservationService : IReservationService
    {
        private readonly IMapper _mapper;
        private readonly IReservationRepository _reservationRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public ReservationService(IMapper mapper,
            IReservationRepository reservationRepository,
            ITableRepository tableRepository,
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository)
        {
            _mapper = mapper;
            _reservationRepository = reservationRepository;
            _tableRepository = tableRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
        }
        public async Task<Reservation> AddAsync(ReservationModelForCreate createReservationModel)
        {
            var reservation = new Reservation();

            reservation.Customer = createReservationModel.Customer;
            reservation.NumberOfPersons = createReservationModel.NumberOfPerson;
            reservation.StartTime = createReservationModel.Date.ToUniversalTime();
            reservation.Tables = createReservationModel.Tables;
            reservation.Status = createReservationModel.Status;
            reservation.ReservationDetails = createReservationModel.ReservationDetails;

            reservation.EndTime = createReservationModel.Date.AddHours(2).ToUniversalTime();
            await _reservationRepository.AddAsync(reservation);
            return reservation;
        }

        public IEnumerable<ReservationModel> GetAllReservations()
        {
            var reservationsFromDatabase = _reservationRepository.GetAllReservationsAsync().Result;
            List<ReservationModel> reservations = new List<ReservationModel>();
            foreach (var reservation in reservationsFromDatabase)
            {
                var reservationDto = _mapper.Map<ReservationModel>(reservation);
                reservationDto.StartTime = DateTime.Parse(reservationDto.StartTime).ToString("yyyy-MM-dd  HH:mm");
                reservations.Add(reservationDto);
            }
            return reservations.AsEnumerable();
        }
        public IEnumerable<ReservationModel> GetAllReservationsForCustomer(string customerId)
        {
            var reservationsFromDatabase = _reservationRepository.GetAllReservationsAsync().Result.Where(r => (r.Customer != null && r.Customer.Id == customerId));
            List<ReservationModel> reservations = new List<ReservationModel>();
            foreach (var reservation in reservationsFromDatabase)
            {
                var reservationDto = _mapper.Map<ReservationModel>(reservation);
                reservationDto.StartTime = DateTime.Parse(reservationDto.StartTime).ToString("yyyy-MM-dd  HH:mm");
                reservations.Add(reservationDto);
            }
            return reservations.AsEnumerable();
        }

        public IEnumerable<ReservationModel> GetAllReservationsForManager(string managerId)
        {
            var currentUserTables = _restaurantRepository.GetRestaurantByManagerIdAsync(managerId).Result.Tables.ToList();
            List<ReservationModel> reservations = new List<ReservationModel>();
            foreach (var table in currentUserTables)
            {
                var managersReservation = _reservationRepository.GetAllReservationsAsync().Result.Where(r => r.Tables.Contains(table));

                foreach (var reservation in managersReservation)
                {
                    var reservationDto = _mapper.Map<ReservationModel>(reservation);
                    reservationDto.StartTime = DateTime.Parse(reservationDto.StartTime).ToString("yyyy-MM-dd  HH:mm");
                    reservations.Add(reservationDto);
                }
            }

            return reservations.DistinctBy(r => r.Id).AsEnumerable();
        }

        public Reservation EditReservationAsync(ReservationModelForUpdate reservationModel)
        {
            var reservation = _mapper.Map<Reservation>(reservationModel);
            return _reservationRepository.UpdateAsync(reservation).Result;
        }

        public List<string> GetReservationModelFields()
        {
            var reservationDto = new ReservationModel();
            return reservationDto.GetType().GetProperties().Where( x => x.Name != "Id" && x.Name != "EndTime" && x.Name != "Customer").Select(x => x.Name).ToList();
        }

        public IEnumerable<ReservationModel> ReservationSorting(IEnumerable<ReservationModel> reservations, string sortOrder)
        {
            IEnumerable<ReservationModel> sortedReservations = reservations;
            switch (sortOrder)
            {
                case "NumberOfPersons":
                    return reservations.OrderBy(r => r.NumberOfPersons);
                case "NumberOfPersonsDesc":
                    return reservations.OrderByDescending(r => r.NumberOfPersons);
                case "StartTime":
                    return reservations.OrderBy(r => r.StartTime);
                case "StartTimeDesc":
                    return reservations.OrderByDescending(r => r.StartTime);
                default:
                    return reservations.OrderByDescending(r => r.StartTime);
            }
        }
        public IEnumerable<ReservationModel> ReservationFilterByStatus(IEnumerable<ReservationModel> reservations, string status)
        {
            IEnumerable<ReservationModel> filteredReservations= reservations;
            if (!String.IsNullOrEmpty(status))
            {
                var statusTrim = status.ToLower().Trim();
                filteredReservations = reservations.Where(t => t.Status.ToLower() == statusTrim);
            }
            return filteredReservations;
        }

         public IEnumerable<ReservationModel> ReservationFilterByReservationDate(IEnumerable<ReservationModel> reservations, string reservationDate)
         {
            IEnumerable<ReservationModel> filteredReservations= reservations;
            if (!String.IsNullOrEmpty(reservationDate))
            {
                var reservationDateTrim = reservationDate.ToLower().Trim();
                filteredReservations = reservations.Where(t => t.StartTime.ToLower().Contains(reservationDateTrim));
            }
            return filteredReservations;
         }

        public IEnumerable<ReservationModel> ReservationFilterByReservationTime(IEnumerable<ReservationModel> reservations, string reservationTime)
        {
            IEnumerable<ReservationModel> filteredReservations = reservations;
            if (!String.IsNullOrEmpty(reservationTime))
            {
                var reservationTimeTrim = reservationTime.ToLower().Trim();
                filteredReservations = reservations.Where(t => t.StartTime.ToLower().Contains(reservationTimeTrim));
            }
            return filteredReservations;
        }

        //public IEnumerable<ReservationModel> GetAllReservationsByManagerIdAsync(IEnumerable<ReservationModel> reservations, string managerId)
        //{
        //    var reservationsFromDB = _reservationRepository.GetAllReservationsAsync().Result.Where(r => r.Tables.First().Restaurant.Manager.Id == managerId);
        //    var returnedValues = new List<ReservationModel>();
        //    foreach (var reservation in reservationsFromDB)
        //    {
        //        returnedValues.Add(_mapper.Map<ReservationModel>(reservation));
        //    }
        //    reservations = returnedValues;
        //    return reservations;
        //}

        public IEnumerable<ReservationModel> ReservationSearch(IEnumerable<ReservationModel> reservations, string searchString)
        {
            IEnumerable<ReservationModel> searchedReservations = reservations;
            Console.WriteLine($"{reservations.Select(r => r.ReservationDetails).ToList().Count}");
            foreach(var a in reservations.Select(r=> r.ReservationDetails))
            {
                Console.WriteLine(a);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower();
                searchedReservations = reservations.Where(s => s.NumberOfPersons == searchStrTrim
                                        || (!string.IsNullOrWhiteSpace(s.ReservationDetails) && s.ReservationDetails.ToLower().Contains(searchStrTrim)));
            }
            return searchedReservations;
        }
    }
}
