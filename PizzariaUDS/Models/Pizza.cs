using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace PizzariaUDS.Models
{
    [Table("pizza")]
    public class Pizza
    {
        public Pizza()
        {
            Adicionais = new HashSet<Adicional>();
        }
        [Key]
        public int Id { get; set; }
        public short SaborId { get; set; }
        public short TamanhoId { get; set; }
        public int TempoPreparo { get; private set; }
        public decimal Valor { get; private set; }

        private Sabor _sabor;
        [Computed]
        public Sabor Sabor
        {
            get { return _sabor; }
            set
            {
                //Antes de trocar de sabor subtrai o tempo adicional se tiver
                if (_sabor != null)
                    TempoPreparo -= _sabor?.TempoAdicional ?? 0;

                if (value != null)
                {
                    TempoPreparo += value.TempoAdicional ?? 0;
                    SaborId = value.Id;
                }
                _sabor = value;
            }
        }

        private Tamanho _tamanho;
        [Computed]
        public Tamanho Tamanho
        {
            get { return _tamanho; }
            set
            {
                //Antes de trovar o tamanho subtrai o valor e tempo de preparo do antigo
                if (_tamanho != null)
                {
                    TempoPreparo -= _tamanho.TempoPreparo;
                    Valor -= _tamanho.Valor;
                }

                if (value != null)
                {
                    TempoPreparo += value.TempoPreparo;
                    Valor += value.Valor;
                    TamanhoId = value.Id;
                }
                _tamanho = value;
            }
        }

        [Computed]
        public HashSet<Adicional> Adicionais { get;  set; }

        public void AdicionalAdicional(Adicional adicional)
        {
            if (adicional.TempoPreparo.HasValue)
                TempoPreparo += adicional.TempoPreparo.Value;
            if (adicional.Valor.HasValue)
                Valor += adicional.Valor.Value;

            Adicionais.Add(adicional);
        }

        public bool TemAdicional(Adicional adicional)
        {
            return Adicionais.Any(a => a.Id == adicional.Id);
        }
    }
}
