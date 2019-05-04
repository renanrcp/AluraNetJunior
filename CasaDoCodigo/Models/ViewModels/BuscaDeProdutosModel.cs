﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models.ViewModels
{
    public class BuscaDeProdutosModel
    {
        public List<string> Erros { get; set; } = new List<string>();
        public IList<Produto> Produtos { get; set; }
        public string Pesquisa { get; set; }
    }
}