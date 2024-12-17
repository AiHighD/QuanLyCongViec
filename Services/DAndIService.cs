using System.Drawing.Printing;
using System.Net.Http.Headers;
using AutoMapper;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using CourseManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Services
{
    public class DAndIService : IDAndIService
    {
        private readonly CourseDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public DAndIService(CourseDbContext context, IMapper mapper, IStorageService storageService)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<int> Create(DocAndImageRequest request)
        {
            var DocAndImage = _mapper.Map<DocumentsAndImages>(request);

            // Save image file
            if (request.Image != null)
            {
                DocAndImage.Images = await SaveFile(request.Image);
            }
            _context.Add(DocAndImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var lson = await _context.DocumentsAndImages.FindAsync(id);
            if (lson != null)
            {
                if (!string.IsNullOrEmpty(lson.Images))
                    await _storageService.DeleteFileAsync(lson.Images.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                _context.DocumentsAndImages.Remove(lson);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<DocAndImageViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? courseId, int? pageNumber, int pageSize)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var lsons = from m in _context.DocumentsAndImages select m;

            if (courseId != null)
            {
                lsons = lsons.Where(s => s.CourseId == courseId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                lsons = lsons.Where(s => s.Name!.Contains(searchString)
                || s.Documents!.Contains(searchString));
            }

            lsons = sortOrder switch
            {
                "title_desc" => lsons.OrderByDescending(s => s.Name),
                _ => lsons.OrderBy(s => s.Name),
            };

            return PaginatedList<DocAndImageViewModel>.Create(_mapper.Map<IEnumerable<DocAndImageViewModel>>(await lsons.ToListAsync()), pageNumber ?? 1, pageSize);
        }

        public async Task<DocAndImageViewModel> GetById(int id)
        {
            var lson = await _context.DocumentsAndImages
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<DocAndImageViewModel>(lson);
        }

        public async Task<int> Update(DocAndImageViewModel request)
        {
            if (!LsonExists(request.Id))
            {
                throw new Exception("Task does not exist");
            }
            // Save image file
            if (request.Image != null)
            {
                if (!string.IsNullOrEmpty(request.Images))
                    await _storageService.DeleteFileAsync(request.Images.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                request.Images = await SaveFile(request.Image);
            }

            _context.Update(_mapper.Map<DocumentsAndImages>(request));
            return await _context.SaveChangesAsync();
        }

        private bool LsonExists(int id)
        {
            return _context.DocumentsAndImages.Any(e => e.Id == id);
        }
    }
}