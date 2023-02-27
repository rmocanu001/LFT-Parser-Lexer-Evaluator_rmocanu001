using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LFT
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            var parser = new Parser();
            while (true)
            {
                parser.clear();
                Console.Write("> ");
                var text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text))
                    return;
                var culoare = Console.ForegroundColor;

                parser.ParseazaText(text);
                if (parser.erori.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var eroare in parser.erori)
                        Console.WriteLine(eroare);

                    Console.ForegroundColor = culoare;
                }
                else
                {
                    var arboreSintactic = parser.Parseaza();
                    AfiseazaArbore(arboreSintactic);
                    if (parser.erori.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (var eroare in parser.erori)
                            Console.WriteLine(eroare);

                        Console.ForegroundColor = culoare;
                    }
                    else
                    {
                        try
                        {
                            Evaluator e = new Evaluator(arboreSintactic);
                            dynamic result = e.Evalueaza();
                            Console.WriteLine(result);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                        }
                    }
                }
                
            }

        }

        static void AfiseazaArbore(NodSintactic nod, string indentare = "", bool ultimulNod = true)
        {
            var prefix = ultimulNod ? "└──" : "├──";
            Console.Write(indentare);
            Console.Write(prefix);
            Console.Write(nod.Tip);



            if (nod is AtomLexical t && t.Valoare != null)
            {
                Console.Write(" ");
                Console.Write(t.Valoare);
            }

            Console.WriteLine();

            indentare += ultimulNod ? "    " : "|   ";

            var ultimulCopil = nod.GetCopii().LastOrDefault();

            foreach (var c in nod.GetCopii())
            {
                AfiseazaArbore(c, indentare, c == ultimulCopil);
            }
        }
    }

    enum TipAtomLexical
    {
        NumarAtomLexical,
        PlusAtomLexical,
        MinusAtomLexical,
        StarAtomLexical,
        SlashAtomLexical,
        ParantezaDeschisaAtomLexical,
        ParantezaInchisaAtomLexical,
        SpatiuAtomLexical,
        InvalidAtomLexical,
        TerminatorAtomLexical,
        ExpresieBinaraAtomLexical,
        ExpresieNumerica,
        ExpresieParanteza,
        ExpresieString,
        Double,
        String,
        Variabila,
        TipDataDeclarare,
        ExpresieDeclarare,
        ExpresieAtribuire,
        Egal
        
    }


}
