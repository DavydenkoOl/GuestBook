using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuestBook.Models;
using GuestBook.Repository;

namespace GuestBook.Controllers
{
    public class MessagesController : Controller
    {
        IRepository<Messages> _repository;
        IRepository<Users> _repo;
        public MessagesController(IRepository<Messages> context, IRepository<Users> repo)
        {
            _repository = context;
            _repo = repo;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetList());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _repository.GetObject(id);
            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                messages.CreatedDate = DateTime.Now;
                messages.Id_user =Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                messages.Owner =await _repo.GetObject(messages.Id_user);
                await _repository.Create(messages);
                await _repository.Save();
                return View(nameof(Index),await _repository.GetList());
            }
            return View("~/View/Account/Create.cshtml",messages);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _repository.GetObject(id);
            if (messages == null)
            {
                return NotFound();
            }
            return View(messages);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message")] Messages messages)
        {
            if (id != messages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    messages.CreatedDate = DateTime.Now;
                    messages.Id_user = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                    messages.Owner = await _repo.GetObject(messages.Id_user);
                    _repository.Update(messages);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessagesExists(messages.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), messages);
            }
            return View(messages);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _repository.GetObject(id);
            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            
            if (id != null)
            {
                _repository.Delete(id);
            }

            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool MessagesExists(int id)
        {
            if(_repository.GetObject(id).Result!=null)
            return true;
            return false;
        }
    }
}
