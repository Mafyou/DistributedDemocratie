using DistributedDemocratie.Models;

namespace DistributedDemocratie.Services
{
    public interface IVotingService
    {
        Task<Vote?> GetUserVoteAsync(int lawId, string userId);
        Task<Vote> CastVoteAsync(int lawId, string userId, VoteChoice choice, string? comment = null);
        Task<bool> HasUserVotedAsync(int lawId, string userId);
        Task<VotingResult> GetVotingResultsAsync(int lawId);
        Task<List<Vote>> GetVotesForLawAsync(int lawId);
    }

    public class VotingService : IVotingService
    {
        private readonly List<Vote> _votes;
        private readonly ILawService _lawService;
        private readonly ICitizenService _citizenService;

        public VotingService(ILawService lawService, ICitizenService citizenService)
        {
            _lawService = lawService;
            _citizenService = citizenService;
            _votes = new List<Vote>();
            
            // Votes d'exemple
            SeedExampleVotes();
        }

        private void SeedExampleVotes()
        {
            var random = new Random();
            var choices = Enum.GetValues<VoteChoice>();
            
            // Ajouter des votes d'exemple pour les lois 1 et 2
            for (int lawId = 1; lawId <= 2; lawId++)
            {
                for (int i = 0; i < random.Next(50, 200); i++)
                {
                    _votes.Add(new Vote
                    {
                        Id = _votes.Count + 1,
                        LawId = lawId,
                        UserId = $"user_{i}_{lawId}",
                        Choice = choices[random.Next(choices.Length)],
                        VotedAt = DateTime.Now.AddDays(-random.Next(0, 3)),
                        IsVerified = true,
                        IpAddress = $"192.168.1.{random.Next(1, 255)}"
                    });
                }
            }
        }

        public Task<Vote?> GetUserVoteAsync(int lawId, string userId)
        {
            var vote = _votes.FirstOrDefault(v => v.LawId == lawId && v.UserId == userId);
            return Task.FromResult(vote);
        }

        public async Task<Vote> CastVoteAsync(int lawId, string userId, VoteChoice choice, string? comment = null)
        {
            // Vérifier si la loi existe et est ouverte au vote
            var law = await _lawService.GetLawByIdAsync(lawId);
            if (law == null || !law.IsVotingActive)
                throw new InvalidOperationException("Cette loi n'est pas ouverte au vote");

            // Vérifier si l'utilisateur peut voter
            var citizen = await _citizenService.GetCitizenByIdAsync(userId);
            if (citizen == null || !citizen.CanVote)
                throw new InvalidOperationException("Vous n'êtes pas autorisé à voter");

            // Vérifier si l'utilisateur a déjà voté
            var existingVote = await GetUserVoteAsync(lawId, userId);
            if (existingVote != null)
                throw new InvalidOperationException("Vous avez déjà voté sur cette loi");

            var vote = new Vote
            {
                Id = _votes.Count + 1,
                LawId = lawId,
                UserId = userId,
                Choice = choice,
                Comment = comment,
                VotedAt = DateTime.Now,
                IsVerified = true,
                IpAddress = "127.0.0.1" // TODO: récupérer la vraie IP
            };

            _votes.Add(vote);
            return vote;
        }

        public async Task<bool> HasUserVotedAsync(int lawId, string userId)
        {
            var vote = await GetUserVoteAsync(lawId, userId);
            return vote != null;
        }

        public Task<VotingResult> GetVotingResultsAsync(int lawId)
        {
            var lawVotes = _votes.Where(v => v.LawId == lawId).ToList();
            
            var result = new VotingResult
            {
                LawId = lawId,
                TotalVotes = lawVotes.Count,
                VotesFor = lawVotes.Count(v => v.Choice == VoteChoice.For),
                VotesAgainst = lawVotes.Count(v => v.Choice == VoteChoice.Against),
                VotesAbstain = lawVotes.Count(v => v.Choice == VoteChoice.Abstain),
                LastUpdated = DateTime.Now
            };

            return Task.FromResult(result);
        }

        public Task<List<Vote>> GetVotesForLawAsync(int lawId)
        {
            var votes = _votes.Where(v => v.LawId == lawId).ToList();
            return Task.FromResult(votes);
        }
    }

    public class VotingResult
    {
        public int LawId { get; set; }
        public int TotalVotes { get; set; }
        public int VotesFor { get; set; }
        public int VotesAgainst { get; set; }
        public int VotesAbstain { get; set; }
        public DateTime LastUpdated { get; set; }
        
        public double PercentageFor => TotalVotes > 0 ? (double)VotesFor / TotalVotes * 100 : 0;
        public double PercentageAgainst => TotalVotes > 0 ? (double)VotesAgainst / TotalVotes * 100 : 0;
        public double PercentageAbstain => TotalVotes > 0 ? (double)VotesAbstain / TotalVotes * 100 : 0;
        
        public bool IsPassing => VotesFor > VotesAgainst;
    }
}