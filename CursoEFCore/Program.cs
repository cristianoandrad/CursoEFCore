using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //InserirDados();            
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            RemoveRegistro();
        }
        private static void RemoveRegistro()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.Find(2);
            var cliente = new Cliente { Id = 3 };

            //db.Clientes.Remove(cliente);
            //db.Remove(cliente);            
            db.Entry(cliente).State = EntityState.Deleted;

            
            db.SaveChanges();

        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            //var cliente = db.Clientes.FirstOrDefault(p => p.Id == 1);
            //var cliente = db.Clientes.Find(1);

            var cliente = new Cliente
            {
                Id = 1
            };


            cliente.Nome = "Cliente alterado 1";

            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado 3",
                Telefone = "778787878"
            };

            db.Attach(cliente);

            
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            //db.Clientes.Update(cliente);

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count());

        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido teste",
                Status = StatusPedidos.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };
            db.Pedidos.Add(pedido);

            db.SaveChanges();

        }

        public static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultarPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();
            //var consultaPorMetodo = db.Clientes.AsNoTracking().Where(p => p.Id > 0).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();
            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando cliente: {cliente.Id}.");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {

            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "123456454567",
                Valor = 10m,
                TipopProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Cristiano",
                CEP = "99999",
                Cidade = "Joao Pessoa",
                Estado = "PB",
                Telefone = "12121332"
            };

            var listaClientes = new[]
            {
                new Cliente
            {
                Nome = "teste1",
                CEP = "99999",
                Cidade = "Joao Pessoa",
                Estado = "PB",
                Telefone = "12121332"
            },

            new Cliente
            {
                Nome = "teste2",
                CEP = "99999",
                Cidade = "Joao Pessoa",
                Estado = "PB",
                Telefone = "12121332"
            },

        };

            using var db = new Data.ApplicationContext();

            //db.AddRange(produto, cliente);
            db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total Registros: {registros}.");


        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "123456454567",
                Valor = 10m,
                TipopProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();

            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total Registros: {registros}.");



        }
    }
}
