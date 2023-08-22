using PrimeCareMed.Application.Models.Exam;
using PrimeCareMed.Application.Models.HospitalExam;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services
{
    public interface IExamService
    {
        Task<ExamModel> AddAsync(ExamModelForCreate createExamModel);
        List<string> GetExamModelFields();
        IEnumerable<ExamModel> GetAllExams();
        IEnumerable<ExamModel> ExamSorting(IEnumerable<ExamModel> exams, string sortOrder);
        IEnumerable<ExamModel> ExamSearch(IEnumerable<ExamModel> exams, string searchString);
        Exam EditExamAsync(ExamModelForCreate examModel);
        ExamModelForCreate GetExamById(string Id);
        Task DeleteExamAsync(Guid Id);
        IEnumerable<ExamModel> GetAllHospitalExams(Guid HospitalId);
        Task<HospitalExam> AddHospitalExam(HospitalExamModelForCreate createHospitalExamModel);
        IEnumerable<ExamModel> GetAllExamsNotInHospital(Guid Id);
    }
}
