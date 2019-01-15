using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace PizzariaUDS.Models
{
    [Table("pedido")]
    public class Pedido
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public short TempoPreparoTotal { get; set; }
        public decimal Valor { get; set; }

        [Computed]
        public Pizza Pizza { get; set; }
    }
}
