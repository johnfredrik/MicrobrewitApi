using AutoMapper;
using Microbrewit.Model;

namespace Microbrewit.Service.Automapper
{
    public class ResolveRecipeId<T> : ValueResolver<T,int>
    {
        protected override int ResolveCore(T source)
        {
            using (var context = new MicrobrewitContext())
            {
                return 1;
            }
        }
    }
}