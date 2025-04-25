using System.ComponentModel.DataAnnotations.Schema;
using Domain.Abstract;

namespace Domain.Common.Entities;

[Table("Table_2")]
public class Table2 : EntityBase<int>
{
    public string? Test { get; set; }

    public string? Aaa { get; set; }

    public string? Sss { get; set; }
}
