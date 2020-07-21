using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Antipalindromic_numbers
{
    public partial class Form1 : Form
    {
        static ulong M = 908107198;                     // Nejnižší číslo, které chceme zkoumat
        static ulong N = 908107202;            //Nejvyšší číslo, které chceme zkoumat
        static uint p = 25;
        static uint Krok = 6;
        ulong B = 908107210;                                        //Báze
        static int L = 4;                                        //Druhá báze, parametr
        static char Mode = 'c';
        static bool outputDoSouboru = false;

        public class ControlWriter : TextWriter
        {
            private Control textbox;
            public ControlWriter(Control textbox)
            {
                this.textbox = textbox;
            }

            public override void Write(char value)
            {
                textbox.Text += value;
            }

            public override void Write(string value)
            {
                textbox.Text += value;
            }

            public override Encoding Encoding
            {
                get { return Encoding.ASCII; }
            }
        }
        public class Antipa
        {
            //m = mezery mezi palindromy a antipalindromy
            //p = prvočísla mezi antipalindromy
            //q = prvočísla mezi palindromy
            //b = antipalindromy ve více bázích
            //c = všechny báze, ve kterých je číslo antipalindrom
            //s = součty antipalindromů
            //t = součty antipalindromů pouze pro palindromy
            //e = mocniny
            //f = mocniny jen pro nejvhodnější báze
            private static bool JePalindrom(ulong n, uint b, int[] Soustava)
            {
                bool palindrom = true;
                int pocetCislic = 1;
                ulong zbytek = n;
                int konec = 0;
                while (zbytek > (ulong)b - 1)
                {
                    konec = (int)(zbytek % (ulong)b);
                    zbytek = (zbytek - (ulong)konec) / (ulong)b;
                    Soustava[40 - pocetCislic] = konec;
                    pocetCislic++;
                }
                Soustava[0] = pocetCislic;
                Soustava[40 - pocetCislic] = (int)zbytek;
                for (int j = 0; j < pocetCislic; j++)
                {
                    if (Soustava[39 - j] != Soustava[40 - pocetCislic + j])
                        palindrom = false;
                }
                return palindrom;
            }

            static void Vypis(int co)
            {
                Console.Write(co);
            }
            static void Vypis(string co)
            {
                if (co == "\n")
                {
                    Console.WriteLine();
                    return;
                }
                Console.Write(co);
            }
            static void Vypis(ulong co)
            {
                Console.Write(co);
            }
            static bool JeAntipalindrom(ulong n, uint b, int[] Soustava)
            {
                bool antipalindrom = true;
                int pocetCislic = 1;
                uint zbytek = (uint)n;
                uint konec;
                while (zbytek > b - 1)
                {
                    konec = zbytek % b;
                    zbytek = (zbytek - konec) / b;
                    Soustava[40 - pocetCislic] = (int)konec;
                    pocetCislic++;
                }
                Soustava[0] = pocetCislic;
                Soustava[40 - pocetCislic] = (int)zbytek;
                for (int j = 0; j < pocetCislic; j++)
                {
                    if (Soustava[39 - j] != b - 1 - Soustava[40 - pocetCislic + j])
                        antipalindrom = false;
                }
                return antipalindrom;
            }
            static bool JePrvocislo(int n)
            {
                if (n < 2) return false;
                if ((n == 2) || (n == 3)) return true;
                if ((n % 2 == 0) || (n % 3 == 0))
                    return false;
                for (int e = 5; e * e <= n; e = e + 6)
                    if (n % e == 0 || n % (e + 2) == 0)
                        return false;

                return true;
            }
            static bool JePrvocislo(ulong n)
            {
                if (n < 2) return false;
                if ((n == 2) || (n == 3)) return true;
                if ((n % 2 == 0) || (n % 3 == 0))
                    return false;
                for (ulong e = 5; e * e <= n; e = e + 6)
                    if (n % e == 0 || n % (e + 2) == 0)
                        return false;

                return true;
            }
            static long mocnina(long i, int mocnina)
            {
                long nasobek = 1;
                for (int k = 0; k < mocnina; k++)
                    nasobek *= i;
                return nasobek;
            }
            static long testovane = 0;
            static int sito = 0;
            static bool JePrvocislo2(ulong n, ulong[] prvocisla)
            {
                if (n < 2) return false;
                for (uint e = 0; e < p; e++)
                {
                    if (n > prvocisla[e])
                        if (n % prvocisla[e] == 0)
                            return false;
                }
                return true;
            }
            public static void Antipalindromy(ulong m, ulong n, ulong krok, int b,
                                int l, char mode, bool vystupDoSouboru)
            {
                if (mode == 'm')
                {
                    L = b;
                    Vypis("Showing palindromic and antipalindromic numbers in base ");
                    Vypis(b);
                    Vypis(" from 1 to ");
                    Vypis(N);
                    Vypis(": ");
                    Vypis("\n");
                    Vypis("The third column represents the number of digits");
                    Vypis("\n");
                    Vypis("\n");
                    int delkaBaze = 1;
                    int z = b;
                    while (z > 10)
                    {
                        delkaBaze++;
                        z /= 10;
                    }
                    int mezeraMeziAnti = 0;
                    int mezeraMeziPali = 0;
                    int maxMezeraA = 0;
                    int maxMezeraP = 0;
                    for (ulong i = 1; i < N + 1; i++)
                    {
                        int[] Soustava = new int[40];
                        if (JeAntipalindrom(i, (uint)b, Soustava))
                        {
                            mezeraMeziPali++;
                            mezeraMeziAnti = 0;
                            if (mezeraMeziPali > maxMezeraP)
                                maxMezeraP++;
                            Vypis("Antipalindromic number: ".PadRight(30));
                            Vypis(i.ToString().PadRight(10));
                            Vypis(Soustava[0].ToString().PadRight(5));
                            Vypis(" ");

                            for (int r = 40 - Soustava[0]; r < 40; r++)
                            {
                                Vypis(Soustava[r].ToString().PadRight(delkaBaze));
                            }
                            Vypis("\n");
                        }
                        if (JePalindrom(i, (uint)b, Soustava))
                        {
                            mezeraMeziAnti++;
                            mezeraMeziPali = 0;
                            if (mezeraMeziAnti > maxMezeraA)
                                maxMezeraA++;
                            Vypis("Palindromic number: ".PadRight(30));
                            Vypis(i.ToString().PadRight(10));
                            Vypis(Soustava[0].ToString().PadRight(5));
                            Vypis(" ");
                            for (int r = 40 - Soustava[0]; r < 40; r++)
                            {
                                Vypis(Soustava[r].ToString().PadRight(delkaBaze));
                            }
                            Vypis("\n");
                        }
                    }

                    Vypis("\n");
                    Vypis("The maximum number of antipalindromic numbers between two palindromic numbers from 1 to ");
                    Vypis(n);
                    Vypis(" in base ");
                    Vypis(b);
                    Vypis(" is ");
                    Vypis(maxMezeraP);
                    Vypis("\n");
                    Vypis("The maximum number of palindromic numbers between two antipalindromic numbers from 1 to ");
                    Vypis(n);
                    Vypis(" in base ");
                    Vypis(b);
                    Vypis(" is ");
                    Vypis(maxMezeraA);
                    Vypis("\n");
                    return;
                }
                if (mode == 'q')
                {
                    Vypis("Showing all palindromic primes from ");
                    Vypis(M);
                    Vypis(" to ");
                    Vypis(N);
                    Vypis(" in base ");
                    Vypis(b);
                    Vypis(":");
                    Vypis("\n");
                    Vypis("\n");
                    Vypis("               Number      Expansion");
                    Vypis("\n");

                    int pocetPalPr = 0;
                    int[] Soustava = new int[40];
                    int delkaBaze = 0;
                    int kob = b;
                    while (kob >= 10)
                    {
                        kob /= 10;
                        delkaBaze++;
                    }

                    for (ulong i = 1; i < N; i++)
                    {
                        if (JePrvocislo(i) && (JePalindrom(i, (uint)b, Soustava)))
                        {
                            Vypis(i.ToString().PadLeft(21));
                            Vypis("      ");
                            for (int s = 40 - Soustava[0]; s < 40; s++)
                            {
                                Vypis(Soustava[s].ToString().PadRight(delkaBaze));
                                Vypis(" ");
                            }
                            Vypis("\n");
                            pocetPalPr++;
                        }
                    }

                    Vypis("\n");
                    Vypis("Prime entries: ");
                    Vypis(pocetPalPr);
                    return;
                }
                if (mode == 'p')
                {
                    Vypis("Showing all antipalindromic primes from ");
                    Vypis(M);
                    Vypis(" to ");
                    Vypis(N);
                    Vypis(" in base ");
                    Vypis(b);
                    Vypis(":");
                    Vypis("\n");
                    Vypis("\n");
                    Vypis("               Number           Expansion");
                    Vypis("\n");
                    testovane = 0;
                    sito = 0;
                    ulong kN = N;
                    while (kN > 100)
                    {
                        p = p * 5;
                        kN = kN / 100;
                    }
                    ulong[] prvocisla = new ulong[p];
                    while (sito < p)
                    {
                        while (!JePrvocislo((int)testovane))
                        {
                            testovane++;
                        }
                        prvocisla[sito] = (ulong)testovane;
                        testovane++;
                        sito++;
                    }
                    int unIndex = 0;
                    long[] kroky = new long[5];
                    kroky[0] = 12;
                    kroky[1] = 84;
                    kroky[2] = 24;
                    kroky[3] = 24;
                    kroky[4] = 636;
                    long nasobek0 = 636;
                    long mezera0 = 120;
                    long dPole = 3;
                    long kr = 24;
                    int delkaBaze = 1;
                    int z = b;
                    while (z > 10)
                    {
                        delkaBaze++;
                        z /= 10;
                    }

                    bool novaDelka = false;
                    int delka = 0;
                    ulong predchozi = 0;

                    int pocetPrvocisel = 0;
                    if ((JePrvocislo((b - 1) / 2)) && (b % 2 == 1))
                    {
                        int temp = (b - 1) / 2;
                        string tem = temp.ToString();
                        Vypis("Antipalindromic number: ".PadRight(25));
                        Vypis(tem.PadRight(10));
                        Vypis(1);
                        Vypis(tem.PadRight(28));
                        Vypis("        ");
                        Vypis(tem);
                        Vypis("\n");
                    }
                    for (ulong i = M; i < N + 1; i = i + krok)
                    {
                        int[] Soustava = new int[40];
                        bool zapis = JeAntipalindrom(i, (uint)b, Soustava);
                        if (Soustava[0] != delka) novaDelka = true;
                        delka = Soustava[0];
                        if (novaDelka)
                        {
                            if ((Soustava[0] > 6) && (Soustava[0] % 2 == 1))
                            {
                                nasobek0 = nasobek0 * 9;
                                mezera0 = mezera0 * 3 + 48;
                                kr = kr * 3;
                                long mocnina = 1;
                                long kopieKr = kr;
                                dPole = dPole * 3;
                                kroky = new long[dPole];
                                for (long r = 0; r < dPole; r++)
                                    kroky[r] = 0;
                                while (kopieKr >= 24)
                                {
                                    for (long r = 0; r < dPole - 1; r++)
                                    {
                                        if ((r + 1) % mocnina == 0)
                                            kroky[r] = kroky[r] + kopieKr;
                                    }
                                    mocnina *= 3;
                                    kopieKr /= 3;
                                }
                                kroky[dPole - 1] = nasobek0 - mezera0;
                                nasobek0 = kroky[dPole - 1];
                                unIndex = 0;
                                novaDelka = false;
                            }

                        }
                        krok = (ulong)kroky[unIndex];
                        unIndex++;
                        if (JeAntipalindrom(i, (uint)b, Soustava))
                        {
                            if (JePrvocislo2(i, prvocisla))
                            {
                                Vypis(i.ToString().PadLeft(21));
                                Vypis("           ");
                                for (int s = 40 - Soustava[0]; s < 40; s++)
                                {
                                    Vypis(Soustava[s].ToString().PadRight(delkaBaze));
                                    Vypis(" ");
                                }
                                Vypis("\n");
                                predchozi = i;
                                pocetPrvocisel++;
                            }
                        }
                    }
                    Vypis("Prime entries: ");
                    Vypis(pocetPrvocisel);
                    Vypis("\n");
                    return;
                }
                if (mode == 'b')
                {

                    Vypis("Showing all numbers with antipalindromic expansions in bases ");
                    if (krok == 6)
                    {
                        Vypis(l);
                        Vypis(" and ");
                        Vypis(b);
                    }
                    else
                    {
                        Vypis(krok);
                        Vypis(", ");
                        Vypis(l);
                        Vypis(", and ");
                        Vypis(b);
                    }

                    Vypis(" between 1 and ");
                    Vypis(N);
                    Vypis(": ");
                    Vypis("\n");
                    Vypis("\n");
                    bool neco = false;
                    int delkaBaze = 1;
                    long z = Math.Max((int)Math.Max(b, l), (int)krok);
                    while (z > 10)
                    {
                        delkaBaze++;
                        z = z / 10;
                    }
                    for (long i = 1; (ulong)i < N + 1; i++)
                    {
                        int[] Soustava1 = new int[40];
                        int[] Soustava2 = new int[40];
                        int[] Soustava3 = new int[40];
                        if (JeAntipalindrom((ulong)i, (uint)b, Soustava1))
                        {
                            if (JeAntipalindrom((ulong)i, (uint)l, Soustava2))
                            {
                                if ((JeAntipalindrom((ulong)i, (uint)krok, Soustava3)) || (krok == 6))
                                {
                                    neco = true;
                                    Vypis("Antipalindromic number: ");
                                    Vypis(i.ToString().PadRight(10));
                                    Vypis("base: ");
                                    Vypis(b.ToString().PadRight(4));
                                    Vypis("   ");
                                    for (int r = 40 - Soustava1[0]; r < 40; r++)
                                    {
                                        Vypis(Soustava1[r].ToString().PadRight(delkaBaze + 1));
                                    }
                                    Vypis("\n");
                                    Vypis("base: ".PadLeft(40));
                                    Vypis(l.ToString().PadRight(4));
                                    Vypis("   ");
                                    for (int r = 40 - Soustava2[0]; r < 40; r++)
                                    {
                                        Vypis(Soustava2[r].ToString().PadRight(delkaBaze + 1));
                                    }
                                    Vypis("\n");
                                    if (krok != 6)
                                    {
                                        Vypis("base: ".PadLeft(40));
                                        Vypis(krok.ToString().PadRight(4));
                                        Vypis("   ");
                                        for (int r = 40 - Soustava3[0]; r < 40; r++)
                                        {
                                            Vypis(Soustava3[r].ToString().PadRight(delkaBaze + 1));
                                        }
                                        Vypis("\n");
                                    }
                                    Vypis("\n");
                                }
                            }
                        }
                    }
                    if (!neco)
                    {
                        Vypis("There are no such numbers.");
                    }
                    return;
                }
                if (mode == 'c')
                {
                    ulong[] mnuly = new ulong[100];
                    int indexMnuly = 0;
                    Vypis("For numbers ");
                    Vypis(M);
                    Vypis(" to ");
                    Vypis(N);
                    Vypis(", showing all bases in which the number has an antipalindromic expansion: ");
                    Vypis("\n");
                    Vypis("\n");
                    for (ulong i = M; i <= N; i++)
                    {
                        int pocet = 0;
                        bool prvni = true;
                        Vypis("number: ");
                        Vypis(i);
                        Vypis("\n");
                        Vypis("bases: ");
                        int[] Soustava = new int[40];
                        for (int j = 2; (j <= b) && (i >= (ulong)j); j++)
                        {
                            if (JeAntipalindrom(i, (uint)j, Soustava))
                            {
                                pocet++;
                                if (!prvni) Vypis(", ");
                                Vypis(j);
                                prvni = false;
                            }
                        }
                        if (!prvni) Vypis(", ");
                        else { mnuly[indexMnuly] = i; indexMnuly++; }
                        Vypis(2 * i + 1);
                        pocet++;
                        Vypis("\n");
                        Vypis("number of bases: ");
                        Vypis(pocet);
                        Vypis("\n");
                        Vypis("\n");
                    }
                    Vypis("1 Antipalindromic base: ");
                    for (int k = 0; k < indexMnuly; k++)
                    {
                        Vypis(mnuly[k]);
                        Vypis(" ");
                    }
                    Vypis("\n");
                    return;
                }
                if ((mode == 's') || (mode == 't'))
                {
                    if (mode == 's')
                    {
                        Vypis("Showing numbers from ");
                    }
                    if (mode == 't')
                    {
                        Vypis("Showing palindromic numbers from ");
                    }
                    bool neco = false;
                    Vypis((int)M);
                    Vypis(" to ");
                    Vypis((int)N);
                    Vypis(" as a sum of three antipalindromic numbers in base 3:");
                    Vypis("\n");
                    Vypis("\n");
                    Assembly asm = Assembly.GetExecutingAssembly();
                    StreamReader sr = new StreamReader(asm.GetManifestResourceStream("Antipalindromic_numbers.Resources.input.txt"));
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    const int smb = 1312;
                    int[] mezery = new int[smb - 1];
                    int i = 0;
                    int next;
                    int prev = 0;
                    int nasobek;
                    string temp = sr.ReadLine();
                    while (i < smb - 1)
                    {
                        mezery[i] = 0;
                        next = prev + 1;
                        while (temp[next] != ',')
                        {
                            next++;
                        }
                        prev = next;
                        next--;
                        nasobek = 1;
                        while (temp[next] != ',')
                        {
                            mezery[i] += nasobek * (temp[next] - 48);
                            nasobek = nasobek * 10;
                            next--;
                        }
                        i++;
                    }
                    long[] scitance = new long[smb];
                    scitance[0] = 0;

                    for (int x = 0; x < smb - 1; x++)
                        scitance[x + 1] = scitance[x] + mezery[x];
                    int[] soucet = { 0, 0, 0 };
                    int[] soustava = new int[40];
                    for (int r = (int)M; r <= (int)N; r++)
                    {
                        if ((mode == 't') && (!JePalindrom((ulong)r, (uint)b, soustava))) continue;
                        for (int j = smb - 1; j >= 0; j--)
                        {
                            if (scitance[j] > r) continue;
                            if (scitance[j] * 3 < r) break;
                            soucet[0] = (int)scitance[j];
                            for (int k = 0; k < smb; k++)
                            {
                                soucet[1] = (int)scitance[k];
                                for (int s = 0; s < smb; s++)
                                {
                                    soucet[2] = (int)scitance[s];
                                    if (soucet[0] + soucet[1] + soucet[2] >= r)
                                    {
                                        break;
                                    }
                                }
                                if (soucet[0] + soucet[1] + soucet[2] == r)
                                {
                                    break;
                                }
                            }
                            if (soucet[0] + soucet[1] + soucet[2] == r)
                            {
                                break;
                            }

                        }
                        if (soucet[0] + soucet[1] + soucet[2] != r)
                        {
                            neco = true;
                            Vypis(r.ToString().PadRight(8));
                            Vypis(" CANNOT BE A SUM OF THREE ANTIPALINDROMIC NUMBERS IN BASE 3");
                            Vypis("\n");
                            Vypis("\n");
                            continue;
                        }
                        neco = true;
                        Vypis("number: ");
                        Vypis(r);
                        Vypis("\n");
                        Vypis("\n");
                        int nejvetsiPocetCislic = 1;
                        long zbytek = r;
                        int[] Soustava = new int[40];
                        while (zbytek > b - 1)
                        {
                            int konec = (int)zbytek % b;
                            zbytek = (zbytek - konec) / b;
                            Soustava[40 - nejvetsiPocetCislic] = konec;
                            nejvetsiPocetCislic++;
                        }
                        Soustava[40 - nejvetsiPocetCislic] = (int)zbytek;
                        for (int t = 40 - nejvetsiPocetCislic; t < 40; t++)
                        {
                            Vypis(" ");
                            Vypis(Soustava[t]);
                            Vypis(" ");
                        }
                        Vypis("  =");
                        Vypis("\n");

                        for (int s0 = 0; s0 < 3; s0++)
                        {
                            int[] Soustava2 = new int[40];
                            bool zapis = JeAntipalindrom((ulong)soucet[s0], (uint)b, Soustava2);
                            for (int s1 = Soustava2[0]; s1 < nejvetsiPocetCislic; s1++)
                            {
                                Vypis("   ");
                            }
                            for (int s2 = 40 - Soustava2[0]; s2 < 40; s2++)
                            {
                                Vypis(" ");
                                Vypis(Soustava2[s2]);
                                Vypis(" ");
                            }
                            if (s0 != 1)
                            {
                                Vypis("  +");
                            }
                            Vypis("\n");
                            switch (s0)
                            {
                                case 0: s0++; break;
                                case 1: s0++; break;
                                case 2: s0 -= 2; break;

                            }
                        }
                        Vypis("\n");

                    }
                    if (!neco)
                    {
                        Vypis("There are no such numbers.");
                    }
                    return;

                }
                if (mode == 'e')
                {
                    if (b - (int)m != 3)
                    {
                        Vypis("Showing ");
                        if (l == 2)
                        {
                            Vypis("squares");
                        }
                        if (l == 3)
                        {
                            Vypis("third powers");
                        }
                        if (l > 3)
                        {
                            Vypis(l);
                            Vypis("th powers");
                        }
                        Vypis(" from 1 to ");
                        Vypis(N);
                        Vypis(" that have an antipalindromic expansion in bases 2 to ");
                        Vypis(b);
                        Vypis(":");
                        Vypis("\n");
                    }
                    for (int c = (int)m+1; c <= b; c++)
                    {
                        Vypis("\n");
                        Vypis("base: ");
                        Vypis(c);
                        Vypis("\n");
                        int pocetAnti = 0;
                        for (long i = 1; mocnina(i, l) < (long)N; i++)
                        {
                            int[] Soustava = new int[40];
                            if (JeAntipalindrom((ulong)mocnina(i, l), (uint)c, Soustava))
                            {
                                pocetAnti++;
                                string str = mocnina(i, l).ToString();
                                Vypis(str.PadRight(18));
                                for (int r = 40 - Soustava[0]; r < 40; r++)
                                {
                                    Vypis(Soustava[r]);
                                    Vypis(" ");
                                }
                                Vypis("\n");
                            }
                        }
                        Vypis("Entries: ");
                        Vypis(pocetAnti);
                        Vypis("\n");
                    }
                    return;
                }

                if (mode == 'f')
                {
                    int hledanaBaze = 0;
                    for (long i = 1; hledanaBaze * hledanaBaze < (int)N; i++)
                    {
                        hledanaBaze = (int)mocnina(i, l);
                        if (i > 1)
                            Antipalindromy((ulong)hledanaBaze - 1, n, krok, hledanaBaze + 2, l, 'e', vystupDoSouboru);
                    }
                    return;
                }
                


                
                
                
            }
        }
        public Form1()
        {
            InitializeComponent();
            File.Create("output.txt").Close();
            File.Create("outputF.txt").Close();
            Console.SetOut(new ControlWriter(textBox6));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        label2.Visible = true;
                        label2.Text = "Base:";
                        textBox1.Visible = true;
                        label3.Visible = false;
                        textBox2.Visible = false;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = false;
                        textBox4.Visible = false;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 'm';
                        break;
                    }
                case 1:
                    {
                        label2.Visible = true;
                        label2.Text = "Base:";
                        textBox1.Visible = true;
                        label3.Visible = false;
                        textBox2.Visible = false;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = true;
                        textBox4.Visible = true;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 'q';
                        break;
                    }
                case 2:
                    {
                        label2.Visible = false;
                        textBox1.Visible = false;
                        label3.Visible = false;
                        textBox2.Visible = false;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = false;
                        textBox4.Visible = false;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 'p';
                        break;
                    }
                case 3:
                    {
                        label2.Visible = true;
                        label2.Text = "Base:";
                        textBox1.Visible = true;
                        label3.Visible = true;
                        label3.Text = "Second base:";
                        textBox2.Visible = true;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = false;
                        textBox4.Visible = false;
                        label8.Visible = true;
                        checkBox2.Visible = true;
                        Mode = 'b';
                        break;
                    }
                case 4:
                    {
                        label2.Visible = false;
                        textBox1.Visible = false;
                        label3.Visible = false;
                        textBox2.Visible = false;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = true;
                        textBox4.Visible = true;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 'c';
                        break;
                    }
                case 5:
                    {
                        label2.Visible =false;
                        textBox1.Visible = false;
                        label3.Visible = false;
                        textBox2.Visible = false;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = true;
                        textBox4.Visible = true;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 's';
                        break;
                    }
                case 6:
                    {
                        label2.Visible = false;
                        textBox1.Visible = false;
                        label3.Visible = false;
                        textBox2.Visible = false;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = true;
                        textBox4.Visible = true;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 't';
                        break;
                    }
                case 7:
                    {

                        label2.Visible = true;
                        label2.Text = "Maximal base:";
                        textBox1.Visible = true;
                        label3.Visible = true;
                        label3.Text = "Exponent:";
                        textBox2.Visible = true;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = false;
                        textBox4.Visible = false;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 'e';
                        break;
                    }
                case 8:
                    {
                        label2.Visible = false;
                        textBox1.Visible = false;
                        label3.Visible = true;
                        label3.Text = "Exponent:";
                        textBox2.Visible = true;
                        label4.Visible = false;
                        textBox3.Visible = false;
                        label5.Visible = false;
                        textBox4.Visible = false;
                        label8.Visible = false;
                        checkBox2.Visible = false;
                        checkBox2.Checked = false;
                        Mode = 'f';
                        break;
                    }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            try
            {
                B = Convert.ToUInt64(str);
            }
            catch (Exception) { }
        }

        private void label8_Click(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label4.Visible = true;
                textBox3.Visible = true;
                textBox3.Text = "";
            }
            else
            {
                label4.Visible = false;
                textBox3.Visible = false;
                textBox3.Text = "6";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool validni = true;
            long number;
            if (!((long.TryParse(textBox1.Text, out number))|| (textBox1.Text == ""))) validni = false;
            if (!((long.TryParse(textBox2.Text, out number)) || (textBox2.Text == ""))) validni = false;
            if (!((long.TryParse(textBox3.Text, out number)) || (textBox3.Text == ""))) validni = false;
            if (!((long.TryParse(textBox4.Text, out number)) || (textBox4.Text == ""))) validni = false;
            if (!((long.TryParse(textBox5.Text, out number)) || (textBox5.Text == ""))) validni = false;
            if (!validni)
            {
                MessageBox.Show("One or more of the inputs are invalid!", "Invalid characters",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text != "")
            {
                B = Convert.ToUInt64(textBox1.Text);
            }
            if (textBox2.Text != "")
            {
                L = Convert.ToInt32(textBox2.Text);
            }
            if (textBox3.Text != "")
            {
                Krok = Convert.ToUInt32(textBox3.Text);
            }
            if (textBox4.Text != "")
            {
                M = Convert.ToUInt64(textBox4.Text);
            }
            if (textBox5.Text != "")
            {
                N = Convert.ToUInt64(textBox5.Text);
            }
            textBox6.Text = "";
            if (comboBox1.SelectedIndex == 7) M = 1;
            if (comboBox1.SelectedIndex == 8) M = 1;
            if (comboBox1.SelectedIndex == 2) { B = 3; M = 1; }
            if (comboBox1.SelectedIndex == 5) B = 3;
            if (comboBox1.SelectedIndex == 6) B = 3;
            if (comboBox1.SelectedIndex == 4) { B = Convert.ToUInt32(textBox5.Text); }
            bool nevyhovuje = false;
            if ((comboBox1.SelectedIndex == 0) && (N > 3333)) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 1) && ((N-M)*B > 1000000)) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 2) && (N > 5000000)) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 3) && ((long)N > 9999999*Krok*L)) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 4) && (N-M>100)) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 5) && (N-M>50)) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 6) && (N-M > 1000) &&((float)(M/N)>1.1)) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 7) && ((N > 999999999)||(B>100))) nevyhovuje = true;
            if ((comboBox1.SelectedIndex == 8) && ((N > 99999999)||((N>200000)&&(L==2)))) nevyhovuje = true;
            if (nevyhovuje)
            {
                MessageBox.Show("Too many entries! Try using lower parameters!", "Optimal calculation time exceeded",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (comboBox1.SelectedIndex == -1) return;
            Antipa.Antipalindromy(M, N, Krok, (int)B, L, Mode, outputDoSouboru);

            if (outputDoSouboru)
            {
                SaveFileDialog saveDialog1 = new SaveFileDialog();
                if (saveDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (Stream s = File.Open(saveDialog1.FileName, FileMode.CreateNew))
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(textBox6.Text);

                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                outputDoSouboru = true;
                
            }
            else
            {
                outputDoSouboru = false;
            }
        }
    }
}
