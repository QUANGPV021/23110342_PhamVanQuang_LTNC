using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVienDAO.Entity;

namespace QuanLyThuVienDAO.Reposistory
{
    /// <summary>
    /// Data Access Layer (Repository) cho thực thể Category (Thể loại sách).
    /// </summary>
    public class CategoryRepository
    {
        private readonly QLTVDBContext _context;

        public CategoryRepository() : this(new QLTVDBContext())
        {
        }

        public CategoryRepository(QLTVDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Category> getCategories()
        {
            return GetAll();
        }

        public Category? getCategory(string id)
        {
            return GetById(id);
        }

        public List<Category> GetAll()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.Categories
                .OrderBy(c => c.CategoryId)
                .ToList();
        }

        public Category? GetById(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return null;

            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            return _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public void Add(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (_context.Categories.Any(c => c.CategoryId == category.CategoryId))
            {
                throw new InvalidOperationException("Mã thể loại đã tồn tại.");
            }

            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch
            {
                if (_context.Database.CurrentTransaction == null)
                {
                    _context.ChangeTracker.Clear();
                }
                throw;
            }
        }

        public bool Update(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (_context.Database.CurrentTransaction == null &&
                _context.Entry(category).State == EntityState.Detached)
            {
                _context.ChangeTracker.Clear();
            }

            var existing = _context.Categories.Find(category.CategoryId);
            if (existing == null)
                return false;

            existing.Description = category.Description;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return false;

            if (_context.Database.CurrentTransaction == null)
            {
                _context.ChangeTracker.Clear();
            }

            var category = _context.Categories.Find(categoryId);
            if (category == null)
                return false;

            if (_context.Books.Any(b => b.CategoryId == categoryId))
            {
                throw new InvalidOperationException("Không thể xóa thể loại đang có sách.");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }
    }
}
