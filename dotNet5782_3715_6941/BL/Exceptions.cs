using System;

namespace IBL
{
    namespace BO
    {
        public class IdAlreadyExists : Exception
        {
            public int id { get; set; }

            public IdAlreadyExists() : base() {}
            public IdAlreadyExists(String message) : base(message) {}
            public IdAlreadyExists(String message, int _id) : base(message)
            {
                id = _id;
            }
            public override string ToString()
            {
                return Message + id;
            }
        }
        public class IdDosntExists : Exception
        {
            public int id { get; set; }

            public IdDosntExists() : base() {}
            public IdDosntExists(String message) : base(message) {}
            public IdDosntExists(String message, int _id) : base(message)
            {
                id = _id;
            }
            public override string ToString()
            {
                return Message + id;
            }
        }
    }
}