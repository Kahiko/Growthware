using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using System.Data;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    public interface IDBInformation : IDDBInteraction
    {
        MDBInformation Profile
        {
            get;
            set;

        }
        DataRow GetProfile();
        bool UpdateProfile();
    }
}
