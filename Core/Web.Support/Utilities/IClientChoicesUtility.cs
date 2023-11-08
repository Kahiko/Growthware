using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public interface IClientChoicesUtility
{
    public MClientChoicesState GetClientChoicesState(string account, bool fromDB);

    public MClientChoicesState GetClientChoicesState(string account);

    public void Save(MClientChoicesState clientChoicesState, bool updateContext);

    public void Save(MClientChoicesState clientChoicesState);
}