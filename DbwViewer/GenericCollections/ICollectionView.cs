using System.Collections.Generic;
using System.ComponentModel;

namespace DbwViewer.GenericCollections;

public interface ICollectionView<out T> : IEnumerable<T>, ICollectionView
{
    
}