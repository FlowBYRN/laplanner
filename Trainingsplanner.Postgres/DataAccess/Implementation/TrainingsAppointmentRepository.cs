using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trainingsplanner.Postgres.Data;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.DataAccess.Implementation
{
    public class TrainingsAppointmentRepository : ITrainigsAppointmentRepository
    {

        private ApplicationDbContext Context { get; set; }

        public TrainingsAppointmentRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<List<TrainingsAppointment>> ReadAllAppointments()
        {
            List<TrainingsAppointment> appointments = await Context.TrainingsAppointments.ToListAsync();

            return appointments;
        }

        public async Task<TrainingsAppointment> ReadAppointmentById(int id)
        {
            var appointment = await Context.TrainingsAppointments
                .Include(appointment => appointment.TrainingsAppointmentsTrainingsModules)
                .ThenInclude(tatm => tatm.TrainingsModule)
                .Where(appointment => appointment.Id == id)
                .FirstOrDefaultAsync();

            return appointment;

        }

        public async Task<TrainingsAppointment> ReadFullAppointmentById(int id)
        {
            var appointment = await Context.TrainingsAppointments
                .Include(appointment => appointment.TrainingsAppointmentsTrainingsModules)
                .ThenInclude(tatm => tatm.TrainingsModule)
                .ThenInclude(tm => tm.TrainingsModulesTrainingsExercises)
                .ThenInclude(tmte => tmte.TrainingsExercise)
                .Where(appointment => appointment.Id == id)
                .FirstOrDefaultAsync();

            return appointment;
        }

        public async Task<List<TrainingsModule>> ReadModulesByAppointmentId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException();
            }

            var modules = await Context.TrainingsAppointmentsTrainingsModules.Where(x => x.TrainingsAppointmentId == id).Select(y => y.TrainingsModule).ToListAsync();

            if (modules.Count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return modules;
        }

        public async Task<TrainingsAppointment> CreateAppointment(TrainingsAppointment trainingsAppointment)
        {
            if (trainingsAppointment == null)
            {
                throw new ArgumentNullException();
            }
            trainingsAppointment.Created = DateTime.UtcNow;
            var appointment = await Context.TrainingsAppointments.AddAsync(trainingsAppointment);

            await Context.SaveChangesAsync();

            return appointment.Entity;
        }

        public async Task<TrainingsAppointmentTrainingsModule> AddModuleToAppointment(TrainingsAppointmentTrainingsModule trainingsAppointmentTrainingsModule)
        {
            if (trainingsAppointmentTrainingsModule == null)
            {
                throw new ArgumentNullException();
            }
            trainingsAppointmentTrainingsModule.Created = DateTime.UtcNow;

            var appointmentmodule = await Context.TrainingsAppointmentsTrainingsModules.AddAsync(trainingsAppointmentTrainingsModule);

            await Context.SaveChangesAsync();

            return appointmentmodule.Entity;
        }

        public async Task<TrainingsAppointment> UpdateAppointment(TrainingsAppointment trainingsAppointment)
        {
            if (trainingsAppointment == null)
            {
                throw new ArgumentNullException();
            }

            trainingsAppointment.Updated = DateTime.Now;

            Context.TrainingsAppointments.Update(trainingsAppointment).State = EntityState.Modified;
            Context.TrainingsAppointments.Update(trainingsAppointment).Property(x => x.Created).IsModified = false;

            await Context.SaveChangesAsync();

            var appointment = await Context.TrainingsAppointments.FindAsync(trainingsAppointment.Id);

            return appointment;
        }

        public async Task<TrainingsAppointment> DeleteAppointment(TrainingsAppointment trainingsAppointment)
        {
            if (trainingsAppointment == null)
            {
                throw new ArgumentNullException();
            }

            var appointment = Context.TrainingsAppointments.Remove(trainingsAppointment);

            await Context.SaveChangesAsync();

            return appointment.Entity;
        }

        public async Task<TrainingsAppointmentTrainingsModule> DeleteModuleFromAppointment(int appointmentId, int moduleId)
        {
            if (appointmentId < 0)
            {
                throw new ArgumentNullException();
            }

            if (moduleId < 0)
            {
                throw new ArgumentNullException();
            }

            var appointmentModule = await Context.TrainingsAppointmentsTrainingsModules
                .Where(x => x.TrainingsAppointmentId == appointmentId)
                .Where(y => y.TrainingsModuleId == moduleId).FirstOrDefaultAsync();

            var deletedItem = Context.TrainingsAppointmentsTrainingsModules.Remove(appointmentModule);

            await Context.SaveChangesAsync();

            return deletedItem.Entity;
        }

        public async Task<List<TrainingsAppointment>> ReadTrainingsAppointmentsByUserId(string userId)
        {
            return await Context.TrainingsAppointments
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
