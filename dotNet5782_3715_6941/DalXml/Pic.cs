using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        public DronePic GetDronePic(string Model)
        {
            List<DronePic> data = Read<DronePic>();

            DronePic res = data.Find(x => x.Model == Model);
            if (res.Model != Model)
                throw new IdDosntExists("there is no pic saved under that Id");

            return res;
        }
        public CustomerPic GetCustomerPic(int customerId)
        {
            List<CustomerPic> data = Read<CustomerPic>();

            CustomerPic res = data.Find(x => x.Id == customerId);
            if (res.Id != customerId)
                throw new IdDosntExists("there is no pic saved under that Id");

            return res;
        }
        public void AddDronePic(DronePic pic)
        {
            List<DronePic> data = Read<DronePic>();

            if (data.Any(x => x.Model == pic.Model))
                throw new IdAlreadyExists("there is already pic saved under that Model Name");

            data.Add(pic);

            Write(data);
        }
        public void AddCustomerPic(CustomerPic pic)
        {
            List<CustomerPic> data = Read<CustomerPic>();

            if (data.Any(x => x.Id == pic.Id))
                throw new IdAlreadyExists("there is already pic saved under that type and Id", pic.Id);

            data.Add(pic);

            Write(data);
        }
    }
}
