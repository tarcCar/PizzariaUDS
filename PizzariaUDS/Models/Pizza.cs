using Dapper.Contrib.Extensions;
using System.Collections.Generic;
namespace PizzariaUDS.Models
{
    public partial class Pizza
    {
        public Pizza()
        {
            Adicionais = new HashSet<Adicional>();
        }

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
                TempoPreparo -= _sabor?.TempoAdicional ?? 0;
                if (value != null)
                {
                    TempoPreparo += value.TempoAdicional ?? 0;
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
                TempoPreparo -= _tamanho.TempoPreparo;
                Valor -= _tamanho.Valor;
                if (value != null)
                {
                    TempoPreparo += value.TempoPreparo;
                    Valor += value.Valor;
                }
                _tamanho = value;
            }
        }

        [Computed]
        public HashSet<Adicional> Adicionais { get; private set; }

        public void AdicionalAdicional(Adicional adicional)
        {
            Adicionais.Add(adicional);
        }
    }
}
