using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LFT
{
    
    class Evaluator
    {
        public string substract_expresie(string s1, string s2)
        {
            return s1.ToString().Replace(s2.ToString(),""); 
        }
        public Expresie Expresie { get; }

        public Evaluator(Expresie expresie)
        {
            Expresie = expresie;
        }

        public dynamic Evalueaza()
        {
            return EvalueazaExpresie(Expresie);
        }

        public dynamic EvalueazaExpresie(Expresie expresie)
        {
            if(expresie is ExpresieNumerica n)
            {
                return Math.Round((double)n.NumarAtomLexical.Valoare,2);
            }

            if (expresie is ExpresieString k)
            {
                return (string)k.StringAtom.Valoare;
            }
            

            if (expresie is ExpresieBinara b)
            {
                var stanga = b.Stanga;
                var operatorExpresie = b.OperatorExpresie;
                var dreapta = b.Dreapta;

                if (operatorExpresie.Tip == TipAtomLexical.PlusAtomLexical)
                {
                    if (stanga.Tip!=dreapta.Tip)
                    {
                        throw new Exception("Operatie imposibila tipuri diferite");
                    }
                    return EvalueazaExpresie(stanga) + EvalueazaExpresie(dreapta);
                }
                
                if (operatorExpresie.Tip == TipAtomLexical.MinusAtomLexical)
                {
                    if((stanga.Tip==TipAtomLexical.ExpresieString|stanga.Tip==TipAtomLexical.ExpresieParanteza)|(dreapta.Tip==TipAtomLexical.ExpresieString|dreapta.Tip==TipAtomLexical.ExpresieParanteza))
                        return substract_expresie(EvalueazaExpresie(stanga), EvalueazaExpresie(dreapta));
                    return EvalueazaExpresie(stanga) - EvalueazaExpresie(dreapta);
                }
                if (operatorExpresie.Tip == TipAtomLexical.StarAtomLexical )
                {
                    if(stanga.Tip==TipAtomLexical.ExpresieString|dreapta.Tip==TipAtomLexical.ExpresieString)
                    {
                        throw new Exception("Operatie imposibila de inmultire cu stringuri");
                    }
                    return EvalueazaExpresie(stanga) * EvalueazaExpresie(dreapta);
                }
                if (operatorExpresie.Tip == TipAtomLexical.SlashAtomLexical)
                {
                    if (stanga.Tip==TipAtomLexical.ExpresieString|dreapta.Tip==TipAtomLexical.ExpresieString)
                    {
                        throw new Exception("Operatie imposibila de impartire cu stringuri");
                    }
                    if (EvalueazaExpresie(dreapta) == 0)
                    {
                        throw new Exception("Impartire la 0!");
                    }
                    return Math.Round(EvalueazaExpresie(stanga) / EvalueazaExpresie(dreapta),2);

                }
            }
                if (expresie is ExpresieParanteze p)
                {
                    return EvalueazaExpresie(p.Expresie);
                }

                throw new Exception("Tip exceptie necunoscut");
      
            
        }
    }
}
