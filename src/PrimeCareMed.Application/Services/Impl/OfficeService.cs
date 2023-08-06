using AutoMapper;
using PrimeCareMed.Application.Models.GeneralMedicineOffice;
using PrimeCareMed.Application.Models.Vaccine;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services.Impl
{
    public class OfficeService : IOfficeService
    {
        private readonly IMapper _mapper;
        private readonly IOfficeRepository _officeRepository;
        private readonly IUserRepository _userRepository;

        public OfficeService(IMapper mapper,
            IOfficeRepository officeRepository,
            IUserRepository userRepository
            )
        {
            _mapper = mapper;
            _officeRepository = officeRepository;
            _userRepository = userRepository;
        }

        public async Task<OfficeModel> AddAsync(OfficeModelForCreate createOfficeModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<OfficeModelForCreate, GeneralMedicineOffice>();

            });
            var office = config.CreateMapper().Map<GeneralMedicineOffice>(createOfficeModel);
            await _officeRepository.AddAsync(office);
            return _mapper.Map<OfficeModel>(office);
        }

        public List<string> GetOfficeModelFields()
        {
            var officeDto = new OfficeModel();
            return officeDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<OfficeModel> GetAllOffices()
        {
            var officesFromDatabase = _officeRepository.GetAllOfficesAsync().Result;

            List<OfficeModel> offices = new List<OfficeModel>();
            foreach (var office in officesFromDatabase)
            {
                var officeDto = _mapper.Map<OfficeModel>(office);
                officeDto.Address = office.Address;
                officeDto.City = office.City;
                offices.Add(officeDto);

            }
            return offices.AsEnumerable();
        }
        public IEnumerable<OfficeModel> OfficeSorting(IEnumerable<OfficeModel> offices, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Name":
                    return offices.OrderBy(s => s.Name);
                case "NameDesc":
                    return offices.OrderByDescending(s => s.Name);
                case "City":
                    return offices.OrderBy(s => s.City);
                case "CityDesc":
                    return offices.OrderByDescending(s => s.City);
                default:
                    return offices.OrderBy(s => s.Name);
            }
        }

        public IEnumerable<OfficeModel> OfficeSearch(IEnumerable<OfficeModel> offices, string searchString)
        {
            IEnumerable<OfficeModel> searchedOffices = offices;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedOffices = offices.Where(s => s.Name.ToLower().Contains(searchStrTrim)
                                            || s.Address.ToLower().Contains(searchStrTrim)
                                            || s.City.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedOffices;
        }
        public GeneralMedicineOffice EditOfficeAsync(OfficeModelForCreate officeModel)
        {
            var office = _mapper.Map<GeneralMedicineOffice>(officeModel);
            return _officeRepository.UpdateAsync(office).Result;
        }
        public OfficeModelForCreate GetOfficeById(string Id)
        {
            var officeFromDB = _officeRepository.GetOfficeByIdAsync(Id).Result;
            return _mapper.Map<OfficeModelForCreate>(officeFromDB);
        }

        public async Task DeleteOfficeAsync(Guid Id)
        {
            await _officeRepository.DeleteAsync(Id);
        }
    }
}
