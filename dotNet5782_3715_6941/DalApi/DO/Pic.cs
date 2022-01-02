using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct DronePic
    {
        // key
        public string Model { get; set; }
        public string Path { get; set; }
    }
    public struct CustomerPic
    {
        // key
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
