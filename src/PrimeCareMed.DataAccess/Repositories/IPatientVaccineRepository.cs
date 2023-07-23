using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IPatientVaccineRepository
    {
        Task<IEnumerable<PatientsVaccine>> GetAllPatientsVaccinesForAppointmentAsync(Guid id);
        Task<PatientsVaccine> AddAsync(PatientsVaccine patientsVaccine);
        Task DeleteAsync(Guid id);
        Task<PatientsVaccine> UpdateAsync(PatientsVaccine patientsVaccine);
        Task<PatientsVaccine> GetPatientsVaccineByIdAsync(Guid id);
        bool CheckPatientsVaccinesForAppointmentAsync(Guid id);
    }
}
