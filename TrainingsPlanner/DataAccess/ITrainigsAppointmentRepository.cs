using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingsPlanner.Infrastructure.Models;
using TrainingsPlanner.ViewModels;

namespace TrainingsPlanner.DataAccess
{
    public interface ITrainigsAppointmentRepository
    {

        Task<List<TrainingsAppointment>> ReadAllAppointments();

        Task<TrainingsAppointment> ReadAppointmentById(int id);
        Task<TrainingsAppointment> ReadFullAppointmentById(int id);
        
        Task<List<TrainingsModule>> ReadModulesByAppointmentId(int id);

        Task<TrainingsAppointment> CreateAppointment(TrainingsAppointment trainingsAppointment);

        Task<TrainingsAppointmentTrainingsModule> AddModuleToAppointment(TrainingsAppointmentTrainingsModule trainingsAppointmentTrainingsModule);

        Task<TrainingsAppointment> UpdateAppointment(TrainingsAppointment trainingsAppointment);

        Task<TrainingsAppointment> DeleteAppointment(TrainingsAppointment trainingsAppointment);

        Task<TrainingsAppointmentTrainingsModule> DeleteAppointmentWithModule(int appointmentId, int moduleId);
        Task<List<TrainingsAppointment>> ReadTrainingsAppointmentsByUserId(string userId);

    }
}
