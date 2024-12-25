using System;
using System.ComponentModel.DataAnnotations;

namespace Football_Project.Data.Models
{
	public class TeamViewModel
	{
        public string Name { get; set; } = null!;

        public int Points { get; set; }

        public int PlayedMatchesCount { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Losses { get; set; }

    }
}

