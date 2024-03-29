﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingsPlanner.Infrastructure;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.DataAccess.Implementation
{
    public class TrainingsModuleTagRepository : ITrainingsModuleTagRepository
    {

        private readonly TrainingDbContext _context;

        public TrainingsModuleTagRepository(TrainingDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainingsModuleTag>> ReadAllTags()
        {
            return _context.TrainingsModuleTags.ToList();
        }

        public async Task<TrainingsModuleTag> ReadTagById(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException();
            }

            var tag = _context.TrainingsModuleTags.FindAsync(id);

            return await tag;
        }

        public async Task<TrainingsModuleTag> InsertTag(TrainingsModuleTag trainingsModuleTag)
        {
            if(trainingsModuleTag == null)
            {
                throw new ArgumentNullException();
            }

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
