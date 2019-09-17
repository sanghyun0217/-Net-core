using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models {
  public class Article {
    public int Id { get; set; }
    public string Title { get; set; }

    public string Author { get; set; }
    public DateTime Date { get; set; }

    public string Content { get; set; }

    public string AttachFile { get; set; }

  }
}
