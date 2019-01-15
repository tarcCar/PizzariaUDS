using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PizzariaUDS.Models
{
    [Table("sabor")]
    public class Sabor
    {
        [Key]
        public short Id { get; set; }
        public string Descricao { get; set; }
        public short? TempoAdicional { get; set; }

    }
}
