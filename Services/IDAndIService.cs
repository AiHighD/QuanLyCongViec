using CourseManagement.ViewModels;

namespace CourseManagement.Services
{
    public interface IDAndIService
    {
        Task<PaginatedList<DocAndImageViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? courseId, int? pageNumber, int pageSize);
        Task<DocAndImageViewModel> GetById(int id);
        Task<int> Create(DocAndImageRequest request);
        Task<int> Update(DocAndImageViewModel request);
        Task<int> Delete(int id);
    }
}