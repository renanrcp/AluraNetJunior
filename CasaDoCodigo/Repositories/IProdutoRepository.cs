using CasaDoCodigo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IProdutoRepository
    {
        Task SaveProdutosAsync(List<Livro> livros);

        Task<IList<Produto>> GetProdutosAsync(string pesquisa);
    }
}