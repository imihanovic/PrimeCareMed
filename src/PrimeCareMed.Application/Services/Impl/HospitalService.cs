using AutoMapper;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Hospital;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class HospitalService : IHospitalService
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IUserRepository _userRepository;

        public HospitalService(IMapper mapper,
            IHospitalRepository hospitalRepository,
            IUserRepository userRepository
            )
        {
            _mapper = mapper;
            _hospitalRepository = hospitalRepository;
            _userRepository = userRepository;
        }

        public async Task<HospitalModel> AddAsync(HospitalModelForCreate createHospitalModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<HospitalModelForCreate, Hospital>();

            });
            var hospital = config.CreateMapper().Map<Hospital>(createHospitalModel);
            await _hospitalRepository.AddAsync(hospital);
            return _mapper.Map<HospitalModel>(hospital);
        }

        public List<string> GetHospitalModelFields()
        {
            var hospitalDto = new HospitalModel();
            return hospitalDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<HospitalModel> GetAllHospitals()
        {
            var hospitalsFromDatabase = _hospitalRepository.GetAllHospitalsAsync().Result;

            List<HospitalModel> hospitals = new List<HospitalModel>();
            foreach (var hospital in hospitalsFromDatabase)
            {
                var hospitalDto = _mapper.Map<HospitalModel>(hospital);
                hospitalDto.Name = hospital.Name;
                hospitalDto.Address = hospital.Address;
                hospitalDto.City = hospital.City;
                hospitals.Add(hospitalDto);

            }
            return hospitals.AsEnumerable();
        }
        public IEnumerable<HospitalModel> HospitalSorting(IEnumerable<HospitalModel> hospitals, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Name":
                    return hospitals.OrderBy(s => s.Name);
                case "NameDesc":
                    return hospitals.OrderByDescending(s => s.Name);
                case "City":
                    return hospitals.OrderBy(s => s.City);
                case "CityDesc":
                    return hospitals.OrderByDescending(s => s.City);
                default:
                    return hospitals.OrderBy(s => s.Name);
            }
        }

        public IEnumerable<HospitalModel> HospitalSearch(IEnumerable<HospitalModel> hospitals, string searchString)
        {
            IEnumerable<HospitalModel> searchedHospitals = hospitals;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedHospitals = hospitals.Where(s => s.Name.ToLower().Contains(searchStrTrim)
                                            || s.Address.ToLower().Contains(searchStrTrim)
                                            || s.City.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedHospitals;
        }
        public Hospital EditHospitalAsync(HospitalModelForCreate hospitalModel)
        {
            var hospital = _mapper.Map<Hospital>(hospitalModel);
            return _hospitalRepository.UpdateAsync(hospital).Result;
        }
        public HospitalModelForCreate GetHospitalById(string Id)
        {
            var hospitalFromDB = _hospitalRepository.GetHospitalByIdAsync(Id).Result;
            return _mapper.Map<HospitalModelForCreate>(hospitalFromDB);
        }
        public HospitalModel GetHospitalModelById(string Id)
        {
            var hospitalFromDB = _hospitalRepository.GetHospitalByIdAsync(Id).Result;
            return _mapper.Map<HospitalModel>(hospitalFromDB);
        }

        public async Task DeleteHospitalAsync(Guid Id)
        {
            await _hospitalRepository.DeleteAsync(Id);
        }
    }
}
