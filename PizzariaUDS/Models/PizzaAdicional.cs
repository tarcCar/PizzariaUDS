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

        private Adicional _adicional;

        [Computed]
        public Adicional Adicional
        {
            get { return _adicional; }
            set
            {
                _adicional = value;
                if(value != null)
                {
                    this.AdicionalId = value.Id;
                }
            }
        }

        private Pizza _pizza;

        [Computed]
        public Pizza Pizza
        {
            get { return _pizza; }
            set
            {
                _pizza = value;
                if (value != null)
                {
                    this.PizzaId = value.Id;
                }
            }
        }
    }
}
