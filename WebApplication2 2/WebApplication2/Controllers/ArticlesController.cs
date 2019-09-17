using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers {
  public class ArticlesController : Controller {
    private readonly UserContent _context;

    /**************************************************************************************
        Function Name    : ArticlesController
        Description      : Constructor
        Caution          : Parameter    :   UserContent context   ->    Database context object
                           Return       :   ArticlesController object
     **************************************************************************************/

    public ArticlesController(UserContent context) {
      _context = context;
    }

    /**************************************************************************************
        Function Name    : Index
        Description      : Control page returns to the article home page
        Caution          : Parameter    :   empty
                           Return       :   Page model and view
     **************************************************************************************/
    public async Task<IActionResult> Index() {
      return View();
    }

    /**************************************************************************************
        Function Name    : Create
        Description      : Control page returns to the article create page
        Caution          : Parameter    :   empty
                           Return       :   Page model and view
     **************************************************************************************/
    public async Task<IActionResult> Create() {
      int c = _context.Article.Count();
      return View();
    }

    // GET: Articles/Details/5
    /**************************************************************************************
       Function Name    : Details
       Description      : Get specific article information based on id
       Caution          : Parameter    :   int? id -> the id of article
                          Return       :   The view of article
    **************************************************************************************/
    public async Task<IActionResult> Details(int? id) {
      if (id == null) {
        return NotFound();
      }

      var article = await _context.Article
          .SingleOrDefaultAsync(m => m.Id == id);
      if (article == null) {
        return NotFound();
      }

      return View(article);
    }


    // GET: Articles/Edit/5
    /**************************************************************************************
      Function Name    : Edit
      Description      : Get specific article information based on id and jump to the page that edits the article
      Caution          : Parameter    :   int? id -> the id of article
                         Return       :   The view of edit article
     **************************************************************************************/
    public async Task<IActionResult> Edit(int? id) {
      if (id == null) {
        return NotFound();
      }

      var article = await _context.Article.SingleOrDefaultAsync(m => m.Id == id);
      if (article == null) {
        return NotFound();
      }
      return View(article);
    }

    [HttpPost]
    /**************************************************************************************
        Function Name    : PostCreate
        Description      : Receive the attributes of the article , and insert the article into the dataset.
        Caution          : Parameter    :   Article article     -> the attributes of the article 
                           Return       :   The JSON result
     **************************************************************************************/

    public async Task<JsonResult> PostCreate([Bind("Title,Author,Date,Content,AttachFile")] Article article) {
      if (ModelState.IsValid) {
        try {
          int id = 0;
          int allcount = _context.Article.Count();
          if (allcount != 0) {
            id = _context.Article.Max(a => a.Id);
          }
          article.Id = id + 1;

          _context.Add(article);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
          return Json(new { result = 0, msg = "unexcepted error" });
        }
        return Json(new { result = 1, msg = "" });
      }

      var errorValue = ModelState.Where(a => a.Value.Errors.Count > 0).First();
      return Json(new { result = 0, msg = "Input: " + errorValue.Key + " is not valid" });
    }

    // POST: Articles/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    /**************************************************************************************
        Function Name    : PostEditEdit
        Description      : Receive the modified parameters from the page, and update the individual attributes of the article according to the id.
        Caution          : Parameter    :   int? id             -> the id of article
                                            Article article     -> the attributes of the article 
                           Return       :   The json result of edit
     **************************************************************************************/
    public async Task<JsonResult> PostEdit(int id, [Bind("Id,Title,Author,Date,Content,AttachFile")] Article article) {
      if (id != article.Id) {
        return Json(new {result = 0, msg = "article id error" });
      }

      if (ModelState.IsValid) {
        try {
          _context.Update(article);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
          if (!ArticleExists(article.Id)) {
            return Json(new { result = 0, msg = "article not find" });
          }
          else {
            return Json(new { result = 0, msg = "unexcepted error" });
          }
        }
        return Json(new { result = 1, msg = "" });
      }

      var errorValue = ModelState.Where(a => a.Value.Errors.Count > 0).First();
      return Json(new { result = 0, msg = "Input: " + errorValue.Key + " is not valid" });
    }

    // GET: Articles/Delete/5
    /**************************************************************************************
     Function Name    : Delete
     Description      : Get specific article information based on id and jump to the page that delete the article
     Caution          : Parameter    :   int? id -> the id of article
                        Return       :   The view of delete article
    **************************************************************************************/
    public async Task<IActionResult> Delete(int? id) {
      if (id == null) {
        return NotFound();
      }

      var article = await _context.Article
          .SingleOrDefaultAsync(m => m.Id == id);
      if (article == null) {
        return NotFound();
      }

      return View(article);
    }

    // POST: Articles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    /**************************************************************************************
       Function Name    : DeleteConfirmed
       Description      : Execute the logic to delete an article based on id, first find the article and then delete
       Caution          : Parameter    :   int? id -> the id of article
                          Return       :   The index page of article
      **************************************************************************************/
    public async Task<IActionResult> DeleteConfirmed(int id) {
      var article = await _context.Article.SingleOrDefaultAsync(m => m.Id == id);
      _context.Article.Remove(article);
      await _context.SaveChangesAsync();
      return View(nameof(Index));
    }


    // POST: Articles/batchRemove
    [HttpPost, ActionName("batchRemove")]
    [Produces("application/json")]

    /**************************************************************************************
      Function Name    : BatchRemove
      Description      : Use ajax request to delete articles in bulk
      Caution          : Parameter    :   int[] ids -> the ids of article
                         Return       :   The result of remove
     **************************************************************************************/
    public async Task<IActionResult> BatchRemove(int[] ids) {
      System.Diagnostics.Debug.Write("==================================\n\n");
      List<Article> articles = new List<Article>();
      foreach (var item in ids) {
        var article = await _context.Article.SingleOrDefaultAsync(m => m.Id == item);
        articles.Add(article);
      }

      _context.Article.RemoveRange(articles);
      await _context.SaveChangesAsync();

      System.Diagnostics.Debug.Write("==================================\n\n");
      return Ok("success");
    }

    private bool ArticleExists(int id) {
      return _context.Article.Any(e => e.Id == id);
    }

  }


}
