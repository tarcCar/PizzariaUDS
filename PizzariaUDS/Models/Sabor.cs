using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PizzariaUDS.Models
{
    public partial class Sabor
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public short? TempoAdicional { get; set; }

    }
}
