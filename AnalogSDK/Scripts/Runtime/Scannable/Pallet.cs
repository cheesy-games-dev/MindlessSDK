using System.Collections.Generic;
using System.Linq;

namespace AnalogSDK
{
    public class Pallet : Scannable
    {
        public string Author;
        public string Version;
        public List<Crate> Crates;

        public void SortCrates() => Crates.Sort();
    }
}