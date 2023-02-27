using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFT
{
    class AtomLexical : NodSintactic
    {
        public TipAtomLexical TipAtomLexical { get; set; }
        public string Text { get; } // text = "123"
        public object Valoare { get; set; } // object ca poate fi de mai multe tipuri // int val = 123
        public int Index { get; }

        public override TipAtomLexical Tip => TipAtomLexical;

        public AtomLexical(TipAtomLexical tipAtomLexical, string text,
            object valoare, int index)
        {
            TipAtomLexical = tipAtomLexical;
            Text = text;
            Valoare = valoare;
            Index = index;
        }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            return Enumerable.Empty<NodSintactic>();        // daca ajungem la nod sintactic nu mai avem copii
        }
    }
}
