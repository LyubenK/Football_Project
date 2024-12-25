using System;
using Football_Project.Data.Entities;

namespace Football_Project.Data.Models
{
	public class AddMatchViewModel
	{

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }

    }
}

