using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
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
                    Id = mashStepDto.Id,
                    Length = mashStepDto.Length,
                    Number = mashStepDto.Number,
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
                        hop.StepId = mashStep.Id;
                        mashStep.Hops.Add(hop);
                    }
                }
                if (mashStepDto.Fermentables != null)
                {

                    foreach (var fermentableDto in mashStepDto.Fermentables)
                    {
                        var fermentable = Mapper.Map<FermentableStepDto, MashStepFermentable>(fermentableDto);
                        fermentable.StepId = mashStep.Id;
                        mashStep.Fermentables.Add(fermentable);
                    }
                }

                if (mashStepDto.Others != null)
                {

                    foreach (var otherDto in mashStepDto.Others)
                    {
                        var other = Mapper.Map<OtherStepDto, MashStepOther>(otherDto);
                        other.StepId = mashStep.Id;
                        mashStep.Others.Add(other);

                    }
                }

                mashStepList.Add(mashStep);
            }

            return mashStepList;
        }
    }
}