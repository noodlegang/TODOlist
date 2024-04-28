using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TODOlist.Models;

namespace TODOlist.Controllers
{
    public class HomeController : Controller
    {
        private ToDoContext _toDoContext;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ToDoContext toDoContext, ILogger<HomeController> logger)
                                    => (_toDoContext, _logger) = (toDoContext, logger);

        public IActionResult Index()
        {
            ViewBag.Statuses = _toDoContext.Statuses.ToList();

            IQueryable<ToDoItem> query = _toDoContext.ToDos
                .Include(t => t.Status);

            var tasks = query.OrderBy(s => s.Status).ToList();
            return View(tasks);
        }
        [HttpGet]
        public IActionResult Add()
        { 
            ViewBag.Statuses = _toDoContext.Statuses.ToList();
            int lastToDoItemId = _toDoContext.ToDos.Any() ? _toDoContext.ToDos.Max(item => item.Id) : 0;
            lastToDoItemId++;
            var task = new ToDoItem { Id = lastToDoItemId, StatusId = "Ready" };
            return View(task);
        }
        [HttpPost]
        public IActionResult Add(ToDoItem item) 
        { 
            if(ModelState.IsValid)
            {
                _toDoContext.ToDos.Add(item);
                _toDoContext.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                //ViewBag.Statuses = _toDoContext.Statuses.ToList();
                return View(item);
            }
        }
        [HttpPost]
        public IActionResult MarkComplete(int id) 
        {
            try
            {
                var selected = _toDoContext.ToDos.Find(id);
                if (selected != null)
                {
                    selected.StatusId = "Done";
                    _toDoContext.SaveChanges();
                } else { return RedirectToAction("Index"); }
            } catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the MarkComplete action.");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult MarkWIP(int id)
        {
            try
            {
                var selectedItem = _toDoContext.ToDos.Find(id);
                if (selectedItem != null)
                {
                    selectedItem.StatusId = "In Progress";
                    _toDoContext.SaveChanges();
                } else { return RedirectToAction("Index"); }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the MarkWIP action.");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                var itemToDelete = _toDoContext.ToDos.Find(id);
                if (itemToDelete != null)
                {
                    _toDoContext.ToDos.Remove(itemToDelete);
                    _toDoContext.SaveChanges();
                } else { return RedirectToAction("Index"); }
            } catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the DeleteTask action.");
            }
            return RedirectToAction("Index");
        }
    }
}