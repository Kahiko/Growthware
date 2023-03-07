using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Services;

public interface IClientChoicesService
{
    public MClientChoicesState GetClientChoicesState(string account, bool fromDB);

    public MClientChoicesState GetClientChoicesState(string account);

    public void Save(MClientChoicesState clientChoicesState, bool updateContext);

    public void Save(MClientChoicesState clientChoicesState);

    public int SelectedSecurityEntity();
}