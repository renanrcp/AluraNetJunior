using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CasaDoCodigo.Repositories
{
    public interface ICategoriaRepository
    {
        IList<Categoria> GetCategorias();

        Task<Categoria> GetOrCreateCategoryAsync(string Name);
    }

    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public IList<Categoria> GetCategorias()
        {
            return dbSet.ToList();
        }

        public async Task<Categoria> GetOrCreateCategoryAsync(string Name)
        {
            if (!await dbSet.AnyAsync(a => a.Nome == Name))
            {
                var category = new Categoria()
                {
                    Nome = Name
                };
                await dbSet.AddAsync(category);
                await contexto.SaveChangesAsync();
                return category;
            }
            return await dbSet.Where(a => a.Nome == Name).FirstOrDefaultAsync();
        }
    }
}