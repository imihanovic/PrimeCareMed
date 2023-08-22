using AutoMapper;
using PrimeCareMed.Application.Models.Checkup;
using PrimeCareMed.Application.Models.HospitalCheckup;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class CheckupService : ICheckupService
    {
        private readonly IMapper _mapper;
        private readonly ICheckupRepository _checkupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHospitalRepository _hospitalRepository;

        public CheckupService(IMapper mapper,
            ICheckupRepository checkupRepository,
            IUserRepository userRepository,
            IHospitalRepository hospitalRepository
            )
        {
            _mapper = mapper;
            _checkupRepository = checkupRepository;
            _userRepository = userRepository;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<CheckupModel> AddAsync(CheckupModelForCreate createCheckupModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<CheckupModelForCreate, Checkup>();

            });
            var checkup = config.CreateMapper().Map<Checkup>(createCheckupModel);
            await _checkupRepository.AddAsync(checkup);
            return _mapper.Map<CheckupModel>(checkup);
        }

        public async Task<HospitalCheckup> AddHospitalCheckup(HospitalCheckupModelForCreate createHospitalCheckupModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<HospitalCheckupModelForCreate, HospitalCheckup>();

            });
            var hospitalCheckup = config.CreateMapper().Map<HospitalCheckup>(createHospitalCheckupModel);
            hospitalCheckup.Hospital = _hospitalRepository.GetHospitalByIdAsync(createHospitalCheckupModel.HospitalId.ToString()).Result;
            hospitalCheckup.Checkup = _checkupRepository.GetCheckupByIdAsync(createHospitalCheckupModel.CheckupId.ToString()).Result;
            await _checkupRepository.AddHospitalCheckupAsync(hospitalCheckup);
            return _mapper.Map<HospitalCheckup>(hospitalCheckup);
        }

        public List<string> GetCheckupModelFields()
        {
            var checkupDto = new CheckupModel();
            return checkupDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<CheckupModel> GetAllCheckups()
        {
            var checkupsFromDatabase = _checkupRepository.GetAllCheckupsAsync().Result;

            List<CheckupModel> checkups = new List<CheckupModel>();
            foreach (var checkup in checkupsFromDatabase)
            {
                var checkupDto = _mapper.Map<CheckupModel>(checkup);
                checkupDto.Name = checkup.Name;
                checkupDto.Description = checkup.Description;
                checkupDto.Duration = checkup.Duration;
                checkupDto.Preparation = checkup.Preparation;
                checkups.Add(checkupDto);

            }
            return checkups.AsEnumerable();
        }
        public IEnumerable<CheckupModel> GetAllCheckupsNotInHospital(Guid Id)
        {
            var checkupsFromDatabase = _checkupRepository.GetAllCheckupsAsync().Result;
            var hospitalCheckupsFromDB = _checkupRepository.GetAllHospitalCheckupsAsync(Id).Result.Select(r=>r.CheckupId);
            List<CheckupModel> checkupsNotInHospital = new List<CheckupModel>();
            foreach (var checkup in checkupsFromDatabase)
            {
                if (!hospitalCheckupsFromDB.Contains(checkup.Id))
                {
                    var checkupDto = _mapper.Map<CheckupModel>(checkup);
                    checkupsNotInHospital.Add(checkupDto);
                }
            }
            return checkupsNotInHospital.AsEnumerable();
        }
        public IEnumerable<CheckupModel> CheckupSorting(IEnumerable<CheckupModel> checkups, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Name":
                    return checkups.OrderBy(s => s.Name);
                case "NameDesc":
                    return checkups.OrderByDescending(s => s.Name);
                default:
                    return checkups.OrderBy(s => s.Name);
            }
        }
        public IEnumerable<CheckupModel> GetAllHospitalCheckups(Guid HospitalId)
        {
            var checkupsFromDatabase = _checkupRepository.GetAllHospitalCheckupsAsync(HospitalId).Result.Select(r=>r.Checkup);

            List<CheckupModel> checkups = new List<CheckupModel>();
            foreach (var checkup in checkupsFromDatabase)
            {
                var checkupDto = _mapper.Map<CheckupModel>(checkup);
                checkupDto.Name = checkup.Name;
                checkupDto.Description = checkup.Description;
                checkupDto.Duration = checkup.Duration;
                checkupDto.Preparation = checkup.Preparation;
                checkups.Add(checkupDto);

            }
            return checkups.AsEnumerable();
        }
        public IEnumerable<CheckupModel> CheckupSearch(IEnumerable<CheckupModel> checkups, string searchString)
        {
            IEnumerable<CheckupModel> searchedCheckups = checkups;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedCheckups = checkups.Where(s => s.Name.ToLower().Contains(searchStrTrim)
                                            || s.Description.ToLower().Contains(searchStrTrim)
                                            || s.Preparation.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedCheckups;
        }
        public Checkup EditCheckupAsync(CheckupModelForCreate checkupModel)
        {
            var checkup = _mapper.Map<Checkup>(checkupModel);
            return _checkupRepository.UpdateAsync(checkup).Result;
        }
        public CheckupModelForCreate GetCheckupById(string Id)
        {
            var checkupFromDB = _checkupRepository.GetCheckupByIdAsync(Id).Result;
            return _mapper.Map<CheckupModelForCreate>(checkupFromDB);
        }

        public async Task DeleteCheckupAsync(Guid Id)
        {
            await _checkupRepository.DeleteAsync(Id);
        }
    }
}
