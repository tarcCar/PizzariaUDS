using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PizzariaUDS.Models
{
    [Table("Adicional")]
    public partial class Adicional
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public short? TempoPreparo { get; set; }
        public decimal? Valor { get; set; }
    }
}
