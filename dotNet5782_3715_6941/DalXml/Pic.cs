using DO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DronePic GetDronePic(string Model)
        {
            List<DronePic> data = Read<DronePic>();

            DronePic res = data.Find(x => x.Model == Model);
            if (res.Model != Model)
            {
                throw new IdDosntExists("there is no pic saved under that Id");
            }

            return res;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerPic GetCustomerPic(int customerId)
        {
            List<CustomerPic> data = Read<CustomerPic>();

            CustomerPic res = data.Find(x => x.Id == customerId);
            if (res.Id != customerId)
            {
                throw new IdDosntExists("there is no pic saved under that Id");
            }

            return res;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDronePic(DronePic pic)
        {
            List<DronePic> data = Read<DronePic>();

            if (data.Any(x => x.Model == pic.Model))
            {
                throw new IdAlreadyExists("there is already pic saved under that Model Name");
            }

            data.Add(pic);

            Write(data);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomerPic(CustomerPic pic)
        {
            List<CustomerPic> data = Read<CustomerPic>();

            if (data.Any(x => x.Id == pic.Id))
            {
                throw new IdAlreadyExists("there is already pic saved under that type and Id", pic.Id);
            }

            data.Add(pic);

            Write(data);
        }
    }
}
