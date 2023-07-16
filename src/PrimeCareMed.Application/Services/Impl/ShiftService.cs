using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.DataAccess.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services.Impl
{
    public class ShiftService : IShiftService
    {
        private readonly IMapper _mapper;
        private readonly IShiftRepository _shiftRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOfficeRepository _officeRepository;
        private readonly ISessionRepository _sessionRepository;

        public ShiftService(IMapper mapper,
            IShiftRepository shiftRepository,
            IUserRepository userRepository,
            IOfficeRepository officeRepository,
            ISessionRepository sessionRepository
            )
        {
            _mapper = mapper;
            _shiftRepository = shiftRepository;
            _userRepository = userRepository;
            _officeRepository = officeRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<ShiftModel> AddAsync(ShiftModelForCreate createShiftModel)
        {
            var shift = new Shift();
            shift.Nurse = _userRepository.GetUserByIdAsync(createShiftModel.NurseId).Result;
            shift.Doctor = _userRepository.GetUserByIdAsync(createShiftModel.DoctorId).Result;
            shift.Office = _officeRepository.GetOfficeByIdAsync(createShiftModel.OfficeId).Result;
            await _shiftRepository.AddAsync(shift);
            return _mapper.Map<ShiftModel>(shift);
        }

        public List<string> GetShiftModelFields()
        {
            var shiftDto = new Shift();
            return shiftDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<ShiftModel> GetAllShiftsForOffice(Guid Id)
        {
            var shiftsFromDB = _shiftRepository.GetAllShiftsForOffice(Id).Result;

            return GetShiftsEnumerable(shiftsFromDB);
        }
        public IEnumerable<ShiftModel> GetShiftsEnumerable(IEnumerable<Shift> shiftsFromDB)
        {
            List<ShiftModel> shifts = new List<ShiftModel>();
            foreach (var shift in shiftsFromDB)
            {
                var shiftDto = _mapper.Map<ShiftModel>(shift);
                shiftDto.DoctorFirstName = shift.Doctor.FirstName;
                shiftDto.DoctorLastName = shift.Doctor.LastName;
                shiftDto.NurseFirstName = shift.Nurse.FirstName;
                shiftDto.NurseLastName = shift.Nurse.LastName;
                shiftDto.OfficeAddress = shift.Office.Address;
                shiftDto.OfficeCity = shift.Office.City;

                shifts.Add(shiftDto);

            }
            return shifts.AsEnumerable();
        }
        public IEnumerable<ShiftModel> GetAllShiftsForDoctor(string Id)
        {
            var shiftsFromDB = _shiftRepository.GetAllShiftsForDoctor(Id).Result;
            return GetShiftsEnumerable(shiftsFromDB);
        }

        public IEnumerable<ShiftModel> GetAllShifts()
        {
            var shiftsFromDB = _shiftRepository.GetAllShiftsAsync().Result;
            return GetShiftsEnumerable(shiftsFromDB);
        }
        public ShiftModel GetShiftById(string Id)
        {
            var shiftFromDB = _shiftRepository.GetAllShiftsAsync().Result.Where(r=>r.Id.ToString() == Id).FirstOrDefault();
            return _mapper.Map<ShiftModel>(shiftFromDB);
        }
        public IEnumerable<ShiftModel> GetAllShiftsForNurse(string Id)
        {
            var shiftsFromDB = _shiftRepository.GetAllShiftsForNurse(Id).Result;
            return GetShiftsEnumerable(shiftsFromDB);
        }
        public async Task DeleteShiftAsync(Guid Id)
        {
            await _shiftRepository.DeleteAsync(Id);
        }
        public bool CheckIfShiftExists(string officeId, string nurseId, string doctorId)
        {
            var shift = _shiftRepository.GetAllShiftsAsync().Result.Where(r => r.Office.Id.ToString() == officeId && r.Nurse.Id == nurseId && r.Doctor.Id == doctorId).FirstOrDefault();
            return shift != null ? true : false;
        }
        public IEnumerable<ShiftModel> GetAllAvailableShifts(IEnumerable<ShiftModel> shifts, string currentUserId, string currentUserRole)
        {
            var availableShifts = new List<ShiftModel>();

            var allShifts = new List<Shift>();
            var activeSessions = new List<string>();
            if (currentUserRole == "Doctor")
            {
                allShifts = _shiftRepository.GetAllShiftsForDoctor(currentUserId).Result.ToList();
                activeSessions = _sessionRepository.GetAllCurrentSessions().Result.Select(r => r.Shift.Nurse.Id).ToList();
                foreach (var shift in allShifts)
                {
                    if (!activeSessions.Contains(shift.Nurse.Id))
                    {
                        var shiftDto = _mapper.Map<ShiftModel>(shift);
                        shiftDto.DoctorFirstName = shift.Doctor.FirstName;
                        shiftDto.DoctorLastName = shift.Doctor.LastName;
                        shiftDto.NurseFirstName = shift.Nurse.FirstName;
                        shiftDto.NurseLastName = shift.Nurse.LastName;
                        shiftDto.OfficeAddress = shift.Office.Address;
                        shiftDto.OfficeCity = shift.Office.City;
                        availableShifts.Add(shiftDto);
                    }
                }
            }
            else if (currentUserRole == "Nurse")
            {
                allShifts = _shiftRepository.GetAllShiftsForNurse(currentUserId).Result.ToList();
                activeSessions = _sessionRepository.GetAllCurrentSessions().Result.Select(r => r.Shift.Doctor.Id).ToList();
                foreach (var shift in allShifts)
                {
                    if (!activeSessions.Contains(shift.Doctor.Id))
                    {
                        var shiftDto = _mapper.Map<ShiftModel>(shift);
                        shiftDto.DoctorFirstName = shift.Doctor.FirstName;
                        shiftDto.DoctorLastName = shift.Doctor.LastName;
                        shiftDto.NurseFirstName = shift.Nurse.FirstName;
                        shiftDto.NurseLastName = shift.Nurse.LastName;
                        shiftDto.OfficeAddress = shift.Office.Address;
                        shiftDto.OfficeCity = shift.Office.City;
                        availableShifts.Add(shiftDto);
                    }
                }
            }
            
            return availableShifts.AsEnumerable();
        }
    }
}
