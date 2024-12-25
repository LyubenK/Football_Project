using System;
using System.ComponentModel.DataAnnotations;
using Football_Project.Data.Entities.Abstact;

namespace Football_Project.Data.Entities
{
    public class Team : IBaseEntity
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int PlayedMatchesCount { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Losses { get; set; }

        public int Points { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}

