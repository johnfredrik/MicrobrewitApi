using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class RecipeFermentationStepResolver : ValueResolver<RecipeDto, IList<FermentationStep>>
    {
        protected override IList<FermentationStep> ResolveCore(RecipeDto recipe)
        {
            var fermentationStepList = new List<FermentationStep>();
            foreach (var fermentationStepDto in recipe.FermentationSteps)
            {
                var fermentationStep = new FermentationStep()
                {
                    Fermentables = new List<FermentationStepFermentable>(),
                    Hops = new List<FermentationStepHop>(),
                    Others = new List<FermentationStepOther>(),
                    Yeasts = new List<FermentationStepYeast>(),
                  //  Id = fermentationStepDto.Id,
                    Length = fermentationStepDto.Length,
                    StepNumber = fermentationStepDto.Number,
                    Notes = fermentationStepDto.Notes,
                    Temperature = fermentationStepDto.Temperature,
                    RecipeId = recipe.Id,

                };
                if (fermentationStepDto.Hops != null)
                {
                    foreach (var hopDto in fermentationStepDto.Hops)
                    {
                        var hop = Mapper.Map<HopStepDto, FermentationStepHop>(hopDto);
                      //  hop.StepNumber = fermentationStep.Id;
                        fermentationStep.Hops.Add(hop);
                    }
                }
                if (fermentationStepDto.Fermentables != null)
                {

                    foreach (var fermentableDto in fermentationStepDto.Fermentables)
                    {
                        var fermentable = Mapper.Map<FermentableStepDto, FermentationStepFermentable>(fermentableDto);
                        fermentable.StepNumber = fermentationStep.StepNumber;
                        fermentationStep.Fermentables.Add(fermentable);
                    }
                }

                if (fermentationStepDto.Others != null)
                {

                    foreach (var otherDto in fermentationStepDto.Others)
                    {
                        var other = Mapper.Map<OtherStepDto, FermentationStepOther>(otherDto);
                        other.StepNumber = fermentationStep.StepNumber;
                        fermentationStep.Others.Add(other);

                    }
                }

                if (fermentationStepDto.Yeasts != null)
                {

                    foreach (var yeastDto in fermentationStepDto.Yeasts)
                    {
                        var yeast = Mapper.Map<YeastStepDto, FermentationStepYeast>(yeastDto);
                        yeast.StepNumber = fermentationStep.StepNumber;
                        fermentationStep.Yeasts.Add(yeast);
                    }
                }
                fermentationStepList.Add(fermentationStep);
            }

            return fermentationStepList;
        }
    }
}