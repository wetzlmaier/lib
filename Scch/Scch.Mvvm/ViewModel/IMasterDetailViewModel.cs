using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// interface for the master detail viewmodel.
    /// Each master or detail viewmodel can have a number of detail view models.
    /// Each viewmodel has a property of type CollectionViewSource.
    /// </summary>
    public interface IMasterDetailViewModel
    {
        List<IMasterDetailViewModel> Details { get; }
        Action OnMasterChanged { get; }
        CollectionViewSource ViewSource { get; }
    }
}
