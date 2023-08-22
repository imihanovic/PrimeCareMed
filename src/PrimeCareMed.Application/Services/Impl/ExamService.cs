using AutoMapper;
using PrimeCareMed.Application.Models.Exam;
using PrimeCareMed.Application.Models.HospitalExam;
using PrimeCareMed.Core.Entities;
using PrimeCareMed.DataAccess.Repositories;

namespace PrimeCareMed.Application.Services.Impl
{
    public class ExamService : IExamService
    {
        private readonly IMapper _mapper;
        private readonly IExamRepository _examRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHospitalRepository _hospitalRepository;

        public ExamService(IMapper mapper,
            IExamRepository examRepository,
            IUserRepository userRepository,
            IHospitalRepository hospitalRepository
            )
        {
            _mapper = mapper;
            _examRepository = examRepository;
            _userRepository = userRepository;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<ExamModel> AddAsync(ExamModelForCreate createExamModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<ExamModelForCreate, Exam>();

            });
            var exam = config.CreateMapper().Map<Exam>(createExamModel);
            await _examRepository.AddAsync(exam);
            return _mapper.Map<ExamModel>(exam);
        }

        public async Task<HospitalExam> AddHospitalExam(HospitalExamModelForCreate createHospitalExamModel)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<HospitalExamModelForCreate, HospitalExam>();

            });
            var hospitalExam = config.CreateMapper().Map<HospitalExam>(createHospitalExamModel);
            hospitalExam.Hospital = _hospitalRepository.GetHospitalByIdAsync(createHospitalExamModel.HospitalId.ToString()).Result;
            hospitalExam.Exam = _examRepository.GetExamByIdAsync(createHospitalExamModel.ExamId.ToString()).Result;
            await _examRepository.AddHospitalExamAsync(hospitalExam);
            return _mapper.Map<HospitalExam>(hospitalExam);
        }

        public List<string> GetExamModelFields()
        {
            var examDto = new ExamModel();
            return examDto.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).ToList();
        }
        public IEnumerable<ExamModel> GetAllExams()
        {
            var examsFromDatabase = _examRepository.GetAllExamsAsync().Result;

            List<ExamModel> exams = new List<ExamModel>();
            foreach (var exam in examsFromDatabase)
            {
                var examDto = _mapper.Map<ExamModel>(exam);
                examDto.Name = exam.Name;
                examDto.Description = exam.Description;
                examDto.Duration = exam.Duration;
                examDto.Preparation = exam.Preparation;
                exams.Add(examDto);

            }
            return exams.AsEnumerable();
        }
        public IEnumerable<ExamModel> GetAllExamsNotInHospital(Guid Id)
        {
            var examsFromDatabase = _examRepository.GetAllExamsAsync().Result;
            var hospitalExamsFromDB = _examRepository.GetAllHospitalExamsAsync(Id).Result.Select(r=>r.ExamId);
            List<ExamModel> examsNotInHospital = new List<ExamModel>();
            foreach (var exam in examsFromDatabase)
            {
                if (!hospitalExamsFromDB.Contains(exam.Id))
                {
                    var examDto = _mapper.Map<ExamModel>(exam);
                    examsNotInHospital.Add(examDto);
                }
            }
            return examsNotInHospital.AsEnumerable();
        }
        public IEnumerable<ExamModel> ExamSorting(IEnumerable<ExamModel> exams, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Name":
                    return exams.OrderBy(s => s.Name);
                case "NameDesc":
                    return exams.OrderByDescending(s => s.Name);
                default:
                    return exams.OrderBy(s => s.Name);
            }
        }
        public IEnumerable<ExamModel> GetAllHospitalExams(Guid HospitalId)
        {
            var examsFromDatabase = _examRepository.GetAllHospitalExamsAsync(HospitalId).Result.Select(r=>r.Exam);

            List<ExamModel> exams = new List<ExamModel>();
            foreach (var exam in examsFromDatabase)
            {
                var examDto = _mapper.Map<ExamModel>(exam);
                examDto.Name = exam.Name;
                examDto.Description = exam.Description;
                examDto.Duration = exam.Duration;
                examDto.Preparation = exam.Preparation;
                exams.Add(examDto);

            }
            return exams.AsEnumerable();
        }
        public IEnumerable<ExamModel> ExamSearch(IEnumerable<ExamModel> exams, string searchString)
        {
            IEnumerable<ExamModel> searchedExams = exams;
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStrTrim = searchString.ToLower().Trim();
                searchedExams = exams.Where(s => s.Name.ToLower().Contains(searchStrTrim)
                                            || s.Description.ToLower().Contains(searchStrTrim)
                                            || s.Preparation.ToLower().Contains(searchStrTrim)
                                            );
            }
            return searchedExams;
        }
        public Exam EditExamAsync(ExamModelForCreate examModel)
        {
            var exam = _mapper.Map<Exam>(examModel);
            return _examRepository.UpdateAsync(exam).Result;
        }
        public ExamModelForCreate GetExamById(string Id)
        {
            var examFromDB = _examRepository.GetExamByIdAsync(Id).Result;
            return _mapper.Map<ExamModelForCreate>(examFromDB);
        }

        public async Task DeleteExamAsync(Guid Id)
        {
            await _examRepository.DeleteAsync(Id);
        }
    }
}
