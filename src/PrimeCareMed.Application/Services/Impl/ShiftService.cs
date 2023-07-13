using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;
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
        private readonly IShiftRepository _shiftSessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOfficeRepository _officeRepository;

        public ShiftService(IMapper mapper,
            IShiftRepository shiftSessionRepository,
            IUserRepository userRepository,
            IOfficeRepository officeRepository
            )
        {
            _mapper = mapper;
            _shiftSessionRepository = shiftSessionRepository;
            _userRepository = userRepository;
            _officeRepository = officeRepository;
        }

        public async Task<ShiftModel> AddAsync(ShiftModelForCreate createShiftModel)
        {
            var shiftSession = new Shift();
            shiftSession.Nurse = _userRepository.GetUserByIdAsync(createShiftModel.NurseId).Result;
            shiftSession.Doctor = _userRepository.GetUserByIdAsync(createShiftModel.DoctorId).Result;
            shiftSession.Office = _officeRepository.GetOfficeByIdAsync(createShiftModel.OfficeId).Result;
            await _shiftSessionRepository.AddAsync(shiftSession);
            return _mapper.Map<ShiftModel>(shiftSession);
        }

        public List<string> GetShiftModelFields()
        {
            var shiftSessionDto = new Shift();
            return shiftSessionDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<ShiftModel> GetAllShiftsForOffice(Guid Id)
        {
            var shiftsFromDB = _shiftSessionRepository.GetAllShiftsForOffice(Id).Result;

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
            var shiftsFromDB = _shiftSessionRepository.GetAllShiftsForDoctor(Id).Result;
            return GetShiftsEnumerable(shiftsFromDB);
        }

        public IEnumerable<ShiftModel> GetAllShifts()
        {
            var shiftsFromDB = _shiftSessionRepository.GetAllShiftsAsync().Result;
            return GetShiftsEnumerable(shiftsFromDB);
        }
        public ShiftModel GetShiftById(string Id)
        {
            var shiftFromDB = _shiftSessionRepository.GetAllShiftsAsync().Result.Where(r=>r.Id.ToString() == Id).FirstOrDefault();
            return _mapper.Map<ShiftModel>(shiftFromDB);
        }
        public IEnumerable<ShiftModel> GetAllShiftsForNurse(string Id)
        {
            var shiftsFromDB = _shiftSessionRepository.GetAllShiftsForNurse(Id).Result;
            return GetShiftsEnumerable(shiftsFromDB);
        }
        public async Task DeleteMedicineAsync(Guid Id)
        {
            await _shiftSessionRepository.DeleteAsync(Id);
        }
        public async Task<bool> CheckIfShiftExists(string officeId, string nurseId, string doctorId)
        {
            var shift = _shiftSessionRepository.GetAllShiftsAsync().Result.Where(r => r.Office.Id.ToString() == officeId && r.Nurse.Id == nurseId && r.Doctor.Id == doctorId).FirstOrDefault();
            return shift != null ? true : false;
        }
    }
}
