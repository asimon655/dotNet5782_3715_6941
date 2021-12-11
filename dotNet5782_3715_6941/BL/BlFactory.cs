using BlApi;
using BL;

namespace BlFactory 
{
    public static class BlFactory 
    {
        public static Ibl GetBl()
        {
            return Bl.Instance;
        }
    }
}