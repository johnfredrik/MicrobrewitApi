using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class RecipeBoilStepResolver : ValueResolver<RecipeDto, IList<BoilStep>>
    {
        protected override IList<BoilStep> ResolveCore(RecipeDto recipe)
        {
            var boilStepList = new List<BoilStep>();
            {
                foreach (var boilStepDto in recipe.BoilSteps)
                {

                    var boilStep = new BoilStep()
                    {
                        Fermentables = new List<BoilStepFermentable>(),
                        Hops = new List<BoilStepHop>(),
                        Others = new List<BoilStepOther>(),
                       // Id = boilStepDto.Id,
                        Length = boilStepDto.Length,
                        StepNumber = boilStepDto.StepNumber,
                        Notes = boilStepDto.Notes,
                        Volume = boilStepDto.Volume,
                        RecipeId = recipe.Id,
                    };
                    if (boilStepDto.Fermentables != null)
                    {
                        foreach (var fermentableDto in boilStepDto.Fermentables)
                        {
                            var fermentable = Mapper.Map<FermentableStepDto, BoilStepFermentable>(fermentableDto);
                            fermentable.StepNumber = boilStep.StepNumber;
                            boilStep.Fermentables.Add(fermentable);

                        }
                    }
                    if (boilStepDto.Hops != null)
                    {
                        foreach (var hopDto in boilStepDto.Hops)
                        {
                            var hop = Mapper.Map<HopStepDto, BoilStepHop>(hopDto);
                            hop.StepNumber = boilStepDto.StepNumber;
                            boilStep.Hops.Add(hop);
                        }
                    }

                    if (boilStepDto.Others != null)
                    {
                        foreach (var otherDto in boilStepDto.Others)
                        {
                            var other = Mapper.Map<OtherStepDto, BoilStepOther>(otherDto);
                            other.StepNumber = boilStepDto.StepNumber;
                            boilStep.Others.Add(other);
                        }
                    }

                    boilStepList.Add(boilStep);
                }

                return boilStepList;
            }
        }
    }
}