using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class RecipeMashStepResolver : ValueResolver<RecipeDto,IList<MashStep>>
    {
        protected override IList<MashStep> ResolveCore(RecipeDto recipe)
        {
            var mashStepList = new List<MashStep>();
            foreach (var mashStepDto in recipe.MashSteps)
            {
                var mashStep = new MashStep()
                {
                    Fermentables = new List<MashStepFermentable>(),
                    Hops = new List<MashStepHop>(),
                    Others = new List<MashStepOther>(),
                  //  Id = mashStepDto.Id,
                    Length = mashStepDto.Length,
                    StepNumber = mashStepDto.StepNumber,
                    Notes = mashStepDto.Notes,
                    Volume = mashStepDto.Volume,
                    Temperature = mashStepDto.Temperature,
                    RecipeId = recipe.Id,

                };
                if (mashStepDto.Hops != null)
                {
                    foreach (var hopDto in mashStepDto.Hops)
                    {
                        var hop = Mapper.Map<HopStepDto, MashStepHop>(hopDto);
                        hop.StepNumber = mashStep.StepNumber;
                        mashStep.Hops.Add(hop);
                    }
                }
                if (mashStepDto.Fermentables != null)
                {

                    foreach (var fermentableDto in mashStepDto.Fermentables)
                    {
                        var fermentable = Mapper.Map<FermentableStepDto, MashStepFermentable>(fermentableDto);
                        fermentable.StepNumber = mashStep.StepNumber;
                        mashStep.Fermentables.Add(fermentable);
                    }
                }

                if (mashStepDto.Others != null)
                {

                    foreach (var otherDto in mashStepDto.Others)
                    {
                        var other = Mapper.Map<OtherStepDto, MashStepOther>(otherDto);
                        other.StepNumber = mashStep.StepNumber;
                        mashStep.Others.Add(other);

                    }
                }

                mashStepList.Add(mashStep);
            }

            return mashStepList;
        }
    }
}