using DistributedDemocratie.Models;

namespace DistributedDemocratie.Services
{
    public interface ILawService
    {
        Task<List<Law>> GetLawsAsync();
        Task<List<Law>> GetActiveVotingLawsAsync();
        Task<Law?> GetLawByIdAsync(int id);
        Task<Law> CreateLawAsync(Law law);
        Task<Law> UpdateLawAsync(Law law);
        Task<bool> DeleteLawAsync(int id);
        Task<List<Law>> SearchLawsAsync(string query, LawType? type = null, LawStatus? status = null);
    }

    public class LawService : ILawService
    {
        private readonly List<Law> _laws;

        public LawService()
        {
            // Données d'exemple pour le développement
            _laws = new List<Law>
            {
                new Law
                {
                    Id = 1,
                    Title = "Loi sur la transition énergétique",
                    Description = "Proposition de loi visant à accélérer la transition vers les énergies renouvelables",
                    FullText = "Cette loi propose de fixer des objectifs ambitieux pour la transition énergétique...",
                    Type = LawType.Law,
                    Status = LawStatus.Voting,
                    CreatedDate = DateTime.Now.AddDays(-5),
                    VotingStartDate = DateTime.Now.AddDays(-2),
                    VotingEndDate = DateTime.Now.AddDays(5),
                    CreatedBy = "Collectif Énergie Citoyenne",
                    Tags = new List<string> { "environnement", "énergie", "climat" }
                },
                new Law
                {
                    Id = 2,
                    Title = "Décret sur la simplification administrative",
                    Description = "Mesures pour réduire la bureaucratie et simplifier les démarches administratives",
                    FullText = "Ce décret propose plusieurs mesures concrètes...",
                    Type = LawType.Decree,
                    Status = LawStatus.Voting,
                    CreatedDate = DateTime.Now.AddDays(-3),
                    VotingStartDate = DateTime.Now.AddDays(-1),
                    VotingEndDate = DateTime.Now.AddDays(7),
                    CreatedBy = "Initiative Citoyenne Numérique",
                    Tags = new List<string> { "administration", "simplification", "numérique" }
                },
                new Law
                {
                    Id = 3,
                    Title = "Loi sur la santé mentale",
                    Description = "Amélioration de l'accès aux soins de santé mentale",
                    FullText = "Cette loi vise à garantir un meilleur accès aux soins de santé mentale...",
                    Type = LawType.Law,
                    Status = LawStatus.UnderReview,
                    CreatedDate = DateTime.Now.AddDays(-10),
                    CreatedBy = "Association Santé Pour Tous",
                    Tags = new List<string> { "santé", "mental", "soins" }
                }
            };
        }

        public Task<List<Law>> GetLawsAsync()
        {
            return Task.FromResult(_laws.ToList());
        }

        public Task<List<Law>> GetActiveVotingLawsAsync()
        {
            var activeLaws = _laws.Where(l => l.IsVotingActive).ToList();
            return Task.FromResult(activeLaws);
        }

        public Task<Law?> GetLawByIdAsync(int id)
        {
            var law = _laws.FirstOrDefault(l => l.Id == id);
            return Task.FromResult(law);
        }

        public Task<Law> CreateLawAsync(Law law)
        {
            law.Id = _laws.Max(l => l.Id) + 1;
            law.CreatedDate = DateTime.Now;
            _laws.Add(law);
            return Task.FromResult(law);
        }

        public Task<Law> UpdateLawAsync(Law law)
        {
            var existingLaw = _laws.FirstOrDefault(l => l.Id == law.Id);
            if (existingLaw != null)
            {
                var index = _laws.IndexOf(existingLaw);
                _laws[index] = law;
            }
            return Task.FromResult(law);
        }

        public Task<bool> DeleteLawAsync(int id)
        {
            var law = _laws.FirstOrDefault(l => l.Id == id);
            if (law != null)
            {
                _laws.Remove(law);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<Law>> SearchLawsAsync(string query, LawType? type = null, LawStatus? status = null)
        {
            var results = _laws.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                results = results.Where(l => 
                    l.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    l.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    l.Tags.Any(t => t.Contains(query, StringComparison.OrdinalIgnoreCase)));
            }

            if (type.HasValue)
            {
                results = results.Where(l => l.Type == type.Value);
            }

            if (status.HasValue)
            {
                results = results.Where(l => l.Status == status.Value);
            }

            return Task.FromResult(results.ToList());
        }
    }
}