using System;
using System.Collections.Generic;

namespace Libray_Managment_System.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Authorid { get; set; }

    public int Publisherid { get; set; }

    public int Categoryid { get; set; }

    public string? Isbn { get; set; }

    public int? Quantity { get; set; }

    public string? Filepath { get; set; }

    public string? Publishedyear { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Bookcopy> Bookcopies { get; set; } = new List<Bookcopy>();

    public virtual Category Category { get; set; } = null!;

    public virtual Publisher Publisher { get; set; } = null!;
}
