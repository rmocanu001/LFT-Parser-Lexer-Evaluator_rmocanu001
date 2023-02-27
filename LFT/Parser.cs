using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFT
{

    internal class Variabila
    {
        public string name { get; }
        public TipAtomLexical tip { get; }
        public object Valoare { get; set; }
        public Variabila(string name, TipAtomLexical tip, object valoare)
        {
            this.name=name;
            this.tip=tip;
            Valoare=valoare;
        }
    }
    
    internal class Parser
    {
        private readonly List<AtomLexical> atomiLexicali = new List<AtomLexical>(); // atomi lexicali de pe nivelul index
        private int index; // reprezinta nivelul din arbore
        public List<string> erori = new List<string>();
        public List<Variabila>variabilas=new List<Variabila>();
        //public List<AtomLexical>atoms=new List<AtomLexical>();  
        private AtomLexical AtomCurent => Avans(0);
        public void clear()
        {
            atomiLexicali.Clear();
            index=0;
            erori.Clear();
            
        }

        public void ParseazaText(string text)
        {
            Lexer l = new Lexer(text);
            AtomLexical at;
            do
            {
                at = l.AtomLexical();
                if (at.TipAtomLexical != TipAtomLexical.SpatiuAtomLexical &&
                    at.TipAtomLexical != TipAtomLexical.InvalidAtomLexical)
                    atomiLexicali.Add(at);

            } 
            while (at.TipAtomLexical != TipAtomLexical.TerminatorAtomLexical);

            erori.AddRange(l.Erori); // adauga mai multe elemente
        }

        private AtomLexical Avans(int k) // sa luam de unde vrem noi un atom lexical, ne miscam cu k pozitii => ajuta la respectarea ordinii operatiilor
        {
            if (index + k >= atomiLexicali.Count)
            {
                return atomiLexicali.ElementAt(atomiLexicali.Count - 1);
            }
            else
                return atomiLexicali.ElementAt(index + k);
        }

       

        private AtomLexical AtomLexicalCurentSIIncrementeaza()
        {
            var atom = AtomCurent;
            index++;
            return atom;
        }

        private AtomLexical VerificaTipAtomLexical(TipAtomLexical tip)
        {
            if(AtomCurent.TipAtomLexical != tip)
            {
                erori.Add($"Atom lexical invalid. Tipul atomului este {AtomCurent.TipAtomLexical}. Se asteapta {tip}.");
                return new AtomLexical(tip,null,null,index);
                // return AtomCurent;
            }
            return AtomLexicalCurentSIIncrementeaza();
        }
        

        public Expresie Parseaza()
        {
            return ParseazaTermen();
        }
        // aplicam acel backtracking (un fel de backtracking) pt a construi arborele sintactic
        // Parseaza -> Parseaza termen -> ParseazaFactor ->ParseazaExpresie => pt ordinea operatiilor
        private Expresie ParseazaTermen()
        {
            var stanga = ParseazaFactor();
            while(AtomCurent.TipAtomLexical == TipAtomLexical.PlusAtomLexical ||
                AtomCurent.TipAtomLexical == TipAtomLexical.MinusAtomLexical)
            {
                var operatorExpr = AtomLexicalCurentSIIncrementeaza();
                var dreapta = ParseazaFactor();
                stanga = new ExpresieBinara(stanga, operatorExpr, dreapta);
            }
            return stanga;
        }

        private Expresie ParseazaFactor()
        {
            var stanga = ParseazaExpresie();
            while (AtomCurent.TipAtomLexical == TipAtomLexical.StarAtomLexical ||
                AtomCurent.TipAtomLexical == TipAtomLexical.SlashAtomLexical)
            {
                var operatorExpr = AtomLexicalCurentSIIncrementeaza();
                var dreapta = ParseazaExpresie();
                stanga = new ExpresieBinara(stanga, operatorExpr, dreapta);
            }
            return stanga;
        }

        // iesirea din backtracking pt parseaza => verifica daca am ajuns o expresie numerica ca inseamna ca mai avem un singur nivel dupa ea

        private Variabila GasesteVariabila(string nume)
        {
            foreach (var v in variabilas)
            {
                if(v.name==nume) return v;
            }
            return null;
        }
        private void DeclaraVariabila(TipAtomLexical tip, string name)
        {
            if (tip==TipAtomLexical.String){
                variabilas.Add(new Variabila(name, tip, "")); }
            if (tip==TipAtomLexical.NumarAtomLexical)
                { variabilas.Add(new Variabila(name, tip, 0)); }
        }
        private void DeclaraSiAtribuie(TipAtomLexical tip, string name, object valoare)
        {
            if (tip==TipAtomLexical.String)
            {
                variabilas.Add(new Variabila(name, tip, valoare));
            }
            if (tip==TipAtomLexical.NumarAtomLexical)
            { variabilas.Add(new Variabila(name, tip, valoare)); }
        }
        private bool Atrbuire(string nume,object valoare)
        {
            var v=GasesteVariabila(nume);
            if (v!=null)
            {
                v.Valoare=valoare;
                return true;
            }
            return false;
        }

        private Expresie ParseazaExpresie()
        {

            if(AtomCurent.TipAtomLexical==TipAtomLexical.ParantezaDeschisaAtomLexical)
            {
                var parantezaDeschisa = AtomLexicalCurentSIIncrementeaza();
                var expresie = Parseaza();
                var parantezaInchisa = VerificaTipAtomLexical(TipAtomLexical.ParantezaInchisaAtomLexical);
                return new ExpresieParanteze(parantezaDeschisa, expresie, parantezaInchisa);
            }
            if (AtomCurent.Tip==TipAtomLexical.NumarAtomLexical)
            {
                var numar = VerificaTipAtomLexical(TipAtomLexical.NumarAtomLexical);
                return new ExpresieNumerica(numar);
            }
            if (AtomCurent.Tip==TipAtomLexical.String)
            {
                var str = VerificaTipAtomLexical(TipAtomLexical.String);
                return new ExpresieString(str);
            }
            if (AtomCurent.Tip==TipAtomLexical.Variabila)
            {
                var nume = VerificaTipAtomLexical(TipAtomLexical.Variabila);
                var urm = AtomLexicalCurentSIIncrementeaza();
                if (urm.Tip==TipAtomLexical.Egal)
                {
                    if (nume.Tip==TipAtomLexical.NumarAtomLexical)
                    {
                        var val = AtomLexicalCurentSIIncrementeaza();
                        if (GasesteVariabila(nume.Text)==null)
                            erori.Add($"Variablia int/double cu numele {nume.Text} nu este declarata");
                        Atrbuire(nume.Text, val.Valoare);

                        nume.Valoare=val.Valoare;
                        return new ExpresieNumerica(nume);
                    }
                    else
                    {
                        var val = AtomLexicalCurentSIIncrementeaza();
                        if (GasesteVariabila(nume.Text)==null)
                            erori.Add($"Variablia string cu numele {nume.Text} nu este declarata");
                        Atrbuire(nume.Text, val.Valoare);

                        nume.Valoare=val.Valoare;
                        return new ExpresieString(nume);
                    }
                }
                if (GasesteVariabila(nume.Text)==null) {
                    erori.Add($"Variablia cu numele {nume.Text} nu este declarata");
                    throw new Exception("Nedeclarare\n");
                }
                var valoare = GasesteVariabila(nume.Text);
                if (valoare.tip==TipAtomLexical.String)
                {
                    nume.TipAtomLexical=TipAtomLexical.String;
                    nume.Valoare=valoare.Valoare;
                    return new ExpresieString(nume);

                }
                if (valoare.tip==TipAtomLexical.NumarAtomLexical)
                {
                    nume.TipAtomLexical=TipAtomLexical.NumarAtomLexical;
                    nume.Valoare=valoare.Valoare;
                    return new ExpresieNumerica(nume);
                }
            }
            if (AtomCurent.Tip==TipAtomLexical.TipDataDeclarare)
            {
                var str = AtomLexicalCurentSIIncrementeaza();
                if (str.Text == "int" || str.Text == "double" || str.Text == "string")
                {
                    var nume = AtomLexicalCurentSIIncrementeaza();
                    var urm = AtomLexicalCurentSIIncrementeaza();
                    if (urm.Tip==TipAtomLexical.TerminatorAtomLexical)
                    {
                        if (str.Text=="int" || str.Text == "double")
                        {
                            DeclaraVariabila(TipAtomLexical.NumarAtomLexical, nume.Text);
                            nume.Valoare=0;
                            return new ExpresieDeclarare(nume);
                        }
                        else
                        {
                            DeclaraVariabila(TipAtomLexical.String, nume.Text);
                            nume.Valoare="";
                            return new ExpresieDeclarare(nume);
                        }
                    }
                    if (urm.Tip==TipAtomLexical.Egal)
                    {
                        if (str.Text=="int" || str.Text == "double")
                        {
                            var val = AtomLexicalCurentSIIncrementeaza();
                            if (GasesteVariabila(nume.Text)!=null)
                                erori.Add($"Variablia int/double cu numele {nume.Text} este deja declarata");
                            DeclaraSiAtribuie(TipAtomLexical.NumarAtomLexical, nume.Text,val.Valoare);

                            nume.Valoare=val.Valoare;
                            return new ExpresieNumerica(nume);
                        }
                        else
                        {
                            var val = AtomLexicalCurentSIIncrementeaza();
                            if (GasesteVariabila(nume.Text)!=null)
                                erori.Add($"Variablia string cu numele {nume.Text} este deja declarata");
                            DeclaraSiAtribuie(TipAtomLexical.String, nume.Text,val.Valoare);

                            nume.Valoare=val.Valoare;
                            return new ExpresieString(nume);
                        }
                    }
                }
            }
            return null;
        }

    }
}
