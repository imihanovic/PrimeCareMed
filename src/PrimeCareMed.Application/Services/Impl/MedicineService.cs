using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using PrimeCareMed.DataAccess.Repositories;
using PrimeCareMed.Application.Models;
using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.Application.Models.Vaccine;

namespace PrimeCareMed.Application.Services.Impl
{
    public class MedicineService : IMedicineService
    {
        private readonly IMapper _mapper;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IUserRepository _userRepository;

        public MedicineService(IMapper mapper,
            IMedicineRepository medicineRepository,
            IUserRepository userRepository
            )
        {
            _mapper = mapper;
            _medicineRepository = medicineRepository;
            _userRepository = userRepository;
        }

        public async Task<MedicineModel> AddAsync(MedicineModelForCreate createMedicineModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<MedicineModelForCreate, Medicine>();

            });
            var medicine = config.CreateMapper().Map<Medicine>(createMedicineModel);
            await _medicineRepository.AddAsync(medicine);
            return _mapper.Map<MedicineModel>(medicine);
        }

        public List<string> GetMedicineModelFields()
        {
            var medicineDto = new MedicineModel();
            return medicineDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<MedicineModel> GetAllMedicines()
        {
            var medicinesFromDatabase = _medicineRepository.GetAllMedicinesAsync().Result;

            List<MedicineModel> medicines = new List<MedicineModel>();
            foreach (var medicine in medicinesFromDatabase)
            {
                var medicineDto = _mapper.Map<MedicineModel>(medicine);
                medicineDto.Name = medicine.Name;
                medicineDto.Description = medicine.Description;
                medicines.Add(medicineDto);
       
            }
            return medicines.AsEnumerable();
        }
        public IEnumerable<MedicineModel> MedicineSorting(IEnumerable<MedicineModel> medicines, string sortOrder)
        {
            IEnumerable<MedicineModel> sortedMedicines = medicines;
            switch (sortOrder)
            {
                case "Name":
                    return medicines.OrderBy(s => s.Name);
                case "NameDesc":
                    return medicines.OrderByDescending(s => s.Name);
                default:
                    return medicines.OrderBy(s => s.Name);
            }
        }

        public IEnumerable<MedicineModel> MedicineSearch(IEnumerable<MedicineModel> medicines, string searchString)
        {
            IEnumerable<MedicineModel> searchedMedicines = medicines;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedMedicines = medicines.Where(s => s.Name.ToLower().Contains(searchStrTrim));
            }
            return searchedMedicines;
        }
        public Medicine EditMedicineAsync(MedicineModelForCreate medicineModel)
        {
            var medicine = _mapper.Map<Medicine>(medicineModel);
            return _medicineRepository.UpdateAsync(medicine).Result;
        }

        public async Task DeleteMedicineAsync(Guid Id)
        {
            await _medicineRepository.DeleteAsync(Id);
        }

    }
}
