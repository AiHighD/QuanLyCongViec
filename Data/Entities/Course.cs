using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseManagement.Data.Entities;

public class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Topic { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartTime { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndTime { get; set; }

    public string? Status { get; set; }


    public ICollection<DocumentsAndImages>? DocumentsAndImages { get; set; }
}

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasMany(e => e.DocumentsAndImages)
        .WithOne(e => e.Course)
        .HasForeignKey(e => e.CourseId)
        .HasPrincipalKey(e => e.Id);
    }
}