using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels;

public class CourseRequest
{
    public int Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Name { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Topic { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime StartTime { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public string? Status { get; set; }
}

public class CourseViewModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Topic { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartTime { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndTime { get; set; }

    public string? Status { get; set; }
}