using System;

namespace SBICT.Data
{
    public interface IGroup
    {
        Guid Id { get; }
        string Title { get; set; }
    }
}