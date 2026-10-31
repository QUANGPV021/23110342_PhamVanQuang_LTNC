using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using QuanLyThuVienDAO.Entity;
using QuanLyThuVienDAO.Reposistory;

namespace QuanLyThuVienDAO.Business
{
    /// <summary>
    /// Business Logic Layer cho thực thể Category (Thể loại sách).
    /// </summary>
    public class CategoryBuss
    {
        private readonly CategoryRepository _categoryDao;

        public CategoryBuss() : this(new QLTVDBContext())
        {
        }

        public CategoryBuss(QLTVDBContext context)
        {
            _categoryDao = new CategoryRepository(context);
        }

        public List<Category> GetAll()
        {
            return _categoryDao.GetAll();
        }

        public List<Category> GetByName(string? name)
        {
            var categories = _categoryDao.GetAll();

            if (string.IsNullOrWhiteSpace(name))
                return categories;

            string keyword = name.Trim();

            return categories
                .Where(c => c.Description.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Category? GetById(string id)
        {
            ValidateId(id);

            return _categoryDao.GetById(id.Trim());
        }

        public void Add(Category category)
        {
            NormalizeCategory(category);
            ValidateCategory(category);

            _categoryDao.Add(category);
        }

        public void Edit(Category category)
        {
            NormalizeCategory(category);
            ValidateCategory(category);

            if (!_categoryDao.Update(category))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy thể loại cần cập nhật!");
            }
        }

        public void Update(Category category) => Edit(category);

        public void Delete(string id)
        {
            ValidateId(id);

            if (!_categoryDao.Delete(id.Trim()))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy thể loại cần xóa!");
            }
        }

        private static void NormalizeCategory(Category? category)
        {
            if (category == null)
                return;

            category.CategoryId = (category.CategoryId ?? string.Empty).Trim();
            category.Description = Regex.Replace((category.Description ?? string.Empty).Trim(), @"\s+", " ");
        }

        private static void ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Mã thể loại không được để trống!");
            }

            if (id.Trim().Length > 10)
            {
                throw new ArgumentException(
                    "Mã thể loại không được vượt quá 10 ký tự!");
            }
        }

        private static void ValidateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            ValidateId(category.CategoryId);

            if (string.IsNullOrWhiteSpace(category.Description))
            {
                throw new ArgumentException(
                    "Tên hoặc mô tả thể loại không được để trống!");
            }

            if (category.Description.Length > 200)
            {
                throw new ArgumentException(
                    "Mô tả thể loại không được vượt quá 200 ký tự!");
            }
        }
    }
}
