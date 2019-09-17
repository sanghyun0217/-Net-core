using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models {
  public class ArticleForm {

    public string Title { get; set; }

    public string Author { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Order { get; set; }
    public string SortField { get; set; }

    public override string ToString() {
      return "Author=" + Author + "\t" + "Title=" + Title + "\tPageNumber=" + PageNumber
          + "\tPageSize=" + PageSize + "\tOrder=" + Order + "\tSortField=" + SortField + "\n\n";
    }
  }
}
