using System.Collections.Generic;
using System.ComponentModel;

namespace DbwViewer.GenericCollections;

public interface ICollectionView<T> : IEnumerable<T>, ICollectionView
{
    
}