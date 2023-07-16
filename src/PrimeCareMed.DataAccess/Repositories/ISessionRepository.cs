using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.DataAccess.Repositories
{
    public interface ISessionRepository
    {
        Task<Session> AddAsync(Session session);
        Task DeleteAsync(Guid id);
        Task<Session> UpdateAsync(Session session);
        Task<Session> GetSessionByIdAsync(Guid id);
        Session CheckIfOpenSessionExistsForDoctor(string Id);
        Session CheckIfOpenSessionExistsForNurse(string Id);
        Task<IEnumerable<Session>> GetAllCurrentSessions();
    }
}
