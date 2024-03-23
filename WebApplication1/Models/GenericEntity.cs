// GenericEntity.cs
using System.ComponentModel.DataAnnotations.Schema;

[Table("Students")]
public class Student
{
    public int Id { get; set; }
    public string TenantId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string State { get; set; }
}