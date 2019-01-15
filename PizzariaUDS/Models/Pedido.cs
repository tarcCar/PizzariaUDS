using System;
using System.Collections.Generic;

namespace PizzariaUDS.Models
{
    public partial class Pedido
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public short TempoPreparoTotal { get; set; }
        public decimal Valor { get; set; }

        public virtual Pizza Pizza { get; set; }
    }
}
