using AutoMapper;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class VaccineService : IVaccineService
    {
        private readonly IMapper _mapper;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IUserRepository _userRepository;

        public VaccineService(IMapper mapper,
            IVaccineRepository vaccineRepository,
            IUserRepository userRepository
            )
        {
            _mapper = mapper;
            _vaccineRepository = vaccineRepository;
            _userRepository = userRepository;
        }

        public async Task<VaccineModel> AddAsync(VaccineModelForCreate createVaccineModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<VaccineModelForCreate, Vaccine>();

            });
            var vaccine = config.CreateMapper().Map<Vaccine>(createVaccineModel);
            await _vaccineRepository.AddAsync(vaccine);
            return _mapper.Map<VaccineModel>(vaccine);
        }

        public List<string> GetVaccineModelFields()
        {
            var vaccineDto = new VaccineModel();
            return vaccineDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<VaccineModel> GetAllVaccines()
        {
            var vaccinesFromDatabase = _vaccineRepository.GetAllVaccinesAsync().Result;

            List<VaccineModel> vaccines = new List<VaccineModel>();
            foreach (var vaccine in vaccinesFromDatabase)
            {
                var vaccineDto = _mapper.Map<VaccineModel>(vaccine);
                vaccineDto.Name = vaccine.Name;
                vaccineDto.SideEffects = vaccine.SideEffects;
                vaccines.Add(vaccineDto);

            }
            return vaccines.AsEnumerable();
        }
        public IEnumerable<VaccineModel> VaccineSorting(IEnumerable<VaccineModel> vaccines, string sortOrder)
        {
            IEnumerable<VaccineModel> sortedVaccines = vaccines;
            switch (sortOrder)
            {
                case "Name":
                    return vaccines.OrderBy(s => s.Name);
                case "NameDesc":
                    return vaccines.OrderByDescending(s => s.Name);
                default:
                    return vaccines.OrderBy(s => s.Name);
            }
        }

        public IEnumerable<VaccineModel> VaccineSearch(IEnumerable<VaccineModel> vaccines, string searchString)
        {
            IEnumerable<VaccineModel> searchedVaccines = vaccines;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedVaccines = vaccines.Where(s => s.Name.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedVaccines;
        }
        public Vaccine EditVaccineAsync(VaccineModelForCreate vaccineModel)
        {
            var vaccine = _mapper.Map<Vaccine>(vaccineModel);
            return _vaccineRepository.UpdateAsync(vaccine).Result;
        }

        public async Task DeleteTableAsync(Guid Id)
        {
            await _vaccineRepository.DeleteAsync(Id);
        }
    }
}
