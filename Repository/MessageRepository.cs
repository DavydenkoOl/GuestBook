using Microsoft.EntityFrameworkCore;
using GuestBook.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace GuestBook.Repository
{
    public class MessageRepository: IRepository<Messages>
    {
        private readonly UserContext _context;
        

        public MessageRepository(UserContext context)
        {
            _context = context;
        }

        public async Task Create(Messages item)
        {
            await _context.Messages.AddAsync(item);
        }

        public async Task Delete(int? id)

        {

            Messages? m =await _context.Messages.FindAsync(id);
            if(m != null)
                _context.Messages.Remove(m);
        }

        public async Task<Messages> GetObject(int? id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<List<Messages>> GetList()
        {


           return await _context.Messages.Include("Owner").ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

       

        public void Update(Messages item)
        {
            _context.Entry(item).State = EntityState.Modified; 
        }

        

        
    }
}
