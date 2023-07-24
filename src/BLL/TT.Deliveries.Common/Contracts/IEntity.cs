using System;

namespace TT.Deliveries.Common.Contracts {

    /// <summary>
    /// Interface of entity with guid Id as a key
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; }
    }
}