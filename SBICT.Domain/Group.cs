using System;

namespace SBICT.Data
{
    public struct Group : IGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Group(Guid id, string title)
        {
            Id = id;
            Name = title;
        }
    }
}