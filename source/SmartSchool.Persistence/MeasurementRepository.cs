using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SmartSchool.Persistence
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private ApplicationDbContext _dbContext;

        public MeasurementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  void AddRange(Measurement[] measurements)
        {
            _dbContext.Measurements.AddRange(measurements);
        }

        public IEnumerable<Measurement> GetAllMeasurementsByLocationAndName(string location, string name) => _dbContext.Measurements
                                                                                        .Include(s => s.Sensor)
                                                                                        .Where(s => s.Sensor.Location == location && s.Sensor.Name == name);
    }
}