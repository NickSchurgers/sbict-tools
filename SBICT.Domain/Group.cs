using System;

namespace SBICT.Data
{
    public struct Group : IGroup
    {
        public Guid Id { get; }
        public string Title { get; set; }

        public Group(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}