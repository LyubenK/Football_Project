using System;
using System.ComponentModel.DataAnnotations;
using Football_Project.Data.Entities.Abstact;

namespace Football_Project.Data.Entities
{
    public class Match : IBaseEntity
    {
        [Key]
        public int MatchId { get; set; }

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } = null!;

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}

