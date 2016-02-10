using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualityWithT4
{
    /// <summary>
    /// The base entity interface
    /// </summary>
    public interface IEntity
    {
        object GetId();
    }

    /// <summary>
    /// A specialized identifier entity interface
    /// </summary>
    public interface IEntity<TIdentifier> : IEntity
    {
        TIdentifier Id { get; }
    }
}
