using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Abstract;

namespace Domain.Common.Entities;

[Table("Table_1")]
public class Table1 : EntityBase<int>
{
    public string? Name { get; set; }
}
