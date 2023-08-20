using AutoMapper;
using PrimeCareMed.Application.Models.Appointment;
using PrimeCareMed.Application.Models.Patient;
using PrimeCareMed.Application.Models.Shift;
using PrimeCareMed.Application.Models.User;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class ShiftService : IShiftService
    {
        private readonly IMapper _mapper;
        private readonly IShiftRepository _shiftRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOfficeRepository _officeRepository;
        private readonly IUserService _userService;

        public ShiftService(IMapper mapper,
            IShiftRepository shiftRepository,
            IUserRepository userRepository,
            IOfficeRepository officeRepository,
            IUserService userService
            )
        {
            _mapper = mapper;
            _shiftRepository = shiftRepository;
            _userRepository = userRepository;
            _officeRepository = officeRepository;
            _userService = userService;
        }

        public async Task<ShiftModel> AddAsync(ShiftModelForCreate createShiftModel)
        {
            var shift = new Shift();
            shift.Nurse = _userRepository.GetUserByIdAsync(createShiftModel.NurseId).Result;
            shift.Doctor = _userRepository.GetUserByIdAsync(createShiftModel.DoctorId).Result;
            shift.Office = _officeRepository.GetOfficeByIdAsync(createShiftModel.OfficeId).Result;
            shift.ShiftStartTime = DateTime.Now.ToUniversalTime().AddHours(2);
            await _shiftRepository.AddAsync(shift);
            return _mapper.Map<ShiftModel>(shift);
        }
        public IEnumerable<ListUsersModel> GetAllAvailableDoctors()
        {
            var doctorsFromDB = _userService.GetAllUsers().Where(r => r.UserRole == "Doctor").ToList();
            var availableDoctors = new List<ListUsersModel>();
            foreach (var user in doctorsFromDB)
            {
                try
                {
                    var doctorsShift = _shiftRepository.CheckIfOpenShiftExistsForDoctor(user.Id.ToString());
                    if (doctorsShift is null)
                    {
                        availableDoctors.Add(user);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return availableDoctors.AsEnumerable();
        }

        public IEnumerable<ListUsersModel> GetAllAvailableNurses()
        {
            var nursesFromDB = _userService.GetAllUsers().Where(r => r.UserRole == "Nurse").ToList();
            var availableNurses = new List<ListUsersModel>();
            foreach (var user in nursesFromDB)
            {
                try
                {
                    var doctorsShift = _shiftRepository.CheckIfOpenShiftExistsForNurse(user.Id.ToString());
                    if (doctorsShift is null)
                    {
                        availableNurses.Add(user);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return availableNurses.AsEnumerable();
        }
        public Shift EditShiftAsync(Shift shift)
        {
            shift.ShiftEndTime = DateTime.Now.ToUniversalTime();
            return _shiftRepository.UpdateAsync(shift).Result;
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

        public IEnumerable<ShiftModel> ShiftSorting(IEnumerable<ShiftModel> shifts, string sortOrder)
        {
            IEnumerable<ShiftModel> sortedShifts = shifts;
            switch (sortOrder)
            {
                case "DoctorLastName":
                    return shifts.OrderBy(s => s.DoctorLastName);
                case "DoctorLastNameDesc":
                    return shifts = shifts.OrderByDescending(s => s.DoctorLastName);
                case "NurseLastName":
                    return shifts.OrderBy(s => s.NurseLastName);
                case "NurseLastNameDesc":
                    return shifts.OrderByDescending(s => s.NurseLastName);
                case "ShiftStartTime":
                    return shifts.OrderBy(s => s.ShiftStartTime);
                case "ShiftStartTimeDesc":
                    return shifts.OrderByDescending(s => s.ShiftStartTime);
                case "ShiftEndTime":
                    return shifts.OrderBy(s => s.ShiftEndTime);
                case "ShiftEndTimeDesc":
                    return shifts.OrderByDescending(s => s.ShiftEndTime);
                default:
                    return shifts.OrderByDescending(s => s.ShiftEndTime);
            }
        }
        public IEnumerable<ShiftModel> ShiftFilterDate(IEnumerable<ShiftModel> shifts, string date)
        {
            IEnumerable<ShiftModel> filteredShifts = shifts;
            if (!String.IsNullOrEmpty(date))
            {
                var dateTrim = date.ToLower().Trim();
                filteredShifts = shifts.Where(s => s.ShiftStartTime.ToString("yyyy-MM-dd").ToLower().Contains(dateTrim));
            }
            return filteredShifts;
        }
        public IEnumerable<ShiftModel> ShiftSearch(IEnumerable<ShiftModel> shifts, string searchString)
        {
            IEnumerable<ShiftModel> searchedShifts = shifts;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedShifts = shifts.Where(s => s.OfficeCity.ToLower().Contains(searchStrTrim)
                                            || s.NurseLastName.ToLower().Contains(searchStrTrim)
                                            || s.NurseFirstName.ToLower().Contains(searchStrTrim)
                                            || s.DoctorLastName.ToLower().Contains(searchStrTrim)
                                            || s.DoctorFirstName.ToLower().Contains(searchStrTrim)
                                            || s.OfficeName.ToString().ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedShifts;
        }
    }
}
