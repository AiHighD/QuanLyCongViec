using System.Drawing.Printing;
using AutoMapper;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using CourseManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly CourseDbContext _context;
        private readonly IMapper _mapper;

        public CoursesService(CourseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(CourseRequest request)
        {
            var course = _mapper.Map<Course>(request);
            _context.Add(course);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseViewModel>> GetAll()
        {
            var courses = await _context.Courses.ToListAsync();
            return _mapper.Map<IEnumerable<CourseViewModel>>(courses);
        }

        public async Task<PaginatedList<CourseViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var courses = from m in _context.Courses select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name!.Contains(searchString)
                || s.Topic!.Contains(searchString)
                || s.Status!.Contains(searchString));
            }

            courses = sortOrder switch
            {
                "name_desc" => courses.OrderByDescending(s => s.Name),
                "topic" => courses.OrderBy(s => s.Topic),
                "topic_desc" => courses.OrderByDescending(s => s.Topic),
                "starttime_date" => courses.OrderBy(s => s.StartTime),
                "start_time_desc" => courses.OrderByDescending(s => s.StartTime),
                "endtime_date" => courses.OrderBy(s => s.EndTime),
                "end_time_desc" => courses.OrderByDescending(s => s.EndTime),
                "status" => courses.OrderBy(s => s.Status),
                "status_desc" => courses.OrderByDescending(s => s.Status),
                _ => courses.OrderBy(s => s.Name),
            };

            return PaginatedList<CourseViewModel>.Create(_mapper.Map<IEnumerable<CourseViewModel>>(await courses.ToListAsync()), pageNumber ?? 1, pageSize);
        }

        public async Task<CourseViewModel> GetById(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<CourseViewModel>(course);
        }

        public async Task<int> Update(CourseViewModel request)
        {
            if (!CourseExists(request.Id))
            {
                throw new Exception("Task does not exist");
            }
            _context.Update(_mapper.Map<Course>(request));
            return await _context.SaveChangesAsync();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}