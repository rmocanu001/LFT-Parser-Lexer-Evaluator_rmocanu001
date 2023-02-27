using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LFT
{
    internal class Lexer
    {
        private readonly string Text;
        private int Index; // daca nu punem in constructor implicit este 0
        public List<string> Erori = new List<string>();

        public Lexer(string text)
        {
            Text = text;
        }
        private char SimbolCurent
        {
            get {
                if (Index >= Text.Length)
                    return '\0';
                return Text[Index];
            }
        }
        public bool IsKeyWord(string input)
        {
            string pattern = @"\b(int|float|double|string)\b";
            Match m = Regex.Match(input, pattern);
            return m.Success;
        }
        public bool IsVariable(string input)
        {
            string pattern = @"^[a-zA-Z_$][a-zA-Z_$0-9]*$";
            Match m = Regex.Match(input, pattern);
            return m.Success;
        }
        private void IncrementeazaIndex()
        {
            Index++;
        }
        public AtomLexical AtomLexical()
        {
            if (SimbolCurent == '\0')
                return new AtomLexical(TipAtomLexical.TerminatorAtomLexical, "\0", null, Index);
            int punct = 0;
            // numere
            if(char.IsDigit(SimbolCurent)|SimbolCurent=='.')
            {
                int start = Index;
                do
                {
                    if(SimbolCurent=='.')
                        punct++;
                    if (punct>1)
                    {
                        Erori.Add("Numar double invalid");
                    }
                    IncrementeazaIndex();

                } while(char.IsDigit(SimbolCurent)|SimbolCurent=='.');

                int end = Index;
                string nrText = Text.Substring(start, end - start);
                double numar;
                if (Double.TryParse(nrText, out numar))
                {
                    return new AtomLexical(TipAtomLexical.NumarAtomLexical, nrText, numar, start);
                }
                Erori.Add($"Nu s-a putut face parsarea la Int32/Double a numarului {nrText}");
            }
            //ADDED
       
            //spatiu
            if (SimbolCurent == ' ')
            {
                int start = Index;
                do
                {
                    IncrementeazaIndex();

                } while (SimbolCurent == ' ');
                int end = Index;
                string textStr = Text.Substring(start, end - start);
                return new AtomLexical(TipAtomLexical.SpatiuAtomLexical, textStr, textStr, start);
            }

            if (SimbolCurent == '"') //string constant
            {
                var start = Index++;
                while (SimbolCurent != '"' && SimbolCurent!='\0')
                    IncrementeazaIndex();
                if (SimbolCurent == '\0')//am ajuns la finalul inputului si nu s-au inchis ghulimelele
                {
                    Erori.Add("Lexer: String-ul constant nu a fost inchis");
                    //throw new Exception("Lexer: Ghilimelele deschise nu a fost niciodata inchise");
                }
                IncrementeazaIndex();
                var lungime = Index-1 - (start+1);
                var input = Text.Substring(start+1, lungime);
                return new AtomLexical(TipAtomLexical.String, input, input, start+1);
            }
            if (char.IsLetter(SimbolCurent)|| SimbolCurent=='_')
            {
                var start = Index++;
                while (char.IsLetter(SimbolCurent)|| SimbolCurent=='_' && SimbolCurent!='\0')
                    IncrementeazaIndex();
                var lungime = Index - (start);
                var input = Text.Substring(start, lungime);
                if (IsKeyWord(input))
                {
                    return new AtomLexical(TipAtomLexical.TipDataDeclarare, input, input, start);
                }
                if (IsVariable(input))
                {
                    return new AtomLexical(TipAtomLexical.Variabila,input, input, start);
                }
                Erori.Add("Atomul lexical de tip invalid");
            }


                //operatori
                if (SimbolCurent == '+')
                return new AtomLexical(TipAtomLexical.PlusAtomLexical, "+", null, Index++);
            if (SimbolCurent == '-')
                return new AtomLexical(TipAtomLexical.MinusAtomLexical, "-", null, Index++);
            if (SimbolCurent == '*')
                return new AtomLexical(TipAtomLexical.StarAtomLexical, "*", null, Index++);
            if (SimbolCurent == '/')
                return new AtomLexical(TipAtomLexical.SlashAtomLexical, "/", null, Index++);
            if (SimbolCurent == '(')
                return new AtomLexical(TipAtomLexical.ParantezaDeschisaAtomLexical, "(", null, Index++);
            if (SimbolCurent == ')')
                return new AtomLexical(TipAtomLexical.ParantezaInchisaAtomLexical, ")", null, Index++);
            if (SimbolCurent == '=')
                return new AtomLexical(TipAtomLexical.Egal, "=", null, Index++);

            Erori.Add($"Simbol {SimbolCurent.ToString()} invalid");
            return new AtomLexical(TipAtomLexical.InvalidAtomLexical, SimbolCurent.ToString(), null, Index++);
        }
    }
}
