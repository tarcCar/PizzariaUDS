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
        public int TempoPreparoTotal { get; set; }
        public decimal ValorTotal { get; set; }

        private Pizza _pizza;

        [Computed]
        public Pizza Pizza
        {
            get { return _pizza; }
            set
            {
               
                if(value != null)
                {
                    TempoPreparoTotal = value.TempoPreparo;
                    ValorTotal = value.Valor;
                    PizzaId = value.Id;
                }
                else
                {
                    TempoPreparoTotal = 0;
                    ValorTotal = 0;
                }
                _pizza = value;
            }

        }

    }
}
