using DO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DronePic GetDronePic(string Model)
        {
            DronePic res = DataSource.DronePics.Find(x => x.Model == Model);
            if (res.Model != Model)
            {
                throw new IdDosntExists("there is no pic saved under that Id");
            }

            return res;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerPic GetCustomerPic(int customerId)
        {
            CustomerPic res = DataSource.CustomerPics.Find(x => x.Id == customerId);
            if (res.Id != customerId)
            {
                throw new IdDosntExists("there is no pic saved under that Id");
            }

            return res;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDronePic(DronePic pic)
        {
            if (DataSource.DronePics.Any(x => x.Model == pic.Model))
            {
                throw new IdAlreadyExists("there is already pic saved under that Model Name");
            }

            DataSource.DronePics.Add(pic);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomerPic(CustomerPic pic)
        {
            if (DataSource.CustomerPics.Any(x => x.Id == pic.Id))
            {
                throw new IdAlreadyExists("there is already pic saved under that type and Id", pic.Id);
            }

            DataSource.CustomerPics.Add(pic);
        }
    }
}
