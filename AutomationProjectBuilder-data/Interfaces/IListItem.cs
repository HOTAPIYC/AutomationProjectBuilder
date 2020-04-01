using System;

namespace AutomationProjectBuilder.Interfaces
{
    public interface IListItem
    {
        public Guid Id { get; }
        public Guid ModuleId { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }
    }
}
