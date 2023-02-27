using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Realsphere.Spirit.Input
{
    public static class SKeyTranslator
    {
        /// <summary>
        /// Converts a SKey to a Char
        /// </summary>
        public static char KeyToChar(SKeyboard.SKey key)
        {
            char chr = (char)0;
            switch(key)
            {
                case SKeyboard.SKey.A: chr = 'a'; break;
                case SKeyboard.SKey.B: chr = 'b'; break;
                case SKeyboard.SKey.C: chr = 'c'; break;
                case SKeyboard.SKey.D: chr = 'd'; break;
                case SKeyboard.SKey.E: chr = 'e'; break;
                case SKeyboard.SKey.F: chr = 'f'; break;
                case SKeyboard.SKey.G: chr = 'g'; break;
                case SKeyboard.SKey.H: chr = 'h'; break;
                case SKeyboard.SKey.I: chr = 'i'; break;
                case SKeyboard.SKey.J: chr = 'j'; break;
                case SKeyboard.SKey.K: chr = 'k'; break;
                case SKeyboard.SKey.L: chr = 'l'; break;
                case SKeyboard.SKey.M: chr = 'm'; break;
                case SKeyboard.SKey.N: chr = 'n'; break;
                case SKeyboard.SKey.O: chr = 'o'; break;
                case SKeyboard.SKey.P: chr = 'p'; break;
                case SKeyboard.SKey.Q: chr = 'q'; break;
                case SKeyboard.SKey.R: chr = 'r'; break;
                case SKeyboard.SKey.S: chr = 's'; break;
                case SKeyboard.SKey.T: chr = 't'; break;
                case SKeyboard.SKey.U: chr = 'u'; break;
                case SKeyboard.SKey.V: chr = 'v'; break;
                case SKeyboard.SKey.W: chr = 'w'; break;
                case SKeyboard.SKey.X: chr = 'x'; break;
                case SKeyboard.SKey.Y: chr = 'y'; break;
                case SKeyboard.SKey.Z: chr = 'z'; break;
                default: return (char)0;
            }
            if (SKeyboard.IsKeyDown(SKeyboard.SKey.LShift) || SKeyboard.IsKeyDown(SKeyboard.SKey.RShift))
                chr = char.ToUpper(chr);

            return chr;
        }
        /// <summary>
        /// Converts a Key to a Char
        /// </summary>
        public static char KeyToChar(int key)
        {
            char chr = (char)0;
            switch (key)
            {
                case (int)Keys.A: chr = 'a'; break;
                case (int)Keys.B: chr = 'b'; break;
                case (int)Keys.C: chr = 'c'; break;
                case (int)Keys.D: chr = 'd'; break;
                case (int)Keys.E: chr = 'e'; break;
                case (int)Keys.F: chr = 'f'; break;
                case (int)Keys.G: chr = 'g'; break;
                case (int)Keys.H: chr = 'h'; break;
                case (int)Keys.I: chr = 'i'; break;
                case (int)Keys.J: chr = 'j'; break;
                case (int)Keys.K: chr = 'k'; break;
                case (int)Keys.L: chr = 'l'; break;
                case (int)Keys.M: chr = 'm'; break;
                case (int)Keys.N: chr = 'n'; break;
                case (int)Keys.O: chr = 'o'; break;
                case (int)Keys.P: chr = 'p'; break;
                case (int)Keys.Q: chr = 'q'; break;
                case (int)Keys.R: chr = 'r'; break;
                case (int)Keys.S: chr = 's'; break;
                case (int)Keys.T: chr = 't'; break;
                case (int)Keys.U: chr = 'u'; break;
                case (int)Keys.V: chr = 'v'; break;
                case (int)Keys.W: chr = 'w'; break;
                case (int)Keys.X: chr = 'x'; break;
                case (int)Keys.Y: chr = 'y'; break;
                case (int)Keys.Z: chr = 'z'; break;
                default: return (char)0;
            }
            if (SKeyboard.IsKeyDown(SKeyboard.SKey.LShift) || SKeyboard.IsKeyDown(SKeyboard.SKey.RShift))
                chr = char.ToUpper(chr);

            return chr;
        }
    }
}
