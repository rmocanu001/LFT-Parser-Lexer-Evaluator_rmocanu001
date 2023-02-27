using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFT
{
    abstract class NodSintactic // este o clasa abstracta ca poate avea mai multe tipuri de noduri
    {
        public abstract TipAtomLexical Tip { get; }
        public abstract IEnumerable<NodSintactic> GetCopii();

    }
}
