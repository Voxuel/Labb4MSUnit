using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TeamLemon.Controls
{
    public class ASCIIArt
    {
        public static void IntroArt()
        {
            Console.WriteLine(@"
         _._._                       _._._
        _|   |_                     _|   |_
        | ... |_._._._._._._._._._._| ... |
        | ||| |  o  LEMON BANK  o   | ||| |
        |     |                     |     |
   ())  |[-|-]| [-|-][-|-][-|-][-|-]|[-|-]|  ())
  (())) |     | ------------------- |     | (()))
 (())())|     |                     |     |(())())
 (()))()|[-|-]|  :::   .- -.   :::  |[-|-]|(()))()
 ()))(()|     | |~|~|  |_|_|  |~|~|~|     |()))(()
   ||   |_____|_|_|_|__|_|_|__|_|_|_|_____|  ||
  ~~^^ @@@@@@@@@@@@@@/=======\@@@@@@@@@@@@@@ ^^~~
            ^~^~~                           ^~^");

            Thread.Sleep(2500);
            Console.Clear();
        }



    }
}
