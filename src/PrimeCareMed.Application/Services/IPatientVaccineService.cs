﻿using PrimeCareMed.Application.Models.PatientVaccine;
using PrimeCareMed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCareMed.Application.Services
{
    public interface IPatientVaccineService
    {
        Task<PatientVaccineModel> AddAsync(PatientVaccineModelForCreate createReportModel, Guid appointmentId);
        List<string> GetPatientVaccineModelFields();
        PatientsVaccine EditPatientVaccineAsync(PatientVaccineModelForCreate patientVaccineModel);
        Task DeletePatientVaccineAsync(Guid Id);
        IEnumerable<PatientVaccineModel> GetPatientVaccineForAppointment(Guid id);
        IEnumerable<PatientVaccineModel> GetPatientVaccineForPatient(Guid id);
        IEnumerable<PatientVaccineModel> VaccineSearch(IEnumerable<PatientVaccineModel> vaccines, string searchString);
        IEnumerable<PatientVaccineModel> VaccineSorting(IEnumerable<PatientVaccineModel> vaccines, string sortOrder);
    }
}
