﻿using PrimeCareMed.Application.Models.Medicine;
using PrimeCareMed.Core.Entities;

namespace PrimeCareMed.Application.Services
{
    public interface IMedicineService
    {
        Task<MedicineModel> AddAsync(MedicineModelForCreate createMedicineModel);
        List<string> GetMedicineModelFields();
        IEnumerable<MedicineModel> GetAllMedicines();
        Medicine EditMedicineAsync(MedicineModelForCreate medicineModel);
        Task DeleteMedicineAsync(Guid Id);
        IEnumerable<MedicineModel> MedicineSorting(IEnumerable<MedicineModel> medicines, string sortOrder);
        IEnumerable<MedicineModel> MedicineSearch(IEnumerable<MedicineModel> medicines, string searchString);
    }
}
