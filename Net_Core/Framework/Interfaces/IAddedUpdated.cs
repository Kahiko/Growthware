using System;

namespace GrowthWare.Framework.Interfaces;

public interface IAddedUpdated: IDatabaseTable
{
    int AddedBy { get; set; }

    DateTime AddedDate { get; set; }

    int UpdatedBy { get; set; }

    DateTime UpdatedDate { get; set; }
}