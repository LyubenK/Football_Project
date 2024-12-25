using System;
namespace Football_Project.Data.Entities.Abstact
{
	public interface IBaseEntity
	{
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}

