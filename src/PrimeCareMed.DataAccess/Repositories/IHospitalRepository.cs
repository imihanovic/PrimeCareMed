using Microsoft.EntityFrameworkCore;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface IHospitalRepository
    {
        Task<IEnumerable<Hospital>> GetAllHospitalsAsync();
        Task<Hospital> AddAsync(Hospital hospital);
        Task DeleteAsync(Guid id);
        Task<Hospital> UpdateAsync(Hospital hospital);
        Task<Hospital> GetHospitalByIdAsync(string id);
    }
}
