using Microsoft.EntityFrameworkCore;
using RaceDay.Data1;
using RaceDay.Models1;

namespace RaceDay.Service
{
    public class CategoryService
    {
        private readonly RaceDayContext _db;

        public CategoryService(RaceDayContext db)
        {
            _db = db;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _db.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> UpdateAsync(int id, Category updatedCategory)
        {
            var existing = await _db.Categories.FindAsync(id);

            if (existing == null)
                return false;

            existing.CategoryName = updatedCategory.CategoryName;
            existing.EventId = updatedCategory.EventId;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Categories.FindAsync(id);

            if (existing == null)
                return false;

            _db.Categories.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
