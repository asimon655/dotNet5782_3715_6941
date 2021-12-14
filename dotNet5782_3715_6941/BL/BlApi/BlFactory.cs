using BlApi;
using BL;

namespace BlApi 
{
    public static class BlFactory 
    {
        public static Ibl GetBl()
        {
            return Bl.Instance;
        }
    }
}