using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Data.Entities;

public class DocumentsAndImages
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Documents { get; set; }

    public string? Images { get; set; }

    public int CourseId { get; set; }

    public Course? Course { get; set; }

    

}