using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace PizzariaUDS.Models
{
    [Table("pizza_adicional")]
    public partial class PizzaAdicional
    {
        [Key]
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public int AdicionalId { get; set; }

        public virtual Adicional Adicional { get; set; }
        public virtual Pizza Pizza { get; set; }
    }
}
