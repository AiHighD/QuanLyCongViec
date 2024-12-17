using System.Net.Mime;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels;

public class DocAndImageRequest
{

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Name { get; set; }

    [StringLength(100, MinimumLength = 3)]
    [Required]
    public string? Documents { get; set; }

    [Required]
    public IFormFile? Image { get; set; }

    [Required]
    public int CourseId { get; set; }

}

public class DocAndImageViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Documents { get; set; }

    public string? Images { get; set; }

    public IFormFile? Image { get; set; }

    [Display(Name = "Task")]
    public int CourseId { get; set; }


}

