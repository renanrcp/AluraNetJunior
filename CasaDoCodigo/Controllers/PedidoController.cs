using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;
        private readonly ICategoriaRepository categoriaRepository;

        public PedidoController(IProdutoRepository produtoRepository,
            IPedidoRepository pedidoRepository,
            IItemPedidoRepository itemPedidoRepository,
            ICategoriaRepository categoriaRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
            this.categoriaRepository = categoriaRepository;
        }

        public async Task<IActionResult> BuscaDeProdutos(BuscaDeProdutosModel buscaDeProdutosModel)
        {
            buscaDeProdutosModel = buscaDeProdutosModel ?? new BuscaDeProdutosModel();
            buscaDeProdutosModel.Erros.Clear();
            buscaDeProdutosModel.Produtos = await produtoRepository.GetProdutosAsync(buscaDeProdutosModel.Pesquisa);
            if (buscaDeProdutosModel.Produtos.Count <= 0)
                buscaDeProdutosModel.Erros.Add("Nenhum produto encontrado.");
            return View(buscaDeProdutosModel);
        }

        public async Task<IActionResult> Carrossel()
        {
            return View(await produtoRepository.GetProdutosAsync(""));
        }

        public async Task<IActionResult> Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                await pedidoRepository.AddItem(codigo);
            }

            Pedido taskPedido = await pedidoRepository.GetPedido();
            List<ItemPedido> itens = taskPedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }

        public async Task<IActionResult> Cadastro()
        {
            var pedido = await pedidoRepository.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }

            return View(pedido.Cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                return View(await pedidoRepository.UpdateCadastro(cadastro));
            }
            return RedirectToAction("Cadastro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<UpdateQuantidadeResponse> UpdateQuantidade([FromBody]ItemPedido itemPedido)
        {
            return await pedidoRepository.UpdateQuantidade(itemPedido);
        }
    }
}