using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private ICategoriaRepository _categoriaRepository { get; set; }

        public ProdutoRepository(ApplicationContext contexto, ICategoriaRepository categoriaRepository) : base(contexto)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IList<Produto>> GetProdutosAsync(string pesquisa)
        {
            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                return await dbSet.Include(a => a.Categoria).Where(a => a.Nome.Contains(pesquisa) || a.Categoria.Nome.Contains(pesquisa)).ToListAsync();
            }
            return await dbSet.Include(a => a.Categoria).ToListAsync();
        }

        public async Task SaveProdutosAsync(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, await _categoriaRepository.GetOrCreateCategoryAsync(livro.Categoria)));
                }
            }
            await contexto.SaveChangesAsync();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}