using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFT
{
    abstract class Expresie : NodSintactic 
    {

    }

    class ExpresieBinara : Expresie
    {
        public Expresie Stanga { get; }
        public AtomLexical OperatorExpresie { get; }
        public Expresie Dreapta { get; }

        public ExpresieBinara(Expresie stanga, AtomLexical operatorExpr, Expresie dreapta)
        {
            Stanga = stanga;
            OperatorExpresie = operatorExpr;    
            Dreapta = dreapta;  
        }
        public override TipAtomLexical Tip => TipAtomLexical.ExpresieBinaraAtomLexical;

        public override IEnumerable<NodSintactic> GetCopii()
        {
            // yield return preia toate elementele pe care le dam si returneaza IEnumerable (trebuie sa aiba acelasi tip sau primitiva)
            yield return Stanga; 
            yield return OperatorExpresie;
            yield return Dreapta;
        }
    }

    class ExpresieNumerica : Expresie
    {
        public AtomLexical NumarAtomLexical { get; }

        public ExpresieNumerica(AtomLexical numar)
        {
            NumarAtomLexical = numar;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieNumerica;

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return NumarAtomLexical; // sa transforamam AtomLexical in IEnumerable
            // diferenta dintre List si IEnumerable este ca daca luam elemente dintr-o alta lista
            // list nu va vedea modificarile din lista initiala dar ienumerable va vedea modificarile si va tine cont de ele
        }
    }

    class ExpresieParanteze : Expresie
    {
        public AtomLexical ParantezaDeschisa { get; }
        public Expresie Expresie { get; }
        public AtomLexical ParantezaInchisa { get; }

        public ExpresieParanteze(AtomLexical parantezaDeschisa, Expresie expresie, AtomLexical parantezaInchisa)
        {
            ParantezaDeschisa = parantezaDeschisa;
            Expresie = expresie;
            ParantezaInchisa = parantezaInchisa;
        }
        public override TipAtomLexical Tip => TipAtomLexical.ExpresieParanteza;

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return ParantezaDeschisa;
            yield return Expresie;
            yield return ParantezaInchisa;  
        }
    }
    class ExpresieString : Expresie
    {
        public AtomLexical StringAtom { get; }

        public ExpresieString(AtomLexical str)
        {
            StringAtom = str;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieString;

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return StringAtom; // sa transforamam AtomLexical in IEnumerable
            // diferenta dintre List si IEnumerable este ca daca luam elemente dintr-o alta lista
            // list nu va vedea modificarile din lista initiala dar ienumerable va vedea modificarile si va tine cont de ele
        }
    }
    class ExpresieDeclarare : Expresie
    {
        public AtomLexical StringAtom { get; }

        public ExpresieDeclarare(AtomLexical str)
        {
            StringAtom = str;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieDeclarare;

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return StringAtom; // sa transforamam AtomLexical in IEnumerable
            // diferenta dintre List si IEnumerable este ca daca luam elemente dintr-o alta lista
            // list nu va vedea modificarile din lista initiala dar ienumerable va vedea modificarile si va tine cont de ele
        }
    }

}

