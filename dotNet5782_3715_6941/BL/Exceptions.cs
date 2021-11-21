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
            public IdAlreadyExists(IDAL.DO.IdAlreadyExists err) : base(err.Message)
            {
                id = err.id;
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
            public IdDosntExists(IDAL.DO.IdDosntExists err) : base(err.Message)
            {
                id = err.id;
            }
            public override string ToString()
            {
                return Message + id;
            }
        }
        public class EnumOutOfRange : Exception
        {
            public int value { get; set; }
            public EnumOutOfRange(String message, int _value) : base(message)
            {
                value = _value;
            }
            public override string ToString()
            {
                return Message + value;
            }
        }
        public class SenderGetterAreSame : Exception
        {
            public int id { get; set; }
            public SenderGetterAreSame(String message, int _id) : base(message)
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