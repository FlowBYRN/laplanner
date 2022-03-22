using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.Data;
using Trainingsplanner.Postgres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Trainingsplanner.Postgres.DataAccess.Implementation
{
    public class TrainingsModuleTagRepository : ITrainingsModuleTagRepository
    {

        private readonly ApplicationDbContext _context;

        public TrainingsModuleTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainingsModuleTag>> ReadAllTags()
        {
            return _context.TrainingsModuleTags.ToList();
        }

        public async Task<TrainingsModuleTag> ReadTagById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException();
            }

            var tag = _context.TrainingsModuleTags.FindAsync(id);

            return await tag;
        }

        public async Task<TrainingsModuleTag> ReadTagByName(string name)
        {
            var tag = await _context.TrainingsModuleTags.Where(tmt => tmt.Title == name).FirstOrDefaultAsync();

            return tag;
        }

        public async Task<TrainingsModuleTag> InsertTag(TrainingsModuleTag trainingsModuleTag)
        {
            if (trainingsModuleTag == null)
            {
                throw new ArgumentNullException();
            }
            trainingsModuleTag.Created = DateTime.Now;
            var tag = _context.TrainingsModuleTags.Add(trainingsModuleTag);

            await _context.SaveChangesAsync();

            return tag.Entity;
        }

        public async Task<TrainingsModuleTag> UpdateTag(TrainingsModuleTag trainingsModuleTag)
        {
            if (trainingsModuleTag == null)
            {
                throw new ArgumentNullException();
            }

            trainingsModuleTag.Updated = DateTime.Now;

            _context.TrainingsModuleTags.Update(trainingsModuleTag).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.TrainingsModuleTags.Update(trainingsModuleTag).Property(x => x.Created).IsModified = false;

            await _context.SaveChangesAsync();

            var updatedTag = await _context.TrainingsModuleTags.FindAsync(trainingsModuleTag.Id);

            return updatedTag;
        }

        public async Task<TrainingsModuleTag> DeleteTag(TrainingsModuleTag trainingsModuleTag)
        {
            if (trainingsModuleTag == null)
            {
                throw new ArgumentNullException();
            }

            var deletedTag = _context.TrainingsModuleTags.Remove(trainingsModuleTag);

            await _context.SaveChangesAsync();

            return deletedTag.Entity;
        }
    }
}
