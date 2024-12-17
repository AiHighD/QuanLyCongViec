using Microsoft.AspNetCore.Mvc;
using CourseManagement.ViewModels;
using CourseManagement.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseManagement.Controllers
{
    public class DocumentAndImagesController : Controller
    {
        private readonly IDAndIService _lessonsService;
        private readonly ICoursesService _coursesService;
        int PAGESIZE = 5;

        public DocumentAndImagesController(IDAndIService lessonsService, ICoursesService coursesService)
        {
            _lessonsService = lessonsService;
            _coursesService = coursesService;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? courseId, int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "title_desc" : "";

            var courses = await _coursesService.GetAll();
            ViewData["CourseId"] = courses.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = courseId.HasValue && courseId == x.Id
            });
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            return View(await _lessonsService.GetAllFilter(sortOrder, currentFilter, searchString, courseId, pageNumber, PAGESIZE));
        }

        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _lessonsService.GetById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CourseId"] = new SelectList(await _coursesService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocAndImageRequest request)
        {
            if (ModelState.IsValid)
            {
                await _lessonsService.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonsService.GetById(id);
            if (lesson == null)
            {
                return NotFound();
            }

            ViewData["CourseId"] = new SelectList(await _coursesService.GetAll(), "Id", "Name", lesson.CourseId);

            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocAndImageViewModel lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Cập nhật đối tượng với thông tin từ ViewModel
                await _lessonsService.Update(lesson);
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, trả lại view với model đã nhập và danh sách 
            ViewData["CourseId"] = new SelectList(await _coursesService.GetAll(), "Id", "Name", lesson.CourseId);
            return View(lesson);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _lessonsService.GetById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _lessonsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
