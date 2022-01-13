using System;

namespace BO
{
    public class InValidSumOfChargeSlots : Exception
    {
        public int chargingstaionsenter { get; set; }
        public int usingchargingstaions { get; set; }
        public InValidSumOfChargeSlots() : base() { }
        public InValidSumOfChargeSlots(string message) : base(message) { }
        public InValidSumOfChargeSlots(string message, int _chargings, int _chargingenterstaions) : base(message)
        {
            chargingstaionsenter = _chargingenterstaions;
            usingchargingstaions = _chargings;
        }
        public override string ToString()
        {
            return Message + " \nenter: " + chargingstaionsenter.ToString() + "\nin use: " +
                usingchargingstaions.ToString();
        }
    }
    public class CouldntFindPatcelThatsFits : Exception
    {
        public CouldntFindPatcelThatsFits() : base() { }
        public CouldntFindPatcelThatsFits(string message) : base(message) { }
    }
    public class notEnoughBattery : Exception
    {
        public notEnoughBattery() : base() { }
        public notEnoughBattery(string message) : base(message) { }
    }

    public class CantReachToDest : Exception
    {
        public double battery { get; set; }
        public double required { get; set; }
        public CantReachToDest() : base() { }
        public CantReachToDest(string message) : base(message) { }
        public CantReachToDest(string message, double _battery, double _required) : base(message)
        {
            battery = _battery;
            required = _required;
        }
        public override string ToString()
        {
            return Message + " \nbattery: " + battery.ToString() + "\nrequired: " +
                required.ToString() + "\nmissing: " + (required - battery).ToString();
        }
    }


    public class EnumNotInRightStatus<T> : Exception
    {
        public T stat { get; set; }

        public EnumNotInRightStatus() : base() { }
        public EnumNotInRightStatus(string message) : base(message) { }
        public EnumNotInRightStatus(string message, T _stat) : base(message)
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
        private double time { set; get; }
        public NotValidTimePeriod() : base() { }
        public NotValidTimePeriod(string message) : base(message) { }
        public NotValidTimePeriod(string message, double _time) : base(message)
        {
            time = _time;
        }
        public override string ToString()
        {
            return Message + time;
        }
    }


    public class IdAlreadyExists : Exception
    {
        public int id { get; set; }

        public IdAlreadyExists() : base() { }
        public IdAlreadyExists(string message) : base(message) { }
        public IdAlreadyExists(string message, int _id) : base(message)
        {
            id = _id;
        }
        public IdAlreadyExists(DO.IdAlreadyExists err) : base(err.Message)
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

        public IdDosntExists() : base() { }
        public IdDosntExists(string message) : base(message) { }
        public IdDosntExists(string message, int _id) : base(message)
        {
            id = _id;
        }
        public IdDosntExists(DO.IdDosntExists err) : base(err.Message)
        {
            id = err.id;
        }
        public override string ToString()
        {
            return Message + id;
        }
    }


    public class LocationOutOfRange : Exception
    {
        public double Lonigtuide { get; set; }
        public double Latitude { get; set; }
        public LocationOutOfRange(string message, double Lonigtuide, double Latitude) : base(message)
        {
            this.Latitude = Latitude;
            this.Lonigtuide = Lonigtuide;
        }
        public override string ToString()
        {
            return Message + "Latitude: " + Latitude.ToString() + "Longituide : " + Lonigtuide.ToString();
        }
    }


    public class EnumOutOfRange : Exception
    {
        public int value { get; set; }
        public EnumOutOfRange(string message, int _value) : base(message)
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
        public SenderGetterAreSame(string message, int _id) : base(message)
        {
            id = _id;
        }
        public override string ToString()
        {
            return Message + id;
        }
    }
    public class NoOrBadInternet : Exception
    {
        public NoOrBadInternet(string message) : base(message) { }
    }
    public class CantDelete : Exception
    {
        public int id { get; set; }
        public CantDelete(string message) : base(message) { }
        public CantDelete(string message, int _id) : base(message) { id = _id; }
        public override string ToString()
        {
            return Message + id;
        }
    }
}
