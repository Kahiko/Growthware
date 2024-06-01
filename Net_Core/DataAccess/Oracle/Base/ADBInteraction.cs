using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;

namespace GrowthWare.DataAccess.Oracle.Base;

public abstract class AbstractDBInteraction : IDBInteraction, IDisposable
{
    private bool m_DisposedValue;

    public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected virtual void Dispose(bool disposing)
    {
        if (!m_DisposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            m_DisposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~AbstractDBInteraction()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}