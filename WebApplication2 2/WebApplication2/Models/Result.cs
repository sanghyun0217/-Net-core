using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models {
  public class Result<T> {
    public string Msg { get; set; }
    public int Code { get; set; }
    public T Data { get; set; }

  }
}
