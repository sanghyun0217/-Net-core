using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models {
  public class Page {
    public int Total { get; set; }
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int Pages { get; set; }
    public Object Data { get; set; }

  }
}
