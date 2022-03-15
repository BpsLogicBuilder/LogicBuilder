using Contoso.Domain.Entities;
using System.Threading.Tasks;

namespace Contoso.Test.Flow.Rules
{
    public interface IRulesLoader
    {
        Task LoadRulesOnStartUp(RulesModuleModel modules, RulesCache cache);
        void LoadRules(RulesModuleModel modules, RulesCache cache);
    }
}
