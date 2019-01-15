using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PizzariaUDS.Models
{
    public partial class Tamanho
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public short TempoPreparo { get; set; }
    }
}
