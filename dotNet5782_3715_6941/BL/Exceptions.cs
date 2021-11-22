using System;

namespace IBL
{
    namespace BO
    {
        public class CantReachToDest : Exception
        {
            public double battery { get; set; }
            public double required { get; set;  } 
            public CantReachToDest() : base() { }
            public CantReachToDest(String message) : base(message) { }
            public CantReachToDest(String message, double _battery ,double _required ) : base(message)
            {
                battery = _battery;
                required = _required; 
            }
            public override string ToString()
            {
                return Message + " \nbattery: "+battery.ToString()+"\nrequired: "+
                    required.ToString() + "\nmissing: "+(required-battery).ToString();
            }
        }
        public class EnumNotInRightStatus<T> : Exception
        {
            public T stat { get; set; }

            public EnumNotInRightStatus() : base() { }
            public EnumNotInRightStatus(String message) : base(message) { }
            public EnumNotInRightStatus(String message, T _stat) : base(message)
            {
                stat = _stat;
            }   
            public override string ToString()
            {
                return Message + stat.ToString();
            }
        }
        public class NotValidTimePeriod : Exception
        {
            
            double time { set; get;  } 
            public NotValidTimePeriod() : base() { }
            public NotValidTimePeriod(String message) : base(message) { }
            public NotValidTimePeriod(String message, double _time) : base(message)
            {
                time = _time;
            }
            public override string ToString()
            {
                return Message ;
            }
        }


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