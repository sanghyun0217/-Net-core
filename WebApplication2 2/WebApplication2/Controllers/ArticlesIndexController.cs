using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers {
  [Produces("application/json")]
  [Route("api/Articles")]
  public class ArticlesIndexController : Controller {
    private readonly UserContent _context;

    /**************************************************************************************
        Function Name    : ArticlesIndexController
        Description      : Constructor, add some articles to database
        Caution          : Parameter    :   UserContent context   ->    Database context object
                           Return       :   ArticlesIndexController object
     **************************************************************************************/
    public ArticlesIndexController(UserContent context) {

      _context = context;
      if (_context.Article.Count() == 0) {
        _context.Article.Add(new Article { Title = "a", Date = new DateTime(2019, 05, 06), Author = "test1", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "b", Date = new DateTime(2019, 05, 06), Author = "test", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "c", Date = new DateTime(2019, 05, 06), Author = "test2", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "d", Date = new DateTime(2019, 05, 06), Author = "test3", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "e", Date = new DateTime(2019, 05, 06), Author = "test1", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "f", Date = new DateTime(2019, 05, 06), Author = "test2", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "g", Date = new DateTime(2019, 05, 06), Author = "test1", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "h", Date = new DateTime(2019, 05, 06), Author = "test1", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "i", Date = new DateTime(2019, 05, 06), Author = "test1", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "j", Date = new DateTime(2019, 05, 06), Author = "test2", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "k", Date = new DateTime(2019, 05, 06), Author = "test2", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "l", Date = new DateTime(2019, 05, 06), Author = "test3", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "m", Date = new DateTime(2019, 05, 06), Author = "test4", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "n", Date = new DateTime(2019, 05, 06), Author = "test", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "aaa", Date = new DateTime(2019, 05, 06), Author = "test", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "aaa", Date = new DateTime(2019, 05, 06), Author = "test", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "aaa", Date = new DateTime(2019, 05, 06), Author = "test", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "aaa", Date = new DateTime(2019, 05, 06), Author = "test", Content = "aaa", AttachFile = "aaa" });
        _context.Article.Add(new Article { Title = "aaa", Date = new DateTime(2019, 05, 06), Author = "test", Content = "aaa", AttachFile = "aaa" });
        _context.SaveChanges();
      }

    }

    // GET: api/Articles1
    [HttpPost]
    /**************************************************************************************
        Function Name    : PostArticle
        Description      : Use Ajax to find article data, filter it by combo parameters
        Caution          : Parameter    :   ArticleForm form   ->    Filter parameters from the page
                           Return       :   Queryed article data
     **************************************************************************************/
    public Page PostArticle([FromForm] ArticleForm form) {

      var articles = from s in _context.Article
                     select s;
      if (!String.IsNullOrEmpty(form.Author)) {
        articles = articles.Where((e) => e.Author.Contains(form.Author));
      }
      if (!String.IsNullOrEmpty(form.Title)) {
        articles = articles.Where((e) => e.Title.Contains(form.Title));
      }
      var name = HttpContext.Session.GetString("username");

      if (!name.Equals("admin")) {

        articles = articles.Where((e) => e.Author.Equals(name));
      }
      if (!String.IsNullOrEmpty(form.Order) && !String.IsNullOrEmpty(form.SortField)) {
        if (form.Order.Equals("asc")) {
          switch (form.SortField) {
            case "title":
              articles = articles.OrderBy((e) => e.Title);
              break;
            case "author":
              articles = articles.OrderBy((e) => e.Author);
              break;
            case "date":
              articles = articles.OrderBy((e) => e.Date);
              break;
            default:
              articles = articles.OrderBy((e) => e.Id);
              break;
          }
        }
        else {
          switch (form.SortField) {
            case "title":
              articles = articles.OrderByDescending((e) => e.Title);
              break;
            case "author":
              articles = articles.OrderByDescending((e) => e.Author);
              break;
            case "date":
              articles = articles.OrderByDescending((e) => e.Date);
              break;
            default:
              articles = articles.OrderByDescending((e) => e.Id);
              break;
          }
        }
      }

      var count = articles.Count();
      var items = articles.Skip((form.PageNumber - 1) * form.PageSize).Take(form.PageSize).ToListAsync();
      int page = count / form.PageSize;
      if (count % form.PageSize != 0) {
        page++;
      }

      return new Page { Data = items.Result, Total = count, PageNum = form.PageNumber, PageSize = form.PageSize, Pages = page };
    }

    private bool ArticleExists(int id) {
      return _context.Article.Any(e => e.Id == id);
    }
  }
}