// GenericEntity.cs
using System.ComponentModel.DataAnnotations.Schema;

[Table("GenericEntity")]
public class GenericEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string TenantId { get; set; }
}