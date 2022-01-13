using System;

namespace DO
{
    public class IdAlreadyExists : Exception
    {
        public int id { get; set; }

        public IdAlreadyExists() : base() { }
        public IdAlreadyExists(string message) : base(message) { }
        public IdAlreadyExists(string message, int _id) : base(message)
        {
            id = _id;
        }
    }
    public class IdDosntExists : Exception
    {
        public int id { get; set; }

        public IdDosntExists() : base() { }
        public IdDosntExists(string message) : base(message) { }
        public IdDosntExists(string message, int _id) : base(message)
        {
            id = _id;
        }
    }
}
